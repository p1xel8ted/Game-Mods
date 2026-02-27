// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.ScrollRectSelectableChild
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Rewired.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
namespace Rewired.UI.ControlMapper;

[AddComponentMenu("")]
[RequireComponent(typeof (Selectable))]
public class ScrollRectSelectableChild : MonoBehaviour, ISelectHandler, IEventSystemHandler
{
  public bool useCustomEdgePadding;
  public float customEdgePadding = 50f;
  public ScrollRect parentScrollRect;
  public Selectable _selectable;

  public RectTransform parentScrollRectContentTransform => this.parentScrollRect.content;

  public Selectable selectable
  {
    get => this._selectable ?? (this._selectable = this.GetComponent<Selectable>());
  }

  public RectTransform rectTransform => this.transform as RectTransform;

  public void Start()
  {
    this.parentScrollRect = this.transform.GetComponentInParent<ScrollRect>();
    if (!((Object) this.parentScrollRect == (Object) null))
      return;
    Debug.LogError((object) "Rewired Control Mapper: No ScrollRect found! This component must be a child of a ScrollRect!");
  }

  public void OnSelect(BaseEventData eventData)
  {
    if ((Object) this.parentScrollRect == (Object) null || !(eventData is AxisEventData))
      return;
    RectTransform transform = this.parentScrollRect.transform as RectTransform;
    Rect child = MathTools.TransformRect(this.rectTransform.rect, (Transform) this.rectTransform, (Transform) transform);
    Rect rect1 = transform.rect;
    Rect rect2 = transform.rect;
    float num = !this.useCustomEdgePadding ? child.height : this.customEdgePadding;
    rect2.yMax -= num;
    rect2.yMin += num;
    Vector2 offset;
    if (MathTools.RectContains(rect2, child) || !MathTools.GetOffsetToContainRect(rect2, child, out offset))
      return;
    Vector2 anchoredPosition = this.parentScrollRectContentTransform.anchoredPosition;
    anchoredPosition.x = Mathf.Clamp(anchoredPosition.x + offset.x, 0.0f, Mathf.Abs(rect1.width - this.parentScrollRectContentTransform.sizeDelta.x));
    anchoredPosition.y = Mathf.Clamp(anchoredPosition.y + offset.y, 0.0f, Mathf.Abs(rect1.height - this.parentScrollRectContentTransform.sizeDelta.y));
    this.parentScrollRectContentTransform.anchoredPosition = anchoredPosition;
  }
}
