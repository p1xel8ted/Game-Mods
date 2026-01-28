// Decompiled with JetBrains decompiler
// Type: ProfileSlideAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class ProfileSlideAnimation : MonoBehaviour
{
  public float Duration;
  public float Delay;
  public Vector3 StartPosition;
  public Vector3 EndPosition;

  public void OnEnable()
  {
    this.transform.localPosition = this.StartPosition;
    this.StartCoroutine((IEnumerator) this.Animate());
  }

  public void OnDisable() => this.transform.localPosition = this.StartPosition;

  public IEnumerator Animate()
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
