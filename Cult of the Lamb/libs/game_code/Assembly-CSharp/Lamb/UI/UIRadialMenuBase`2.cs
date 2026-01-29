// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIRadialMenuBase`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace Lamb.UI;

public abstract class UIRadialMenuBase<T, U> : UIMenuBase where T : UIRadialWheelItem
{
  public const string kXPositionProperty = "_XPosition";
  public const string kYPositionProperty = "_YPosition";
  public const float kPointerSpeed = 10f;
  public const float kAbsTargetStickThreshold = 0.15f;
  public Action<U> OnItemChosen;
  public new System.Action OnCancel;
  [SerializeField]
  public UIMenuControlPrompts _controlPrompts;
  [Header("Radial")]
  [SerializeField]
  public MMUIRadialGraphic _radialGraphic;
  [Header("Eye")]
  [SerializeField]
  public RectTransform _eye;
  [SerializeField]
  public RectTransform _pupil;
  [Header("Selectables")]
  [SerializeField]
  public List<T> _wheelItems;
  [Header("Text")]
  [SerializeField]
  public TextMeshProUGUI _itemName;
  [SerializeField]
  public CanvasGroup _itemNameCanvasGroup;
  [SerializeField]
  public TextMeshProUGUI _itemDescription;
  [SerializeField]
  public CanvasGroup _itemDescriptionCanvasGroup;
  public bool _finalizedSelection;
  public Material _radialMaterialInstance;
  public float _radiusThreshold;
  public Vector2 _targetVector;
  public T _pointerSelectedOption;
  public bool _didCancel;
  public bool cursorEnabled = true;

  public abstract bool SelectOnHighlight { get; }

  public virtual void Start()
  {
    this._radialMaterialInstance = new Material(this._radialGraphic.material);
    this._radialGraphic.material = this._radialMaterialInstance;
    this._itemNameCanvasGroup.alpha = 0.0f;
    this._itemDescriptionCanvasGroup.alpha = 0.0f;
    this._radiusThreshold = this._radialGraphic.Radius * 0.5f;
  }

  public override void OnShowCompleted() => this.StartCoroutine((IEnumerator) this.DoWheelLoop());

  public virtual IEnumerator DoWheelLoop()
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    UIRadialMenuBase<T, U>.\u003C\u003Ec__DisplayClass26_0 cDisplayClass260 = new UIRadialMenuBase<T, U>.\u003C\u003Ec__DisplayClass26_0();
    // ISSUE: reference to a compiler-generated field
    cDisplayClass260.\u003C\u003E4__this = this;
    foreach (T wheelItem1 in this._wheelItems)
    {
      T wheelItem = wheelItem1;
      // ISSUE: reference to a compiler-generated method
      wheelItem.Button.OnPointerEntered += (System.Action) (() => cDisplayClass260.\u003CDoWheelLoop\u003Eg__PointerSelect\u007C0(wheelItem));
      // ISSUE: reference to a compiler-generated method
      wheelItem.Button.OnPointerExited += new System.Action(cDisplayClass260.\u003CDoWheelLoop\u003Eg__PointerDeselect\u007C1);
      if (wheelItem.IsValidOption())
      {
        // ISSUE: reference to a compiler-generated method
        wheelItem.Button.onClick.AddListener((UnityAction) (() => cDisplayClass260.\u003CDoWheelLoop\u003Eg__ChooseItem\u007C2(wheelItem)));
      }
    }
    // ISSUE: reference to a compiler-generated field
    cDisplayClass260.itemChosen = false;
    T cachedSelection = default (T);
    // ISSUE: reference to a compiler-generated field
    while (!cDisplayClass260.itemChosen)
    {
      T obj = default (T);
      if ((double) this._targetVector.Abs().magnitude > 0.15000000596046448)
      {
        float num1 = float.MinValue;
        foreach (T wheelItem in this._wheelItems)
        {
          if (wheelItem.Button.interactable && wheelItem.Visible())
          {
            float num2 = Vector2.Dot(wheelItem.Vector, this._targetVector);
            if ((double) num2 > (double) num1 && (double) num2 > 0.75)
            {
              num1 = num2;
              obj = wheelItem;
            }
          }
        }
      }
      if ((UnityEngine.Object) obj != (UnityEngine.Object) cachedSelection)
      {
        foreach (T wheelItem in this._wheelItems)
        {
          if ((UnityEngine.Object) wheelItem == (UnityEngine.Object) obj)
          {
            wheelItem.DoSelected();
            if (this.SelectOnHighlight)
              wheelItem.Button.onClick?.Invoke();
          }
          else
            wheelItem.DoDeselected();
        }
        cachedSelection = obj;
      }
      if ((UnityEngine.Object) obj != (UnityEngine.Object) null)
      {
        this._controlPrompts.ShowAcceptButton();
        this._itemName.text = obj.GetTitle();
        if ((bool) (UnityEngine.Object) this._itemDescription)
          this._itemDescription.text = obj.GetDescription();
        if ((double) this._itemNameCanvasGroup.alpha < 1.0)
          this._itemNameCanvasGroup.alpha += Time.unscaledDeltaTime * 3f;
        if ((UnityEngine.Object) this._itemDescriptionCanvasGroup != (UnityEngine.Object) null && (double) this._itemDescriptionCanvasGroup.alpha < 1.0)
          this._itemDescriptionCanvasGroup.alpha += Time.unscaledDeltaTime * 3f;
      }
      else
      {
        this._controlPrompts.HideAcceptButton();
        if ((double) this._itemNameCanvasGroup.alpha > 0.0)
          this._itemNameCanvasGroup.alpha -= Time.unscaledDeltaTime * 3f;
        if ((UnityEngine.Object) this._itemDescriptionCanvasGroup != (UnityEngine.Object) null && (double) this._itemDescriptionCanvasGroup.alpha > 0.0)
          this._itemDescriptionCanvasGroup.alpha -= Time.unscaledDeltaTime * 3f;
      }
      if (!this.SelectOnHighlight && (UnityEngine.Object) obj != (UnityEngine.Object) null && InputManager.UI.GetAcceptButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
        obj.Button.onClick?.Invoke();
      yield return (object) null;
    }
    yield return (object) null;
    this.CleanupWheelLoop();
    this.OnChoiceFinalized();
  }

  public void Update()
  {
    if (this._finalizedSelection)
      return;
    if (this._canvasGroup.interactable)
      this._targetVector = !((UnityEngine.Object) this._pointerSelectedOption != (UnityEngine.Object) null) ? new Vector2(InputManager.UI.GetHorizontalAxis(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer), InputManager.UI.GetVerticalAxis(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer)).normalized : this._pointerSelectedOption.Vector;
    else
      this._targetVector.x = this._targetVector.y = 0.0f;
    if (!this.cursorEnabled)
      return;
    this._eye.localPosition = (Vector3) Vector2.Lerp((Vector2) this._eye.localPosition, this._targetVector * this._radiusThreshold, Time.unscaledDeltaTime * 10f);
    this._pupil.transform.localPosition = this._eye.localPosition / 4f;
    Vector2 vector2 = new Vector2()
    {
      x = Mathf.Clamp(this._eye.localPosition.x / this._radiusThreshold, -1f, 1f),
      y = Mathf.Clamp(this._eye.localPosition.y / this._radiusThreshold, -1f, 1f)
    };
    this._radialMaterialInstance.SetFloat("_XPosition", vector2.x);
    this._radialMaterialInstance.SetFloat("_YPosition", vector2.y);
  }

  public void CleanupWheelLoop()
  {
    foreach (T wheelItem in this._wheelItems)
    {
      wheelItem.Button.OnPointerEntered = (System.Action) null;
      wheelItem.Button.OnPointerExited = (System.Action) null;
      wheelItem.Button.onClick.RemoveAllListeners();
    }
  }

  public abstract void OnChoiceFinalized();

  public abstract void MakeChoice(T item);

  public override IEnumerator DoHideAnimation()
  {
    if (!this._finalizedSelection)
    {
      float time = 0.0f;
      while ((double) time < 0.20000000298023224)
      {
        time += Time.unscaledDeltaTime;
        this._eye.localPosition = (Vector3) Vector2.Lerp((Vector2) this._eye.localPosition, (Vector2) Vector3.zero, time / 0.2f);
        this._pupil.transform.localPosition = this._eye.localPosition / 4f;
        yield return (object) null;
      }
    }
    yield return (object) this.\u003C\u003En__0();
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable)
      return;
    this._didCancel = true;
    this.StopAllCoroutines();
    this.Hide();
  }

  public override void OnHideCompleted()
  {
    if (this._didCancel)
    {
      System.Action onCancel = this.OnCancel;
      if (onCancel != null)
        onCancel();
    }
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if (!((UnityEngine.Object) this._radialMaterialInstance != (UnityEngine.Object) null))
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this._radialMaterialInstance);
    this._radialMaterialInstance = (Material) null;
  }

  [CompilerGenerated]
  [DebuggerHidden]
  public IEnumerator \u003C\u003En__0() => base.DoHideAnimation();
}
