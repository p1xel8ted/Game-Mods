// Decompiled with JetBrains decompiler
// Type: Lamb.UI.FillCap
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

[ExecuteInEditMode]
public class FillCap : BaseMonoBehaviour
{
  [SerializeField]
  private RectTransform _rectTransform;
  [SerializeField]
  private Image _fillImage;
  [SerializeField]
  private RectTransform _fillRectTranform;
  private Vector2 _position;

  private void Update()
  {
    if ((Object) this._rectTransform == (Object) null || (Object) this._fillImage == (Object) null || (Object) this._fillRectTranform == (Object) null)
      return;
    this._position.y = this._fillRectTranform.rect.height * this._fillImage.fillAmount;
    Debug.Log((object) this._position.y);
    this._rectTransform.anchoredPosition = this._position;
  }
}
