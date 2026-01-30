// Decompiled with JetBrains decompiler
// Type: Structures_NatureShrine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class Structures_NatureShrine : StructureBrain
{
  public override void OnAdded()
  {
    base.OnAdded();
    TimeManager.OnNewDayStarted += new System.Action(this.OnNewDayStarted);
  }

  public override void OnRemoved()
  {
    base.OnRemoved();
    TimeManager.OnNewDayStarted -= new System.Action(this.OnNewDayStarted);
  }

  public virtual void OnNewDayStarted() => this.UpdateFuel();
}
