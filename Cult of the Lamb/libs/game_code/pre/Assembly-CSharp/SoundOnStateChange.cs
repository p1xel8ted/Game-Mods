// Decompiled with JetBrains decompiler
// Type: SoundOnStateChange
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SoundOnStateChange : BaseMonoBehaviour
{
  public List<SoundOnStateData> Sounds = new List<SoundOnStateData>();
  public List<SoundOnStateData> OnHitSounds = new List<SoundOnStateData>();
  public List<SoundOnStateData> OnDieSounds = new List<SoundOnStateData>();
  private StateMachine.State cs;
  private StateMachine _state;
  private Health health;

  private StateMachine.State CurrentState
  {
    set
    {
      if (this.cs != value)
      {
        foreach (SoundOnStateData sound in this.Sounds)
        {
          AudioManager.Instance.StopLoop(sound.LoopedSound);
          if (sound.State == value && sound.position == SoundOnStateData.Position.Beginning || sound.State == this.cs && sound.position == SoundOnStateData.Position.End)
            AudioManager.Instance.PlayOneShot(sound.AudioSourcePath, this.transform.position);
          else if (sound.State == value && sound.position == SoundOnStateData.Position.Loop)
            sound.LoopedSound = AudioManager.Instance.CreateLoop(sound.AudioSourcePath, this.gameObject, true);
        }
      }
      this.cs = value;
    }
  }

  private StateMachine state
  {
    get
    {
      if ((Object) this._state == (Object) null)
        this._state = this.GetComponent<StateMachine>();
      return this._state;
    }
  }

  private void OnEnable()
  {
    this.health = this.GetComponent<Health>();
    this.health.OnHit += new Health.HitAction(this.OnHit);
    this.health.OnDie += new Health.DieAction(this.OnDie);
  }

  private void OnDisable()
  {
    this.DisableLoops();
    this.health.OnHit -= new Health.HitAction(this.OnHit);
    this.health.OnDie -= new Health.DieAction(this.OnDie);
  }

  private void DisableLoops()
  {
    foreach (SoundOnStateData sound in this.Sounds)
    {
      if (sound.position == SoundOnStateData.Position.Loop)
        AudioManager.Instance.StopLoop(sound.LoopedSound);
    }
  }

  private void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    if (this.OnHitSounds.Count <= 0)
      return;
    AudioManager.Instance.PlayOneShot(this.OnHitSounds[Random.Range(0, this.OnHitSounds.Count)].AudioSourcePath);
  }

  private void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    this.DisableLoops();
    if (this.OnDieSounds.Count <= 0)
      return;
    AudioManager.Instance.PlayOneShot(this.OnHitSounds[Random.Range(0, this.OnHitSounds.Count)].AudioSourcePath);
  }

  private void Update()
  {
    if (!((Object) this.state != (Object) null))
      return;
    this.CurrentState = this.state.CURRENT_STATE;
  }
}
