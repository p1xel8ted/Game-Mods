// Decompiled with JetBrains decompiler
// Type: CustomFlowScript
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using FlowCanvas;
using NodeCanvas.Framework;
using UnityEngine;

#nullable disable
public class CustomFlowScript : CustomScript
{
  public FlowScriptController _fsc;
  public Blackboard _bb;
  public CustomFlowScript.OnFinishedDelegate _on_finished;
  public UniversalStorage storage = new UniversalStorage();

  public static CustomFlowScript Create(
    GameObject parent_go,
    string script_name,
    bool is_global = false,
    CustomFlowScript.OnFinishedDelegate on_finished = null)
  {
    FlowGraph graph = CustomFlowScript.GetGraph(script_name);
    if (!((Object) graph == (Object) null))
      return CustomFlowScript.Create(parent_go, graph, is_global, on_finished, script_name);
    Debug.LogError((object) ("Error loading graph: " + script_name));
    return (CustomFlowScript) null;
  }

  public static CustomFlowScript Create(
    GameObject parent_go,
    FlowGraph g,
    bool is_global = false,
    CustomFlowScript.OnFinishedDelegate on_finished = null,
    string custom_script_name = null)
  {
    string str = string.IsNullOrEmpty(custom_script_name) ? g.name : custom_script_name;
    Debug.Log((object) $"<color=yellow>Run FlowScript:</color> {str}, parent_go: {((Object) parent_go != (Object) null ? parent_go.name : "null")}, is_global = {is_global.ToString()}", (Object) parent_go);
    Stats.DesignEvent("FlowScript:" + str);
    GameObject gameObject = new GameObject("[FS] " + str);
    if ((Object) parent_go != (Object) null)
      gameObject.transform.SetParent(parent_go.transform, false);
    CustomFlowScript customFlowScript = gameObject.AddComponent<CustomFlowScript>();
    customFlowScript.is_global = is_global;
    customFlowScript.script_name = str;
    customFlowScript._fsc = gameObject.AddComponent<FlowScriptController>();
    customFlowScript._fsc.disableAction = GraphOwner.DisableAction.DoNothing;
    customFlowScript._bb = gameObject.AddComponent<Blackboard>();
    customFlowScript._on_finished = on_finished;
    g.blackboard = customFlowScript._fsc.blackboard = (IBlackboard) customFlowScript._bb;
    customFlowScript._fsc.graph = (Graph) g;
    customFlowScript.started = true;
    return customFlowScript;
  }

  public static FlowGraph GetGraph(string name)
  {
    FlowScript resourceAs = SmartResourceHelper.GetResourceAs<FlowScript>("FlowCanvas/" + name);
    if (!((Object) resourceAs == (Object) null))
      return (FlowGraph) resourceAs;
    Debug.LogError((object) ("No flow script: " + name));
    return (FlowGraph) null;
  }

  public void FireEvent(string event_id)
  {
    Debug.Log((object) $"Fire event:[{event_id}] on global script:[{this.script_name}]");
    this._fsc.SendEvent(event_id);
  }

  public void FireEvent(string event_id, string param)
  {
    Debug.Log((object) $"Fire event:[{event_id}] on global script:[{this.script_name}] param:[{param}]");
    this._fsc.SendEvent<string>(event_id, param);
  }

  public void StartBehaviour() => this._fsc.StartBehaviour();

  public override void TerminateMe()
  {
    this._fsc.PauseBehaviour();
    this._fsc.graph = (Graph) null;
    base.TerminateMe();
    if (this._on_finished == null)
      return;
    this._on_finished(this.script_name);
  }

  public delegate void OnFinishedDelegate(string script_name);
}
