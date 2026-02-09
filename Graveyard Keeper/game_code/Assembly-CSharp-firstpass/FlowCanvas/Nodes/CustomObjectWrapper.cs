// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.CustomObjectWrapper
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace FlowCanvas.Nodes;

public abstract class CustomObjectWrapper : FlowNode
{
  public abstract void SetTarget(UnityEngine.Object target);

  public class ExplusiveWrapperAttribute : Attribute
  {
  }
}
