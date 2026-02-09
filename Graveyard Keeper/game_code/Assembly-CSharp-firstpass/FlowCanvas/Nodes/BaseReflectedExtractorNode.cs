// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.BaseReflectedExtractorNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Reflection;
using System.Runtime.CompilerServices;

#nullable disable
namespace FlowCanvas.Nodes;

public abstract class BaseReflectedExtractorNode
{
  [CompilerGenerated]
  public ParametresDef \u003CParams\u003Ek__BackingField;
  [CompilerGenerated]
  public Type \u003CTargetType\u003Ek__BackingField;

  public static event Func<Type, bool, MemberInfo[], BaseReflectedExtractorNode> OnGetAotExtractorNode;

  public static BaseReflectedExtractorNode GetExtractorNode(
    Type targetType,
    bool isStatic,
    MemberInfo[] infos)
  {
    ParametresDef parametres;
    if (!ReflectedNodesHelper.InitParams(targetType, isStatic, infos, out parametres))
      return (BaseReflectedExtractorNode) null;
    JitExtractorNode extractorNode1 = new JitExtractorNode();
    if (extractorNode1.Init(parametres, targetType))
      return (BaseReflectedExtractorNode) extractorNode1;
    if (BaseReflectedExtractorNode.OnGetAotExtractorNode != null)
    {
      BaseReflectedExtractorNode extractorNode2 = BaseReflectedExtractorNode.OnGetAotExtractorNode(targetType, isStatic, infos);
      if (extractorNode2 != null && extractorNode2.Init(parametres, targetType))
        return extractorNode2;
    }
    PureReflectedExtractorNode reflectedExtractorNode = new PureReflectedExtractorNode();
    return !reflectedExtractorNode.Init(parametres, targetType) ? (BaseReflectedExtractorNode) null : (BaseReflectedExtractorNode) reflectedExtractorNode;
  }

  public ParametresDef Params
  {
    get => this.\u003CParams\u003Ek__BackingField;
    set => this.\u003CParams\u003Ek__BackingField = value;
  }

  public Type TargetType
  {
    get => this.\u003CTargetType\u003Ek__BackingField;
    set => this.\u003CTargetType\u003Ek__BackingField = value;
  }

  public bool Init(ParametresDef paramsDef, Type targetType)
  {
    this.Params = paramsDef;
    this.TargetType = targetType;
    return this.InitInternal();
  }

  public abstract bool InitInternal();

  public abstract void RegisterPorts(FlowNode node);
}
