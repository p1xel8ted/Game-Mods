// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MMRotatedLayoutGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

[ExecuteInEditMode]
public class MMRotatedLayoutGroup : MonoBehaviour, ILayoutGroup, ILayoutController
{
  [SerializeField]
  [Range(0.0f, 360f)]
  public float _offset;

  public float Offset
  {
    get => this._offset;
    set => this._offset = value;
  }

  public void UpdateLayout()
  {
    List<Transform> transformList = new List<Transform>();
    for (int index = 0; index < this.transform.childCount; ++index)
    {
      LayoutElement component;
      if (this.transform.GetChild(index).gameObject.activeSelf && (!this.transform.GetChild(index).TryGetComponent<LayoutElement>(out component) || !component.ignoreLayout))
        transformList.Add(this.transform.GetChild(index));
    }
    for (int index = 0; index < transformList.Count; ++index)
      transformList[index].rotation = Quaternion.Euler(0.0f, 0.0f, 360f / (float) transformList.Count * (float) index + this._offset);
  }

  public void SetLayoutHorizontal() => this.UpdateLayout();

  public void SetLayoutVertical() => this.UpdateLayout();

  public void OnTransformChildrenChanged() => this.UpdateLayout();
}
