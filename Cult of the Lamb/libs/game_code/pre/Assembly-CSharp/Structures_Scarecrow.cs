// Decompiled with JetBrains decompiler
// Type: Structures_Scarecrow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class Structures_Scarecrow : StructureBrain
{
  public System.Action OnCatchBird;
  private float LastBirdCaught;
  public bool HasBird;

  public override void OnAdded()
  {
    if (this.Data.Type != StructureBrain.TYPES.SCARECROW_2)
      return;
    TimeManager.OnNewPhaseStarted += new System.Action(this.OnNewPhaseStarted);
  }

  public override void OnRemoved()
  {
    if (this.Data.Type != StructureBrain.TYPES.SCARECROW_2)
      return;
    TimeManager.OnNewPhaseStarted -= new System.Action(this.OnNewPhaseStarted);
  }

  private void OnNewPhaseStarted()
  {
    if (this.HasBird || (double) UnityEngine.Random.value > 0.60000002384185791 || (double) TimeManager.TotalElapsedGameTime - (double) this.LastBirdCaught <= 720.0)
      return;
    this.HasBird = true;
    System.Action onCatchBird = this.OnCatchBird;
    if (onCatchBird == null)
      return;
    onCatchBird();
  }

  public void EmptyTrap()
  {
    this.HasBird = false;
    this.LastBirdCaught = TimeManager.TotalElapsedGameTime;
  }
}
