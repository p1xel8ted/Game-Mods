// Decompiled with JetBrains decompiler
// Type: HUDManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class HUDManager : BaseMonoBehaviour
{
  public static HUDManager Instance;
  public RectTransform rectTransform;
  public Vector3 Offscreen = new Vector3(0.0f, 350f);
  public static bool isHiding;
  public RectTransform TopLeftUI;

  public void OnEnable()
  {
    HUDManager.Instance = this;
    this.rectTransform = this.GetComponent<RectTransform>();
  }

  public void OnDisable()
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
      this.StartCoroutine(this.DoHide());
  }

  public IEnumerator DoHide()
  {
    Vector3 speed = new Vector3(0.0f, -15f);
    Vector3 acceleration = new Vector3(0.0f, 2f);
    RectTransform rt = this.rectTransform;
    Vector3 pos = rt.localPosition;
    float targetY = this.Offscreen.y;
    while ((double) pos.y < (double) targetY)
    {
      speed += acceleration;
      pos += speed;
      rt.localPosition = pos;
      yield return (object) null;
    }
    rt.localPosition = this.Offscreen;
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
      this.StartCoroutine(this.DoShow());
  }

  public IEnumerator DoShow()
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
    this.StartCoroutine(this.TopLeftUIAddOffsetRoutine(Offset, Duration));
  }

  public IEnumerator TopLeftUIAddOffsetRoutine(Vector3 Offset, float Duration)
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
