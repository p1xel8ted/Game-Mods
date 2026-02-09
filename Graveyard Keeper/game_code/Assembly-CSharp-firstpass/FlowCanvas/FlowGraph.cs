// Decompiled with JetBrains decompiler
// Type: FlowCanvas.FlowGraph
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using FlowCanvas.Macros;
using FlowCanvas.Nodes;
using NodeCanvas.Framework;
using ParadoxNotion;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace FlowCanvas;

[GraphInfo(packageName = "FlowCanvas", docsURL = "http://flowcanvas.paradoxnotion.com/documentation/", resourcesURL = "http://flowcanvas.paradoxnotion.com/downloads/", forumsURL = "http://flowcanvas.paradoxnotion.com/forums-page/")]
[Serializable]
public abstract class FlowGraph : Graph
{
  public bool hasInitialized;
  public List<IUpdatable> updatableNodes;
  public Dictionary<string, CustomFunctionEvent> functions;
  public Dictionary<System.Type, Component> cachedAgentComponents = new Dictionary<System.Type, Component>();

  public override System.Type baseNodeType => typeof (FlowNode);

  public override bool useLocalBlackboard => false;

  public sealed override bool requiresAgent => false;

  public sealed override bool requiresPrimeNode => false;

  public sealed override bool autoSort => false;

  public T CallFunction<T>(string name, params object[] args) => (T) this.CallFunction(name, args);

  public object CallFunction(string name, params object[] args)
  {
    CustomFunctionEvent customFunctionEvent = (CustomFunctionEvent) null;
    return this.functions.TryGetValue(name, out customFunctionEvent) ? customFunctionEvent.Invoke(new Flow(), args) : (object) null;
  }

  public UnityEngine.Object GetAgentComponent(System.Type type)
  {
    if ((UnityEngine.Object) this.agent == (UnityEngine.Object) null)
      return (UnityEngine.Object) null;
    if (System.Type.op_Equality(type, typeof (GameObject)))
      return (UnityEngine.Object) this.agent.gameObject;
    if (System.Type.op_Equality(type, typeof (Transform)))
      return (UnityEngine.Object) this.agent.transform;
    if (System.Type.op_Equality(type, typeof (Component)))
      return (UnityEngine.Object) this.agent;
    Component agentComponent = (Component) null;
    if (this.cachedAgentComponents.TryGetValue(type, out agentComponent))
      return (UnityEngine.Object) agentComponent;
    if (typeof (Component).RTIsAssignableFrom(type))
      agentComponent = this.agent.GetComponent(type);
    return (UnityEngine.Object) (this.cachedAgentComponents[type] = agentComponent);
  }

  public override void OnGraphStarted()
  {
    if (!this.hasInitialized)
    {
      this.updatableNodes = new List<IUpdatable>();
      this.functions = new Dictionary<string, CustomFunctionEvent>((IEqualityComparer<string>) StringComparer.Ordinal);
    }
    for (int index = 0; index < this.allNodes.Count; ++index)
    {
      NodeCanvas.Framework.Node allNode = this.allNodes[index];
      if (allNode is MacroNodeWrapper)
      {
        MacroNodeWrapper macroNodeWrapper = (MacroNodeWrapper) allNode;
        if ((UnityEngine.Object) macroNodeWrapper.macro != (UnityEngine.Object) null)
        {
          macroNodeWrapper.CheckInstance();
          macroNodeWrapper.macro.StartGraph(this.agent, this.blackboard, false);
        }
      }
      if (!this.hasInitialized)
      {
        if (allNode is IUpdatable)
          this.updatableNodes.Add((IUpdatable) allNode);
        if (allNode is CustomFunctionEvent)
        {
          CustomFunctionEvent customFunctionEvent = (CustomFunctionEvent) allNode;
          this.functions[customFunctionEvent.identifier] = customFunctionEvent;
        }
      }
    }
    if (!this.hasInitialized)
    {
      for (int index = 0; index < this.allNodes.Count; ++index)
      {
        if (this.allNodes[index] is FlowNode)
        {
          FlowNode allNode = (FlowNode) this.allNodes[index];
          allNode.AssignSelfInstancePort();
          allNode.BindPorts();
        }
      }
    }
    this.hasInitialized = true;
  }

  public override void OnGraphUpdate()
  {
    if (this.updatableNodes == null || this.updatableNodes.Count <= 0)
      return;
    for (int index = 0; index < this.updatableNodes.Count; ++index)
      this.updatableNodes[index].Update();
  }

  public override void OnGraphStoped()
  {
    for (int index = 0; index < this.allNodes.Count; ++index)
    {
      NodeCanvas.Framework.Node allNode = this.allNodes[index];
      if (allNode is MacroNodeWrapper)
      {
        MacroNodeWrapper macroNodeWrapper = (MacroNodeWrapper) allNode;
        if ((UnityEngine.Object) macroNodeWrapper.macro != (UnityEngine.Object) null)
          macroNodeWrapper.macro.Stop();
      }
    }
  }

  public static System.Type[] FindCustomObjectWrappers(System.Type targetType)
  {
    List<System.Type> typeList = new List<System.Type>();
    foreach (System.Type allType in ReflectionTools.GetAllTypes())
    {
      if (allType.IsSubclassOf(typeof (CustomObjectWrapper)))
      {
        System.Type[] genericArguments = allType.BaseType.GetGenericArguments();
        if (genericArguments.Length == 1 && System.Type.op_Equality(genericArguments[0], targetType))
          typeList.Add(allType);
      }
    }
    return typeList.ToArray();
  }
}
