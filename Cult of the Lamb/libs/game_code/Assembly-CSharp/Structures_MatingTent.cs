// Decompiled with JetBrains decompiler
// Type: Structures_MatingTent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Structures_MatingTent : StructureBrain
{
  public static List<string> goldenSkins = new List<string>()
  {
    "Seal",
    "Lemur",
    "Caterpillar"
  };

  public void SetEggInfo(FollowerBrain f1, FollowerBrain f2, float chanceForSuccess)
  {
    StructuresData.EggData eggData = new StructuresData.EggData();
    eggData.Parent_1_ID = f1.Info.ID;
    eggData.Parent_2_ID = f2.Info.ID;
    eggData.Parent_1_SkinColor = f1.Info.SkinColour;
    eggData.Parent_2_SkinColor = f2.Info.SkinColour;
    eggData.Parent_1_SkinVariant = f1.Info.SkinVariation;
    eggData.Parent_2_SkinVariant = f2.Info.SkinVariation;
    eggData.Parent_1_SkinName = f1.Info.SkinName;
    eggData.Parent_2_SkinName = f2.Info.SkinName;
    eggData.Parent1Name = f1.Info.Name;
    eggData.Parent2Name = f2.Info.Name;
    eggData.Traits = f1.Info.Traits;
    eggData.EggSeed = Random.Range(1, int.MaxValue);
    eggData.Golden = DataManager.Instance.ForceGoldenEgg || DataManager.Instance.EggsProduced == 1 || (double) Random.value < 0.20000000298023224 || f1.Info.Traits.Contains(FollowerTrait.TraitType.PureBlood_3);
    if (FollowerManager.UniqueFollowerIDs.Contains(f1.Info.ID) || FollowerManager.UniqueFollowerIDs.Contains(f2.Info.ID))
      eggData.Golden = false;
    else if (DataManager.Instance.PalworldSkins.Contains<string>(f1.Info.SkinName) && DataManager.Instance.PalworldSkins.Contains<string>(f2.Info.SkinName) && f1.Info.SkinName != f2.Info.SkinName && !DataManager.Instance.FollowerSkinsUnlocked.Contains("PalworldTwo"))
      eggData.Golden = false;
    else if (f1.Info.IsSnowman && f2.Info.IsSnowman)
    {
      eggData.Golden = false;
    }
    else
    {
      if (!eggData.Golden && DataManager.Instance.EggsProduced > 10 && Structures_MatingTent.GetPossibleGoldenEggSkins().Count > 0 && (double) Random.value < 0.40000000596046448)
        eggData.Golden = true;
      DataManager.Instance.ForceGoldenEgg = false;
    }
    if (eggData.Traits.Contains(FollowerTrait.TraitType.Mutated))
    {
      eggData.Golden = false;
      eggData.Rotting = true;
      if (f1.Info.HasTrait(FollowerTrait.TraitType.Mutated) && f2.Info.HasTrait(FollowerTrait.TraitType.Mutated))
        eggData.RottingUnique = true;
    }
    this.Data.EggInfo = eggData;
    this.Data.HasEgg = true;
    this.Data.MatingFailed = false;
    if (!DataManager.Instance.HadInitialMatingTentInteraction || (double) chanceForSuccess >= 1.0 || eggData.Golden)
      return;
    float num = 0.05f;
    if (f1.Info.CursedState == Thought.OldAge)
      num += 0.05f;
    if (f2.Info.CursedState == Thought.OldAge)
      num += 0.05f;
    this.Data.MatingFailed = (double) Random.value < (double) num;
  }

  public void SetEggReady() => this.Data.EggReady = true;

  public bool HasEgg() => this.Data.EggReady && this.Data.EggInfo.EggSeed != -1;

  public void CollectEgg()
  {
    this.Data.EggInfo = (StructuresData.EggData) null;
    this.Data.EggReady = false;
    this.Data.MatingFailed = false;
    this.Data.HasEgg = false;
  }

  public static List<string> GetPossibleGoldenEggSkins()
  {
    List<string> ts = new List<string>();
    foreach (string goldenSkin in Structures_MatingTent.goldenSkins)
    {
      if (!DataManager.GetFollowerSkinUnlocked(goldenSkin))
        ts.Add(goldenSkin);
    }
    ts.Shuffle<string>();
    return ts;
  }
}
