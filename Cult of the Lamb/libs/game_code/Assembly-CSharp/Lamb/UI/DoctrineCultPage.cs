// Decompiled with JetBrains decompiler
// Type: Lamb.UI.DoctrineCultPage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI.FollowerSelect;
using src.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class DoctrineCultPage : UISubmenuBase
{
  [SerializeField]
  public TextMeshProUGUI _cultName;
  [SerializeField]
  public MMButton _renameCultButton;
  [SerializeField]
  public MMButton _viewFollowersButton;
  [SerializeField]
  public TMP_Text _quoteText;
  [SerializeField]
  public LayoutElement _quoteLayoutElement;
  [Header("Statistics")]
  [SerializeField]
  public TMP_Text _totalFollowersCount;
  [SerializeField]
  public TMP_Text _murderCount;
  [SerializeField]
  public TMP_Text _starvationCount;
  [SerializeField]
  public TMP_Text _sacrificesCount;
  [SerializeField]
  public TMP_Text _naturalDeaths;
  [SerializeField]
  public TMP_Text _crusadesCount;
  [SerializeField]
  public TMP_Text _deathsCount;
  [SerializeField]
  public TMP_Text _killsCount;
  [SerializeField]
  public TMP_Text _wintersSurvived;
  [SerializeField]
  public TMP_Text _wintersSurvivedCount;
  public bool isLoadingAssets;

  public override void Awake()
  {
    base.Awake();
    this._renameCultButton.gameObject.SetActive(DataManager.Instance.OnboardedCultName);
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
    this._wintersSurvivedCount.text = DataManager.Instance.WintersOccured.ToString();
    this._wintersSurvived.gameObject.SetActive(SeasonsManager.Active);
    this._quoteLayoutElement.preferredWidth = !SettingsManager.Settings.Accessibility.DyslexicFont ? 350f : 550f;
    if (DataManager.Instance.BeatenPostGame)
      this._quoteText.text = ScriptLocalization.QUOTE.QuoteBoss5;
    else
      this._quoteText.text = ScriptLocalization.QUOTE.IntroQuote;
  }

  public void OnRenameCultClicked()
  {
    if (this.isLoadingAssets)
      return;
    this.isLoadingAssets = true;
    this.StartCoroutine((IEnumerator) UIManager.LoadAssets(MonoSingleton<UIManager>.Instance.LoadCultNameAssets(), (System.Action) (() =>
    {
      this.isLoadingAssets = false;
      UICultNameMenuController menu = MonoSingleton<UIManager>.Instance.CultNameMenuTemplate.Instantiate<UICultNameMenuController>();
      menu.Show(DataManager.Instance.CultName, true, false);
      menu.OnNameConfirmed += (Action<string>) (result =>
      {
        this._cultName.text = result;
        DataManager.Instance.CultName = result;
      });
      this.PushInstance<UICultNameMenuController>(menu);
    })));
  }

  public void OnViewFollowersClicked()
  {
    List<FollowerSelectEntry> followerSelectEntries = new List<FollowerSelectEntry>();
    foreach (FollowerInfo follower in DataManager.Instance.Followers)
      followerSelectEntries.Add(new FollowerSelectEntry(follower));
    UIFollowerSelectMenuController menu = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
    menu.AllowsVoting = false;
    menu.Show(followerSelectEntries, false, UpgradeSystem.Type.Count, false, true, false, false, true);
    this.PushInstance<UIFollowerSelectMenuController>(menu);
  }

  [CompilerGenerated]
  public void \u003COnRenameCultClicked\u003Eb__17_0()
  {
    this.isLoadingAssets = false;
    UICultNameMenuController menu = MonoSingleton<UIManager>.Instance.CultNameMenuTemplate.Instantiate<UICultNameMenuController>();
    menu.Show(DataManager.Instance.CultName, true, false);
    menu.OnNameConfirmed += (Action<string>) (result =>
    {
      this._cultName.text = result;
      DataManager.Instance.CultName = result;
    });
    this.PushInstance<UICultNameMenuController>(menu);
  }

  [CompilerGenerated]
  public void \u003COnRenameCultClicked\u003Eb__17_1(string result)
  {
    this._cultName.text = result;
    DataManager.Instance.CultName = result;
  }
}
