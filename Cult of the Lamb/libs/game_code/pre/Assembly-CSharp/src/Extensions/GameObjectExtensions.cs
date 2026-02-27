// Decompiled with JetBrains decompiler
// Type: src.Extensions.GameObjectExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
