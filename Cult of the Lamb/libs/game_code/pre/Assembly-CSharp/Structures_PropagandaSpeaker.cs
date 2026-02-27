// Decompiled with JetBrains decompiler
// Type: Structures_PropagandaSpeaker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class Structures_PropagandaSpeaker : StructureBrain
{
  public static float EFFECTIVE_DISTANCE = 8f;

  public override void OnAdded()
  {
    TimeManager.OnNewPhaseStarted += new System.Action(this.OnNewPhaseStarted);
  }

  public override void OnRemoved()
  {
    TimeManager.OnNewPhaseStarted -= new System.Action(this.OnNewPhaseStarted);
  }

  private void OnNewPhaseStarted() => this.UpdateFuel(1);
}
