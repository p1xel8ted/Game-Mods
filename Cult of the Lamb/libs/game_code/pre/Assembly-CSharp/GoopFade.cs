// Decompiled with JetBrains decompiler
// Type: GoopFade
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class GoopFade : BaseMonoBehaviour
{
  public Image image;
  private float Duration = 2f;
  private float Delay;
  public float value;
  private bool UseDeltaTime = true;
  private Material clonedMaterial;

  private void Start()
  {
    this.clonedMaterial = new Material(this.image.materialForRendering);
    this.image.material = this.clonedMaterial;
  }

  public void FadeIn(float _Duration = 2f, float _Delay = 0.0f, bool UseDeltaTime = true)
  {
    this.image.enabled = true;
    this.UseDeltaTime = UseDeltaTime;
    this.Duration = _Duration;
    this.Delay = _Delay;
    this.StartCoroutine((IEnumerator) this.FadeInRoutine());
  }

  public void FadeOut(float _Duration = 2f, float _Delay = 0.0f, bool UseDeltaTime = true)
  {
    this.image.enabled = true;
    this.UseDeltaTime = UseDeltaTime;
    this.Duration = _Duration;
    this.Delay = _Delay;
    this.StartCoroutine((IEnumerator) this.FadeOutRoutine());
  }

  private IEnumerator FadeInRoutine()
  {
    if (this.UseDeltaTime)
      yield return (object) new WaitForSeconds(this.Delay);
    else
      yield return (object) new WaitForSecondsRealtime(this.Delay);
    float Progress = 0.0f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) this.Duration)
    {
      this.image.materialForRendering.SetFloat("_RectMaskCutoff", Mathf.SmoothStep(0.85f, 0.3f, Progress / this.Duration));
      this.value = Mathf.SmoothStep(1f, 0.0f, Progress / this.Duration);
      yield return (object) null;
    }
  }

  private IEnumerator FadeOutRoutine()
  {
    GoopFade goopFade = this;
    if (goopFade.UseDeltaTime)
      yield return (object) new WaitForSeconds(goopFade.Delay);
    else
      yield return (object) new WaitForSecondsRealtime(goopFade.Delay);
    float Progress = 0.0f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) goopFade.Duration)
    {
      goopFade.image.materialForRendering.SetFloat("_RectMaskCutoff", Mathf.SmoothStep(0.3f, 0.85f, Progress / goopFade.Duration));
      goopFade.value = Mathf.SmoothStep(0.0f, 1f, Progress / goopFade.Duration);
      yield return (object) null;
    }
    goopFade.gameObject.SetActive(false);
  }

  private void OnEnable()
  {
    this.image.enabled = false;
    this.image.materialForRendering.SetFloat("_RectMaskCutoff", 1f);
  }
}
