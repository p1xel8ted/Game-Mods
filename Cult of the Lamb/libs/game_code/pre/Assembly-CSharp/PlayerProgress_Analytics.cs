// Decompiled with JetBrains decompiler
// Type: PlayerProgress_Analytics
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private float updateInterval = 0.5f;
  private float lastInterval;
  private int frames;
  private float fps;

  public override void Start()
  {
    for (int index = 0; index < this.ItemsToCheck.Count; ++index)
      this.parameters.Add(InventoryItem.Name(this.ItemsToCheck[index]), (object) 0);
    this.parameters.Add("TimeInDungeon", (object) 0);
    this.parameters.Add("TimeInGame", (object) 0);
    this.parameters.Add("DeathsInDungeon", (object) 0);
    this.parameters.Add("FollowerCount", (object) 0);
    this.parameters.Add("DungeonRun", (object) 0);
    this.lastInterval = Time.realtimeSinceStartup;
    this.frames = 0;
  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.F10))
      PlayerProgress_Analytics.Dispatch(Inventory.itemsDungeon);
    if (Application.isEditor)
    {
      DataManager instance = DataManager.Instance;
      this.dungeonRun = instance.dungeonRun;
      this.DeathsInDungeon = instance.playerDeaths;
      this.FollowerCount = instance.Followers.Count;
      this.TimeInDungeon = GameManager.TimeInDungeon;
      this.TimeInGame = instance.TimeInGame;
    }
    ++this.frames;
    float realtimeSinceStartup = Time.realtimeSinceStartup;
    if ((double) realtimeSinceStartup > (double) this.lastInterval + (double) this.updateInterval)
    {
      this.fps = (float) this.frames / (realtimeSinceStartup - this.lastInterval);
      this.frames = 0;
      this.lastInterval = realtimeSinceStartup;
    }
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
