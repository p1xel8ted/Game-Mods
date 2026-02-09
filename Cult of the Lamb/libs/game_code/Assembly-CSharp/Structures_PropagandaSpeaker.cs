// Decompiled with JetBrains decompiler
// Type: Structures_PropagandaSpeaker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class Structures_PropagandaSpeaker : StructureBrain
{
  public static float EFFECTIVE_DISTANCE = 8f;

  public override void OnNewPhaseStarted() => this.UpdateFuel(1);
}
