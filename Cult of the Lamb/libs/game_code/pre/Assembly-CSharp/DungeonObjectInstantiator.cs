// Decompiled with JetBrains decompiler
// Type: DungeonObjectInstantiator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DungeonObjectInstantiator : BaseMonoBehaviour
{
  public List<DungeonObjectInstantiator.DecorationAndLocation> DecorationsAndLocations = new List<DungeonObjectInstantiator.DecorationAndLocation>();
  [SerializeField]
  private GameObject placeholderObj;
  private bool spawned;

  private void OnEnable() => LocationManager.OnPlayerLocationSet += new System.Action(this.Start);

  private void OnDisable() => LocationManager.OnPlayerLocationSet -= new System.Action(this.Start);

  private void Start()
  {
    if (this.spawned)
      return;
    this.placeholderObj?.gameObject.SetActive(false);
    foreach (DungeonObjectInstantiator.DecorationAndLocation decorationsAndLocation in this.DecorationsAndLocations)
    {
      if (decorationsAndLocation.Location == PlayerFarming.Location)
      {
        ObjectPool.Spawn(decorationsAndLocation.Decorations[UnityEngine.Random.Range(0, decorationsAndLocation.Decorations.Length)], this.transform);
        break;
      }
    }
    this.spawned = true;
  }

  [Serializable]
  public class DecorationAndLocation
  {
    public GameObject[] Decorations;
    public FollowerLocation Location;
  }
}
