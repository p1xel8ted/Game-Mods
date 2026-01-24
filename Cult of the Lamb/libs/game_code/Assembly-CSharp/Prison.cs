// Decompiled with JetBrains decompiler
// Type: Prison
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Prison : BaseMonoBehaviour
{
  public static List<Prison> Prisons = new List<Prison>();
  public Structure Structure;
  public Transform PrisonerLocation;
  public GameObject PrisonerExitLocation;

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public void OnEnable() => Prison.Prisons.Add(this);

  public void OnDisable() => Prison.Prisons.Remove(this);

  public static bool HasAvailablePrisons()
  {
    foreach (Prison prison in Prison.Prisons)
    {
      if (prison.StructureInfo.FollowerID == -1 && !prison.StructureInfo.IsCollapsed)
        return true;
    }
    return false;
  }
}
