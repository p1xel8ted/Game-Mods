// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.ParamDef
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Reflection;

#nullable disable
namespace FlowCanvas.Nodes;

public struct ParamDef
{
  public Type paramType;
  public Type arrayType;
  public ParamMode paramMode;
  public string portName;
  public string portId;
  public bool isParamsArray;
  public MemberInfo presentedInfo;
}
