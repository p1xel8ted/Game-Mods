// Decompiled with JetBrains decompiler
// Type: HUDTaskItemGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class HUDTaskItemGUI : MonoBehaviour
{
  public UILabel txt;
  public Transform marker_point;
  public UILabel quest_marker;
  [NonSerialized]
  public KnownNPC.TaskState linked_task;
}
