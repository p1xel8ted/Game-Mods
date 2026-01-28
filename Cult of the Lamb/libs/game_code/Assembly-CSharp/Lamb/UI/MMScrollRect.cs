// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MMScrollRect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI.Assets;
using Rewired;
using src.UINavigator;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class MMScrollRect : ScrollRect
{
  [Header("MM Scroll Rect")]
  [SerializeField]
  public MMScrollRectConfiguration _scrollRectConfiguration;
  [SerializeField]
  public bool _ignoreSelection;
  [SerializeField]
  public float ScrollSpeedModifier = 1f;
  [SerializeField]
  public bool _scrollToBottom = true;
  public bool _initialSetToggle;

  public float MinY => 0.0f;

  public float MaxY
  {
    get
    {
      Rect rect = this.content.rect;
      double height1 = (double) rect.height;
      rect = this.viewport.rect;
      double height2 = (double) rect.height;
      return (float) (height1 - height2);
    }
  }

  public void OnValidate()
  {
    if (!((UnityEngine.Object) this._scrollRectConfiguration != (UnityEngine.Object) null))
      return;
    this._scrollRectConfiguration.ApplySettings(this);
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this._initialSetToggle = false;
    if (this._ignoreSelection || !((UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance != (UnityEngine.Object) null))
      return;
    MonoSingleton<UINavigatorNew>.Instance.OnDefaultSetComplete += new Action<Selectable>(this.OnDefaultSelectableSet);
    MonoSingleton<UINavigatorNew>.Instance.OnSelectionChanged += new Action<Selectable, Selectable>(this.OnSelectableChanged);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    if (this._ignoreSelection || !((UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance != (UnityEngine.Object) null))
      return;
    MonoSingleton<UINavigatorNew>.Instance.OnDefaultSetComplete -= new Action<Selectable>(this.OnDefaultSelectableSet);
    MonoSingleton<UINavigatorNew>.Instance.OnSelectionChanged -= new Action<Selectable, Selectable>(this.OnSelectableChanged);
  }

  public void OnDefaultSelectableSet(Selectable selectable)
  {
    if (this._initialSetToggle && InputManager.General.MouseInputActive)
      return;
    this._initialSetToggle = true;
    this.StartCoroutine((IEnumerator) this.DeferredDefaultCheck(selectable));
  }

  public IEnumerator DeferredDefaultCheck(Selectable selectable)
  {
    yield return (object) null;
    this.Focus(selectable.transform as RectTransform);
  }

  public void Focus(RectTransform rectTransform)
  {
    float canvasScale = MMCanvasScaler.CanvasScale;
    Vector3 position1 = rectTransform.position;
    Rect rect1 = rectTransform.rect;
    Vector3 position2 = this.viewport.transform.position;
    Rect rect2 = this.viewport.rect;
    Vector3 vector3_1 = position2;
    Vector3 vector3_2 = position2;
    vector3_2.y -= rect2.height * canvasScale;
    Vector3 vector3_3 = position1;
    vector3_3.y += rect1.height * canvasScale;
    Vector3 vector3_4 = position1;
    vector3_4.y -= rect1.height * canvasScale;
    if ((double) vector3_3.y <= (double) vector3_1.y && (double) vector3_4.y >= (double) vector3_2.y)
      return;
    this.content.anchoredPosition = new Vector2()
    {
      x = 0.0f,
      y = -Mathf.Clamp(this.content.InverseTransformPoint(rectTransform.position).y + this.viewport.rect.height * 0.5f, -this.MaxY, this.MinY)
    };
  }

  public void OnSelectableChanged(Selectable newSelectable, Selectable previous)
  {
    this.ScrollTo(newSelectable);
  }

  public bool WasLastInputTypeOf(ControllerType controllerType)
  {
    Controller activeController = InputManager.General.GetLastActiveController(MonoSingleton<UIManager>.Instance.MenusBlocked ? MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer : (PlayerFarming) null);
    return activeController != null && activeController.type == controllerType;
  }

  public void ScrollTo(Selectable selectable)
  {
    if (InputManager.General.MouseInputActive && !this.WasLastInputTypeOf(ControllerType.Keyboard) || !selectable.transform.IsChildOf(this.transform))
      return;
    float canvasScale = MMCanvasScaler.CanvasScale;
    Vector3 position1 = selectable.transform.position;
    Rect rect1 = selectable.GetComponent<RectTransform>().rect;
    Vector3 position2 = this.viewport.transform.position;
    Rect rect2 = this.viewport.rect;
    Vector3 position3 = this.content.transform.position;
    Rect rect3 = this.content.rect;
    Vector3 vector3_1 = position3;
    Vector3 vector3_2 = position3;
    vector3_2.y -= rect3.height * canvasScale;
    Vector3 vector3_3 = position2;
    Vector3 vector3_4 = position2;
    vector3_4.y -= rect2.height * canvasScale;
    Vector3 vector3_5 = position1;
    vector3_5.y += rect1.height * canvasScale;
    Vector3 vector3_6 = position1;
    vector3_6.y -= rect1.height * canvasScale;
    if (!this.vertical || (double) Mathf.Abs(MonoSingleton<UINavigatorNew>.Instance.RecentMoveVector.y) <= 5.0)
      return;
    float num = 0.0f;
    if ((double) MonoSingleton<UINavigatorNew>.Instance.RecentMoveVector.y > 0.0)
    {
      if ((double) vector3_5.y > (double) vector3_3.y)
        num = vector3_3.y - vector3_5.y;
      Selectable selectableOnUp = selectable.FindSelectableOnUp();
      if ((UnityEngine.Object) selectableOnUp == (UnityEngine.Object) null || !selectableOnUp.transform.IsChildOf((Transform) this.content))
        num = vector3_3.y - vector3_1.y;
    }
    else if ((double) MonoSingleton<UINavigatorNew>.Instance.RecentMoveVector.y < 0.0)
    {
      if ((double) vector3_6.y < (double) vector3_4.y)
        num = vector3_4.y - vector3_6.y;
      Selectable selectableOnDown = selectable.FindSelectableOnDown();
      if (this._scrollToBottom && ((UnityEngine.Object) selectableOnDown == (UnityEngine.Object) null || !selectableOnDown.transform.IsChildOf((Transform) this.content)))
        num = vector3_4.y - vector3_2.y;
    }
    if ((double) canvasScale < 1.0)
      num *= 1f / canvasScale;
    if ((double) Mathf.Abs(num) <= 0.0)
      return;
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.DoScrollTo(this.ClampPosition(this.content.anchoredPosition + new Vector2(0.0f, num)), this.content.anchoredPosition, 0.2f));
  }

  public override void OnBeginDrag(PointerEventData eventData)
  {
    if (!InputManager.General.MouseInputEnabled)
      return;
    base.OnBeginDrag(eventData);
  }

  public void ScrollTo(RectTransform target)
  {
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.DoScrollTo(target));
  }

  public IEnumerator ScrollToTop()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    MMScrollRect mmScrollRect = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    Vector2 anchoredPosition = mmScrollRect.content.anchoredPosition;
    Vector2 to = new Vector2(0.0f, mmScrollRect.MinY);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) mmScrollRect.DoScrollTo(to, anchoredPosition);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator ScrollToBottom()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    MMScrollRect mmScrollRect = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    Vector2 anchoredPosition = mmScrollRect.content.anchoredPosition;
    Vector2 to = new Vector2(0.0f, mmScrollRect.MaxY);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) mmScrollRect.DoScrollTo(to, anchoredPosition);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator DoScrollTo(RectTransform target)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    MMScrollRect mmScrollRect = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    Vector2 anchoredPosition = mmScrollRect.content.anchoredPosition;
    Vector2 to = new Vector2()
    {
      x = 0.0f,
      y = -Mathf.Clamp(mmScrollRect.content.InverseTransformPoint(target.parent.TransformPoint(target.localPosition)).y + mmScrollRect.viewport.rect.height * 0.5f, -mmScrollRect.MaxY, mmScrollRect.MinY)
    };
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) mmScrollRect.DoScrollTo(to, anchoredPosition);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator DoScrollTo(Vector2 to, Vector2 from)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    MMScrollRect mmScrollRect = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    float time = Mathf.Clamp(Mathf.Abs(to.y - from.y) / mmScrollRect.viewport.rect.height, 0.0f, 1f) * mmScrollRect._scrollRectConfiguration.MoveToTravelTime / mmScrollRect.ScrollSpeedModifier;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) mmScrollRect.DoScrollTo(to, from, time);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator DoScrollTo(Vector2 to, Vector2 from, float time)
  {
    MMScrollRect mmScrollRect = this;
    float t = 0.0f;
    while ((double) (t += Time.unscaledDeltaTime) <= (double) time)
    {
      mmScrollRect.content.anchoredPosition = Vector2.Lerp(from, to, mmScrollRect._scrollRectConfiguration.ScrollToEase.Evaluate(Mathf.Clamp(t / time, 0.0f, 1f)));
      yield return (object) null;
    }
    mmScrollRect.content.anchoredPosition = to;
  }

  public Vector2 ClampPosition(Vector2 position)
  {
    return new Vector2()
    {
      x = position.x,
      y = Mathf.Clamp(position.y, this.MinY, this.MaxY)
    };
  }
}
