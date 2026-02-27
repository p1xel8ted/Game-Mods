// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MMSizeToScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

[ExecuteAlways]
[RequireComponent(typeof (RectTransform))]
public class MMSizeToScreen : MonoBehaviour
{
  [SerializeField]
  private RectTransform _canvasRectTransform;
  [SerializeField]
  private LayoutElement _layoutElement;

  private void Awake() => this._layoutElement = this.GetComponent<LayoutElement>();

  private void Update()
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
