// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.UISelectionUtility
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Rewired.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Rewired.UI.ControlMapper;

public static class UISelectionUtility
{
  public static Selectable[] s_reusableAllSelectables = new Selectable[0];

  public static Selectable FindNextSelectable(
    Selectable selectable,
    Transform transform,
    Vector3 direction)
  {
    RectTransform rectTransform = transform as RectTransform;
    if ((UnityEngine.Object) rectTransform == (UnityEngine.Object) null)
      return (Selectable) null;
    if (Selectable.allSelectableCount > UISelectionUtility.s_reusableAllSelectables.Length)
      UISelectionUtility.s_reusableAllSelectables = new Selectable[Selectable.allSelectableCount];
    int num1 = Selectable.AllSelectablesNoAlloc(UISelectionUtility.s_reusableAllSelectables);
    IList<Selectable> reusableAllSelectables = (IList<Selectable>) UISelectionUtility.s_reusableAllSelectables;
    direction.Normalize();
    Vector2 vector2 = (Vector2) direction;
    Vector2 pointOnRectEdge = (Vector2) UITools.GetPointOnRectEdge(rectTransform, vector2);
    bool flag = vector2 == Vector2.right * -1f || vector2 == Vector2.right;
    float num2 = float.PositiveInfinity;
    float num3 = float.PositiveInfinity;
    Selectable selectable1 = (Selectable) null;
    Selectable selectable2 = (Selectable) null;
    Vector2 point2 = pointOnRectEdge + vector2 * 999999f;
    for (int index = 0; index < num1; ++index)
    {
      Selectable selectable3 = reusableAllSelectables[index];
      if (!((UnityEngine.Object) selectable3 == (UnityEngine.Object) selectable) && !((UnityEngine.Object) selectable3 == (UnityEngine.Object) null) && selectable3.navigation.mode != Navigation.Mode.None && (selectable3.IsInteractable() ? 1 : (ReflectionTools.GetPrivateField<Selectable, bool>(selectable3, "m_GroupsAllowInteraction") ? 1 : 0)) != 0)
      {
        RectTransform transform1 = selectable3.transform as RectTransform;
        if (!((UnityEngine.Object) transform1 == (UnityEngine.Object) null))
        {
          Rect rect = UITools.InvertY(UITools.TransformRectTo((Transform) transform1, transform, transform1.rect));
          float sqrMagnitude1;
          if (MathTools.LineIntersectsRect(pointOnRectEdge, point2, rect, out sqrMagnitude1))
          {
            if (flag)
              sqrMagnitude1 *= 0.25f;
            if ((double) sqrMagnitude1 < (double) num3)
            {
              num3 = sqrMagnitude1;
              selectable2 = selectable3;
            }
          }
          Vector2 to = (Vector2) UnityTools.TransformPoint((Transform) transform1, transform, (Vector3) transform1.rect.center) - pointOnRectEdge;
          if ((double) Mathf.Abs(Vector2.Angle(vector2, to)) <= 75.0)
          {
            float sqrMagnitude2 = to.sqrMagnitude;
            if ((double) sqrMagnitude2 < (double) num2)
            {
              num2 = sqrMagnitude2;
              selectable1 = selectable3;
            }
          }
        }
      }
    }
    if ((UnityEngine.Object) selectable2 != (UnityEngine.Object) null && (UnityEngine.Object) selectable1 != (UnityEngine.Object) null)
      return (double) num3 > (double) num2 ? selectable1 : selectable2;
    Array.Clear((Array) UISelectionUtility.s_reusableAllSelectables, 0, UISelectionUtility.s_reusableAllSelectables.Length);
    return selectable2 ?? selectable1;
  }
}
