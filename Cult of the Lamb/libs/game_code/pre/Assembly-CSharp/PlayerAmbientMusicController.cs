// Decompiled with JetBrains decompiler
// Type: PlayerAmbientMusicController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PlayerAmbientMusicController : BaseMonoBehaviour
{
  public Health health;
  private bool isPlaying;
  private float Timer;

  private void Update()
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
