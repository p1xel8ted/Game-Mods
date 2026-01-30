// Decompiled with JetBrains decompiler
// Type: MonoSingleton`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
  public static T _instance;

  public static T Instance
  {
    get
    {
      if ((Object) MonoSingleton<T>._instance == (Object) null)
        MonoSingleton<T>._instance = Object.FindObjectOfType<T>();
      return MonoSingleton<T>._instance;
    }
  }

  public virtual void Awake()
  {
    if ((Object) MonoSingleton<T>.Instance != (Object) this)
      Object.Destroy((Object) this.gameObject);
    else
      Object.DontDestroyOnLoad((Object) this);
  }

  public virtual void Start()
  {
  }
}
