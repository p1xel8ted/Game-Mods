// Decompiled with JetBrains decompiler
// Type: Stats
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class Stats
{
  public static bool _inited;

  public static void Init()
  {
    if (Stats._inited)
      return;
    Stats._inited = true;
  }

  public static void DesignEvent(string event_name)
  {
  }

  public static void DesignEvent(string event_name, float value)
  {
  }

  public static void PlayerAddMoney(float amount, string trader_name)
  {
    if ((double) amount > 0.0)
      return;
    Debug.LogWarning((object) $"PlayerAddMoney: Stats error: can't send a zero or negative number, trader_name = {trader_name}, amount = {amount.ToString()}");
  }

  public static void PlayerDecMoney(float amount, string trader_name)
  {
    if ((double) amount > 0.0)
      return;
    Debug.LogWarning((object) $"PlayerDecMoney: Stats error: can't send a zero or negative number, trader_name = {trader_name}, amount = {amount.ToString()}");
  }

  public static void ResourceEvent(
    bool is_source,
    string resource_name,
    float amount,
    string item_type)
  {
    if ((double) amount > 0.0)
      return;
    Debug.LogWarning((object) $"ResourceEvent: Stats error: can't send a zero or negative number, resource_name = {resource_name}, amount = {amount.ToString()}");
  }
}
