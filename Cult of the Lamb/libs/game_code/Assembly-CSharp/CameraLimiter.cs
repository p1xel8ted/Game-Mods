// Decompiled with JetBrains decompiler
// Type: CameraLimiter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
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
