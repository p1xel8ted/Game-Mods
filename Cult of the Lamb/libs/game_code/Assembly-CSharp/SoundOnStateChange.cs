// Decompiled with JetBrains decompiler
// Type: SoundOnStateChange
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SoundOnStateChange : BaseMonoBehaviour
{
  public List<SoundOnStateData> Sounds = new List<SoundOnStateData>();
  public List<SoundOnStateData> OnHitSounds = new List<SoundOnStateData>();
  public List<SoundOnStateData> OnDieSounds = new List<SoundOnStateData>();
  public StateMachine.State cs;
  public StateMachine _state;
  public Health health;

  public StateMachine.State CurrentState
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

  public StateMachine state
  {
    get
    {
      if ((Object) this._state == (Object) null)
        this._state = this.GetComponent<StateMachine>();
      return this._state;
    }
  }

  public void OnEnable()
  {
    this.health = this.GetComponent<Health>();
    this.health.OnHit += new Health.HitAction(this.OnHit);
    this.health.OnDie += new Health.DieAction(this.OnDie);
  }

  public void OnDisable()
  {
    this.DisableLoops();
    this.health.OnHit -= new Health.HitAction(this.OnHit);
    this.health.OnDie -= new Health.DieAction(this.OnDie);
  }

  public void DisableLoops()
  {
    foreach (SoundOnStateData sound in this.Sounds)
    {
      if (sound.position == SoundOnStateData.Position.Loop)
        AudioManager.Instance.StopLoop(sound.LoopedSound);
    }
  }

  public void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    if (this.OnHitSounds.Count <= 0)
      return;
    AudioManager.Instance.PlayOneShot(this.OnHitSounds[Random.Range(0, this.OnHitSounds.Count)].AudioSourcePath);
  }

  public void OnDie(
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

  public void Update()
  {
    if (!((Object) this.state != (Object) null))
      return;
    this.CurrentState = this.state.CURRENT_STATE;
  }
}
