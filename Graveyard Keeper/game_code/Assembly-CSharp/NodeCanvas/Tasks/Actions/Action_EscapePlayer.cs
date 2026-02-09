// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.Action_EscapePlayer
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Name("Escape Player", 0)]
[Category("Player")]
public class Action_EscapePlayer : WGOBehaviourAction
{
  public BBParameter<float> speed = new BBParameter<float>(0.0f);
  public BBParameter<float> range = new BBParameter<float>(1f);

  public override void OnUpdate()
  {
    if (this.self_wgo.IsInRange(this.player_wgo, this.range.value))
    {
      this.self_ch.GoToVector(-this.self_wgo.DirTo(this.player_wgo), 1f);
      this.self_ch.SetSpeed(this.speed.value);
    }
    else
      this.self_ch.StopMovement();
    this.EndAction(true);
  }
}
