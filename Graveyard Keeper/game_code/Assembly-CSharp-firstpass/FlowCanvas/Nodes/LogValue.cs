// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.LogValue
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Utility")]
[Description("Log input value on the console")]
public class LogValue : CallableActionNode<object>
{
  public override void Invoke(object obj) => Debug.Log(obj);
}
