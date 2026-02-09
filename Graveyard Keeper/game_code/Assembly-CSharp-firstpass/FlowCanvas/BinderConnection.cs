// Decompiled with JetBrains decompiler
// Type: FlowCanvas.BinderConnection
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Serialization.FullSerializer;
using System;
using UnityEngine;

#nullable disable
namespace FlowCanvas;

public class BinderConnection : Connection
{
  [SerializeField]
  [fsProperty("_sourcePortName")]
  public string _sourcePortID;
  [SerializeField]
  [fsProperty("_targetPortName")]
  public string _targetPortID;
  [NonSerialized]
  public Port _sourcePort;
  [NonSerialized]
  public Port _targetPort;

  public string sourcePortID
  {
    get => this.sourcePort == null ? this._sourcePortID : this.sourcePort.ID;
    set => this._sourcePortID = value;
  }

  public string targetPortID
  {
    get => this.targetPort == null ? this._targetPortID : this.targetPort.ID;
    set => this._targetPortID = value;
  }

  public Port sourcePort
  {
    get
    {
      if (this._sourcePort == null && this.sourceNode is FlowNode)
        this._sourcePort = (this.sourceNode as FlowNode).GetOutputPort(this._sourcePortID);
      return this._sourcePort;
    }
  }

  public Port targetPort
  {
    get
    {
      if (this._targetPort == null && this.targetNode is FlowNode)
        this._targetPort = (this.targetNode as FlowNode).GetInputPort(this._targetPortID);
      return this._targetPort;
    }
  }

  public System.Type bindingType
  {
    get
    {
      return !this.GetType().RTIsGenericType() ? typeof (Flow) : this.GetType().RTGetGenericArguments()[0];
    }
  }

  public void GatherAndValidateSourcePort()
  {
    this._sourcePort = (Port) null;
    if (this.sourcePort != null && TypeConverter.HasConvertion(this.sourcePort.type, this.bindingType))
    {
      this.sourcePortID = this.sourcePort.ID;
      ++this.sourcePort.connections;
    }
    else
      this.graph.RemoveConnection((Connection) this, false);
  }

  public void GatherAndValidateTargetPort()
  {
    this._targetPort = (Port) null;
    if (this.targetPort != null)
    {
      if (System.Type.op_Equality(this.targetPort.type, this.bindingType))
      {
        this.targetPortID = this.targetPort.ID;
        ++this.targetPort.connections;
        return;
      }
      if (this.targetPort is ValueInput && this.sourcePort is ValueOutput && TypeConverter.HasConvertion(this.sourcePort.type, this.targetPort.type))
      {
        this.ReplaceWith(typeof (BinderConnection<>).MakeGenericType(this.bindingType));
        this.targetPortID = this.targetPort.ID;
        ++this.targetPort.connections;
        return;
      }
    }
    this.graph.RemoveConnection((Connection) this, false);
  }

  public static BinderConnection Create(Port source, Port target)
  {
    if (source == null || target == null)
      return (BinderConnection) null;
    if (source == target)
      return (BinderConnection) null;
    if (!source.CanAcceptConnections())
    {
      ParadoxNotion.Services.Logger.LogWarning((object) "Source port can accept no more connections.", "Editor", (object) source.parent);
      return (BinderConnection) null;
    }
    if (!target.CanAcceptConnections())
    {
      ParadoxNotion.Services.Logger.LogWarning((object) "Target port can accept no more connections.", "Editor", (object) source.parent);
      return (BinderConnection) null;
    }
    if (source.parent == target.parent)
    {
      ParadoxNotion.Services.Logger.LogWarning((object) "Can't connect ports on the same parent node.", "Editor", (object) source.parent);
      return (BinderConnection) null;
    }
    switch (source)
    {
      case FlowOutput _ when !(target is FlowInput):
        ParadoxNotion.Services.Logger.LogWarning((object) "Flow ports can only be connected to other Flow ports.", "Editor", (object) source.parent);
        return (BinderConnection) null;
      case FlowInput _ when target is FlowInput:
      case ValueInput _ when target is ValueInput:
        ParadoxNotion.Services.Logger.LogWarning((object) "Can't connect input to input.", "Editor", (object) source.parent);
        return (BinderConnection) null;
      case FlowOutput _ when target is FlowOutput:
      case ValueOutput _ when target is ValueOutput:
        ParadoxNotion.Services.Logger.LogWarning((object) "Can't connect output to output.", "Editor", (object) source.parent);
        return (BinderConnection) null;
      default:
        if (!TypeConverter.HasConvertion(source.type, target.type))
        {
          ParadoxNotion.Services.Logger.LogWarning((object) $"Can't connect ports. Type '{target.type.FriendlyName()}' is not assignable from Type '{source.type.FriendlyName()}' and there exists no automatic conversion for those types.", "Editor", (object) source.parent);
          return (BinderConnection) null;
        }
        BinderConnection binderConnection = (BinderConnection) null;
        if (source is FlowOutput && target is FlowInput)
          binderConnection = new BinderConnection();
        if (source is ValueOutput && target is ValueInput)
          binderConnection = (BinderConnection) Activator.CreateInstance(typeof (BinderConnection<>).RTMakeGenericType(target.type));
        binderConnection?.OnCreate(source, target);
        return binderConnection;
    }
  }

  public virtual void Bind()
  {
    if (!this.isActive || !(this.sourcePort is FlowOutput) || !(this.targetPort is FlowInput))
      return;
    (this.sourcePort as FlowOutput).BindTo((FlowInput) this.targetPort);
  }

  public virtual void UnBind()
  {
    if (!(this.sourcePort is FlowOutput))
      return;
    (this.sourcePort as FlowOutput).UnBind();
  }

  public void OnCreate(Port source, Port target)
  {
    this.sourceNode = (Node) source.parent;
    this.targetNode = (Node) target.parent;
    this.sourcePortID = source.ID;
    this.targetPortID = target.ID;
    this.sourceNode.outConnections.Add((Connection) this);
    this.targetNode.inConnections.Add((Connection) this);
    this.sourceNode.OnChildConnected(this.sourceNode.outConnections.Count - 1);
    this.targetNode.OnParentConnected(this.targetNode.inConnections.Count - 1);
    ++source.connections;
    ++target.connections;
    if (!Application.isPlaying)
      return;
    this.Bind();
  }

  public override void OnDestroy()
  {
    if (this.sourcePort != null)
      --this.sourcePort.connections;
    if (this.targetPort != null)
      --this.targetPort.connections;
    if (!Application.isPlaying)
      return;
    this.UnBind();
  }

  public BinderConnection ReplaceWith(System.Type t)
  {
    Port sourcePort = this.sourcePort;
    Port targetPort = this.targetPort;
    this.graph.RemoveConnection((Connection) this);
    Port target = targetPort;
    return BinderConnection.Create(sourcePort, target);
  }
}
