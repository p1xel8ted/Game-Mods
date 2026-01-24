// Decompiled with JetBrains decompiler
// Type: Structures_Scarecrow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class Structures_Scarecrow : StructureBrain
{
  public System.Action OnCatchBird;
  public float LastBirdCaught;
  public bool HasBird;

  public override void OnNewPhaseStarted()
  {
    if (this.Data.Type != StructureBrain.TYPES.SCARECROW_2 || this.HasBird || (double) UnityEngine.Random.value > 0.60000002384185791 || SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter || (double) TimeManager.TotalElapsedGameTime - (double) this.LastBirdCaught <= 720.0)
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
