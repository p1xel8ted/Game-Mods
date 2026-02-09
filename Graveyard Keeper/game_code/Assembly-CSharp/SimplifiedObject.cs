// Decompiled with JetBrains decompiler
// Type: SimplifiedObject
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public abstract class SimplifiedObject : MonoBehaviour
{
  [SerializeField]
  public int sg_grid_divider;
  [SerializeField]
  public int sg_fine_tune_z;
  [SerializeField]
  public int go_layer;
  [SerializeField]
  public int spr_sorting_layer_id;
  [SerializeField]
  public int spr_sorting_order;
  [NonSerialized]
  public ChunkedGameObject chunk;

  public virtual GameObject Restore() => (GameObject) null;

  public void CommonRestore(GameObject o)
  {
    o.layer = this.go_layer;
    o.name = this.name;
    SnapToGridComponent component1 = o.GetComponent<SnapToGridComponent>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
    {
      component1.grid_divider = this.sg_grid_divider;
      component1.fine_tune_z = this.sg_fine_tune_z;
    }
    SpriteRenderer component2 = o.GetComponent<SpriteRenderer>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
    {
      component2.sortingLayerID = this.spr_sorting_layer_id;
      component2.sortingOrder = this.spr_sorting_order;
    }
    this.chunk = o.GetComponent<ChunkedGameObject>();
    if (!((UnityEngine.Object) this.chunk != (UnityEngine.Object) null))
      return;
    this.chunk.Start();
  }

  public static void RestoreAll(Transform root)
  {
    SimplifiedObject[] componentsInChildren = root.GetComponentsInChildren<SimplifiedObject>(true);
    Debug.Log((object) ("Restoring objects on scene: " + componentsInChildren.Length.ToString()));
    foreach (SimplifiedObject simplifiedObject in componentsInChildren)
      simplifiedObject.Restore();
  }
}
