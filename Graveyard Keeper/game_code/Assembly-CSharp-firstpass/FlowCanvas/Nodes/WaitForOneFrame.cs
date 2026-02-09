// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.WaitForOneFrame
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using System.Collections;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Time")]
[Description("Wait for one (the next) frame")]
public class WaitForOneFrame : LatentActionNode
{
  public override bool exposeRoutineControls => false;

  public override IEnumerator Invoke()
  {
    yield return (object) null;
  }
}
