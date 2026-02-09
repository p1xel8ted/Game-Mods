// Decompiled with JetBrains decompiler
// Type: Pathfinding.Path
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

#nullable disable
namespace Pathfinding;

public abstract class Path
{
  [CompilerGenerated]
  public PathHandler \u003CpathHandler\u003Ek__BackingField;
  public OnPathDelegate callback;
  public OnPathDelegate immediateCallback;
  public PathState state;
  public object stateLock = new object();
  public PathCompleteState pathCompleteState;
  public string _errorLog = "";
  public List<GraphNode> path;
  public List<Vector3> vectorPath;
  public float maxFrameTime;
  public PathNode currentR;
  public float duration;
  public int searchIterations;
  public int searchedNodes;
  [CompilerGenerated]
  public DateTime \u003CcallTime\u003Ek__BackingField;
  public bool pooled;
  public bool hasBeenReset;
  public NNConstraint nnConstraint = (NNConstraint) PathNNConstraint.Default;
  public Path next;
  public Heuristic heuristic;
  public float heuristicScale = 1f;
  [CompilerGenerated]
  public ushort \u003CpathID\u003Ek__BackingField;
  public GraphNode hTargetNode;
  public Int3 hTarget;
  public int enabledTags = -1;
  public static int[] ZeroTagPenalties = new int[32 /*0x20*/];
  public int[] internalTagPenalties;
  public int[] manualTagPenalties;
  public List<object> claimed = new List<object>();
  public bool releasedNotSilent;

  public PathHandler pathHandler
  {
    get => this.\u003CpathHandler\u003Ek__BackingField;
    set => this.\u003CpathHandler\u003Ek__BackingField = value;
  }

  public PathCompleteState CompleteState
  {
    get => this.pathCompleteState;
    set => this.pathCompleteState = value;
  }

  public bool error => this.CompleteState == PathCompleteState.Error;

  public string errorLog => this._errorLog;

  public DateTime callTime
  {
    get => this.\u003CcallTime\u003Ek__BackingField;
    set => this.\u003CcallTime\u003Ek__BackingField = value;
  }

  [Obsolete("Has been renamed to 'pooled' to use more widely underestood terminology")]
  public bool recycled
  {
    get => this.pooled;
    set => this.pooled = value;
  }

  public ushort pathID
  {
    get => this.\u003CpathID\u003Ek__BackingField;
    set => this.\u003CpathID\u003Ek__BackingField = value;
  }

  public int[] tagPenalties
  {
    get => this.manualTagPenalties;
    set
    {
      if (value == null || value.Length != 32 /*0x20*/)
      {
        this.manualTagPenalties = (int[]) null;
        this.internalTagPenalties = Path.ZeroTagPenalties;
      }
      else
      {
        this.manualTagPenalties = value;
        this.internalTagPenalties = value;
      }
    }
  }

  public virtual bool FloodingPath => false;

  public float GetTotalLength()
  {
    if (this.vectorPath == null)
      return float.PositiveInfinity;
    float totalLength = 0.0f;
    for (int index = 0; index < this.vectorPath.Count - 1; ++index)
      totalLength += Vector3.Distance(this.vectorPath[index], this.vectorPath[index + 1]);
    return totalLength;
  }

  public IEnumerator WaitForPath()
  {
    if (this.GetState() == PathState.Created)
      throw new InvalidOperationException("This path has not been started yet");
    while (this.GetState() != PathState.Returned)
      yield return (object) null;
  }

  public uint CalculateHScore(GraphNode node)
  {
    switch (this.heuristic)
    {
      case Heuristic.Manhattan:
        Int3 position = node.position;
        uint val1_1 = (uint) ((double) (Math.Abs(this.hTarget.x - position.x) + Math.Abs(this.hTarget.y - position.y) + Math.Abs(this.hTarget.z - position.z)) * (double) this.heuristicScale);
        if (this.hTargetNode != null)
          val1_1 = Math.Max(val1_1, AstarPath.active.euclideanEmbedding.GetHeuristic(node.NodeIndex, this.hTargetNode.NodeIndex));
        return val1_1;
      case Heuristic.DiagonalManhattan:
        Int3 int3 = this.GetHTarget() - node.position;
        int3.x = Math.Abs(int3.x);
        int3.y = Math.Abs(int3.y);
        int3.z = Math.Abs(int3.z);
        int num1 = Math.Min(int3.x, int3.z);
        int num2 = Math.Max(int3.x, int3.z);
        uint val1_2 = (uint) ((double) (14 * num1 / 10 + (num2 - num1) + int3.y) * (double) this.heuristicScale);
        if (this.hTargetNode != null)
          val1_2 = Math.Max(val1_2, AstarPath.active.euclideanEmbedding.GetHeuristic(node.NodeIndex, this.hTargetNode.NodeIndex));
        return val1_2;
      case Heuristic.Euclidean:
        uint val1_3 = (uint) ((double) (this.GetHTarget() - node.position).costMagnitude * (double) this.heuristicScale);
        if (this.hTargetNode != null)
          val1_3 = Math.Max(val1_3, AstarPath.active.euclideanEmbedding.GetHeuristic(node.NodeIndex, this.hTargetNode.NodeIndex));
        return val1_3;
      default:
        return 0;
    }
  }

  public uint GetTagPenalty(int tag) => (uint) this.internalTagPenalties[tag];

  public Int3 GetHTarget() => this.hTarget;

  public bool CanTraverse(GraphNode node)
  {
    return node.Walkable && (this.enabledTags >> (int) node.Tag & 1) != 0;
  }

  public uint GetTraversalCost(GraphNode node) => this.GetTagPenalty((int) node.Tag) + node.Penalty;

  public virtual uint GetConnectionSpecialCost(GraphNode a, GraphNode b, uint currentCost)
  {
    return currentCost;
  }

  public bool IsDone() => this.CompleteState != 0;

  public void AdvanceState(PathState s)
  {
    lock (this.stateLock)
      this.state = (PathState) Math.Max((int) this.state, (int) s);
  }

  public PathState GetState() => this.state;

  public void LogError(string msg)
  {
    if (AstarPath.isEditor || AstarPath.active.logPathResults != PathLog.None)
      this._errorLog += msg;
    if (AstarPath.active.logPathResults == PathLog.None || AstarPath.active.logPathResults == PathLog.InGame)
      return;
    Debug.LogWarning((object) msg);
  }

  public void ForceLogError(string msg)
  {
    this.Error();
    this._errorLog += msg;
    Debug.LogError((object) msg);
  }

  public void Log(string msg)
  {
    if (!AstarPath.isEditor && AstarPath.active.logPathResults == PathLog.None)
      return;
    this._errorLog += msg;
  }

  public void Error() => this.CompleteState = PathCompleteState.Error;

  public void ErrorCheck()
  {
    if (!this.hasBeenReset)
      throw new Exception("The path has never been reset. Use pooling API or call Reset() after creating the path with the default constructor.");
    if (this.pooled)
      throw new Exception("The path is currently in a path pool. Are you sending the path for calculation twice?");
    if (this.pathHandler == null)
      throw new Exception("Field pathHandler is not set. Please report this bug.");
    if (this.GetState() > PathState.Processing)
      throw new Exception("This path has already been processed. Do not request a path with the same path object twice.");
  }

  public virtual void OnEnterPool()
  {
    if (this.vectorPath != null)
      ListPool<Vector3>.Release(this.vectorPath);
    if (this.path != null)
      ListPool<GraphNode>.Release(this.path);
    this.vectorPath = (List<Vector3>) null;
    this.path = (List<GraphNode>) null;
  }

  public virtual void Reset()
  {
    if (AstarPath.active == null)
      throw new NullReferenceException("No AstarPath object found in the scene. Make sure there is one or do not create paths in Awake");
    this.hasBeenReset = true;
    this.state = PathState.Created;
    this.releasedNotSilent = false;
    this.pathHandler = (PathHandler) null;
    this.callback = (OnPathDelegate) null;
    this._errorLog = "";
    this.pathCompleteState = PathCompleteState.NotCalculated;
    this.path = ListPool<GraphNode>.Claim();
    this.vectorPath = ListPool<Vector3>.Claim();
    this.currentR = (PathNode) null;
    this.duration = 0.0f;
    this.searchIterations = 0;
    this.searchedNodes = 0;
    this.nnConstraint = (NNConstraint) PathNNConstraint.Default;
    this.next = (Path) null;
    this.heuristic = AstarPath.active.heuristic;
    this.heuristicScale = AstarPath.active.heuristicScale;
    this.enabledTags = -1;
    this.tagPenalties = (int[]) null;
    this.callTime = DateTime.UtcNow;
    this.pathID = AstarPath.active.GetNextPathID();
    this.hTarget = Int3.zero;
    this.hTargetNode = (GraphNode) null;
  }

  public bool HasExceededTime(int searchedNodes, long targetTime)
  {
    return DateTime.UtcNow.Ticks >= targetTime;
  }

  public void Claim(object o)
  {
    if (o == null)
      throw new ArgumentNullException(nameof (o));
    for (int index = 0; index < this.claimed.Count; ++index)
    {
      if (this.claimed[index] == o)
        throw new ArgumentException($"You have already claimed the path with that object ({o?.ToString()}). Are you claiming the path with the same object twice?");
    }
    this.claimed.Add(o);
  }

  [Obsolete("Use Release(o, true) instead")]
  public void ReleaseSilent(object o) => this.Release(o, true);

  public void Release(object o, bool silent = false)
  {
    if (o == null)
      throw new ArgumentNullException(nameof (o));
    for (int index = 0; index < this.claimed.Count; ++index)
    {
      if (this.claimed[index] == o)
      {
        this.claimed.RemoveAt(index);
        if (!silent)
          this.releasedNotSilent = true;
        if (this.claimed.Count != 0 || !this.releasedNotSilent)
          return;
        PathPool.Pool(this);
        return;
      }
    }
    if (this.claimed.Count == 0)
      throw new ArgumentException($"You are releasing a path which is not claimed at all (most likely it has been pooled already). Are you releasing the path with the same object ({o?.ToString()}) twice?\nCheck out the documentation on path pooling for help.");
    throw new ArgumentException($"You are releasing a path which has not been claimed with this object ({o?.ToString()}). Are you releasing the path with the same object twice?\nCheck out the documentation on path pooling for help.");
  }

  public virtual void Trace(PathNode from)
  {
    PathNode pathNode1 = from;
    int num1 = 0;
    while (pathNode1 != null)
    {
      pathNode1 = pathNode1.parent;
      ++num1;
      if (num1 > 2048 /*0x0800*/)
      {
        Debug.LogWarning((object) "Infinite loop? >2048 node path. Remove this message if you really have that long paths (Path.cs, Trace method)");
        break;
      }
    }
    if (this.path.Capacity < num1)
      this.path.Capacity = num1;
    if (this.vectorPath.Capacity < num1)
      this.vectorPath.Capacity = num1;
    PathNode pathNode2 = from;
    for (int index = 0; index < num1; ++index)
    {
      this.path.Add(pathNode2.node);
      pathNode2 = pathNode2.parent;
    }
    int num2 = num1 / 2;
    for (int index = 0; index < num2; ++index)
    {
      GraphNode graphNode = this.path[index];
      this.path[index] = this.path[num1 - index - 1];
      this.path[num1 - index - 1] = graphNode;
    }
    for (int index = 0; index < num1; ++index)
      this.vectorPath.Add((Vector3) this.path[index].position);
  }

  public void DebugStringPrefix(PathLog logMode, StringBuilder text)
  {
    text.Append(this.error ? "Path Failed : " : "Path Completed : ");
    text.Append("Computation Time ");
    text.Append(this.duration.ToString(logMode == PathLog.Heavy ? "0.000 ms " : "0.00 ms "));
    text.Append("Searched Nodes ").Append(this.searchedNodes);
    if (this.error)
      return;
    text.Append(" Path Length ");
    text.Append(this.path == null ? "Null" : this.path.Count.ToString());
    if (logMode != PathLog.Heavy)
      return;
    text.Append("\nSearch Iterations ").Append(this.searchIterations);
  }

  public void DebugStringSuffix(PathLog logMode, StringBuilder text)
  {
    if (this.error)
      text.Append("\nError: ").Append(this.errorLog);
    if (logMode == PathLog.Heavy && !AstarPath.IsUsingMultithreading)
    {
      text.Append("\nCallback references ");
      if (this.callback != null)
        text.Append(this.callback.Target.GetType().FullName).AppendLine();
      else
        text.AppendLine("NULL");
    }
    text.Append("\nPath Number ").Append(this.pathID).Append(" (unique id)");
  }

  public virtual string DebugString(PathLog logMode)
  {
    if (logMode == PathLog.None || !this.error && logMode == PathLog.OnlyErrors)
      return "";
    StringBuilder debugStringBuilder = this.pathHandler.DebugStringBuilder;
    debugStringBuilder.Length = 0;
    this.DebugStringPrefix(logMode, debugStringBuilder);
    this.DebugStringSuffix(logMode, debugStringBuilder);
    return debugStringBuilder.ToString();
  }

  public virtual void ReturnPath()
  {
    if (this.callback == null)
      return;
    this.callback(this);
  }

  public void PrepareBase(PathHandler pathHandler)
  {
    if ((int) pathHandler.PathID > (int) this.pathID)
      pathHandler.ClearPathIDs();
    this.pathHandler = pathHandler;
    pathHandler.InitializeForPath(this);
    if (this.internalTagPenalties == null || this.internalTagPenalties.Length != 32 /*0x20*/)
      this.internalTagPenalties = Path.ZeroTagPenalties;
    try
    {
      this.ErrorCheck();
    }
    catch (Exception ex)
    {
      this.ForceLogError($"Exception in path {this.pathID.ToString()}\n{ex?.ToString()}");
    }
  }

  public abstract void Prepare();

  public virtual void Cleanup()
  {
  }

  public abstract void Initialize();

  public abstract void CalculateStep(long targetTick);
}
