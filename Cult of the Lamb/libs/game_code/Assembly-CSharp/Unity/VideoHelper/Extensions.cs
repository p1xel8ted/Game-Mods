// Decompiled with JetBrains decompiler
// Type: Unity.VideoHelper.Extensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Unity.VideoHelper;

public static class Extensions
{
  public static TComponent GetOrAddComponent<TComponent>(this GameObject target) where TComponent : Component
  {
    TComponent orAddComponent = !((UnityEngine.Object) target == (UnityEngine.Object) null) ? target.GetComponent<TComponent>() : throw new ArgumentNullException("Target gameobject must not be null.");
    if ((UnityEngine.Object) orAddComponent == (UnityEngine.Object) null)
      orAddComponent = target.AddComponent<TComponent>();
    return orAddComponent;
  }

  public static void SetGameObjectActive(this Component component, bool isActive)
  {
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    component.gameObject.SetActive(isActive);
  }

  public static void OnClick(this Component component, UnityAction action)
  {
    if ((UnityEngine.Object) component == (UnityEngine.Object) null || action == null)
      return;
    Button componentInParent = component.gameObject.GetComponentInParent<Button>();
    if ((UnityEngine.Object) componentInParent != (UnityEngine.Object) null)
      componentInParent.onClick.AddListener(action);
    else
      component.gameObject.GetOrAddComponent<ClickRouter>().OnClick.AddListener(action);
  }

  public static void OnDoubleClick(this Component component, UnityAction action)
  {
    component.gameObject.GetOrAddComponent<ClickRouter>().OnDoubleClick.AddListener(action);
  }
}
