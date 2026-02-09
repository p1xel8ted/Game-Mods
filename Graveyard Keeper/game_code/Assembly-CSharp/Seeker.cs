// Decompiled with JetBrains decompiler
// Type: Seeker
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

#nullable disable
[AddComponentMenu("Pathfinding/Seeker")]
[HelpURL("http://arongranberg.com/astar/docs/class_seeker.php")]
public class Seeker : MonoBehaviour, ISerializationCallbackReceiver
{
  public bool drawGizmos = true;
  public bool detailedGizmos;
  public StartEndModifier startEndModifier = new StartEndModifier();
  [HideInInspector]
  public int traversableTags = -1;
  [HideInInspector]
  [SerializeField]
  [FormerlySerializedAs("traversableTags")]
  public TagMask traversableTagsCompatibility = new TagMask(-1, -1);
  [HideInInspector]
  public int[] tagPenalties = new int[32 /*0x20*/];
  public OnPathDelegate pathCallback;
  public OnPathDelegate preProcessPath;
  public OnPathDelegate postProcessPath;
  [NonSerialized]
  public List<Vector3> lastCompletedVectorPath;
  [NonSerialized]
  public List<GraphNode> lastCompletedNodePath;
  [NonSerialized]
  public Path path;
  [NonSerialized]
  public Path prevPath;
  public OnPathDelegate onPathDelegate;
  public OnPathDelegate onPartialPathDelegate;
  public OnPathDelegate tmpPathCallback;
  public uint lastPathID;
  public List<IPathModifier> modifiers = new List<IPathModifier>();

  public Seeker()
  {
    this.onPathDelegate = new OnPathDelegate(this.OnPathComplete);
    this.onPartialPathDelegate = new OnPathDelegate(this.OnPartialPathComplete);
  }

  public void Awake() => this.startEndModifier.Awake(this);

  public Path GetCurrentPath() => this.path;

  public void OnDestroy()
  {
    this.ReleaseClaimedPath();
    this.startEndModifier.OnDestroy(this);
  }

  public void ReleaseClaimedPath()
  {
    if (this.prevPath == null)
      return;
    this.prevPath.Release((object) this, true);
    this.prevPath = (Path) null;
  }

  public void RegisterModifier(IPathModifier mod)
  {
    this.modifiers.Add(mod);
    this.modifiers.Sort((Comparison<IPathModifier>) ((a, b) => a.Order.CompareTo(b.Order)));
  }

  public void DeregisterModifier(IPathModifier mod) => this.modifiers.Remove(mod);

  public void PostProcess(Path p) => this.RunModifiers(Seeker.ModifierPass.PostProcess, p);

  public void RunModifiers(Seeker.ModifierPass pass, Path p)
  {
    if (pass == Seeker.ModifierPass.PreProcess && this.preProcessPath != null)
      this.preProcessPath(p);
    else if (pass == Seeker.ModifierPass.PostProcess && this.postProcessPath != null)
      this.postProcessPath(p);
    for (int index = 0; index < this.modifiers.Count; ++index)
    {
      MonoModifier modifier = this.modifiers[index] as MonoModifier;
      if (!((UnityEngine.Object) modifier != (UnityEngine.Object) null) || modifier.enabled)
      {
        switch (pass)
        {
          case Seeker.ModifierPass.PreProcess:
            this.modifiers[index].PreProcess(p);
            continue;
          case Seeker.ModifierPass.PostProcess:
            this.modifiers[index].Apply(p);
            continue;
          default:
            continue;
        }
      }
    }
  }

  public bool IsDone() => this.path == null || this.path.GetState() >= PathState.Returned;

  public void OnPathComplete(Path p) => this.OnPathComplete(p, true, true);

  public void OnPathComplete(Path p, bool runModifiers, bool sendCallbacks)
  {
    if (((p == null ? 0 : (p != this.path ? 1 : 0)) & (sendCallbacks ? 1 : 0)) != 0 || (UnityEngine.Object) this == (UnityEngine.Object) null || p == null || p != this.path)
      return;
    if (!this.path.error & runModifiers)
      this.RunModifiers(Seeker.ModifierPass.PostProcess, this.path);
    if (!sendCallbacks)
      return;
    p.Claim((object) this);
    this.lastCompletedNodePath = p.path;
    this.lastCompletedVectorPath = p.vectorPath;
    if (this.tmpPathCallback != null)
      this.tmpPathCallback(p);
    if (this.pathCallback != null)
      this.pathCallback(p);
    if (this.prevPath != null)
      this.prevPath.Release((object) this, true);
    this.prevPath = p;
    if (this.drawGizmos)
      return;
    this.ReleaseClaimedPath();
  }

  public void OnPartialPathComplete(Path p) => this.OnPathComplete(p, true, false);

  public void OnMultiPathComplete(Path p) => this.OnPathComplete(p, false, true);

  public ABPath GetNewPath(Vector3 start, Vector3 end) => ABPath.Construct(start, end);

  public Path StartPath(Vector3 start, Vector3 end)
  {
    return this.StartPath(start, end, (OnPathDelegate) null, -1);
  }

  public Path StartPath(Vector3 start, Vector3 end, OnPathDelegate callback)
  {
    return this.StartPath(start, end, callback, -1);
  }

  public Path StartPath(Vector3 start, Vector3 end, OnPathDelegate callback, int graphMask)
  {
    return this.StartPath((Path) this.GetNewPath(start, end), callback, graphMask);
  }

  public Path StartPath(Path p, OnPathDelegate callback = null, int graphMask = -1)
  {
    if (p is MultiTargetPath multiTargetPath)
    {
      OnPathDelegate[] onPathDelegateArray = new OnPathDelegate[multiTargetPath.targetPoints.Length];
      for (int index = 0; index < onPathDelegateArray.Length; ++index)
        onPathDelegateArray[index] = this.onPartialPathDelegate;
      multiTargetPath.callbacks = onPathDelegateArray;
      p.callback += new OnPathDelegate(this.OnMultiPathComplete);
    }
    else
      p.callback += this.onPathDelegate;
    p.enabledTags = this.traversableTags;
    p.tagPenalties = this.tagPenalties;
    p.nnConstraint.graphMask = graphMask;
    this.StartPathInternal(p, callback);
    return p;
  }

  public void StartPathInternal(Path p, OnPathDelegate callback)
  {
    if (this.path != null && this.path.GetState() <= PathState.Processing && (int) this.lastPathID == (int) this.path.pathID)
    {
      this.path.Error();
      this.path.LogError("Canceled path because a new one was requested.\nThis happens when a new path is requested from the seeker when one was already being calculated.\nFor example if a unit got a new order, you might request a new path directly instead of waiting for the now invalid path to be calculated. Which is probably what you want.\nIf you are getting this a lot, you might want to consider how you are scheduling path requests.");
    }
    this.path = p;
    this.tmpPathCallback = callback;
    this.lastPathID = (uint) this.path.pathID;
    this.RunModifiers(Seeker.ModifierPass.PreProcess, this.path);
    AstarPath.StartPath(this.path);
  }

  public MultiTargetPath StartMultiTargetPath(
    Vector3 start,
    Vector3[] endPoints,
    bool pathsForAll,
    OnPathDelegate callback = null,
    int graphMask = -1)
  {
    MultiTargetPath p = MultiTargetPath.Construct(start, endPoints, (OnPathDelegate[]) null);
    p.pathsForAll = pathsForAll;
    this.StartPath((Path) p, callback, graphMask);
    return p;
  }

  public MultiTargetPath StartMultiTargetPath(
    Vector3[] startPoints,
    Vector3 end,
    bool pathsForAll,
    OnPathDelegate callback = null,
    int graphMask = -1)
  {
    MultiTargetPath p = MultiTargetPath.Construct(startPoints, end, (OnPathDelegate[]) null);
    p.pathsForAll = pathsForAll;
    this.StartPath((Path) p, callback, graphMask);
    return p;
  }

  [Obsolete("You can use StartPath instead of this method now. It will behave identically.")]
  public MultiTargetPath StartMultiTargetPath(
    MultiTargetPath p,
    OnPathDelegate callback = null,
    int graphMask = -1)
  {
    this.StartPath((Path) p, callback, graphMask);
    return p;
  }

  public void OnDrawGizmos()
  {
    if (this.lastCompletedNodePath == null || !this.drawGizmos)
      return;
    if (this.detailedGizmos)
    {
      Gizmos.color = new Color(0.7f, 0.5f, 0.1f, 0.5f);
      if (this.lastCompletedNodePath != null)
      {
        for (int index = 0; index < this.lastCompletedNodePath.Count - 1; ++index)
          Gizmos.DrawLine((Vector3) this.lastCompletedNodePath[index].position, (Vector3) this.lastCompletedNodePath[index + 1].position);
      }
    }
    Gizmos.color = new Color(0.0f, 1f, 0.0f, 1f);
    if (this.lastCompletedVectorPath == null)
      return;
    for (int index = 0; index < this.lastCompletedVectorPath.Count - 1; ++index)
      Gizmos.DrawLine(this.lastCompletedVectorPath[index], this.lastCompletedVectorPath[index + 1]);
  }

  void ISerializationCallbackReceiver.OnBeforeSerialize()
  {
  }

  void ISerializationCallbackReceiver.OnAfterDeserialize()
  {
    if (this.traversableTagsCompatibility == null || this.traversableTagsCompatibility.tagsChange == -1)
      return;
    this.traversableTags = this.traversableTagsCompatibility.tagsChange;
    this.traversableTagsCompatibility = new TagMask(-1, -1);
  }

  public enum ModifierPass
  {
    PreProcess = 0,
    PostProcess = 2,
  }
}
