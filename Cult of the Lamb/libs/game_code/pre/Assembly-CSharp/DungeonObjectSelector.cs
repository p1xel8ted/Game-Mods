// Decompiled with JetBrains decompiler
// Type: DungeonObjectSelector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using MMBiomeGeneration;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DungeonObjectSelector : BaseMonoBehaviour
{
  public List<DungeonObjectSelector.DecorationAndLocation> DecorationsAndLocations = new List<DungeonObjectSelector.DecorationAndLocation>();

  private void OnEnable() => LocationManager.OnPlayerLocationSet += new System.Action(this.Start);

  private void OnDisable() => LocationManager.OnPlayerLocationSet -= new System.Action(this.Start);

  private void Start()
  {
    foreach (DungeonObjectSelector.DecorationAndLocation decorationsAndLocation in this.DecorationsAndLocations)
    {
      if ((UnityEngine.Object) decorationsAndLocation.Decoration != (UnityEngine.Object) null)
        decorationsAndLocation.Decoration.SetActive(false);
    }
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

  [Serializable]
  public class DecorationAndLocation
  {
    public GameObject Decoration;
    public FollowerLocation Location;
  }
}
