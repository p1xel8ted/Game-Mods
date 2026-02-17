// Decompiled with JetBrains decompiler
// Type: BuildPlotPlacer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class BuildPlotPlacer : BaseMonoBehaviour
{
  public static List<BuildPlotPlacer> BuildPlotPlacers = new List<BuildPlotPlacer>();

  public void OnEnable() => BuildPlotPlacer.BuildPlotPlacers.Add(this);

  public void OnDisable() => BuildPlotPlacer.BuildPlotPlacers.Remove(this);

  public void OnDrawGizmos()
  {
  }
}
