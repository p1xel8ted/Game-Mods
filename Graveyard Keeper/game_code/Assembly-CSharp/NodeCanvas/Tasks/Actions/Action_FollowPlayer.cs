// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.Action_FollowPlayer
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Name("Follow Player", 0)]
[Category("Player")]
public class Action_FollowPlayer : WGOBehaviourAction
{
  public BBParameter<float> speed = new BBParameter<float>(0.0f);
  public BBParameter<float> min_dist = new BBParameter<float>(0.5f);

  public override void OnExecute()
  {
    try
    {
      this.self_wgo.components.character.idle.StopIdle();
    }
    catch (Exception ex)
    {
      Debug.LogError((object) $"Exception at Action_FollowPlayer, WGO \"{this.self_wgo.name}\": {ex?.ToString()}", (UnityEngine.Object) this.self_wgo);
      Debug.LogError((object) ex.StackTrace);
    }
    this.self_ch.FollowTarget(this.player_wgo, this.min_dist.value, (GJCommons.VoidDelegate) (() =>
    {
      if (!this.isRunning)
        return;
      this.EndAction();
    }));
    this.self_ch.SetSpeed(this.speed.value);
  }

  [CompilerGenerated]
  public void \u003COnExecute\u003Eb__2_0()
  {
    if (!this.isRunning)
      return;
    this.EndAction();
  }
}
