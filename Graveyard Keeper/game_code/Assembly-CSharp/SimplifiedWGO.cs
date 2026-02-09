// Decompiled with JetBrains decompiler
// Type: SimplifiedWGO
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SimplifiedWGO : SimplifiedObject
{
  public float floor_line;
  public int fine_tune_z;
  public SerializableWGO swgo;
  public bool _restored;

  [ContextMenu("Restore")]
  public override GameObject Restore()
  {
    if (this._restored)
      return (GameObject) null;
    RoundAndSortComponent andSortComponent = this.gameObject.AddComponent<RoundAndSortComponent>();
    this.gameObject.AddComponent<ChunkedGameObject>();
    andSortComponent.floor_line = this.floor_line;
    andSortComponent.fine_tune_z = this.fine_tune_z;
    WorldGameObject worldGameObject = this.gameObject.AddComponent<WorldGameObject>();
    worldGameObject.ForceDeinitComponents();
    worldGameObject.tf = worldGameObject.transform;
    this.CommonRestore(this.gameObject);
    worldGameObject.RestoreFromSerializedObject(this.swgo, false);
    if (Application.isPlaying)
      Object.Destroy((Object) this);
    else
      Object.DestroyImmediate((Object) this);
    this._restored = true;
    return worldGameObject.gameObject;
  }

  public void Start()
  {
  }
}
