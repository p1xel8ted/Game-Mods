// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.BaseReflectedConstructorNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace FlowCanvas.Nodes;

public abstract class BaseReflectedConstructorNode
{
  public ConstructorInfo constructorInfo;
  public List<ParamDef> paramDefinitions;
  public ParamDef instanceDef;
  public ParamDef resultDef;
  public ReflectedMethodRegistrationOptions options;

  public static event Func<ConstructorInfo, BaseReflectedConstructorNode> OnGetAotReflectedConstructorNode;

  public static BaseReflectedConstructorNode GetConstructorNode(
    ConstructorInfo targetConstructor,
    ReflectedMethodRegistrationOptions options)
  {
    ParametresDef parametres;
    if (!ReflectedNodesHelper.InitParams(targetConstructor, out parametres))
      return (BaseReflectedConstructorNode) null;
    JitConstructorNode constructorNode1 = new JitConstructorNode();
    constructorNode1.options = options;
    if (constructorNode1.Init(targetConstructor, parametres))
      return (BaseReflectedConstructorNode) constructorNode1;
    if (BaseReflectedConstructorNode.OnGetAotReflectedConstructorNode != null)
    {
      BaseReflectedConstructorNode constructorNode2 = BaseReflectedConstructorNode.OnGetAotReflectedConstructorNode(targetConstructor);
      if (constructorNode2 != null)
      {
        constructorNode2.options = options;
        if (constructorNode2.Init(targetConstructor, parametres))
          return constructorNode2;
      }
    }
    PureReflectionConstructorNode reflectionConstructorNode = new PureReflectionConstructorNode();
    reflectionConstructorNode.options = options;
    return !reflectionConstructorNode.Init(targetConstructor, parametres) ? (BaseReflectedConstructorNode) null : (BaseReflectedConstructorNode) reflectionConstructorNode;
  }

  public bool Init(ConstructorInfo constructor, ParametresDef parametres)
  {
    if (ConstructorInfo.op_Equality(constructor, (ConstructorInfo) null) || constructor.ContainsGenericParameters || constructor.IsGenericMethodDefinition)
      return false;
    this.paramDefinitions = parametres.paramDefinitions == null ? new List<ParamDef>() : parametres.paramDefinitions;
    this.instanceDef = parametres.instanceDef;
    this.resultDef = parametres.resultDef;
    this.constructorInfo = constructor;
    return this.InitInternal(constructor);
  }

  public abstract bool InitInternal(ConstructorInfo constructor);

  public abstract void RegisterPorts(FlowNode node, ReflectedMethodRegistrationOptions options);
}
