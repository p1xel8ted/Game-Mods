// Decompiled with JetBrains decompiler
// Type: Structures_Hatchery
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class Structures_Hatchery : StructureBrain
{
  public System.Action OnEggReady;

  public float Normalised_Progress => this.Data.Progress / this.Data.ProgressTarget;

  public void EggAdded() => this.Data.Progress = 0.0f;

  public override void OnNewPhaseStarted()
  {
    base.OnNewPhaseStarted();
    if (!this.Data.Watered || ++this.Data.WateredCount < 5)
      return;
    ++this.Data.Progress;
    if ((double) this.Data.Progress >= (double) this.Data.ProgressTarget)
      this.SetEggReady();
    this.Data.WateredCount = 0;
    this.Data.Watered = false;
  }

  public void SetEggReady()
  {
    this.Data.WateredCount = 0;
    this.Data.Watered = false;
    this.Data.EggReady = true;
    System.Action onEggReady = this.OnEggReady;
    if (onEggReady == null)
      return;
    onEggReady();
  }

  public override bool SnowedUnder(bool showNotifications = true, bool refreshFollowerTasks = true)
  {
    return !this.Data.HasEgg && base.SnowedUnder(showNotifications, refreshFollowerTasks);
  }
}
