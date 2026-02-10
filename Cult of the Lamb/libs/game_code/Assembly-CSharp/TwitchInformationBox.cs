// Decompiled with JetBrains decompiler
// Type: TwitchInformationBox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using src.Extensions;
using src.UI.Overlays.TwitchFollowerVotingOverlay;
using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class TwitchInformationBox : FollowerSelectItem
{
  [SerializeField]
  public TMP_Text votingText;
  public ITwitchVotingProvider _votingProvider;

  public void Configure(ITwitchVotingProvider votingProvider)
  {
    this._votingProvider = votingProvider;
    this.ConfigureImpl();
  }

  public override void ConfigureImpl()
  {
    this.Button.onClick.AddListener((UnityAction) (() =>
    {
      UIMenuBase uiMenuBase = UIMenuBase.ActiveMenus.LastElement<UIMenuBase>();
      UITwitchFollowerVotingOverlayController overlayController = MonoSingleton<UIManager>.Instance.TwitchFollowerVotingOverlayController.Instantiate<UITwitchFollowerVotingOverlayController>();
      overlayController.Show(this._votingProvider.ProvideInfo(), this._votingProvider.VotingType, false);
      UITwitchFollowerVotingOverlayController menu = overlayController;
      uiMenuBase.PushInstance<UITwitchFollowerVotingOverlayController>(menu);
      overlayController.OnFollowerChosen += (Action<FollowerBrain>) (result => this._votingProvider.FinalizeVote(result._directInfoAccess));
    }));
  }

  [CompilerGenerated]
  public void \u003CConfigureImpl\u003Eb__3_0()
  {
    UIMenuBase uiMenuBase = UIMenuBase.ActiveMenus.LastElement<UIMenuBase>();
    UITwitchFollowerVotingOverlayController overlayController = MonoSingleton<UIManager>.Instance.TwitchFollowerVotingOverlayController.Instantiate<UITwitchFollowerVotingOverlayController>();
    overlayController.Show(this._votingProvider.ProvideInfo(), this._votingProvider.VotingType, false);
    UITwitchFollowerVotingOverlayController menu = overlayController;
    uiMenuBase.PushInstance<UITwitchFollowerVotingOverlayController>(menu);
    overlayController.OnFollowerChosen += (Action<FollowerBrain>) (result => this._votingProvider.FinalizeVote(result._directInfoAccess));
  }

  [CompilerGenerated]
  public void \u003CConfigureImpl\u003Eb__3_1(FollowerBrain result)
  {
    this._votingProvider.FinalizeVote(result._directInfoAccess);
  }
}
