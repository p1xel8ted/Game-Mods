// Decompiled with JetBrains decompiler
// Type: Lamb.UI.FillCap
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

[ExecuteInEditMode]
public class FillCap : BaseMonoBehaviour
{
  [SerializeField]
  public RectTransform _rectTransform;
  [SerializeField]
  public Image _fillImage;
  [SerializeField]
  public RectTransform _fillRectTranform;
  public Vector2 _position;

  public void Update()
  {
    if ((Object) this._rectTransform == (Object) null || (Object) this._fillImage == (Object) null || (Object) this._fillRectTranform == (Object) null)
      return;
    this._position.y = this._fillRectTranform.rect.height * this._fillImage.fillAmount;
    Debug.Log((object) this._position.y);
    this._rectTransform.anchoredPosition = this._position;
  }
}
