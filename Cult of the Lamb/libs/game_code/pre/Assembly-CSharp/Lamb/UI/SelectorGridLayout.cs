// Decompiled with JetBrains decompiler
// Type: Lamb.UI.SelectorGridLayout
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class SelectorGridLayout : GridLayoutGroup
{
  public void OnValidate() => this.constraint = GridLayoutGroup.Constraint.FixedRowCount;

  protected override void OnTransformChildrenChanged()
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

  private void UpdateConstraints()
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
    else
      this.constraintCount = 2;
  }
}
