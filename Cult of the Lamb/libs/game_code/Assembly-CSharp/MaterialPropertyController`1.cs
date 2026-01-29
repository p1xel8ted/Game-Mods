// Decompiled with JetBrains decompiler
// Type: MaterialPropertyController`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteAlways]
[DefaultExecutionOrder(-1000)]
public abstract class MaterialPropertyController<T> : MonoBehaviour
{
  [SerializeField]
  public T _sourceComponent;
  public Material _material;

  public void Awake() => this.SetupSource();

  public abstract void SetupSource();

  public abstract void UpdateMaterialProperties();

  public void Update()
  {
    if (!((Object) this._material != (Object) null))
      return;
    this.UpdateMaterialProperties();
  }

  public void OnDestroy()
  {
    if (!Application.isPlaying)
      return;
    Object.Destroy((Object) this._material);
    this._material = (Material) null;
  }
}
