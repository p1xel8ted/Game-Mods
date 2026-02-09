// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.Condition_DamageKickBack
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Conditions;

[Name("Damage Kick Back", 0)]
[Category("Player")]
public class Condition_DamageKickBack : WGOBehaviourCondition
{
  public BBParameter<float> speed = new BBParameter<float>(4f);
  public BBParameter<float> friction = new BBParameter<float>(0.9f);
  public BBParameter<float> sleep_time = new BBParameter<float>(0.0f);
  public float left_sleep_time;

  public override bool OnCheck()
  {
    KickComponent kick = this.self_wgo.components.kick;
    bool flag = this.self_ch.WasDamaged(true);
    if (kick == null || !flag && !kick.in_process && (double) this.left_sleep_time < 0.0)
      return false;
    if (flag)
    {
      if (!this.self_ch.IsStopped)
        this.self_ch.StopMovement();
      if (this.self_ch.components.character.attack.enabled && this.self_ch.components.character.attack.performing_attack)
        this.self_ch.InterruptAttack();
      kick.KickFrom(this.player_wgo.pos).SetSpeed(this.speed.value).SetFriction(this.friction.value);
      this.left_sleep_time = this.sleep_time.value;
    }
    if (!flag && !kick.in_process)
      this.left_sleep_time -= Time.deltaTime;
    return true;
  }
}
