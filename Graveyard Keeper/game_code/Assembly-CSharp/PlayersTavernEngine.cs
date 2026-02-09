// Decompiled with JetBrains decompiler
// Type: PlayersTavernEngine
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class PlayersTavernEngine
{
  public const string IDLE_POINT_PREFIX = "players_tavern_idle_";
  public const string FORCE_SHUFFLE_FLAG = "force_tavern_shuffle";
  public const string TAVERN_VISITOR_TAG = "npc_event_visitor";
  public const string DO_ROLL_ANIIM = "do_roll_anim";
  public string[] TAVERN_VISITORS = new string[5]
  {
    "npc_tavern_visitor_1",
    "npc_tavern_visitor_2",
    "npc_tavern_visitor_3",
    "npc_tavern_visitor_4",
    "npc_tavern_visitor_5"
  };
  public string[] ITEMS_SELLING_IN_TAVERN = new string[10]
  {
    "cup_beer:1",
    "cup_beer:2",
    "cup_beer:3",
    "cup_mead:1",
    "cup_mead:2",
    "cup_mead:3",
    "bottle_red_vine:1",
    "bottle_red_vine:2",
    "bottle_red_vine:3",
    "bottle_booz_xxx"
  };
  public const float TAVERN_SELLING_COEFF = 1f;
  public const float MONEY_TO_REPUTATION = 0.01f;
  public const float VISITORS_RATIO = 0.6f;
  [SerializeField]
  public List<int> available_idle_points = new List<int>();
  [SerializeField]
  public List<long> visitors_unique_ids = new List<long>();
  [NonSerialized]
  public List<WorldGameObject> visitors = new List<WorldGameObject>();
  [SerializeField]
  public List<PlayersTavernEngine.GDPointLock> locks = new List<PlayersTavernEngine.GDPointLock>();
  [SerializeField]
  public bool visitors_temporarily_removed;

  public void Init()
  {
    if (this.available_idle_points == null)
    {
      Debug.LogError((object) "PlayersTavernEngine error: available_idle_points list was null! Call Bulat.");
      this.available_idle_points = new List<int>();
    }
    if (this.visitors_unique_ids == null)
    {
      Debug.LogError((object) "PlayersTavernEngine error: visitors_unique_ids list was null! Call Bulat.");
      this.visitors_unique_ids = new List<long>();
    }
    this.FillVisitorsList();
    if (this.locks == null)
    {
      Debug.LogError((object) "PlayersTavernEngine error: locks was null! Call Bulat.");
      this.locks = new List<PlayersTavernEngine.GDPointLock>();
      foreach (int availableIdlePoint in this.available_idle_points)
        this.locks.Add(new PlayersTavernEngine.GDPointLock()
        {
          num = availableIdlePoint,
          locker_unique_id = -1L
        });
    }
    this.visitors_temporarily_removed = false;
    Debug.Log((object) $"Tavern Engine: initialized. available_idle_points={this.available_idle_points.Count}, visitors_unique_ids={this.visitors_unique_ids.Count}, locks={this.locks.Count}");
  }

  public void FillVisitorsList()
  {
    if (this.visitors_unique_ids == null)
    {
      Debug.LogError((object) "FATAL ERROR: PlayersTavernEngine visitors_unique_ids list is null!");
    }
    else
    {
      this.visitors = new List<WorldGameObject>();
      foreach (long visitorsUniqueId in this.visitors_unique_ids)
      {
        WorldGameObject objectByUniqueId = WorldMap.GetWorldGameObjectByUniqueId(visitorsUniqueId);
        if ((UnityEngine.Object) objectByUniqueId == (UnityEngine.Object) null)
          Debug.LogError((object) ("FATAL ERROR: not found visitor with unique_id = " + visitorsUniqueId.ToString()));
        else
          this.visitors.Add(objectByUniqueId);
      }
      Debug.Log((object) $"Filled visitors list. Count={this.visitors.Count}");
    }
  }

  public void AddNewVisitor(WorldGameObject visitor_wgo)
  {
    if ((UnityEngine.Object) visitor_wgo == (UnityEngine.Object) null || visitor_wgo.unique_id < 0L)
    {
      Debug.LogError((object) ("AddNewVisitor error: " + ((UnityEngine.Object) visitor_wgo == (UnityEngine.Object) null ? "visitor is null!" : "visitor_wgo.unique_id < 0")));
    }
    else
    {
      this.visitors_unique_ids.Add(visitor_wgo.unique_id);
      this.visitors.Add(visitor_wgo);
      Debug.Log((object) $"Tavern Engine: added new visitor: id=\"{visitor_wgo.obj_id}\", unique_id=\"{visitor_wgo.unique_id}\"");
    }
  }

  public void RemoveVisitor(WorldGameObject visitor_wgo)
  {
    if ((UnityEngine.Object) visitor_wgo == (UnityEngine.Object) null || visitor_wgo.unique_id < 0L)
    {
      Debug.LogError((object) ("RemoveVisitor error: " + ((UnityEngine.Object) visitor_wgo == (UnityEngine.Object) null ? "visitor is null!" : "visitor_wgo.unique_id < 0")));
    }
    else
    {
      if (!this.visitors_unique_ids.Contains(visitor_wgo.unique_id))
        Debug.LogError((object) $"RemoveVisitor error: not found visitor unique id \"{visitor_wgo.unique_id}\"");
      else
        this.visitors_unique_ids.Remove(visitor_wgo.unique_id);
      if (!this.visitors.Contains(visitor_wgo))
      {
        bool flag = false;
        for (int index = 0; index < this.visitors.Count; ++index)
        {
          if (this.visitors[index].unique_id == visitor_wgo.unique_id)
          {
            this.visitors.RemoveAt(index);
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          Debug.LogError((object) $"RemoveVisitor error: not found visitor \"{visitor_wgo.obj_id}\" with unique_id=\"{visitor_wgo.unique_id}\" in WGOs list. Doing rescan.");
          this.FillVisitorsList();
        }
      }
      else
        this.visitors.Remove(visitor_wgo);
      bool flag1 = false;
      foreach (PlayersTavernEngine.GDPointLock gdPointLock in this.locks)
      {
        if (gdPointLock.locker_unique_id == visitor_wgo.unique_id)
        {
          gdPointLock.locker_unique_id = -1L;
          Debug.Log((object) $"Tavern Engine: unlocked GDPoint num={gdPointLock.num}");
          flag1 = true;
          break;
        }
      }
      if (!flag1)
        Debug.LogError((object) $"RemoveVisitor error: not removed lock for visitor \"{visitor_wgo.obj_id}\", unique_id=\"{visitor_wgo.unique_id}\"");
      else
        Debug.Log((object) $"Successfully removed visitor id=\"{visitor_wgo.obj_id}\" with unique_id=\"{visitor_wgo.unique_id}\"");
    }
  }

  public List<int> GetAvailablePointsList() => this.available_idle_points;

  public void AddGDPoint(GDPoint gd_point)
  {
    if ((UnityEngine.Object) gd_point == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "AddGDPoint error: gd_point is null!");
    }
    else
    {
      string[] strArray = gd_point.gd_tag.Split(new char[1]
      {
        '_'
      }, StringSplitOptions.RemoveEmptyEntries);
      int result;
      if (!int.TryParse(strArray[strArray.Length - 1], out result))
        Debug.LogError((object) $"AddGDPoint error: unknown point num gd_point=\"{gd_point.gd_tag}\", num=\"{strArray[strArray.Length - 1]}\"");
      else if (this.available_idle_points.Contains(result))
      {
        Debug.LogError((object) $"TavernEngine.AddGDPoint error: trying to add point that already exist: \"{gd_point.gd_tag}\", num=\"{result}\"");
      }
      else
      {
        this.available_idle_points.Add(result);
        this.locks.Add(new PlayersTavernEngine.GDPointLock()
        {
          num = result,
          locker_unique_id = -1L
        });
        Debug.Log((object) $"TavernEngine: added GDPoint \"{gd_point.gd_tag}\", num=\"{result}\"");
      }
    }
  }

  public bool TryGetAvailablePoint(out GDPoint out_point, long lock_by = -1)
  {
    out_point = (GDPoint) null;
    List<PlayersTavernEngine.GDPointLock> gdPointLockList = new List<PlayersTavernEngine.GDPointLock>();
    foreach (PlayersTavernEngine.GDPointLock gdPointLock in this.locks)
    {
      if (!gdPointLock.is_locked)
        gdPointLockList.Add(gdPointLock);
    }
    if (gdPointLockList.Count == 0)
      return false;
    PlayersTavernEngine.GDPointLock gdPointLock1 = gdPointLockList.Count == 1 ? gdPointLockList[0] : gdPointLockList[UnityEngine.Random.Range(0, gdPointLockList.Count)];
    out_point = WorldMap.GetGDPointByGDTag("players_tavern_idle_" + gdPointLock1.num.ToString());
    if ((UnityEngine.Object) out_point == (UnityEngine.Object) null)
      return false;
    if (lock_by > 0L)
    {
      foreach (PlayersTavernEngine.GDPointLock gdPointLock2 in this.locks)
      {
        if (gdPointLock2.locker_unique_id == lock_by)
        {
          gdPointLock2.locker_unique_id = -1L;
          Debug.Log((object) $"Tavern Engine: unlocked GDPoint num={gdPointLock2.num}");
        }
      }
      gdPointLock1.locker_unique_id = lock_by;
      Debug.Log((object) $"Tavern Engine: locked GDPoint num={gdPointLock1.num}, locker_id={gdPointLock1.locker_unique_id}");
    }
    return true;
  }

  public void TemporarilyRemoveVisitors()
  {
    this.visitors_temporarily_removed = true;
    GDPoint gdPointByGdTag = WorldMap.GetGDPointByGDTag("default_destroy_point");
    if ((UnityEngine.Object) gdPointByGdTag == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "TemporarilyRemoveVisitors error: stock)point not found!");
    }
    else
    {
      Vector2 position = (Vector2) gdPointByGdTag.transform.position;
      foreach (WorldGameObject visitor in this.visitors)
      {
        visitor.transform.position = (Vector3) position;
        if (!visitor.obj_def.IsCharacter())
        {
          Debug.LogError((object) "WGO is not character!");
        }
        else
        {
          BaseCharacterComponent character = visitor.components.character;
          if (character == null)
            Debug.LogError((object) "BaseCharacterComponent is null!");
          else
            character.StopMovement();
        }
      }
      Debug.Log((object) $"Tavern Engine: temporarily removed visitors. Count={this.visitors.Count}");
    }
  }

  public void PlaceVisitorsBackAfterEvent()
  {
    int num = 0;
    foreach (PlayersTavernEngine.GDPointLock gdPointLock in this.locks)
    {
      if (gdPointLock.is_locked)
      {
        GDPoint gdPointByGdTag = WorldMap.GetGDPointByGDTag("players_tavern_idle_" + gdPointLock.num.ToString());
        if ((UnityEngine.Object) gdPointByGdTag == (UnityEngine.Object) null)
        {
          Debug.LogError((object) $"FATAL ERROR: PlaceVisitorsBackAfterEvent error: not found GDPoint with tag \"{"players_tavern_idle_"}{gdPointLock}\"!");
        }
        else
        {
          gdPointLock.locker.transform.position = gdPointByGdTag.transform.position;
          gdPointLock.locker.RefreshPositionCache();
          gdPointLock.locker.gameObject.SetActive(true);
          gdPointLock.locker.OnCameToGDPoint(gdPointByGdTag);
          ++num;
        }
      }
    }
    this.visitors_temporarily_removed = false;
    Debug.Log((object) $"Tavern Engine: placed back visitors after event. Count={num}");
  }

  public List<string> GetAvailableIdlePoints(TavernEventDefinition event_def)
  {
    List<string> availableIdlePoints = new List<string>();
    List<int> availablePointsList = this.GetAvailablePointsList();
    if (availablePointsList == null || availablePointsList.Count == 0)
      return new List<string>();
    List<string> stringList = new List<string>();
    List<string> collection = new List<string>();
    if (event_def != null)
    {
      stringList.AddRange((IEnumerable<string>) event_def.idle_points_blacklist);
      collection.AddRange((IEnumerable<string>) event_def.idle_points_whitelist);
    }
    foreach (int num in availablePointsList)
    {
      string str = "players_tavern_idle_" + num.ToString();
      if (!stringList.Contains(str))
        availableIdlePoints.Add(str);
    }
    foreach (string str in collection)
    {
      if (!availableIdlePoints.Contains(str))
        availableIdlePoints.Remove(str);
    }
    availableIdlePoints.InsertRange(0, (IEnumerable<string>) collection);
    return availableIdlePoints;
  }

  public static float CalculateCorrecterCoeff(float quality, float average_item_price)
  {
    float num1 = (float) (-0.078947365283966064 * (double) quality + 7.3947367668151855);
    float num2 = quality * num1 * average_item_price;
    if ((double) num2 < 0.01)
      num2 = 100f;
    return 100f / num2;
  }

  public static float CalculateAlcoholSellingBonus(float money_earned)
  {
    float totalQuality = WorldZone.GetZoneByID("players_tavern").GetTotalQuality();
    float num = 0.0f;
    if ((double) totalQuality > 30.0 && (double) totalQuality <= 55.0)
      num = 0.1f;
    else if ((double) totalQuality > 55.0)
      num = 0.2f;
    return num * money_earned;
  }

  [Serializable]
  public class GDPointLock
  {
    public int num;
    public long locker_unique_id;
    public WorldGameObject _locker;

    public WorldGameObject locker
    {
      get
      {
        if (this.locker_unique_id == -1L)
          return (WorldGameObject) null;
        if ((UnityEngine.Object) this._locker == (UnityEngine.Object) null)
          this._locker = WorldMap.GetWorldGameObjectByUniqueId(this.locker_unique_id);
        return this._locker;
      }
      set
      {
        if ((UnityEngine.Object) value == (UnityEngine.Object) null)
        {
          this._locker = (WorldGameObject) null;
          this.locker_unique_id = -1L;
          Debug.Log((object) $"Tavern Engine: unlocked GDPoint num={this.num}");
        }
        else
        {
          this.locker_unique_id = value.unique_id;
          Debug.Log((object) $"Tavern Engine: locked GDPoint num={this.num}, locker_id={this.locker_unique_id}");
          this._locker = value;
        }
      }
    }

    public bool is_locked => this.locker_unique_id > 0L;
  }
}
