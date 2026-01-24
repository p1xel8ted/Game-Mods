// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MMSizeToScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

[ExecuteAlways]
[RequireComponent(typeof (RectTransform))]
public class MMSizeToScreen : MonoBehaviour
{
  [SerializeField]
  public RectTransform _canvasRectTransform;
  [SerializeField]
  public LayoutElement _layoutElement;

  public void Awake() => this._layoutElement = this.GetComponent<LayoutElement>();

  public void Update()
  {
    if ((Object) this._canvasRectTransform == (Object) null)
      return;
    LayoutElement layoutElement1 = this._layoutElement;
    LayoutElement layoutElement2 = this._layoutElement;
    Rect rect1 = this._canvasRectTransform.rect;
    double height;
    float num1 = (float) (height = (double) rect1.height);
    layoutElement2.minHeight = (float) height;
    double num2 = (double) num1;
    layoutElement1.preferredHeight = (float) num2;
    LayoutElement layoutElement3 = this._layoutElement;
    LayoutElement layoutElement4 = this._layoutElement;
    Rect rect2 = this._canvasRectTransform.rect;
    double width;
    float num3 = (float) (width = (double) rect2.width);
    layoutElement4.minWidth = (float) width;
    double num4 = (double) num3;
    layoutElement3.preferredWidth = (float) num4;
  }
}
