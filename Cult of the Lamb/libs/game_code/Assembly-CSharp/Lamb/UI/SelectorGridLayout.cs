// Decompiled with JetBrains decompiler
// Type: Lamb.UI.SelectorGridLayout
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class SelectorGridLayout : GridLayoutGroup
{
  public void OnValidate() => this.constraint = GridLayoutGroup.Constraint.FixedRowCount;

  public override void OnTransformChildrenChanged()
  {
    this.UpdateConstraints();
    base.OnTransformChildrenChanged();
  }

  public override void SetLayoutHorizontal()
  {
    this.UpdateConstraints();
    base.SetLayoutHorizontal();
  }

  public override void SetLayoutVertical()
  {
    this.UpdateConstraints();
    base.SetLayoutVertical();
  }

  public void UpdateConstraints()
  {
    int num = 0;
    for (int index = 0; index < this.transform.childCount; ++index)
    {
      LayoutElement component;
      if (this.transform.GetChild(index).TryGetComponent<LayoutElement>(out component))
      {
        if (!component.ignoreLayout)
          ++num;
      }
      else
        ++num;
    }
    if (num <= 4)
      this.constraintCount = 1;
    else if (num >= 20)
      this.constraintCount = 3;
    else
      this.constraintCount = 2;
  }
}
