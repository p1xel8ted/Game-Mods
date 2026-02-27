// Decompiled with JetBrains decompiler
// Type: HUDManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class HUDManager : BaseMonoBehaviour
{
  public static HUDManager Instance;
  private RectTransform rectTransform;
  private Vector3 Offscreen = new Vector3(0.0f, 350f);
  public static bool isHiding;
  public RectTransform TopLeftUI;

  private void OnEnable()
  {
    HUDManager.Instance = this;
    this.rectTransform = this.GetComponent<RectTransform>();
  }

  private void OnDisable()
  {
    if (!((Object) HUDManager.Instance == (Object) this))
      return;
    HUDManager.Instance = (HUDManager) null;
  }

  public static void Hide(bool Snap)
  {
    if (!((Object) HUDManager.Instance != (Object) null))
      return;
    HUDManager.Instance.hide(Snap);
  }

  public void hide(bool Snap)
  {
    HUDManager.isHiding = true;
    this.StopAllCoroutines();
    if (Snap)
      this.rectTransform.localPosition = this.Offscreen;
    else
      this.StartCoroutine((IEnumerator) this.DoHide());
  }

  private IEnumerator DoHide()
  {
    Vector3 Speed = new Vector3(0.0f, -15f);
    while ((double) this.rectTransform.localPosition.y < (double) this.Offscreen.y)
    {
      Speed += new Vector3(0.0f, 2f);
      this.rectTransform.localPosition = this.rectTransform.localPosition + Speed;
      yield return (object) null;
    }
    this.rectTransform.localPosition = this.Offscreen;
  }

  public static void Show(bool Snap)
  {
    if (!((Object) HUDManager.Instance != (Object) null))
      return;
    HUDManager.Instance.show(Snap);
  }

  public void show(bool Snap)
  {
    this.StopAllCoroutines();
    if (Snap)
    {
      HUDManager.isHiding = false;
      this.rectTransform.localPosition = Vector3.zero;
    }
    else
      this.StartCoroutine((IEnumerator) this.DoShow());
  }

  private IEnumerator DoShow()
  {
    Vector3 vector3 = new Vector3(0.0f, 15f);
    float Timer = 0.0f;
    while ((double) (Timer += Time.unscaledDeltaTime) < 1.0)
    {
      this.rectTransform.localPosition = Vector3.Lerp(this.rectTransform.localPosition, Vector3.zero, 15f * Time.unscaledDeltaTime);
      yield return (object) null;
    }
    this.rectTransform.localPosition = Vector3.zero;
    HUDManager.isHiding = false;
  }

  public void TopLeftUISetOffset(Vector3 Offset, float Duration)
  {
    this.StartCoroutine((IEnumerator) this.TopLeftUIAddOffsetRoutine(Offset, Duration));
  }

  private IEnumerator TopLeftUIAddOffsetRoutine(Vector3 Offset, float Duration)
  {
    float Progress = 0.0f;
    Vector3 StartPosition = this.TopLeftUI.localPosition;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      this.TopLeftUI.localPosition = Vector3.Lerp(StartPosition, Offset, Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
      yield return (object) null;
    }
  }
}
