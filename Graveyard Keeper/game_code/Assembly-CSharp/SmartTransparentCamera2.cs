// Decompiled with JetBrains decompiler
// Type: SmartTransparentCamera2
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (Camera))]
[ExecuteInEditMode]
public class SmartTransparentCamera2 : GJShaderEffect
{
  public SmartTransparentCamera camera_1;
  public Camera _camera_1;
  public Camera _c;
  public float range = 1f;

  public void Update()
  {
    if ((Object) this.camera_1 == (Object) null)
      return;
    if ((Object) this._camera_1 == (Object) null)
      this._camera_1 = this.camera_1.GetComponent<Camera>();
    if ((Object) this._c == (Object) null)
      this._c = this.GetComponent<Camera>();
    if ((int) this._camera_1.orthographicSize != (int) this._c.orthographicSize)
      this._c.orthographicSize = this._camera_1.orthographicSize;
    this._c.nearClipPlane = -2000f;
    this._c.farClipPlane = -this.camera_1.z_shift;
  }

  public override void SetValues(Material mat) => mat.SetFloat("_Range", this.range);
}
