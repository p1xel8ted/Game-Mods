// Decompiled with JetBrains decompiler
// Type: Structures_EggFollower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class Structures_EggFollower : StructureBrain
{
  public System.Action OnRotten;

  public override void OnNewPhaseStarted()
  {
    base.OnNewPhaseStarted();
    if (this.Data.Rotten || this.Data.Burned)
      return;
    ++this.Data.Age;
    if (this.Data.Age < 10 || this.ReservedForTask || !this.Data.CanBecomeRotten || this.Data.EggInfo.Special != FollowerSpecialType.None || this.Data.EggInfo.Rotting)
      return;
    this.Data.Rotten = true;
    System.Action onRotten = this.OnRotten;
    if (onRotten == null)
      return;
    onRotten();
  }
}
