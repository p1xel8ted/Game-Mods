// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.BaseReflectedFieldNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Reflection;

#nullable disable
namespace FlowCanvas.Nodes;

public abstract class BaseReflectedFieldNode
{
  public FieldInfo fieldInfo;
  public ParamDef instanceDef;
  public ParamDef resultDef;

  public static event Func<FieldInfo, BaseReflectedFieldNode> OnGetAotReflectedFieldNode;

  public static BaseReflectedFieldNode GetFieldNode(FieldInfo targetField)
  {
    ParametresDef parametres;
    if (!ReflectedNodesHelper.InitParams(targetField, out parametres))
      return (BaseReflectedFieldNode) null;
    JitFieldNode fieldNode1 = new JitFieldNode();
    if (fieldNode1.Init(targetField, parametres))
      return (BaseReflectedFieldNode) fieldNode1;
    if (BaseReflectedFieldNode.OnGetAotReflectedFieldNode != null)
    {
      BaseReflectedFieldNode fieldNode2 = BaseReflectedFieldNode.OnGetAotReflectedFieldNode(targetField);
      if (fieldNode2 != null && fieldNode2.Init(targetField, parametres))
        return fieldNode2;
    }
    PureReflectedFieldNode reflectedFieldNode = new PureReflectedFieldNode();
    return !reflectedFieldNode.Init(targetField, parametres) ? (BaseReflectedFieldNode) null : (BaseReflectedFieldNode) reflectedFieldNode;
  }

  public bool Init(FieldInfo field, ParametresDef parametres)
  {
    if (FieldInfo.op_Equality(field, (FieldInfo) null) || field.FieldType.ContainsGenericParameters || field.FieldType.IsGenericTypeDefinition)
      return false;
    this.instanceDef = parametres.instanceDef;
    this.resultDef = parametres.resultDef;
    this.fieldInfo = field;
    return this.InitInternal(this.fieldInfo);
  }

  public abstract bool InitInternal(FieldInfo field);

  public abstract void RegisterPorts(FlowNode node, ReflectedFieldNodeWrapper.AccessMode accessMode);
}
