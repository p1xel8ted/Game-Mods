// Decompiled with JetBrains decompiler
// Type: AstarPath
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding;
using Pathfinding.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

#nullable disable
[HelpURL("http://arongranberg.com/astar/docs/class_astar_path.php")]
[AddComponentMenu("Pathfinding/Pathfinder")]
[ExecuteInEditMode]
public class AstarPath : MonoBehaviour
{
  public static AstarPath.AstarDistribution Distribution = AstarPath.AstarDistribution.WebsiteDownload;
  public static string Branch = "master_Pro";
  public AstarData astarData;
  public static AstarPath active;
  public bool showNavGraphs = true;
  public bool showUnwalkableNodes = true;
  public GraphDebugMode debugMode;
  public float debugFloor;
  public float debugRoof = 20000f;
  public bool manualDebugFloorRoof;
  public bool showSearchTree;
  public float unwalkableNodeDebugSize = 0.3f;
  public PathLog logPathResults = PathLog.Normal;
  public float maxNearestNodeDistance = 100f;
  public bool scanOnStartup = true;
  public bool fullGetNearestSearch;
  public bool prioritizeGraphs;
  public float prioritizeGraphsLimit = 1f;
  public AstarColor colorSettings;
  [SerializeField]
  public string[] tagNames;
  public Heuristic heuristic = Heuristic.Euclidean;
  public float heuristicScale = 1f;
  public ThreadCount threadCount;
  public float maxFrameTime = 1f;
  public int minAreaSize;
  public bool batchGraphUpdates;
  public float graphUpdateBatchingInterval = 0.2f;
  [NonSerialized]
  public float lastScanTime;
  [NonSerialized]
  public Path debugPath;
  [NonSerialized]
  public string inGameDebugPath;
  [CompilerGenerated]
  public bool \u003CisScanning\u003Ek__BackingField;
  public bool graphUpdateRoutineRunning;
  public bool isRegisteredForUpdate;
  public bool workItemsQueued;
  public bool queuedWorkItemFloodFill;
  public static System.Action OnAwakeSettings;
  public static OnGraphDelegate OnGraphPreScan;
  public static OnGraphDelegate OnGraphPostScan;
  public static OnPathDelegate OnPathPreSearch;
  public static OnPathDelegate OnPathPostSearch;
  public static OnScanDelegate OnPreScan;
  public static OnScanDelegate OnPostScan;
  public static OnScanDelegate OnLatePostScan;
  public static OnScanDelegate OnGraphsUpdated;
  public static System.Action On65KOverflow;
  public static System.Action OnThreadSafeCallback;
  public System.Action OnDrawGizmosCallback;
  public System.Action OnUnloadGizmoMeshes;
  [Obsolete]
  public System.Action OnGraphsWillBeUpdated;
  [Obsolete]
  public System.Action OnGraphsWillBeUpdated2;
  public Queue<GraphUpdateObject> graphUpdateQueue;
  public Stack<GraphNode> floodStack;
  public ThreadControlQueue pathQueue = new ThreadControlQueue(0);
  public static Thread[] threads;
  public Thread graphUpdateThread;
  public static PathThreadInfo[] threadInfos = new PathThreadInfo[0];
  public static IEnumerator threadEnumerator;
  public static LockFreeStack pathReturnStack = new LockFreeStack();
  public EuclideanEmbedding euclideanEmbedding = new EuclideanEmbedding();
  public int nextNodeIndex = 1;
  public Stack<int> nodeIndexPool = new Stack<int>();
  public Path pathReturnPop;
  public Queue<AstarPath.GUOSingle> graphUpdateQueueAsync = new Queue<AstarPath.GUOSingle>();
  public Queue<AstarPath.GUOSingle> graphUpdateQueueRegular = new Queue<AstarPath.GUOSingle>();
  public bool showGraphs;
  public static bool isEditor = true;
  public uint lastUniqueAreaIndex;
  public static object safeUpdateLock = new object();
  public AutoResetEvent graphUpdateAsyncEvent = new AutoResetEvent(false);
  public ManualResetEvent processingGraphUpdatesAsync = new ManualResetEvent(true);
  public float lastGraphUpdate = -9999f;
  public ushort nextFreePathID = 1;
  public Queue<AstarPath.AstarWorkItem> workItems = new Queue<AstarPath.AstarWorkItem>();
  public bool processingWorkItems;
  public static bool can_scan_on_startup = true;
  public static int waitForPathDepth = 0;
  public static bool quittingApplication = false;

  public static Version Version => new Version(3, 8, 8, 1);

  [Obsolete]
  public System.Type[] graphTypes => this.astarData.graphTypes;

  public NavGraph[] graphs
  {
    get
    {
      if (this.astarData == null)
        this.astarData = new AstarData();
      return this.astarData.graphs;
    }
    set
    {
      if (this.astarData == null)
        this.astarData = new AstarData();
      this.astarData.graphs = value;
    }
  }

  public float maxNearestNodeDistanceSqr
  {
    get => this.maxNearestNodeDistance * this.maxNearestNodeDistance;
  }

  [Obsolete("This field has been renamed to 'batchGraphUpdates'")]
  public bool limitGraphUpdates
  {
    get => this.batchGraphUpdates;
    set => this.batchGraphUpdates = value;
  }

  [Obsolete("This field has been renamed to 'graphUpdateBatchingInterval'")]
  public float maxGraphUpdateFreq
  {
    get => this.graphUpdateBatchingInterval;
    set => this.graphUpdateBatchingInterval = value;
  }

  public PathHandler debugPathData
  {
    get => this.debugPath == null ? (PathHandler) null : this.debugPath.pathHandler;
  }

  public bool isScanning
  {
    get => this.\u003CisScanning\u003Ek__BackingField;
    set => this.\u003CisScanning\u003Ek__BackingField = value;
  }

  public static int NumParallelThreads
  {
    get => AstarPath.threadInfos == null ? 0 : AstarPath.threadInfos.Length;
  }

  public static bool IsUsingMultithreading
  {
    get
    {
      if (AstarPath.threads != null && AstarPath.threads.Length != 0)
        return true;
      if (AstarPath.threads != null && AstarPath.threads.Length == 0 && AstarPath.threadEnumerator != null || !Application.isPlaying)
        return false;
      throw new Exception($"Not 'using threading' and not 'not using threading'... Are you sure pathfinding is set up correctly?\nIf scripts are reloaded in unity editor during play this could happen.\n{(AstarPath.threads != null ? AstarPath.threads.Length.ToString() ?? "" : "NULL")} {(AstarPath.threadEnumerator != null).ToString()}");
    }
  }

  public bool IsAnyGraphUpdatesQueued
  {
    get => this.graphUpdateQueue != null && this.graphUpdateQueue.Count > 0;
  }

  public string[] GetTagNames()
  {
    if (this.tagNames == null || this.tagNames.Length != 32 /*0x20*/)
    {
      this.tagNames = new string[32 /*0x20*/];
      for (int index = 0; index < this.tagNames.Length; ++index)
        this.tagNames[index] = index.ToString() ?? "";
      this.tagNames[0] = "Basic Ground";
    }
    return this.tagNames;
  }

  public static string[] FindTagNames()
  {
    if ((UnityEngine.Object) AstarPath.active != (UnityEngine.Object) null)
      return AstarPath.active.GetTagNames();
    AstarPath objectOfType = UnityEngine.Object.FindObjectOfType(typeof (AstarPath)) as AstarPath;
    if ((UnityEngine.Object) objectOfType != (UnityEngine.Object) null)
    {
      AstarPath.active = objectOfType;
      return objectOfType.GetTagNames();
    }
    return new string[1]
    {
      "There is no AstarPath component in the scene"
    };
  }

  public ushort GetNextPathID()
  {
    if (this.nextFreePathID == (ushort) 0)
    {
      ++this.nextFreePathID;
      Debug.Log((object) "65K cleanup (this message is harmless, it just means you have searched a lot of paths)");
      if (AstarPath.On65KOverflow != null)
      {
        System.Action on65Koverflow = AstarPath.On65KOverflow;
        AstarPath.On65KOverflow = (System.Action) null;
        on65Koverflow();
      }
    }
    return this.nextFreePathID++;
  }

  public void OnDrawGizmos()
  {
    if (this.isScanning)
      return;
    if ((UnityEngine.Object) AstarPath.active == (UnityEngine.Object) null)
      AstarPath.active = this;
    else if ((UnityEngine.Object) AstarPath.active != (UnityEngine.Object) this)
      return;
    if (this.graphs == null || this.pathQueue != null && this.pathQueue.AllReceiversBlocked && this.workItems.Count > 0)
      return;
    if (this.showNavGraphs && !this.manualDebugFloorRoof)
    {
      this.debugFloor = float.PositiveInfinity;
      this.debugRoof = float.NegativeInfinity;
      for (int index = 0; index < this.graphs.Length; ++index)
      {
        if (this.graphs[index] != null && this.graphs[index].drawGizmos)
          this.graphs[index].GetNodes((GraphNodeDelegateCancelable) (node =>
          {
            if (!AstarPath.active.showSearchTree || this.debugPathData == null || NavGraph.InSearchTree(node, this.debugPath))
            {
              PathNode pathNode = this.debugPathData != null ? this.debugPathData.GetPathNode(node) : (PathNode) null;
              if (pathNode != null || this.debugMode == GraphDebugMode.Penalty)
              {
                switch (this.debugMode)
                {
                  case GraphDebugMode.G:
                    this.debugFloor = Mathf.Min(this.debugFloor, (float) pathNode.G);
                    this.debugRoof = Mathf.Max(this.debugRoof, (float) pathNode.G);
                    break;
                  case GraphDebugMode.H:
                    this.debugFloor = Mathf.Min(this.debugFloor, (float) pathNode.H);
                    this.debugRoof = Mathf.Max(this.debugRoof, (float) pathNode.H);
                    break;
                  case GraphDebugMode.F:
                    this.debugFloor = Mathf.Min(this.debugFloor, (float) pathNode.F);
                    this.debugRoof = Mathf.Max(this.debugRoof, (float) pathNode.F);
                    break;
                  case GraphDebugMode.Penalty:
                    this.debugFloor = Mathf.Min(this.debugFloor, (float) node.Penalty);
                    this.debugRoof = Mathf.Max(this.debugRoof, (float) node.Penalty);
                    break;
                }
              }
            }
            return true;
          }));
      }
      if (float.IsInfinity(this.debugFloor))
      {
        this.debugFloor = 0.0f;
        this.debugRoof = 1f;
      }
      if ((double) this.debugRoof - (double) this.debugFloor < 1.0)
        ++this.debugRoof;
    }
    for (int index = 0; index < this.graphs.Length; ++index)
    {
      if (this.graphs[index] != null && this.graphs[index].drawGizmos)
        this.graphs[index].OnDrawGizmos(this.showNavGraphs);
    }
    if (this.showNavGraphs)
      this.euclideanEmbedding.OnDrawGizmos();
    if (this.showUnwalkableNodes && this.showNavGraphs)
    {
      Gizmos.color = AstarColor.UnwalkableNode;
      GraphNodeDelegateCancelable del = new GraphNodeDelegateCancelable(this.DrawUnwalkableNode);
      for (int index = 0; index < this.graphs.Length; ++index)
      {
        if (this.graphs[index] != null && this.graphs[index].drawGizmos)
          this.graphs[index].GetNodes(del);
      }
    }
    if (this.OnDrawGizmosCallback == null)
      return;
    this.OnDrawGizmosCallback();
  }

  public bool DrawUnwalkableNode(GraphNode node)
  {
    if (!node.Walkable)
      Gizmos.DrawCube((Vector3) node.position, Vector3.one * this.unwalkableNodeDebugSize);
    return true;
  }

  public void OnGUI()
  {
    if (this.logPathResults != PathLog.InGame || !(this.inGameDebugPath != ""))
      return;
    GUI.Label(new Rect(5f, 5f, 400f, 600f), this.inGameDebugPath);
  }

  public static void AstarLog(string s)
  {
    if (AstarPath.active == null)
    {
      Debug.Log((object) ("No AstarPath object was found : " + s));
    }
    else
    {
      if (AstarPath.active.logPathResults == PathLog.None || AstarPath.active.logPathResults == PathLog.OnlyErrors)
        return;
      Debug.Log((object) s);
    }
  }

  public static void AstarLogError(string s)
  {
    if ((UnityEngine.Object) AstarPath.active == (UnityEngine.Object) null)
    {
      Debug.Log((object) ("No AstarPath object was found : " + s));
    }
    else
    {
      if (AstarPath.active.logPathResults == PathLog.None)
        return;
      Debug.LogError((object) s);
    }
  }

  public void LogPathResults(Path p)
  {
    if (this.logPathResults == PathLog.None || this.logPathResults == PathLog.OnlyErrors && !p.error)
      return;
    string message = p.DebugString(this.logPathResults);
    if (this.logPathResults == PathLog.InGame)
      this.inGameDebugPath = message;
    else
      Debug.Log((object) message);
  }

  public void Update()
  {
    if (!Application.isPlaying)
      return;
    this.PerformBlockingActions();
    if (AstarPath.threadEnumerator != null)
    {
      try
      {
        AstarPath.threadEnumerator.MoveNext();
      }
      catch (Exception ex)
      {
        AstarPath.threadEnumerator = (IEnumerator) null;
        if (!(ex is ThreadControlQueue.QueueTerminationException))
        {
          Debug.LogException(ex);
          Debug.LogError((object) "Unhandled exception during pathfinding. Terminating.");
          this.pathQueue.TerminateReceivers();
          try
          {
            this.pathQueue.PopNoBlock(false);
          }
          catch
          {
          }
        }
      }
    }
    this.ReturnPaths(true);
  }

  public void PerformBlockingActions(bool force = false, bool unblockOnComplete = true)
  {
    if (!this.pathQueue.AllReceiversBlocked)
      return;
    this.ReturnPaths(false);
    if (AstarPath.OnThreadSafeCallback != null)
    {
      System.Action threadSafeCallback = AstarPath.OnThreadSafeCallback;
      AstarPath.OnThreadSafeCallback = (System.Action) null;
      threadSafeCallback();
    }
    if (this.ProcessWorkItems(force) != 2)
      return;
    this.workItemsQueued = false;
    if (!unblockOnComplete)
      return;
    if (this.euclideanEmbedding.dirty)
      this.euclideanEmbedding.RecalculateCosts();
    this.pathQueue.Unblock();
  }

  public void QueueWorkItemFloodFill()
  {
    if (!this.pathQueue.AllReceiversBlocked)
      throw new Exception("You are calling QueueWorkItemFloodFill from outside a WorkItem. This might cause unexpected behaviour.");
    this.queuedWorkItemFloodFill = true;
  }

  public void EnsureValidFloodFill()
  {
    if (!this.queuedWorkItemFloodFill)
      return;
    this.FloodFill();
  }

  public void AddWorkItem(AstarPath.AstarWorkItem itm)
  {
    this.workItems.Enqueue(itm);
    if (this.workItemsQueued)
      return;
    this.workItemsQueued = true;
    if (this.isScanning)
      return;
    this.InterruptPathfinding();
  }

  public int ProcessWorkItems(bool force)
  {
    if (!this.pathQueue.AllReceiversBlocked)
      return 0;
    this.processingWorkItems = !this.processingWorkItems ? true : throw new Exception("Processing work items recursively. Please do not wait for other work items to be completed inside work items. If you think this is not caused by any of your scripts, this might be a bug.");
    while (this.workItems.Count > 0)
    {
      AstarPath.AstarWorkItem astarWorkItem = this.workItems.Peek();
      if (astarWorkItem.init != null)
      {
        astarWorkItem.init();
        astarWorkItem.init = (System.Action) null;
      }
      bool flag;
      try
      {
        flag = astarWorkItem.update == null || astarWorkItem.update(force);
      }
      catch
      {
        this.workItems.Dequeue();
        this.processingWorkItems = false;
        throw;
      }
      if (!flag)
      {
        if (force)
          Debug.LogError((object) "Misbehaving WorkItem. 'force'=true but the work item did not complete.\nIf force=true is passed to a WorkItem it should always return true.");
        this.processingWorkItems = false;
        return 1;
      }
      this.workItems.Dequeue();
    }
    this.EnsureValidFloodFill();
    this.processingWorkItems = false;
    return 2;
  }

  public void QueueGraphUpdates()
  {
    if (this.isRegisteredForUpdate)
      return;
    this.isRegisteredForUpdate = true;
    this.AddWorkItem(new AstarPath.AstarWorkItem()
    {
      init = new System.Action(this.QueueGraphUpdatesInternal),
      update = new Func<bool, bool>(this.ProcessGraphUpdates)
    });
    this.lastGraphUpdate = Time.realtimeSinceStartup;
  }

  public IEnumerator DelayedGraphUpdate()
  {
    this.graphUpdateRoutineRunning = true;
    yield return (object) new WaitForSeconds(this.graphUpdateBatchingInterval - (Time.realtimeSinceStartup - this.lastGraphUpdate));
    this.QueueGraphUpdates();
    this.graphUpdateRoutineRunning = false;
  }

  public void UpdateGraphs(Bounds bounds, float t)
  {
    this.UpdateGraphs(new GraphUpdateObject(bounds), t);
  }

  public void UpdateGraphs(GraphUpdateObject ob, float t)
  {
    this.StartCoroutine(this.UpdateGraphsInteral(ob, t));
  }

  public IEnumerator UpdateGraphsInteral(GraphUpdateObject ob, float t)
  {
    yield return (object) new WaitForSeconds(t);
    this.UpdateGraphs(ob);
  }

  public void UpdateGraphs(Bounds bounds) => this.UpdateGraphs(new GraphUpdateObject(bounds));

  public void UpdateGraphs(GraphUpdateObject ob)
  {
    if (this.graphUpdateQueue == null)
      this.graphUpdateQueue = new Queue<GraphUpdateObject>();
    this.graphUpdateQueue.Enqueue(ob);
    if (this.batchGraphUpdates && (double) Time.realtimeSinceStartup - (double) this.lastGraphUpdate < (double) this.graphUpdateBatchingInterval)
    {
      if (this.graphUpdateRoutineRunning)
        return;
      this.StartCoroutine(this.DelayedGraphUpdate());
    }
    else
      this.QueueGraphUpdates();
  }

  public void FlushGraphUpdates()
  {
    if (!this.IsAnyGraphUpdatesQueued)
      return;
    this.QueueGraphUpdates();
    this.FlushWorkItems();
  }

  public void FlushWorkItems() => this.FlushWorkItemsInternal(true);

  [Obsolete("Use FlushWorkItems() instead or use FlushWorkItemsInternal if you really need to")]
  public void FlushWorkItems(bool unblockOnComplete, bool block)
  {
    this.BlockUntilPathQueueBlocked();
    this.PerformBlockingActions(block, unblockOnComplete);
  }

  public void FlushWorkItemsInternal(bool unblockOnComplete)
  {
    this.BlockUntilPathQueueBlocked();
    this.PerformBlockingActions(true, unblockOnComplete);
  }

  public void QueueGraphUpdatesInternal()
  {
    this.isRegisteredForUpdate = false;
    while (this.graphUpdateQueue.Count > 0)
    {
      GraphUpdateObject graphUpdateObject = this.graphUpdateQueue.Dequeue();
      foreach (IUpdatableGraph updateableGraph in this.astarData.GetUpdateableGraphs())
      {
        NavGraph graph = updateableGraph as NavGraph;
        if (graphUpdateObject.nnConstraint == null || graphUpdateObject.nnConstraint.SuitableGraph(AstarPath.active.astarData.GetGraphIndex(graph), graph))
          this.graphUpdateQueueRegular.Enqueue(new AstarPath.GUOSingle()
          {
            order = AstarPath.GraphUpdateOrder.GraphUpdate,
            obj = graphUpdateObject,
            graph = updateableGraph
          });
      }
      if (graphUpdateObject.requiresFloodFill)
        this.graphUpdateQueueRegular.Enqueue(new AstarPath.GUOSingle()
        {
          order = AstarPath.GraphUpdateOrder.FloodFill,
          obj = graphUpdateObject
        });
    }
    this.debugPath = (Path) null;
    GraphModifier.TriggerEvent(GraphModifier.EventType.PreUpdate);
  }

  public bool ProcessGraphUpdates(bool force)
  {
    if (force)
      this.processingGraphUpdatesAsync.WaitOne();
    else if (!this.processingGraphUpdatesAsync.WaitOne(0))
      return false;
    if (this.graphUpdateQueueAsync.Count != 0)
      throw new Exception("Queue should be empty at this stage");
    while (this.graphUpdateQueueRegular.Count > 0)
    {
      AstarPath.GUOSingle guoSingle = this.graphUpdateQueueRegular.Peek();
      GraphUpdateThreading graphUpdateThreading = guoSingle.order == AstarPath.GraphUpdateOrder.FloodFill ? GraphUpdateThreading.SeparateThread : guoSingle.graph.CanUpdateAsync(guoSingle.obj);
      bool flag = force;
      if (!Application.isPlaying || this.graphUpdateThread == null || !this.graphUpdateThread.IsAlive)
        flag = true;
      if (!flag && graphUpdateThreading == GraphUpdateThreading.SeparateAndUnityInit)
      {
        if (this.graphUpdateQueueAsync.Count > 0)
        {
          this.processingGraphUpdatesAsync.Reset();
          this.graphUpdateAsyncEvent.Set();
          return false;
        }
        guoSingle.graph.UpdateAreaInit(guoSingle.obj);
        this.graphUpdateQueueRegular.Dequeue();
        this.graphUpdateQueueAsync.Enqueue(guoSingle);
        this.processingGraphUpdatesAsync.Reset();
        this.graphUpdateAsyncEvent.Set();
        return false;
      }
      if (!flag && graphUpdateThreading == GraphUpdateThreading.SeparateThread)
      {
        this.graphUpdateQueueRegular.Dequeue();
        this.graphUpdateQueueAsync.Enqueue(guoSingle);
      }
      else
      {
        if (this.graphUpdateQueueAsync.Count > 0)
        {
          if (force)
            throw new Exception("This should not happen");
          this.processingGraphUpdatesAsync.Reset();
          this.graphUpdateAsyncEvent.Set();
          return false;
        }
        this.graphUpdateQueueRegular.Dequeue();
        if (guoSingle.order == AstarPath.GraphUpdateOrder.FloodFill)
        {
          this.FloodFill(guoSingle.obj.only_specific_graph);
        }
        else
        {
          if (graphUpdateThreading == GraphUpdateThreading.SeparateAndUnityInit)
          {
            try
            {
              guoSingle.graph.UpdateAreaInit(guoSingle.obj);
            }
            catch (Exception ex)
            {
              Debug.LogError((object) ("Error while initializing GraphUpdates\n" + ex?.ToString()));
            }
          }
          try
          {
            guoSingle.graph.UpdateArea(guoSingle.obj);
          }
          catch (Exception ex)
          {
            Debug.LogError((object) ("Error while updating graphs\n" + ex?.ToString()));
            throw;
          }
        }
      }
    }
    if (this.graphUpdateQueueAsync.Count > 0)
    {
      this.processingGraphUpdatesAsync.Reset();
      this.graphUpdateAsyncEvent.Set();
      return false;
    }
    GraphModifier.TriggerEvent(GraphModifier.EventType.PostUpdate);
    if (AstarPath.OnGraphsUpdated != null)
      AstarPath.OnGraphsUpdated(this);
    return true;
  }

  public void ProcessGraphUpdatesAsync(object _astar)
  {
    if (!(_astar is AstarPath astarPath))
    {
      Debug.LogError((object) "ProcessGraphUpdatesAsync started with invalid parameter _astar (was no AstarPath object)");
    }
    else
    {
      while (!astarPath.pathQueue.IsTerminating)
      {
        this.graphUpdateAsyncEvent.WaitOne();
        if (astarPath.pathQueue.IsTerminating)
        {
          this.graphUpdateQueueAsync.Clear();
          this.processingGraphUpdatesAsync.Set();
          break;
        }
        while (this.graphUpdateQueueAsync.Count > 0)
        {
          AstarPath.GUOSingle guoSingle = this.graphUpdateQueueAsync.Dequeue();
          try
          {
            if (guoSingle.order == AstarPath.GraphUpdateOrder.GraphUpdate)
            {
              guoSingle.graph.UpdateArea(guoSingle.obj);
            }
            else
            {
              if (guoSingle.order != AstarPath.GraphUpdateOrder.FloodFill)
                throw new NotSupportedException(guoSingle.order.ToString() ?? "");
              astarPath.FloodFill(guoSingle.obj.only_specific_graph);
            }
          }
          catch (Exception ex)
          {
            Debug.LogError((object) ("Exception while updating graphs:\n" + ex?.ToString()));
          }
        }
        this.processingGraphUpdatesAsync.Set();
      }
    }
  }

  public void FlushThreadSafeCallbacks()
  {
    if (AstarPath.OnThreadSafeCallback == null)
      return;
    this.BlockUntilPathQueueBlocked();
    this.PerformBlockingActions();
  }

  public static int CalculateThreadCount(ThreadCount count)
  {
    if (count != ThreadCount.AutomaticLowLoad && count != ThreadCount.AutomaticHighLoad)
      return (int) count;
    int val1_1 = Mathf.Max(1, SystemInfo.processorCount);
    int num = SystemInfo.systemMemorySize;
    if (num <= 0)
    {
      Debug.LogError((object) "Machine reporting that is has <= 0 bytes of RAM. This is definitely not true, assuming 1 GiB");
      num = 1024 /*0x0400*/;
    }
    if (val1_1 <= 1 || num <= 512 /*0x0200*/)
      return 0;
    if (count == ThreadCount.AutomaticHighLoad)
    {
      if (num <= 1024 /*0x0400*/)
        val1_1 = Math.Min(val1_1, 2);
    }
    else
    {
      int val1_2 = Mathf.Max(1, val1_1 / 2);
      if (num <= 1024 /*0x0400*/)
        val1_2 = Math.Min(val1_2, 2);
      val1_1 = Math.Min(val1_2, 6);
    }
    return val1_1;
  }

  public void Awake()
  {
    AstarPath.active = this;
    this.useGUILayout = false;
    AstarPath.isEditor = Application.isEditor;
    if (!Application.isPlaying)
      return;
    if (AstarPath.OnAwakeSettings != null)
      AstarPath.OnAwakeSettings();
    GraphModifier.FindAllModifiers();
    RelevantGraphSurface.FindAllGraphSurfaces();
    int threadCount = AstarPath.CalculateThreadCount(this.threadCount);
    AstarPath.threads = new Thread[threadCount];
    AstarPath.threadInfos = new PathThreadInfo[Math.Max(threadCount, 1)];
    this.pathQueue = new ThreadControlQueue(AstarPath.threadInfos.Length);
    for (int index = 0; index < AstarPath.threadInfos.Length; ++index)
      AstarPath.threadInfos[index] = new PathThreadInfo(index, this, new PathHandler(index, AstarPath.threadInfos.Length));
    AstarPath.threadEnumerator = threadCount != 0 ? (IEnumerator) null : AstarPath.CalculatePaths((object) AstarPath.threadInfos[0]);
    for (int index = 0; index < AstarPath.threads.Length; ++index)
    {
      AstarPath.threads[index] = new Thread(new ParameterizedThreadStart(AstarPath.CalculatePathsThreaded));
      AstarPath.threads[index].Name = "Pathfinding Thread " + index.ToString();
      AstarPath.threads[index].IsBackground = true;
    }
    for (int index = 0; index < AstarPath.threads.Length; ++index)
    {
      if (this.logPathResults == PathLog.Heavy)
        Debug.Log((object) ("Starting pathfinding thread " + index.ToString()));
      AstarPath.threads[index].Start((object) AstarPath.threadInfos[index]);
    }
    if (threadCount != 0)
    {
      this.graphUpdateThread = new Thread(new ParameterizedThreadStart(this.ProcessGraphUpdatesAsync));
      this.graphUpdateThread.IsBackground = true;
      this.graphUpdateThread.Priority = System.Threading.ThreadPriority.Lowest;
      this.graphUpdateThread.Start((object) this);
    }
    this.Initialize();
    this.FlushWorkItems();
    this.euclideanEmbedding.dirty = true;
    if (!this.scanOnStartup || !AstarPath.can_scan_on_startup || this.astarData.cacheStartup && !((UnityEngine.Object) this.astarData.file_cachedStartup == (UnityEngine.Object) null))
      return;
    this.Scan();
    Debug.Log((object) "A* scan on awake");
  }

  public void VerifyIntegrity()
  {
    if ((UnityEngine.Object) AstarPath.active != (UnityEngine.Object) this)
      throw new Exception("Singleton pattern broken. Make sure you only have one AstarPath object in the scene");
    if (this.astarData == null)
      throw new NullReferenceException("AstarData is null... Astar not set up correctly?");
    if (this.astarData.graphs == null)
      this.astarData.graphs = new NavGraph[0];
    if (this.pathQueue == null && !Application.isPlaying)
      this.pathQueue = new ThreadControlQueue(0);
    if (AstarPath.threadInfos == null && !Application.isPlaying)
      AstarPath.threadInfos = new PathThreadInfo[0];
    int num = AstarPath.IsUsingMultithreading ? 1 : 0;
  }

  public void SetUpReferences()
  {
    AstarPath.active = this;
    if (this.astarData == null)
      this.astarData = new AstarData();
    if (this.colorSettings == null)
      this.colorSettings = new AstarColor();
    this.colorSettings.OnEnable();
  }

  public void Initialize()
  {
    this.SetUpReferences();
    this.astarData.FindGraphTypes();
    this.astarData.Awake();
    this.astarData.UpdateShortcuts();
    for (int index = 0; index < this.astarData.graphs.Length; ++index)
    {
      if (this.astarData.graphs[index] != null)
        this.astarData.graphs[index].Awake();
    }
  }

  public void OnDisable()
  {
    if (this.OnUnloadGizmoMeshes == null)
      return;
    this.OnUnloadGizmoMeshes();
  }

  public void OnDestroy()
  {
    if (!Application.isPlaying)
      return;
    if (this.logPathResults == PathLog.Heavy)
      Debug.Log((object) "+++ AstarPath Component Destroyed - Cleaning Up Pathfinding Data +++");
    if ((UnityEngine.Object) AstarPath.active != (UnityEngine.Object) this)
      return;
    this.BlockUntilPathQueueBlocked();
    this.euclideanEmbedding.dirty = false;
    this.FlushWorkItemsInternal(false);
    this.pathQueue.TerminateReceivers();
    if (this.logPathResults == PathLog.Heavy)
      Debug.Log((object) "Processing Eventual Work Items");
    this.graphUpdateAsyncEvent.Set();
    if (AstarPath.threads != null)
    {
      for (int index = 0; index < AstarPath.threads.Length; ++index)
      {
        if (!AstarPath.threads[index].Join(50))
        {
          Debug.LogError((object) $"Could not terminate pathfinding thread[{index.ToString()}] in 50ms, trying Thread.Abort");
          AstarPath.threads[index].Abort();
        }
      }
    }
    if (this.logPathResults == PathLog.Heavy)
      Debug.Log((object) "Returning Paths");
    this.ReturnPaths(false);
    AstarPath.pathReturnStack.PopAll();
    if (this.logPathResults == PathLog.Heavy)
      Debug.Log((object) "Destroying Graphs");
    this.astarData.OnDestroy();
    if (this.logPathResults == PathLog.Heavy)
      Debug.Log((object) "Cleaning up variables");
    this.floodStack = (Stack<GraphNode>) null;
    this.graphUpdateQueue = (Queue<GraphUpdateObject>) null;
    this.OnDrawGizmosCallback = (System.Action) null;
    AstarPath.OnAwakeSettings = (System.Action) null;
    AstarPath.OnGraphPreScan = (OnGraphDelegate) null;
    AstarPath.OnGraphPostScan = (OnGraphDelegate) null;
    AstarPath.OnPathPreSearch = (OnPathDelegate) null;
    AstarPath.OnPathPostSearch = (OnPathDelegate) null;
    AstarPath.OnPreScan = (OnScanDelegate) null;
    AstarPath.OnPostScan = (OnScanDelegate) null;
    AstarPath.OnLatePostScan = (OnScanDelegate) null;
    AstarPath.On65KOverflow = (System.Action) null;
    AstarPath.OnGraphsUpdated = (OnScanDelegate) null;
    AstarPath.OnThreadSafeCallback = (System.Action) null;
    AstarPath.threads = (Thread[]) null;
    AstarPath.threadInfos = (PathThreadInfo[]) null;
    AstarPath.active = (AstarPath) null;
  }

  public void FloodFill(GraphNode seed)
  {
    this.FloodFill(seed, this.lastUniqueAreaIndex + 1U);
    ++this.lastUniqueAreaIndex;
  }

  public void FloodFill(GraphNode seed, uint area)
  {
    if (area > 131071U /*0x01FFFF*/)
      Debug.LogError((object) ("Too high area index - The maximum area index is " + 131071U /*0x01FFFF*/.ToString()));
    else if (area < 0U)
    {
      Debug.LogError((object) "Too low area index - The minimum area index is 0");
    }
    else
    {
      if (this.floodStack == null)
        this.floodStack = new Stack<GraphNode>(1024 /*0x0400*/);
      Stack<GraphNode> floodStack = this.floodStack;
      floodStack.Clear();
      floodStack.Push(seed);
      seed.Area = area;
      while (floodStack.Count > 0)
        floodStack.Pop().FloodFill(floodStack, area);
    }
  }

  [ContextMenu("Flood Fill Graphs")]
  public void FloodFill(int only_specific_graph = -1)
  {
    this.queuedWorkItemFloodFill = false;
    if (this.astarData.graphs == null)
      return;
    uint area = 0;
    this.lastUniqueAreaIndex = 0U;
    if (this.floodStack == null)
      this.floodStack = new Stack<GraphNode>(1024 /*0x0400*/);
    Stack<GraphNode> stack = this.floodStack;
    for (int index = 0; index < this.graphs.Length; ++index)
    {
      if (only_specific_graph == -1 || index == only_specific_graph)
        this.graphs[index]?.GetNodes((GraphNodeDelegateCancelable) (node =>
        {
          node.Area = 0U;
          return true;
        }));
    }
    int smallAreasDetected = 0;
    bool warnAboutAreas = false;
    List<GraphNode> smallAreaList = ListPool<GraphNode>.Claim();
    for (int index = 0; index < this.graphs.Length; ++index)
    {
      if (only_specific_graph == -1 || index == only_specific_graph)
      {
        NavGraph graph = this.graphs[index];
        if (graph != null)
        {
          GraphNodeDelegateCancelable del = (GraphNodeDelegateCancelable) (node =>
          {
            if (node != null && node.Walkable && node.Area == 0U)
            {
              ++area;
              uint region = area;
              if (area > 131071U /*0x01FFFF*/)
              {
                if (smallAreaList.Count > 0)
                {
                  GraphNode graphNode = smallAreaList[smallAreaList.Count - 1];
                  region = graphNode.Area;
                  smallAreaList.RemoveAt(smallAreaList.Count - 1);
                  stack.Clear();
                  stack.Push(graphNode);
                  graphNode.Area = 131071U /*0x01FFFF*/;
                  while (stack.Count > 0)
                    stack.Pop().FloodFill(stack, 131071U /*0x01FFFF*/);
                  ++smallAreasDetected;
                }
                else
                {
                  --area;
                  region = area;
                  warnAboutAreas = true;
                }
              }
              stack.Clear();
              stack.Push(node);
              int num = 1;
              node.Area = region;
              while (stack.Count > 0)
              {
                ++num;
                stack.Pop().FloodFill(stack, region);
              }
              if (num < this.minAreaSize)
                smallAreaList.Add(node);
            }
            return true;
          });
          graph.GetNodes(del);
        }
      }
    }
    this.lastUniqueAreaIndex = area;
    if (warnAboutAreas)
      Debug.LogError((object) $"Too many areas - The maximum number of areas is {131071U /*0x01FFFF*/.ToString()}. Try raising the A* Inspector -> Settings -> Min Area Size value. Enable the optimization ASTAR_MORE_AREAS under the Optimizations tab.");
    if (smallAreasDetected > 0)
      AstarPath.AstarLog($"{smallAreasDetected.ToString()} small areas were detected (fewer than {this.minAreaSize.ToString()} nodes),these might have the same IDs as other areas, but it shouldn't affect pathfinding in any significant way (you might get All Nodes Searched as a reason for path failure).\nWhich areas are defined as 'small' is controlled by the 'Min Area Size' variable, it can be changed in the A* inspector-->Settings-->Min Area Size\nThe small areas will use the area id {131071U /*0x01FFFF*/.ToString()}");
    ListPool<GraphNode>.Release(smallAreaList);
  }

  public int GetNewNodeIndex()
  {
    return this.nodeIndexPool.Count > 0 ? this.nodeIndexPool.Pop() : this.nextNodeIndex++;
  }

  public void InitializeNode(GraphNode node)
  {
    if (!this.pathQueue.AllReceiversBlocked)
      throw new Exception("Trying to initialize a node when it is not safe to initialize any nodes. Must be done during a graph update");
    if (AstarPath.threadInfos == null)
      AstarPath.threadInfos = new PathThreadInfo[0];
    for (int index = 0; index < AstarPath.threadInfos.Length; ++index)
      AstarPath.threadInfos[index].runData.InitializeNode(node);
  }

  public void DestroyNode(GraphNode node)
  {
    if (node.NodeIndex == -1)
      return;
    this.nodeIndexPool.Push(node.NodeIndex);
    if (AstarPath.threadInfos == null)
      AstarPath.threadInfos = new PathThreadInfo[0];
    for (int index = 0; index < AstarPath.threadInfos.Length; ++index)
      AstarPath.threadInfos[index].runData.DestroyNode(node);
  }

  public void BlockUntilPathQueueBlocked()
  {
    if (this.pathQueue == null)
      return;
    this.pathQueue.Block();
    while (!this.pathQueue.AllReceiversBlocked)
    {
      if (AstarPath.IsUsingMultithreading)
        Thread.Sleep(1);
      else
        AstarPath.threadEnumerator.MoveNext();
    }
  }

  public void Scan(int only_specific_graph = -1)
  {
    Debug.Log((object) ("A* scan, graph = " + only_specific_graph.ToString()));
    this.ScanLoop((OnScanStatus) null, only_specific_graph);
  }

  public void ScanLoop(OnScanStatus statusCallback, int only_specific_graph = -1)
  {
    if (this.graphs == null)
      return;
    this.isScanning = true;
    this.euclideanEmbedding.dirty = false;
    this.VerifyIntegrity();
    this.BlockUntilPathQueueBlocked();
    this.ReturnPaths(false);
    this.BlockUntilPathQueueBlocked();
    if (!Application.isPlaying)
    {
      GraphModifier.FindAllModifiers();
      RelevantGraphSurface.FindAllGraphSurfaces();
    }
    RelevantGraphSurface.UpdateAllPositions();
    this.astarData.UpdateShortcuts();
    if (statusCallback != null)
      statusCallback(new Progress(0.05f, "Pre processing graphs"));
    if (AstarPath.OnPreScan != null)
      AstarPath.OnPreScan(this);
    GraphModifier.TriggerEvent(GraphModifier.EventType.PreScan);
    DateTime utcNow = DateTime.UtcNow;
    for (int index = 0; index < this.graphs.Length; ++index)
    {
      if ((only_specific_graph == -1 || index == only_specific_graph) && this.graphs[index] != null)
        this.graphs[index].GetNodes((GraphNodeDelegateCancelable) (node =>
        {
          node.Destroy();
          return true;
        }));
    }
    int num;
    for (int i = 0; i < this.graphs.Length; num = i++)
    {
      if (only_specific_graph == -1 || i == only_specific_graph)
      {
        NavGraph graph = this.graphs[i];
        if (graph == null)
        {
          if (statusCallback != null)
          {
            OnScanStatus onScanStatus = statusCallback;
            double p = (double) Mathf.Lerp(0.05f, 0.7f, ((float) i + 0.5f) / (float) (this.graphs.Length + 1));
            string[] strArray = new string[5]
            {
              "Skipping graph ",
              null,
              null,
              null,
              null
            };
            num = i + 1;
            strArray[1] = num.ToString();
            strArray[2] = " of ";
            num = this.graphs.Length;
            strArray[3] = num.ToString();
            strArray[4] = " because it is null";
            string d = string.Concat(strArray);
            Progress progress = new Progress((float) p, d);
            onScanStatus(progress);
          }
        }
        else
        {
          if (AstarPath.OnGraphPreScan != null)
          {
            if (statusCallback != null)
            {
              OnScanStatus onScanStatus = statusCallback;
              double p = (double) Mathf.Lerp(0.1f, 0.7f, (float) i / (float) this.graphs.Length);
              string[] strArray = new string[5]
              {
                "Scanning graph ",
                null,
                null,
                null,
                null
              };
              num = i + 1;
              strArray[1] = num.ToString();
              strArray[2] = " of ";
              num = this.graphs.Length;
              strArray[3] = num.ToString();
              strArray[4] = " - Pre processing";
              string d = string.Concat(strArray);
              Progress progress = new Progress((float) p, d);
              onScanStatus(progress);
            }
            AstarPath.OnGraphPreScan(graph);
          }
          float minp = Mathf.Lerp(0.1f, 0.7f, (float) i / (float) this.graphs.Length);
          float maxp = Mathf.Lerp(0.1f, 0.7f, ((float) i + 0.95f) / (float) this.graphs.Length);
          if (statusCallback != null)
          {
            OnScanStatus onScanStatus = statusCallback;
            double p = (double) minp;
            num = i + 1;
            string str1 = num.ToString();
            num = this.graphs.Length;
            string str2 = num.ToString();
            string d = $"Scanning graph {str1} of {str2}";
            Progress progress = new Progress((float) p, d);
            onScanStatus(progress);
          }
          OnScanStatus statusCallback1 = (OnScanStatus) null;
          if (statusCallback != null)
            statusCallback1 = (OnScanStatus) (p =>
            {
              p.progress = Mathf.Lerp(minp, maxp, p.progress);
              statusCallback(p);
            });
          graph.ScanInternal(statusCallback1);
          graph.GetNodes((GraphNodeDelegateCancelable) (node =>
          {
            node.GraphIndex = (uint) i;
            return true;
          }));
          if (AstarPath.OnGraphPostScan != null)
          {
            if (statusCallback != null)
            {
              OnScanStatus onScanStatus = statusCallback;
              double p = (double) Mathf.Lerp(0.1f, 0.7f, ((float) i + 0.95f) / (float) this.graphs.Length);
              string[] strArray = new string[5]
              {
                "Scanning graph ",
                null,
                null,
                null,
                null
              };
              num = i + 1;
              strArray[1] = num.ToString();
              strArray[2] = " of ";
              num = this.graphs.Length;
              strArray[3] = num.ToString();
              strArray[4] = " - Post processing";
              string d = string.Concat(strArray);
              Progress progress = new Progress((float) p, d);
              onScanStatus(progress);
            }
            AstarPath.OnGraphPostScan(graph);
          }
        }
      }
    }
    if (statusCallback != null)
      statusCallback(new Progress(0.8f, "Post processing graphs"));
    if (AstarPath.OnPostScan != null)
      AstarPath.OnPostScan(this);
    GraphModifier.TriggerEvent(GraphModifier.EventType.PostScan);
    try
    {
      this.FlushWorkItemsInternal(false);
    }
    catch (Exception ex)
    {
      Debug.LogException(ex);
    }
    this.isScanning = false;
    if (statusCallback != null)
      statusCallback(new Progress(0.9f, "Computing areas"));
    this.FloodFill(only_specific_graph);
    this.VerifyIntegrity();
    if (statusCallback != null)
      statusCallback(new Progress(0.95f, "Late post processing"));
    if (AstarPath.OnLatePostScan != null)
      AstarPath.OnLatePostScan(this);
    GraphModifier.TriggerEvent(GraphModifier.EventType.LatePostScan);
    this.euclideanEmbedding.dirty = true;
    this.euclideanEmbedding.RecalculatePivots();
    this.PerformBlockingActions(true);
    this.lastScanTime = (float) (DateTime.UtcNow - utcNow).TotalSeconds;
    if (only_specific_graph == -1)
      GC.Collect();
    AstarPath.AstarLog($"Scanning - Process took {(this.lastScanTime * 1000f).ToString("0")} ms to complete");
  }

  public static void WaitForPath(Path p)
  {
    if ((UnityEngine.Object) AstarPath.active == (UnityEngine.Object) null)
      throw new Exception("Pathfinding is not correctly initialized in this scene (yet?). AstarPath.active is null.\nDo not call this function in Awake");
    if (p == null)
      throw new ArgumentNullException("Path must not be null");
    if (AstarPath.active.pathQueue.IsTerminating)
      return;
    if (p.GetState() == PathState.Created)
      throw new Exception("The specified path has not been started yet.");
    ++AstarPath.waitForPathDepth;
    if (AstarPath.waitForPathDepth == 5)
      Debug.LogError((object) "You are calling the WaitForPath function recursively (maybe from a path callback). Please don't do this.");
    if (p.GetState() < PathState.ReturnQueue)
    {
      if (AstarPath.IsUsingMultithreading)
      {
        while (p.GetState() < PathState.ReturnQueue)
        {
          if (AstarPath.active.pathQueue.IsTerminating)
          {
            --AstarPath.waitForPathDepth;
            throw new Exception("Pathfinding Threads seems to have crashed.");
          }
          Thread.Sleep(1);
          AstarPath.active.PerformBlockingActions();
        }
      }
      else
      {
        while (p.GetState() < PathState.ReturnQueue)
        {
          if (AstarPath.active.pathQueue.IsEmpty && p.GetState() != PathState.Processing)
          {
            --AstarPath.waitForPathDepth;
            throw new Exception($"Critical error. Path Queue is empty but the path state is '{p.GetState().ToString()}'");
          }
          AstarPath.threadEnumerator.MoveNext();
          AstarPath.active.PerformBlockingActions();
        }
      }
    }
    AstarPath.active.ReturnPaths(false);
    --AstarPath.waitForPathDepth;
  }

  [Obsolete("The threadSafe parameter has been deprecated")]
  public static void RegisterSafeUpdate(System.Action callback, bool threadSafe)
  {
    AstarPath.RegisterSafeUpdate(callback);
  }

  public static void RegisterSafeUpdate(System.Action callback)
  {
    if (callback == null || !Application.isPlaying)
      return;
    if (AstarPath.active.pathQueue.AllReceiversBlocked)
    {
      AstarPath.active.pathQueue.Lock();
      try
      {
        if (AstarPath.active.pathQueue.AllReceiversBlocked)
        {
          callback();
          return;
        }
      }
      finally
      {
        AstarPath.active.pathQueue.Unlock();
      }
    }
    lock (AstarPath.safeUpdateLock)
      AstarPath.OnThreadSafeCallback += callback;
    AstarPath.active.pathQueue.Block();
  }

  public void InterruptPathfinding() => this.pathQueue.Block();

  public static void StartPath(Path p, bool pushToFront = false)
  {
    if (AstarPath.active == null)
    {
      Debug.LogError((object) "There is no AstarPath object in the scene");
    }
    else
    {
      if (p.GetState() != PathState.Created)
        throw new Exception($"The path has an invalid state. Expected {PathState.Created.ToString()} found {p.GetState().ToString()}\nMake sure you are not requesting the same path twice");
      if (AstarPath.active.pathQueue.IsTerminating)
      {
        p.Error();
        p.LogError("No new paths are accepted");
      }
      else if (AstarPath.active.graphs == null || AstarPath.active.graphs.Length == 0)
      {
        Debug.LogError((object) "There are no graphs in the scene");
        p.Error();
        p.LogError("There are no graphs in the scene");
        Debug.LogError((object) p.errorLog);
      }
      else
      {
        p.Claim((object) AstarPath.active);
        p.AdvanceState(PathState.PathQueue);
        if (pushToFront)
          AstarPath.active.pathQueue.PushFront(p);
        else
          AstarPath.active.pathQueue.Push(p);
      }
    }
  }

  public void OnApplicationQuit()
  {
    AstarPath.quittingApplication = true;
    if (this.logPathResults == PathLog.Heavy)
      Debug.Log((object) "+++ Application Quitting - Cleaning Up +++");
    this.OnDestroy();
    if (AstarPath.threads == null)
      return;
    for (int index = 0; index < AstarPath.threads.Length; ++index)
    {
      if (AstarPath.threads[index] != null && AstarPath.threads[index].IsAlive)
        AstarPath.threads[index].Abort();
    }
    AstarPath.quittingApplication = false;
  }

  public void ReturnPaths(bool timeSlice)
  {
    Path path1 = AstarPath.pathReturnStack.PopAll();
    if (this.pathReturnPop == null)
    {
      this.pathReturnPop = path1;
    }
    else
    {
      Path path2 = this.pathReturnPop;
      while (path2.next != null)
        path2 = path2.next;
      path2.next = path1;
    }
    DateTime utcNow;
    long num1;
    if (!timeSlice)
    {
      num1 = 0L;
    }
    else
    {
      utcNow = DateTime.UtcNow;
      num1 = utcNow.Ticks + 10000L;
    }
    long num2 = num1;
    int num3 = 0;
    while (this.pathReturnPop != null)
    {
      Path pathReturnPop = this.pathReturnPop;
      this.pathReturnPop = this.pathReturnPop.next;
      pathReturnPop.next = (Path) null;
      pathReturnPop.ReturnPath();
      pathReturnPop.AdvanceState(PathState.Returned);
      pathReturnPop.Release((object) this, true);
      ++num3;
      if (num3 > 5 & timeSlice)
      {
        num3 = 0;
        utcNow = DateTime.UtcNow;
        if (utcNow.Ticks >= num2)
          break;
      }
    }
  }

  public static void CalculatePathsThreaded(object _threadInfo)
  {
    PathThreadInfo pathThreadInfo;
    try
    {
      pathThreadInfo = (PathThreadInfo) _threadInfo;
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ("Arguments to pathfinding threads must be of type ThreadStartInfo\n" + ex?.ToString()));
      throw new ArgumentException("Argument must be of type ThreadStartInfo", ex);
    }
    AstarPath astar = pathThreadInfo.astar;
    try
    {
      PathHandler runData = pathThreadInfo.runData;
      if (runData.nodes == null)
        throw new NullReferenceException("NodeRuns must be assigned to the threadInfo.runData.nodes field before threads are started\nthreadInfo is an argument to the thread functions");
      long targetTick = DateTime.UtcNow.Ticks + (long) ((double) astar.maxFrameTime * 10000.0);
      while (true)
      {
        long num1;
        DateTime utcNow;
        do
        {
          Path p = astar.pathQueue.Pop();
          num1 = (long) ((double) astar.maxFrameTime * 10000.0);
          p.PrepareBase(runData);
          p.AdvanceState(PathState.Processing);
          if (AstarPath.OnPathPreSearch != null)
            AstarPath.OnPathPreSearch(p);
          utcNow = DateTime.UtcNow;
          long ticks = utcNow.Ticks;
          long num2 = 0;
          p.Prepare();
          if (!p.IsDone())
          {
            astar.debugPath = p;
            p.Initialize();
            while (!p.IsDone())
            {
              p.CalculateStep(targetTick);
              ++p.searchIterations;
              if (!p.IsDone())
              {
                long num3 = num2;
                utcNow = DateTime.UtcNow;
                long num4 = utcNow.Ticks - ticks;
                num2 = num3 + num4;
                Thread.Sleep(0);
                utcNow = DateTime.UtcNow;
                ticks = utcNow.Ticks;
                targetTick = ticks + num1;
                if (astar.pathQueue.IsTerminating)
                  p.Error();
              }
              else
                break;
            }
            long num5 = num2;
            utcNow = DateTime.UtcNow;
            long num6 = utcNow.Ticks - ticks;
            long num7 = num5 + num6;
            p.duration = (float) num7 * 0.0001f;
          }
          p.Cleanup();
          astar.LogPathResults(p);
          if (p.immediateCallback != null)
            p.immediateCallback(p);
          if (AstarPath.OnPathPostSearch != null)
            AstarPath.OnPathPostSearch(p);
          AstarPath.pathReturnStack.Push(p);
          p.AdvanceState(PathState.ReturnQueue);
          utcNow = DateTime.UtcNow;
        }
        while (utcNow.Ticks <= targetTick);
        Thread.Sleep(1);
        utcNow = DateTime.UtcNow;
        targetTick = utcNow.Ticks + num1;
      }
    }
    catch (Exception ex)
    {
      switch (ex)
      {
        case ThreadAbortException _:
        case ThreadControlQueue.QueueTerminationException _:
          if (astar.logPathResults != PathLog.Heavy)
            return;
          Debug.LogWarning((object) ("Shutting down pathfinding thread #" + pathThreadInfo.threadIndex.ToString()));
          return;
        default:
          Debug.LogException(ex);
          Debug.LogError((object) "Unhandled exception during pathfinding. Terminating.");
          astar.pathQueue.TerminateReceivers();
          break;
      }
    }
    Debug.LogError((object) "Error : This part should never be reached.");
    astar.pathQueue.ReceiverTerminated();
  }

  public static IEnumerator CalculatePaths(object _threadInfo)
  {
    PathThreadInfo pathThreadInfo;
    try
    {
      pathThreadInfo = (PathThreadInfo) _threadInfo;
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ("Arguments to pathfinding threads must be of type ThreadStartInfo\n" + ex?.ToString()));
      throw new ArgumentException("Argument must be of type ThreadStartInfo", ex);
    }
    int numPaths = 0;
    PathHandler runData = pathThreadInfo.runData;
    AstarPath astar = pathThreadInfo.astar;
    if (runData.nodes == null)
      throw new NullReferenceException("NodeRuns must be assigned to the threadInfo.runData.nodes field before threads are started\nthreadInfo is an argument to the thread functions");
    long maxTicks = (long) ((double) AstarPath.active.maxFrameTime * 10000.0);
    long targetTick = DateTime.UtcNow.Ticks + maxTicks;
    while (true)
    {
      Path p = (Path) null;
      bool blockedBefore = false;
      while (p == null)
      {
        try
        {
          p = astar.pathQueue.PopNoBlock(blockedBefore);
          if (p == null)
            blockedBefore = true;
        }
        catch (ThreadControlQueue.QueueTerminationException ex)
        {
          yield break;
        }
        if (p == null)
          yield return (object) null;
      }
      maxTicks = (long) ((double) AstarPath.active.maxFrameTime * 10000.0);
      p.PrepareBase(runData);
      p.AdvanceState(PathState.Processing);
      OnPathDelegate onPathPreSearch = AstarPath.OnPathPreSearch;
      if (onPathPreSearch != null)
        onPathPreSearch(p);
      ++numPaths;
      DateTime utcNow = DateTime.UtcNow;
      long ticks = utcNow.Ticks;
      long totalTicks = 0;
      p.Prepare();
      if (!p.IsDone())
      {
        AstarPath.active.debugPath = p;
        p.Initialize();
        while (!p.IsDone())
        {
          p.CalculateStep(targetTick);
          ++p.searchIterations;
          if (!p.IsDone())
          {
            long num1 = totalTicks;
            utcNow = DateTime.UtcNow;
            long num2 = utcNow.Ticks - ticks;
            totalTicks = num1 + num2;
            yield return (object) null;
            ticks = DateTime.UtcNow.Ticks;
            if (astar.pathQueue.IsTerminating)
              p.Error();
            targetTick = DateTime.UtcNow.Ticks + maxTicks;
          }
          else
            break;
        }
        long num3 = totalTicks;
        utcNow = DateTime.UtcNow;
        long num4 = utcNow.Ticks - ticks;
        totalTicks = num3 + num4;
        p.duration = (float) totalTicks * 0.0001f;
      }
      p.Cleanup();
      AstarPath.active.LogPathResults(p);
      OnPathDelegate immediateCallback = p.immediateCallback;
      if (immediateCallback != null)
        immediateCallback(p);
      OnPathDelegate onPathPostSearch = AstarPath.OnPathPostSearch;
      if (onPathPostSearch != null)
        onPathPostSearch(p);
      AstarPath.pathReturnStack.Push(p);
      p.AdvanceState(PathState.ReturnQueue);
      utcNow = DateTime.UtcNow;
      if (utcNow.Ticks > targetTick)
      {
        yield return (object) null;
        utcNow = DateTime.UtcNow;
        targetTick = utcNow.Ticks + maxTicks;
        numPaths = 0;
      }
      p = (Path) null;
    }
  }

  public NNInfo GetNearest(Vector3 position) => this.GetNearest(position, NNConstraint.None);

  public NNInfo GetNearest(Vector3 position, NNConstraint constraint)
  {
    return this.GetNearest(position, constraint, (GraphNode) null);
  }

  public NNInfo GetNearest(Vector3 position, NNConstraint constraint, GraphNode hint)
  {
    if (this.graphs == null)
      return new NNInfo();
    float num = float.PositiveInfinity;
    NNInfo nearest = new NNInfo();
    int index = -1;
    for (int graphIndex = 0; graphIndex < this.graphs.Length; ++graphIndex)
    {
      NavGraph graph = this.graphs[graphIndex];
      if (graph != null && constraint.SuitableGraph(graphIndex, graph))
      {
        NNInfo nnInfo = !this.fullGetNearestSearch ? graph.GetNearest(position, constraint) : graph.GetNearestForce(position, constraint);
        if (nnInfo.node != null)
        {
          float magnitude = (nnInfo.clampedPosition - position).magnitude;
          if (this.prioritizeGraphs && (double) magnitude < (double) this.prioritizeGraphsLimit)
          {
            nearest = nnInfo;
            index = graphIndex;
            break;
          }
          if ((double) magnitude < (double) num)
          {
            num = magnitude;
            nearest = nnInfo;
            index = graphIndex;
          }
        }
      }
    }
    if (index == -1)
      return nearest;
    if (nearest.constrainedNode != null)
    {
      nearest.node = nearest.constrainedNode;
      nearest.clampedPosition = nearest.constClampedPosition;
    }
    if (!this.fullGetNearestSearch && nearest.node != null && !constraint.Suitable(nearest.node))
    {
      NNInfo nearestForce = this.graphs[index].GetNearestForce(position, constraint);
      if (nearestForce.node != null)
        nearest = nearestForce;
    }
    return !constraint.Suitable(nearest.node) || constraint.constrainDistance && (double) (nearest.clampedPosition - position).sqrMagnitude > (double) this.maxNearestNodeDistanceSqr ? new NNInfo() : nearest;
  }

  public GraphNode GetNearest(Ray ray)
  {
    if (this.graphs == null)
      return (GraphNode) null;
    float minDist = float.PositiveInfinity;
    GraphNode nearestNode = (GraphNode) null;
    Vector3 lineDirection = ray.direction;
    Vector3 lineOrigin = ray.origin;
    for (int index = 0; index < this.graphs.Length; ++index)
      this.graphs[index].GetNodes((GraphNodeDelegateCancelable) (node =>
      {
        Vector3 position = (Vector3) node.position;
        Vector3 vector3 = lineOrigin + Vector3.Dot(position - lineOrigin, lineDirection) * lineDirection;
        double num1 = (double) Mathf.Abs(vector3.x - position.x);
        if (num1 * num1 > (double) minDist)
          return true;
        double num2 = (double) Mathf.Abs(vector3.z - position.z);
        if (num2 * num2 > (double) minDist)
          return true;
        float sqrMagnitude = (vector3 - position).sqrMagnitude;
        if ((double) sqrMagnitude < (double) minDist)
        {
          minDist = sqrMagnitude;
          nearestNode = node;
        }
        return true;
      }));
    return nearestNode;
  }

  [CompilerGenerated]
  public bool \u003COnDrawGizmos\u003Eb__104_0(GraphNode node)
  {
    if (!AstarPath.active.showSearchTree || this.debugPathData == null || NavGraph.InSearchTree(node, this.debugPath))
    {
      PathNode pathNode = this.debugPathData != null ? this.debugPathData.GetPathNode(node) : (PathNode) null;
      if (pathNode != null || this.debugMode == GraphDebugMode.Penalty)
      {
        switch (this.debugMode)
        {
          case GraphDebugMode.G:
            this.debugFloor = Mathf.Min(this.debugFloor, (float) pathNode.G);
            this.debugRoof = Mathf.Max(this.debugRoof, (float) pathNode.G);
            break;
          case GraphDebugMode.H:
            this.debugFloor = Mathf.Min(this.debugFloor, (float) pathNode.H);
            this.debugRoof = Mathf.Max(this.debugRoof, (float) pathNode.H);
            break;
          case GraphDebugMode.F:
            this.debugFloor = Mathf.Min(this.debugFloor, (float) pathNode.F);
            this.debugRoof = Mathf.Max(this.debugRoof, (float) pathNode.F);
            break;
          case GraphDebugMode.Penalty:
            this.debugFloor = Mathf.Min(this.debugFloor, (float) node.Penalty);
            this.debugRoof = Mathf.Max(this.debugRoof, (float) node.Penalty);
            break;
        }
      }
    }
    return true;
  }

  public enum AstarDistribution
  {
    WebsiteDownload,
    AssetStore,
  }

  public enum GraphUpdateOrder
  {
    GraphUpdate,
    FloodFill,
  }

  public struct GUOSingle
  {
    public AstarPath.GraphUpdateOrder order;
    public IUpdatableGraph graph;
    public GraphUpdateObject obj;
  }

  public struct AstarWorkItem
  {
    public System.Action init;
    public Func<bool, bool> update;

    public AstarWorkItem(Func<bool, bool> update)
    {
      this.init = (System.Action) null;
      this.update = update;
    }

    public AstarWorkItem(System.Action init, Func<bool, bool> update)
    {
      this.init = init;
      this.update = update;
    }
  }
}
