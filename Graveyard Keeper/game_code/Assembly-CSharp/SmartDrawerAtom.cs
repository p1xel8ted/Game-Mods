// Decompiled with JetBrains decompiler
// Type: SmartDrawerAtom
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class SmartDrawerAtom
{
  public GameObject obj;
  public SmartDrawerAtom.SmartDrawingCondition condition;
  public float update_period;
  public float _cur_update_time;
  public int int_value;
  public int int_value_2;
  public string string_value = string.Empty;
  public float float_value;
  public float float_value_2;

  public bool CheckNeedRecalc(float delta_time)
  {
    this._cur_update_time += delta_time;
    if ((double) this._cur_update_time < (double) this.update_period)
      return false;
    this._cur_update_time = 0.0f;
    return true;
  }

  public enum SmartDrawingCondition
  {
    None = 0,
    OnCrafting = 1,
    ItemsEqual = 2,
    ItemsMoreEqual = 3,
    GameResEqual = 4,
    GameResMore = 5,
    DayTime_Day = 6,
    RainLess = 7,
    RainMore = 8,
    WindLess = 9,
    WindMore = 10, // 0x0000000A
    FogLess = 11, // 0x0000000B
    FogMore = 12, // 0x0000000C
    DayTime_Night = 13, // 0x0000000D
    ContainItem = 14, // 0x0000000E
    GameResBetween = 15, // 0x0000000F
    GameResLess = 16, // 0x00000010
    ConcreteItemsBetween = 17, // 0x00000011
    ConcreteItemsMore = 18, // 0x00000012
    NoLinkedWorker = 19, // 0x00000013
    PorterStationStateIsNot = 20, // 0x00000014
    HasLinkedWorker = 21, // 0x00000015
    TotalItemsCount = 22, // 0x00000016
    PlayerHasItem = 23, // 0x00000017
    PlayerGameResEqual = 24, // 0x00000018
    GDPointEnabled = 26, // 0x0000001A
    GDPointDisabled = 27, // 0x0000001B
    HasItemInInventoryByIndex = 28, // 0x0000001C
  }
}
