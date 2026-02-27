// Decompiled with JetBrains decompiler
// Type: Structures_DeadWorshipper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class Structures_DeadWorshipper : StructureBrain
{
  public override void OnAdded() => TimeManager.OnNewDayStarted += new System.Action(this.OnNewDayStarted);

  public override void OnRemoved()
  {
    TimeManager.OnNewDayStarted -= new System.Action(this.OnNewDayStarted);
  }

  private void OnNewDayStarted()
  {
    ++this.Data.Age;
    if (this.Data.Age < 2)
      return;
    this.Data.Rotten = true;
  }
}
