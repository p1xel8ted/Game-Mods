// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.GeneratedKeyAttribute
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace FlowCanvas.Nodes;

[AttributeUsage(AttributeTargets.Class)]
public class GeneratedKeyAttribute : Attribute
{
  public string memberString;

  public GeneratedKeyAttribute(string memberName) => this.memberString = memberName;

  public string MemberName => this.memberString;
}
