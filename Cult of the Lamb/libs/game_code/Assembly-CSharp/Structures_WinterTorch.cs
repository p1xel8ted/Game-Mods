// Decompiled with JetBrains decompiler
// Type: Structures_WinterTorch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
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
