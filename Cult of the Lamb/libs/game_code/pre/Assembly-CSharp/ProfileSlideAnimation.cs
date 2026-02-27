// Decompiled with JetBrains decompiler
// Type: ProfileSlideAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class ProfileSlideAnimation : MonoBehaviour
{
  public float Duration;
  public float Delay;
  public Vector3 StartPosition;
  public Vector3 EndPosition;

  private void OnEnable()
  {
    this.transform.localPosition = this.StartPosition;
    this.StartCoroutine((IEnumerator) this.Animate());
  }

  private void OnDisable() => this.transform.localPosition = this.StartPosition;

  private IEnumerator Animate()
  {
    ProfileSlideAnimation profileSlideAnimation = this;
    float timeElapsed = 0.0f;
    while ((double) timeElapsed < (double) profileSlideAnimation.Delay)
    {
      timeElapsed += Time.deltaTime;
      yield return (object) null;
    }
    while ((double) timeElapsed < (double) profileSlideAnimation.Duration + (double) profileSlideAnimation.Delay)
    {
      profileSlideAnimation.transform.localPosition = Vector3.Lerp(profileSlideAnimation.StartPosition, profileSlideAnimation.EndPosition, (timeElapsed - profileSlideAnimation.Delay) / profileSlideAnimation.Duration);
      timeElapsed += Time.deltaTime;
      yield return (object) null;
    }
    profileSlideAnimation.transform.localPosition = profileSlideAnimation.EndPosition;
  }
}
