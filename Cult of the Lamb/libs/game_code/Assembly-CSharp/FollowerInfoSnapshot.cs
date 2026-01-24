// Decompiled with JetBrains decompiler
// Type: FollowerInfoSnapshot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using System;

#nullable disable
[MessagePackObject(false)]
[Serializable]
public class FollowerInfoSnapshot
{
  [Key(0)]
  public string Name;
  [Key(1)]
  public int SkinCharacter;
  [Key(2)]
  public int SkinVariation;
  [Key(3)]
  public int SkinColour;
  [Key(4)]
  public string SkinName;
  [Key(5)]
  public FollowerHatType Hat;
  [Key(6)]
  public FollowerOutfitType Outfit;
  [Key(7)]
  public FollowerClothingType Clothing;
  [Key(8)]
  public string ClothingVariant;
  [Key(9)]
  public FollowerCustomisationType Customisation;
  [Key(10)]
  public FollowerSpecialType Special;
  [Key(11)]
  public InventoryItem.ITEM_TYPE Necklace;
  [Key(12)]
  public float Illness;
  [Key(13)]
  public float Rest;
  [Key(14)]
  public bool Brainwashed;
  [Key(15)]
  public bool Dissenter;
  [Key(16 /*0x10*/)]
  public float CultFaith;
  [Key(17)]
  public int Rotten;
  [Key(18)]
  public int ID;

  public FollowerInfoSnapshot()
  {
  }

  public FollowerInfoSnapshot(FollowerInfo followerInfo)
  {
    this.Name = followerInfo.Name;
    this.ID = followerInfo.ID;
    this.SkinCharacter = followerInfo.SkinCharacter;
    this.SkinVariation = followerInfo.SkinVariation;
    this.SkinColour = followerInfo.SkinColour;
    this.SkinName = followerInfo.SkinName;
    this.Outfit = followerInfo.Outfit;
    this.Clothing = followerInfo.Clothing;
    this.ClothingVariant = followerInfo.ClothingVariant;
    this.Necklace = followerInfo.Necklace;
    this.Illness = followerInfo.Illness;
    this.Rest = followerInfo.Rest;
    this.Brainwashed = FollowerBrainStats.BrainWashed;
    this.Dissenter = followerInfo.HasThought(Thought.Dissenter);
    this.Rotten = followerInfo.Traits.Contains(FollowerTrait.TraitType.Mutated) ? (followerInfo.RottingUnique ? 2 : 1) : 0;
    this.CultFaith = DataManager.Instance.CultFaith;
  }
}
