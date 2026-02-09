// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.BaseReflectedMethodNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace FlowCanvas.Nodes;

public abstract class BaseReflectedMethodNode
{
  public MethodInfo methodInfo;
  public List<ParamDef> paramDefinitions;
  public ParamDef instanceDef;
  public ParamDef resultDef;
  public ReflectedMethodRegistrationOptions options;

  public static event Func<MethodInfo, BaseReflectedMethodNode> OnGetAotReflectedMethodNode;

  public static BaseReflectedMethodNode GetMethodNode(
    MethodInfo targetMethod,
    ReflectedMethodRegistrationOptions options)
  {
    ParametresDef parametres;
    if (!ReflectedNodesHelper.InitParams(targetMethod, out parametres))
      return (BaseReflectedMethodNode) null;
    JitMethodNode methodNode1 = new JitMethodNode();
    methodNode1.options = options;
    if (methodNode1.Init(targetMethod, parametres))
      return (BaseReflectedMethodNode) methodNode1;
    if (BaseReflectedMethodNode.OnGetAotReflectedMethodNode != null)
    {
      BaseReflectedMethodNode methodNode2 = BaseReflectedMethodNode.OnGetAotReflectedMethodNode(targetMethod);
      if (methodNode2 != null)
      {
        methodNode2.options = options;
        if (methodNode2.Init(targetMethod, parametres))
          return methodNode2;
      }
    }
    PureReflectedMethodNode reflectedMethodNode = new PureReflectedMethodNode();
    reflectedMethodNode.options = options;
    return !reflectedMethodNode.Init(targetMethod, parametres) ? (BaseReflectedMethodNode) null : (BaseReflectedMethodNode) reflectedMethodNode;
  }

  public bool Init(MethodInfo method, ParametresDef parametres)
  {
    if (MethodInfo.op_Equality(method, (MethodInfo) null) || method.ContainsGenericParameters || method.IsGenericMethodDefinition)
      return false;
    this.paramDefinitions = parametres.paramDefinitions == null ? new List<ParamDef>() : parametres.paramDefinitions;
    this.instanceDef = parametres.instanceDef;
    this.resultDef = parametres.resultDef;
    this.methodInfo = method;
    return this.InitInternal(method);
  }

  public abstract bool InitInternal(MethodInfo method);

  public abstract void RegisterPorts(FlowNode node, ReflectedMethodRegistrationOptions options);
}
