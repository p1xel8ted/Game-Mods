// Decompiled with JetBrains decompiler
// Type: FollowerFootsteps
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FollowerFootsteps : MonoBehaviour
{
  public Follower parentFollower;

  public void Awake() => this.parentFollower = this.GetComponentInParent<Follower>();

  public void PlayFootstep()
  {
    if ((Object) this.parentFollower != (Object) null && this.parentFollower.Brain != null && this.parentFollower.Brain._directInfoAccess.IsSnowman)
      AudioManager.Instance.PlayOneShot("event:/material/footstep_snow", this.transform.position);
    else
      AudioManager.Instance.PlayFootstep(this.transform.position);
  }
}
