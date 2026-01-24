// Decompiled with JetBrains decompiler
// Type: BlackSoulUpdater
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BlackSoulUpdater : MonoBehaviour
{
  public static List<BlackSoul> BlackSouls = new List<BlackSoul>();
  public static BlackSoulUpdater instance;
  public static GameObject blacksoulupdateGO;
  public bool TickTack;

  public static BlackSoulUpdater Instance
  {
    get
    {
      if ((Object) BlackSoulUpdater.instance == (Object) null)
      {
        BlackSoulUpdater.blacksoulupdateGO = new GameObject("blacksoulupdate");
        BlackSoulUpdater.instance = BlackSoulUpdater.blacksoulupdateGO.AddComponent<BlackSoulUpdater>();
      }
      return BlackSoulUpdater.instance;
    }
  }

  public void Clear()
  {
    for (int index = BlackSoulUpdater.BlackSouls.Count - 1; index >= 0; --index)
      BlackSoulUpdater.BlackSouls[index].DisableMe();
    BlackSoulUpdater.BlackSouls.Clear();
  }

  public void OnDestroy()
  {
    BlackSoulUpdater.instance = (BlackSoulUpdater) null;
    this.Clear();
  }
}
