// Decompiled with JetBrains decompiler
// Type: MonoSingleton`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
  private static T _instance;

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
