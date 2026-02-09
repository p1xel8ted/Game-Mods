// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_EndScript
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Color("ff0000")]
[Category("Game General")]
[Name("End Script", 0)]
[ParadoxNotion.Design.Icon("RedCross", false, "")]
public class Flow_EndScript : MyFlowNode
{
  public override void RegisterPorts()
  {
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if (!((Object) this.cfs != (Object) null))
        return;
      this.cfs.TerminateMe();
    }));
  }

  [CompilerGenerated]
  public void \u003CRegisterPorts\u003Eb__0_0(Flow f)
  {
    if (!((Object) this.cfs != (Object) null))
      return;
    this.cfs.TerminateMe();
  }
}
