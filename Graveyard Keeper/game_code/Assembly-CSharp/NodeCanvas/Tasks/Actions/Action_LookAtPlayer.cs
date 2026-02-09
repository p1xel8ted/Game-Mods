// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.Action_LookAtPlayer
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("Player")]
[Name("Look At Player", 0)]
public class Action_LookAtPlayer : WGOBehaviourAction
{
  public override void OnExecute()
  {
    this.self_ch.LookAt(this.player_wgo);
    this.EndAction(true);
  }
}
