// Decompiled with JetBrains decompiler
// Type: DevotionCounterOverlay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class DevotionCounterOverlay : BaseMonoBehaviour
{
  private RectTransform rectTransform;
  private HUD_Souls HUD_Souls;

  private void Start()
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

  private IEnumerator PlayRoutine()
  {
    float Duration = 0.5f;
    float Progress = 0.0f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      this.rectTransform.anchoredPosition = Vector2.Lerp(new Vector2(0.0f, -150f), new Vector2(0.0f, 150f), Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
      yield return (object) null;
    }
  }

  private IEnumerator HideRoutine()
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
