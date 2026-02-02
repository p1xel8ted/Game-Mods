// Decompiled with JetBrains decompiler
// Type: HUD_TimerToken
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class HUD_TimerToken : BaseMonoBehaviour
{
  public Image CircleImage;
  public Image HourGlassImage;
  public Image PauseImage;
  public RectTransform rectTransform;
  public bool FlashWhenActive;
  public Coroutine cWarningFlash;

  public float fillAmount
  {
    get => this.CircleImage.fillAmount;
    set
    {
      if (this.gameObject.activeSelf && this.FlashWhenActive)
      {
        if ((double) value < 1.0 && this.cWarningFlash == null)
          this.cWarningFlash = this.StartCoroutine((IEnumerator) this.WarningFlash());
        if ((double) value >= 1.0 && this.cWarningFlash != null)
        {
          this.StopCoroutine(this.cWarningFlash);
          this.CircleImage.color = Color.white;
          this.CircleImage.rectTransform.localScale = Vector3.one;
        }
      }
      this.CircleImage.fillAmount = value;
    }
  }

  public bool TimerPaused
  {
    set
    {
      if (value)
      {
        this.HourGlassImage.enabled = false;
        this.PauseImage.enabled = true;
      }
      else
      {
        this.HourGlassImage.enabled = true;
        this.PauseImage.enabled = false;
      }
    }
  }

  public void OnEnable()
  {
    HUD_Timer.OnPauseTimer += new HUD_Timer.PauseTimer(this.OnPauseTimer);
    HUD_Timer.OnUnPauseTimer += new HUD_Timer.UnPauseTimer(this.OnUnPauseTimer);
    this.StartCoroutine((IEnumerator) this.DoScale());
    this.TimerPaused = HUD_Timer.TimerPaused;
  }

  public IEnumerator WarningFlash()
  {
    bool Loop = true;
    float FlashRed = 180f;
    float FlashScale = 180f;
    while (Loop)
    {
      this.CircleImage.color = Color.Lerp(Color.white, Color.red, (float) (0.5 + 0.5 * (double) Mathf.Cos(FlashRed += Time.deltaTime * 4f)));
      this.CircleImage.rectTransform.localScale = Vector3.one * (float) (1.0 + 0.20000000298023224 * (double) Mathf.Cos(FlashScale += Time.deltaTime * 5f));
      if ((double) this.fillAmount < 1.0)
        yield return (object) null;
      else
        Loop = false;
    }
    this.CircleImage.color = Color.white;
    this.CircleImage.rectTransform.localScale = Vector3.one;
    this.cWarningFlash = (Coroutine) null;
  }

  public void OnDisable()
  {
    HUD_Timer.OnPauseTimer -= new HUD_Timer.PauseTimer(this.OnPauseTimer);
    HUD_Timer.OnUnPauseTimer -= new HUD_Timer.UnPauseTimer(this.OnUnPauseTimer);
    this.StopAllCoroutines();
  }

  public void OnPauseTimer() => this.TimerPaused = true;

  public void OnUnPauseTimer() => this.TimerPaused = false;

  public IEnumerator DoScale()
  {
    float Scale = 0.0f;
    float ScaleSpeed = 0.0f;
    float Timer = 0.0f;
    this.rectTransform.localScale = Vector3.one * Scale;
    while ((double) (Timer += Time.deltaTime) < 2.0)
    {
      ScaleSpeed += (float) ((1.0 - (double) Scale) * 0.40000000596046448);
      Scale += (ScaleSpeed *= 0.6f);
      this.rectTransform.localScale = Vector3.one * Scale;
      yield return (object) null;
    }
  }
}
