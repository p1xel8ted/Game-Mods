// Decompiled with JetBrains decompiler
// Type: PrayLogics
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class PrayLogics
{
  public static List<GameObject> _places_hi_priority = new List<GameObject>();
  public static List<GameObject> _places_low_priority = new List<GameObject>();
  public static List<GameObject> _places_very_low_priority = new List<GameObject>();
  public static GameObject _default_place = (GameObject) null;
  public static PrayLogics.PrayResult last_pray_result;
  public static List<Item> _sermon_drops = (List<Item>) null;

  public static void InitPrayPlacesList()
  {
    PrayLogics._places_hi_priority = WorldMap.GetGDPointsByGDTag("church_plc");
    PrayLogics._places_low_priority = WorldMap.GetGDPointsByGDTag("church_plc_2");
    PrayLogics._places_very_low_priority = WorldMap.GetGDPointsByGDTag("church_plc_3");
    PrayLogics._default_place = PrayLogics._places_very_low_priority.Count > 0 ? PrayLogics._places_very_low_priority.RandomElement<GameObject>() : PrayLogics._places_low_priority.RandomElement<GameObject>();
  }

  public static GameObject GetPrayPlace()
  {
    if (PrayLogics._places_hi_priority.Count > 0)
      return PrayLogics._places_hi_priority.RandomElement<GameObject>(true);
    if (PrayLogics._places_low_priority.Count > 0)
      return PrayLogics._places_low_priority.RandomElement<GameObject>(true);
    return PrayLogics._places_very_low_priority.Count > 0 ? PrayLogics._places_very_low_priority.RandomElement<GameObject>(true) : PrayLogics._default_place;
  }

  public static void SpreadFaithIncome(List<WorldGameObject> prayers, int faith)
  {
    if (prayers == null)
    {
      Debug.LogError((object) "SpreadFaithIncome error: prayers list is null");
    }
    else
    {
      for (int index = 0; index < prayers.Count; ++index)
      {
        if ((UnityEngine.Object) prayers[index] == (UnityEngine.Object) null)
        {
          prayers.RemoveAt(index);
          --index;
        }
      }
      for (int index = 0; index < faith; ++index)
      {
        WorldGameObject worldGameObject = prayers.RandomElement<WorldGameObject>();
        if (!((UnityEngine.Object) worldGameObject == (UnityEngine.Object) null))
          worldGameObject.AddToParams("_faith", 1f);
      }
    }
  }

  public static void SpreadMoneyIncome(
    List<WorldGameObject> prayers,
    float money,
    float success_percent = 1f)
  {
    if (prayers == null)
    {
      Debug.LogError((object) "SpreadMoneyIncome: prayers list is null");
    }
    else
    {
      List<WorldGameObject> list = new List<WorldGameObject>();
      foreach (WorldGameObject prayer in prayers)
      {
        if ((double) UnityEngine.Random.Range(0, 1) < (double) success_percent)
          list.Add(prayer);
      }
      float num1 = Mathf.Floor(money * 100f / (float) list.Count) / 100f;
      float num2 = 0.0f;
      foreach (WorldGameObject worldGameObject in list)
      {
        num2 += num1;
        double num3 = (double) num1;
        worldGameObject.AddToParams("_money", (float) num3);
      }
      while ((double) num2 < (double) money)
      {
        num2 += 0.01f;
        list.RandomElement<WorldGameObject>().AddToParams("_money", 0.01f);
      }
    }
  }

  public static PrayLogics.PrayResult CalculatePray(string pray_event_id)
  {
    PrayLogics.PrayResult pray = new PrayLogics.PrayResult();
    PrayEventDefinition data = GameBalance.me.GetData<PrayEventDefinition>(pray_event_id);
    pray.people = Mathf.Max(0, Mathf.RoundToInt(data.people.EvaluateFloat()));
    pray.faith = Mathf.Max(0, Mathf.RoundToInt(data.faith.EvaluateFloat()));
    pray.money = Mathf.Max(0.0f, data.money.EvaluateFloat());
    pray.pray_event_id = pray_event_id;
    CraftDefinition prayCraft = GUIElements.me.pray_craft.pray_craft;
    pray.pray_craft_id = prayCraft.id;
    pray.success_percent = prayCraft.needs_quality.EqualsTo(0.0f) ? 100 : Mathf.RoundToInt((float) ((double) GUIElements.me.pray_craft.GetCurrentZoneQuality() / (double) prayCraft.needs_quality * 100.0));
    if (pray.success_percent > 100)
      pray.success_percent = 100;
    else if (pray.success_percent < 0)
      pray.success_percent = 0;
    Debug.Log((object) ("res.success_percent = " + pray.success_percent.ToString()));
    pray.success = pray.success_percent == 100 || UnityEngine.Random.Range(0, 100) < pray.success_percent;
    PrayLogics._sermon_drops = new List<Item>();
    if (pray.success)
    {
      PrayLogics._sermon_drops.AddRange((IEnumerable<Item>) prayCraft.output);
      Item itemFromList1 = Item.GetItemFromList(PrayLogics._sermon_drops, "faith");
      if (itemFromList1 != null)
      {
        PrayLogics._sermon_drops.Remove(itemFromList1);
        pray.faith_bonus += itemFromList1.value;
      }
      Item itemFromList2 = Item.GetItemFromList(PrayLogics._sermon_drops, "money");
      if (itemFromList2 != null)
      {
        PrayLogics._sermon_drops.Remove(itemFromList2);
        pray.money_bonus += (float) itemFromList2.value / 100f;
      }
      if (!prayCraft.k_faith.EqualsTo(0.0f))
        pray.faith_bonus += Mathf.RoundToInt((float) pray.faith * prayCraft.k_faith);
      if (!prayCraft.k_money.EqualsTo(0.0f))
        pray.money_bonus += Mathf.Round((float) ((double) pray.money * (double) prayCraft.k_money * 100.0)) / 100f;
    }
    else
    {
      pray.faith_bonus = 0;
      pray.money_bonus = 0.0f;
    }
    Debug.Log((object) $"CalculatePray '{pray_event_id}' ppl = {pray.people.ToString()}, faith = {pray.faith.ToString()}, money = {pray.money.ToString()}, success = {pray.success.ToString()}");
    PrayLogics.last_pray_result = pray;
    return pray;
  }

  public static void DropPrayItems()
  {
    GDPoint gd_point = WorldMap.GetGDPointByName("faith_drop_point");
    float seconds = 0.0f;
    foreach (Item sermonDrop in PrayLogics._sermon_drops)
    {
      for (int index = 0; index < sermonDrop.value; ++index)
      {
        Item drop = new Item(sermonDrop) { value = 1 };
        GJTimer.AddTimer(seconds, (GJTimer.VoidDelegate) (() =>
        {
          DropResGameObject dropResGameObject = DropResGameObject.DropAndFly(gd_point.pos, drop, MainGame.me.world_root, (Vector2) gd_point.pos);
          Transform t = dropResGameObject.go_small_drop.transform;
          t.localPosition = new Vector3(0.0f, 5f);
          DOTween.To((DOGetter<float>) (() => t.localPosition.y), (DOSetter<float>) (y => t.localPosition = new Vector3(t.localPosition.x, y)), 0.0f, 1f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutBounce);
          Color color = dropResGameObject.sprite_res_item.color;
          dropResGameObject.sprite_res_item.color = new Color(dropResGameObject.sprite_res_item.color.r, dropResGameObject.sprite_res_item.color.g, dropResGameObject.sprite_res_item.color.b, 0.0f);
          dropResGameObject.sprite_res_item.DOColor(color, 0.3f);
        }));
        seconds += 0.2f;
      }
    }
  }

  [Serializable]
  public struct PrayResult
  {
    public int people;
    public int faith;
    public int faith_bonus;
    public float money;
    public float money_bonus;
    public bool success;
    public string pray_craft_id;
    public string pray_event_id;
    public int success_percent;
  }
}
