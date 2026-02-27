// Decompiled with JetBrains decompiler
// Type: LoreStone
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Febucci.UI;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.Menus.PlayerMenu;
using MMTools;
using Spine;
using Spine.Unity;
using src.UI.InfoCards;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class LoreStone : Interaction
{
  [SerializeField]
  public SkeletonGraphic spine;
  [SerializeField]
  public SpriteRenderer sprite;
  [SerializeField]
  public DOTweenAnimation spriteAnimation;
  [SerializeField]
  public Image vfx;
  [SerializeField]
  public Image screenFlash;
  [SerializeField]
  public Canvas canvas;
  [SerializeField]
  public CanvasGroup canvasGroup;
  [SerializeField]
  public CanvasGroup canvasControlPrompts;
  [SerializeField]
  public LoreInfoCard loreInfoCard;
  [SerializeField]
  public BiomeLightingSettings LightingSettings;
  [SerializeField]
  public OverrideLightingProperties overrideLightingProperties;
  public bool useLightingVolume = true;
  public int chosenLore = -1;
  public bool forceLore;
  public int forcedLore;
  [SerializeField]
  public Sprite graveSprite;
  public Vector3 BookTargetPosition;
  public float Timer;
  [CompilerGenerated]
  public bool \u003CIsRunning\u003Ek__BackingField;
  public bool Activated;
  [SerializeField]
  public Interaction_TeleportHome Teleporter;
  public bool DisabledTeleporter;
  public List<ConversationEntry> conversationEntries;

  public bool IsRunning
  {
    get => this.\u003CIsRunning\u003Ek__BackingField;
    set => this.\u003CIsRunning\u003Ek__BackingField = value;
  }

  public void SetLore(int _lore, bool isGraveLore = false, bool isLambLore = false, bool isWolfLore = false)
  {
    this.forceLore = true;
    this.forcedLore = _lore;
    if (isGraveLore)
    {
      this.sprite.sprite = this.graveSprite;
      this.spine.initialSkinName = "lostmessage";
    }
    else if (isLambLore)
    {
      this.spine.initialSkinName = "lamb";
    }
    else
    {
      if (!isWolfLore)
        return;
      this.spine.initialSkinName = "wolf";
    }
  }

  public override void OnDisable()
  {
    base.OnDisable();
    if (!((UnityEngine.Object) this.spine != (UnityEngine.Object) null) || this.spine.AnimationState == null)
      return;
    this.spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if (!((UnityEngine.Object) this.spine != (UnityEngine.Object) null) || this.spine.AnimationState == null)
      return;
    this.spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.canvas.gameObject.SetActive(false);
    this.loreInfoCard.gameObject.SetActive(false);
    if ((UnityEngine.Object) this.Teleporter != (UnityEngine.Object) null && !this.DisabledTeleporter)
    {
      this.Teleporter.DisableTeleporter();
      this.DisabledTeleporter = true;
    }
    if (!this.forceLore || !LoreSystem.LoreAvailable(this.forcedLore))
      return;
    this.gameObject.SetActive(false);
  }

  public void MagnetToPlayer()
  {
    this.IsRunning = true;
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir_Short", this.transform.position);
    this.Activated = true;
    this.AutomaticallyInteract = true;
    PickUp component = this.GetComponent<PickUp>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      component.enabled = false;
    this.ChooseLore();
    this.StartCoroutine(this.PlayerPickUpBook((System.Action) (() => this.StartCoroutine(this.SpawnMenu()))));
  }

  public void HandleEvent(TrackEntry trackentry, Spine.Event e)
  {
    Debug.Log((object) e.Data.Name);
    switch (e.Data.Name)
    {
      case "CameraShake":
        CameraManager.instance.ShakeCameraForDuration(0.7f, 1f, 0.2f);
        MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
        break;
      case "Break":
        AudioManager.Instance.PlayOneShot("event:/temple_key/become_whole", PlayerFarming.Instance.gameObject);
        AudioManager.Instance.PlayOneShot("event:/lore_stone/lore_stone_break", PlayerFarming.Instance.gameObject);
        this.vfx.color = new Color(1f, 1f, 1f, 0.0f);
        if (SettingsManager.Settings.Accessibility.FlashingLights)
        {
          this.screenFlash.enabled = true;
          DOTweenModuleUI.DOFade(this.screenFlash, 0.0f, 0.25f).SetDelay<TweenerCore<Color, Color, ColorOptions>>(0.25f);
        }
        if (!this.useLightingVolume)
          break;
        LightingManager.Instance.inOverride = true;
        this.LightingSettings.overrideLightingProperties = this.overrideLightingProperties;
        LightingManager.Instance.overrideSettings = this.LightingSettings;
        LightingManager.Instance.transitionDurationMultiplier = 0.0f;
        LightingManager.Instance.UpdateLighting(true);
        break;
      case "Shake":
        AudioManager.Instance.PlayOneShot("event:/doctrine_stone/doctrine_shake", PlayerFarming.Instance.gameObject);
        break;
      case "Close":
        AudioManager.Instance.PlayOneShot("event:/ui/close_menu", PlayerFarming.Instance.gameObject);
        break;
    }
  }

  public IEnumerator PlayerPickUpBook(System.Action callback)
  {
    LoreStone loreStone = this;
    yield return (object) new WaitForEndOfFrame();
    PlayerFarming.Instance.health.untouchable = true;
    AudioManager.Instance.PlayOneShot("event:/Stings/lore_sting", loreStone.transform.position);
    AudioManager.Instance.PlayOneShot("event:/lore_stone/lore_stone_pickup", loreStone.transform.position);
    loreStone.Timer = 0.0f;
    GameManager.GetInstance().OnConversationNew();
    PlayerSimpleInventory component = loreStone.state.gameObject.GetComponent<PlayerSimpleInventory>();
    loreStone.BookTargetPosition = new Vector3(component.ItemImage.transform.position.x, component.ItemImage.transform.position.y, -2f);
    loreStone.spriteAnimation.DOKill();
    loreStone.spriteAnimation.enabled = false;
    loreStone.sprite.gameObject.transform.DOMove(loreStone.BookTargetPosition, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart);
    loreStone.state.CURRENT_STATE = StateMachine.State.FoundItem;
    GameManager.GetInstance().OnConversationNext(loreStone.sprite.gameObject, 3f);
    loreStone.sprite.DOFade(0.0f, 1f);
    loreStone.screenFlash.enabled = false;
    loreStone.vfx.color = new Color(1f, 1f, 1f, 0.0f);
    DOTweenModuleUI.DOFade(loreStone.vfx, 1f, 1f);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    MMVibrate.RumbleContinuous(0.1f, 0.25f);
    loreStone.canvas.gameObject.SetActive(true);
    loreStone.canvasControlPrompts.DOFade(0.0f, 0.0f);
    loreStone.spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(loreStone.HandleEvent);
    loreStone.transform.position = loreStone.BookTargetPosition;
    loreStone.sprite.transform.DOScale(Vector3.zero, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart);
    if (LoreSystem.loreBlacklist.Contains(loreStone.chosenLore))
      yield return (object) loreStone.spine.YieldForAnimation("declare-doctrine_nobreak");
    else
      yield return (object) loreStone.spine.YieldForAnimation("declare-doctrine_zelot");
    MMVibrate.StopRumble();
    System.Action action = callback;
    if (action != null)
      action();
    loreStone.spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(loreStone.HandleEvent);
  }

  public void ChooseLore()
  {
    this.chosenLore = this.forceLore ? this.forcedLore : LoreSystem.GetUnlockedLore();
    if (this.chosenLore != -1)
      return;
    Debug.Log((object) "Error: No More Lores!");
  }

  public IEnumerator SpawnMenu()
  {
    LoreStone loreStone = this;
    GameManager.InMenu = true;
    PlayerFarming.Instance.state.facingAngle = PlayerFarming.Instance.state.LookAngle = 0.0f;
    BiomeConstants.Instance.EmitPickUpVFX(loreStone.transform.position);
    loreStone.conversationEntries = new List<ConversationEntry>();
    loreStone.loreInfoCard.gameObject.SetActive(true);
    loreStone.loreInfoCard.CanvasGroup.alpha = 0.3f;
    loreStone.loreInfoCard.CanvasGroup.DOFade(1f, 0.5f);
    loreStone.loreInfoCard.transform.localScale = Vector3.one * 1.5f;
    loreStone.loreInfoCard.transform.DOScale(Vector3.one, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => { }));
    loreStone.loreInfoCard.Configure(loreStone.chosenLore);
    loreStone.loreInfoCard._itemHeader.alpha = 0.0f;
    ShortcutExtensionsTMPText.DOFade(loreStone.loreInfoCard._itemHeader, 1f, 1f);
    loreStone.loreInfoCard._itemDescription.alpha = 0.0f;
    ShortcutExtensionsTMPText.DOFade(loreStone.loreInfoCard._itemDescription, 1f, 1f);
    loreStone.loreInfoCard._itemLore.alpha = 0.0f;
    ShortcutExtensionsTMPText.DOFade(loreStone.loreInfoCard._itemLore, 1f, 1f);
    TextAnimatorPlayer t = loreStone.loreInfoCard._itemLore.GetComponent<TextAnimatorPlayer>();
    t.ShowText(loreStone.loreInfoCard._itemLore.text);
    loreStone.canvasControlPrompts.DOFade(1f, 0.5f);
    while (!t.textAnimator.allLettersShown)
    {
      if (InputManager.UI.GetAcceptButtonDown())
      {
        t.SkipTypewriter();
        yield return (object) null;
        break;
      }
      yield return (object) null;
    }
    while (!InputManager.UI.GetAcceptButtonDown())
      yield return (object) null;
    AudioManager.Instance.PlayOneShot("event:/ui/close_menu", loreStone.transform.position);
    GameManager.InMenu = false;
    loreStone.canvasGroup.DOFade(0.0f, 0.5f);
    LoreSystem.UnlockLore(loreStone.chosenLore);
    DataManager.Instance.Alerts.LoreAlerts.Add(loreStone.chosenLore);
    NotificationCentre.Instance.PlayGenericNotification("Notifications/LoreUnlocked/Notification/On", NotificationBase.Flair.Positive);
    Debug.Log((object) ("Lore Unlocked: " + loreStone.chosenLore.ToString()));
    loreStone.PlayerToIdle();
    if (!DataManager.Instance.LoreOnboarded)
      GameManager.GetInstance().StartCoroutine(loreStone.ShowLoreMenu());
    else
      loreStone.IsRunning = false;
    if ((UnityEngine.Object) loreStone.Teleporter != (UnityEngine.Object) null)
      loreStone.Teleporter.EnableTeleporter();
  }

  public IEnumerator ShowLoreMenu()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    LoreStone loreStone = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      UIPauseDetailsMenuTabNavigatorBase.Instance.TransitionToLore();
      loreStone.IsRunning = false;
      DataManager.Instance.LoreOnboarded = true;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    MonoSingleton<UIManager>.Instance.ShowDetailsMenu(loreStone._playerFarming);
    UIManager.PlayAudio("event:/Stings/church_bell");
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) null;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void PlayerToIdle()
  {
    PlayerFarming.Instance.health.untouchable = false;
    PlayerFarming.Instance._state.CURRENT_STATE = StateMachine.State.Idle;
    if (this.useLightingVolume)
    {
      LightingManager.Instance.inOverride = false;
      LightingManager.Instance.overrideSettings = (BiomeLightingSettings) null;
      LightingManager.Instance.transitionDurationMultiplier = 1f;
      LightingManager.Instance.lerpActive = false;
      LightingManager.Instance.UpdateLighting(true);
    }
    GameManager.GetInstance().OnConversationEnd();
    this.gameObject.SetActive(false);
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (this.Activated)
      return;
    this.MagnetToPlayer();
  }

  public override void GetLabel()
  {
    this.Label = ScriptLocalization.Interactions.PickUp;
    if (this.Interactable)
      return;
    this.Label = "";
  }

  [CompilerGenerated]
  public void \u003CMagnetToPlayer\u003Eb__29_0() => this.StartCoroutine(this.SpawnMenu());
}
