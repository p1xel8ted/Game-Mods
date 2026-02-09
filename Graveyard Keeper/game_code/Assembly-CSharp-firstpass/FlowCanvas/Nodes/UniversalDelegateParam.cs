// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.UniversalDelegateParam
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Reflection;

#nullable disable
namespace FlowCanvas.Nodes;

public abstract class UniversalDelegateParam
{
  public ParamDef paramDef;
  public bool paramsArrayNeeded;
  public int paramsArrayCount;
  public UniversalDelegate referencedDelegate;
  public UniversalDelegateParam[] referencedParams;

  public abstract Type GetCurrentType();

  public abstract void RegisterAsInput(FlowNode node);

  public abstract void RegisterAsOutput(FlowNode node);

  public abstract void RegisterAsOutput(FlowNode node, Action beforeReturn);

  public abstract void RegisterAsOutput(FlowNode node, Action<UniversalDelegateParam> beforeReturn);

  public abstract void SetFromInput();

  public abstract void SetFromValue(object value);

  public abstract FieldInfo ValueField { get; }
}
