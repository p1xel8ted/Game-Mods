// Decompiled with JetBrains decompiler
// Type: Structures_Scarecrow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
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
