// Decompiled with JetBrains decompiler
// Type: CameraWeatherVFX_Follower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class CameraWeatherVFX_Follower : MonoBehaviour
{
  [SerializeField]
  public Camera targetCamera;
  [SerializeField]
  public float frustumDepth = 100f;

  public Vector3 CurrentLocation => this.transform.position;

  public Bounds FrustumBounds
  {
    get
    {
      if ((UnityEngine.Object) this.targetCamera == (UnityEngine.Object) null)
        this.targetCamera = Camera.main;
      return (UnityEngine.Object) this.targetCamera == (UnityEngine.Object) null ? new Bounds(this.transform.position, Vector3.one) : this.CalculateFrustumBounds();
    }
  }

  public Vector3 FrustumSize => this.FrustumBounds.size;

  public Bounds CalculateFrustumBounds()
  {
    float frustumDepth = this.frustumDepth;
    float f = (float) ((double) this.targetCamera.fieldOfView * 0.5 * (Math.PI / 180.0));
    float aspect = this.targetCamera.aspect;
    float num1 = frustumDepth * Mathf.Tan(f);
    float num2 = num1 * aspect;
    return new Bounds(this.targetCamera.transform.position + this.targetCamera.transform.forward * (frustumDepth * 0.5f), new Vector3(num2 * 2f, num1 * 2f, frustumDepth));
  }

  public void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.yellow;
    Bounds frustumBounds = this.FrustumBounds;
    Gizmos.matrix = Matrix4x4.TRS(frustumBounds.center, (UnityEngine.Object) this.targetCamera != (UnityEngine.Object) null ? this.targetCamera.transform.rotation : this.transform.rotation, Vector3.one);
    Vector3 zero = Vector3.zero;
    frustumBounds = this.FrustumBounds;
    Vector3 size = frustumBounds.size;
    Gizmos.DrawWireCube(zero, size);
  }
}
