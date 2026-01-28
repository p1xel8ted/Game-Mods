// Decompiled with JetBrains decompiler
// Type: Altar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Altar : BaseMonoBehaviour
{
  public List<Transform> NewRecruitPositions = new List<Transform>();
  public List<Transform> SacrificePositions = new List<Transform>();
  public GameObject CentrePoint;
  public static Altar Instance;

  public void OnEnable() => Altar.Instance = this;

  public void OnDisable() => Altar.Instance = (Altar) null;
}
