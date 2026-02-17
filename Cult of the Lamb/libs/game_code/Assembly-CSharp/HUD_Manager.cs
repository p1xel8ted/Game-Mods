// Decompiled with JetBrains decompiler
// Type: HUD_Manager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using MMBiomeGeneration;
using Spine;
using Spine.Unity;
using src.UI.Prompts;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

#nullable disable
public class HUD_Manager : BaseMonoBehaviour
{
  public static HUD_Manager Instance;
  public static System.Action OnShown;
  public static System.Action OnHidden;
  [SerializeField]
  public GameObject _baseDetailsContainer;
  public UI_Transitions BaseDetailsTransition;
  [SerializeField]
  public GameObject _dungeonDetailsContainer;
  public UI_Transitions ConinsContainerTransition;
  [SerializeField]
  public GameObject _miniMapContainer;
  public UI_Transitions MiniMapTransition;
  [SerializeField]
  public GameObject _timeContainer;
  public UI_Transitions TimeTransitions;
  [Space]
  [SerializeField]
  public CanvasGroup _topRightCanvasGroup;
  [SerializeField]
  public TMP_Text _currentDungeonModifierText;
  [SerializeField]
  public GameObject _damageMultiplierContainer;
  [SerializeField]
  public UI_Transitions _damageMultiplierTransition;
  [SerializeField]
  public TMP_Text _fleeceDamageMultiplierText;
  public GameObject healthContainer;
  public Health_Manager healthManager;
  [SerializeField]
  public GameObject _xpBarContainer;
  public UI_Transitions XPBarTransitions;
  [SerializeField]
  public GameObject _returnToBaseContainer;
  public UI_Transitions ReturnToBaseTransitions;
  public UI_Transitions _curseTransitions;
  public UI_Transitions _weaponTransitions;
  public UI_Transitions _relicTransitions;
  public UI_Transitions _doctrineTransitions;
  [SerializeField]
  public GameObject returnToBaseContainer;
  [SerializeField]
  public GameObject _healContainer;
  [SerializeField]
  public GameObject _burrowContainer;
  [SerializeField]
  public GameObject _heavyAttacksContainer;
  public UI_Transitions HoldTransitions;
  [SerializeField]
  public NotificationCentre _notificatioCenter;
  [SerializeField]
  public UIObjectivesController _objectivesController;
  [SerializeField]
  public UI_Transitions _centerElementTransitions;
  [SerializeField]
  public UI_Transitions _editModeTransition;
  [SerializeField]
  public UI_Transitions _survivalBars;
  [SerializeField]
  public UIPlayerDamageNoti _playerDamageNoti;
  [SerializeField]
  public UI_Transitions _playerDamageNotiTransitions;
  [SerializeField]
  public SkeletonGraphic tentaclesOverlay;
  [SerializeField]
  public CanvasGroup _playerDamageNotiCanvasGroup;
  [SerializeField]
  public Image _playerOneWidget;
  [SerializeField]
  public RectTransform _playerOneArrow;
  [SerializeField]
  public Image _playerTwoWidget;
  [SerializeField]
  public RectTransform _playerTwoArrow;
  [SerializeField]
  public Indicator indicator;
  public Tween p1Tween;
  public Tween p2Tween;
  [SerializeField]
  public Image _furnaceWidget;
  [SerializeField]
  public RectTransform _furnaceArrow;
  [SerializeField]
  public Image[] _wolfWidgets;
  [SerializeField]
  public RectTransform[] _wolfArrows;
  [SerializeField]
  public Image[] _rottingCorpseWidgets;
  [SerializeField]
  public RectTransform[] _rottingCorpseArrows;
  [SerializeField]
  public Image _fightingWidget;
  [SerializeField]
  public Image _fightingIcon;
  [SerializeField]
  public RectTransform _fightingArrow;
  [SerializeField]
  public Image _lightningWidget;
  [SerializeField]
  public RectTransform _lightningArrow;
  public float _lightningWidgetTimer;
  public float _fightingWidgetTimer;
  public Tween lightningTween;
  public Tween fightingTween;
  public Tween furnaceTween;
  public Tween[] _wolfTweens = new Tween[20];
  public Tween[] _rottingCorpseTweens = new Tween[20];
  public TextMeshProUGUI CoinsAmount;
  public GameObject CoinsParent;
  public LayoutElement CoinsLayoutElement;
  [CompilerGenerated]
  public bool \u003CHidden\u003Ek__BackingField;
  public static bool IsTransitioning = false;
  public Tween topRightTween;
  public string _localizedDamage;
  public float fleeceDamageContainerSinglePlayerY;
  public GameObject BWImage;
  public Tween punchTween;
  public static bool ForceFurnaceWidget = false;
  public HUD_Manager.FightingTarget _currentFightingTarget;
  public SeasonsManager.LightningTarget _currentLightningTarget;
  public static int SingleIndicatorYOffset = -320;
  public static int CoopIndicatorXOffset = 325;
  public static int CoopIndicatorYOffset = -270;
  public static float CoopIndicatorScale = 0.95f;

  public static void UpdatePlayerHudScale()
  {
    float num = (float) (1.0 - 0.20000000298023224 * (double) (PlayerFarming.playersCount - 1));
    if ((double) num < 0.40000000596046448)
      num = 0.4f;
    if (!(bool) (UnityEngine.Object) HUD_Manager.Instance)
      return;
    HUD_Manager.Instance.healthContainer.transform.localScale = Vector3.one * num;
  }

  public TMP_Text CurrentDungeonModifierText => this._currentDungeonModifierText;

  public NotificationCentre NotificationCenter => this._notificatioCenter;

  public bool Hidden
  {
    set => this.\u003CHidden\u003Ek__BackingField = value;
    get => this.\u003CHidden\u003Ek__BackingField;
  }

  public void Awake()
  {
    this._fleeceDamageMultiplierText.text = "";
    this._damageMultiplierContainer.SetActive(false);
    this.UpdateLocalisation();
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.UpdateLocalisation);
    this._playerOneWidget.gameObject.SetActive(false);
    this._playerTwoWidget.gameObject.SetActive(false);
    this._furnaceWidget.gameObject.SetActive(false);
    this._fightingWidget.gameObject.SetActive(false);
    foreach (Component wolfWidget in this._wolfWidgets)
      wolfWidget.gameObject.SetActive(false);
    foreach (Component rottingCorpseWidget in this._rottingCorpseWidgets)
      rottingCorpseWidget.gameObject.SetActive(false);
    this._rottingCorpseTweens = new Tween[this._rottingCorpseWidgets.Length];
    this.fleeceDamageContainerSinglePlayerY = this._damageMultiplierContainer.transform.localPosition.y;
  }

  public void Start()
  {
    this.Hidden = false;
    this.Hide(true);
    this._survivalBars.gameObject.SetActive(DataManager.Instance.SurvivalModeActive);
    this._playerDamageNoti.gameObject.SetActive(DataManager.Instance.SurvivalModeActive);
    this._playerDamageNotiCanvasGroup.alpha = 0.0f;
    if (!((UnityEngine.Object) CoopManager.Instance != (UnityEngine.Object) null))
      return;
    CoopManager.Instance.OnPlayerJoined += new System.Action(this.RefreshHudElementsForGoldenFleeceCoop);
    CoopManager.Instance.OnPlayerLeft += new System.Action(this.RefreshHudElementsForGoldenFleeceCoop);
  }

  public void OnDestroy()
  {
    if ((bool) (UnityEngine.Object) this._currentDungeonModifierText)
      this._currentDungeonModifierText.text = "";
    if ((bool) (UnityEngine.Object) this._fleeceDamageMultiplierText)
      this._fleeceDamageMultiplierText.text = "";
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.UpdateLocalisation);
    CoopManager.Instance.OnPlayerJoined -= new System.Action(this.RefreshHudElementsForGoldenFleeceCoop);
    CoopManager.Instance.OnPlayerLeft -= new System.Action(this.RefreshHudElementsForGoldenFleeceCoop);
  }

  public void OnDisable()
  {
    if ((UnityEngine.Object) HUD_Manager.Instance == (UnityEngine.Object) this)
      HUD_Manager.Instance = (HUD_Manager) null;
    PlayerFleeceManager.OnDamageMultiplierModified -= new PlayerFleeceManager.DamageEvent(this.DamageMultiplierModified);
    TrinketManager.OnTrinketAdded -= new TrinketManager.TrinketUpdated(this.TrinketModified);
    TrinketManager.OnTrinketRemoved -= new TrinketManager.TrinketUpdated(this.TrinketModified);
  }

  public void HealthTransitionsHide() => this.healthManager.HideBars();

  public void HealthTransitionsMoveBackOut() => this.healthManager.Hide(false);

  public void HealthTransitionsMoveBackIn() => this.healthManager.Show();

  public void SingleRelicTransitionHide()
  {
    if (!((UnityEngine.Object) this._relicTransitions != (UnityEngine.Object) null) || !this._relicTransitions.gameObject.activeInHierarchy)
      return;
    this._relicTransitions.hideBar();
  }

  public void SingleRelicTransitionMoveBackOut()
  {
    if (CoopManager.CoopActive || !((UnityEngine.Object) this._relicTransitions != (UnityEngine.Object) null) || !this._relicTransitions.gameObject.activeInHierarchy)
      return;
    this._relicTransitions.MoveBackOutFunction();
  }

  public void SingleRelicTransitionMoveBackIn()
  {
    if (CoopManager.CoopActive || !((UnityEngine.Object) this._relicTransitions != (UnityEngine.Object) null) || !this._relicTransitions.gameObject.activeInHierarchy)
      return;
    this._relicTransitions.MoveBackInFunction();
  }

  public void FaithTransitionsHide()
  {
  }

  public void FaithTransitionsMoveBackOut()
  {
  }

  public void FaithTransitionsMoveBackIn()
  {
  }

  public void Hide(bool Snap, int Delay = 1, bool both = false)
  {
    if (this.Hidden)
      return;
    HUD_Manager.IsTransitioning = true;
    this.StopAllCoroutines();
    if (Snap)
    {
      this.StopAllCoroutines();
      this.healthManager.Hide(false);
      this._notificatioCenter.Hide(true);
      this.HealthTransitionsHide();
      this.BaseDetailsTransition.hideBar();
      this.FaithTransitionsHide();
      this.ConinsContainerTransition.hideBar();
      this.XPBarTransitions.hideBar();
      this.TimeTransitions.hideBar();
      this.MiniMapTransition.hideBar();
      this._survivalBars.hideBar();
      this._playerDamageNotiTransitions.hideBar();
      this.HoldTransitions.hideBar();
      this._damageMultiplierTransition.hideBar();
      this.ReturnToBaseTransitions.hideBar();
      this.SingleRelicTransitionHide();
      if ((UnityEngine.Object) this._curseTransitions != (UnityEngine.Object) null)
        this._curseTransitions.hideBar();
      if ((UnityEngine.Object) this._weaponTransitions != (UnityEngine.Object) null)
        this._weaponTransitions.hideBar();
      if ((UnityEngine.Object) this._doctrineTransitions != (UnityEngine.Object) null)
        this._doctrineTransitions.hideBar();
      this.SingleRelicTransitionHide();
      if ((bool) (UnityEngine.Object) this._dungeonDetailsContainer)
        this._dungeonDetailsContainer.SetActive(false);
      this.FaithTransitionsHide();
      HUD_Manager.IsTransitioning = false;
      if ((bool) (UnityEngine.Object) this._centerElementTransitions)
        this._centerElementTransitions.hideBar();
      this.returnToBaseContainer.SetActive(false);
      this._healContainer.SetActive(false);
      this._burrowContainer.SetActive(false);
      this._heavyAttacksContainer.SetActive(false);
    }
    else if (!both)
    {
      if (!GameManager.IsDungeon(PlayerFarming.Location))
        this.StartCoroutine((IEnumerator) this.HideBaseHUD(Delay));
      else
        this.StartCoroutine((IEnumerator) this.HideDungeonHUD(Delay));
    }
    else
    {
      this.StartCoroutine((IEnumerator) this.HideBaseHUD(Delay));
      this.StartCoroutine((IEnumerator) this.HideDungeonHUD(Delay));
    }
    if ((bool) (UnityEngine.Object) this._objectivesController)
      this._objectivesController.Hide(Snap);
    if ((UnityEngine.Object) NotificationCentre.Instance != (UnityEngine.Object) null)
      NotificationCentre.Instance.Hide(Snap);
    this.Hidden = true;
  }

  public void OnEnable()
  {
    HUD_Manager.Instance = this;
    HUD_Manager.UpdatePlayerHudScale();
    HUD_Manager.IsTransitioning = false;
    this.Hide(true);
    this._editModeTransition.hideBar();
    PlayerFleeceManager.OnDamageMultiplierModified += new PlayerFleeceManager.DamageEvent(this.DamageMultiplierModified);
    TrinketManager.OnTrinketAdded += new TrinketManager.TrinketUpdated(this.TrinketModified);
    TrinketManager.OnTrinketRemoved += new TrinketManager.TrinketUpdated(this.TrinketModified);
  }

  public void ShowEditMode(bool show)
  {
    if (show)
      this._editModeTransition.MoveBackInFunction();
    else
      this._editModeTransition.MoveBackOutFunction();
  }

  public void ShowBW(float Duration, float StartValue, float EndValue)
  {
    this.StopCoroutine((IEnumerator) this.FadeInBW(Duration, StartValue, EndValue));
    this.StartCoroutine((IEnumerator) this.FadeInBW(Duration, StartValue, EndValue));
  }

  public IEnumerator FadeInBW(float Duration, float StartValue, float EndValue)
  {
    this.BWImage.SetActive(true);
    float Progress = 0.0f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      this.BWImage.GetComponent<CanvasGroup>().alpha = Mathf.SmoothStep(StartValue, EndValue, Progress / Duration);
      yield return (object) null;
    }
    this.BWImage.GetComponent<CanvasGroup>().alpha = EndValue;
    if ((double) EndValue == 0.0)
      this.BWImage.SetActive(false);
  }

  public void Show(int Delay = 1, bool Force = false)
  {
    if (!this.Hidden && !Force)
      return;
    if (LetterBox.IsPlaying)
    {
      this.StartCoroutine((IEnumerator) this.FrameDelay((System.Action) (() => this.Show(Delay, Force))));
    }
    else
    {
      HUD_Manager.IsTransitioning = true;
      this.healthContainer.SetActive(DataManager.Instance.dungeonRun > 0);
      this._timeContainer.SetActive(true);
      this._xpBarContainer.SetActive(true);
      this._miniMapContainer.SetActive(true);
      if ((UnityEngine.Object) this._curseTransitions != (UnityEngine.Object) null)
        this._curseTransitions.hideBar();
      if ((UnityEngine.Object) this._weaponTransitions != (UnityEngine.Object) null)
        this._weaponTransitions.hideBar();
      if ((UnityEngine.Object) this._doctrineTransitions != (UnityEngine.Object) null)
        this._doctrineTransitions.hideBar();
      this.SingleRelicTransitionHide();
      this._currentDungeonModifierText.text = "";
      this._currentDungeonModifierText.gameObject.SetActive(false);
      float num1 = -0.5f;
      if ((bool) (UnityEngine.Object) this.returnToBaseContainer)
      {
        this.returnToBaseContainer.SetActive(UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ability_TeleportHome) && this.AbleToMeditateBackToBase() && !DungeonSandboxManager.Active);
        if (this.returnToBaseContainer.activeSelf && BiomeGenerator.RevealHudAbilityIcons)
          this.returnToBaseContainer.GetComponent<HUD_AbilityIcon>().Play(num1 += 0.5f);
      }
      if ((bool) (UnityEngine.Object) this._heavyAttacksContainer)
        this._heavyAttacksContainer.SetActive(true);
      if ((bool) (UnityEngine.Object) this._healContainer)
      {
        this._healContainer.SetActive(this.IsHoldToHealActive(PlayerFarming.Instance));
        if (this._healContainer.activeSelf && BiomeGenerator.RevealHudAbilityIcons)
          this._healContainer.GetComponent<HUD_AbilityIcon>().Play(num1 += 0.5f);
      }
      if ((bool) (UnityEngine.Object) this._burrowContainer)
      {
        this._burrowContainer.SetActive(this.IsBurrowActive());
        if (this._burrowContainer.activeSelf && BiomeGenerator.RevealHudAbilityIcons)
        {
          float num2;
          this._burrowContainer.GetComponent<HUD_AbilityIcon>().Play(num2 = num1 + 0.5f);
        }
      }
      BiomeGenerator.RevealHudAbilityIcons = false;
      this.HealthTransitionsHide();
      this.BaseDetailsTransition.hideBar();
      this.FaithTransitionsHide();
      this.ConinsContainerTransition.hideBar();
      this.XPBarTransitions.hideBar();
      this.TimeTransitions.hideBar();
      this._survivalBars.hideBar();
      this._playerDamageNotiTransitions.hideBar();
      this.MiniMapTransition.hideBar();
      this.HoldTransitions.hideBar();
      this._damageMultiplierTransition.hideBar();
      this.healthManager.HideBars();
      this._notificatioCenter.Hide(true);
      this._fleeceDamageMultiplierText.text = "";
      this._damageMultiplierContainer.SetActive(false);
      this._objectivesController.Show();
      if ((bool) (UnityEngine.Object) this._centerElementTransitions)
        this._centerElementTransitions.MoveBackInFunction();
      if ((UnityEngine.Object) NotificationCentre.Instance != (UnityEngine.Object) null)
        NotificationCentre.Instance.Show();
      Debug.Log((object) $"Are we in dungeon to start with? {GameManager.IsDungeon(PlayerFarming.Location).ToString()} {PlayerFarming.Location.ToString()}");
      if (GameManager.IsDungeon(PlayerFarming.Location))
        this.StartCoroutine((IEnumerator) this.ShowDungeonHUD(Delay));
      else
        this.StartCoroutine((IEnumerator) this.ShowBaseHUD(Delay));
      this.Hidden = false;
    }
  }

  public IEnumerator FrameDelay(System.Action callback)
  {
    yield return (object) new WaitForEndOfFrame();
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void RefreshHudElementsForGoldenFleeceCoop()
  {
    bool state = PlayerFarming.playersCount > 1 && DataManager.Instance.PlayerFleece == 1 && GameManager.IsDungeon(PlayerFarming.Location);
    this._damageMultiplierTransition.SetCoopPositions(state);
    this._notificatioCenter.SetCoopPositions(state);
  }

  public IEnumerator ShowDungeonHUD(int Delay)
  {
    this.RefreshHudElementsForGoldenFleeceCoop();
    this.healthContainer.SetActive(PlayerFarming.Location != FollowerLocation.IntroDungeon);
    this._dungeonDetailsContainer.SetActive(DataManager.Instance.EnabledSpells && PlayerFarming.Location != FollowerLocation.IntroDungeon);
    this._timeContainer.SetActive(!TimeManager.PauseGameTime);
    this._xpBarContainer.SetActive(DataManager.Instance.XPEnabled && (GameManager.HasUnlockAvailable() || DataManager.Instance.DeathCatBeaten));
    this._miniMapContainer.SetActive(true);
    foreach (PlayerFarming player in PlayerFarming.players)
      player.hudHearts.faithAmmoContainer.SetActive(DataManager.Instance.EnabledSpells && PlayerFarming.Location != FollowerLocation.IntroDungeon);
    yield return (object) new WaitForSecondsRealtime((float) Delay);
    if (PlayerFarming.Location != FollowerLocation.IntroDungeon)
    {
      this._notificatioCenter.Show();
      this.healthManager.Show();
      this.HealthTransitionsMoveBackIn();
      this.ReturnToBaseTransitions.MoveBackInFunction();
      this._curseTransitions.MoveBackInFunction();
      this._weaponTransitions.MoveBackInFunction();
      if ((UnityEngine.Object) this._doctrineTransitions != (UnityEngine.Object) null)
        this._doctrineTransitions.MoveBackInFunction();
      this.SingleRelicTransitionMoveBackIn();
      if (DataManager.Instance.EnabledSpells)
      {
        this._dungeonDetailsContainer.SetActive(true);
        this.FaithTransitionsMoveBackIn();
        this.ConinsContainerTransition.MoveBackInFunction();
      }
      if (DataManager.Instance.XPEnabled && (GameManager.HasUnlockAvailable() || DataManager.Instance.DeathCatBeaten))
        this._xpBarContainer.SetActive(true);
      if (!TimeManager.PauseGameTime)
      {
        this._timeContainer.SetActive(true);
        this.TimeTransitions.MoveBackInFunction();
      }
      this.HoldTransitions.MoveBackInFunction();
    }
    this.MiniMapTransition.MoveBackInFunction();
    if (DataManager.Instance.SurvivalModeActive)
    {
      this._survivalBars.MoveBackInFunction();
      this._playerDamageNotiTransitions.MoveBackInFunction();
    }
    this._damageMultiplierContainer.SetActive(DataManager.Instance.PlayerFleece == 1 && GameManager.IsDungeon(PlayerFarming.Location));
    this.RefreshHudElementsForGoldenFleeceCoop();
    this._damageMultiplierTransition.MoveBackInFunction();
    if (this._damageMultiplierContainer.activeSelf)
      this.DamageMultiplierModified(PlayerFleeceManager.GetWeaponDamageMultiplier());
    this._currentDungeonModifierText.text = DungeonModifier.GetCurrentModifierText();
    this._currentDungeonModifierText.gameObject.SetActive(!this._currentDungeonModifierText.text.IsNullOrEmpty());
    if ((UnityEngine.Object) this._centerElementTransitions != (UnityEngine.Object) null)
      this._centerElementTransitions.MoveBackInFunction();
    yield return (object) new WaitForSecondsRealtime(1.5f);
    HUD_Manager.IsTransitioning = false;
    System.Action onShown = HUD_Manager.OnShown;
    if (onShown != null)
      onShown();
  }

  public IEnumerator ShowBaseHUD(int Delay)
  {
    this._dungeonDetailsContainer.SetActive(false);
    this._xpBarContainer.SetActive(DataManager.Instance.XPEnabled && (GameManager.HasUnlockAvailable() || DataManager.Instance.DeathCatBeaten));
    this._timeContainer.SetActive(!TimeManager.PauseGameTime);
    this._dungeonDetailsContainer.SetActive(false);
    yield return (object) new WaitForSeconds((float) Delay);
    this._notificatioCenter.Show();
    this.HealthTransitionsMoveBackIn();
    this.healthManager.Show();
    this._survivalBars.MoveBackInFunction();
    this._playerDamageNotiTransitions.MoveBackInFunction();
    if (DataManager.Instance.XPEnabled && (GameManager.HasUnlockAvailable() || DataManager.Instance.DeathCatBeaten))
    {
      this._xpBarContainer.SetActive(true);
      this.XPBarTransitions.MoveBackInFunction();
    }
    if (!TimeManager.PauseGameTime)
    {
      this._timeContainer.SetActive(true);
      this.TimeTransitions.MoveBackInFunction();
    }
    yield return (object) new WaitForSeconds(1f);
    HUD_Manager.IsTransitioning = false;
    System.Action onShown = HUD_Manager.OnShown;
    if (onShown != null)
      onShown();
  }

  public IEnumerator HideDungeonHUD(int Delay)
  {
    yield return (object) new WaitForSeconds((float) Delay);
    this.HealthTransitionsMoveBackOut();
    this._notificatioCenter.Hide();
    this.ReturnToBaseTransitions.MoveBackOutFunction();
    this._curseTransitions.MoveBackOutFunction();
    this.SingleRelicTransitionMoveBackOut();
    this._weaponTransitions.MoveBackOutFunction();
    if ((UnityEngine.Object) this._doctrineTransitions != (UnityEngine.Object) null)
      this._doctrineTransitions.MoveBackOutFunction();
    this.SingleRelicTransitionMoveBackOut();
    if (DataManager.Instance.EnabledSpells)
    {
      this.FaithTransitionsMoveBackOut();
      this.ConinsContainerTransition.MoveBackOutFunction();
    }
    if (DataManager.Instance.XPEnabled && (GameManager.HasUnlockAvailable() || DataManager.Instance.DeathCatBeaten))
      this.XPBarTransitions.MoveBackOutFunction();
    if (!TimeManager.PauseGameTime)
      this.TimeTransitions.MoveBackOutFunction();
    if (DataManager.Instance.SurvivalModeActive)
    {
      this._survivalBars.MoveBackOutFunction();
      this._playerDamageNotiTransitions.MoveBackOutFunction();
    }
    this.MiniMapTransition.MoveBackOutFunction();
    this.HoldTransitions.MoveBackOutFunction();
    this._centerElementTransitions.MoveBackOutFunction();
    this._damageMultiplierTransition.MoveBackOutFunction();
    yield return (object) new WaitForSeconds(1f);
    HUD_Manager.IsTransitioning = false;
    System.Action onHidden = HUD_Manager.OnHidden;
    if (onHidden != null)
      onHidden();
  }

  public IEnumerator HideBaseHUD(int Delay)
  {
    this._dungeonDetailsContainer.SetActive(false);
    this.returnToBaseContainer.SetActive(false);
    this._healContainer.SetActive(false);
    this._burrowContainer.SetActive(false);
    this._heavyAttacksContainer.SetActive(false);
    yield return (object) new WaitForSecondsRealtime((float) Delay);
    this.healthManager.Hide(false);
    this._playerDamageNotiTransitions.MoveBackOutFunction();
    this.BaseDetailsTransition.MoveBackOutFunction();
    this._survivalBars.MoveBackOutFunction();
    this.XPBarTransitions.MoveBackOutFunction();
    if ((UnityEngine.Object) this._curseTransitions != (UnityEngine.Object) null)
      this._curseTransitions.MoveBackOutFunction();
    if ((UnityEngine.Object) this._weaponTransitions != (UnityEngine.Object) null)
      this._weaponTransitions.MoveBackOutFunction();
    if ((UnityEngine.Object) this._doctrineTransitions != (UnityEngine.Object) null)
      this._doctrineTransitions.MoveBackOutFunction();
    this.SingleRelicTransitionMoveBackOut();
    if (!TimeManager.PauseGameTime)
      this.TimeTransitions.MoveBackOutFunction();
    if ((UnityEngine.Object) this._centerElementTransitions != (UnityEngine.Object) null)
      this._centerElementTransitions.MoveBackOutFunction();
    this.HealthTransitionsMoveBackOut();
    this._notificatioCenter.Hide();
    yield return (object) new WaitForSeconds(1f);
    HUD_Manager.IsTransitioning = false;
    System.Action onHidden = HUD_Manager.OnHidden;
    if (onHidden != null)
      onHidden();
  }

  public void UpdateLocalisation()
  {
    this._localizedDamage = ScriptLocalization.UI_WeaponSelect.Damage;
    this._currentDungeonModifierText.text = DungeonModifier.GetCurrentModifierText();
    this._currentDungeonModifierText.gameObject.SetActive(!this._currentDungeonModifierText.text.IsNullOrEmpty());
  }

  public void HideTopRight()
  {
    if (this.topRightTween != null && this.topRightTween.active)
      this.topRightTween.Complete();
    this.topRightTween = (Tween) this._topRightCanvasGroup.DOFade(0.0f, 1f);
    this._objectivesController.HideAllExcluding("Objectives/GroupTitles/Challenge");
  }

  public void ShowTopRight()
  {
    if (this.topRightTween != null && this.topRightTween.active)
      this.topRightTween.Complete();
    this.topRightTween = (Tween) this._topRightCanvasGroup.DOFade(1f, 1f);
    this._objectivesController.ShowAll();
  }

  public void DamageMultiplierModified(float damageMultiplier)
  {
    if (this.punchTween != null && this.punchTween.active)
      this.punchTween.Complete();
    double num = Math.Round((double) damageMultiplier, 2) * 100.0;
    if (LocalizeIntegration.IsArabic())
      this._fleeceDamageMultiplierText.text = $" %{LocalizeIntegration.ReverseText(num.ToString())}+ {this._localizedDamage} ";
    else
      this._fleeceDamageMultiplierText.text = $"{this._localizedDamage} +{num}%";
    if ((double) damageMultiplier == 1.0)
      this.punchTween = (Tween) this._fleeceDamageMultiplierText.transform.DOShakePosition(1f, new Vector3(10f, 0.0f));
    else
      this.punchTween = (Tween) this._fleeceDamageMultiplierText.transform.DOPunchScale(Vector3.one * 0.25f, 0.25f);
  }

  public void TrinketModified(TarotCards.Card card, PlayerFarming playerFarming = null)
  {
    this._healContainer.SetActive(this.IsHoldToHealActive(playerFarming));
  }

  public bool IsHoldToHealActive(PlayerFarming playerFarming)
  {
    return DataManager.Instance.PlayerFleece == 8 && GameManager.IsDungeon(PlayerFarming.Location) || TrinketManager.HasTrinket(TarotCards.Card.HoldToHeal, playerFarming);
  }

  public bool IsBurrowActive()
  {
    return PlayerFleeceManager.BleatToBurrow() && GameManager.IsDungeon(PlayerFarming.Location);
  }

  public bool AbleToMeditateBackToBase()
  {
    if ((UnityEngine.Object) RespawnRoomManager.Instance != (UnityEngine.Object) null && RespawnRoomManager.Instance.gameObject.activeSelf || PlayerReturnToBase.Disabled)
      return false;
    switch (PlayerFarming.Location)
    {
      case FollowerLocation.Dungeon1_1:
      case FollowerLocation.Dungeon1_2:
      case FollowerLocation.Dungeon1_3:
      case FollowerLocation.Dungeon1_4:
      case FollowerLocation.Dungeon1_5:
      case FollowerLocation.Dungeon1_6:
        return true;
      default:
        return false;
    }
  }

  public void Update()
  {
    if ((UnityEngine.Object) CultFaithManager.Instance != (UnityEngine.Object) null && !CultFaithManager.Instance.gameObject.activeInHierarchy)
      CultFaithManager.Instance.BarController.Update();
    int itemQuantity = Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD);
    this.CoinsAmount.text = itemQuantity.ToString();
    this.CoinsLayoutElement.preferredWidth = itemQuantity <= 9999 ? -1f : 58f;
    if (DataManager.Instance.SurvivalModeActive)
    {
      if (((double) DataManager.Instance.SurvivalMode_Hunger <= 0.0 || (double) DataManager.Instance.SurvivalMode_Sleep <= 0.0) && (double) TimeManager.SurvivalDamagedTimer != -1.0)
      {
        bool showHunger = (double) DataManager.Instance.SurvivalMode_Hunger <= 0.0;
        if (!this._playerDamageNoti.Active && (double) this._playerDamageNotiCanvasGroup.alpha == 0.0 || this._playerDamageNoti.CachedShowHunger != showHunger)
        {
          this._playerDamageNoti.gameObject.SetActive(true);
          this._playerDamageNotiCanvasGroup.DOFade(1f, 0.25f).SetDelay<TweenerCore<float, float, FloatOptions>>(0.5f);
          this._playerDamageNoti.Configure(showHunger);
          for (int index = 0; index < PlayerFarming.playersCount; ++index)
            PlayerFarming.players[index].hudHearts.LowHealthCheckUpdate();
          this._playerDamageNotiTransitions.MoveBackInFunction();
        }
      }
      else if (((double) DataManager.Instance.SurvivalMode_Hunger > 0.0 && (double) DataManager.Instance.SurvivalMode_Sleep > 0.0 || (double) TimeManager.SurvivalDamagedTimer == -1.0) && this._playerDamageNoti.Active)
      {
        this._playerDamageNoti.Active = false;
        this._playerDamageNotiCanvasGroup.DOFade(0.0f, 0.25f).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() =>
        {
          this._playerDamageNoti.gameObject.SetActive(false);
          for (int index = 0; index < PlayerFarming.playersCount; ++index)
            PlayerFarming.players[index].hudHearts.StopLoopedSound();
        }));
      }
    }
    if (PlayerFarming.playersCount <= 1)
      return;
    if ((double) Vector3.Distance(PlayerFarming.players[0].transform.position, PlayerFarming.players[1].transform.position) > 15.0 && !LetterBox.IsPlaying)
    {
      Vector3 endValue1 = Vector3.zero;
      Vector3 endValue2 = Vector3.zero;
      Vector3 screenPos1;
      if (!PlayerFarming.players[0].IsPlayerWithinScreenView(out screenPos1))
      {
        Vector2 viewportPoint = (Vector2) CameraManager.instance.CameraRef.ScreenToViewportPoint(screenPos1);
        viewportPoint.x += (double) viewportPoint.x > 0.5 ? -0.5f : 0.5f;
        endValue1 = Vector3.one * Mathf.Lerp(1f, 0.5f, viewportPoint.magnitude / 1.5f);
        if (!this._playerOneWidget.gameObject.activeSelf)
        {
          this._playerOneWidget.gameObject.SetActive(true);
          this._playerOneWidget.transform.localScale = Vector3.zero;
          this.p1Tween = (Tween) this._playerOneWidget.transform.DOScale(endValue1, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.p1Tween = (Tween) null));
        }
      }
      else if (this._playerOneWidget.gameObject.activeSelf && this.p1Tween == null)
        this.p1Tween = (Tween) this._playerOneWidget.transform.DOScale(Vector3.zero, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
        {
          this._playerOneWidget.gameObject.SetActive(false);
          this.p1Tween = (Tween) null;
        }));
      Vector3 screenPos2;
      if (!PlayerFarming.players[1].IsPlayerWithinScreenView(out screenPos2))
      {
        Vector2 viewportPoint = (Vector2) CameraManager.instance.CameraRef.ScreenToViewportPoint(screenPos2);
        viewportPoint.x += (double) viewportPoint.x > 0.5 ? -0.5f : 0.5f;
        endValue2 = Vector3.one * Mathf.Lerp(1f, 0.5f, viewportPoint.magnitude / 1.5f);
        if (!this._playerTwoWidget.gameObject.activeSelf)
        {
          this._playerTwoWidget.gameObject.SetActive(true);
          this._playerTwoWidget.transform.localScale = Vector3.zero;
          this.p2Tween = (Tween) this._playerTwoWidget.transform.DOScale(endValue2, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.p2Tween = (Tween) null));
        }
      }
      else if (this._playerTwoWidget.gameObject.activeSelf && this.p2Tween == null)
        this.p2Tween = (Tween) this._playerTwoWidget.transform.DOScale(Vector3.zero, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
        {
          this._playerTwoWidget.gameObject.SetActive(false);
          this.p2Tween = (Tween) null;
        }));
      int num = -50;
      RectTransform rectTransform1 = this._playerOneWidget.rectTransform;
      double x1 = (double) screenPos1.x;
      double min1 = (double) this._playerOneWidget.rectTransform.rect.width + (double) (num * -1);
      double width1 = (double) Screen.width;
      Rect rect = this._playerOneWidget.rectTransform.rect;
      double width2 = (double) rect.width;
      double max1 = width1 - width2 - (double) (num * -1);
      double x2 = (double) Mathf.Clamp((float) x1, (float) min1, (float) max1);
      double y1 = (double) screenPos1.y;
      rect = this._playerOneWidget.rectTransform.rect;
      double min2 = (double) rect.height + (double) num;
      double height1 = (double) Screen.height;
      rect = this._playerOneWidget.rectTransform.rect;
      double height2 = (double) rect.height;
      double max2 = height1 - height2 - (double) num;
      double y2 = (double) Mathf.Clamp((float) y1, (float) min2, (float) max2);
      Vector3 vector3_1 = (Vector3) new Vector2((float) x2, (float) y2);
      rectTransform1.position = vector3_1;
      RectTransform rectTransform2 = this._playerTwoWidget.rectTransform;
      double x3 = (double) screenPos2.x;
      rect = this._playerTwoWidget.rectTransform.rect;
      double min3 = (double) rect.width + (double) (num * -1);
      double width3 = (double) Screen.width;
      rect = this._playerTwoWidget.rectTransform.rect;
      double width4 = (double) rect.width;
      double max3 = width3 - width4 - (double) (num * -1);
      double x4 = (double) Mathf.Clamp((float) x3, (float) min3, (float) max3);
      double y3 = (double) screenPos2.y;
      rect = this._playerTwoWidget.rectTransform.rect;
      double min4 = (double) rect.height + (double) num;
      double height3 = (double) Screen.height;
      rect = this._playerTwoWidget.rectTransform.rect;
      double height4 = (double) rect.height;
      double max4 = height3 - height4 - (double) num;
      double y4 = (double) Mathf.Clamp((float) y3, (float) min4, (float) max4);
      Vector3 vector3_2 = (Vector3) new Vector2((float) x4, (float) y4);
      rectTransform2.position = vector3_2;
      this._playerOneArrow.up = this._playerOneArrow.transform.position - screenPos1;
      this._playerTwoArrow.up = this._playerTwoArrow.transform.position - screenPos2;
      if (this.p1Tween == null)
        this._playerOneWidget.transform.localScale = endValue1;
      if (this.p2Tween != null)
        return;
      this._playerTwoWidget.transform.localScale = endValue2;
    }
    else
    {
      this._playerOneWidget.gameObject.SetActive(false);
      this._playerTwoWidget.gameObject.SetActive(false);
    }
  }

  public void LateUpdate()
  {
    if (PlayerFarming.Location == FollowerLocation.Base)
    {
      for (int index = 0; index < this._wolfWidgets.Length; ++index)
      {
        bool showWidget = index < Interaction_WolfBase.Wolfs.Count && (double) Time.timeScale > 0.0 && !LetterBox.IsPlaying && Interaction_WolfBase.WolvesActive && Interaction_WolfBase.Wolfs[index].CurrentState != Interaction_WolfBase.State.Fleeing && (!Interaction_WolfBase.Wolfs[index].IsWithinScreenView() || (double) Vector3.Distance(PlayerFarming.players[0].transform.position, Interaction_WolfBase.Wolfs[index].transform.position) > 7.0);
        Vector3 vector3 = showWidget ? Interaction_WolfBase.Wolfs[index].transform.position + Vector3.back : Vector3.zero;
        Vector3 screenPos = showWidget ? CameraManager.instance.CameraRef.WorldToScreenPoint(vector3) : this._wolfWidgets[index].transform.position;
        Vector3 b = this.SetWidgetPosition(this._wolfWidgets[index], this._wolfArrows[index], vector3, screenPos, showWidget, this._wolfTweens[index]);
        this._wolfWidgets[index].gameObject.SetActive(true);
        if (showWidget && (double) this._wolfWidgets[index].transform.localScale.magnitude < (double) b.magnitude)
          this._wolfWidgets[index].rectTransform.localScale = Vector3.Lerp(this._wolfWidgets[index].rectTransform.localScale, b, 30f * Time.deltaTime);
        else if (!showWidget && (double) this._wolfWidgets[index].transform.localScale.magnitude > 0.0)
          this._wolfWidgets[index].rectTransform.localScale = Vector3.Lerp(this._wolfWidgets[index].rectTransform.localScale, Vector3.zero, 30f * Time.deltaTime);
      }
      for (int index = 0; index < this._rottingCorpseWidgets.Length; ++index)
      {
        if (index >= DeadWorshipper.DeadWorshippers.Count)
        {
          if (this._rottingCorpseWidgets[index].gameObject.activeSelf)
            this._rottingCorpseWidgets[index].gameObject.SetActive(false);
        }
        else
        {
          int idx = index;
          bool showWidget = false;
          if (PlayerFarming.players.Count > 0)
            showWidget = DeadWorshipper.DeadWorshippers.Count > 0 && idx < DeadWorshipper.DeadWorshippers.Count && (double) Time.timeScale > 0.0 && !LetterBox.IsPlaying && DeadWorshipper.DeadWorshippers[idx].gameObject.activeSelf && DeadWorshipper.DeadWorshippers[idx].ShowWidget && (!DeadWorshipper.DeadWorshippers[idx].IsWithinScreenView() || (double) Vector3.Distance(PlayerFarming.players[0].transform.position, DeadWorshipper.DeadWorshippers[idx].transform.position) > 5.0);
          Vector3 position = DeadWorshipper.DeadWorshippers[idx].transform.position;
          Vector3 screenPoint = CameraManager.instance.CameraRef.WorldToScreenPoint(position);
          Vector3 endValue = this.SetWidgetPosition(this._rottingCorpseWidgets[idx], this._rottingCorpseArrows[idx], position, screenPoint, showWidget, this._rottingCorpseTweens[idx]);
          if (showWidget && !this._rottingCorpseWidgets[idx].gameObject.activeSelf)
          {
            this._rottingCorpseWidgets[idx].gameObject.SetActive(true);
            this._rottingCorpseWidgets[idx].rectTransform.localScale = Vector3.zero;
            DeadWorshipper.DeadWorshippers[idx].HideIndicator();
            Tween rottingCorpseTween = this._rottingCorpseTweens[idx];
            if (rottingCorpseTween != null)
              rottingCorpseTween.Kill();
            this._rottingCorpseTweens[idx] = (Tween) this._rottingCorpseWidgets[idx].rectTransform.DOScale(endValue, 0.25f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this._rottingCorpseTweens[idx] = (Tween) null));
          }
          else if (!showWidget && this._rottingCorpseWidgets[idx].gameObject.activeSelf && this._rottingCorpseTweens[idx] == null)
          {
            this._rottingCorpseWidgets[idx].rectTransform.DOKill();
            Tween rottingCorpseTween = this._rottingCorpseTweens[idx];
            if (rottingCorpseTween != null)
              rottingCorpseTween.Kill();
            DeadWorshipper.DeadWorshippers[idx].ShowIndicator();
            this._rottingCorpseTweens[idx] = (Tween) this._rottingCorpseWidgets[idx].rectTransform.DOScale(Vector3.zero, 0.25f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
            {
              this._rottingCorpseWidgets[idx].gameObject.SetActive(false);
              this._rottingCorpseTweens[idx] = (Tween) null;
            }));
          }
        }
      }
      if (this._currentFightingTarget != null && PlayerFarming.Location == FollowerLocation.Base)
      {
        bool showWidget = (!this._currentFightingTarget.IsWithinScreenView() || (double) Vector3.Distance(PlayerFarming.players[0].transform.position, this._currentFightingTarget.Position) > 7.0) && !LetterBox.IsPlaying && (double) Time.timeScale > 0.0;
        Vector3 position = this._currentFightingTarget.Position;
        Vector3 screenPoint = CameraManager.instance.CameraRef.WorldToScreenPoint(position);
        Vector3 endValue = this.SetWidgetPosition(this._fightingWidget, this._fightingArrow, position, screenPoint, showWidget, this.fightingTween, 0.75f);
        if (showWidget)
        {
          this._fightingWidgetTimer += Time.deltaTime;
          if ((double) this._fightingWidgetTimer >= 0.11999999731779099 && BiomeConstants.Instance.IsFlashLightsActive)
          {
            this._fightingIcon.color = this._fightingIcon.color == Color.white ? Color.red : Color.white;
            this._fightingWidgetTimer = 0.0f;
          }
        }
        if (showWidget && !this._fightingWidget.gameObject.activeSelf)
        {
          this._fightingWidget.gameObject.SetActive(true);
          this._fightingWidget.rectTransform.localScale = Vector3.zero;
          this.fightingTween = (Tween) this._fightingWidget.rectTransform.DOScale(endValue, 0.25f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.fightingTween = (Tween) null));
        }
        else if (!showWidget && this._fightingWidget.gameObject.activeSelf && this.fightingTween == null)
        {
          this._fightingWidget.rectTransform.DOKill();
          this.fightingTween = (Tween) this._fightingWidget.rectTransform.DOScale(Vector3.zero, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
          {
            this._fightingWidget.gameObject.SetActive(false);
            this.fightingTween = (Tween) null;
          }));
        }
      }
      else
        this._fightingWidget.gameObject.SetActive(false);
      if ((UnityEngine.Object) Interaction_DLCFurnace.Instance != (UnityEngine.Object) null)
      {
        if (PlayerFarming.Location != FollowerLocation.Base)
        {
          if (this._furnaceWidget.gameObject.activeSelf)
            this._furnaceWidget.gameObject.SetActive(false);
        }
        else
        {
          bool showWidget = (HUD_Manager.ForceFurnaceWidget || !HUD_Manager.ForceFurnaceWidget && (UnityEngine.Object) Interaction_DLCFurnace.Instance.Structure != (UnityEngine.Object) null && Interaction_DLCFurnace.Instance.Structure.Brain != null && Interaction_DLCFurnace.Instance.Structure.Brain.Data.Fuel <= 0 && SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter) && (!Interaction_DLCFurnace.Instance.IsWithinScreenView() || (double) Vector3.Distance(PlayerFarming.players[0].transform.position, Interaction_DLCFurnace.Instance.transform.position) > 7.0) && !LetterBox.IsPlaying && (double) Time.timeScale > 0.0;
          Vector3 endValue = this.SetWidgetPosition(this._furnaceWidget, this._furnaceArrow, Interaction_DLCFurnace.Instance.transform.position, Interaction_DLCFurnace.Instance.GetScreenPosition(), showWidget, this.furnaceTween);
          if (showWidget && !this._furnaceWidget.gameObject.activeSelf)
          {
            this._furnaceWidget.gameObject.SetActive(true);
            this._furnaceWidget.rectTransform.localScale = Vector3.zero;
            this.furnaceTween = (Tween) this._furnaceWidget.rectTransform.DOScale(endValue, 0.25f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.furnaceTween = (Tween) null));
          }
          else if (!showWidget && this._furnaceWidget.gameObject.activeSelf && this.furnaceTween == null)
          {
            this._furnaceWidget.rectTransform.DOKill();
            this.furnaceTween = (Tween) this._furnaceWidget.rectTransform.DOScale(Vector3.zero, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
            {
              this._furnaceWidget.gameObject.SetActive(false);
              this.furnaceTween = (Tween) null;
            }));
          }
        }
      }
      if (this._currentLightningTarget != null && PlayerFarming.Location == FollowerLocation.Base)
      {
        if (this._currentLightningTarget != null && this._currentLightningTarget.FollowerID == -1 || this._currentLightningTarget != null && !DataManager.Instance.Followers_Dead_IDs.Contains(this._currentLightningTarget.FollowerID))
        {
          bool showWidget = (!this._currentLightningTarget.IsWithinScreenView() || (double) Vector3.Distance(PlayerFarming.players[0].transform.position, this._currentLightningTarget.Position) > 7.0) && !LetterBox.IsPlaying && (double) Time.timeScale > 0.0;
          Vector3 position = this._currentLightningTarget.Position;
          Vector3 screenPoint = CameraManager.instance.CameraRef.WorldToScreenPoint(position);
          Vector3 endValue = this.SetWidgetPosition(this._lightningWidget, this._lightningArrow, position, screenPoint, showWidget, this.lightningTween, 0.75f);
          if (showWidget)
          {
            this._lightningWidgetTimer += Time.deltaTime;
            if ((double) this._lightningWidgetTimer >= 0.11999999731779099 && BiomeConstants.Instance.IsFlashLightsActive)
            {
              this._lightningWidget.color = this._lightningWidget.color == Color.white ? Color.red : Color.white;
              this._lightningWidgetTimer = 0.0f;
            }
          }
          if (showWidget && !this._lightningWidget.gameObject.activeSelf)
          {
            this._lightningWidget.gameObject.SetActive(true);
            this._lightningWidget.rectTransform.localScale = Vector3.zero;
            this.lightningTween = (Tween) this._lightningWidget.rectTransform.DOScale(endValue, 0.25f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.lightningTween = (Tween) null));
          }
          else
          {
            if (showWidget || !this._lightningWidget.gameObject.activeSelf || this.lightningTween != null)
              return;
            this._lightningWidget.rectTransform.DOKill();
            this.lightningTween = (Tween) this._lightningWidget.rectTransform.DOScale(Vector3.zero, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
            {
              this._lightningWidget.gameObject.SetActive(false);
              this.lightningTween = (Tween) null;
            }));
          }
        }
        else
          this._lightningWidget.gameObject.SetActive(false);
      }
      else
        this._lightningWidget.gameObject.SetActive(false);
    }
    else
    {
      this._furnaceWidget.gameObject.SetActive(false);
      this._lightningWidget.gameObject.SetActive(false);
      this._fightingWidget.gameObject.SetActive(false);
      foreach (Component wolfWidget in this._wolfWidgets)
        wolfWidget.gameObject.SetActive(false);
      foreach (Component rottingCorpseWidget in this._rottingCorpseWidgets)
        rottingCorpseWidget.gameObject.SetActive(false);
    }
  }

  public void SetFightingTarget(HUD_Manager.FightingTarget target)
  {
    this._currentFightingTarget = target;
  }

  public void ClearFightingTarget()
  {
    this._currentFightingTarget = (HUD_Manager.FightingTarget) null;
  }

  public void SetLightningTarget(SeasonsManager.LightningTarget target)
  {
    this._currentLightningTarget = target;
  }

  public void ClearLightningTarget()
  {
    this._currentLightningTarget = (SeasonsManager.LightningTarget) null;
  }

  public void SetHunderSleepOverlay(float norm)
  {
    this.tentaclesOverlay.unscaledTime = !MonoSingleton<UIManager>.Instance.IsPaused;
    if ((double) norm >= 1.0 && !this.tentaclesOverlay.gameObject.activeSelf)
    {
      this.tentaclesOverlay.gameObject.SetActive(true);
      this.tentaclesOverlay.AnimationState.SetAnimation(0, "tentacles-in", false);
      this.tentaclesOverlay.AnimationState.AddAnimation(0, "tentacles-loop-faster", true, 0.0f);
    }
    else
    {
      if ((double) norm >= 1.0 || !this.tentaclesOverlay.gameObject.activeSelf || !(this.tentaclesOverlay.AnimationState.GetCurrent(0).Animation.Name != "tentacles-out"))
        return;
      if (this.tentaclesOverlay.AnimationState.GetCurrent(0).Animation.Name == "tentacles-in" && (double) this.tentaclesOverlay.AnimationState.GetCurrent(0).TrackTime < 0.10000000149011612)
        this.tentaclesOverlay.gameObject.SetActive(false);
      else
        this.tentaclesOverlay.AnimationState.SetAnimation(0, "tentacles-out", false).Complete += (Spine.AnimationState.TrackEntryDelegate) (trackEntry => this.tentaclesOverlay.gameObject.SetActive(false));
    }
  }

  public Vector3 SetWidgetPosition(
    Image widget,
    RectTransform arrow,
    Vector3 worldPos,
    Vector3 screenPos,
    bool showWidget,
    Tween tween,
    float minScale = 0.5f)
  {
    Vector3 position = screenPos;
    Vector3 zero = Vector3.zero;
    Vector3 rhs = worldPos - CameraManager.instance.CameraRef.transform.position;
    if ((double) Vector3.Dot(CameraManager.instance.CameraRef.transform.forward, rhs) < 0.0)
    {
      position.x = (float) Screen.width - position.x;
      position.y = (float) Screen.height - position.y;
    }
    Vector2 viewportPoint = (Vector2) CameraManager.instance.CameraRef.ScreenToViewportPoint(position);
    viewportPoint.x += (double) viewportPoint.x > 0.5 ? -0.5f : 0.5f;
    float t = (viewportPoint - new Vector2(0.5f, 0.5f)).magnitude / 1.5f;
    Vector3 vector3_1 = Vector3.one * Mathf.Lerp(1f, minScale, t);
    int num1 = -50;
    RectTransform rectTransform1 = widget.rectTransform;
    double x = (double) Mathf.Clamp(position.x, widget.rectTransform.rect.width + (float) (num1 * -1), (float) Screen.width - widget.rectTransform.rect.width - (float) (num1 * -1));
    double y1 = (double) position.y;
    Rect rect = widget.rectTransform.rect;
    double min1 = (double) rect.height + (double) num1;
    double height1 = (double) Screen.height;
    rect = widget.rectTransform.rect;
    double height2 = (double) rect.height;
    double max1 = height1 - height2 - (double) num1;
    double y2 = (double) Mathf.Clamp((float) y1, (float) min1, (float) max1);
    Vector3 vector3_2 = (Vector3) new Vector2((float) x, (float) y2);
    rectTransform1.position = vector3_2;
    RectTransform rectTransform2 = widget.rectTransform;
    rect = rectTransform2.rect;
    float num2 = rect.width * 0.5f;
    rect = rectTransform2.rect;
    float num3 = rect.height * 0.5f;
    float min2 = (float) Screen.width * 0.026041666f + num2;
    float max2 = (float) Screen.width - (float) Screen.width * 0.192708328f - num2;
    float min3 = (float) Screen.height * 0.0462962948f + num3;
    float max3 = (float) Screen.height - (float) Screen.height * 0.2777778f - num3;
    widget.rectTransform.position = (Vector3) new Vector2(Mathf.Clamp(position.x, min2, max2), Mathf.Clamp(position.y, min3, max3));
    arrow.up = arrow.transform.position - position;
    if (((tween == null ? 1 : (tween.IsComplete() ? 1 : 0)) & (showWidget ? 1 : 0)) != 0)
      widget.rectTransform.localScale = vector3_1;
    return vector3_1;
  }

  public Indicator InitialiseIndicator(PlayerFarming playerFarming)
  {
    this.indicator.gameObject.SetActive(false);
    Indicator indicator = UnityEngine.Object.Instantiate<Indicator>(this.indicator, this.transform);
    indicator.playerFarming = playerFarming;
    indicator.gameObject.SetActive(true);
    UIPromptBase.UpdateAllPromptPositions();
    return indicator;
  }

  public Indicator InitialiseIndicator()
  {
    this.indicator.gameObject.SetActive(false);
    Indicator indicator = UnityEngine.Object.Instantiate<Indicator>(this.indicator, this.transform);
    indicator.DontDestroy = true;
    indicator.gameObject.SetActive(true);
    UIPromptBase.UpdateAllPromptPositions();
    return indicator;
  }

  public void ClearPlayersWidgets()
  {
    if (this.p1Tween != null && this.p1Tween.active)
    {
      this.p1Tween.Kill();
      this.p1Tween = (Tween) null;
    }
    if (this.p2Tween != null && this.p2Tween.active)
    {
      this.p2Tween.Kill();
      this.p2Tween = (Tween) null;
    }
    this._playerTwoWidget.gameObject.SetActive(false);
    this._playerOneWidget.gameObject.SetActive(false);
  }

  [CompilerGenerated]
  public void \u003CUpdate\u003Eb__116_0()
  {
    this._playerDamageNoti.gameObject.SetActive(false);
    for (int index = 0; index < PlayerFarming.playersCount; ++index)
      PlayerFarming.players[index].hudHearts.StopLoopedSound();
  }

  [CompilerGenerated]
  public void \u003CUpdate\u003Eb__116_3() => this.p1Tween = (Tween) null;

  [CompilerGenerated]
  public void \u003CUpdate\u003Eb__116_1()
  {
    this._playerOneWidget.gameObject.SetActive(false);
    this.p1Tween = (Tween) null;
  }

  [CompilerGenerated]
  public void \u003CUpdate\u003Eb__116_4() => this.p2Tween = (Tween) null;

  [CompilerGenerated]
  public void \u003CUpdate\u003Eb__116_2()
  {
    this._playerTwoWidget.gameObject.SetActive(false);
    this.p2Tween = (Tween) null;
  }

  [CompilerGenerated]
  public void \u003CLateUpdate\u003Eb__118_2() => this.fightingTween = (Tween) null;

  [CompilerGenerated]
  public void \u003CLateUpdate\u003Eb__118_3()
  {
    this._fightingWidget.gameObject.SetActive(false);
    this.fightingTween = (Tween) null;
  }

  [CompilerGenerated]
  public void \u003CLateUpdate\u003Eb__118_4() => this.furnaceTween = (Tween) null;

  [CompilerGenerated]
  public void \u003CLateUpdate\u003Eb__118_5()
  {
    this._furnaceWidget.gameObject.SetActive(false);
    this.furnaceTween = (Tween) null;
  }

  [CompilerGenerated]
  public void \u003CLateUpdate\u003Eb__118_6() => this.lightningTween = (Tween) null;

  [CompilerGenerated]
  public void \u003CLateUpdate\u003Eb__118_7()
  {
    this._lightningWidget.gameObject.SetActive(false);
    this.lightningTween = (Tween) null;
  }

  [CompilerGenerated]
  public void \u003CSetHunderSleepOverlay\u003Eb__125_0(TrackEntry trackEntry)
  {
    this.tentaclesOverlay.gameObject.SetActive(false);
  }

  public class FightingTarget
  {
    public Vector3 _staticPosition;

    public Vector3 Position => this._staticPosition;

    public FightingTarget(Vector3 staticPosition) => this._staticPosition = staticPosition;

    public bool IsWithinScreenView()
    {
      Vector3 screenPoint = CameraManager.instance.CameraRef.WorldToScreenPoint(this.Position);
      return (double) screenPoint.x > 0.0 & (double) screenPoint.x < (double) Screen.width && (double) screenPoint.y > 0.0 && (double) screenPoint.y < (double) (Screen.height - 100);
    }
  }
}
