// Decompiled with JetBrains decompiler
// Type: TriggerCanvasGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class TriggerCanvasGroup : BaseMonoBehaviour
{
  public TriggerCanvasGroup.Mode CurrentMode;
  public float FadeIn = 1f;
  public float FadeOut = 1f;
  public float WaitDuration = 5f;
  public bool Activated;
  public CanvasGroup CanvasGroup;
  public bool StartHidden = true;
  public string sfx;
  public UnityEvent eventTrigger;
  public TriggerCanvasGroup.Triggered OnTriggered;

  public void Start()
  {
    if (!this.StartHidden || !((Object) this.CanvasGroup != (Object) null))
      return;
    this.CanvasGroup.alpha = 0.0f;
  }

  public void OnTriggerEnter2D(Collider2D collision)
  {
    if (this.Activated || !collision.gameObject.CompareTag("Player"))
      return;
    this.Activated = true;
    this.Play();
  }

  public void Play()
  {
    TriggerCanvasGroup.Triggered onTriggered = this.OnTriggered;
    if (onTriggered != null)
      onTriggered();
    this.eventTrigger?.Invoke();
    if ((Object) this.CanvasGroup == (Object) null)
      return;
    switch (this.CurrentMode)
    {
      case TriggerCanvasGroup.Mode.Timer:
        this.StartCoroutine((IEnumerator) this.DoRoutine());
        break;
      case TriggerCanvasGroup.Mode.Enable:
        this.CanvasGroup.DOFade(1f, this.FadeIn);
        break;
      case TriggerCanvasGroup.Mode.Disable:
        this.CanvasGroup.DOFade(0.0f, this.FadeIn);
        break;
    }
  }

  public IEnumerator EnableRoutine()
  {
    float Progress = 0.0f;
    float Duration = this.FadeIn;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      this.CanvasGroup.alpha = Mathf.Lerp(0.0f, 1f, Progress / Duration);
      yield return (object) null;
    }
    this.CanvasGroup.alpha = 1f;
  }

  public IEnumerator DisableRoutine()
  {
    float Progress = 0.0f;
    float Duration = this.FadeOut;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      this.CanvasGroup.alpha = Mathf.Lerp(1f, 0.0f, Progress / Duration);
      yield return (object) null;
    }
    this.CanvasGroup.alpha = 0.0f;
  }

  public IEnumerator DoRoutine()
  {
    this.CanvasGroup.DOFade(1f, this.FadeIn);
    AudioManager.Instance.PlayOneShot(this.sfx);
    yield return (object) new WaitForSeconds(this.WaitDuration);
    this.CanvasGroup.DOFade(0.0f, this.FadeIn);
  }

  public enum Mode
  {
    Timer,
    Enable,
    Disable,
  }

  public delegate void Triggered();
}
