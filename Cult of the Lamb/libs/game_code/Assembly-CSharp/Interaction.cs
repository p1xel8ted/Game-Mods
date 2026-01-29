// Decompiled with JetBrains decompiler
// Type: Interaction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using FMOD.Studio;
using I2.Loc;
using Lamb.UI;
using MMTools;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityFx.Outline;

#nullable disable
public abstract class Interaction : BaseMonoBehaviour
{
  public bool LambOnly;
  public bool GoatOnly;
  public bool SwitchMainPlayerOnInteract = true;
  public bool InputOnlyFromInteractingPlayer = true;
  public OutlineEffect Outliner;
  public Transform LockPosition;
  public static List<Interaction> interactions = new List<Interaction>();
  [HideInInspector]
  public Vector3 Position;
  public StateMachine state;
  public Vector3 Offset = new Vector3(0.0f, 3.5f, 0.0f);
  public float PriorityWeight = 1f;
  public float ActivateDistance = 1f;
  public Vector3 ActivatorOffset = Vector3.zero;
  public bool HoldToInteract;
  public bool FreezeCoopPlayersOnHoldToInteract = true;
  [HideInInspector]
  public bool ContinuouslyHold;
  public bool AutomaticallyInteract;
  [HideInInspector]
  public float HoldProgress;
  [HideInInspector]
  public bool HoldBegun;
  public bool Interactable = true;
  public bool HasSecondaryInteraction;
  public bool SecondaryInteractable = true;
  public bool HasThirdInteraction;
  public bool ThirdInteractable = true;
  public bool HasFourthInteraction;
  public bool FourthInteractable = true;
  public UnityEvent CallbackStart;
  [HideInInspector]
  public bool IgnoreTutorial;
  [HideInInspector]
  public bool isActiveInHierachey;
  [HideInInspector]
  public Transform cachedTransform;
  public PlayerFarming _playerFarming;
  [SerializeField]
  public string label;
  [CompilerGenerated]
  public bool \u003CHasChanged\u003Ek__BackingField;
  [SerializeField]
  public string secondaryLabel;
  [SerializeField]
  public string thirdLabel;
  [SerializeField]
  public string fourthLabel;
  public List<Interaction.RendererAndColor> SpriteRendererAndColors = new List<Interaction.RendererAndColor>();
  public List<Interaction.RendererAndColorSpine> SpineRendererAndColors = new List<Interaction.RendererAndColorSpine>();
  public GameObject OutlineTarget;
  public UnityEvent indicateHighlight;
  public UnityEvent indicateHighlightEnd;
  [HideInInspector]
  public List<PlayerFarming> IndicatorPlayerFarmings = new List<PlayerFarming>();
  public EventInstance loopingSoundInstance;
  public string holdTime = "hold_time";
  public bool hasPlayed;
  public float previousHoldValue;
  public float visibilityTimer = 0.1f;
  public SkeletonAnimation interactorSkeletonAnimation;
  public bool allowMultipleInteractors = true;

  public virtual bool InactiveAfterStopMoving => true;

  public virtual bool CanMultiplePlayersInteract => false;

  public OutlineEffect OutlineEffect => this.Outliner;

  public virtual bool AllowInteractionWithoutLabel => false;

  public int Action => 9;

  public int SecondaryAction => 68;

  public int ThirdAction => 67;

  public int FourthAction => 66;

  public PlayerFarming playerFarming
  {
    get
    {
      if ((UnityEngine.Object) this._playerFarming == (UnityEngine.Object) null)
      {
        if ((UnityEngine.Object) this.state == (UnityEngine.Object) null)
          return PlayerFarming.Instance;
        this._playerFarming = this.state.GetComponent<PlayerFarming>();
      }
      return this._playerFarming;
    }
    set => this._playerFarming = value;
  }

  public event Interaction.InteractionEvent OnInteraction;

  public string Label
  {
    get
    {
      if (!this.IgnoreTutorial && !DataManager.Instance.AllowBuilding && (UnityEngine.Object) BiomeBaseManager.Instance != (UnityEngine.Object) null)
        return "";
      this.GetLabel();
      return this.label;
    }
    set => this.label = value;
  }

  public bool HasChanged
  {
    get => this.\u003CHasChanged\u003Ek__BackingField;
    set => this.\u003CHasChanged\u003Ek__BackingField = value;
  }

  public virtual void GetLabel()
  {
  }

  public string SecondaryLabel
  {
    get
    {
      if (!this.IgnoreTutorial && !DataManager.Instance.AllowBuilding)
        return "";
      this.GetSecondaryLabel();
      return this.secondaryLabel;
    }
    set => this.secondaryLabel = value;
  }

  public string ThirdLabel
  {
    get
    {
      if (!this.IgnoreTutorial && !DataManager.Instance.AllowBuilding)
        return "";
      this.GetThirdLabel();
      return this.thirdLabel;
    }
    set => this.thirdLabel = value;
  }

  public string FourthLabel
  {
    get
    {
      if (!this.IgnoreTutorial && !DataManager.Instance.AllowBuilding)
        return "";
      this.GetFourthLabel();
      return this.fourthLabel;
    }
    set => this.fourthLabel = value;
  }

  public virtual void GetSecondaryLabel()
  {
  }

  public virtual void GetThirdLabel()
  {
  }

  public virtual void GetFourthLabel()
  {
  }

  public virtual void OnEnable()
  {
    this.isActiveInHierachey = true;
    this.cachedTransform = this.transform;
    Interaction.interactions.Add(this);
    this.OnEnableInteraction();
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.UpdateLocalisation);
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.UpdateLocalisation);
    if (this.gameObject.activeInHierarchy)
      this.StartCoroutine((IEnumerator) this.AddToRegion());
    if (!((UnityEngine.Object) this.Outliner == (UnityEngine.Object) null) || !((UnityEngine.Object) Camera.main != (UnityEngine.Object) null))
      return;
    this.Outliner = Camera.main.GetComponent<OutlineEffect>();
  }

  public IEnumerator AddToRegion()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Interaction l = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      l.Position = l.transform.position;
      Interactor.AddToRegion(l);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(0.1f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public virtual void UpdateLocalisation() => this.HasChanged = true;

  public virtual void OnEnableInteraction()
  {
  }

  public virtual void OnDestroy()
  {
    AudioManager.Instance.StopLoop(this.loopingSoundInstance);
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.UpdateLocalisation);
    if ((bool) (UnityEngine.Object) CoopManager.Instance)
      CoopManager.Instance.OnPlayerLeft -= new System.Action(this.OnPlayerLeft);
    Interaction.interactions.Remove(this);
    Interactor.RemoveFromRegion(this);
  }

  public virtual void OnDisable()
  {
    this.isActiveInHierachey = false;
    if ((bool) (UnityEngine.Object) CoopManager.Instance)
      CoopManager.Instance.OnPlayerLeft -= new System.Action(this.OnPlayerLeft);
    Interaction.interactions.Remove(this);
    this.OnDisableInteraction();
    AudioManager.Instance.StopLoop(this.loopingSoundInstance);
    Interactor.RemoveFromRegion(this);
    if (!Interactor.UseInteractionCulling)
      return;
    this.RemoveInteraction();
  }

  public virtual void OnDisableInteraction()
  {
    AudioManager.Instance.StopLoop(this.loopingSoundInstance);
  }

  public virtual void OnBecomeCurrent(PlayerFarming playerFarming)
  {
    this.HasChanged = false;
    if (this.AutomaticallyInteract)
      return;
    this.IndicateHighlighted(playerFarming);
  }

  public void IndicateHighlightUpdate(PlayerFarming playerFarming)
  {
    if (CheatConsole.HidingUI)
      return;
    if ((UnityEngine.Object) this.Outliner != (UnityEngine.Object) null)
    {
      this.Outliner.OutlineLayers[0].Add((UnityEngine.Object) this.OutlineTarget == (UnityEngine.Object) null ? this.gameObject : this.OutlineTarget);
      this.indicateHighlight?.Invoke();
    }
    else
      Debug.Log((object) "Outliner = null");
  }

  public virtual void IndicateHighlighted(PlayerFarming playerFarming = null)
  {
    bool flag = this.IndicatorPlayerFarmings.Contains(playerFarming);
    if (!(bool) (UnityEngine.Object) playerFarming || flag)
      return;
    if (this.IndicatorPlayerFarmings.Count == 0)
      this.IndicateHighlightUpdate(playerFarming);
    this.IndicatorPlayerFarmings.Add(playerFarming);
  }

  public virtual void EndIndicateHighlighted(PlayerFarming playerFarming = null)
  {
    if ((bool) (UnityEngine.Object) playerFarming && this.IndicatorPlayerFarmings.Contains(playerFarming))
      this.IndicatorPlayerFarmings.Remove(playerFarming);
    if (this.IndicatorPlayerFarmings.Count != 0)
      return;
    this.EndIndicateHighlightedUpdate();
    AudioManager.Instance.StopLoop(this.loopingSoundInstance);
  }

  public virtual void OnBecomeNotCurrent(PlayerFarming playerFarming)
  {
    if (this.AutomaticallyInteract)
      return;
    this.EndIndicateHighlighted(playerFarming);
  }

  public virtual void EndIndicateHighlightedUpdate()
  {
    if ((UnityEngine.Object) this.Outliner != (UnityEngine.Object) null && this.Outliner.OutlineLayers.Count > 0 && this.Outliner.OutlineLayers[0] != null)
    {
      this.Outliner.OutlineLayers[0].Remove((UnityEngine.Object) this.OutlineTarget == (UnityEngine.Object) null ? this.gameObject : this.OutlineTarget);
      this.Outliner.RemoveGameObject((UnityEngine.Object) this.OutlineTarget == (UnityEngine.Object) null ? this.gameObject : this.OutlineTarget);
      this.indicateHighlightEnd?.Invoke();
    }
    this.SpriteRendererAndColors.Clear();
    AudioManager.Instance.StopLoop(this.loopingSoundInstance);
  }

  public virtual void OnHoldProgressDown(Indicator indicator, PlayerFarming currentPlayerFarming = null)
  {
    if ((UnityEngine.Object) currentPlayerFarming != (UnityEngine.Object) null)
      this.playerFarming = currentPlayerFarming;
    if ((UnityEngine.Object) indicator == (UnityEngine.Object) null)
      indicator = this.playerFarming.indicator;
    indicator.ControlPromptContainer.DOKill();
    indicator.ControlPromptContainer.localScale = Vector3.one;
    indicator.ControlPromptContainer.DOPunchScale(new Vector3(0.2f, 0.2f), 0.2f);
    this.loopingSoundInstance = AudioManager.Instance.CreateLoop("event:/ui/hold_button_loop", this.gameObject, true);
    if ((double) this.HoldProgress <= 0.949999988079071)
      return;
    AudioManager.Instance.StopLoop(this.loopingSoundInstance);
  }

  public virtual void Update()
  {
    if ((double) this.HoldProgress > 0.0)
    {
      this.hasPlayed = true;
      int num = (int) this.loopingSoundInstance.setParameterByName(this.holdTime, this.HoldProgress);
      if (this.HoldBegun && (double) this.HoldProgress > (double) this.previousHoldValue)
        MMVibrate.RumbleContinuous(this.HoldProgress * 0.2f, this.HoldProgress * 0.2f, this.playerFarming);
      this.previousHoldValue = this.HoldProgress;
    }
    else if (this.hasPlayed)
    {
      AudioManager.Instance.StopLoop(this.loopingSoundInstance);
      this.hasPlayed = false;
    }
    if (!Interactor.UseInteractionCulling)
      return;
    this.UpdateVisibility();
  }

  public void UpdateVisibility()
  {
    if (!Application.isPlaying || (double) (this.visibilityTimer -= Time.unscaledDeltaTime) > 0.0)
      return;
    this.visibilityTimer = 0.1f + UnityEngine.Random.Range(0.0f, 0.1f);
    float num1 = float.PositiveInfinity;
    for (int index = 0; index < PlayerFarming.players.Count; ++index)
    {
      float num2 = Vector3.Distance(PlayerFarming.players[index].transform.position with
      {
        z = this.gameObject.transform.position.z
      }, this.gameObject.transform.position + this.ActivatorOffset);
      if ((double) num1 > (double) num2)
        num1 = num2;
    }
    if ((double) num1 < (double) Mathf.Max(3f, this.ActivateDistance) || this.AutomaticallyInteract || (double) this.ActivateDistance == 3.4028234663852886E+38)
      this.AddInteraction();
    else
      this.RemoveInteraction();
  }

  public void RemoveInteraction() => Interactor.Remove(this);

  public void AddInteraction() => Interactor.Add(this);

  public virtual void OnHoldProgressRelease()
  {
    if (this.FreezeCoopPlayersOnHoldToInteract)
      PlayerFarming.SetStateForAllPlayers(PlayerNotToInclude: this.playerFarming);
    MMVibrate.StopRumble(this.playerFarming);
  }

  public virtual void OnHoldProgressStop()
  {
    this.HoldBegun = false;
    if (this.FreezeCoopPlayersOnHoldToInteract)
      PlayerFarming.SetStateForAllPlayers(PlayerNotToInclude: this.playerFarming);
    this.HoldProgress = 0.0f;
    MMVibrate.StopRumble(this.playerFarming);
    AudioManager.Instance.StopLoop(this.loopingSoundInstance);
  }

  public void OnInteractGetStaticState() => this.OnInteract(this.state);

  public void OnPlayerLeft()
  {
    if (!((UnityEngine.Object) this.playerFarming != (UnityEngine.Object) null) || this.playerFarming.isLamb || PlayerFarming.players.Count <= 0)
      return;
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if (player.isLamb)
        this.playerFarming = player;
    }
  }

  public virtual void OnInteract(StateMachine state)
  {
    SimpleBarkRepeating.CloseAllBarks(true);
    SimpleBark.CloseAllBarks(true);
    this.state = state;
    this._playerFarming = state.GetComponent<PlayerFarming>();
    CoopManager.Instance.OnPlayerLeft += new System.Action(this.OnPlayerLeft);
    if (this.SwitchMainPlayerOnInteract)
      PlayerFarming.SetMainPlayer(state);
    this.EndIndicateHighlighted(this.playerFarming);
    if ((bool) (UnityEngine.Object) this.playerFarming && (bool) (UnityEngine.Object) this.playerFarming.playerWeapon)
      this.playerFarming.playerWeapon.StopHeavyAttackRoutine();
    if (this.CallbackStart != null)
      this.CallbackStart.Invoke();
    Interaction.InteractionEvent onInteraction = this.OnInteraction;
    if (onInteraction != null)
      onInteraction(state);
    if (this.HoldToInteract && SettingsManager.Settings.Accessibility.HoldActions)
    {
      AudioManager.Instance.PlayOneShot("event:/ui/hold_activate", this.transform.position);
      MMVibrate.StopRumble(this.playerFarming);
    }
    else
      UIManager.PlayAudio("event:/ui/open_menu");
    if ((bool) (UnityEngine.Object) this.playerFarming)
      this.interactorSkeletonAnimation = this.playerFarming.Spine;
    if (!this.InactiveAfterStopMoving || !this.gameObject.activeSelf)
      return;
    this.StartCoroutine((IEnumerator) this.WaitForPlayerToStopMoving(this.playerFarming, (System.Action) (() =>
    {
      if (!LetterBox.IsPlaying && !MMConversation.isPlaying && !GameManager.GetInstance().CamFollowTarget.IN_CONVERSATION)
        return;
      foreach (PlayerFarming player in PlayerFarming.players)
      {
        if ((UnityEngine.Object) player != (UnityEngine.Object) this._playerFarming)
          player.state.CURRENT_STATE = StateMachine.State.InActive;
      }
    })));
  }

  public IEnumerator WaitForPlayerToStopMoving(PlayerFarming player, System.Action callback)
  {
    yield return (object) new WaitForEndOfFrame();
    if ((UnityEngine.Object) player != (UnityEngine.Object) null)
      yield return (object) new WaitUntil((Func<bool>) (() => !player.GoToAndStopping));
    System.Action action = callback;
    if (action != null)
      action();
  }

  public virtual void OnSecondaryInteract(StateMachine state)
  {
    this.EndIndicateHighlighted(this.playerFarming);
    this.state = state;
  }

  public virtual void OnThirdInteract(StateMachine state)
  {
    this.EndIndicateHighlighted(this.playerFarming);
    this.state = state;
  }

  public virtual void OnFourthInteract(StateMachine state)
  {
    this.EndIndicateHighlighted(this.playerFarming);
    this.state = state;
  }

  public virtual void OnEndInteraction()
  {
  }

  public virtual void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position + this.ActivatorOffset, this.ActivateDistance, Color.white);
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__115_0()
  {
    if (!LetterBox.IsPlaying && !MMConversation.isPlaying && !GameManager.GetInstance().CamFollowTarget.IN_CONVERSATION)
      return;
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if ((UnityEngine.Object) player != (UnityEngine.Object) this._playerFarming)
        player.state.CURRENT_STATE = StateMachine.State.InActive;
    }
  }

  public delegate void InteractionEvent(StateMachine state);

  public class RendererAndColor
  {
    public SpriteRenderer spriteRenderer;
    public Color color;

    public RendererAndColor(SpriteRenderer spriteRenderer, Color color)
    {
      this.spriteRenderer = spriteRenderer;
      this.color = color;
    }
  }

  public class RendererAndColorSpine
  {
    public SkeletonAnimation obj;
    public Color color;

    public RendererAndColorSpine(SkeletonAnimation obj, Color color)
    {
      this.obj = obj;
      this.color = color;
    }
  }
}
