// Decompiled with JetBrains decompiler
// Type: Structures_Scarecrow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
