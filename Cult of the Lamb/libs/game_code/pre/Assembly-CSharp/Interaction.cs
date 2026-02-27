// Decompiled with JetBrains decompiler
// Type: Interaction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using FMOD.Studio;
using I2.Loc;
using Lamb.UI;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityFx.Outline;

#nullable disable
public abstract class Interaction : BaseMonoBehaviour
{
  protected OutlineEffect Outliner;
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
  [SerializeField]
  private string label;
  [SerializeField]
  private string secondaryLabel;
  [SerializeField]
  private string thirdLabel;
  [SerializeField]
  private string fourthLabel;
  private List<Interaction.RendererAndColor> SpriteRendererAndColors = new List<Interaction.RendererAndColor>();
  private List<Interaction.RendererAndColorSpine> SpineRendererAndColors = new List<Interaction.RendererAndColorSpine>();
  public GameObject OutlineTarget;
  public UnityEvent indicateHighlight;
  public UnityEvent indicateHighlightEnd;
  private EventInstance loopingSoundInstance;
  private string holdTime = "hold_time";
  private bool hasPlayed;

  public OutlineEffect OutlineEffect => this.Outliner;

  public int Action => 9;

  public int SecondaryAction => 68;

  public int ThirdAction => 67;

  public int FourthAction => 66;

  public event Interaction.InteractionEvent OnInteraction;

  public string Label
  {
    get
    {
      if (!this.IgnoreTutorial && !DataManager.Instance.AllowBuilding && (Object) BiomeBaseManager.Instance != (Object) null)
        return "";
      this.GetLabel();
      return this.label;
    }
    set => this.label = value;
  }

  public bool HasChanged { get; set; }

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

  protected virtual void OnEnable()
  {
    Interaction.interactions.Add(this);
    this.OnEnableInteraction();
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.UpdateLocalisation);
    if (this.gameObject.activeInHierarchy)
      this.StartCoroutine((IEnumerator) this.AddToRegion());
    if (!((Object) this.Outliner == (Object) null) || !((Object) Camera.main != (Object) null))
      return;
    this.Outliner = Camera.main.GetComponent<OutlineEffect>();
  }

  private IEnumerator AddToRegion()
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

  protected virtual void OnDestroy()
  {
    AudioManager.Instance.StopLoop(this.loopingSoundInstance);
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.UpdateLocalisation);
    Interaction.interactions.Remove(this);
    Interactor.RemoveFromRegion(this);
  }

  protected virtual void OnDisable()
  {
    Interaction.interactions.Remove(this);
    this.OnDisableInteraction();
    AudioManager.Instance.StopLoop(this.loopingSoundInstance);
    Interactor.RemoveFromRegion(this);
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.UpdateLocalisation);
  }

  public virtual void OnDisableInteraction()
  {
    AudioManager.Instance.StopLoop(this.loopingSoundInstance);
  }

  public virtual void OnBecomeCurrent()
  {
    this.HasChanged = false;
    if (this.AutomaticallyInteract)
      return;
    this.IndicateHighlighted();
  }

  public virtual void IndicateHighlighted()
  {
    if (CheatConsole.HidingUI)
      return;
    if ((Object) this.Outliner != (Object) null)
    {
      this.Outliner.OutlineLayers[0].Add((Object) this.OutlineTarget == (Object) null ? this.gameObject : this.OutlineTarget);
      this.indicateHighlight?.Invoke();
    }
    else
      Debug.Log((object) "Outliner = null");
  }

  public virtual void OnBecomeNotCurrent()
  {
    if (this.AutomaticallyInteract)
      return;
    this.EndIndicateHighlighted();
    AudioManager.Instance.StopLoop(this.loopingSoundInstance);
  }

  public virtual void EndIndicateHighlighted()
  {
    if ((Object) this.Outliner != (Object) null && this.Outliner.OutlineLayers.Count > 0 && this.Outliner.OutlineLayers[0] != null)
    {
      this.Outliner.OutlineLayers[0].Remove((Object) this.OutlineTarget == (Object) null ? this.gameObject : this.OutlineTarget);
      this.Outliner.RemoveGameObject((Object) this.OutlineTarget == (Object) null ? this.gameObject : this.OutlineTarget);
      this.indicateHighlightEnd?.Invoke();
    }
    this.SpriteRendererAndColors.Clear();
    AudioManager.Instance.StopLoop(this.loopingSoundInstance);
  }

  public virtual void OnHoldProgressDown()
  {
    MonoSingleton<Indicator>.Instance.ControlPromptContainer.DOKill();
    MonoSingleton<Indicator>.Instance.ControlPromptContainer.localScale = Vector3.one;
    MonoSingleton<Indicator>.Instance.ControlPromptContainer.DOPunchScale(new Vector3(0.2f, 0.2f), 0.2f);
    this.loopingSoundInstance = AudioManager.Instance.CreateLoop("event:/ui/hold_button_loop", this.gameObject, true);
    if ((double) this.HoldProgress <= 0.949999988079071)
      return;
    AudioManager.Instance.StopLoop(this.loopingSoundInstance);
  }

  protected virtual void Update()
  {
    if ((double) this.HoldProgress > 0.0)
    {
      this.hasPlayed = true;
      int num = (int) this.loopingSoundInstance.setParameterByName(this.holdTime, this.HoldProgress);
      if (!this.HoldBegun)
        return;
      MMVibrate.RumbleContinuous(this.HoldProgress * 0.2f, this.HoldProgress * 0.2f);
    }
    else
    {
      if (!this.hasPlayed)
        return;
      AudioManager.Instance.StopLoop(this.loopingSoundInstance);
      this.hasPlayed = false;
    }
  }

  public virtual void OnHoldProgressRelease() => MMVibrate.StopRumble();

  public virtual void OnHoldProgressStop()
  {
    this.HoldBegun = false;
    this.HoldProgress = 0.0f;
    MMVibrate.StopRumble();
    AudioManager.Instance.StopLoop(this.loopingSoundInstance);
  }

  public void OnInteractGetStaticState()
  {
    this.state = PlayerFarming.Instance.state;
    this.OnInteract(this.state);
  }

  public virtual void OnInteract(StateMachine state)
  {
    this.EndIndicateHighlighted();
    this.state = state;
    if (this.CallbackStart != null)
      this.CallbackStart.Invoke();
    Interaction.InteractionEvent onInteraction = this.OnInteraction;
    if (onInteraction != null)
      onInteraction(state);
    if (this.HoldToInteract && SettingsManager.Settings.Accessibility.HoldActions)
    {
      AudioManager.Instance.PlayOneShot("event:/ui/hold_activate", this.transform.position);
      MMVibrate.StopRumble();
    }
    else
      UIManager.PlayAudio("event:/ui/open_menu");
  }

  public virtual void OnSecondaryInteract(StateMachine state)
  {
    this.EndIndicateHighlighted();
    this.state = state;
  }

  public virtual void OnEndInteraction()
  {
  }

  public virtual void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position + this.ActivatorOffset, this.ActivateDistance, Color.white);
  }

  public delegate void InteractionEvent(StateMachine state);

  private class RendererAndColor
  {
    public SpriteRenderer spriteRenderer;
    public Color color;

    public RendererAndColor(SpriteRenderer spriteRenderer, Color color)
    {
      this.spriteRenderer = spriteRenderer;
      this.color = color;
    }
  }

  private class RendererAndColorSpine
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
