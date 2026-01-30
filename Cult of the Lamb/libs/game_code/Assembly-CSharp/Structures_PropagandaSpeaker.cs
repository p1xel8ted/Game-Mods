// Decompiled with JetBrains decompiler
// Type: Structures_PropagandaSpeaker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class Structures_PropagandaSpeaker : StructureBrain
{
  public static float EFFECTIVE_DISTANCE = 8f;

  public override void OnNewPhaseStarted() => this.UpdateFuel(1);
}
