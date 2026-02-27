// Decompiled with JetBrains decompiler
// Type: Structures_PropagandaSpeaker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class Structures_PropagandaSpeaker : StructureBrain
{
  public static float EFFECTIVE_DISTANCE = 8f;

  public override void OnNewPhaseStarted() => this.UpdateFuel(1);
}
