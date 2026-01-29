// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MaskStabilizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Lamb.UI;

[ExecuteInEditMode]
public class MaskStabilizer : BaseMonoBehaviour
{
  [SerializeField]
  public RectTransform _rectTransform;
  [SerializeField]
  public RectTransform _mask;
  [SerializeField]
  public RectTransform _parent;

  public void Update()
  {
    if ((Object) this._rectTransform == (Object) null || (Object) this._mask == (Object) null || (Object) this._parent == (Object) null)
      return;
    this._rectTransform.anchoredPosition = (Vector2) this._mask.InverseTransformPoint(this._parent.TransformPoint((Vector3) Vector2.zero));
  }
}
