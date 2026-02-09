// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.UISelectionUtility
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using Rewired.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Rewired.UI.ControlMapper;

public static class UISelectionUtility
{
  public static Selectable FindNextSelectable(
    Selectable selectable,
    Transform transform,
    List<Selectable> allSelectables,
    Vector3 direction)
  {
    RectTransform rectTransform = transform as RectTransform;
    if ((Object) rectTransform == (Object) null)
      return (Selectable) null;
    direction.Normalize();
    Vector2 vector2 = (Vector2) direction;
    Vector2 pointOnRectEdge = (Vector2) UITools.GetPointOnRectEdge(rectTransform, vector2);
    bool flag = vector2 == Vector2.right * -1f || vector2 == Vector2.right;
    float num1 = float.PositiveInfinity;
    float num2 = float.PositiveInfinity;
    Selectable selectable1 = (Selectable) null;
    Selectable selectable2 = (Selectable) null;
    Vector2 point2 = pointOnRectEdge + vector2 * 999999f;
    for (int index = 0; index < allSelectables.Count; ++index)
    {
      Selectable allSelectable = allSelectables[index];
      if (!((Object) allSelectable == (Object) selectable) && !((Object) allSelectable == (Object) null) && allSelectable.navigation.mode != Navigation.Mode.None && (allSelectable.IsInteractable() ? 1 : (ReflectionTools.GetPrivateField<Selectable, bool>(allSelectable, "m_GroupsAllowInteraction") ? 1 : 0)) != 0)
      {
        RectTransform transform1 = allSelectable.transform as RectTransform;
        if (!((Object) transform1 == (Object) null))
        {
          Rect rect = UITools.InvertY(UITools.TransformRectTo((Transform) transform1, transform, transform1.rect));
          float sqrMagnitude1;
          if (MathTools.LineIntersectsRect(pointOnRectEdge, point2, rect, out sqrMagnitude1))
          {
            if (flag)
              sqrMagnitude1 *= 0.25f;
            if ((double) sqrMagnitude1 < (double) num2)
            {
              num2 = sqrMagnitude1;
              selectable2 = allSelectable;
            }
          }
          Vector2 to = (Vector2) UnityTools.TransformPoint((Transform) transform1, transform, (Vector3) transform1.rect.center) - pointOnRectEdge;
          if ((double) Mathf.Abs(Vector2.Angle(vector2, to)) <= 75.0)
          {
            float sqrMagnitude2 = to.sqrMagnitude;
            if ((double) sqrMagnitude2 < (double) num1)
            {
              num1 = sqrMagnitude2;
              selectable1 = allSelectable;
            }
          }
        }
      }
    }
    if (!((Object) selectable2 != (Object) null) || !((Object) selectable1 != (Object) null))
      return selectable2 ?? selectable1;
    return (double) num2 > (double) num1 ? selectable1 : selectable2;
  }
}
