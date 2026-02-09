// Decompiled with JetBrains decompiler
// Type: NodeCanvas.BehaviourTrees.BehaviourTree
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using System;
using UnityEngine;

#nullable disable
namespace NodeCanvas.BehaviourTrees;

[GraphInfo(packageName = "NodeCanvas", docsURL = "http://nodecanvas.paradoxnotion.com/documentation/", resourcesURL = "http://nodecanvas.paradoxnotion.com/downloads/", forumsURL = "http://nodecanvas.paradoxnotion.com/forums-page/")]
public class BehaviourTree : Graph
{
  [SerializeField]
  public bool repeat = true;
  [SerializeField]
  public float updateInterval;
  public float intervalCounter;
  public NodeCanvas.Status _rootStatus = NodeCanvas.Status.Resting;

  public override object OnDerivedDataSerialization()
  {
    return (object) new BehaviourTree.DerivedSerializationData()
    {
      repeat = this.repeat,
      updateInterval = this.updateInterval
    };
  }

  public override void OnDerivedDataDeserialization(object data)
  {
    if (!(data is BehaviourTree.DerivedSerializationData))
      return;
    this.repeat = ((BehaviourTree.DerivedSerializationData) data).repeat;
    this.updateInterval = ((BehaviourTree.DerivedSerializationData) data).updateInterval;
  }

  public static event Action<BehaviourTree, NodeCanvas.Status> onRootStatusChanged;

  public NodeCanvas.Status rootStatus
  {
    get => this._rootStatus;
    set
    {
      if (this._rootStatus == value)
        return;
      this._rootStatus = value;
      if (BehaviourTree.onRootStatusChanged == null)
        return;
      BehaviourTree.onRootStatusChanged(this, value);
    }
  }

  public override System.Type baseNodeType => typeof (BTNode);

  public override bool requiresAgent => true;

  public override bool requiresPrimeNode => true;

  public override bool autoSort => true;

  public override bool useLocalBlackboard => false;

  public override void OnGraphStarted()
  {
    this.intervalCounter = this.updateInterval;
    this.rootStatus = this.primeNode.status;
  }

  public override void OnGraphUpdate()
  {
    if ((double) this.intervalCounter >= (double) this.updateInterval)
    {
      this.intervalCounter = 0.0f;
      if (this.Tick(this.agent, this.blackboard) != NodeCanvas.Status.Running && !this.repeat)
        this.Stop(this.rootStatus == NodeCanvas.Status.Success);
    }
    if ((double) this.updateInterval <= 0.0)
      return;
    this.intervalCounter += Time.deltaTime;
  }

  public NodeCanvas.Status Tick(Component agent, IBlackboard blackboard)
  {
    if (this.rootStatus != NodeCanvas.Status.Running)
      this.primeNode.Reset();
    this.rootStatus = this.primeNode.Execute(agent, blackboard);
    return this.rootStatus;
  }

  [Serializable]
  public struct DerivedSerializationData
  {
    public bool repeat;
    public float updateInterval;
  }
}
