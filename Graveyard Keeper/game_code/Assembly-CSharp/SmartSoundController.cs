// Decompiled with JetBrains decompiler
// Type: SmartSoundController
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DarkTonic.MasterAudio;
using UnityEngine;

#nullable disable
public class SmartSoundController : MonoBehaviour
{
  public string sound;
  public float min_delay;
  public float max_delay;
  public bool dont_stop_sound_on_disable;
  [Range(0.0f, 10f)]
  public float custom_distance;
  public Vector2 _pos;
  public float last_played_time;
  public float result_delay;
  public PlaySoundResult _ps;

  public void Start() => this.RandomDelay();

  public void OnEnable()
  {
    this._pos = (Vector2) this.transform.position;
    this.Update();
  }

  public void OnDisable()
  {
    if (this._ps == null || this.dont_stop_sound_on_disable)
      return;
    this._ps.ActingVariation.Stop();
    this._ps = (PlaySoundResult) null;
  }

  public void Update()
  {
    if ((double) Time.time > (double) this.last_played_time + (double) this.result_delay)
    {
      this._ps = Sounds.PlaySound(this.sound, new Vector2?(this._pos), custom_distance: this.custom_distance);
      this.last_played_time = Time.time;
      this.RandomDelay();
    }
    if (this._ps == null)
      return;
    this._ps.ActingVariation.AdjustVolume(Sounds.CalcSoundVolume(new Vector2?(this._pos), this.custom_distance));
  }

  public void RandomDelay() => this.result_delay = Random.Range(this.min_delay, this.max_delay);
}
