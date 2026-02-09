// Decompiled with JetBrains decompiler
// Type: SerializedWalker
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DungeonGenerator;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class SerializedWalker
{
  [SerializeField]
  public int thickness = 4;
  [SerializeField]
  public int step_length = 4;
  [SerializeField]
  public int max_steps_between_rooms = 3;
  [SerializeField]
  public int min_length = 32 /*0x20*/;
  [SerializeField]
  public int max_length = 512 /*0x0200*/;
  [SerializeField]
  public List<SerializedRoom> rooms = new List<SerializedRoom>();
  [SerializeField]
  public DungeonWalker.ActionChances action_chances = new DungeonWalker.ActionChances();
}
