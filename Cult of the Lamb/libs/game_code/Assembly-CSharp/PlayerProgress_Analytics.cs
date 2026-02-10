// Decompiled with JetBrains decompiler
// Type: PlayerProgress_Analytics
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class PlayerProgress_Analytics : MonoSingleton<PlayerProgress_Analytics>
{
  public List<InventoryItem.ITEM_TYPE> ItemsToCheck;
  public List<InventoryItem> itemsDungeon = new List<InventoryItem>();
  public const string _Name = "player_progress";
  public Dictionary<string, object> parameters = new Dictionary<string, object>();
  public int DeathsInDungeon;
  public int dungeonRun;
  public int FollowerCount;
  public float TimeInDungeon;
  public float TimeInGame;
  public float updateInterval = 0.5f;
  public float lastInterval;
  public int frames;
  public float fps;

  public override void Start()
  {
    this.parameters.Add("TimeInDungeon", (object) 0);
    this.parameters.Add("TimeInGame", (object) 0);
    this.parameters.Add("DeathsInDungeon", (object) 0);
    this.parameters.Add("FollowerCount", (object) 0);
    this.parameters.Add("DungeonRun", (object) 0);
    this.lastInterval = Time.realtimeSinceStartup;
    this.frames = 0;
  }

  public void Update()
  {
    if (!((Object) MonoSingleton<UIManager>.Instance != (Object) null) || MonoSingleton<UIManager>.Instance.IsPaused)
      return;
    DataManager.Instance.TimeInGame += Time.unscaledDeltaTime;
  }

  public void GameComplete()
  {
    string version = Application.version;
    Dictionary<string, object> dictionary = new Dictionary<string, object>()
    {
      {
        "TimeInDungeon",
        (object) DataManager.Instance.TimeInGame
      },
      {
        "FollowersDead",
        (object) DataManager.Instance.Followers_Dead.Count
      },
      {
        "Followers",
        (object) DataManager.Instance.Followers.Count
      }
    };
  }

  public void CultLeaderComplete(int _cultLeader)
  {
    string version = Application.version;
    Dictionary<string, object> dictionary = new Dictionary<string, object>()
    {
      {
        "TimeInDungeon",
        (object) DataManager.Instance.TimeInGame
      },
      {
        "Followers",
        (object) DataManager.Instance.Followers.Count
      },
      {
        "BossCompleted",
        (object) _cultLeader
      }
    };
  }

  public void LevelComplete(
    int _dungeon,
    int _dungeonFloor,
    int _woodCollected,
    int _goldCollected,
    int _foodCollected,
    int _kills,
    int _time,
    int damageTaken,
    bool _beatBoss)
  {
    string version = Application.version;
    Dictionary<string, object> dictionary = new Dictionary<string, object>()
    {
      {
        "DungeonLayer",
        (object) _dungeon
      },
      {
        "DungeonNode",
        (object) _dungeonFloor
      },
      {
        "WoodCollected",
        (object) _woodCollected
      },
      {
        "GoldCollected",
        (object) _goldCollected
      },
      {
        "FoodCollected",
        (object) _foodCollected
      },
      {
        "Kills",
        (object) _kills
      },
      {
        "Time",
        (object) _time
      },
      {
        "DamageTaken",
        (object) damageTaken
      },
      {
        "BeatBoss",
        (object) _beatBoss
      },
      {
        "TimeInDungeon",
        (object) DataManager.Instance.TimeInGame
      }
    };
  }

  public static bool Dispatch(List<InventoryItem> _itemsDungeon) => false;
}
