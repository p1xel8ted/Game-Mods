// Decompiled with JetBrains decompiler
// Type: DevotionCounterOverlay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class DevotionCounterOverlay : BaseMonoBehaviour
{
  public RectTransform rectTransform;
  public HUD_Souls HUD_Souls;

  public void Start()
  {
    this.rectTransform = this.GetComponent<RectTransform>();
    this.HUD_Souls = this.GetComponent<HUD_Souls>();
    this.gameObject.SetActive(false);
  }

  public void Play()
  {
    this.gameObject.SetActive(true);
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.PlayRoutine());
  }

  public void Hide()
  {
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.HideRoutine());
  }

  public IEnumerator PlayRoutine()
  {
    float Duration = 0.5f;
    float Progress = 0.0f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      this.rectTransform.anchoredPosition = Vector2.Lerp(new Vector2(0.0f, -150f), new Vector2(0.0f, 150f), Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
      yield return (object) null;
    }
  }

  public IEnumerator HideRoutine()
  {
    DevotionCounterOverlay devotionCounterOverlay = this;
    bool StillCounting = devotionCounterOverlay.HUD_Souls.CurrentDelta > 0;
    while (devotionCounterOverlay.HUD_Souls.CurrentDelta > 0)
      yield return (object) null;
    yield return (object) new WaitForSeconds(StillCounting ? 0.0f : 2f);
    float Duration = 0.4f;
    float Progress = 0.0f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      devotionCounterOverlay.rectTransform.anchoredPosition = Vector2.Lerp(new Vector2(0.0f, 150f), new Vector2(0.0f, -150f), Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
      yield return (object) null;
    }
    devotionCounterOverlay.gameObject.SetActive(false);
  }
}
