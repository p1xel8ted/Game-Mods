// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.ReflectedFieldNodeWrapper
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion;
using ParadoxNotion.Design;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[DoNotList]
[ParadoxNotion.Design.Icon("", false, "GetRuntimeIconType")]
public class ReflectedFieldNodeWrapper : FlowNode
{
  [SerializeField]
  public string fieldName;
  [SerializeField]
  public System.Type targetType;
  [SerializeField]
  public ReflectedFieldNodeWrapper.AccessMode accessMode;
  [CompilerGenerated]
  public BaseReflectedFieldNode \u003CreflectedFieldNode\u003Ek__BackingField;
  public FieldInfo _field;

  public System.Type GetRuntimeIconType()
  {
    return !FieldInfo.op_Inequality(this.field, (FieldInfo) null) ? (System.Type) null : this.field.DeclaringType;
  }

  public BaseReflectedFieldNode reflectedFieldNode
  {
    get => this.\u003CreflectedFieldNode\u003Ek__BackingField;
    set => this.\u003CreflectedFieldNode\u003Ek__BackingField = value;
  }

  public FieldInfo field
  {
    get
    {
      return FieldInfo.op_Inequality(this._field, (FieldInfo) null) ? this._field : (this._field = System.Type.op_Inequality(this.targetType, (System.Type) null) ? this.targetType.GetField(this.fieldName) : (FieldInfo) null);
    }
  }

  public override string name
  {
    get
    {
      if (!FieldInfo.op_Inequality(this.field, (FieldInfo) null))
        return $"* Missing '{(System.Type.op_Inequality(this.targetType, (System.Type) null) ? (object) this.targetType.Name : (object) "null")}.{this.fieldName}' *";
      bool flag = this.accessMode == ReflectedFieldNodeWrapper.AccessMode.GetField;
      bool isStatic = this.field.IsStatic;
      if (this.field.IsConstant())
        return $"{this.field.DeclaringType.FriendlyName()}.{this.field.Name}";
      return isStatic ? $"{(flag ? (object) "Get" : (object) "Set")} {this.field.DeclaringType.FriendlyName()}.{this.field.Name}" : $"{(flag ? (object) "Get" : (object) "Set")} {this.field.Name}";
    }
  }

  public void SetField(
    FieldInfo newField,
    ReflectedFieldNodeWrapper.AccessMode mode,
    object instance = null)
  {
    if (FieldInfo.op_Equality(newField, (FieldInfo) null))
      return;
    newField = newField.GetBaseDefinition();
    this.fieldName = newField.Name;
    this.targetType = newField.DeclaringType;
    this.accessMode = mode;
    this.GatherPorts();
    if (instance == null || newField.IsStatic)
      return;
    ValueInput firstInputOfType = (ValueInput) this.GetFirstInputOfType(instance.GetType());
    if (firstInputOfType == null)
      return;
    firstInputOfType.serializedValue = instance;
  }

  public override void RegisterPorts()
  {
    if (FieldInfo.op_Equality(this.field, (FieldInfo) null))
      return;
    this.reflectedFieldNode = BaseReflectedFieldNode.GetFieldNode(this.field);
    if (this.reflectedFieldNode == null)
      return;
    this.reflectedFieldNode.RegisterPorts((FlowNode) this, this.accessMode);
  }

  public enum AccessMode
  {
    GetField,
    SetField,
  }
}
