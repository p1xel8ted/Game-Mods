// Decompiled with JetBrains decompiler
// Type: Structures_HealingBay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class Structures_HealingBay : StructureBrain
{
  public int Level;

  public float Multiplier => this.Level != 0 ? 1.5f : 1f;

  public Structures_HealingBay(int level) => this.Level = level;

  public bool CheckIfOccupied() => this.Data.FollowerID != -1;
}
