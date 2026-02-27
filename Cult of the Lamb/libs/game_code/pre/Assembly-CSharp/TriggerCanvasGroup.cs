// Decompiled with JetBrains decompiler
// Type: TriggerCanvasGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using System.Collections;
using UnityEngine;

#nullable disable
public class TriggerCanvasGroup : BaseMonoBehaviour
{
  public TriggerCanvasGroup.Mode CurrentMode;
  public float FadeIn = 1f;
  public float FadeOut = 1f;
  public float WaitDuration = 5f;
  private bool Activated;
  public CanvasGroup CanvasGroup;
  public bool StartHidden = true;
  public TriggerCanvasGroup.Triggered OnTriggered;

  private void Start()
  {
    if (!this.StartHidden || !((Object) this.CanvasGroup != (Object) null))
      return;
    this.CanvasGroup.alpha = 0.0f;
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (this.Activated || !(collision.gameObject.tag == "Player"))
      return;
    this.Activated = true;
    this.Play();
  }

  public void Play()
  {
    TriggerCanvasGroup.Triggered onTriggered = this.OnTriggered;
    if (onTriggered != null)
      onTriggered();
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

  private IEnumerator EnableRoutine()
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

  private IEnumerator DisableRoutine()
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

  private IEnumerator DoRoutine()
  {
    this.CanvasGroup.DOFade(1f, this.FadeIn);
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
