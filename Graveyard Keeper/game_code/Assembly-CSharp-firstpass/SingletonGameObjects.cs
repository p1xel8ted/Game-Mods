// Decompiled with JetBrains decompiler
// Type: SingletonGameObjects
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SingletonGameObjects : MonoBehaviour
{
  public static bool _me_is_set = false;
  public static SingletonGameObjects _me = (SingletonGameObjects) null;
  public static Dictionary<System.Type, Component> _members = new Dictionary<System.Type, Component>();

  public static SingletonGameObjects me
  {
    get
    {
      if (!SingletonGameObjects._me_is_set)
      {
        GameObject target = new GameObject("~ Singletons");
        SingletonGameObjects._me = target.AddComponent<SingletonGameObjects>();
        SingletonGameObjects._me_is_set = true;
        UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) target);
      }
      return SingletonGameObjects._me;
    }
  }

  public static T FindOrCreate<T>() where T : Component
  {
    Component orCreate1;
    if (SingletonGameObjects._members.TryGetValue(typeof (T), out orCreate1))
      return (T) orCreate1;
    GameObject gameObject = new GameObject(typeof (T).ToString());
    gameObject.transform.SetParent(SingletonGameObjects.me.transform, false);
    T orCreate2 = gameObject.AddComponent<T>();
    SingletonGameObjects._members.Add(typeof (T), (Component) orCreate2);
    return orCreate2;
  }
}
