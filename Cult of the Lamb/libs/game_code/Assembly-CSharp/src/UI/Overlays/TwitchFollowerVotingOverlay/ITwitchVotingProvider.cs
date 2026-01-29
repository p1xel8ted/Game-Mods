// Decompiled with JetBrains decompiler
// Type: src.UI.Overlays.TwitchFollowerVotingOverlay.ITwitchVotingProvider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace src.UI.Overlays.TwitchFollowerVotingOverlay;

public interface ITwitchVotingProvider
{
  bool AllowsVoting { get; set; }

  TwitchVoting.VotingType VotingType { get; set; }

  List<FollowerInfo> ProvideInfo();

  void FinalizeVote(FollowerInfo followerInfo);
}
