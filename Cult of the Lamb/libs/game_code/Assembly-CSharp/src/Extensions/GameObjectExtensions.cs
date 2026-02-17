// Decompiled with JetBrains decompiler
// Type: src.Extensions.GameObjectExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace src.Extensions;

public static class GameObjectExtensions
{
  public static T Instantiate<T>(this T component) where T : MonoBehaviour
  {
    return Object.Instantiate<GameObject>(component.gameObject).GetComponent<T>();
  }

  public static T Instantiate<T>(this T component, Transform parent, bool worldPositionStays = true) where T : MonoBehaviour
  {
    T obj = component.Instantiate<T>();
    obj.transform.SetParent(parent, worldPositionStays);
    obj.transform.localScale = Vector3.one;
    return obj;
  }
}
