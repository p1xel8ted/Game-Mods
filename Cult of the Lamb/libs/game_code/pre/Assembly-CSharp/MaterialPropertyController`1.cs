// Decompiled with JetBrains decompiler
// Type: MaterialPropertyController`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteAlways]
[DefaultExecutionOrder(-1000)]
public abstract class MaterialPropertyController<T> : MonoBehaviour
{
  [SerializeField]
  protected T _sourceComponent;
  protected Material _material;

  private void Awake() => this.SetupSource();

  private void OnValidate() => this.SetupSource();

  protected abstract void SetupSource();

  protected abstract void UpdateMaterialProperties();

  private void Update()
  {
    if (!((Object) this._material != (Object) null))
      return;
    this.UpdateMaterialProperties();
  }

  private void OnDestroy()
  {
    if (!Application.isPlaying)
      return;
    Object.Destroy((Object) this._material);
    this._material = (Material) null;
  }
}
