// Decompiled with JetBrains decompiler
// Type: Structures_WinterTorch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class Structures_WinterTorch : StructureBrain
{
  public override void OnAdded()
  {
    base.OnAdded();
    this.Data.Fuel = this.Data.MaxFuel;
    this.Data.FullyFueled = true;
  }

  public override void OnNewPhaseStarted()
  {
    base.OnNewPhaseStarted();
    if (SeasonsManager.CurrentSeason != SeasonsManager.Season.Winter)
      return;
    this.UpdateFuel(1);
  }
}
