// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Menus.PlayerMenu.CharacterMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI.PauseDetails;
using Spine.Unity;
using src.Extensions;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Lamb.UI.Menus.PlayerMenu;

public class CharacterMenu : UISubmenuBase
{
  [Header("Character Menu")]
  [SerializeField]
  private MMScrollRect _scrollRect;
  [SerializeField]
  private SkeletonGraphic _skeletonGraphic;
  [SerializeField]
  private GameObject _heartsContainer;
  [Header("Items")]
  [SerializeField]
  private WeaponItem _weaponItem;
  [SerializeField]
  private CurseItem _curseItem;
  [SerializeField]
  private FleeceItem _fleeceItem;
  [SerializeField]
  private TalismanPiecesItem _talismanPiecesItem;
  [SerializeField]
  private DoctrineFragmentsItem _doctrineFragmentsItem;
  [SerializeField]
  private CrownAbilityItem _crownAbilityItem1;
  [SerializeField]
  private CrownAbilityItem _crownAbilityItem2;
  [SerializeField]
  private CrownAbilityItem _crownAbilityItem3;
  [SerializeField]
  private CrownAbilityItem _crownAbilityItem4;
  [Header("Tarot")]
  [SerializeField]
  private RectTransform _tarotCardContentContainer;
  [SerializeField]
  private TarotCardItem_Run _tarotCardItemRunTemplate;
  [SerializeField]
  private GameObject _noTarotText;
  private List<TarotCardItem_Run> _tarotCardItems = new List<TarotCardItem_Run>();

  private void Start()
  {
    this._skeletonGraphic.Skeleton.SetSkin("Lamb_" + (object) DataManager.Instance.PlayerFleece);
    this._weaponItem.Configure(DataManager.Instance.CurrentWeapon);
    this._curseItem.Configure(DataManager.Instance.CurrentCurse);
    this._fleeceItem.Configure(DataManager.Instance.PlayerFleece);
    this._talismanPiecesItem.Configure(DataManager.Instance.CurrentKeyPieces);
    this._doctrineFragmentsItem.Configure(DataManager.Instance.DoctrineCurrentCount);
    this._crownAbilityItem1.Configure(UpgradeSystem.Type.Ability_TeleportHome);
    this._crownAbilityItem2.Configure(UpgradeSystem.Type.Ability_BlackHeart);
    this._crownAbilityItem3.Configure(UpgradeSystem.Type.Ability_Eat);
    this._crownAbilityItem4.Configure(UpgradeSystem.Type.Ability_Resurrection);
  }

  protected override void OnShowStarted()
  {
    this._scrollRect.enabled = false;
    this._scrollRect.normalizedPosition = Vector2.one;
    this._heartsContainer.SetActive(DataManager.Instance.PlayerHasBeenGivenHearts);
    if (this._tarotCardItems.Count == 0 && DataManager.Instance.PlayerRunTrinkets.Count > 0)
    {
      foreach (TarotCards.TarotCard playerRunTrinket in DataManager.Instance.PlayerRunTrinkets)
      {
        TarotCardItem_Run tarotCardItemRun = this._tarotCardItemRunTemplate.Instantiate<TarotCardItem_Run>((Transform) this._tarotCardContentContainer);
        tarotCardItemRun.Configure(playerRunTrinket);
        this._tarotCardItems.Add(tarotCardItemRun);
      }
      this._noTarotText.SetActive(false);
    }
    this._scrollRect.enabled = true;
  }
}
