// Decompiled with JetBrains decompiler
// Type: cameraAspectFix
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (Camera))]
public class cameraAspectFix : BaseMonoBehaviour
{
  public bool copyMainCameraSettings;
  public Camera _referenceCamera;
  public Camera _thisCamera;

  public void Awake()
  {
    this._thisCamera = this.gameObject.GetComponent<Camera>();
    this._referenceCamera = Camera.main;
    this._thisCamera.aspect = Camera.main.aspect;
    this._thisCamera.nearClipPlane = Camera.main.nearClipPlane;
    this._thisCamera.farClipPlane = Camera.main.farClipPlane;
  }

  public void Update()
  {
    this._thisCamera.aspect = this._referenceCamera.aspect;
    this._thisCamera.nearClipPlane = this._referenceCamera.nearClipPlane;
    this._thisCamera.farClipPlane = this._referenceCamera.farClipPlane;
    if (!this.copyMainCameraSettings)
      return;
    this.transform.position = this._referenceCamera.transform.position;
  }
}
