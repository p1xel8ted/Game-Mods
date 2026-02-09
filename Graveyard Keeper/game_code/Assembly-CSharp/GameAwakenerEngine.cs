// Decompiled with JetBrains decompiler
// Type: GameAwakenerEngine
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using LinqTools;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

#nullable disable
[DefaultExecutionOrder(-9999)]
public class GameAwakenerEngine : MonoBehaviour
{
  public const bool REALTIME_AWAKENING = false;
  public static GameAwakenerEngine _me = (GameAwakenerEngine) null;
  public static bool _inited = false;
  public List<GameAwakenerEngine.ObjectData> _objs = new List<GameAwakenerEngine.ObjectData>();
  public Stopwatch _sw = new Stopwatch();
  public float _frame_len;
  public float activation_sqr_radius = 3400000f;
  public static int _prewarm_iterator = 0;
  public static bool prewarm_finished = true;
  public static int left_objects_to_prewarm = 0;
  public static int was_objects_to_prewarm = 0;

  public static GameAwakenerEngine me
  {
    get
    {
      if (!GameAwakenerEngine._inited)
      {
        GameAwakenerEngine._inited = true;
        GameAwakenerEngine._me = new GameObject(nameof (GameAwakenerEngine)).AddComponent<GameAwakenerEngine>();
      }
      return GameAwakenerEngine._me;
    }
  }

  public static void Init()
  {
    UnityEngine.Debug.Log((object) "GameAwakenerEngine.Init", (Object) GameAwakenerEngine.me);
  }

  public static void ScanMap()
  {
    List<SimplifiedObject> list = ((IEnumerable<SimplifiedObject>) MainGame.me.world_root.GetComponentsInChildren<SimplifiedObject>(true)).ToList<SimplifiedObject>();
    GameAwakenerEngine.me._objs.Clear();
    foreach (SimplifiedObject simplifiedObject in list)
      GameAwakenerEngine.me._objs.Add(new GameAwakenerEngine.ObjectData()
      {
        obj = simplifiedObject,
        x = simplifiedObject.transform.position.x,
        y = simplifiedObject.transform.position.y
      });
    GameAwakenerEngine.me.gameObject.SetActive(true);
  }

  public static void StartRestoringSimplifiedObjects()
  {
  }

  public static void Stop()
  {
    GameAwakenerEngine.me._objs.Clear();
    GameAwakenerEngine.me.gameObject.SetActive(false);
  }

  public void Update()
  {
    if (!GameAwakenerEngine.prewarm_finished)
    {
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      while (GameAwakenerEngine._prewarm_iterator < GameAwakenerEngine.me._objs.Count)
      {
        GameAwakenerEngine.me._objs[GameAwakenerEngine._prewarm_iterator].obj.Restore();
        --GameAwakenerEngine.left_objects_to_prewarm;
        ++GameAwakenerEngine._prewarm_iterator;
        if (stopwatch.ElapsedMilliseconds > 200L)
          return;
      }
      GameAwakenerEngine.me._objs.Clear();
      GameAwakenerEngine.prewarm_finished = true;
    }
    this.gameObject.SetActive(false);
  }

  public int RestoreNearObjects() => -1;

  public static void OnPlayerMoved()
  {
  }

  public static void PreWarm()
  {
    GameAwakenerEngine.was_objects_to_prewarm = GameAwakenerEngine.left_objects_to_prewarm = GameAwakenerEngine.me._objs.Count;
    GameAwakenerEngine._prewarm_iterator = 0;
    GameAwakenerEngine.prewarm_finished = false;
    GameAwakenerEngine.me.gameObject.SetActive(true);
  }

  public struct ObjectData
  {
    public SimplifiedObject obj;
    public float x;
    public float y;
  }
}
