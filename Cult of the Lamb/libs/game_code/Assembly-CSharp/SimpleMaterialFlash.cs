// Decompiled with JetBrains decompiler
// Type: SimpleMaterialFlash
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class SimpleMaterialFlash : MonoBehaviour
{
  public Material material;
  public Color baseColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
  public Color warningColor = Color.white;
  public static int FillColorID = Shader.PropertyToID("_FillColor");
  public static int FillAlphaID = Shader.PropertyToID("_FillAlpha");
  public static int TintColorID = Shader.PropertyToID("_Color");
  public Coroutine fadeRoutine;
  public Coroutine flashRoutine;
  public bool isFillWhite;
  public bool flashingRed;

  public void Start()
  {
    if (!((Object) this.material != (Object) null))
      return;
    this.baseColor = this.material.GetColor(SimpleMaterialFlash.FillColorID);
    this.baseColor.a = this.material.GetFloat(SimpleMaterialFlash.FillAlphaID);
  }

  public void Flash(Color color, float duration)
  {
    if (!SettingsManager.Settings.Accessibility.FlashingLights || (Object) this.material == (Object) null)
      return;
    if (this.flashRoutine != null)
      this.StopCoroutine(this.flashRoutine);
    this.flashRoutine = this.StartCoroutine((IEnumerator) this.FlashRoutine(color, duration));
  }

  public IEnumerator FlashRoutine(Color color, float duration)
  {
    float t = 0.0f;
    Color originalColor = this.material.GetColor(SimpleMaterialFlash.FillColorID);
    float originalAlpha = this.material.GetFloat(SimpleMaterialFlash.FillAlphaID);
    while ((double) t < (double) duration)
    {
      float t1 = t / duration;
      this.material.SetColor(SimpleMaterialFlash.FillColorID, Color.Lerp(color, originalColor, t1));
      this.material.SetFloat(SimpleMaterialFlash.FillAlphaID, Mathf.Lerp(1f, originalAlpha, t1));
      t += Time.deltaTime;
      yield return (object) null;
    }
  }

  public void FlashMeWhite(float alpha = 0.5f, int frameCount = 5)
  {
    if (Time.frameCount % frameCount != 0)
      return;
    this.FlashWhite(!this.isFillWhite, alpha);
  }

  public void FlashWhite(bool toggle, float alpha = 0.5f)
  {
    if (!SettingsManager.Settings.Accessibility.FlashingLights || this.flashingRed || (Object) this.material == (Object) null)
      return;
    this.SetColor(this.warningColor with
    {
      a = toggle ? alpha : 0.0f
    });
    this.isFillWhite = toggle;
  }

  public void FlashWhite(float intensity)
  {
    if (!SettingsManager.Settings.Accessibility.FlashingLights || this.flashingRed || (Object) this.material == (Object) null)
      return;
    this.isFillWhite = (double) intensity > 0.0;
    this.SetColor(this.warningColor with
    {
      a = Mathf.Clamp01(intensity * 0.44f)
    });
  }

  public void FlashRed(float intensity)
  {
    if (!SettingsManager.Settings.Accessibility.FlashingLights || (Object) this.material == (Object) null)
      return;
    this.SetColor(new Color(1f, 0.0f, 0.0f, Mathf.Clamp01(intensity)));
  }

  public void FlashFillRed(float opacity = 0.5f)
  {
    if (!SettingsManager.Settings.Accessibility.FlashingLights || (Object) this.material == (Object) null)
      return;
    this.flashingRed = true;
    this.StartCoroutine((IEnumerator) this.FlashFillSequence(opacity));
  }

  public IEnumerator FlashFillSequence(float opacity)
  {
    yield return (object) this.FlashColorStep(Color.white, opacity, 0.06f);
    yield return (object) this.FlashColorStep(Color.black, opacity, 0.03f);
    yield return (object) this.FlashColorStep(Color.red, opacity, 0.02f);
    yield return (object) this.FlashColorStep(Color.black, opacity, 0.02f);
    yield return (object) this.FlashColorStep(Color.red, opacity, 0.02f);
    this.SetColor(new Color(1f, 0.0f, 0.0f, 0.0f));
    this.flashingRed = false;
  }

  public IEnumerator FlashColorStep(Color color, float alpha, float duration)
  {
    color.a = alpha;
    this.SetColor(color);
    yield return (object) new WaitForSeconds(duration);
  }

  public void ResetColor()
  {
    if ((Object) this.material == (Object) null)
      return;
    this.SetColor(this.baseColor, new float?(0.0f));
    this.flashingRed = false;
  }

  public void FlashFill(Color color, float duration = 0.1f)
  {
    if (!SettingsManager.Settings.Accessibility.FlashingLights || (Object) this.material == (Object) null)
      return;
    this.StartCoroutine((IEnumerator) this.DoTimedFade(color, duration));
  }

  public IEnumerator DoTimedFade(Color color, float duration)
  {
    this.SetColor(color);
    yield return (object) new WaitForSeconds(duration);
    float progress = 1f;
    while ((double) (progress -= 0.05f) > 0.0)
    {
      color.a = progress;
      this.SetColor(color);
      yield return (object) null;
    }
  }

  public void Tint(Color color)
  {
    if (!SettingsManager.Settings.Accessibility.FlashingLights || (Object) this.material == (Object) null)
      return;
    if (this.fadeRoutine != null)
      this.StopCoroutine(this.fadeRoutine);
    this.fadeRoutine = this.StartCoroutine((IEnumerator) this.FadeTint(color));
  }

  public IEnumerator FadeTint(Color targetColor)
  {
    Color currentColor = this.material.GetColor(SimpleMaterialFlash.TintColorID);
    float duration = 0.1f;
    float t = 0.0f;
    while ((double) t < (double) duration)
    {
      float t1 = t / duration;
      this.material.SetColor(SimpleMaterialFlash.TintColorID, Color.Lerp(currentColor, targetColor, t1));
      t += Time.deltaTime;
      yield return (object) null;
    }
    this.material.SetColor(SimpleMaterialFlash.TintColorID, targetColor);
  }

  public void SetColor(Color color, float? overrideAlpha = null)
  {
    if ((Object) this.material == (Object) null)
      return;
    this.material.SetColor(SimpleMaterialFlash.FillColorID, color);
    this.material.SetFloat(SimpleMaterialFlash.FillAlphaID, (float) ((double) overrideAlpha ?? (double) color.a));
  }

  public void OverrideBaseColor(Color color) => this.baseColor = color;
}
