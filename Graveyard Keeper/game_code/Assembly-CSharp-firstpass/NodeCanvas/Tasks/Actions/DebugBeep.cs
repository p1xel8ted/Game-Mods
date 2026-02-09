// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.DebugBeep
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("✫ Utility")]
[Description("Plays a 'Beep' in editor only")]
public class DebugBeep : ActionTask
{
  public override void OnExecute() => this.EndAction();
}
