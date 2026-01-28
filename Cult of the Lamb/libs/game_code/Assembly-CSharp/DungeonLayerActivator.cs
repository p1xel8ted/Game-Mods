// Decompiled with JetBrains decompiler
// Type: DungeonLayerActivator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
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
