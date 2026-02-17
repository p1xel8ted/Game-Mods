// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Menus.PlayerMenu.CharacterMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI.PauseDetails;
using MMBiomeGeneration;
using Spine;
using Spine.Unity;
using src.Extensions;
using src.UI.Items;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI.Menus.PlayerMenu;

public class CharacterMenu : UISubmenuBase
{
  [Header("Character Menu")]
  [SerializeField]
  public MMScrollRect _scrollRect;
  [SerializeField]
  public SkeletonGraphic _skeletonGraphic;
  [SerializeField]
  public HUD_Hearts _heartsContainer;
  [SerializeField]
  public TMP_Text _titleText;
  [Header("Items")]
  [SerializeField]
  public WeaponItem _weaponItem;
  [SerializeField]
  public CurseItem _curseItem;
  [SerializeField]
  public RelicPlayerMenuItem _relicItem;
  [SerializeField]
  public FleeceItem _fleeceItem;
  [SerializeField]
  public TalismanPiecesItem _talismanPiecesItem;
  [SerializeField]
  public DoctrineFragmentsItem _doctrineFragmentsItem;
  [SerializeField]
  public CrownAbilityItem _crownAbilityItem1;
  [SerializeField]
  public CrownAbilityItem _crownAbilityItem2;
  [SerializeField]
  public CrownAbilityItem _crownAbilityItem3;
  [SerializeField]
  public CrownAbilityItem _crownAbilityItem4;
  [Header("Tarot")]
  [SerializeField]
  public RectTransform _tarotCardContentContainer;
  [SerializeField]
  public TarotCardItem_Run _tarotCardItemRunTemplate;
  [SerializeField]
  public GameObject _noTarotText;
  [Header("Relics")]
  [SerializeField]
  public RectTransform _relicEffectsContentContainer;
  [SerializeField]
  public ActiveRelicItem _activeRelicItemTemplate;
  [SerializeField]
  public GameObject _noRelicsText;
  public List<TarotCardItem_Run> _tarotCardItems = new List<TarotCardItem_Run>();
  public List<ActiveRelicItem> _activeRelicItems = new List<ActiveRelicItem>();
  public static PlayerFarming OpeningPlayerFarming;

  public void Start() => this.Init();

  public void Init()
  {
    Skin newSkin = new Skin("Player Skin");
    Skin skin;
    if (CharacterMenu.OpeningPlayerFarming.isLamb)
    {
      skin = DataManager.Instance.PlayerVisualFleece != 1003 ? this._skeletonGraphic.Skeleton.Data.FindSkin("Lamb_" + DataManager.Instance.PlayerVisualFleece.ToString()) : this._skeletonGraphic.Skeleton.Data.FindSkin("Goat");
    }
    else
    {
      skin = this._skeletonGraphic.Skeleton.Data.FindSkin("Goat");
      this._titleText.text = LocalizationManager.GetTranslation("NAMES/Knucklebones/Goat");
    }
    newSkin.AddSkin(skin);
    this._skeletonGraphic.Skeleton.SetSkin(newSkin);
    this._weaponItem.Configure(CharacterMenu.OpeningPlayerFarming.currentWeapon);
    this._curseItem.Configure(CharacterMenu.OpeningPlayerFarming.currentCurse);
    this._relicItem.Configure(CharacterMenu.OpeningPlayerFarming.playerRelic.CurrentRelic);
    this._fleeceItem.Configure(DataManager.Instance.PlayerFleece);
    this._talismanPiecesItem.Configure(DataManager.Instance.CurrentKeyPieces);
    this._doctrineFragmentsItem.Configure(DataManager.Instance.DoctrineCurrentCount);
    this._crownAbilityItem1.Configure(UpgradeSystem.Type.Ability_TeleportHome);
    this._crownAbilityItem2.Configure(UpgradeSystem.Type.Ability_BlackHeart);
    this._crownAbilityItem3.Configure(UpgradeSystem.Type.Ability_Eat);
    this._crownAbilityItem4.Configure(UpgradeSystem.Type.Ability_Resurrection);
    if (GameManager.IsDungeon(PlayerFarming.Location))
      this._heartsContainer.InitDungeon(CharacterMenu.OpeningPlayerFarming);
    else
      this._heartsContainer.InitBase(CharacterMenu.OpeningPlayerFarming);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    foreach (PlayerFarming player in PlayerFarming.players)
      HUD_Manager.Instance.healthManager.Init(player);
  }

  public override void OnShowStarted()
  {
    this._scrollRect.enabled = false;
    this._scrollRect.normalizedPosition = Vector2.one;
    this._heartsContainer.gameObject.SetActive(DataManager.Instance.PlayerHasBeenGivenHearts);
    if (this._tarotCardItems.Count == 0 && CharacterMenu.OpeningPlayerFarming.RunTrinkets.Count > 0)
    {
      foreach (TarotCards.TarotCard runTrinket in CharacterMenu.OpeningPlayerFarming.RunTrinkets)
      {
        TarotCardItem_Run tarotCardItemRun = this._tarotCardItemRunTemplate.Instantiate<TarotCardItem_Run>((Transform) this._tarotCardContentContainer);
        tarotCardItemRun.Configure(runTrinket);
        this._tarotCardItems.Add(tarotCardItemRun);
      }
      this._noTarotText.SetActive(false);
      this._talismanPiecesItem.Selectable.navigation = this._talismanPiecesItem.Selectable.navigation with
      {
        selectOnDown = (Selectable) this._tarotCardItems[0].Selectable
      };
      this._doctrineFragmentsItem.Selectable.navigation = this._doctrineFragmentsItem.Selectable.navigation with
      {
        selectOnDown = (Selectable) this._tarotCardItems[0].Selectable
      };
    }
    if ((Object) BiomeGenerator.Instance != (Object) null && this._activeRelicItems.Count == 0)
    {
      List<RelicType> relicTypeList = new List<RelicType>();
      foreach (Familiar familiar in Familiar.Familiars)
      {
        RelicType relicType;
        if (familiar.TryGetRelicType(out relicType) && !relicTypeList.Contains(relicType))
        {
          this.AddActiveRelic(EquipmentManager.GetRelicData(relicType));
          relicTypeList.Add(relicType);
        }
      }
      if ((double) CharacterMenu.OpeningPlayerFarming.playerRelic.PlayerScaleModifier < 1.0)
        this.AddActiveRelic(EquipmentManager.GetRelicData(RelicType.Shrink));
      else if ((double) CharacterMenu.OpeningPlayerFarming.playerRelic.PlayerScaleModifier > 1.0)
        this.AddActiveRelic(EquipmentManager.GetRelicData(RelicType.Enlarge));
      if (this._tarotCardItems.Count == 0 && this._activeRelicItems.Count > 0)
      {
        this._talismanPiecesItem.Selectable.navigation = this._talismanPiecesItem.Selectable.navigation with
        {
          selectOnDown = (Selectable) this._activeRelicItems[0].Selectable
        };
        this._doctrineFragmentsItem.Selectable.navigation = this._doctrineFragmentsItem.Selectable.navigation with
        {
          selectOnDown = (Selectable) this._activeRelicItems[0].Selectable
        };
      }
    }
    this._noRelicsText.SetActive(this._activeRelicItems.Count == 0);
    this._scrollRect.enabled = true;
  }

  public ActiveRelicItem AddActiveRelic(RelicData relicData)
  {
    ActiveRelicItem activeRelicItem = this._activeRelicItemTemplate.Instantiate<ActiveRelicItem>();
    activeRelicItem.transform.SetParent((Transform) this._relicEffectsContentContainer);
    activeRelicItem.transform.localScale = Vector3.one;
    activeRelicItem.Configure(relicData);
    this._activeRelicItems.Add(activeRelicItem);
    return activeRelicItem;
  }
}
