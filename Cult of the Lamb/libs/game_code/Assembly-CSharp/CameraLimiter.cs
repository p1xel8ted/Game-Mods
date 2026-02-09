// Decompiled with JetBrains decompiler
// Type: CameraLimiter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CameraLimiter : BaseMonoBehaviour
{
  public Bounds LimitBounds;

  public void OnTriggerEnter2D(Collider2D collision)
  {
    if (!collision.gameObject.CompareTag("Player"))
      return;
    CameraFollowTarget instance = CameraFollowTarget.Instance;
    Bounds limitBounds = this.LimitBounds;
    limitBounds.center += this.transform.position;
    Bounds Limits = limitBounds;
    instance.SetCameraLimits(Limits);
  }

  public void OnTriggerExit2D(Collider2D collision)
  {
    if (!collision.gameObject.CompareTag("Player"))
      return;
    CameraFollowTarget.Instance.DisableCameraLimits();
  }

  public void OnDrawGizmos()
  {
    Gizmos.DrawWireCube(this.transform.position + this.LimitBounds.center, this.LimitBounds.size);
  }
}
