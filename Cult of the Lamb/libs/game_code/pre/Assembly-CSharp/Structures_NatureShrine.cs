// Decompiled with JetBrains decompiler
// Type: Structures_NatureShrine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class Structures_NatureShrine : StructureBrain
{
  public override void OnAdded() => TimeManager.OnNewDayStarted += new System.Action(this.OnNewDayStarted);

  public override void OnRemoved()
  {
    TimeManager.OnNewDayStarted -= new System.Action(this.OnNewDayStarted);
  }

  protected virtual void OnNewDayStarted() => this.UpdateFuel();
}
