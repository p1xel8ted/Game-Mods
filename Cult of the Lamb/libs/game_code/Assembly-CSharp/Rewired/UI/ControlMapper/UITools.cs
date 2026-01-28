// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.UITools
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Rewired.UI.ControlMapper;

public static class UITools
{
  public static GameObject InstantiateGUIObject<T>(
    GameObject prefab,
    Transform parent,
    string name)
    where T : Component
  {
    GameObject gameObject = UITools.InstantiateGUIObject_Pre<T>(prefab, parent, name);
    if ((Object) gameObject == (Object) null)
      return (GameObject) null;
    RectTransform component = gameObject.GetComponent<RectTransform>();
    if ((Object) component == (Object) null)
      Debug.LogError((object) (name + " prefab is missing RectTransform component!"));
    else
      component.localScale = Vector3.one;
    return gameObject;
  }

  public static GameObject InstantiateGUIObject<T>(
    GameObject prefab,
    Transform parent,
    string name,
    Vector2 pivot,
    Vector2 anchorMin,
    Vector2 anchorMax,
    Vector2 anchoredPosition)
    where T : Component
  {
    GameObject gameObject = UITools.InstantiateGUIObject_Pre<T>(prefab, parent, name);
    if ((Object) gameObject == (Object) null)
      return (GameObject) null;
    RectTransform component = gameObject.GetComponent<RectTransform>();
    if ((Object) component == (Object) null)
    {
      Debug.LogError((object) (name + " prefab is missing RectTransform component!"));
    }
    else
    {
      component.localScale = Vector3.one;
      component.pivot = pivot;
      component.anchorMin = anchorMin;
      component.anchorMax = anchorMax;
      component.anchoredPosition = anchoredPosition;
    }
    return gameObject;
  }

  public static GameObject InstantiateGUIObject_Pre<T>(
    GameObject prefab,
    Transform parent,
    string name)
    where T : Component
  {
    if ((Object) prefab == (Object) null)
    {
      Debug.LogError((object) (name + " prefab is null!"));
      return (GameObject) null;
    }
    GameObject gameObject = Object.Instantiate<GameObject>(prefab);
    if (!string.IsNullOrEmpty(name))
      gameObject.name = name;
    T component = gameObject.GetComponent<T>();
    if ((Object) component == (Object) null)
    {
      Debug.LogError((object) $"{name} prefab is missing the {((object) component).GetType().ToString()} component!");
      return (GameObject) null;
    }
    if ((Object) parent != (Object) null)
      gameObject.transform.SetParent(parent, false);
    return gameObject;
  }

  public static Vector3 GetPointOnRectEdge(RectTransform rectTransform, Vector2 dir)
  {
    if ((Object) rectTransform == (Object) null)
      return Vector3.zero;
    if (dir != Vector2.zero)
      dir /= Mathf.Max(Mathf.Abs(dir.x), Mathf.Abs(dir.y));
    Rect rect = rectTransform.rect;
    dir = rect.center + Vector2.Scale(rect.size, dir * 0.5f);
    return (Vector3) dir;
  }

  public static Rect GetWorldSpaceRect(RectTransform rt)
  {
    if ((Object) rt == (Object) null)
      return new Rect();
    Rect rect = rt.rect;
    Vector2 vector2_1 = (Vector2) rt.TransformPoint((Vector3) new Vector2(rect.xMin, rect.yMin));
    Vector2 vector2_2 = (Vector2) rt.TransformPoint((Vector3) new Vector2(rect.xMin, rect.yMax));
    Vector2 vector2_3 = (Vector2) rt.TransformPoint((Vector3) new Vector2(rect.xMax, rect.yMin));
    return new Rect(vector2_1.x, vector2_1.y, vector2_3.x - vector2_1.x, vector2_2.y - vector2_1.y);
  }

  public static Rect TransformRectTo(Transform from, Transform to, Rect rect)
  {
    Vector3 position1;
    Vector3 position2;
    Vector3 position3;
    if ((Object) from != (Object) null)
    {
      position1 = from.TransformPoint((Vector3) new Vector2(rect.xMin, rect.yMin));
      position2 = from.TransformPoint((Vector3) new Vector2(rect.xMin, rect.yMax));
      position3 = from.TransformPoint((Vector3) new Vector2(rect.xMax, rect.yMin));
    }
    else
    {
      position1 = (Vector3) new Vector2(rect.xMin, rect.yMin);
      position2 = (Vector3) new Vector2(rect.xMin, rect.yMax);
      position3 = (Vector3) new Vector2(rect.xMax, rect.yMin);
    }
    if ((Object) to != (Object) null)
    {
      position1 = to.InverseTransformPoint(position1);
      position2 = to.InverseTransformPoint(position2);
      position3 = to.InverseTransformPoint(position3);
    }
    return new Rect(position1.x, position1.y, position3.x - position1.x, position1.y - position2.y);
  }

  public static Rect InvertY(Rect rect) => new Rect(rect.xMin, rect.yMin, rect.width, -rect.height);

  public static void SetInteractable(Selectable selectable, bool state, bool playTransition)
  {
    if ((Object) selectable == (Object) null)
      return;
    if (!playTransition)
    {
      if (selectable.transition == Selectable.Transition.ColorTint)
      {
        ColorBlock colors = selectable.colors;
        float fadeDuration = colors.fadeDuration;
        colors.fadeDuration = 0.0f;
        selectable.colors = colors;
        selectable.interactable = state;
        colors.fadeDuration = fadeDuration;
        selectable.colors = colors;
      }
      else
        selectable.interactable = state;
    }
    else
      selectable.interactable = state;
  }
}
