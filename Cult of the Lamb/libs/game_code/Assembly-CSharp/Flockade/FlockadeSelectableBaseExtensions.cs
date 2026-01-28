// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeSelectableBaseExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace Flockade;

public static class FlockadeSelectableBaseExtensions
{
  public static Dictionary<(FlockadeSelectableBase, object), UnityAction> _CACHED_ON_CLICK = new Dictionary<(FlockadeSelectableBase, object), UnityAction>();
  public static Dictionary<(FlockadeSelectableBase, object), Action> _CACHED_ON_SELECTED = new Dictionary<(FlockadeSelectableBase, object), Action>();

  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
  public static void ResetCaches()
  {
    FlockadeSelectableBaseExtensions._CACHED_ON_CLICK.Clear();
    FlockadeSelectableBaseExtensions._CACHED_ON_SELECTED.Clear();
  }

  public static void SetConfirmable<T>(this IEnumerable<T> self, bool confirmable) where T : FlockadeSelectableBase
  {
    foreach (T obj in self)
      obj.Selectable.Confirmable = confirmable;
  }

  public static void SetInteractable<T>(
    this IEnumerable<T> self,
    bool interactable,
    Color highlight = default (Color),
    Action<T> onClick = null,
    Action<T> onSelected = null)
    where T : FlockadeSelectableBase
  {
    foreach (T obj in self)
    {
      T selectable = obj;
      ((T) selectable).SetInteractable(interactable, highlight);
      if (interactable)
      {
        if (onClick != null)
        {
          UnityAction call;
          if (!FlockadeSelectableBaseExtensions._CACHED_ON_CLICK.TryGetValue(((FlockadeSelectableBase) selectable, (object) onClick), out call))
            FlockadeSelectableBaseExtensions._CACHED_ON_CLICK.Add(((FlockadeSelectableBase) selectable, (object) onClick), call = (UnityAction) (() => onClick(selectable)));
          ((T) selectable).Selectable.onClick.AddListener(call);
        }
        if (onSelected != null)
        {
          Action action;
          if (!FlockadeSelectableBaseExtensions._CACHED_ON_SELECTED.TryGetValue(((FlockadeSelectableBase) selectable, (object) onSelected), out action))
            FlockadeSelectableBaseExtensions._CACHED_ON_SELECTED.Add(((FlockadeSelectableBase) selectable, (object) onSelected), action = (Action) (() => onSelected(selectable)));
          ((T) selectable).Selectable.OnSelected += action;
        }
      }
      else
      {
        UnityAction call;
        if (onClick != null && FlockadeSelectableBaseExtensions._CACHED_ON_CLICK.Remove(((FlockadeSelectableBase) selectable, (object) onClick), ref call))
          ((T) selectable).Selectable.onClick.RemoveListener(call);
        Action action;
        if (onSelected != null && FlockadeSelectableBaseExtensions._CACHED_ON_SELECTED.Remove(((FlockadeSelectableBase) selectable, (object) onSelected), ref action))
          ((T) selectable).Selectable.OnSelected -= action;
      }
    }
  }
}
