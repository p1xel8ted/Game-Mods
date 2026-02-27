// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UpgradeMenuCursor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using src.UINavigator;
using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UpgradeMenuCursor : MonoBehaviour
{
  public System.Action OnAtRest;
  public Action<Vector2> OnCursorMoved;
  [Header("Settings")]
  [SerializeField]
  private float _horizontalSensitivity = 1f;
  [SerializeField]
  private float _verticalSensisitivty = 1f;
  [SerializeField]
  private float _selectionRadius = 100f;
  [Header("Components")]
  [SerializeField]
  private RectTransform _rectTransform;
  [SerializeField]
  [ReadOnly]
  private List<Selectable> _validSelectables;
  [SerializeField]
  private RectTransform _cursorViewport;
  [SerializeField]
  private RectTransform _viewport;
  [SerializeField]
  private RectTransform _bounds;
  [SerializeField]
  private CanvasGroup _canvasGroup;
  private bool _snapToggle;
  public bool LockPosition;
  private Tween scaleTween;

  public RectTransform RectTransform => this._rectTransform;

  public CanvasGroup CanvasGroup => this._canvasGroup;

  private void OnEnable()
  {
    MonoSingleton<UINavigatorNew>.Instance.OnSelectionChanged += new Action<Selectable, Selectable>(this.OnSelectionChanged);
    MonoSingleton<UINavigatorNew>.Instance.OnDefaultSetComplete += new Action<Selectable>(this.OnSelection);
  }

  private void OnDisable()
  {
    if (!((UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance != (UnityEngine.Object) null))
      return;
    MonoSingleton<UINavigatorNew>.Instance.OnSelectionChanged -= new Action<Selectable, Selectable>(this.OnSelectionChanged);
    MonoSingleton<UINavigatorNew>.Instance.OnDefaultSetComplete -= new Action<Selectable>(this.OnSelection);
  }

  private void OnSelectionChanged(Selectable current, Selectable previous)
  {
    this.OnSelection(current);
  }

  private void OnSelection(Selectable selectable)
  {
    if (!InputManager.General.MouseInputActive)
      return;
    this._rectTransform.anchoredPosition = selectable.GetComponent<RectTransform>().anchoredPosition;
  }

  private void Update()
  {
    if (this.LockPosition)
      return;
    if (InputManager.General.MouseInputActive)
    {
      if ((double) this._canvasGroup.alpha > 0.0)
        this._canvasGroup.alpha -= Time.unscaledDeltaTime * 4f;
    }
    else if ((double) this._canvasGroup.alpha < 1.0)
      this._canvasGroup.alpha += Time.unscaledDeltaTime * 4f;
    if (InputManager.General.MouseInputActive)
      return;
    float horizontalAxis = InputManager.UI.GetHorizontalAxis();
    float verticalAxis = InputManager.UI.GetVerticalAxis();
    if ((double) Mathf.Abs(horizontalAxis) > 0.20000000298023224 || (double) Mathf.Abs(verticalAxis) > 0.20000000298023224)
    {
      if (this.transform.localScale != Vector3.one && (this.scaleTween == null || !this.scaleTween.active))
        this.scaleTween = (Tween) this._rectTransform.DOScale(Vector3.one, 0.25f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      else
        this._rectTransform.DOKill();
      Vector2 anchoredPosition = this._rectTransform.anchoredPosition;
      if ((double) Mathf.Abs(horizontalAxis) > 0.20000000298023224)
        anchoredPosition.x += InputManager.UI.GetHorizontalAxis() * 1080f * this._horizontalSensitivity * Time.unscaledDeltaTime;
      if ((double) Mathf.Abs(verticalAxis) > 0.20000000298023224)
        anchoredPosition.y += InputManager.UI.GetVerticalAxis() * 1080f * this._verticalSensisitivty * Time.unscaledDeltaTime;
      this._rectTransform.anchoredPosition = anchoredPosition;
      this._snapToggle = true;
    }
    else if (this._snapToggle)
    {
      this._snapToggle = false;
      float num1 = float.MaxValue;
      Selectable newSelectable = (Selectable) null;
      foreach (Selectable validSelectable in this._validSelectables)
      {
        float num2 = Vector3.Distance((Vector3) validSelectable.GetComponent<RectTransform>().anchoredPosition, (Vector3) this._rectTransform.anchoredPosition);
        if ((double) num2 < (double) num1)
        {
          newSelectable = validSelectable;
          num1 = num2;
        }
      }
      MonoSingleton<UINavigatorNew>.Instance.NavigateToNew(newSelectable as IMMSelectable);
      this._rectTransform.DOKill();
      this._rectTransform.DOAnchorPos(newSelectable.GetComponent<RectTransform>().anchoredPosition, 0.5f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutSine).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true);
      this._rectTransform.DOScale(newSelectable.transform.localScale, 0.25f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      System.Action onAtRest = this.OnAtRest;
      if (onAtRest != null)
        onAtRest();
    }
    foreach (Selectable validSelectable in this._validSelectables)
    {
      if (MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable != validSelectable as IMMSelectable && (double) Vector2.Distance(validSelectable.GetComponent<RectTransform>().anchoredPosition, this._rectTransform.anchoredPosition) < (double) this._selectionRadius)
      {
        MonoSingleton<UINavigatorNew>.Instance.NavigateToNew(validSelectable as IMMSelectable);
        break;
      }
    }
    float num3 = 350f;
    float num4 = num3 * 0.5f;
    Rect rect1 = this._bounds.rect;
    rect1.width -= num4 * 2f;
    rect1.width *= 0.5f;
    rect1.height -= num4 * 2f;
    rect1.height *= 0.5f;
    Rect rect2 = this._viewport.rect;
    rect2.width -= num3 * 2f;
    rect2.width *= 0.5f;
    rect2.height -= num3 * 2f;
    rect2.height *= 0.5f;
    Vector2 vector2 = (Vector2) this._cursorViewport.InverseTransformPoint((Vector3) (Vector2) this._rectTransform.parent.TransformPoint((Vector3) this._rectTransform.anchoredPosition));
    float x = 0.0f;
    if ((double) vector2.x < -(double) rect2.width)
      x = Mathf.Abs(vector2.x) - rect2.width;
    else if ((double) vector2.x > (double) rect2.width)
      x = -(Mathf.Abs(vector2.x) - rect2.width);
    float y = 0.0f;
    if ((double) vector2.y < -(double) rect2.height)
      y = Mathf.Abs(vector2.y) - rect2.height;
    else if ((double) vector2.y > (double) rect2.height)
      y = -(Mathf.Abs(vector2.y) - rect2.height);
    Action<Vector2> onCursorMoved = this.OnCursorMoved;
    if (onCursorMoved != null)
      onCursorMoved(new Vector2(x, y));
    Vector2 anchoredPosition1 = this._rectTransform.anchoredPosition;
    anchoredPosition1.x = Mathf.Clamp(anchoredPosition1.x, -rect1.width, rect1.width);
    anchoredPosition1.y = Mathf.Clamp(anchoredPosition1.y, -rect1.height, rect1.height);
    this._rectTransform.anchoredPosition = anchoredPosition1;
  }
}
