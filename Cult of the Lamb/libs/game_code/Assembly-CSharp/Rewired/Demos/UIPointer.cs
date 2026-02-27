// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.UIPointer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
namespace Rewired.Demos;

[AddComponentMenu("")]
[RequireComponent(typeof (RectTransform))]
public sealed class UIPointer : UIBehaviour
{
  [Tooltip("Should the hardware pointer be hidden?")]
  [SerializeField]
  public bool _hideHardwarePointer = true;
  [Tooltip("Sets the pointer to the last sibling in the parent hierarchy. Do not enable this on multiple UIPointers under the same parent transform or they will constantly fight each other for dominance.")]
  [SerializeField]
  public bool _autoSort = true;
  public Canvas _canvas;

  public bool autoSort
  {
    get => this._autoSort;
    set
    {
      if (value == this._autoSort)
        return;
      this._autoSort = value;
      if (!value)
        return;
      this.transform.SetAsLastSibling();
    }
  }

  public override void Awake()
  {
    base.Awake();
    foreach (Graphic componentsInChild in this.GetComponentsInChildren<Graphic>())
      componentsInChild.raycastTarget = false;
    if (this._hideHardwarePointer)
      Cursor.visible = false;
    if (this._autoSort)
      this.transform.SetAsLastSibling();
    this.GetDependencies();
  }

  public void Update()
  {
    if (!this._autoSort || this.transform.GetSiblingIndex() >= this.transform.parent.childCount - 1)
      return;
    this.transform.SetAsLastSibling();
  }

  public override void OnTransformParentChanged()
  {
    base.OnTransformParentChanged();
    this.GetDependencies();
  }

  public override void OnCanvasGroupChanged()
  {
    base.OnCanvasGroupChanged();
    this.GetDependencies();
  }

  public void OnScreenPositionChanged(Vector2 screenPosition)
  {
    if ((Object) this._canvas == (Object) null)
      return;
    Camera cam = (Camera) null;
    switch (this._canvas.renderMode)
    {
      case RenderMode.ScreenSpaceCamera:
      case RenderMode.WorldSpace:
        cam = this._canvas.worldCamera;
        break;
    }
    Vector2 localPoint;
    RectTransformUtility.ScreenPointToLocalPointInRectangle(this.transform.parent as RectTransform, screenPosition, cam, out localPoint);
    this.transform.localPosition = new Vector3(localPoint.x, localPoint.y, this.transform.localPosition.z);
  }

  public void GetDependencies()
  {
    this._canvas = this.transform.root.GetComponentInChildren<Canvas>();
  }
}
