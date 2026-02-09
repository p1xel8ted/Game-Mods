// Decompiled with JetBrains decompiler
// Type: FlowCanvas.FlowScriptController
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;

#nullable disable
namespace FlowCanvas;

public class FlowScriptController : GraphOwner<FlowScript>
{
  public object CallFunction(string name, params object[] args)
  {
    return this.behaviour.CallFunction(name, args);
  }
}
