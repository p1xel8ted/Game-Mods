// Decompiled with JetBrains decompiler
// Type: DungeonObjectSelector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMBiomeGeneration;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DungeonObjectSelector : BaseMonoBehaviour
{
  public List<DungeonObjectSelector.DecorationAndLocation> DecorationsAndLocations = new List<DungeonObjectSelector.DecorationAndLocation>();

  public void OnEnable() => LocationManager.OnPlayerLocationSet += new System.Action(this.OnLoaded);

  public void OnDisable() => LocationManager.OnPlayerLocationSet -= new System.Action(this.OnLoaded);

  public void OnLoaded()
  {
    foreach (DungeonObjectSelector.DecorationAndLocation decorationsAndLocation in this.DecorationsAndLocations)
    {
      if ((UnityEngine.Object) BiomeGenerator.Instance == (UnityEngine.Object) null)
        break;
      if ((UnityEngine.Object) decorationsAndLocation.Decoration != (UnityEngine.Object) null && decorationsAndLocation.Location == BiomeGenerator.Instance.DungeonLocation)
      {
        decorationsAndLocation.Decoration.SetActive(true);
        break;
      }
    }
  }

  public void Start()
  {
    foreach (DungeonObjectSelector.DecorationAndLocation decorationsAndLocation in this.DecorationsAndLocations)
    {
      if ((UnityEngine.Object) decorationsAndLocation.Decoration != (UnityEngine.Object) null)
        decorationsAndLocation.Decoration.SetActive(false);
    }
    this.OnLoaded();
  }

  [Serializable]
  public class DecorationAndLocation
  {
    public GameObject Decoration;
    public FollowerLocation Location;
  }
}
