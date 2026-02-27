// Decompiled with JetBrains decompiler
// Type: HUD_Manager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using Lamb.UI;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using WebSocketSharp;

#nullable disable
public class HUD_Manager : BaseMonoBehaviour
{
  public static HUD_Manager Instance;
  public static System.Action OnShown;
  public static System.Action OnHidden;
  [SerializeField]
  private GameObject _baseDetailsContainer;
  public UI_Transitions BaseDetailsTransition;
  [SerializeField]
  private GameObject _dungeonDetailsContainer;
  public UI_Transitions FaithAmmoTransition;
  [SerializeField]
  private GameObject _miniMapContainer;
  public UI_Transitions MiniMapTransition;
  [SerializeField]
  private GameObject _timeContainer;
  public UI_Transitions TimeTransitions;
  [Space]
  [SerializeField]
  private CanvasGroup _topRightCanvasGroup;
  [SerializeField]
  private TMP_Text _currentDungeonModifierText;
  [SerializeField]
  private GameObject _damageMultiplierContainer;
  [SerializeField]
  private UI_Transitions _damageMultiplierTransition;
  [SerializeField]
  private TMP_Text _fleeceDamageMultiplierText;
  [SerializeField]
  private GameObject _healthContainer;
  [SerializeField]
  private UI_Transitions _healthTransitions;
  [SerializeField]
  private GameObject _xpBarContainer;
  public UI_Transitions XPBarTransitions;
  [SerializeField]
  private GameObject _returnToBaseContainer;
  public UI_Transitions ReturnToBaseTransitions;
  public UI_Transitions _curseTransitions;
  public UI_Transitions _weaponTransitions;
  public UI_Transitions _doctrineTransitions;
  [SerializeField]
  private GameObject returnToBaseContainer;
  [SerializeField]
  private GameObject _healContainer;
  public UI_Transitions HoldTransitions;
  [SerializeField]
  private NotificationCentre _notificatioCenter;
  [SerializeField]
  private UIObjectivesController _objectivesController;
  [SerializeField]
  private UI_Transitions _centerElementTransitions;
  [SerializeField]
  private UI_Transitions _editModeTransition;
  public static bool IsTransitioning;
  private Tween topRightTween;
  private string _localizedDamage;
  public GameObject BWImage;
  private Tween punchTween;

  public TMP_Text CurrentDungeonModifierText => this._currentDungeonModifierText;

  public bool Hidden { set; get; }

  private void Awake()
  {
    this._fleeceDamageMultiplierText.text = "";
    this._damageMultiplierContainer.SetActive(false);
    this.UpdateLocalisation();
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.UpdateLocalisation);
  }

  private void Start()
  {
    this.Hidden = false;
    this.Hide(true);
  }

  private void OnDestroy()
  {
    if ((bool) (UnityEngine.Object) this._currentDungeonModifierText)
      this._currentDungeonModifierText.text = "";
    if ((bool) (UnityEngine.Object) this._fleeceDamageMultiplierText)
      this._fleeceDamageMultiplierText.text = "";
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.UpdateLocalisation);
  }

  private void OnDisable()
  {
    if ((UnityEngine.Object) HUD_Manager.Instance == (UnityEngine.Object) this)
      HUD_Manager.Instance = (HUD_Manager) null;
    PlayerFleeceManager.OnDamageMultiplierModified -= new PlayerFleeceManager.DamageEvent(this.DamageMultiplierModified);
    TrinketManager.OnTrinketAdded -= new TrinketManager.TrinketUpdated(this.TrinketModified);
    TrinketManager.OnTrinketRemoved -= new TrinketManager.TrinketUpdated(this.TrinketModified);
  }

  public void Hide(bool Snap, int Delay = 1, bool both = false)
  {
    if (this.Hidden)
      return;
    HUD_Manager.IsTransitioning = true;
    if (Snap)
    {
      this.StopAllCoroutines();
      this._notificatioCenter.Hide(true);
      this._healthTransitions.hideBar();
      this.BaseDetailsTransition.hideBar();
      this.FaithAmmoTransition.hideBar();
      this.XPBarTransitions.hideBar();
      this.TimeTransitions.hideBar();
      this.MiniMapTransition.hideBar();
      this.HoldTransitions.hideBar();
      this._damageMultiplierTransition.hideBar();
      if ((UnityEngine.Object) this._curseTransitions != (UnityEngine.Object) null)
        this._curseTransitions.hideBar();
      if ((UnityEngine.Object) this._weaponTransitions != (UnityEngine.Object) null)
        this._weaponTransitions.hideBar();
      if ((UnityEngine.Object) this._doctrineTransitions != (UnityEngine.Object) null)
        this._doctrineTransitions.hideBar();
      if ((bool) (UnityEngine.Object) this._dungeonDetailsContainer)
        this._dungeonDetailsContainer.SetActive(false);
      if ((UnityEngine.Object) this.FaithAmmoTransition != (UnityEngine.Object) null)
        this.FaithAmmoTransition.hideBar();
      HUD_Manager.IsTransitioning = false;
      if ((bool) (UnityEngine.Object) this._centerElementTransitions)
        this._centerElementTransitions.hideBar();
      this.returnToBaseContainer.SetActive(false);
      this._healContainer.SetActive(false);
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

  private void OnEnable()
  {
    HUD_Manager.Instance = this;
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

  private IEnumerator FadeInBW(float Duration, float StartValue, float EndValue)
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
      this._healthContainer.SetActive(DataManager.Instance.dungeonRun > 0);
      this._baseDetailsContainer.SetActive(true);
      this._dungeonDetailsContainer.SetActive(true);
      this._timeContainer.SetActive(true);
      this._xpBarContainer.SetActive(true);
      this._miniMapContainer.SetActive(true);
      if ((UnityEngine.Object) this._curseTransitions != (UnityEngine.Object) null)
        this._curseTransitions.hideBar();
      if ((UnityEngine.Object) this._weaponTransitions != (UnityEngine.Object) null)
        this._weaponTransitions.hideBar();
      if ((UnityEngine.Object) this._doctrineTransitions != (UnityEngine.Object) null)
        this._doctrineTransitions.hideBar();
      this._currentDungeonModifierText.text = "";
      this._currentDungeonModifierText.gameObject.SetActive(false);
      if ((bool) (UnityEngine.Object) this.returnToBaseContainer)
        this.returnToBaseContainer.SetActive(UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ability_TeleportHome) && this.AbleToMeditateBackToBase());
      if ((bool) (UnityEngine.Object) this._healContainer)
        this._healContainer.SetActive(TrinketManager.HasTrinket(TarotCards.Card.HoldToHeal));
      this._healthTransitions.hideBar();
      this.BaseDetailsTransition.hideBar();
      this.FaithAmmoTransition.hideBar();
      this.XPBarTransitions.hideBar();
      this.TimeTransitions.hideBar();
      this.MiniMapTransition.hideBar();
      this.HoldTransitions.hideBar();
      this._damageMultiplierTransition.hideBar();
      this._notificatioCenter.Hide(true);
      this._fleeceDamageMultiplierText.text = "";
      this._damageMultiplierContainer.SetActive(false);
      this._objectivesController.Show();
      if ((bool) (UnityEngine.Object) this._centerElementTransitions)
        this._centerElementTransitions.MoveBackInFunction();
      if ((UnityEngine.Object) NotificationCentre.Instance != (UnityEngine.Object) null)
        NotificationCentre.Instance.Show();
      if (GameManager.IsDungeon(PlayerFarming.Location))
        this.StartCoroutine((IEnumerator) this.ShowDungeonHUD(Delay));
      else
        this.StartCoroutine((IEnumerator) this.ShowBaseHUD(Delay));
      this.Hidden = false;
    }
  }

  private IEnumerator FrameDelay(System.Action callback)
  {
    yield return (object) new WaitForEndOfFrame();
    System.Action action = callback;
    if (action != null)
      action();
  }

  private IEnumerator ShowDungeonHUD(int Delay)
  {
    this._healthContainer.SetActive(PlayerFarming.Location != FollowerLocation.IntroDungeon);
    this._dungeonDetailsContainer.SetActive(DataManager.Instance.EnabledSpells && PlayerFarming.Location != FollowerLocation.IntroDungeon);
    this._timeContainer.SetActive(!TimeManager.PauseGameTime);
    this._xpBarContainer.SetActive(DataManager.Instance.XPEnabled && GameManager.HasUnlockAvailable());
    this._miniMapContainer.SetActive(true);
    this._baseDetailsContainer.SetActive(false);
    yield return (object) new WaitForSecondsRealtime((float) Delay);
    if (PlayerFarming.Location != FollowerLocation.IntroDungeon)
    {
      this._notificatioCenter.Show();
      this._healthTransitions.MoveBackInFunction();
      if ((UnityEngine.Object) this._curseTransitions != (UnityEngine.Object) null)
        this._curseTransitions.MoveBackInFunction();
      if ((UnityEngine.Object) this._weaponTransitions != (UnityEngine.Object) null)
        this._weaponTransitions.MoveBackInFunction();
      if ((UnityEngine.Object) this._doctrineTransitions != (UnityEngine.Object) null)
        this._doctrineTransitions.MoveBackInFunction();
      if (DataManager.Instance.EnabledSpells)
      {
        this._dungeonDetailsContainer.SetActive(true);
        this.FaithAmmoTransition.MoveBackInFunction();
      }
      if (DataManager.Instance.XPEnabled && GameManager.HasUnlockAvailable())
        this._xpBarContainer.SetActive(true);
      if (!TimeManager.PauseGameTime)
      {
        this._timeContainer.SetActive(true);
        this.TimeTransitions.MoveBackInFunction();
      }
      this.HoldTransitions.MoveBackInFunction();
    }
    this.MiniMapTransition.MoveBackInFunction();
    this._damageMultiplierContainer.SetActive(DataManager.Instance.PlayerFleece == 1 && GameManager.IsDungeon(PlayerFarming.Location));
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

  private IEnumerator ShowBaseHUD(int Delay)
  {
    this._baseDetailsContainer.SetActive(true);
    this._xpBarContainer.SetActive(DataManager.Instance.XPEnabled && GameManager.HasUnlockAvailable());
    this._timeContainer.SetActive(!TimeManager.PauseGameTime);
    this._dungeonDetailsContainer.SetActive(false);
    yield return (object) new WaitForSeconds((float) Delay);
    this._notificatioCenter.Show();
    this._healthTransitions.MoveBackInFunction();
    this._baseDetailsContainer.SetActive(true);
    this.BaseDetailsTransition.MoveBackInFunction();
    if (DataManager.Instance.XPEnabled && GameManager.HasUnlockAvailable())
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

  private IEnumerator HideDungeonHUD(int Delay)
  {
    this._baseDetailsContainer.SetActive(false);
    yield return (object) new WaitForSeconds((float) Delay);
    this._healthTransitions.MoveBackOutFunction();
    this._notificatioCenter.Hide();
    if ((UnityEngine.Object) this._curseTransitions != (UnityEngine.Object) null)
      this._curseTransitions.MoveBackOutFunction();
    if ((UnityEngine.Object) this._weaponTransitions != (UnityEngine.Object) null)
      this._weaponTransitions.MoveBackOutFunction();
    if ((UnityEngine.Object) this._doctrineTransitions != (UnityEngine.Object) null)
      this._doctrineTransitions.MoveBackOutFunction();
    if (DataManager.Instance.EnabledSpells)
      this.FaithAmmoTransition.MoveBackOutFunction();
    if (DataManager.Instance.XPEnabled && GameManager.HasUnlockAvailable())
      this.XPBarTransitions.MoveBackOutFunction();
    if (!TimeManager.PauseGameTime)
      this.TimeTransitions.MoveBackOutFunction();
    if ((UnityEngine.Object) this.MiniMapTransition != (UnityEngine.Object) null)
      this.MiniMapTransition.MoveBackOutFunction();
    if ((UnityEngine.Object) this.HoldTransitions != (UnityEngine.Object) null)
      this.HoldTransitions.MoveBackOutFunction();
    if ((UnityEngine.Object) this._centerElementTransitions != (UnityEngine.Object) null)
      this._centerElementTransitions.MoveBackOutFunction();
    this._damageMultiplierTransition.MoveBackOutFunction();
    yield return (object) new WaitForSeconds(1f);
    HUD_Manager.IsTransitioning = false;
    System.Action onHidden = HUD_Manager.OnHidden;
    if (onHidden != null)
      onHidden();
  }

  private IEnumerator HideBaseHUD(int Delay)
  {
    this._dungeonDetailsContainer.SetActive(false);
    this.returnToBaseContainer.SetActive(false);
    this._healContainer.SetActive(false);
    yield return (object) new WaitForSecondsRealtime((float) Delay);
    this.BaseDetailsTransition.MoveBackOutFunction();
    this.XPBarTransitions.MoveBackOutFunction();
    if ((UnityEngine.Object) this._curseTransitions != (UnityEngine.Object) null)
      this._curseTransitions.MoveBackOutFunction();
    if ((UnityEngine.Object) this._weaponTransitions != (UnityEngine.Object) null)
      this._weaponTransitions.MoveBackOutFunction();
    if ((UnityEngine.Object) this._doctrineTransitions != (UnityEngine.Object) null)
      this._doctrineTransitions.MoveBackOutFunction();
    if (!TimeManager.PauseGameTime)
      this.TimeTransitions.MoveBackOutFunction();
    if ((UnityEngine.Object) this._centerElementTransitions != (UnityEngine.Object) null)
      this._centerElementTransitions.MoveBackOutFunction();
    this._healthTransitions.MoveBackOutFunction();
    this._notificatioCenter.Hide();
    yield return (object) new WaitForSeconds(1f);
    HUD_Manager.IsTransitioning = false;
    System.Action onHidden = HUD_Manager.OnHidden;
    if (onHidden != null)
      onHidden();
  }

  private void UpdateLocalisation()
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

  private void DamageMultiplierModified(float damageMultiplier)
  {
    if (this.punchTween != null && this.punchTween.active)
      this.punchTween.Complete();
    this._fleeceDamageMultiplierText.text = $"{this._localizedDamage} +{Math.Round((double) damageMultiplier, 2) * 100.0}%";
    if ((double) damageMultiplier == 1.0)
      this.punchTween = (Tween) this._fleeceDamageMultiplierText.transform.DOShakePosition(1f, new Vector3(10f, 0.0f));
    else
      this.punchTween = (Tween) this._fleeceDamageMultiplierText.transform.DOPunchScale(Vector3.one * 0.25f, 0.25f);
  }

  private void TrinketModified(TarotCards.Card card)
  {
    this._healContainer.SetActive(TrinketManager.HasTrinket(TarotCards.Card.HoldToHeal));
  }

  private bool AbleToMeditateBackToBase()
  {
    if ((UnityEngine.Object) RespawnRoomManager.Instance != (UnityEngine.Object) null && RespawnRoomManager.Instance.gameObject.activeSelf)
      return false;
    switch (PlayerFarming.Location)
    {
      case FollowerLocation.Dungeon1_1:
      case FollowerLocation.Dungeon1_2:
      case FollowerLocation.Dungeon1_3:
      case FollowerLocation.Dungeon1_4:
        return true;
      default:
        return false;
    }
  }

  private void Update()
  {
    if (!((UnityEngine.Object) CultFaithManager.Instance != (UnityEngine.Object) null) || CultFaithManager.Instance.gameObject.activeInHierarchy)
      return;
    CultFaithManager.Instance.BarController.Update();
  }
}
