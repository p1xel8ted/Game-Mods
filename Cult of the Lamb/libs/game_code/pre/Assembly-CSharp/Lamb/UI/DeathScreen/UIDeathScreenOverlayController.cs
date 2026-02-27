// Decompiled with JetBrains decompiler
// Type: Lamb.UI.DeathScreen.UIDeathScreenOverlayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Coffee.UIExtensions;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Map;
using MMTools;
using src.Extensions;
using System.Collections;
using System.Collections.Generic;
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
  [Header("Selectables")]
  [SerializeField]
  private MMButton _continueButton;
  [SerializeField]
  private MMButton _restartButton;
  [SerializeField]
  private GameObject _buttonHighlight;
  [Header("Text")]
  [SerializeField]
  private TextMeshProUGUI _title;
  [SerializeField]
  private TextMeshProUGUI _subTitle;
  [SerializeField]
  private TextMeshProUGUI _timeText;
  [SerializeField]
  private TextMeshProUGUI _killsText;
  [Header("Containers")]
  [SerializeField]
  private RectTransform _root;
  [SerializeField]
  private RectTransform _titleContainer;
  [SerializeField]
  private RectTransform _levelNodeContainer;
  [SerializeField]
  private RectTransform _itemsContainer;
  [Header("Prefabs")]
  [SerializeField]
  private GameObject _levelNodePrefab;
  [SerializeField]
  private DeathScreenInventoryItem _itemTemplate;
  [Header("Canvas Groups")]
  [SerializeField]
  private CanvasGroup _runtimeCanvasGroup;
  [SerializeField]
  private RectTransform _runtimeRectTransform;
  [SerializeField]
  private CanvasGroup _killsCanvasGroup;
  [SerializeField]
  private RectTransform _killsRectTransform;
  [SerializeField]
  private CanvasGroup _continueCanvasGroup;
  [SerializeField]
  private CanvasGroup _restartCanvasGroup;
  [SerializeField]
  private CanvasGroup _backgroundGroup;
  [SerializeField]
  private CanvasGroup _particleGroup;
  [SerializeField]
  private CanvasGroup _buttonbackgroundGroup;
  [SerializeField]
  private CanvasGroup _controllerPrompts;
  [SerializeField]
  private CanvasGroup _titleGroup;
  [SerializeField]
  private CanvasGroup _itemCanvasGroup;
  [SerializeField]
  private Image _dividerIcon;
  [Header("Effects")]
  [SerializeField]
  private UIParticle _uiParticle;
  [SerializeField]
  private Material _diedMaterial;
  [SerializeField]
  private Material _completedMaterial;
  [Header("Other")]
  [SerializeField]
  private UIMenuControlPrompts _controlPrompts;
  private List<InventoryItem.ITEM_TYPE> _blacklistedItems = new List<InventoryItem.ITEM_TYPE>()
  {
    InventoryItem.ITEM_TYPE.SEEDS,
    InventoryItem.ITEM_TYPE.INGREDIENTS,
    InventoryItem.ITEM_TYPE.MEALS,
    InventoryItem.ITEM_TYPE.GIFT_MEDIUM,
    InventoryItem.ITEM_TYPE.GIFT_SMALL,
    InventoryItem.ITEM_TYPE.MONSTER_HEART
  };
  private List<InventoryItem.ITEM_TYPE> _excludeLootFromBonus = new List<InventoryItem.ITEM_TYPE>()
  {
    InventoryItem.ITEM_TYPE.Necklace_1,
    InventoryItem.ITEM_TYPE.Necklace_2,
    InventoryItem.ITEM_TYPE.Necklace_3,
    InventoryItem.ITEM_TYPE.Necklace_4,
    InventoryItem.ITEM_TYPE.Necklace_5,
    InventoryItem.ITEM_TYPE.GIFT_MEDIUM,
    InventoryItem.ITEM_TYPE.GIFT_SMALL,
    InventoryItem.ITEM_TYPE.MONSTER_HEART,
    InventoryItem.ITEM_TYPE.BEHOLDER_EYE,
    InventoryItem.ITEM_TYPE.SHELL
  };
  private Dictionary<InventoryItem.ITEM_TYPE, int> _lootDelta;
  private UIDeathScreenOverlayController.Results _result;
  private int _levels;
  private bool beatBoss;
  private bool DisplayinPenalty;

  public void Show(UIDeathScreenOverlayController.Results result, bool instant = false)
  {
    this.Show(result, (UnityEngine.Object) MapManager.Instance != (UnityEngine.Object) null ? ((bool) (UnityEngine.Object) MapManager.Instance.DungeonConfig ? MapManager.Instance.DungeonConfig.layers.Count : 1) : 1, instant);
  }

  public void Show(UIDeathScreenOverlayController.Results result, int levels, bool instant = false)
  {
    UIDeathScreenOverlayController.Instance = this;
    this._result = result;
    this._levels = levels;
    this._continueButton.interactable = this._continueButton.enabled = false;
    this._buttonHighlight.SetActive(false);
    this._titleGroup.alpha = 0.0f;
    this._subTitle.alpha = 0.0f;
    this._particleGroup.alpha = 0.0f;
    this._backgroundGroup.alpha = 0.0f;
    this._runtimeCanvasGroup.alpha = 0.0f;
    this._killsCanvasGroup.alpha = 0.0f;
    this._continueCanvasGroup.alpha = 0.0f;
    this._restartCanvasGroup.alpha = 0.0f;
    this._buttonbackgroundGroup.alpha = 0.0f;
    this._controllerPrompts.alpha = 0.0f;
    this._itemCanvasGroup.alpha = 0.0f;
    this._dividerIcon.enabled = false;
    Time.timeScale = 0.0f;
    UIDeathScreenOverlayController.Result = result;
    if ((bool) (UnityEngine.Object) this._controlPrompts)
      this._controlPrompts.HideAcceptButton();
    this.Show(instant);
  }

  protected override void OnShowStarted()
  {
    TwitchHelpHinder.EndHHEvent((TwitchHelpHinder.HHData) null);
    SimulationManager.UnPause();
    DataManager.ResetRunData();
    this._continueButton.onClick.AddListener((UnityAction) (() => this.Hide()));
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
        this._uiParticle.material = this._diedMaterial;
        break;
    }
    DataManager.ResetRunData();
    if (UIDeathScreenOverlayController.Result == UIDeathScreenOverlayController.Results.Killed)
    {
      DataManager.Instance.LastRunResults = UIDeathScreenOverlayController.Results.Killed;
      DataManager.Instance.DiedLastRun = true;
    }
    else
      DataManager.Instance.DiedLastRun = false;
    if (this._result != UIDeathScreenOverlayController.Results.Escaped)
    {
      for (int index = 0; index < DataManager.Instance.Followers_Demons_IDs.Count; ++index)
      {
        FollowerInfo infoById = FollowerInfo.GetInfoByID(DataManager.Instance.Followers_Demons_IDs[index]);
        if (infoById != null)
          FollowerBrain.GetOrCreateBrain(infoById)?.AddThought(this._result == UIDeathScreenOverlayController.Results.Completed ? Thought.DemonSuccessfulRun : Thought.DemonFailedRun);
      }
    }
    else if (this._result == UIDeathScreenOverlayController.Results.Completed)
    {
      DataManager.Instance.playerDeathsInARow = 0;
      DataManager.Instance.playerDeathsInARowFightingLeader = 0;
    }
    DataManager.Instance.Followers_Demons_IDs.Clear();
    DataManager.Instance.Followers_Demons_Types.Clear();
  }

  protected override IEnumerator DoHide()
  {
    UIDeathScreenOverlayController overlayController = this;
    if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
      PlayerFarming.Instance.playerController.enabled = false;
    Debug.Log((object) ("typE: " + (object) DeathCatRoomManager.GetConversationType()));
    if (CheatConsole.IN_DEMO && PlayerFarming.Location == FollowerLocation.Dungeon1_1)
    {
      // ISSUE: reference to a compiler-generated method
      MMTransition.Play(MMTransition.TransitionType.ChangeSceneAutoResume, MMTransition.Effect.BlackFade, "DemoOver", 1f, "", new System.Action(overlayController.\u003CDoHide\u003Eb__42_0));
    }
    else if (overlayController._result == UIDeathScreenOverlayController.Results.GameOver)
    {
      SimulationManager.Pause();
      KeyboardLightingManager.Reset();
      FollowerManager.Reset();
      StructureManager.Reset();
      SaveAndLoad.DeleteSaveSlot(SaveAndLoad.SAVE_SLOT);
      TwitchManager.Abort();
      UIDynamicNotificationCenter.Reset();
      GameManager.GoG_Initialised = false;
      // ISSUE: reference to a compiler-generated method
      MMTransition.Play(MMTransition.TransitionType.ChangeSceneAutoResume, MMTransition.Effect.BlackFade, "Main Menu", 1f, "", new System.Action(overlayController.\u003CDoHide\u003Eb__42_1));
    }
    else if (DeathCatRoomManager.GetConversationType() != DeathCatRoomManager.ConversationTypes.None && (UnityEngine.Object) DeathCatController.Instance == (UnityEngine.Object) null)
    {
      if ((UnityEngine.Object) RespawnRoomManager.Instance != (UnityEngine.Object) null)
        RespawnRoomManager.Instance.gameObject.SetActive(false);
      DeathCatRoomManager.Play();
      DG.Tweening.Sequence sequence = DOTween.Sequence();
      sequence.AppendInterval(0.5f).SetUpdate<DG.Tweening.Sequence>(true);
      sequence.Play<DG.Tweening.Sequence>().SetUpdate<DG.Tweening.Sequence>(true);
    }
    else
      GameManager.ToShip();
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

  protected override void OnShowCompleted()
  {
    this._continueButton.interactable = this._continueButton.enabled = true;
    this._buttonHighlight.SetActive(true);
    this.ActivateNavigation();
    this._controlPrompts.ShowAcceptButton();
  }

  protected override IEnumerator DoShowAnimation()
  {
    UIDeathScreenOverlayController overlayController = this;
    if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
    {
      HealthPlayer component = PlayerFarming.Instance.GetComponent<HealthPlayer>();
      component.HP = component.totalHP;
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
        UIManager.PlayAudio("event:/ui/heretics_defeated");
        break;
      case UIDeathScreenOverlayController.Results.Escaped:
        UIManager.PlayAudio("event:/ui/martyred");
        break;
    }
    if (Inventory.itemsDungeon.Count == 0)
      overlayController._itemsContainer.gameObject.SetActive(false);
    Vector2 titleOrigin = (Vector2) overlayController._title.rectTransform.localPosition;
    overlayController._title.rectTransform.SetParent((Transform) overlayController._root);
    overlayController._title.rectTransform.localPosition = Vector3.zero;
    overlayController._title.rectTransform.localScale = Vector3.one * 4f;
    overlayController._title.rectTransform.DOScale(Vector3.one * 2f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    overlayController._titleGroup.DOFade(1f, 0.5f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    // ISSUE: reference to a compiler-generated method
    // ISSUE: reference to a compiler-generated method
    DOTween.To(new DOGetter<float>(overlayController.\u003CDoShowAnimation\u003Eb__44_0), new DOSetter<float>(overlayController.\u003CDoShowAnimation\u003Eb__44_1), 1f, 2f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(1f);
    overlayController._particleGroup.DOFade(1f, 2f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InQuart);
    if ((UnityEngine.Object) HUD_Manager.Instance != (UnityEngine.Object) null)
      HUD_Manager.Instance.ShowBW(0.5f, 1f, 0.0f);
    overlayController._title.rectTransform.SetParent((Transform) overlayController._titleContainer, true);
    overlayController._title.rectTransform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    overlayController._title.rectTransform.DOLocalMove((Vector3) titleOrigin, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.5f);
    ShortcutExtensionsTMPText.DOFade(overlayController._subTitle, 1f, 0.5f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.OutSine).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    overlayController._titleGroup.DOFade(1f, 0.5f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    overlayController._title.rectTransform.DOShakeScale(0.5f, 0.2f).SetUpdate<Tweener>(true);
    List<DeathScreenInventoryItem> inventoryItems = new List<DeathScreenInventoryItem>();
    if (Inventory.itemsDungeon.Count > 0)
    {
      foreach (InventoryItem inventoryItem in Inventory.itemsDungeon)
      {
        if (!overlayController._blacklistedItems.Contains((InventoryItem.ITEM_TYPE) inventoryItem.type))
        {
          DeathScreenInventoryItem screenInventoryItem = overlayController._itemTemplate.Instantiate<DeathScreenInventoryItem>((Transform) overlayController._itemsContainer);
          screenInventoryItem.Configure(inventoryItem, true);
          screenInventoryItem.CanvasGroup.alpha = 0.0f;
          inventoryItems.Add(screenInventoryItem);
        }
      }
      overlayController._itemCanvasGroup.DOFade(1f, 0.3f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
      yield return (object) new WaitForSecondsRealtime(0.5f);
      foreach (UIInventoryItem uiInventoryItem in inventoryItems)
      {
        uiInventoryItem.CanvasGroup.DOFade(1f, 2f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
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
          Debug.Log((object) $"type: {DataManager.Instance.dungeonVisitedRooms[index].ToString()} blueprint: {(object) blueprint}");
          if ((UnityEngine.Object) blueprint != (UnityEngine.Object) null)
          {
            component.icon.sprite = blueprint.GetSprite(PlayerFarming.Location, false);
            component.icon.gameObject.SetActive(true);
          }
          UIDeathScreenLevelNode.LevelNodeSkins LevelNodeSkin2;
          switch (DataManager.Instance.dungeonVisitedRooms[index])
          {
            case NodeType.FirstFloor:
            case NodeType.DungeonFloor:
              LevelNodeSkin2 = UIDeathScreenLevelNode.LevelNodeSkins.normal;
              break;
            case NodeType.MiniBossFloor:
              LevelNodeSkin2 = UIDeathScreenLevelNode.LevelNodeSkins.boss;
              break;
            default:
              LevelNodeSkin2 = UIDeathScreenLevelNode.LevelNodeSkins.other;
              break;
          }
          component.Play((float) index * 0.5f, UIDeathScreenLevelNode.ResultTypes.Completed, LevelNodeSkin2, index);
        }
        else if (index == DataManager.Instance.dungeonVisitedRooms.Count - 1)
        {
          component.Play((float) index * 0.5f, overlayController._result == UIDeathScreenOverlayController.Results.Killed || overlayController._result == UIDeathScreenOverlayController.Results.Escaped ? UIDeathScreenLevelNode.ResultTypes.Killed : UIDeathScreenLevelNode.ResultTypes.Completed, LevelNodeSkin1, index);
          component.icon.gameObject.SetActive(overlayController._result != 0);
          overlayController.StartCoroutine((IEnumerator) overlayController.ShowPenaltyRoutine(inventoryItems, (float) index * 0.5f));
        }
        else
        {
          component.Play((float) index * 0.5f, UIDeathScreenLevelNode.ResultTypes.Unreached, LevelNodeSkin1, index);
          component.icon.gameObject.SetActive(false);
        }
      }
    }
    else if (overlayController._result != UIDeathScreenOverlayController.Results.GameOver)
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
        object[] objArray = new object[4]
        {
          (object) "type: ",
          null,
          null,
          null
        };
        NodeType dungeonVisitedRoom = DataManager.Instance.dungeonVisitedRooms[index];
        objArray[1] = (object) dungeonVisitedRoom.ToString();
        objArray[2] = (object) " blueprint: ";
        objArray[3] = (object) blueprint;
        Debug.Log((object) string.Concat(objArray));
        if ((UnityEngine.Object) blueprint != (UnityEngine.Object) null)
        {
          component.icon.sprite = blueprint.GetSprite(PlayerFarming.Location, false);
          component.icon.gameObject.SetActive(true);
        }
        dungeonVisitedRoom = DataManager.Instance.dungeonVisitedRooms[index];
        UIDeathScreenLevelNode.LevelNodeSkins LevelNodeSkin;
        switch (dungeonVisitedRoom)
        {
          case NodeType.FirstFloor:
          case NodeType.DungeonFloor:
            LevelNodeSkin = UIDeathScreenLevelNode.LevelNodeSkins.normal;
            break;
          case NodeType.MiniBossFloor:
            LevelNodeSkin = UIDeathScreenLevelNode.LevelNodeSkins.boss;
            break;
          default:
            LevelNodeSkin = UIDeathScreenLevelNode.LevelNodeSkins.other;
            break;
        }
        UIDeathScreenLevelNode.ResultTypes ResultType = UIDeathScreenLevelNode.ResultTypes.Completed;
        if (blueprint.nodeType == NodeType.MiniBossFloor && DataManager.Instance.DungeonBossFight && !DataManager.Instance.BossesCompleted.Contains(PlayerFarming.Location))
          ResultType = UIDeathScreenLevelNode.ResultTypes.Unreached;
        component.Play((float) index * 0.5f, ResultType, LevelNodeSkin, index);
        if (index == DataManager.Instance.dungeonVisitedRooms.Count - 1)
          overlayController.StartCoroutine((IEnumerator) overlayController.ShowPenaltyRoutine(inventoryItems, (float) ((double) index * 0.5 + 0.5)));
      }
    }
    while (overlayController.DisplayinPenalty)
      yield return (object) null;
    overlayController._dividerIcon.color = new Color(1f, 1f, 1f, 0.0f);
    DOTweenModuleUI.DOFade(overlayController._dividerIcon, 1f, 1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    DataManager.Instance.PrayedAtCrownShrine = false;
    float num1 = Time.time - DataManager.Instance.dungeonRunDuration;
    if (overlayController._result == UIDeathScreenOverlayController.Results.GameOver)
      num1 = DataManager.Instance.TimeInGame;
    int _time = Mathf.FloorToInt(num1 / 60f);
    int num2 = 0;
    for (; _time > 60; _time -= 60)
      ++num2;
    int num3 = Mathf.FloorToInt(num1 % 60f);
    string str = "00";
    if (num2 > 0 && num2 < 10)
      str = "0" + (object) num2;
    if (num2 >= 10)
      str = num2.ToString();
    overlayController._timeText.text = $"{str}:{(_time < 10 ? (object) "0" : (object) "")}{(object) _time}:{(num3 < 10 ? (object) "0" : (object) "")}{(object) num3}";
    Vector3 localPosition1 = overlayController._runtimeRectTransform.localPosition;
    RectTransform runtimeRectTransform = overlayController._runtimeRectTransform;
    runtimeRectTransform.localPosition = runtimeRectTransform.localPosition + Vector3.up * 100f;
    overlayController._runtimeRectTransform.DOLocalMove(localPosition1, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    // ISSUE: reference to a compiler-generated method
    // ISSUE: reference to a compiler-generated method
    DOTween.To(new DOGetter<float>(overlayController.\u003CDoShowAnimation\u003Eb__44_2), new DOSetter<float>(overlayController.\u003CDoShowAnimation\u003Eb__44_3), 1f, 1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    overlayController._killsText.text = DataManager.Instance.PlayerKillsOnRun.ToString();
    if (overlayController._result == UIDeathScreenOverlayController.Results.GameOver)
    {
      float killsInGame = (float) DataManager.Instance.KillsInGame;
    }
    Vector3 localPosition2 = overlayController._killsRectTransform.localPosition;
    RectTransform killsRectTransform = overlayController._killsRectTransform;
    killsRectTransform.localPosition = killsRectTransform.localPosition + Vector3.up * 100f;
    overlayController._killsRectTransform.DOLocalMove(localPosition2, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    // ISSUE: reference to a compiler-generated method
    // ISSUE: reference to a compiler-generated method
    DOTween.To(new DOGetter<float>(overlayController.\u003CDoShowAnimation\u003Eb__44_4), new DOSetter<float>(overlayController.\u003CDoShowAnimation\u003Eb__44_5), 1f, 1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    int quantity1 = Inventory.GetDungeonItemByTypeReturnObject(1).quantity;
    int quantity2 = Inventory.GetDungeonItemByTypeReturnObject(20).quantity;
    int quantity3 = Inventory.GetDungeonItemByTypeReturnObject(21).quantity;
    int quantity4 = Inventory.GetDungeonItemByTypeReturnObject(50).quantity;
    int quantity5 = Inventory.GetDungeonItemByTypeReturnObject(97).quantity;
    int quantity6 = Inventory.GetDungeonItemByTypeReturnObject(102).quantity;
    int quantity7 = Inventory.GetDungeonItemByTypeReturnObject(6).quantity;
    int num4 = quantity4;
    int _foodCollected = quantity3 + num4 + quantity5 + quantity6 + quantity7;
    if (overlayController._result != UIDeathScreenOverlayController.Results.GameOver && (bool) (UnityEngine.Object) MonoSingleton<PlayerProgress_Analytics>.Instance)
      MonoSingleton<PlayerProgress_Analytics>.Instance.LevelComplete(GameManager.CurrentDungeonLayer, DataManager.Instance.GetDungeonLayer(PlayerFarming.Location), quantity1, quantity2, _foodCollected, DataManager.Instance.PlayerKillsOnRun, _time, (int) DataManager.Instance.PlayerDamageReceivedThisRun, overlayController.beatBoss);
    Inventory.ClearDungeonItems();
    yield return (object) new WaitForSecondsRealtime(0.5f);
    Vector3 localPosition3 = overlayController._continueCanvasGroup.transform.localPosition;
    overlayController._continueCanvasGroup.transform.localPosition += Vector3.up * 100f;
    overlayController._continueCanvasGroup.transform.DOLocalMove(localPosition3, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    // ISSUE: reference to a compiler-generated method
    // ISSUE: reference to a compiler-generated method
    DOTween.To(new DOGetter<float>(overlayController.\u003CDoShowAnimation\u003Eb__44_6), new DOSetter<float>(overlayController.\u003CDoShowAnimation\u003Eb__44_7), 1f, 1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.25f);
    Vector3 localPosition4 = overlayController._restartCanvasGroup.transform.localPosition;
    overlayController._restartCanvasGroup.transform.localPosition += Vector3.up * 100f;
    overlayController._restartCanvasGroup.transform.DOLocalMove(localPosition4, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    // ISSUE: reference to a compiler-generated method
    // ISSUE: reference to a compiler-generated method
    DOTween.To(new DOGetter<float>(overlayController.\u003CDoShowAnimation\u003Eb__44_8), new DOSetter<float>(overlayController.\u003CDoShowAnimation\u003Eb__44_9), 1f, 1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    overlayController._buttonbackgroundGroup.DOFade(1f, 1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    overlayController._controllerPrompts.DOFade(1f, 1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.75f);
  }

  private IEnumerator ShowPenaltyRoutine(List<DeathScreenInventoryItem> inventoryItems, float Delay)
  {
    this.DisplayinPenalty = true;
    yield return (object) new WaitForSecondsRealtime(Delay);
    Debug.Log((object) ("ShowPenaltyRoutine   Inventory.itemsDungeon.Count: " + (object) Inventory.itemsDungeon.Count));
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
          if (!DataManager.Instance.TakenBossDamage && PlayerFarming.Location != FollowerLocation.IntroDungeon)
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
          if (DataManager.Instance.DungeonBossFight && DataManager.Instance.BossesCompleted.Contains(PlayerFarming.Location))
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
      if ((double) penalty != 0.0)
      {
        penalty += this._result == UIDeathScreenOverlayController.Results.Killed ? PlayerFleeceManager.GetLootMultiplier(this._result) : 0.0f;
        penalty = Mathf.Clamp(penalty, -100f, float.MaxValue);
        if (DataManager.Instance.PlayerFleece == 3 && this._result != UIDeathScreenOverlayController.Results.Completed && this._result != UIDeathScreenOverlayController.Results.Escaped)
          this._subTitle.text = LocalizationManager.GetTranslation("UI/DeathScreen/Killed/Fleece3/Penalty");
        this.AddLoot(penalty);
        yield return (object) new WaitForEndOfFrame();
        if (Inventory.itemsDungeon.Count > 0)
        {
          foreach (DeathScreenInventoryItem inventoryItem in inventoryItems)
          {
            Color color1 = inventoryItem.AmountText.color;
            Color color2 = inventoryItem.AmountText.color;
            color1.a = 0.0f;
            inventoryItem.AmountText.color = color1;
            ShortcutExtensionsTMPText.DOColor(inventoryItem.AmountText, color2, 1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
            inventoryItem.AmountText.text = Inventory.GetDungeonItemByType((int) inventoryItem.Type).quantity.ToString();
            if (!this._excludeLootFromBonus.Contains(inventoryItem.Type))
            {
              if (this._lootDelta[inventoryItem.Type] != 0)
              {
                inventoryItem.ShowDelta(this._lootDelta[inventoryItem.Type]);
                inventoryItem.DeltaText.transform.localScale = Vector3.one * 2f;
                inventoryItem.DeltaText.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
              }
              else
                continue;
            }
            if ((double) penalty < 0.0)
            {
              inventoryItem.RectTransform.DOShakePosition(1f + UnityEngine.Random.Range(0.0f, 0.2f), new Vector3(10f, 0.0f)).SetUpdate<Tweener>(true);
            }
            else
            {
              Vector3 localScale = inventoryItem.RectTransform.localScale;
              inventoryItem.RectTransform.transform.localScale = localScale * 1.4f;
              inventoryItem.RectTransform.transform.DOScale(localScale, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
            }
          }
        }
        yield return (object) new WaitForSecondsRealtime(0.5f);
      }
    }
    this.DisplayinPenalty = false;
  }

  private void AddLoot(float penalty)
  {
    this._lootDelta = new Dictionary<InventoryItem.ITEM_TYPE, int>();
    penalty /= 100f;
    List<InventoryItem.ITEM_TYPE> itemTypeList = new List<InventoryItem.ITEM_TYPE>();
    List<ObjectivesData> objectivesDataList = new List<ObjectivesData>();
    objectivesDataList.AddRange((IEnumerable<ObjectivesData>) DataManager.Instance.Objectives);
    objectivesDataList.AddRange((IEnumerable<ObjectivesData>) DataManager.Instance.CompletedObjectives);
    foreach (ObjectivesData objectivesData in objectivesDataList)
    {
      if (objectivesData is Objectives_CollectItem)
        itemTypeList.Add(((Objectives_CollectItem) objectivesData).ItemType);
    }
    foreach (InventoryItem inventoryItem in Inventory.itemsDungeon)
    {
      if (!this._excludeLootFromBonus.Contains((InventoryItem.ITEM_TYPE) inventoryItem.type))
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

  protected override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

  protected override void OnDestroy()
  {
    base.OnDestroy();
    UIDeathScreenOverlayController.Instance = (UIDeathScreenOverlayController) null;
  }

  private void Update() => Time.timeScale = 0.0f;

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
  }
}
