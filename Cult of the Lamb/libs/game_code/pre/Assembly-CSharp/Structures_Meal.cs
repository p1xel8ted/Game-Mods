// Decompiled with JetBrains decompiler
// Type: Structures_Meal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class Structures_Meal : StructureBrain
{
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
    TimeManager.OnNewPhaseStarted += new System.Action(this.OnNewPhaseStarted);
  }

  public override void OnRemoved()
  {
    TimeManager.OnNewPhaseStarted -= new System.Action(this.OnNewPhaseStarted);
  }

  private void OnNewPhaseStarted()
  {
    if (this.Data.Rotten || this.Data.Burned)
      return;
    ++this.Data.Age;
    if (this.Data.Age < 15 || this.ReservedForTask)
      return;
    this.Data.Rotten = true;
  }
}
