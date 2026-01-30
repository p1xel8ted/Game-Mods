// Decompiled with JetBrains decompiler
// Type: PlayerAmbientMusicController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PlayerAmbientMusicController : BaseMonoBehaviour
{
  public Health health;
  public bool isPlaying;
  public float Timer;

  public void Update()
  {
    if (!AmbientMusicController.AmbientIsPlaying)
      return;
    if (!this.isPlaying && this.health.attackers.Count > 0)
    {
      this.isPlaying = true;
      AmbientMusicController.PlayAmbientCombat();
      AudioManager.Instance.SetMusicCombatState();
      this.Timer = 0.0f;
    }
    if (!this.isPlaying || this.health.attackers.Count > 0)
      return;
    foreach (Health health in Health.team2)
    {
      if ((Object) health != (Object) null && health.team != this.health.team && !health.InanimateObject && (double) Vector3.Distance(this.transform.position, health.transform.position) < 6.0)
        return;
    }
    if ((double) (this.Timer += Time.deltaTime) < 1.0)
      return;
    this.isPlaying = false;
    AmbientMusicController.StopAmbientCombat();
    AudioManager.Instance.SetMusicCombatState(false);
  }
}
