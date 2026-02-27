// Decompiled with JetBrains decompiler
// Type: KnucklebonesOpponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  public TarotCards.Card TarotCardReward;

  public enum OppnentTags
  {
    Ratau,
    Flinky,
    Klunko,
    Shrumy,
  }
}
