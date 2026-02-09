// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.Action_Idle
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("Player")]
[Name("Idle", 0)]
public class Action_Idle : WGOBehaviourAction
{
  public override void OnExecute()
  {
    if (this.self_ch.movement_state == MovementComponent.MovementState.Following)
      this.self_ch.StopTargetFollowing();
    try
    {
      this.self_wgo.components.character.idle.StartIdle();
    }
    catch (Exception ex)
    {
      Debug.LogError((object) $"Exception at Action_Idle, WGO \"{this.self_wgo.name}\": {ex?.ToString()}", (UnityEngine.Object) this.self_wgo);
      Debug.LogError((object) ex.StackTrace);
    }
  }

  public override void OnStop()
  {
    if ((UnityEngine.Object) this.self_wgo == (UnityEngine.Object) null || string.IsNullOrEmpty(this.self_wgo.obj_id) || this.self_wgo.obj_id == "0")
      return;
    this.self_wgo.components.character.idle.StopIdle();
  }
}
