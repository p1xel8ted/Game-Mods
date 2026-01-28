// Decompiled with JetBrains decompiler
// Type: DLCLandController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DLCLandController : MonoBehaviour
{
  public static DLCLandController Instance;
  [SerializeField]
  public GameObject[] dlcLandDisabled;
  [SerializeField]
  public GameObject[] dlcLandEnabled;
  [SerializeField]
  public DLCLandController.LandSlot[] landSlots;

  public void Awake() => DLCLandController.Instance = this;

  public void OnDestroy() => DLCLandController.Instance = (DLCLandController) null;

  public void SetDefault()
  {
    if (Application.isPlaying)
      DataManager.Instance.LandPurchased = -1;
    foreach (DLCLandController.LandSlot landSlot in this.landSlots)
      landSlot.Land.gameObject.SetActive(false);
    foreach (GameObject gameObject in this.dlcLandDisabled)
      gameObject.gameObject.SetActive(true);
    foreach (GameObject gameObject in this.dlcLandEnabled)
      gameObject.gameObject.SetActive(false);
  }

  public void DEBUG_ShowSlot(int slot)
  {
    if (Application.isPlaying)
      DataManager.Instance.LandPurchased = slot;
    this.ShowSlot(slot);
  }

  public void ShowSlot(int slot)
  {
    foreach (GameObject gameObject in this.dlcLandDisabled)
      gameObject.gameObject.SetActive(false);
    foreach (GameObject gameObject in this.dlcLandEnabled)
      gameObject.gameObject.SetActive(true);
    foreach (DLCLandController.LandSlot landSlot in this.landSlots)
      landSlot.Land.gameObject.SetActive(false);
    this.landSlots[slot].Land.gameObject.SetActive(true);
    if (!Application.isPlaying || DataManager.Instance.LandPurchased == -1)
      return;
    BiomeBaseManager.Instance.Room.Pieces[0].Collider = this.CombineColliders(BiomeBaseManager.Instance.CachedColliderPoints, BiomeBaseManager.Instance.Room.Pieces[0].Collider);
    BiomeBaseManager.Instance.Room.SetColliderAndUpdatePathfinding();
    Follower.Points = new Vector2[0];
  }

  public DLCLandController.LandSlot GetLandSlot(int slot) => this.landSlots[slot];

  public PolygonCollider2D CombineColliders(Vector2[] points, PolygonCollider2D collider)
  {
    if (DataManager.Instance.LandPurchased == -1)
      return collider;
    List<Vector2> vector2List = new List<Vector2>();
    vector2List.AddRange((IEnumerable<Vector2>) points);
    DLCLandController.LandSlot landSlot = this.landSlots[DataManager.Instance.LandPurchased];
    for (int index = 0; index < landSlot.FullCollider.pathCount; ++index)
    {
      foreach (Vector2 position in landSlot.FullCollider.GetPath(index))
        vector2List.Add((Vector2) collider.transform.InverseTransformPoint((Vector3) position));
    }
    collider.pathCount = 1;
    collider.SetPath(0, vector2List.ToArray());
    return collider;
  }

  public void ShowBridge()
  {
  }

  public void HideBridge()
  {
  }

  [Serializable]
  public class LandSlot
  {
    public PolygonCollider2D SegmentCollider;
    public PolygonCollider2D FullCollider;
    public GameObject Land;
    public List<PlacementRegion.ResourcesAndCount> ResourcesToPlace;
  }
}
