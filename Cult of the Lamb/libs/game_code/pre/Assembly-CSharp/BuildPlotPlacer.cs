// Decompiled with JetBrains decompiler
// Type: BuildPlotPlacer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class BuildPlotPlacer : BaseMonoBehaviour
{
  public static List<BuildPlotPlacer> BuildPlotPlacers = new List<BuildPlotPlacer>();

  private void OnEnable() => BuildPlotPlacer.BuildPlotPlacers.Add(this);

  private void OnDisable() => BuildPlotPlacer.BuildPlotPlacers.Remove(this);

  private void OnDrawGizmos()
  {
  }
}
