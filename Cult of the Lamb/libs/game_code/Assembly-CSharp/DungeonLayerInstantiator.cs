// Decompiled with JetBrains decompiler
// Type: DungeonLayerInstantiator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class DungeonLayerInstantiator : MonoBehaviour
{
  [SerializeField]
  public DungeonLayerInstantiator.LayerObject[] layerObjects;

  public void Start()
  {
    foreach (DungeonLayerInstantiator.LayerObject layerObject in this.layerObjects)
    {
      if (layerObject.Layer == GameManager.CurrentDungeonLayer)
        UnityEngine.Object.Instantiate<GameObject>(layerObject.Object, this.transform.position, Quaternion.identity, this.transform);
    }
  }

  [Serializable]
  public struct LayerObject
  {
    public int Layer;
    public GameObject Object;
  }
}
