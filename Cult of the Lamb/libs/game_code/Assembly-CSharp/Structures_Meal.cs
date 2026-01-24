// Decompiled with JetBrains decompiler
// Type: Structures_Meal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class Structures_Meal : StructureBrain
{
  public float Created_Timestamp;

  public static bool GetListContainsMealType(List<Structures_Meal> List, StructureBrain.TYPES Type)
  {
    foreach (Structures_Meal structuresMeal in List)
    {
      if (structuresMeal.Data.Type == Type && !structuresMeal.ReservedForTask)
        return true;
    }
    return false;
  }

  public override void OnAdded()
  {
    base.OnAdded();
    this.Created_Timestamp = TimeManager.TotalElapsedGameTime;
  }

  public override void OnNewPhaseStarted()
  {
    if (this.Data.Rotten || this.Data.Burned)
      return;
    ++this.Data.Age;
    if (this.Data.Age < 15 || this.ReservedForTask || !this.Data.CanBecomeRotten)
      return;
    this.Data.Rotten = true;
  }

  public bool CanEat()
  {
    return !DataManager.Instance.SurvivalModeActive || (double) TimeManager.TotalElapsedGameTime - (double) this.Created_Timestamp > 4.5;
  }
}
