// Decompiled with JetBrains decompiler
// Type: Lamb.UI.DoctrineCultPage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI.FollowerSelect;
using src.Extensions;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace Lamb.UI;

public class DoctrineCultPage : UISubmenuBase
{
  [SerializeField]
  private TextMeshProUGUI _cultName;
  [SerializeField]
  private MMButton _renameCultButton;
  [SerializeField]
  private MMButton _viewFollowersButton;
  [Header("Statistics")]
  [SerializeField]
  private TMP_Text _totalFollowersCount;
  [SerializeField]
  private TMP_Text _murderCount;
  [SerializeField]
  private TMP_Text _starvationCount;
  [SerializeField]
  private TMP_Text _sacrificesCount;
  [SerializeField]
  private TMP_Text _naturalDeaths;
  [SerializeField]
  private TMP_Text _crusadesCount;
  [SerializeField]
  private TMP_Text _deathsCount;
  [SerializeField]
  private TMP_Text _killsCount;

  public override void Awake()
  {
    base.Awake();
    this._cultName.text = DataManager.Instance.CultName;
    this._renameCultButton.onClick.AddListener(new UnityAction(this.OnRenameCultClicked));
    this._viewFollowersButton.onClick.AddListener(new UnityAction(this.OnViewFollowersClicked));
    this._totalFollowersCount.text = (DataManager.Instance.Followers.Count + DataManager.Instance.Followers_Dead.Count).ToString();
    this._murderCount.text = DataManager.Instance.STATS_Murders.ToString();
    this._starvationCount.text = DataManager.Instance.STATS_FollowersStarvedToDeath.ToString();
    this._sacrificesCount.text = DataManager.Instance.STATS_Sacrifices.ToString();
    this._naturalDeaths.text = DataManager.Instance.STATS_NaturalDeaths.ToString();
    this._crusadesCount.text = DataManager.Instance.dungeonRun.ToString();
    this._deathsCount.text = DataManager.Instance.playerDeaths.ToString();
    this._killsCount.text = DataManager.Instance.KillsInGame.ToString();
  }

  private void OnRenameCultClicked()
  {
    UICultNameMenuController menu = MonoSingleton<UIManager>.Instance.CultNameMenuTemplate.Instantiate<UICultNameMenuController>();
    menu.Show(true, false);
    menu.OnNameConfirmed += (Action<string>) (result =>
    {
      this._cultName.text = result;
      DataManager.Instance.CultName = result;
    });
    this.PushInstance<UICultNameMenuController>(menu);
  }

  private void OnViewFollowersClicked()
  {
    UIFollowerSelectMenuController menu = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
    menu.Show(DataManager.Instance.Followers, (List<FollowerInfo>) null, false, UpgradeSystem.Type.Count, false, true, false);
    this.PushInstance<UIFollowerSelectMenuController>(menu);
  }
}
