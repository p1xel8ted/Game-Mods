// Decompiled with JetBrains decompiler
// Type: Structures_HealingBay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class Structures_HealingBay : StructureBrain
{
  public int Level;

  public float Multiplier => this.Level != 0 ? 1.5f : 1f;

  public Structures_HealingBay(int level) => this.Level = level;

  public bool CheckIfOccupied() => this.Data.FollowerID != -1;
}
