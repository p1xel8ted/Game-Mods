// Decompiled with JetBrains decompiler
// Type: DLCRefugees.RefugeeWealth
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

#nullable disable
namespace DLCRefugees;

public class RefugeeWealth
{
  public float water_satiety_coeff;
  public float energy_satiety_coeff;
  public int bed_wealth;
  public int refugees_count;

  public RefugeeWealth(float water_satiety_coeff, float energy_satiety_coeff, int refugees_count)
  {
    this.water_satiety_coeff = water_satiety_coeff;
    this.energy_satiety_coeff = energy_satiety_coeff;
    this.refugees_count = refugees_count;
  }
}
