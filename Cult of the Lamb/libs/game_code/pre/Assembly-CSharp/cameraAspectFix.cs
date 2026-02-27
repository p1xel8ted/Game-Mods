// Decompiled with JetBrains decompiler
// Type: cameraAspectFix
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (Camera))]
public class cameraAspectFix : BaseMonoBehaviour
{
  public bool copyMainCameraSettings;
  private Camera _referenceCamera;
  private Camera _thisCamera;

  private void Awake()
  {
    this._thisCamera = this.gameObject.GetComponent<Camera>();
    this._referenceCamera = Camera.main;
    this._thisCamera.aspect = Camera.main.aspect;
    this._thisCamera.nearClipPlane = Camera.main.nearClipPlane;
    this._thisCamera.farClipPlane = Camera.main.farClipPlane;
  }

  private void Update()
  {
    this._thisCamera.aspect = this._referenceCamera.aspect;
    this._thisCamera.nearClipPlane = this._referenceCamera.nearClipPlane;
    this._thisCamera.farClipPlane = this._referenceCamera.farClipPlane;
    if (!this.copyMainCameraSettings)
      return;
    this.transform.position = this._referenceCamera.transform.position;
  }
}
