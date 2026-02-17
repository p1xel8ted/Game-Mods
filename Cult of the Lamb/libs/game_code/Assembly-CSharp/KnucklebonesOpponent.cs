// Decompiled with JetBrains decompiler
// Type: KnucklebonesOpponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System;

#nullable disable
[Serializable]
public class KnucklebonesOpponent
{
  public KnucklebonesOpponent.OppnentTags Tag;
  public KnucklebonesPlayerConfiguration Config;
  public SkeletonAnimation Spine;
  public Interaction_SimpleConversation FirstWinConvo;
  public Interaction_SimpleConversation WinConvo;
  public Interaction_SimpleConversation LoseConvo;
  public Interaction_SimpleConversation DrawConvo;
  public TarotCards.Card TarotCardReward;
  public bool IsFollower;
  public bool IsCoopPlayer;
  public bool IsTwitchChat;

  public enum OppnentTags
  {
    Ratau,
    Flinky,
    Klunko,
    Shrumy,
  }
}
