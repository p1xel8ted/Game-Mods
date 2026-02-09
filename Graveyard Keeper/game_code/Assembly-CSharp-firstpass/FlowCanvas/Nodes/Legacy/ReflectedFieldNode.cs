// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Legacy.ReflectedFieldNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Reflection;

#nullable disable
namespace FlowCanvas.Nodes.Legacy;

public abstract class ReflectedFieldNode
{
  public static ReflectedFieldNode Create(FieldInfo field)
  {
    return (ReflectedFieldNode) new PureReflectedFieldNode();
  }

  public abstract void RegisterPorts(
    FlowNode node,
    FieldInfo field,
    ReflectedFieldNodeWrapper.AccessMode accessMode);
}
