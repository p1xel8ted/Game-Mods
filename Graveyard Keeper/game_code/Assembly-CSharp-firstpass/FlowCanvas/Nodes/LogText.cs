// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.LogText
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Log text in the console")]
[Category("Utility")]
public class LogText : CallableActionNode<string>
{
  public override void Invoke(string text) => Debug.Log((object) text);
}
