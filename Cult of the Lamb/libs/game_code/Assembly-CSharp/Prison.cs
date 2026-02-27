// Decompiled with JetBrains decompiler
// Type: Prison
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
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
