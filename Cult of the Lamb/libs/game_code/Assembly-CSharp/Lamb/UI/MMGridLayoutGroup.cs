// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MMGridLayoutGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class MMGridLayoutGroup : GridLayoutGroup
{
  public override void SetLayoutHorizontal() => this.SetCellsAlongAxis(0);

  public override void SetLayoutVertical() => this.SetCellsAlongAxis(1);

  public new void SetCellsAlongAxis(int axis)
  {
    int count = this.rectChildren.Count;
    if (axis == 0)
    {
      for (int index = 0; index < count; ++index)
      {
        RectTransform rectChild = this.rectChildren[index];
        this.m_Tracker.Add((Object) this, rectChild, DrivenTransformProperties.Anchors | DrivenTransformProperties.AnchoredPosition | DrivenTransformProperties.SizeDelta);
        rectChild.anchorMin = Vector2.up;
        rectChild.anchorMax = Vector2.up;
        rectChild.sizeDelta = this.cellSize;
      }
    }
    else
    {
      float x1 = this.rectTransform.rect.size.x;
      float y1 = this.rectTransform.rect.size.y;
      int num1 = 1;
      int num2 = 1;
      if (this.m_Constraint == GridLayoutGroup.Constraint.FixedColumnCount)
      {
        num1 = this.m_ConstraintCount;
        if (count > num1)
          num2 = count / num1 + (count % num1 > 0 ? 1 : 0);
      }
      else if (this.m_Constraint == GridLayoutGroup.Constraint.FixedRowCount)
      {
        num2 = this.m_ConstraintCount;
        if (count > num2)
          num1 = count / num2 + (count % num2 > 0 ? 1 : 0);
      }
      else
      {
        num1 = (double) this.cellSize.x + (double) this.spacing.x > 0.0 ? Mathf.Max(1, Mathf.FloorToInt((float) (((double) x1 - (double) this.padding.horizontal + (double) this.spacing.x + 1.0 / 1000.0) / ((double) this.cellSize.x + (double) this.spacing.x)))) : int.MaxValue;
        num2 = (double) this.cellSize.y + (double) this.spacing.y > 0.0 ? Mathf.Max(1, Mathf.FloorToInt((float) (((double) y1 - (double) this.padding.vertical + (double) this.spacing.y + 1.0 / 1000.0) / ((double) this.cellSize.y + (double) this.spacing.y)))) : int.MaxValue;
      }
      int num3 = (int) this.startCorner % 2;
      int num4 = (int) this.startCorner / 2;
      int num5;
      int num6;
      int num7;
      if (this.startAxis == GridLayoutGroup.Axis.Horizontal)
      {
        num5 = num1;
        num6 = Mathf.Clamp(num1, 1, count);
        num7 = Mathf.Clamp(num2, 1, Mathf.CeilToInt((float) count / (float) num5));
      }
      else
      {
        num5 = num2;
        num7 = Mathf.Clamp(num2, 1, count);
        num6 = Mathf.Clamp(num1, 1, Mathf.CeilToInt((float) count / (float) num5));
      }
      int num8 = count % num5;
      Vector2 vector2_1 = new Vector2((float) ((double) num6 * (double) this.cellSize.x + (double) (num6 - 1) * (double) this.spacing.x), (float) ((double) num7 * (double) this.cellSize.y + (double) (num7 - 1) * (double) this.spacing.y));
      Vector2 vector2_2 = new Vector2(this.GetStartOffset(0, vector2_1.x), this.GetStartOffset(1, vector2_1.y));
      int num9 = num8 == 0 ? num5 : num8;
      int num10 = this.startAxis == GridLayoutGroup.Axis.Horizontal ? num9 : num6;
      int num11 = this.startAxis == GridLayoutGroup.Axis.Vertical ? num9 : num7;
      Vector2 vector2_3 = new Vector2((float) ((double) num10 * (double) this.cellSize.x + (double) (num10 - 1) * (double) this.spacing.x), (float) ((double) num11 * (double) this.cellSize.y + (double) (num11 - 1) * (double) this.spacing.y));
      Vector2 vector2_4 = new Vector2(this.GetStartOffset(0, vector2_3.x), this.GetStartOffset(1, vector2_3.y));
      for (int index = 0; index < count; ++index)
      {
        Vector2 vector2_5 = index + 1 > count - num9 ? vector2_4 : vector2_2;
        int num12;
        int num13;
        if (this.startAxis == GridLayoutGroup.Axis.Horizontal)
        {
          num12 = index % num5;
          num13 = index / num5;
        }
        else
        {
          num12 = index / num5;
          num13 = index % num5;
        }
        if (num3 == 1)
          num12 = num6 - 1 - num12;
        if (num4 == 1)
          num13 = num7 - 1 - num13;
        RectTransform rectChild1 = this.rectChildren[index];
        double x2 = (double) vector2_5.x;
        Vector2 vector2_6 = this.cellSize;
        double num14 = (double) vector2_6[0];
        vector2_6 = this.spacing;
        double num15 = (double) vector2_6[0];
        double num16 = (num14 + num15) * (double) num12;
        double pos1 = x2 + num16;
        vector2_6 = this.cellSize;
        double size1 = (double) vector2_6[0];
        this.SetChildAlongAxis(rectChild1, 0, (float) pos1, (float) size1);
        RectTransform rectChild2 = this.rectChildren[index];
        double y2 = (double) vector2_5.y;
        vector2_6 = this.cellSize;
        double num17 = (double) vector2_6[1];
        vector2_6 = this.spacing;
        double num18 = (double) vector2_6[1];
        double num19 = (num17 + num18) * (double) num13;
        double pos2 = y2 + num19;
        vector2_6 = this.cellSize;
        double size2 = (double) vector2_6[1];
        this.SetChildAlongAxis(rectChild2, 1, (float) pos2, (float) size2);
      }
    }
  }
}
