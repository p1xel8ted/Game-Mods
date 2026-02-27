// Decompiled with JetBrains decompiler
// Type: CameraLimiter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CameraLimiter : BaseMonoBehaviour
{
  public Bounds LimitBounds;

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (!(collision.gameObject.tag == "Player"))
      return;
    CameraFollowTarget instance = CameraFollowTarget.Instance;
    Bounds limitBounds = this.LimitBounds;
    limitBounds.center += this.transform.position;
    Bounds Limits = limitBounds;
    instance.SetCameraLimits(Limits);
  }

  private void OnTriggerExit2D(Collider2D collision)
  {
    if (!(collision.gameObject.tag == "Player"))
      return;
    CameraFollowTarget.Instance.DisableCameraLimits();
  }

  private void OnDrawGizmos()
  {
    Gizmos.DrawWireCube(this.transform.position + this.LimitBounds.center, this.LimitBounds.size);
  }
}
