// Decompiled with JetBrains decompiler
// Type: Structures_HealingBay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class Structures_HealingBay : StructureBrain
{
  public int Level;

  public float Multiplier => this.Level != 0 ? 1.5f : 1f;

  public Structures_HealingBay(int level) => this.Level = level;

  public bool CheckIfOccupied() => this.Data.FollowerID != -1;
}
