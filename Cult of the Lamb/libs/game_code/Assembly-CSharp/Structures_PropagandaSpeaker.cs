// Decompiled with JetBrains decompiler
// Type: Structures_PropagandaSpeaker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class Structures_PropagandaSpeaker : StructureBrain
{
  public static float EFFECTIVE_DISTANCE = 8f;

  public override void OnNewPhaseStarted() => this.UpdateFuel(1);
}
