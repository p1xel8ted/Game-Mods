// Decompiled with JetBrains decompiler
// Type: DungeonLayerInstantiator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class DungeonLayerInstantiator : MonoBehaviour
{
  [SerializeField]
  private DungeonLayerInstantiator.LayerObject[] layerObjects;

  private void Start()
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
