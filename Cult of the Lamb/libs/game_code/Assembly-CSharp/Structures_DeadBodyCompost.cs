// Decompiled with JetBrains decompiler
// Type: Structures_DeadBodyCompost
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class Structures_DeadBodyCompost : Structures_CompostBin
{
  public override int CompostCost => 20;

  public override int PoopToCreate => 10;

  public override float COMPOST_DURATION => 120f;
}
