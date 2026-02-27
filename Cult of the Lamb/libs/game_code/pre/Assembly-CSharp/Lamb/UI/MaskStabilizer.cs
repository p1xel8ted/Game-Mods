// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MaskStabilizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Lamb.UI;

[ExecuteInEditMode]
public class MaskStabilizer : BaseMonoBehaviour
{
  [SerializeField]
  private RectTransform _rectTransform;
  [SerializeField]
  private RectTransform _mask;
  [SerializeField]
  private RectTransform _parent;

  public void Update()
  {
    if ((Object) this._rectTransform == (Object) null || (Object) this._mask == (Object) null || (Object) this._parent == (Object) null)
      return;
    this._rectTransform.anchoredPosition = (Vector2) this._mask.InverseTransformPoint(this._parent.TransformPoint((Vector3) Vector2.zero));
  }
}
