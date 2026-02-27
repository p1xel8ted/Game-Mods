// Decompiled with JetBrains decompiler
// Type: Structures_CompostBin
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class Structures_CompostBin : StructureBrain
{
  public int CompostCount;
  public int CompostCost = 50;
  public int PoopToCreate = 15;
  public int PoopCount;
  public static float COMPOST_DURATION = 400f;
  public List<float> DepositTimes = new List<float>();
  public System.Action UpdateCompostState;

  public void AddGrass()
  {
    ++this.CompostCount;
    this.DepositTimes.Add(TimeManager.TotalElapsedGameTime);
    System.Action updateCompostState = this.UpdateCompostState;
    if (updateCompostState == null)
      return;
    updateCompostState();
  }

  public void AddPoop()
  {
    this.CompostCount = 0;
    this.PoopCount += this.PoopToCreate;
    System.Action updateCompostState = this.UpdateCompostState;
    if (updateCompostState == null)
      return;
    updateCompostState();
  }

  public void CollectPoop()
  {
    this.PoopCount = 0;
    System.Action updateCompostState = this.UpdateCompostState;
    if (updateCompostState == null)
      return;
    updateCompostState();
  }
}
