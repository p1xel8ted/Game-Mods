// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIRadialMenuBase`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace Lamb.UI;

public abstract class UIRadialMenuBase<T, U> : UIMenuBase where T : UIRadialWheelItem
{
  private const string kXPositionProperty = "_XPosition";
  private const string kYPositionProperty = "_YPosition";
  private const float kPointerSpeed = 10f;
  private const float kAbsTargetStickThreshold = 0.15f;
  public Action<U> OnItemChosen;
  public new System.Action OnCancel;
  [SerializeField]
  protected UIMenuControlPrompts _controlPrompts;
  [Header("Radial")]
  [SerializeField]
  private MMUIRadialGraphic _radialGraphic;
  [Header("Eye")]
  [SerializeField]
  private RectTransform _eye;
  [SerializeField]
  private RectTransform _pupil;
  [Header("Selectables")]
  [SerializeField]
  protected List<T> _wheelItems;
  [Header("Text")]
  [SerializeField]
  private TextMeshProUGUI _itemName;
  [SerializeField]
  private CanvasGroup _itemNameCanvasGroup;
  [SerializeField]
  private TextMeshProUGUI _itemDescription;
  [SerializeField]
  private CanvasGroup _itemDescriptionCanvasGroup;
  private Material _radialMaterialInstance;
  private float _radiusThreshold;
  private Vector2 _targetVector;
  private T _pointerSelectedOption;
  private bool _didCancel;

  protected abstract bool SelectOnHighlight { get; }

  protected virtual void Start()
  {
    this._radialMaterialInstance = new Material(this._radialGraphic.material);
    this._radialGraphic.material = this._radialMaterialInstance;
    this._itemNameCanvasGroup.alpha = 0.0f;
    this._itemDescriptionCanvasGroup.alpha = 0.0f;
    this._radiusThreshold = this._radialGraphic.Radius * 0.5f;
  }

  protected override void OnShowCompleted()
  {
    this.StartCoroutine((IEnumerator) this.DoWheelLoop());
  }

  protected virtual IEnumerator DoWheelLoop()
  {
    foreach (T wheelItem1 in this._wheelItems)
    {
      T wheelItem = wheelItem1;
      wheelItem.Button.OnPointerEntered += (System.Action) (() => PointerSelect(wheelItem));
      wheelItem.Button.OnPointerExited += new System.Action(PointerDeselect);
      if (wheelItem.IsValidOption())
        wheelItem.Button.onClick.AddListener((UnityAction) (() => ChooseItem(wheelItem)));
    }
    bool itemChosen1 = false;
    T cachedSelection = default (T);
    while (!itemChosen1)
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
      if (!this.SelectOnHighlight && (UnityEngine.Object) obj != (UnityEngine.Object) null && InputManager.UI.GetAcceptButtonDown())
        obj.Button.onClick?.Invoke();
      yield return (object) null;
    }
    yield return (object) null;
    this.CleanupWheelLoop();
    this.OnChoiceFinalized();
    UIRadialMenuBase<T, U> uiRadialMenuBase1;

    void PointerSelect(T wheelItem) => uiRadialMenuBase1._pointerSelectedOption = wheelItem;

    void PointerDeselect() => this.\u003C\u003E4__this._pointerSelectedOption = default (T);
    UIRadialMenuBase<T, U> uiRadialMenuBase2;
    bool itemChosen2;

    void ChooseItem(T item)
    {
      uiRadialMenuBase2.MakeChoice(item);
      if (!uiRadialMenuBase2.SelectOnHighlight && (UnityEngine.Object) BiomeConstants.Instance != (UnityEngine.Object) null)
        BiomeConstants.Instance.ShakeCamera();
      if (uiRadialMenuBase2.SelectOnHighlight)
        return;
      itemChosen2 = true;
    }
  }

  private void Update()
  {
    if (this._canvasGroup.interactable)
      this._targetVector = !((UnityEngine.Object) this._pointerSelectedOption != (UnityEngine.Object) null) ? new Vector2(InputManager.UI.GetHorizontalAxis(), InputManager.UI.GetVerticalAxis()).normalized : this._pointerSelectedOption.Vector;
    else
      this._targetVector.x = this._targetVector.y = 0.0f;
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

  protected void CleanupWheelLoop()
  {
    foreach (T wheelItem in this._wheelItems)
    {
      wheelItem.Button.OnPointerEntered = (System.Action) null;
      wheelItem.Button.OnPointerExited = (System.Action) null;
      wheelItem.Button.onClick.RemoveAllListeners();
    }
  }

  protected abstract void OnChoiceFinalized();

  protected abstract void MakeChoice(T item);

  protected override IEnumerator DoHideAnimation()
  {
    float time = 0.0f;
    while ((double) time < 0.20000000298023224)
    {
      time += Time.unscaledDeltaTime;
      this._eye.localPosition = (Vector3) Vector2.Lerp((Vector2) this._eye.localPosition, (Vector2) Vector3.zero, time / 0.2f);
      this._pupil.transform.localPosition = this._eye.localPosition / 4f;
      yield return (object) null;
    }
    yield return (object) base.DoHideAnimation();
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable)
      return;
    this._didCancel = true;
    this.StopAllCoroutines();
    this.Hide();
  }

  protected override void OnHideCompleted()
  {
    if (this._didCancel)
    {
      System.Action onCancel = this.OnCancel;
      if (onCancel != null)
        onCancel();
    }
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  protected override void OnDestroy()
  {
    base.OnDestroy();
    if (!((UnityEngine.Object) this._radialMaterialInstance != (UnityEngine.Object) null))
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this._radialMaterialInstance);
    this._radialMaterialInstance = (Material) null;
  }
}
