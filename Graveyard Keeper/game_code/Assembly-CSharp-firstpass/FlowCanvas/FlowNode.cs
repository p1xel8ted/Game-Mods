// Decompiled with JetBrains decompiler
// Type: FlowCanvas.FlowNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

#nullable disable
namespace FlowCanvas;

public abstract class FlowNode : NodeCanvas.Framework.Node, ISerializationCallbackReceiver
{
  [SerializeField]
  public Dictionary<string, object> _inputPortValues;
  public Dictionary<string, Port> inputPorts = new Dictionary<string, Port>((IEqualityComparer<string>) StringComparer.Ordinal);
  public Dictionary<string, Port> outputPorts = new Dictionary<string, Port>((IEqualityComparer<string>) StringComparer.Ordinal);

  public sealed override int maxInConnections => -1;

  public sealed override int maxOutConnections => -1;

  public sealed override bool allowAsPrime => false;

  public sealed override System.Type outConnectionType => typeof (BinderConnection);

  public sealed override Alignment2x2 commentsAlignment => Alignment2x2.Bottom;

  public override Alignment2x2 iconAlignment => Alignment2x2.Left;

  public FlowGraph flowGraph => (FlowGraph) this.graph;

  void ISerializationCallbackReceiver.OnBeforeSerialize()
  {
    if (this._inputPortValues == null)
      this._inputPortValues = new Dictionary<string, object>();
    foreach (ValueInput valueInput in this.inputPorts.Values.OfType<ValueInput>())
    {
      if (!valueInput.isConnected)
        this._inputPortValues[valueInput.ID] = valueInput.serializedValue;
    }
  }

  void ISerializationCallbackReceiver.OnAfterDeserialize()
  {
  }

  public sealed override void OnValidate(Graph flowGraph) => this.GatherPorts();

  public sealed override void OnParentConnected(int i)
  {
    if (i >= this.inConnections.Count || !(this.inConnections[i] is BinderConnection inConnection))
      return;
    this.TryHandleWildPortConnection(inConnection.targetPort, inConnection.sourcePort);
    this.OnPortConnected(inConnection.targetPort, inConnection.sourcePort);
  }

  public sealed override void OnChildConnected(int i)
  {
    if (i >= this.outConnections.Count || !(this.outConnections[i] is BinderConnection outConnection))
      return;
    this.TryHandleWildPortConnection(outConnection.sourcePort, outConnection.targetPort);
    this.OnPortConnected(outConnection.sourcePort, outConnection.targetPort);
  }

  public sealed override void OnParentDisconnected(int i)
  {
    if (i >= this.inConnections.Count || !(this.inConnections[i] is BinderConnection inConnection))
      return;
    this.OnPortDisconnected(inConnection.targetPort, inConnection.sourcePort);
  }

  public sealed override void OnChildDisconnected(int i)
  {
    if (i >= this.outConnections.Count || !(this.outConnections[i] is BinderConnection outConnection))
      return;
    this.OnPortDisconnected(outConnection.sourcePort, outConnection.targetPort);
  }

  public virtual void OnPortConnected(Port port, Port otherPort)
  {
  }

  public virtual void OnPortDisconnected(Port port, Port otherPort)
  {
  }

  public void BindPorts()
  {
    for (int index = 0; index < this.outConnections.Count; ++index)
      (this.outConnections[index] as BinderConnection).Bind();
  }

  public void UnBindPorts()
  {
    for (int index = 0; index < this.outConnections.Count; ++index)
      (this.outConnections[index] as BinderConnection).UnBind();
  }

  public Port GetInputPort(string ID)
  {
    Port inputPort = (Port) null;
    if (!this.inputPorts.TryGetValue(ID, out inputPort))
      inputPort = this.inputPorts.Values.FirstOrDefault<Port>((Func<Port, bool>) (p => p.name.SplitCamelCase() == ID));
    return inputPort;
  }

  public Port GetOutputPort(string ID)
  {
    Port outputPort = (Port) null;
    if (!this.outputPorts.TryGetValue(ID, out outputPort))
      outputPort = this.outputPorts.Values.FirstOrDefault<Port>((Func<Port, bool>) (p => p.name.SplitCamelCase() == ID));
    return outputPort;
  }

  public List<Port> GetOutputPorts() => this.outputPorts.Values.ToList<Port>();

  public BinderConnection GetInputConnectionForPortID(string ID)
  {
    return this.inConnections.OfType<BinderConnection>().FirstOrDefault<BinderConnection>((Func<BinderConnection, bool>) (c => c.targetPortID == ID));
  }

  public BinderConnection GetOutputConnectionForPortID(string ID)
  {
    return this.outConnections.OfType<BinderConnection>().FirstOrDefault<BinderConnection>((Func<BinderConnection, bool>) (c => c.sourcePortID == ID));
  }

  public Port GetFirstInputOfType(System.Type type)
  {
    return this.inputPorts.Values.OrderBy<Port, int>((Func<Port, int>) (p => !(p is FlowInput) ? 1 : 0)).FirstOrDefault<Port>((Func<Port, bool>) (p => p.type.RTIsAssignableFrom(type)));
  }

  public Port GetFirstOutputOfType(System.Type type)
  {
    return this.outputPorts.Values.OrderBy<Port, int>((Func<Port, int>) (p => !(p is FlowInput) ? 1 : 0)).FirstOrDefault<Port>((Func<Port, bool>) (p => type.RTIsAssignableFrom(p.type)));
  }

  public void AssignSelfInstancePort()
  {
    ValueInput valueInput = this.inputPorts.Values.OfType<ValueInput>().FirstOrDefault<ValueInput>();
    if (valueInput == null || valueInput.isConnected || !valueInput.isDefaultValue)
      return;
    UnityEngine.Object agentComponent = this.flowGraph.GetAgentComponent(valueInput.type);
    if (!(agentComponent != (UnityEngine.Object) null))
      return;
    valueInput.serializedValue = (object) agentComponent;
  }

  public void GatherPorts()
  {
    this.inputPorts.Clear();
    this.outputPorts.Clear();
    this.RegisterPorts();
    this.DeserializeInputPortValues();
    this.ValidateConnections();
  }

  public virtual void RegisterPorts()
  {
    this.TryAddReflectionBasedRegistrationForObject((object) this);
  }

  public void DeserializeInputPortValues()
  {
    if (this._inputPortValues == null)
      return;
    foreach (KeyValuePair<string, object> inputPortValue in this._inputPortValues)
    {
      KeyValuePair<string, object> pair = inputPortValue;
      Port port = (Port) null;
      if (!this.inputPorts.TryGetValue(pair.Key, out port))
        port = this.inputPorts.Values.FirstOrDefault<Port>((Func<Port, bool>) (p => p.name.SplitCamelCase() == pair.Key));
      if (port is ValueInput && pair.Value != null && port.type.RTIsAssignableFrom(pair.Value.GetType()))
        (port as ValueInput).serializedValue = pair.Value;
    }
  }

  public void ValidateConnections()
  {
    foreach (Connection connection in this.outConnections.ToArray())
    {
      if (connection is BinderConnection)
        (connection as BinderConnection).GatherAndValidateSourcePort();
    }
    foreach (Connection connection in this.inConnections.ToArray())
    {
      if (connection is BinderConnection)
        (connection as BinderConnection).GatherAndValidateTargetPort();
    }
  }

  public FlowInput AddFlowInput(string name, string ID, FlowHandler pointer)
  {
    return this.AddFlowInput(name, pointer, ID);
  }

  public FlowInput AddFlowInput(string name, FlowHandler pointer, string ID = "")
  {
    if (string.IsNullOrEmpty(name))
      name = " ";
    if (string.IsNullOrEmpty(ID))
      ID = name;
    return (FlowInput) (this.inputPorts[ID] = (Port) new FlowInput(this, name, ID, pointer));
  }

  public FlowOutput AddFlowOutput(string name, string ID = "")
  {
    if (string.IsNullOrEmpty(name))
      name = " ";
    if (string.IsNullOrEmpty(ID))
      ID = name;
    return (FlowOutput) (this.outputPorts[ID] = (Port) new FlowOutput(this, name, ID));
  }

  public ValueInput<T> AddValueInput<T>(string name, string ID = "")
  {
    if (string.IsNullOrEmpty(name))
      name = " ";
    if (string.IsNullOrEmpty(ID))
      ID = name;
    return (ValueInput<T>) (this.inputPorts[ID] = (Port) new ValueInput<T>(this, name, ID));
  }

  public ValueInput<T> AddVerticalValueInput<T>(string name, string ID = "")
  {
    if (string.IsNullOrEmpty(name))
      name = " ";
    if (string.IsNullOrEmpty(ID))
      ID = name;
    name += " ";
    return (ValueInput<T>) (this.inputPorts[ID] = (Port) new ValueInput<T>(this, name, ID));
  }

  public ValueOutput<T> AddValueOutput<T>(string name, string ID, ValueHandler<T> getter)
  {
    return this.AddValueOutput<T>(name, getter, ID);
  }

  public ValueOutput<T> AddValueOutput<T>(string name, ValueHandler<T> getter, string ID = "")
  {
    if (string.IsNullOrEmpty(name))
      name = " ";
    if (string.IsNullOrEmpty(ID))
      ID = name;
    return (ValueOutput<T>) (this.outputPorts[ID] = (Port) new ValueOutput<T>(this, name, ID, getter));
  }

  public ValueInput AddValueInput(string name, System.Type type, string ID = "")
  {
    if (string.IsNullOrEmpty(name))
      name = " ";
    if (string.IsNullOrEmpty(ID))
      ID = name;
    return (ValueInput) (this.inputPorts[ID] = (Port) ValueInput.CreateInstance(type, this, name, ID));
  }

  public ValueOutput AddValueOutput(string name, string ID, System.Type type, ValueHandlerObject getter)
  {
    return this.AddValueOutput(name, type, getter, ID);
  }

  public ValueOutput AddValueOutput(string name, System.Type type, ValueHandlerObject getter, string ID = "")
  {
    if (string.IsNullOrEmpty(name))
      name = " ";
    if (string.IsNullOrEmpty(ID))
      ID = name;
    return (ValueOutput) (this.outputPorts[ID] = (Port) ValueOutput.CreateInstance(type, this, name, ID, getter));
  }

  public void TryAddReflectionBasedRegistrationForObject(object instance)
  {
    foreach (MethodInfo method in instance.GetType().GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public))
      this.TryAddMethodFlowInput(method, instance);
    foreach (PropertyInfo property in instance.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public))
      this.TryAddPropertyValueOutput(property, instance);
    foreach (FieldInfo field in instance.GetType().GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public))
    {
      this.TryAddFieldDelegateFlowOutput(field, instance);
      this.TryAddFieldDelegateValueInput(field, instance);
    }
  }

  public FlowInput TryAddMethodFlowInput(MethodInfo method, object instance)
  {
    ParameterInfo[] parameters = method.GetParameters();
    if (!System.Type.op_Equality(method.ReturnType, typeof (void)) || parameters.Length != 1 || !System.Type.op_Equality(parameters[0].ParameterType, typeof (Flow)))
      return (FlowInput) null;
    NameAttribute attribute = method.RTGetAttribute<NameAttribute>(false);
    return this.AddFlowInput(attribute != null ? attribute.name : method.Name, method.RTCreateDelegate<FlowHandler>(instance));
  }

  public FlowOutput TryAddFieldDelegateFlowOutput(FieldInfo field, object instance)
  {
    if (!System.Type.op_Equality(field.FieldType, typeof (FlowHandler)))
      return (FlowOutput) null;
    NameAttribute attribute = field.RTGetAttribute<NameAttribute>(false);
    FlowOutput flowOutput = this.AddFlowOutput(attribute != null ? attribute.name : field.Name);
    field.SetValue(instance, (object) new FlowHandler(flowOutput.Call));
    return flowOutput;
  }

  public ValueInput TryAddFieldDelegateValueInput(FieldInfo field, object instance)
  {
    if (typeof (Delegate).RTIsAssignableFrom(field.FieldType))
    {
      MethodInfo method = field.FieldType.GetMethod("Invoke");
      ParameterInfo[] parameters = method.GetParameters();
      if (System.Type.op_Inequality(method.ReturnType, typeof (void)) && parameters.Length == 0)
      {
        NameAttribute attribute = field.RTGetAttribute<NameAttribute>(false);
        string key = attribute != null ? attribute.name : field.Name;
        System.Type returnType = method.ReturnType;
        ValueInput instance1 = (ValueInput) Activator.CreateInstance(typeof (ValueInput<>).RTMakeGenericType(returnType), instance, (object) key, (object) key);
        System.Type type = typeof (ValueHandler<>).RTMakeGenericType(returnType);
        Delegate @delegate = instance1.GetType().GetMethod("get_value").RTCreateDelegate(type, (object) instance1);
        field.SetValue(instance, (object) @delegate);
        this.inputPorts[key] = (Port) instance1;
        return instance1;
      }
    }
    return (ValueInput) null;
  }

  public ValueOutput TryAddPropertyValueOutput(PropertyInfo prop, object instance)
  {
    if (!prop.CanRead)
      return (ValueOutput) null;
    NameAttribute attribute = prop.RTGetAttribute<NameAttribute>(false);
    string key = attribute != null ? attribute.name : prop.Name;
    System.Type type = typeof (ValueHandler<>).RTMakeGenericType(prop.PropertyType);
    Delegate @delegate = prop.RTGetGetMethod().RTCreateDelegate(type, instance);
    ValueOutput instance1 = (ValueOutput) Activator.CreateInstance(typeof (ValueOutput<>).RTMakeGenericType(prop.PropertyType), (object) this, (object) key, (object) key, (object) @delegate);
    return (ValueOutput) (this.outputPorts[key] = (Port) instance1);
  }

  public FlowNode ReplaceWith(System.Type t)
  {
    if (!(this.graph.AddNode(t, this.nodePosition) is FlowNode flowNode))
      return (FlowNode) null;
    foreach (Connection connection in this.inConnections.ToArray())
      connection.SetTarget((NodeCanvas.Framework.Node) flowNode);
    foreach (Connection connection in this.outConnections.ToArray())
      connection.SetSource((NodeCanvas.Framework.Node) flowNode);
    if (this._inputPortValues != null)
      flowNode._inputPortValues = this._inputPortValues.ToDictionary<KeyValuePair<string, object>, string, object>((Func<KeyValuePair<string, object>, string>) (k => k.Key), (Func<KeyValuePair<string, object>, object>) (v => v.Value));
    flowNode.GatherPorts();
    this.graph.RemoveNode((NodeCanvas.Framework.Node) this);
    return flowNode;
  }

  public virtual System.Type GetNodeWildDefinitionType()
  {
    return this.GetType().GetFirstGenericParameterConstraintType();
  }

  public void TryHandleWildPortConnection(Port port, Port otherPort)
  {
    System.Type wildDefinitionType = this.GetNodeWildDefinitionType();
    System.Type type = this.GetType();
    Port port1 = port;
    Port otherPort1 = otherPort;
    System.Type content = type;
    System.Type genericTypeForWild = FlowNode.TryGetNewGenericTypeForWild(wildDefinitionType, port1, otherPort1, content, (System.Type) null);
    if (!System.Type.op_Inequality(genericTypeForWild, (System.Type) null))
      return;
    this.ReplaceWith(genericTypeForWild);
  }

  public static System.Type TryGetNewGenericTypeForWild(
    System.Type wildType,
    Port port,
    Port otherPort,
    System.Type content,
    System.Type context)
  {
    if (System.Type.op_Equality(wildType, (System.Type) null) || !content.IsGenericType)
      return (System.Type) null;
    System.Type[] genericArguments = content.GetGenericArguments();
    System.Type content1 = ((IEnumerable<System.Type>) genericArguments).FirstOrDefault<System.Type>();
    if (System.Type.op_Inequality(content1, wildType) && content1.IsGenericType)
      return FlowNode.TryGetNewGenericTypeForWild(wildType, port, otherPort, content1, content);
    if (genericArguments.Length == 1 && System.Type.op_Equality(content1, wildType))
    {
      System.Type enumerableElementType1 = otherPort.type.GetEnumerableElementType();
      System.Type enumerableElementType2 = port.type.GetEnumerableElementType();
      int num = !System.Type.op_Inequality(enumerableElementType1, (System.Type) null) ? 0 : (System.Type.op_Inequality(enumerableElementType2, (System.Type) null) ? 1 : 0);
      System.Type type1 = num != 0 ? enumerableElementType2 : port.type;
      System.Type type2 = num != 0 ? enumerableElementType1 : otherPort.type;
      if (System.Type.op_Equality(type1, wildType) && System.Type.op_Inequality(type2, type1))
      {
        content = content.GetGenericTypeDefinition();
        System.Type genericArgument = ((IEnumerable<System.Type>) content.GetGenericArguments()).First<System.Type>();
        if (type2.IsAllowedByGenericArgument(genericArgument))
        {
          System.Type genericTypeForWild = content.MakeGenericType(type2);
          if (System.Type.op_Inequality(context, (System.Type) null) && context.IsGenericType)
            genericTypeForWild = context.GetGenericTypeDefinition().MakeGenericType(genericTypeForWild);
          return genericTypeForWild;
        }
      }
    }
    return (System.Type) null;
  }

  public static MethodInfo TryGetNewGenericMethodForWild(
    System.Type wildType,
    Port port,
    Port otherPort,
    MethodInfo content)
  {
    if (System.Type.op_Equality(wildType, (System.Type) null) || !content.IsGenericMethod)
      return (MethodInfo) null;
    System.Type[] genericArguments = content.GetGenericArguments();
    System.Type type1 = ((IEnumerable<System.Type>) genericArguments).FirstOrDefault<System.Type>();
    if (genericArguments.Length == 1 && System.Type.op_Equality(type1, wildType))
    {
      System.Type enumerableElementType1 = otherPort.type.GetEnumerableElementType();
      System.Type enumerableElementType2 = port.type.GetEnumerableElementType();
      int num = !System.Type.op_Inequality(enumerableElementType1, (System.Type) null) ? 0 : (System.Type.op_Inequality(enumerableElementType2, (System.Type) null) ? 1 : 0);
      System.Type type2 = num != 0 ? enumerableElementType2 : port.type;
      System.Type type3 = num != 0 ? enumerableElementType1 : otherPort.type;
      if (System.Type.op_Equality(type2, wildType) && System.Type.op_Inequality(type3, type2))
      {
        content = content.GetGenericMethodDefinition();
        System.Type genericArgument = ((IEnumerable<System.Type>) content.GetGenericArguments()).First<System.Type>();
        if (type3.IsAllowedByGenericArgument(genericArgument))
          return content.MakeGenericMethod(type3);
      }
    }
    return (MethodInfo) null;
  }

  public void Call(FlowOutput port, Flow f) => port.Call(f);

  [AttributeUsage(AttributeTargets.Class)]
  public class ContextDefinedInputsAttribute : Attribute
  {
    public System.Type[] types;

    public ContextDefinedInputsAttribute(params System.Type[] types) => this.types = types;
  }

  [AttributeUsage(AttributeTargets.Class)]
  public class ContextDefinedOutputsAttribute : Attribute
  {
    public System.Type[] types;

    public ContextDefinedOutputsAttribute(params System.Type[] types) => this.types = types;
  }
}
