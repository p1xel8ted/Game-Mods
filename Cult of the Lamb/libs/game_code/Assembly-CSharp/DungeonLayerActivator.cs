// Decompiled with JetBrains decompiler
// Type: DungeonLayerActivator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class DungeonLayerActivator : MonoBehaviour
{
  [SerializeField]
  public DungeonLayerActivator.LayerObject[] layerObjects;

  public void Start()
  {
    foreach (DungeonLayerActivator.LayerObject layerObject in this.layerObjects)
      layerObject.Object.SetActive(false);
    foreach (DungeonLayerActivator.LayerObject layerObject in this.layerObjects)
      layerObject.Object.SetActive(layerObject.Layer == GameManager.CurrentDungeonLayer || GameManager.CurrentDungeonLayer >= layerObject.Layer && layerObject.KeepActiveIfLayerIsGreaterOrEqual);
  }

  [Serializable]
  public struct LayerObject
  {
    public int Layer;
    public GameObject Object;
    public bool KeepActiveIfLayerIsGreaterOrEqual;
  }
}
