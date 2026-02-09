// Decompiled with JetBrains decompiler
// Type: Singleton`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
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
