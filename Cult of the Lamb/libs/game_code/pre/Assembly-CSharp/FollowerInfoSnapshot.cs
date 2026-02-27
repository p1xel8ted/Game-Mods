// Decompiled with JetBrains decompiler
// Type: FollowerInfoSnapshot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public class FollowerInfoSnapshot
{
  public string Name;
  public int SkinCharacter;
  public int SkinVariation;
  public int SkinColour;
  public string SkinName;
  public FollowerOutfitType Outfit;
  public InventoryItem.ITEM_TYPE Necklace;
  public float Illness;
  public float Rest;
  public bool Brainwashed;
  public bool Dissenter;
  public float CultFaith;

  public FollowerInfoSnapshot()
  {
  }

  public FollowerInfoSnapshot(FollowerInfo followerInfo)
  {
    this.Name = followerInfo.Name;
    this.SkinCharacter = followerInfo.SkinCharacter;
    this.SkinVariation = followerInfo.SkinVariation;
    this.SkinColour = followerInfo.SkinColour;
    this.SkinName = followerInfo.SkinName;
    this.Outfit = followerInfo.Outfit;
    this.Necklace = followerInfo.Necklace;
    this.Illness = followerInfo.Illness;
    this.Rest = followerInfo.Rest;
    this.Brainwashed = FollowerBrainStats.BrainWashed;
    this.Dissenter = followerInfo.HasThought(Thought.Dissenter);
  }
}
