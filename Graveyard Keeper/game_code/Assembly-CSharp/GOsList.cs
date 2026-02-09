// Decompiled with JetBrains decompiler
// Type: GOsList
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class GOsList
{
  [SerializeField]
  public List<GameObject> list;

  public GOsList() => this.list = new List<GameObject>();

  public GOsList(List<GameObject> t_list)
  {
    this.list = t_list == null ? new List<GameObject>() : t_list;
  }
}
