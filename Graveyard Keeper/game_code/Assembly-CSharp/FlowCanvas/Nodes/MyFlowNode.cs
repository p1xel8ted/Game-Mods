// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.MyFlowNode
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

public class MyFlowNode : FlowControlNode
{
  public static WorldGameObject GetWGOFromNode(FlowNode node)
  {
    Component graphAgent = node.graphAgent;
    if ((Object) graphAgent == (Object) null)
      return (WorldGameObject) null;
    GameObject gameObject = graphAgent.gameObject;
    WorldGameObject component = gameObject.GetComponent<WorldGameObject>();
    if ((Object) component != (Object) null)
      return component;
    return (Object) gameObject.transform.parent == (Object) null ? (WorldGameObject) null : gameObject.transform.parent.GetComponent<WorldGameObject>();
  }

  public WorldGameObject wgo => MyFlowNode.GetWGOFromNode((FlowNode) this);

  public WorldGameObject WGOParamOrSelf(ValueInput<WorldGameObject> param)
  {
    if ((Object) param.value != (Object) null)
      return param.value;
    WorldGameObject wgo = this.wgo;
    if (!((Object) wgo == (Object) null))
      return wgo;
    Debug.LogError((object) "WGO is null");
    return wgo;
  }

  public bool IsEmptyStringInputPort(string port_id)
  {
    ValueInput<string> inputValuePort = this.GetInputValuePort<string>(port_id);
    return !inputValuePort.isConnected && string.IsNullOrEmpty(inputValuePort.value);
  }

  public ValueInput<T> GetInputValuePort<T>(string port_id)
  {
    return this.GetInputPort(port_id) as ValueInput<T>;
  }

  public ValueInput GetInputValuePort(string port_id) => this.GetInputPort(port_id) as ValueInput;

  public void MakeStringNullIfEmpty(string port_id)
  {
    if (!this.IsEmptyStringInputPort(port_id))
      return;
    this.GetInputValuePort<string>(port_id).serializedValue = (object) null;
  }

  public CustomFlowScript cfs
  {
    get
    {
      if ((Object) this.graph == (Object) null)
      {
        Debug.LogError((object) "Can't get CFS because graph is null", (Object) this.graph);
        return (CustomFlowScript) null;
      }
      return !(bool) (Object) this.graphAgent ? (CustomFlowScript) null : this.graphAgent.gameObject.GetComponent<CustomFlowScript>();
    }
  }

  public T GetVariable<T>(string name) => this.cfs.storage.Get<T>(name);

  public void SetVariable<T>(string name, T val) => this.cfs.storage.Set<T>(name, val);

  public virtual void OnNodeInspectorGUI()
  {
  }
}
