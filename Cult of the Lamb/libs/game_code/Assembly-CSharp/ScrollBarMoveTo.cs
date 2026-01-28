// Decompiled with JetBrains decompiler
// Type: ScrollBarMoveTo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
[RequireComponent(typeof (ScrollRect))]
public class ScrollBarMoveTo : BaseMonoBehaviour
{
  public RectTransform maskTransform;
  public ScrollRect mScrollRect;
  public RectTransform mScrollTransform;
  public RectTransform mContent;

  public IEnumerator CenterOnItem(RectTransform target)
  {
    Vector3 worldPointInWidget = this.GetWorldPointInWidget(this.mScrollTransform, this.GetWidgetWorldPoint(target));
    Vector3 vector3 = (this.GetWorldPointInWidget(this.mScrollTransform, this.GetWidgetWorldPoint(this.maskTransform)) - worldPointInWidget) with
    {
      z = 0.0f
    };
    if (!this.mScrollRect.horizontal)
      vector3.x = 0.0f;
    if (!this.mScrollRect.vertical)
      vector3.y = 0.0f;
    Vector2 vector2;
    ref Vector2 local = ref vector2;
    double x1 = (double) vector3.x;
    Rect rect = this.mContent.rect;
    double x2 = (double) rect.size.x;
    rect = this.mScrollTransform.rect;
    double x3 = (double) rect.size.x;
    double num1 = x2 - x3;
    double x4 = x1 / num1;
    double y1 = (double) vector3.y;
    rect = this.mContent.rect;
    double y2 = (double) rect.size.y;
    rect = this.mScrollTransform.rect;
    double y3 = (double) rect.size.y;
    double num2 = y2 - y3;
    double y4 = y1 / num2;
    local = new Vector2((float) x4, (float) y4);
    Vector2 newNormalizedPosition = this.mScrollRect.normalizedPosition - vector2;
    if (this.mScrollRect.movementType != ScrollRect.MovementType.Unrestricted)
    {
      newNormalizedPosition.x = Mathf.Clamp01(newNormalizedPosition.x);
      newNormalizedPosition.y = Mathf.Clamp01(newNormalizedPosition.y);
    }
    Vector3 StartPos = (Vector3) this.mScrollRect.normalizedPosition;
    float timer = 0.0f;
    float Duration = 0.25f;
    while ((double) timer < (double) Duration)
    {
      this.mScrollRect.normalizedPosition = (Vector2) Vector3.Lerp(StartPos, (Vector3) newNormalizedPosition, timer / Duration);
      timer += Time.deltaTime;
      yield return (object) null;
    }
    this.mScrollRect.normalizedPosition = newNormalizedPosition;
  }

  public void Awake()
  {
    this.mScrollRect = this.GetComponent<ScrollRect>();
    this.mScrollTransform = this.mScrollRect.transform as RectTransform;
    this.mContent = this.mScrollRect.content;
    this.Reset();
  }

  public void Reset()
  {
    if (!((Object) this.maskTransform == (Object) null))
      return;
    Mask componentInChildren1 = this.GetComponentInChildren<Mask>(true);
    if ((bool) (Object) componentInChildren1)
      this.maskTransform = componentInChildren1.rectTransform;
    if (!((Object) this.maskTransform == (Object) null))
      return;
    RectMask2D componentInChildren2 = this.GetComponentInChildren<RectMask2D>(true);
    if (!(bool) (Object) componentInChildren2)
      return;
    this.maskTransform = componentInChildren2.rectTransform;
  }

  public Vector3 GetWidgetWorldPoint(RectTransform target)
  {
    Vector3 vector3 = new Vector3((0.5f - target.pivot.x) * target.rect.size.x, (0.5f - target.pivot.y) * target.rect.size.y, 0.0f);
    Vector3 position = target.localPosition + vector3;
    return target.parent.TransformPoint(position);
  }

  public Vector3 GetWorldPointInWidget(RectTransform target, Vector3 worldPoint)
  {
    return target.InverseTransformPoint(worldPoint);
  }
}
