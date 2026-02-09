// Decompiled with JetBrains decompiler
// Type: GlobalSingletonObject
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[DefaultExecutionOrder(-9000)]
public class GlobalSingletonObject : MonoBehaviour
{
  public static GlobalSingletonObject _me;

  public void Awake()
  {
    if ((Object) GlobalSingletonObject._me == (Object) null)
      GlobalSingletonObject._me = this;
    else
      NGUITools.DestroyImmediate((Object) this.gameObject);
  }
}
