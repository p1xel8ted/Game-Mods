// Decompiled with JetBrains decompiler
// Type: Unity.VideoHelper.Timeline
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
namespace Unity.VideoHelper;

[RequireComponent(typeof (RectTransform))]
public class Timeline : 
  Selectable,
  IDragHandler,
  IEventSystemHandler,
  ICanvasElement,
  IInitializePotentialDragHandler
{
  public Image positionImage;
  public Image previewImage;
  public Text tooltipText;
  public RectTransform positionContainerRect;
  public RectTransform handleContainerRect;
  public RectTransform tooltipContainerRect;
  public Vector2 handleOffset;
  public Camera cam;
  public DrivenRectTransformTracker tracker;
  public float previewPosition;
  public float stepSize = 0.05f;
  public bool isInControl;
  public ITimelineProvider provider;
  [SerializeField]
  public RectTransform positionRect;
  [SerializeField]
  public RectTransform previewRect;
  [SerializeField]
  public RectTransform handleRect;
  [SerializeField]
  public RectTransform tooltipRect;
  [SerializeField]
  [Range(0.0f, 1f)]
  public float position;
  [SerializeField]
  public FloatEvent onSeeked = new FloatEvent();

  public RectTransform PositionRect
  {
    get => this.positionRect;
    set => this.positionRect = value;
  }

  public RectTransform PreviewRect
  {
    get => this.previewRect;
    set => this.previewRect = value;
  }

  public RectTransform HandleRect
  {
    get => this.handleRect;
    set => this.handleRect = value;
  }

  public RectTransform TooltipRect
  {
    get => this.tooltipRect;
    set => this.tooltipRect = value;
  }

  public float Position
  {
    get => this.position;
    set => this.SetPosition(value);
  }

  public UnityEvent<float> OnSeeked => (UnityEvent<float>) this.onSeeked;

  public override void OnEnable()
  {
    base.OnEnable();
    this.UpdateReferences();
    this.SetPosition(this.position, false);
    this.UpdateVisuals();
  }

  public override void OnDisable()
  {
    base.OnDisable();
    this.tracker.Clear();
  }

  public override void OnRectTransformDimensionsChange()
  {
    base.OnRectTransformDimensionsChange();
    if (!this.IsActive())
      return;
    this.UpdateVisuals();
  }

  public void Update()
  {
    if (!this.isInControl)
      return;
    float previewPoint = this.GetPreviewPoint();
    if ((double) previewPoint == (double) this.previewPosition)
      return;
    this.previewPosition = previewPoint;
    this.UpdateFillableVisuals(this.previewRect, this.previewImage, this.previewPosition);
    this.UpdateAnchorBasedVisuals(this.tooltipRect, this.previewPosition);
    this.tooltipText.text = this.provider.GetFormattedPosition(this.previewPosition);
  }

  public override void OnMove(AxisEventData eventData)
  {
    if (!this.IsActive() || !this.IsInteractable())
    {
      base.OnMove(eventData);
    }
    else
    {
      Func<bool> func = (Func<bool>) (() => this.navigation.mode == Navigation.Mode.Automatic);
      switch (eventData.moveDir)
      {
        case MoveDirection.Left:
          if (func())
          {
            this.Move(this.position - this.stepSize);
            break;
          }
          base.OnMove(eventData);
          break;
        case MoveDirection.Up:
        case MoveDirection.Down:
          base.OnMove(eventData);
          break;
        case MoveDirection.Right:
          if (func())
          {
            this.Move(this.position + this.stepSize);
            break;
          }
          base.OnMove(eventData);
          break;
      }
    }
  }

  public void Move(float value)
  {
    this.SetPosition(value);
    this.onSeeked.Invoke(value);
  }

  public void UpdateReferences()
  {
    if ((bool) (UnityEngine.Object) this.positionRect)
    {
      this.positionImage = this.positionRect.GetComponent<Image>();
      this.positionContainerRect = this.positionRect.parent.GetComponent<RectTransform>();
    }
    else
    {
      this.positionRect = (RectTransform) null;
      this.positionImage = (Image) null;
      this.positionContainerRect = (RectTransform) null;
    }
    if ((bool) (UnityEngine.Object) this.previewRect)
    {
      this.previewImage = this.previewRect.GetComponent<Image>();
    }
    else
    {
      this.previewRect = (RectTransform) null;
      this.previewImage = (Image) null;
    }
    if ((bool) (UnityEngine.Object) this.handleRect)
    {
      this.handleContainerRect = this.handleRect.parent.GetComponent<RectTransform>();
    }
    else
    {
      this.handleRect = (RectTransform) null;
      this.handleContainerRect = (RectTransform) null;
    }
    if ((bool) (UnityEngine.Object) this.tooltipRect)
    {
      this.tooltipContainerRect = this.tooltipRect.parent.GetComponent<RectTransform>();
      this.tooltipText = this.tooltipRect.GetComponentInChildren<Text>();
    }
    else
    {
      this.tooltipRect = (RectTransform) null;
      this.tooltipContainerRect = (RectTransform) null;
    }
    this.cam = Camera.main;
    this.provider = (ITimelineProvider) this.GetComponentInParent<VideoPresenter>();
  }

  public void UpdateVisuals()
  {
    this.tracker.Clear();
    if ((bool) (UnityEngine.Object) this.positionContainerRect)
      this.UpdateFillableVisuals(this.positionRect, this.positionImage, this.position);
    if (!this.isInControl)
      this.UpdateFillableVisuals(this.previewRect, this.previewImage, this.position);
    if (!(bool) (UnityEngine.Object) this.handleContainerRect)
      return;
    this.UpdateAnchorBasedVisuals(this.handleRect, this.position);
  }

  public void UpdateAnchorBasedVisuals(RectTransform rect, float position)
  {
    if ((UnityEngine.Object) rect == (UnityEngine.Object) null)
      return;
    this.tracker.Add((UnityEngine.Object) this, rect, DrivenTransformProperties.Anchors);
    Vector2 zero = Vector2.zero;
    Vector2 one = Vector2.one;
    zero[0] = one[0] = position;
    rect.anchorMin = zero;
    rect.anchorMax = one;
  }

  public void UpdateFillableVisuals(RectTransform rect, Image image, float value)
  {
    if ((UnityEngine.Object) rect == (UnityEngine.Object) null)
      return;
    this.tracker.Add((UnityEngine.Object) this, rect, DrivenTransformProperties.Anchors);
    Vector2 one = Vector2.one;
    if ((UnityEngine.Object) image != (UnityEngine.Object) null && image.type == Image.Type.Filled)
      image.fillAmount = value;
    else
      one[0] = value;
    rect.anchorMin = Vector2.zero;
    rect.anchorMax = one;
  }

  public bool CanDrag() => this.IsActive() && this.IsInteractable();

  public void UpdateDrag(PointerEventData eventData, Camera cam)
  {
    RectTransform rect1 = this.handleContainerRect ?? this.positionContainerRect;
    Vector2 localPoint;
    if (!((UnityEngine.Object) rect1 != (UnityEngine.Object) null) || (double) rect1.rect.size[0] <= 0.0 || !RectTransformUtility.ScreenPointToLocalPointInRectangle(rect1, eventData.position, cam, out localPoint))
      return;
    Vector2 vector2_1 = localPoint;
    Rect rect2 = rect1.rect;
    Vector2 position = rect2.position;
    localPoint = vector2_1 - position;
    Vector2 vector2_2 = localPoint - this.handleOffset;
    double num1 = (double) vector2_2[0];
    rect2 = rect1.rect;
    vector2_2 = rect2.size;
    double num2 = (double) vector2_2[0];
    this.Position = Mathf.Clamp01((float) (num1 / num2));
    this.onSeeked.Invoke(this.Position);
  }

  public float GetPreviewPoint()
  {
    Vector2 localPoint;
    RectTransformUtility.ScreenPointToLocalPointInRectangle(this.tooltipContainerRect, RectTransformUtility.WorldToScreenPoint(this.cam, Input.mousePosition), this.cam, out localPoint);
    return Mathf.Clamp01((localPoint - this.tooltipContainerRect.rect.position)[0] / this.tooltipContainerRect.rect.size[0]);
  }

  public void SetPosition(float newPosition, bool sendCallback = true)
  {
    newPosition = Mathf.Clamp01(newPosition);
    if ((double) this.position == (double) newPosition)
      return;
    this.position = newPosition;
    this.UpdateVisuals();
  }

  public override void OnPointerEnter(PointerEventData eventData)
  {
    this.isInControl = true;
    if ((UnityEngine.Object) this.handleRect != (UnityEngine.Object) null)
      this.SetActive(this.handleRect.gameObject, true);
    if (!((UnityEngine.Object) this.tooltipRect != (UnityEngine.Object) null))
      return;
    this.tooltipRect.gameObject.SetActive(true);
  }

  public void SetActive(GameObject gameObject, bool value)
  {
    if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
      return;
    if (value)
    {
      IActivate component = gameObject.GetComponent<IActivate>();
      if (component != null)
      {
        component.Activate();
        return;
      }
    }
    else
    {
      IDeactivate component = gameObject.GetComponent<IDeactivate>();
      if (component != null)
      {
        component.Deactivate();
        return;
      }
    }
    gameObject.SetActive(value);
  }

  public override void OnPointerExit(PointerEventData eventData)
  {
    this.isInControl = false;
    if ((UnityEngine.Object) this.handleRect != (UnityEngine.Object) null)
      this.SetActive(this.handleRect.gameObject, false);
    if ((UnityEngine.Object) this.tooltipRect != (UnityEngine.Object) null)
      this.tooltipRect.gameObject.SetActive(false);
    this.UpdateFillableVisuals(this.previewRect, this.previewImage, 0.0f);
  }

  public override void OnPointerDown(PointerEventData eventData)
  {
    if (!this.CanDrag())
      return;
    base.OnPointerDown(eventData);
    this.handleOffset = Vector2.zero;
    if ((UnityEngine.Object) this.handleContainerRect != (UnityEngine.Object) null && RectTransformUtility.RectangleContainsScreenPoint(this.handleRect, eventData.position, eventData.enterEventCamera))
    {
      Vector2 localPoint;
      if (RectTransformUtility.ScreenPointToLocalPointInRectangle(this.handleRect, eventData.position, eventData.pressEventCamera, out localPoint))
        this.handleOffset = localPoint;
      this.handleOffset.y = -this.handleOffset.y;
    }
    else
      this.UpdateDrag(eventData, eventData.pressEventCamera);
  }

  public virtual void OnDrag(PointerEventData eventData)
  {
    if (!this.CanDrag())
      return;
    this.isInControl = true;
    this.UpdateDrag(eventData, eventData.pressEventCamera);
  }

  public void Rebuild(CanvasUpdate executing)
  {
  }

  public void LayoutComplete()
  {
  }

  public void GraphicUpdateComplete()
  {
  }

  public void OnInitializePotentialDrag(PointerEventData eventData)
  {
    eventData.useDragThreshold = false;
  }

  Transform ICanvasElement.get_transform() => this.transform;

  [CompilerGenerated]
  public bool \u003COnMove\u003Eb__40_0() => this.navigation.mode == Navigation.Mode.Automatic;
}
