// Decompiled with JetBrains decompiler
// Type: Singleton`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public abstract class Singleton<T> where T : new()
{
  public static T _instance;

  public static T Instance
  {
    get
    {
      if ((object) Singleton<T>._instance == null)
        Singleton<T>._instance = new T();
      return Singleton<T>._instance;
    }
  }
}
