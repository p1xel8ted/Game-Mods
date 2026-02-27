// Decompiled with JetBrains decompiler
// Type: Lamb.UI.DeathScreen.UIDeathScreenOverlayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Coffee.UIExtensions;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Map;
using MMTools;
using src.Extensions;
using src.UINavigator;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI.DeathScreen;

public class UIDeathScreenOverlayController : UIMenuBase
{
  public static UIDeathScreenOverlayController.Results Result;
  public static UIDeathScreenOverlayController Instance;
  public static List<UIDeathScreenOverlayController.Results> DestroyFragileLootResults = new List<UIDeathScreenOverlayController.Results>()
  {
    UIDeathScreenOverlayController.Results.Killed,
    UIDeathScreenOverlayController.Results.Escaped,
    UIDeathScreenOverlayController.Results.None,
    UIDeathScreenOverlayController.Results.GameOver
  };
  [Header("Selectables")]
  [SerializeField]
  public MMButton _continueButton;
  [SerializeField]
  public MMButton _restartButton;
  [SerializeField]
  public GameObject _buttonHighlight;
  [Header("Text")]
  [SerializeField]
  public TextMeshProUGUI _title;
  [SerializeField]
  public TextMeshProUGUI _subTitle;
  [SerializeField]
  public TextMeshProUGUI _timeText;
  [SerializeField]
  public TextMeshProUGUI _killsText;
  [SerializeField]
  public GameObject _killsIcon;
  [SerializeField]
  public GameObject _followersIcon;
  [Header("Containers")]
  [SerializeField]
  public RectTransform _root;
  [SerializeField]
  public RectTransform _titleContainer;
  [SerializeField]
  public RectTransform _levelNodeContainer;
  [SerializeField]
  public RectTransform _itemsContainer;
  [SerializeField]
  public RectTransform _skullContainer;
  [Header("Prefabs")]
  [SerializeField]
  public GameObject _levelNodePrefab;
  [SerializeField]
  public DeathScreenInventoryItem _itemTemplate;
  [Header("Canvas Groups")]
  [SerializeField]
  public CanvasGroup _runtimeCanvasGroup;
  [SerializeField]
  public RectTransform _runtimeRectTransform;
  [SerializeField]
  public CanvasGroup _killsCanvasGroup;
  [SerializeField]
  public RectTransform _killsRectTransform;
  [SerializeField]
  public CanvasGroup _continueCanvasGroup;
  [SerializeField]
  public CanvasGroup _restartCanvasGroup;
  [SerializeField]
  public CanvasGroup _backgroundGroup;
  [SerializeField]
  public CanvasGroup _particleGroup;
  [SerializeField]
  public CanvasGroup _particleGroupMountain;
  [SerializeField]
  public CanvasGroup _buttonbackgroundGroup;
  [SerializeField]
  public CanvasGroup _controllerPrompts;
  [SerializeField]
  public CanvasGroup _titleGroup;
  [SerializeField]
  public CanvasGroup _itemCanvasGroup;
  [SerializeField]
  public CanvasGroup _skullCanvasGroup;
  [SerializeField]
  public Image _dividerIcon;
  [Header("Effects")]
  [SerializeField]
  public UIParticle _uiParticle;
  [SerializeField]
  public Material _diedMaterial;
  [SerializeField]
  public Material _completedMaterial;
  [Header("Sandbox")]
  [SerializeField]
  public RectTransform _sandBoxContainer;
  [SerializeField]
  public RectTransform _godTear;
  [SerializeField]
  public DeathScreenInventoryItem _godTearTotal;
  [SerializeField]
  public RectTransform _instantBar;
  [SerializeField]
  public RectTransform _xpBar;
  [SerializeField]
  public CanvasGroup _xpCanvasGroup;
  [SerializeField]
  public TextMeshProUGUI _xpText;
  [Header("Other")]
  [SerializeField]
  public UIMenuControlPrompts _controlPrompts;
  public List<InventoryItem.ITEM_TYPE> _blacklistedItems = new List<InventoryItem.ITEM_TYPE>()
  {
    InventoryItem.ITEM_TYPE.SEEDS,
    InventoryItem.ITEM_TYPE.INGREDIENTS,
    InventoryItem.ITEM_TYPE.MEALS,
    InventoryItem.ITEM_TYPE.GIFT_MEDIUM,
    InventoryItem.ITEM_TYPE.GIFT_SMALL,
    InventoryItem.ITEM_TYPE.MONSTER_HEART
  };
  public static List<InventoryItem.ITEM_TYPE> _excludeLootFromBonus = new List<InventoryItem.ITEM_TYPE>()
  {
    InventoryItem.ITEM_TYPE.Necklace_1,
    InventoryItem.ITEM_TYPE.Necklace_2,
    InventoryItem.ITEM_TYPE.Necklace_3,
    InventoryItem.ITEM_TYPE.Necklace_4,
    InventoryItem.ITEM_TYPE.Necklace_5,
    InventoryItem.ITEM_TYPE.Necklace_Dark,
    InventoryItem.ITEM_TYPE.Necklace_Demonic,
    InventoryItem.ITEM_TYPE.Necklace_Loyalty,
    InventoryItem.ITEM_TYPE.Necklace_Light,
    InventoryItem.ITEM_TYPE.Necklace_Missionary,
    InventoryItem.ITEM_TYPE.Necklace_Gold_Skull,
    InventoryItem.ITEM_TYPE.Necklace_Deaths_Door,
    InventoryItem.ITEM_TYPE.Necklace_Winter,
    InventoryItem.ITEM_TYPE.Necklace_Frozen,
    InventoryItem.ITEM_TYPE.Necklace_Weird,
    InventoryItem.ITEM_TYPE.Necklace_Targeted,
    InventoryItem.ITEM_TYPE.GIFT_MEDIUM,
    InventoryItem.ITEM_TYPE.GIFT_SMALL,
    InventoryItem.ITEM_TYPE.MONSTER_HEART,
    InventoryItem.ITEM_TYPE.BEHOLDER_EYE,
    InventoryItem.ITEM_TYPE.BEHOLDER_EYE_ROT,
    InventoryItem.ITEM_TYPE.SHELL,
    InventoryItem.ITEM_TYPE.GOD_TEAR,
    InventoryItem.ITEM_TYPE.PLEASURE_POINT,
    InventoryItem.ITEM_TYPE.BROKEN_WEAPON_HAMMER,
    InventoryItem.ITEM_TYPE.BROKEN_WEAPON_SWORD,
    InventoryItem.ITEM_TYPE.BROKEN_WEAPON_DAGGER,
    InventoryItem.ITEM_TYPE.BROKEN_WEAPON_AXE,
    InventoryItem.ITEM_TYPE.BROKEN_WEAPON_GAUNTLETS,
    InventoryItem.ITEM_TYPE.BROKEN_WEAPON_BLUNDERBUSS,
    InventoryItem.ITEM_TYPE.BROKEN_WEAPON_CHAIN,
    InventoryItem.ITEM_TYPE.REPAIRED_WEAPON_HAMMER,
    InventoryItem.ITEM_TYPE.REPAIRED_WEAPON_SWORD,
    InventoryItem.ITEM_TYPE.REPAIRED_WEAPON_DAGGER,
    InventoryItem.ITEM_TYPE.REPAIRED_WEAPON_GAUNTLETS,
    InventoryItem.ITEM_TYPE.REPAIRED_WEAPON_BLUNDERBUSS,
    InventoryItem.ITEM_TYPE.REPAIRED_WEAPON_CHAIN,
    InventoryItem.ITEM_TYPE.ILLEGIBLE_LETTER_SCYLLA,
    InventoryItem.ITEM_TYPE.ILLEGIBLE_LETTER_CHARYBDIS,
    InventoryItem.ITEM_TYPE.FISHING_ROD,
    InventoryItem.ITEM_TYPE.YNGYA_GHOST,
    InventoryItem.ITEM_TYPE.SPECIAL_WOOL_RANCHER,
    InventoryItem.ITEM_TYPE.SPECIAL_WOOL_BLACKSMITH,
    InventoryItem.ITEM_TYPE.SPECIAL_WOOL_DECORATION,
    InventoryItem.ITEM_TYPE.SPECIAL_WOOL_LAMBWAR,
    InventoryItem.ITEM_TYPE.SPECIAL_WOOL_TAROT,
    InventoryItem.ITEM_TYPE.SPECIAL_WOOL_6,
    InventoryItem.ITEM_TYPE.SPECIAL_WOOL_7,
    InventoryItem.ITEM_TYPE.SPECIAL_WOOL_8,
    InventoryItem.ITEM_TYPE.SPECIAL_WOOL_9,
    InventoryItem.ITEM_TYPE.SPECIAL_WOOL_10,
    InventoryItem.ITEM_TYPE.SPECIAL_WOOL_11,
    InventoryItem.ITEM_TYPE.BOP,
    InventoryItem.ITEM_TYPE.LEGENDARY_WEAPON_FRAGMENT
  };
  public static List<InventoryItem.ITEM_TYPE> _fragileLoot = new List<InventoryItem.ITEM_TYPE>()
  {
    InventoryItem.ITEM_TYPE.YNGYA_GHOST
  };
  public Dictionary<InventoryItem.ITEM_TYPE, int> _lootDelta;
  public UIDeathScreenOverlayController.Results _result;
  public int _levels;
  public List<DeathScreenInventoryItem> inventoryItems = new List<DeathScreenInventoryItem>();
  public static bool LastRunWasSandBox = false;
  public static bool UsedBossPortal = false;
  public int ResetSandBoxCount;
  public Vector3 ChallengeCoinsStartPosition;
  public Vector3 FullGodTearPosition;
  public Vector3 TotalGodTearPosition;
  public DeathScreenInventoryItem ChallengeCoins;
  public DeathScreenInventoryItem FullGodTear;
  public int FullGodTearCount;
  public bool beatBoss;
  public bool HasGodTears;
  public bool DisplayinPenalty;

  public static List<InventoryItem.ITEM_TYPE> ExcludeLootFromBonus
  {
    get => UIDeathScreenOverlayController._excludeLootFromBonus;
  }

  public void OnEnable()
  {
    MonoSingleton<UIManager>.Instance.SetCurrentCursor(0);
    MonoSingleton<UIManager>.Instance.ResetPreviousCursor();
  }

  public void Show(UIDeathScreenOverlayController.Results result, bool instant = false)
  {
    this.Show(result, (UnityEngine.Object) MapManager.Instance != (UnityEngine.Object) null ? ((bool) (UnityEngine.Object) MapManager.Instance.DungeonConfig ? MapManager.Instance.DungeonConfig.layers.Count : 1) : 1, instant);
  }

  public void Show(UIDeathScreenOverlayController.Results result, int levels, bool instant = false)
  {
    UIDeathScreenOverlayController.Instance = this;
    PlayerReturnToBase.Disabled = false;
    if (result == UIDeathScreenOverlayController.Results.Killed && DataManager.Instance.PermadeDeathActive)
      result = UIDeathScreenOverlayController.Results.GameOver;
    this._result = result;
    this._levels = levels;
    this._continueButton.interactable = this._continueButton.enabled = false;
    this._buttonHighlight.SetActive(false);
    this._xpCanvasGroup.alpha = 0.0f;
    this._titleGroup.alpha = 0.0f;
    this._subTitle.alpha = 0.0f;
    if (this._result != UIDeathScreenOverlayController.Results.AwokenMountain)
    {
      this._particleGroup.alpha = 0.0f;
      this._particleGroupMountain.gameObject.SetActive(false);
    }
    else
    {
      this._particleGroupMountain.alpha = 0.0f;
      this._particleGroup.gameObject.SetActive(false);
    }
    this._backgroundGroup.alpha = 0.0f;
    this._runtimeCanvasGroup.alpha = 0.0f;
    this._killsCanvasGroup.alpha = 0.0f;
    this._continueCanvasGroup.alpha = 0.0f;
    this._restartCanvasGroup.alpha = 0.0f;
    this._buttonbackgroundGroup.alpha = 0.0f;
    this._controllerPrompts.alpha = 0.0f;
    this._itemCanvasGroup.alpha = 0.0f;
    this._skullCanvasGroup.alpha = 0.0f;
    this._dividerIcon.enabled = false;
    Time.timeScale = 0.0f;
    UIDeathScreenOverlayController.Result = result;
    DataManager.Instance.ForcingPlayerWeaponDLC = EquipmentType.None;
    DataManager.Instance.LegendaryBlunderbussPlimboEasterEggActive = false;
    DataManager.Instance.IsMiniBoss = true;
    if ((bool) (UnityEngine.Object) this._controlPrompts)
      this._controlPrompts.HideAcceptButton();
    if (result != UIDeathScreenOverlayController.Results.Killed && result != UIDeathScreenOverlayController.Results.Escaped && result != UIDeathScreenOverlayController.Results.GameOver && !UIDeathScreenOverlayController.UsedBossPortal)
    {
      if (PlayerFarming.Location == FollowerLocation.Dungeon1_5 || PlayerFarming.Location == FollowerLocation.Dungeon1_6 || PlayerFarming.Location == FollowerLocation.Boss_Wolf || PlayerFarming.Location == FollowerLocation.Boss_Yngya)
        DataManager.CompleteDungeonMapNode();
      if (!DungeonSandboxManager.Active)
      {
        foreach (PlayerFarming player in PlayerFarming.players)
        {
          if (player.playerWeapon.IsLegendaryWeapon())
            ObjectiveManager.CompletLegendaryWeaponRunObjective(player.currentWeapon);
        }
      }
    }
    UIDeathScreenOverlayController.UsedBossPortal = false;
    this.Show(instant);
    AudioManager.Instance.StopActiveLoopsAndSFX();
  }

  public void InitSandboxXPBar()
  {
    float x = (float) DataManager.Instance.CurrentChallengeModeXP / (float) DataManager.Instance.CurrentChallengeModeTargetXP;
    this._instantBar.localScale = new Vector3(x, 1f);
    this._xpBar.localScale = new Vector3(x, 1f);
    this._xpText.text = $"{DataManager.Instance.CurrentChallengeModeXP.ToString()}{FontImageNames.GetIconByType(InventoryItem.ITEM_TYPE.GOD_TEAR_FRAGMENT)}/{DataManager.Instance.CurrentChallengeModeTargetXP.ToString()}";
  }

  public void ResetSandboxXPBar()
  {
    RectTransform xpBar = this._xpBar;
    RectTransform instantBar = this._instantBar;
    Vector3 vector3_1 = new Vector3(0.0f, 1f);
    Vector3 vector3_2 = vector3_1;
    instantBar.localScale = vector3_2;
    Vector3 vector3_3 = vector3_1;
    xpBar.localScale = vector3_3;
    ++this.ResetSandBoxCount;
    Color color1 = this._xpText.color;
    Color color2 = this._xpText.color;
    color1.a = 0.0f;
    this._xpText.color = color1;
    ShortcutExtensionsTMPText.DOColor(this._xpText, color2, 0.5f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    this._xpText.text = $"0{FontImageNames.GetIconByType(InventoryItem.ITEM_TYPE.GOD_TEAR_FRAGMENT)}/{DataManager.Instance.CurrentChallengeModeTargetXP.ToString()}";
  }

  public void MoveChallengeGold() => this.StartCoroutine(this.MoveChallengeGoldRoutine());

  public IEnumerator MoveChallengeGoldRoutine()
  {
    yield return (object) new WaitForSecondsRealtime(3f);
    while ((UnityEngine.Object) this.ChallengeCoins == (UnityEngine.Object) null && Inventory.itemsDungeon.Count > 0)
      yield return (object) null;
    yield return (object) new WaitForSecondsRealtime(1f);
    if ((bool) (UnityEngine.Object) this.ChallengeCoins)
    {
      this.ChallengeCoinsStartPosition = this.ChallengeCoins.transform.position;
      this.FullGodTearPosition = this.FullGodTear.transform.position;
      this.ChallengeCoins.transform.DOScale(1.5f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      yield return (object) new WaitForSecondsRealtime(0.25f);
      UIManager.PlayAudio("event:/player/float_follower");
      yield return (object) new WaitForSecondsRealtime(0.5f);
      this.ChallengeCoins.transform.DOMove(this._sandBoxContainer.transform.position, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      this.ChallengeCoins.transform.DOScale(1f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      yield return (object) new WaitForSecondsRealtime(0.5f);
      UIManager.PlayAudio("event:/ui/level_node_end_screen_ui_appear");
      MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
      this.ChallengeCoins.gameObject.SetActive(false);
      this.ChallengeCoins.transform.position = this.ChallengeCoinsStartPosition;
      this.ChallengeCoins.transform.localScale = Vector3.one;
      this.TotalGodTearPosition = this._godTearTotal.transform.position;
      this._godTearTotal.gameObject.SetActive(false);
    }
    this.UpdateSandboxXPBar();
  }

  public void UpdateSandboxXPBar(float Delay = 0.0f)
  {
    this.StartCoroutine(this.UpdateSandboxXpBarRoutine(Delay));
  }

  public IEnumerator UpdateSandboxXpBarRoutine(float Delay)
  {
    // ISSUE: reference to a compiler-generated field
    int num1 = this.\u003C\u003E1__state;
    UIDeathScreenOverlayController overlayController = this;
    float TargetScale;
    if (num1 != 0)
    {
      if (num1 != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      float num2 = 0.5f;
      overlayController.StartCoroutine(overlayController.TweenText(num2));
      overlayController._xpBar.DOScale(new Vector3(Mathf.Min(TargetScale, 1f), 1f), num2).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(new TweenCallback(overlayController.\u003CUpdateSandboxXpBarRoutine\u003Eb__69_0));
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    int itemQuantity = Inventory.GetItemQuantity(128 /*0x80*/);
    Inventory.SetItemQuantity(128 /*0x80*/, 0);
    DataManager.Instance.CurrentChallengeModeXP += itemQuantity;
    TargetScale = Mathf.Min((float) DataManager.Instance.CurrentChallengeModeXP / (float) DataManager.Instance.CurrentChallengeModeTargetXP, 1f);
    overlayController._instantBar.localScale = new Vector3(TargetScale, 1f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSecondsRealtime(Delay);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator TweenText(float Duration)
  {
    float Time = 0.0f;
    float StartingValue = 0.0f;
    while ((double) (Time += Time.unscaledDeltaTime) < (double) Duration)
    {
      this._xpText.text = $"{Mathf.RoundToInt(Mathf.Lerp(StartingValue, (float) Mathf.Min(DataManager.Instance.CurrentChallengeModeXP, DataManager.Instance.CurrentChallengeModeTargetXP), Time / Duration)).ToString()}{FontImageNames.GetIconByType(InventoryItem.ITEM_TYPE.GOD_TEAR_FRAGMENT)}/{DataManager.Instance.CurrentChallengeModeTargetXP.ToString()}";
      if ((double) UnityEngine.Random.value < 0.20000000298023224)
        UIManager.PlayAudio("event:/followers/pop_in");
      yield return (object) null;
    }
    this._xpText.text = $"{Mathf.Min(DataManager.Instance.CurrentChallengeModeXP, DataManager.Instance.CurrentChallengeModeTargetXP).ToString()}{FontImageNames.GetIconByType(InventoryItem.ITEM_TYPE.GOD_TEAR_FRAGMENT)}/{DataManager.Instance.CurrentChallengeModeTargetXP.ToString()}";
  }

  public IEnumerator CheckXP()
  {
    if (DataManager.Instance.CurrentChallengeModeXP >= DataManager.Instance.CurrentChallengeModeTargetXP)
    {
      this._godTear.transform.localScale = Vector3.one * 1.2f;
      this._godTear.transform.DOScale(1f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      UIManager.PlayAudio("event:/player/float_follower");
      yield return (object) new WaitForSecondsRealtime(0.5f);
      Vector3 GodTearStartPosition = this._godTear.position;
      this._godTear.DOMove(this.FullGodTear.transform.position, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      yield return (object) new WaitForSecondsRealtime(0.5f);
      this.FullGodTear.gameObject.SetActive(true);
      this.FullGodTear.transform.localScale = Vector3.one * 1.5f;
      this.FullGodTear.transform.DOScale(1f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      this._godTear.gameObject.SetActive(false);
      this.FullGodTear.Configure(InventoryItem.ITEM_TYPE.GOD_TEAR);
      Inventory.AddItem(InventoryItem.ITEM_TYPE.GOD_TEAR, 1);
      ++this.FullGodTearCount;
      this.FullGodTear.AmountText.text = this.FullGodTearCount.ToString() ?? "";
      UIManager.PlayAudio("event:/ui/level_node_end_screen_ui_appear");
      MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
      yield return (object) new WaitForSecondsRealtime(0.25f);
      this._godTear.transform.position = GodTearStartPosition;
      this._godTear.transform.localScale = Vector3.zero;
      this._godTear.gameObject.SetActive(true);
      this._godTear.transform.DOScale(1f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      yield return (object) new WaitForSecondsRealtime(0.5f);
      DataManager.Instance.CurrentChallengeModeXP -= DataManager.Instance.CurrentChallengeModeTargetXP;
      ++DataManager.Instance.CurrentChallengeModeLevel;
      bool flag = DataManager.Instance.CurrentChallengeModeXP > 0;
      for (int index = this.inventoryItems.Count - 1; index >= 0; --index)
      {
        if (this.inventoryItems[index].Type == InventoryItem.ITEM_TYPE.GOD_TEAR)
        {
          flag = true;
          break;
        }
      }
      if (flag)
      {
        yield return (object) new WaitForSecondsRealtime(0.5f);
        this.ResetSandboxXPBar();
        this.UpdateSandboxXPBar(0.5f);
      }
      else
      {
        this.ShowContinueButton();
        this._continueButton.interactable = this._continueButton.enabled = true;
      }
      GodTearStartPosition = new Vector3();
    }
    else
    {
      this._godTearTotal.gameObject.SetActive(true);
      this._godTearTotal.transform.position = this.TotalGodTearPosition + new Vector3(0.0f, -200f, 0.0f);
      this._godTearTotal.transform.DOMove(this.TotalGodTearPosition, 0.75f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InElastic).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      this._godTearTotal.transform.localScale = Vector3.zero;
      this._godTearTotal.transform.DOScale(1.7f, 0.75f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      this._godTearTotal.Configure(InventoryItem.ITEM_TYPE.GOD_TEAR);
      this._godTearTotal.AmountText.text = (this._godTearTotal.Item.quantity - this.FullGodTearCount).ToString() ?? "";
      UIManager.PlayAudio("event:/ui/level_node_die");
      yield return (object) new WaitForSecondsRealtime(1.5f);
      UIManager.PlayAudio("event:/ui/level_node_end_screen_ui_appear");
      this.FullGodTear.transform.DOMove(this.TotalGodTearPosition, 0.75f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      yield return (object) new WaitForSecondsRealtime(0.75f);
      UIManager.PlayAudio("event:/ui/objective_complete");
      this._godTearTotal.transform.localScale = Vector3.one * 3f;
      this._godTearTotal.transform.DOScale(1.7f, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      this._godTearTotal.AmountText.text = this._godTearTotal.Item.quantity.ToString() ?? "";
      this.FullGodTear.gameObject.SetActive(false);
      this.ShowContinueButton();
      this._continueButton.interactable = this._continueButton.enabled = true;
    }
  }

  public override void OnShowStarted()
  {
    TwitchHelpHinder.Abort();
    SimulationManager.UnPause();
    DataManager.ResetRunData();
    this._continueButton.onClick.AddListener((UnityAction) (() => this.Hide()));
    if (DataManager.Instance.SurvivalModeActive && this._result == UIDeathScreenOverlayController.Results.Killed)
    {
      DataManager.Instance.SurvivalMode_Hunger = Mathf.Clamp(DataManager.Instance.SurvivalMode_Hunger - (float) UnityEngine.Random.Range(10, 20), 10f, 100f);
      DataManager.Instance.SurvivalMode_Sleep = Mathf.Clamp(DataManager.Instance.SurvivalMode_Sleep - (float) UnityEngine.Random.Range(10, 20), 10f, 100f);
    }
    if (this._result == UIDeathScreenOverlayController.Results.GameOver)
    {
      this._skullContainer.gameObject.SetActive(true);
      this._levelNodeContainer.gameObject.SetActive(false);
    }
    else
      this._skullContainer.gameObject.SetActive(false);
    switch (this._result)
    {
      case UIDeathScreenOverlayController.Results.Killed:
        this._title.text = ScriptLocalization.UI_DeathScreen_Killed.Title;
        this._subTitle.text = ScriptLocalization.UI_DeathScreen_Killed.Subtitle;
        this._uiParticle.material = this._diedMaterial;
        break;
      case UIDeathScreenOverlayController.Results.Completed:
        this._title.text = ScriptLocalization.UI_DeathScreen_Completed.Title;
        this._subTitle.text = ScriptLocalization.UI_DeathScreen_Completed.Subtitle;
        this.beatBoss = true;
        this._uiParticle.material = this._completedMaterial;
        break;
      case UIDeathScreenOverlayController.Results.Escaped:
        this._title.text = ScriptLocalization.UI_DeathScreen_Escaped.Title;
        this._subTitle.text = ScriptLocalization.UI_DeathScreen_Escaped.Subtitle;
        this._uiParticle.material = this._diedMaterial;
        break;
      case UIDeathScreenOverlayController.Results.GameOver:
        this._title.text = ScriptLocalization.UI_DeathScreen_GameOver.Title;
        this._subTitle.text = ScriptLocalization.UI_DeathScreen_GameOver.Subtitle;
        if (DataManager.Instance.SurvivalModeActive)
        {
          if ((double) DataManager.Instance.SurvivalMode_Hunger <= 0.0)
            this._subTitle.text = LocalizationManager.GetTranslation("UI/DeathScreen/GameOver/Starving/Subtitle");
          else if ((double) DataManager.Instance.SurvivalMode_Sleep <= 0.0)
            this._subTitle.text = LocalizationManager.GetTranslation("UI/DeathScreen/GameOver/Exhausted/Subtitle");
        }
        this._uiParticle.material = this._diedMaterial;
        break;
      case UIDeathScreenOverlayController.Results.AwokenMountain:
        this._title.text = LocalizationManager.GetTranslation("UI/DeathScreenMountain");
        this._subTitle.text = ScriptLocalization.UI_DeathScreen_Escaped.Subtitle;
        this._uiParticle.material = this._completedMaterial;
        break;
    }
    if (UIDeathScreenOverlayController.Result == UIDeathScreenOverlayController.Results.Killed)
    {
      DataManager.Instance.LastRunResults = UIDeathScreenOverlayController.Results.Killed;
      DataManager.Instance.DiedLastRun = true;
    }
    else
      DataManager.Instance.DiedLastRun = false;
    if (this._result != UIDeathScreenOverlayController.Results.Escaped)
    {
      if (!DungeonSandboxManager.Active)
      {
        for (int index = 0; index < DataManager.Instance.Followers_Demons_IDs.Count; ++index)
        {
          FollowerInfo infoById = FollowerInfo.GetInfoByID(DataManager.Instance.Followers_Demons_IDs[index]);
          if (infoById != null)
            FollowerBrain.GetOrCreateBrain(infoById)?.AddThought(this._result == UIDeathScreenOverlayController.Results.Completed ? Thought.DemonSuccessfulRun : Thought.DemonFailedRun);
        }
      }
    }
    else if (this._result == UIDeathScreenOverlayController.Results.Completed)
    {
      DataManager.Instance.playerDeathsInARow = 0;
      DataManager.Instance.playerDeathsInARowFightingLeader = 0;
    }
    if (!DungeonSandboxManager.Active)
    {
      DataManager.Instance.Followers_Demons_IDs.Clear();
      DataManager.Instance.Followers_Demons_Types.Clear();
    }
    if (this._result != UIDeathScreenOverlayController.Results.GameOver)
      return;
    SimulationManager.Pause();
    FollowerManager.Reset();
    StructureManager.Reset();
    DeviceLightingManager.Reset();
    SaveAndLoad.DeleteSaveSlot(SaveAndLoad.SAVE_SLOT);
    TwitchManager.Abort();
    UIDynamicNotificationCenter.Reset();
  }

  public override IEnumerator DoHide()
  {
    UIDeathScreenOverlayController overlayController = this;
    if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
    {
      for (int index = 0; index < PlayerFarming.playersCount; ++index)
      {
        if ((bool) (UnityEngine.Object) PlayerFarming.players[index].playerController)
          PlayerFarming.players[index].playerController.enabled = false;
      }
    }
    overlayController._continueButton.interactable = false;
    UIDeathScreenOverlayController.Results lastRunResults = DataManager.Instance.LastRunResults;
    int num;
    switch (lastRunResults)
    {
      case UIDeathScreenOverlayController.Results.BeatenMiniBoss:
      case UIDeathScreenOverlayController.Results.BeatenBoss:
      case UIDeathScreenOverlayController.Results.BeatenBossNoDamage:
      case UIDeathScreenOverlayController.Results.AwokenMountain:
        num = 0;
        break;
      default:
        num = lastRunResults != UIDeathScreenOverlayController.Results.Completed ? 1 : 0;
        break;
    }
    bool flag = num == 0;
    Inventory.ClearDungeonItems();
    Debug.Log((object) ("typE: " + DeathCatRoomManager.GetConversationType().ToString()));
    if (CheatConsole.IN_DEMO && PlayerFarming.Location == FollowerLocation.Dungeon1_1)
      MMTransition.Play(MMTransition.TransitionType.ChangeSceneAutoResume, MMTransition.Effect.BlackFade, "DemoOver", 1f, "", new System.Action(overlayController.\u003CDoHide\u003Eb__77_0));
    else if (overlayController._result == UIDeathScreenOverlayController.Results.GameOver)
    {
      SimulationManager.Pause();
      DeviceLightingManager.Reset();
      FollowerManager.Reset();
      StructureManager.Reset();
      SaveAndLoad.DeleteSaveSlot(SaveAndLoad.SAVE_SLOT);
      TwitchManager.Abort();
      UIDynamicNotificationCenter.Reset();
      MMTransition.ForceShowIcon = true;
      MMTransition.Play(MMTransition.TransitionType.ChangeSceneAutoResume, MMTransition.Effect.BlackFade, "Main Menu", 1f, "", new System.Action(overlayController.\u003CDoHide\u003Eb__77_1));
    }
    else if (!DataManager.Instance.ForeshadowedMysticShop && !DataManager.Instance.OnboardedMysticShop && DataManager.Instance.GetDungeonLayer(FollowerLocation.Dungeon1_4) > 2 && !DataManager.Instance.RemovedStoryMomentsActive)
    {
      if ((UnityEngine.Object) RespawnRoomManager.Instance != (UnityEngine.Object) null)
        RespawnRoomManager.Instance.gameObject.SetActive(false);
      MysticShopKeeperManager.Play();
      MysticShopKeeperManager.Instance.transform.parent = (Transform) null;
      DG.Tweening.Sequence sequence = DOTween.Sequence();
      sequence.AppendInterval(0.5f).SetUpdate<DG.Tweening.Sequence>(true);
      sequence.Play<DG.Tweening.Sequence>().SetUpdate<DG.Tweening.Sequence>(true);
    }
    else if (DeathCatRoomManager.GetConversationType() != DeathCatRoomManager.ConversationTypes.None && (UnityEngine.Object) DeathCatController.Instance == (UnityEngine.Object) null && !DataManager.Instance.RemovedStoryMomentsActive && PlayerFarming.Location != FollowerLocation.Boss_Yngya && PlayerFarming.Location != FollowerLocation.Boss_Wolf && PlayerFarming.Location != FollowerLocation.Dungeon1_5 && PlayerFarming.Location != FollowerLocation.Dungeon1_6)
    {
      if ((UnityEngine.Object) RespawnRoomManager.Instance != (UnityEngine.Object) null)
        RespawnRoomManager.Instance.gameObject.SetActive(false);
      DeathCatRoomManager.Play();
      DG.Tweening.Sequence sequence = DOTween.Sequence();
      sequence.AppendInterval(0.5f).SetUpdate<DG.Tweening.Sequence>(true);
      sequence.Play<DG.Tweening.Sequence>().SetUpdate<DG.Tweening.Sequence>(true);
    }
    else if (((PlayerFarming.Location != FollowerLocation.Dungeon1_6 ? 0 : (YngyaRoomManager.ShowHeartRoom() ? 1 : 0)) & (flag ? 1 : 0)) != 0)
    {
      if ((UnityEngine.Object) RespawnRoomManager.Instance != (UnityEngine.Object) null)
        RespawnRoomManager.Instance.gameObject.SetActive(false);
      YngyaRoomManager.Play();
      DG.Tweening.Sequence sequence = DOTween.Sequence();
      sequence.AppendInterval(0.5f).SetUpdate<DG.Tweening.Sequence>(true);
      sequence.Play<DG.Tweening.Sequence>().SetUpdate<DG.Tweening.Sequence>(true);
    }
    else
    {
      string scene = "Base Biome 1";
      if (GameManager.Layer2 && (DataManager.Instance.LastRunResults == UIDeathScreenOverlayController.Results.BeatenBoss || DataManager.Instance.LastRunResults == UIDeathScreenOverlayController.Results.BeatenBossNoDamage))
      {
        DataManager.Instance.LastRunResults = UIDeathScreenOverlayController.Results.None;
        QuoteScreenController.Init(new List<QuoteScreenController.QuoteTypes>()
        {
          overlayController.GetQuoteType()
        }, (System.Action) (() => GameManager.ToShip(scene)), (System.Action) (() => GameManager.ToShip()));
        MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, "QuoteScreen", 5f, "", (System.Action) (() => Time.timeScale = 1f));
      }
      else
        GameManager.ToShip(scene);
    }
    yield return (object) new WaitForSecondsRealtime(1f);
    System.Action onHide = overlayController.OnHide;
    if (onHide != null)
      onHide();
    overlayController.SetActiveStateForMenu(false);
    overlayController.gameObject.SetActive(false);
    System.Action onHidden = overlayController.OnHidden;
    if (onHidden != null)
      onHidden();
    overlayController.OnHideCompleted();
  }

  public override void OnShowCompleted()
  {
    if (!UIDeathScreenOverlayController.LastRunWasSandBox || !this.HasGodTears)
      this._continueButton.interactable = this._continueButton.enabled = true;
    this._buttonHighlight.SetActive(true);
    this.ActivateNavigation();
    this._controlPrompts.ShowAcceptButton();
  }

  public override IEnumerator DoShowAnimation()
  {
    UIDeathScreenOverlayController overlayController = this;
    Debug.Log((object) "Do show animation");
    if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
    {
      for (int index = 0; index < PlayerFarming.playersCount; ++index)
      {
        HealthPlayer component = PlayerFarming.players[index].GetComponent<HealthPlayer>();
        component.HP = component.totalHP;
      }
    }
    overlayController._levelNodeContainer.DestroyAllChildren();
    overlayController._itemsContainer.DestroyAllChildren();
    DataManager.Instance.FollowersRecruitedInNodes.Add(DataManager.Instance.FollowersRecruitedThisNode);
    DataManager.Instance.FollowersRecruitedThisNode = 0;
    yield return (object) null;
    switch (overlayController._result)
    {
      case UIDeathScreenOverlayController.Results.Killed:
      case UIDeathScreenOverlayController.Results.GameOver:
        UIManager.PlayAudio("event:/ui/martyred");
        break;
      case UIDeathScreenOverlayController.Results.Completed:
      case UIDeathScreenOverlayController.Results.AwokenMountain:
        UIManager.PlayAudio("event:/ui/heretics_defeated");
        break;
      case UIDeathScreenOverlayController.Results.Escaped:
        UIManager.PlayAudio("event:/ui/martyred");
        break;
    }
    if (Inventory.itemsDungeon.Count == 0)
    {
      overlayController._itemsContainer.gameObject.SetActive(false);
    }
    else
    {
      string str1 = "0";
      string str2 = "0";
      string str3 = "0";
      string str4 = "0";
      foreach (InventoryItem inventoryItem in Inventory.itemsDungeon)
      {
        if (inventoryItem.type == 172)
          str1 = inventoryItem.quantity.ToString();
        else if (inventoryItem.type == 139)
          str2 = inventoryItem.quantity.ToString();
        else if (inventoryItem.type == 230)
          str3 = inventoryItem.quantity.ToString();
        else if (inventoryItem.type == 165)
          str4 = inventoryItem.quantity.ToString();
      }
      AnalyticsLogger.LogEvent(AnalyticsLogger.EventType.CrusadeResults, str1, str2, str3, str4);
    }
    Vector2 titleOrigin = (Vector2) overlayController._title.rectTransform.localPosition;
    UIDeathScreenOverlayController.LastRunWasSandBox = DungeonSandboxManager.Active;
    overlayController.HasGodTears = Inventory.GetDungeonItemByType(128 /*0x80*/) != null;
    overlayController._sandBoxContainer.gameObject.SetActive(UIDeathScreenOverlayController.LastRunWasSandBox && overlayController.HasGodTears);
    if (UIDeathScreenOverlayController.LastRunWasSandBox)
    {
      if (DungeonSandboxManager.Active && overlayController._result != UIDeathScreenOverlayController.Results.Killed)
      {
        DungeonSandboxManager.ProgressionSnapshot progressionForScenario = DungeonSandboxManager.GetProgressionForScenario(DungeonSandboxManager.CurrentScenario.ScenarioType, (PlayerFleeceManager.FleeceType) DataManager.Instance.PlayerFleece);
        ++progressionForScenario.CompletedRuns;
        if (progressionForScenario.CompletedRuns >= DungeonSandboxManager.GetDataForScenarioType(DungeonSandboxManager.CurrentScenario.ScenarioType).Count)
        {
          AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("COMPLETE_CHALLENGE_ROW"));
          Debug.Log((object) "ACHIEVEMENT GOT : COMPLETE_CHALLENGE_ROW");
        }
      }
      overlayController.InitSandboxXPBar();
      if (overlayController.HasGodTears)
        overlayController.MoveChallengeGold();
    }
    overlayController._title.rectTransform.SetParent((Transform) overlayController._root);
    overlayController._title.rectTransform.localPosition = Vector3.zero;
    overlayController._title.rectTransform.localScale = Vector3.one * 4f;
    overlayController._title.rectTransform.DOScale(Vector3.one * 2f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    overlayController._titleGroup.DOFade(1f, 0.5f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    DOTween.To(new DOGetter<float>(overlayController.\u003CDoShowAnimation\u003Eb__80_0), new DOSetter<float>(overlayController.\u003CDoShowAnimation\u003Eb__80_1), 1f, 2f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(1f);
    if (overlayController._result != UIDeathScreenOverlayController.Results.AwokenMountain)
      overlayController._particleGroup.DOFade(1f, 2f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InQuart);
    else
      overlayController._particleGroupMountain.DOFade(1f, 2f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InQuart);
    if ((UnityEngine.Object) HUD_Manager.Instance != (UnityEngine.Object) null)
      HUD_Manager.Instance.ShowBW(0.5f, 1f, 0.0f);
    overlayController._title.rectTransform.SetParent((Transform) overlayController._titleContainer, true);
    overlayController._title.rectTransform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    overlayController._title.rectTransform.DOLocalMove((Vector3) titleOrigin, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.5f);
    ShortcutExtensionsTMPText.DOFade(overlayController._subTitle, 1f, 0.5f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.OutSine).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    overlayController._titleGroup.DOFade(1f, 0.5f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    overlayController._title.rectTransform.DOShakeScale(0.5f, 0.2f).SetUpdate<Tweener>(true);
    overlayController.inventoryItems = new List<DeathScreenInventoryItem>();
    if (Inventory.itemsDungeon.Count > 0 && overlayController._result != UIDeathScreenOverlayController.Results.GameOver)
    {
      foreach (InventoryItem inventoryItem in Inventory.itemsDungeon)
      {
        if (!overlayController._blacklistedItems.Contains((InventoryItem.ITEM_TYPE) inventoryItem.type))
        {
          DeathScreenInventoryItem screenInventoryItem = overlayController._itemTemplate.Instantiate<DeathScreenInventoryItem>((Transform) overlayController._itemsContainer);
          screenInventoryItem.Configure(inventoryItem, true);
          screenInventoryItem.CanvasGroup.alpha = 0.0f;
          overlayController.inventoryItems.Add(screenInventoryItem);
          if (UIDeathScreenOverlayController.LastRunWasSandBox)
          {
            if (inventoryItem.type == 128 /*0x80*/)
            {
              overlayController.ChallengeCoins = screenInventoryItem;
              if ((UnityEngine.Object) overlayController.FullGodTear == (UnityEngine.Object) null)
              {
                overlayController.FullGodTear = overlayController.ChallengeCoins;
                overlayController.FullGodTearCount = 0;
              }
            }
            if (inventoryItem.type == 119)
            {
              overlayController.FullGodTear = screenInventoryItem;
              overlayController.FullGodTearCount = overlayController.FullGodTear.Item.quantity;
            }
          }
        }
      }
      overlayController._itemCanvasGroup.DOFade(1f, 0.3f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
      yield return (object) new WaitForSecondsRealtime(0.5f);
      if (UIDeathScreenOverlayController.LastRunWasSandBox)
        overlayController._xpCanvasGroup.DOFade(1f, 1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
      foreach (UIInventoryItem inventoryItem in overlayController.inventoryItems)
      {
        inventoryItem.CanvasGroup.DOFade(1f, 2f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
        yield return (object) new WaitForSecondsRealtime(0.1f);
      }
    }
    if (overlayController._result == UIDeathScreenOverlayController.Results.Killed || overlayController._result == UIDeathScreenOverlayController.Results.Escaped)
    {
      int levels = overlayController._levels;
      int index = -1;
      while (++index < levels)
      {
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(overlayController._levelNodePrefab, (Transform) overlayController._levelNodeContainer);
        gameObject.SetActive(true);
        UIDeathScreenLevelNode component = gameObject.GetComponent<UIDeathScreenLevelNode>();
        UIDeathScreenLevelNode.LevelNodeSkins LevelNodeSkin1 = index == levels - 1 ? UIDeathScreenLevelNode.LevelNodeSkins.boss : UIDeathScreenLevelNode.LevelNodeSkins.normal;
        if (index < DataManager.Instance.dungeonVisitedRooms.Count - 1 && (bool) (UnityEngine.Object) MapManager.Instance.DungeonConfig)
        {
          NodeBlueprint blueprint = MapManager.GetBlueprint(DataManager.Instance.dungeonVisitedRooms[index], MapManager.Instance.DungeonConfig);
          Debug.Log((object) $"config:{MapManager.Instance.DungeonConfig?.ToString()} type: {DataManager.Instance.dungeonVisitedRooms[index].ToString()} blueprint: {blueprint?.ToString()}");
          if ((UnityEngine.Object) blueprint != (UnityEngine.Object) null)
          {
            component.icon.sprite = blueprint.GetSprite(DataManager.Instance.dungeonLocationsVisited[index], false);
            component.icon.gameObject.SetActive(true);
          }
          UIDeathScreenLevelNode.LevelNodeSkins LevelNodeSkin2;
          switch (DataManager.Instance.dungeonVisitedRooms[index])
          {
            case Map.NodeType.FirstFloor:
            case Map.NodeType.DungeonFloor:
              LevelNodeSkin2 = UIDeathScreenLevelNode.LevelNodeSkins.normal;
              break;
            case Map.NodeType.MiniBossFloor:
              LevelNodeSkin2 = UIDeathScreenLevelNode.LevelNodeSkins.boss;
              break;
            default:
              LevelNodeSkin2 = UIDeathScreenLevelNode.LevelNodeSkins.other;
              break;
          }
          component.Play((float) index * 0.5f, UIDeathScreenLevelNode.ResultTypes.Completed, LevelNodeSkin2, index, index == levels - 1);
        }
        else if (index == DataManager.Instance.dungeonVisitedRooms.Count - 1)
        {
          component.Play((float) index * 0.5f, overlayController._result == UIDeathScreenOverlayController.Results.Killed || overlayController._result == UIDeathScreenOverlayController.Results.Escaped ? UIDeathScreenLevelNode.ResultTypes.Killed : UIDeathScreenLevelNode.ResultTypes.Completed, LevelNodeSkin1, index, index == levels - 1);
          component.icon.gameObject.SetActive(overlayController._result != 0);
          overlayController.StartCoroutine(overlayController.ShowPenaltyRoutine(overlayController.inventoryItems, (float) index * 0.5f));
        }
        else
        {
          component.Play((float) index * 0.5f, UIDeathScreenLevelNode.ResultTypes.Unreached, LevelNodeSkin1, index, index == levels - 1);
          component.icon.gameObject.SetActive(false);
        }
      }
    }
    else if (overlayController._result == UIDeathScreenOverlayController.Results.GameOver)
    {
      overlayController._skullContainer.transform.localScale = Vector3.one * 1.2f;
      overlayController._skullContainer.transform.DOScale(Vector3.one, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      overlayController._skullCanvasGroup.DOFade(1f, 1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
      yield return (object) new WaitForSecondsRealtime(0.4f);
    }
    else if (PlayerFarming.Location != FollowerLocation.Boss_Wolf && PlayerFarming.Location != FollowerLocation.Boss_Yngya)
    {
      int index = -1;
      while (++index < DataManager.Instance.dungeonVisitedRooms.Count)
      {
        NodeBlueprint blueprint = MapManager.GetBlueprint(DataManager.Instance.dungeonVisitedRooms[index], MapManager.Instance.DungeonConfig);
        UIManager.PlayAudio("event:/ui/level_node_end_screen_ui_appear");
        MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(overlayController._levelNodePrefab, (Transform) overlayController._levelNodeContainer);
        gameObject.SetActive(true);
        UIDeathScreenLevelNode component = gameObject.GetComponent<UIDeathScreenLevelNode>();
        Map.NodeType dungeonVisitedRoom = DataManager.Instance.dungeonVisitedRooms[index];
        Debug.Log((object) $"type: {dungeonVisitedRoom.ToString()} blueprint: {blueprint?.ToString()}");
        if ((UnityEngine.Object) blueprint != (UnityEngine.Object) null && index < DataManager.Instance.dungeonLocationsVisited.Count)
        {
          component.icon.sprite = blueprint.GetSprite(DataManager.Instance.dungeonLocationsVisited[index], false);
          component.icon.gameObject.SetActive(true);
        }
        dungeonVisitedRoom = DataManager.Instance.dungeonVisitedRooms[index];
        UIDeathScreenLevelNode.LevelNodeSkins LevelNodeSkin;
        switch (dungeonVisitedRoom)
        {
          case Map.NodeType.FirstFloor:
          case Map.NodeType.DungeonFloor:
            LevelNodeSkin = UIDeathScreenLevelNode.LevelNodeSkins.normal;
            break;
          case Map.NodeType.MiniBossFloor:
            LevelNodeSkin = UIDeathScreenLevelNode.LevelNodeSkins.boss;
            break;
          default:
            LevelNodeSkin = UIDeathScreenLevelNode.LevelNodeSkins.other;
            break;
        }
        UIDeathScreenLevelNode.ResultTypes ResultType = UIDeathScreenLevelNode.ResultTypes.Completed;
        if ((UnityEngine.Object) blueprint != (UnityEngine.Object) null && blueprint.nodeType == Map.NodeType.MiniBossFloor && DataManager.Instance.DungeonBossFight && !DataManager.Instance.BossesCompleted.Contains(PlayerFarming.Location))
          ResultType = UIDeathScreenLevelNode.ResultTypes.Unreached;
        component.Play((float) index * 0.5f, ResultType, LevelNodeSkin, index, index == DataManager.Instance.dungeonVisitedRooms.Count - 1);
        if (index == DataManager.Instance.dungeonVisitedRooms.Count - 1)
          overlayController.StartCoroutine(overlayController.ShowPenaltyRoutine(overlayController.inventoryItems, (float) ((double) index * 0.5 + 0.5)));
      }
    }
    while (overlayController.DisplayinPenalty)
      yield return (object) null;
    overlayController._dividerIcon.color = new Color(1f, 1f, 1f, 0.0f);
    DOTweenModuleUI.DOFade(overlayController._dividerIcon, 1f, 1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    DataManager.Instance.PrayedAtCrownShrine = false;
    if (overlayController._result == UIDeathScreenOverlayController.Results.GameOver)
    {
      overlayController._timeText.text = string.Format(ScriptLocalization.UI.DayNumber, (object) DataManager.Instance.CurrentDayIndex);
      overlayController._killsText.text = DataManager.Instance.Followers.Count.ToString();
      overlayController._killsIcon.SetActive(false);
      overlayController._followersIcon.SetActive(true);
    }
    else
    {
      float num1 = Time.time - DataManager.Instance.dungeonRunDuration;
      int num2 = Mathf.FloorToInt(num1 / 60f);
      int num3 = 0;
      for (; num2 > 60; num2 -= 60)
        ++num3;
      int num4 = Mathf.FloorToInt(num1 % 60f);
      string str = "00";
      if (num3 > 0 && num3 < 10)
        str = "0" + num3.ToString();
      if (num3 >= 10)
        str = num3.ToString();
      overlayController._timeText.text = $"{str}:{(num2 < 10 ? "0" : "")}{num2.ToString()}:{(num4 < 10 ? "0" : "")}{num4.ToString()}";
      overlayController._killsText.text = DataManager.Instance.PlayerKillsOnRun.ToString();
    }
    Vector3 localPosition1 = overlayController._runtimeRectTransform.localPosition;
    RectTransform runtimeRectTransform = overlayController._runtimeRectTransform;
    runtimeRectTransform.localPosition = runtimeRectTransform.localPosition + Vector3.up * 100f;
    overlayController._runtimeRectTransform.DOLocalMove(localPosition1, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    DOTween.To(new DOGetter<float>(overlayController.\u003CDoShowAnimation\u003Eb__80_2), new DOSetter<float>(overlayController.\u003CDoShowAnimation\u003Eb__80_3), 1f, 1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    Vector3 localPosition2 = overlayController._killsRectTransform.localPosition;
    RectTransform killsRectTransform = overlayController._killsRectTransform;
    killsRectTransform.localPosition = killsRectTransform.localPosition + Vector3.up * 100f;
    overlayController._killsRectTransform.DOLocalMove(localPosition2, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    DOTween.To(new DOGetter<float>(overlayController.\u003CDoShowAnimation\u003Eb__80_4), new DOSetter<float>(overlayController.\u003CDoShowAnimation\u003Eb__80_5), 1f, 1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    Inventory.ClearDungeonItems();
    yield return (object) new WaitForSecondsRealtime(0.5f);
    if (!UIDeathScreenOverlayController.LastRunWasSandBox || !overlayController.HasGodTears)
      overlayController.ShowContinueButton();
    yield return (object) new WaitForSecondsRealtime(0.75f);
  }

  public void ShowContinueButton()
  {
    Debug.Log((object) "ShowContinueButton()");
    this.StartCoroutine(this.ShowContinueButtonRoutine());
  }

  public IEnumerator ShowContinueButtonRoutine()
  {
    UIDeathScreenOverlayController overlayController = this;
    Debug.Log((object) "ShowContinueButtonRoutine()");
    Vector3 localPosition1 = overlayController._continueCanvasGroup.transform.localPosition;
    overlayController._continueCanvasGroup.transform.localPosition += Vector3.up * 100f;
    overlayController._continueCanvasGroup.transform.DOLocalMove(localPosition1, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    DOTween.To(new DOGetter<float>(overlayController.\u003CShowContinueButtonRoutine\u003Eb__82_0), new DOSetter<float>(overlayController.\u003CShowContinueButtonRoutine\u003Eb__82_1), 1f, 1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.25f);
    Vector3 localPosition2 = overlayController._restartCanvasGroup.transform.localPosition;
    overlayController._restartCanvasGroup.transform.localPosition += Vector3.up * 100f;
    overlayController._restartCanvasGroup.transform.DOLocalMove(localPosition2, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    DOTween.To(new DOGetter<float>(overlayController.\u003CShowContinueButtonRoutine\u003Eb__82_2), new DOSetter<float>(overlayController.\u003CShowContinueButtonRoutine\u003Eb__82_3), 1f, 1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    overlayController._buttonbackgroundGroup.DOFade(1f, 1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    overlayController._controllerPrompts.DOFade(1f, 1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.25f);
    overlayController.OverrideDefault((Selectable) overlayController._continueButton);
    MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) overlayController._continueButton);
  }

  public IEnumerator ShowPenaltyRoutine(List<DeathScreenInventoryItem> inventoryItems, float Delay)
  {
    this.DisplayinPenalty = true;
    yield return (object) new WaitForSecondsRealtime(Delay);
    Debug.Log((object) ("ShowPenaltyRoutine   Inventory.itemsDungeon.Count: " + Inventory.itemsDungeon.Count.ToString()));
    if (Inventory.itemsDungeon.Count > 0)
    {
      yield return (object) new WaitForSecondsRealtime(0.5f);
      float penalty = 0.0f;
      switch (this._result)
      {
        case UIDeathScreenOverlayController.Results.Killed:
          if (DataManager.Instance.PrayedAtCrownShrine)
          {
            this._subTitle.text = ScriptLocalization.UI_DeathScreen_CrownShrine.Penalty;
            penalty = 0.0f;
            break;
          }
          if (UIDeathScreenOverlayController.LastRunWasSandBox)
          {
            penalty = 0.0f;
            break;
          }
          penalty = (float) DifficultyManager.GetDeathPeneltyPercentage();
          this._subTitle.text = string.Format(ScriptLocalization.UI_DeathScreen_Killed.Penalty, (object) penalty);
          penalty *= -1f;
          break;
        case UIDeathScreenOverlayController.Results.Completed:
          penalty = 0.0f;
          if ((double) DataManager.Instance.PlayerDamageReceivedThisRun <= 0.0)
          {
            penalty = 50f;
            this._subTitle.text = ScriptLocalization.UI_DeathScreen_NoDamage.Penalty;
            break;
          }
          if (!DataManager.Instance.TakenBossDamage && PlayerFarming.Location != FollowerLocation.IntroDungeon && !UIDeathScreenOverlayController.LastRunWasSandBox && !(bool) (UnityEngine.Object) PuzzleController.Instance)
          {
            if (DataManager.Instance.DungeonBossFight && DataManager.Instance.BossesCompleted.Contains(PlayerFarming.Location))
            {
              this._subTitle.text = ScriptLocalization.UI_DeathScreen_NoDamageCultLeader.Penalty;
              penalty = 30f;
              break;
            }
            this._subTitle.text = ScriptLocalization.UI_DeathScreen_NoDamageBoss.Penalty;
            penalty = 20f;
            break;
          }
          if (!UIDeathScreenOverlayController.LastRunWasSandBox && DataManager.Instance.DungeonBossFight && DataManager.Instance.BossesCompleted.Contains(PlayerFarming.Location))
          {
            this._subTitle.text = ScriptLocalization.UI_DeathScreen_CultLeader.Penalty;
            penalty = 20f;
            break;
          }
          break;
        case UIDeathScreenOverlayController.Results.Escaped:
          penalty = (float) DifficultyManager.GetEscapedPeneltyPercentage();
          this._subTitle.text = string.Format(ScriptLocalization.UI_DeathScreen_Escaped.Penalty, (object) penalty);
          penalty *= -1f;
          break;
      }
      this._lootDelta = new Dictionary<InventoryItem.ITEM_TYPE, int>();
      this.HandleFragileItems(this._result);
      if ((double) penalty != 0.0)
      {
        penalty += this._result == UIDeathScreenOverlayController.Results.Killed ? PlayerFleeceManager.GetLootMultiplier(this._result) : 0.0f;
        penalty = Mathf.Clamp(penalty, -100f, float.MaxValue);
        this.AddLoot(penalty, this._result);
        yield return (object) new WaitForEndOfFrame();
        if (Inventory.itemsDungeon.Count > 0)
        {
          foreach (DeathScreenInventoryItem inventoryItem in inventoryItems)
          {
            if (UIDeathScreenOverlayController._fragileLoot.Contains(inventoryItem.Type))
              this.SetFragileLootDisplay(inventoryItem);
            else
              this.SetLootPenaltyDisplay(inventoryItem, penalty);
          }
        }
        yield return (object) new WaitForSecondsRealtime(0.5f);
      }
      else if ((this._result != UIDeathScreenOverlayController.Results.Killed && this._result != UIDeathScreenOverlayController.Results.Escaped || this._lootDelta.Count <= 0) && this._lootDelta.Count > 0)
      {
        yield return (object) new WaitForEndOfFrame();
        foreach (DeathScreenInventoryItem inventoryItem in inventoryItems)
        {
          if (UIDeathScreenOverlayController._fragileLoot.Contains(inventoryItem.Type))
            this.SetFragileLootDisplay(inventoryItem);
        }
        yield return (object) new WaitForSecondsRealtime(0.5f);
      }
    }
    this.DisplayinPenalty = false;
  }

  public void SetLootPenaltyDisplay(DeathScreenInventoryItem inventoryItem, float penalty)
  {
    if (!UIDeathScreenOverlayController._excludeLootFromBonus.Contains(inventoryItem.Type))
    {
      Color color1 = inventoryItem.AmountText.color;
      Color color2 = inventoryItem.AmountText.color;
      color1.a = 0.0f;
      inventoryItem.AmountText.color = color1;
      ShortcutExtensionsTMPText.DOColor(inventoryItem.AmountText, color2, 1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    }
    inventoryItem.AmountText.text = Inventory.GetDungeonItemByType((int) inventoryItem.Type).quantity.ToString();
    if (!UIDeathScreenOverlayController._excludeLootFromBonus.Contains(inventoryItem.Type))
    {
      if (!this._lootDelta.ContainsKey(inventoryItem.Type) || this._lootDelta[inventoryItem.Type] == 0)
        return;
      inventoryItem.ShowDelta(this._lootDelta[inventoryItem.Type]);
      inventoryItem.DeltaText.transform.localScale = Vector3.one * 2f;
      inventoryItem.DeltaText.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    }
    if ((double) penalty < 0.0)
    {
      if (UIDeathScreenOverlayController._excludeLootFromBonus.Contains(inventoryItem.Type))
        return;
      inventoryItem.RectTransform.DOShakePosition(1f + UnityEngine.Random.Range(0.0f, 0.2f), new Vector3(10f, 0.0f)).SetUpdate<Tweener>(true);
    }
    else
    {
      Vector3 localScale = inventoryItem.RectTransform.localScale;
      inventoryItem.RectTransform.transform.localScale = localScale * 1.4f;
      inventoryItem.RectTransform.transform.DOScale(localScale, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    }
  }

  public void SetFragileLootDisplay(DeathScreenInventoryItem inventoryItem)
  {
    if (!UIDeathScreenOverlayController._fragileLoot.Contains(inventoryItem.Type) || !this._lootDelta.ContainsKey(inventoryItem.Type))
      return;
    Color color1 = inventoryItem.AmountText.color;
    Color color2 = inventoryItem.AmountText.color;
    color1.a = 0.0f;
    inventoryItem.AmountText.color = color1;
    ShortcutExtensionsTMPText.DOColor(inventoryItem.AmountText, color2, 1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    inventoryItem.AmountText.text = Inventory.GetDungeonItemByType((int) inventoryItem.Type).quantity.ToString();
    if (this._lootDelta[inventoryItem.Type] == 0)
      return;
    inventoryItem.ShowDelta(this._lootDelta[inventoryItem.Type]);
    inventoryItem.DeltaText.transform.localScale = Vector3.one * 2f;
    inventoryItem.DeltaText.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    inventoryItem.RectTransform.DOShakePosition(1f + UnityEngine.Random.Range(0.0f, 0.2f), new Vector3(10f, 0.0f)).SetUpdate<Tweener>(true);
  }

  public void AddLoot(float penalty, UIDeathScreenOverlayController.Results result)
  {
    penalty /= 100f;
    List<InventoryItem.ITEM_TYPE> itemTypeList = new List<InventoryItem.ITEM_TYPE>();
    List<ObjectivesData> objectivesDataList = new List<ObjectivesData>();
    objectivesDataList.AddRange((IEnumerable<ObjectivesData>) DataManager.Instance.Objectives);
    objectivesDataList.AddRange((IEnumerable<ObjectivesData>) DataManager.Instance.CompletedObjectives);
    foreach (ObjectivesData objectivesData in objectivesDataList)
    {
      if (objectivesData is Objectives_CollectItem)
        itemTypeList.Add(((Objectives_CollectItem) objectivesData).ItemType);
      else if (objectivesData is Objectives_BlizzardOffering)
        itemTypeList.Add(((Objectives_BlizzardOffering) objectivesData).TargetType);
    }
    foreach (InventoryItem inventoryItem in Inventory.itemsDungeon)
    {
      if (!UIDeathScreenOverlayController._excludeLootFromBonus.Contains((InventoryItem.ITEM_TYPE) inventoryItem.type) && (result != UIDeathScreenOverlayController.Results.Killed && this._result != UIDeathScreenOverlayController.Results.Escaped || !UIDeathScreenOverlayController._fragileLoot.Contains((InventoryItem.ITEM_TYPE) inventoryItem.type)))
      {
        int num1 = Mathf.RoundToInt((float) inventoryItem.quantity * penalty);
        if (itemTypeList.Contains((InventoryItem.ITEM_TYPE) inventoryItem.type) && (double) penalty <= 0.0)
          num1 = 0;
        if ((double) penalty > 0.0 && num1 < 1)
          num1 = 1;
        this._lootDelta.Add((InventoryItem.ITEM_TYPE) inventoryItem.type, num1);
        int num2 = inventoryItem.quantity + num1;
        Inventory.GetDungeonItemByType(inventoryItem.type).quantity = num2;
        Inventory.SetItemQuantity(inventoryItem.type, Inventory.GetItemQuantity(inventoryItem.type) + num1);
      }
    }
  }

  public void HandleFragileItems(UIDeathScreenOverlayController.Results result)
  {
    if (result != UIDeathScreenOverlayController.Results.Escaped && result != UIDeathScreenOverlayController.Results.Killed)
      return;
    foreach (InventoryItem inventoryItem in Inventory.itemsDungeon)
    {
      if (UIDeathScreenOverlayController._fragileLoot.Contains((InventoryItem.ITEM_TYPE) inventoryItem.type))
      {
        this._lootDelta.Add((InventoryItem.ITEM_TYPE) inventoryItem.type, -inventoryItem.quantity);
        Inventory.SetItemQuantity(inventoryItem.type, Inventory.GetItemQuantity(inventoryItem.type) - inventoryItem.quantity);
        inventoryItem.quantity = 0;
      }
    }
  }

  public QuoteScreenController.QuoteTypes GetQuoteType()
  {
    switch (PlayerFarming.Location)
    {
      case FollowerLocation.Dungeon1_1:
        return QuoteScreenController.QuoteTypes.QuoteBoss6;
      case FollowerLocation.Dungeon1_2:
        return QuoteScreenController.QuoteTypes.QuoteBoss7;
      case FollowerLocation.Dungeon1_3:
        return QuoteScreenController.QuoteTypes.QuoteBoss8;
      case FollowerLocation.Dungeon1_4:
        return QuoteScreenController.QuoteTypes.QuoteBoss9;
      default:
        return QuoteScreenController.QuoteTypes.QuoteBoss6;
    }
  }

  public override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

  public override void OnDestroy()
  {
    base.OnDestroy();
    UIDeathScreenOverlayController.Instance = (UIDeathScreenOverlayController) null;
  }

  public void Update() => Time.timeScale = 0.0f;

  [CompilerGenerated]
  public void \u003CUpdateSandboxXpBarRoutine\u003Eb__69_0() => this.StartCoroutine(this.CheckXP());

  [CompilerGenerated]
  public void \u003COnShowStarted\u003Eb__76_0() => this.Hide();

  [CompilerGenerated]
  public void \u003CDoHide\u003Eb__77_0() => this.Hide(true);

  [CompilerGenerated]
  public void \u003CDoHide\u003Eb__77_1() => this.Hide(true);

  [CompilerGenerated]
  public float \u003CDoShowAnimation\u003Eb__80_0() => this._backgroundGroup.alpha;

  [CompilerGenerated]
  public void \u003CDoShowAnimation\u003Eb__80_1(float x) => this._backgroundGroup.alpha = x;

  [CompilerGenerated]
  public float \u003CDoShowAnimation\u003Eb__80_2() => this._runtimeCanvasGroup.alpha;

  [CompilerGenerated]
  public void \u003CDoShowAnimation\u003Eb__80_3(float x) => this._runtimeCanvasGroup.alpha = x;

  [CompilerGenerated]
  public float \u003CDoShowAnimation\u003Eb__80_4() => this._killsCanvasGroup.alpha;

  [CompilerGenerated]
  public void \u003CDoShowAnimation\u003Eb__80_5(float x) => this._killsCanvasGroup.alpha = x;

  [CompilerGenerated]
  public float \u003CShowContinueButtonRoutine\u003Eb__82_0() => this._continueCanvasGroup.alpha;

  [CompilerGenerated]
  public void \u003CShowContinueButtonRoutine\u003Eb__82_1(float x)
  {
    this._continueCanvasGroup.alpha = x;
  }

  [CompilerGenerated]
  public float \u003CShowContinueButtonRoutine\u003Eb__82_2() => this._restartCanvasGroup.alpha;

  [CompilerGenerated]
  public void \u003CShowContinueButtonRoutine\u003Eb__82_3(float x)
  {
    this._restartCanvasGroup.alpha = x;
  }

  public enum Results
  {
    Killed,
    Completed,
    Escaped,
    None,
    BeatenMiniBoss,
    BeatenBoss,
    GameOver,
    BeatenBossNoDamage,
    AwokenMountain,
  }
}
