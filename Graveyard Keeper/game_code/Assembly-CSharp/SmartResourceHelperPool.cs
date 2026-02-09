// Decompiled with JetBrains decompiler
// Type: SmartResourceHelperPool
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SmartResourceHelperPool
{
  public System.Type type;
  public Dictionary<string, UnityEngine.Object> loaded = new Dictionary<string, UnityEngine.Object>();

  public SmartResourceHelperPool(System.Type t) => this.type = t;

  public UnityEngine.Object GetObject(string res_name)
  {
    UnityEngine.Object object1;
    if (this.loaded.TryGetValue(res_name, out object1))
      return object1;
    if (SmartResourceHelper.me.queue.ContainsKey(res_name))
      SmartResourceHelper.me.queue.Remove(res_name);
    UnityEngine.Object object2 = Resources.Load(res_name);
    if (object2 == (UnityEngine.Object) null)
      Debug.LogError((object) $"Error loading resource \"{res_name}\"");
    this.loaded.Add(res_name, object2);
    return object2;
  }
}
