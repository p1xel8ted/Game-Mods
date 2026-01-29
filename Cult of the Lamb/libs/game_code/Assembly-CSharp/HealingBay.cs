// Decompiled with JetBrains decompiler
// Type: HealingBay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class HealingBay : BaseMonoBehaviour
{
  public static List<HealingBay> HealingBays = new List<HealingBay>();
  public Structure Structure;
  public Transform HealingBayLocation;
  public GameObject HealingBayExitLocation;

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public void OnEnable() => HealingBay.HealingBays.Add(this);

  public void OnDisable() => HealingBay.HealingBays.Remove(this);
}
