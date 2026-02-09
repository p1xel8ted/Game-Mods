// Decompiled with JetBrains decompiler
// Type: WorldSound
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DarkTonic.MasterAudio;
using UnityEngine;

#nullable disable
public class WorldSound : MonoBehaviour
{
  public string sound;
  public bool dont_stop_sound_on_disable;
  public bool force_play;
  [Range(0.0f, 10f)]
  public float custom_distance;
  public PlaySoundResult _ps;
  public Vector2 _pos;

  public void OnEnable()
  {
    this._pos = (Vector2) this.transform.position;
    this._ps = Sounds.PlaySound(this.sound, new Vector2?(this._pos), this.force_play, this.custom_distance);
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
    if (this._ps == null)
      return;
    this._ps.ActingVariation.AdjustVolume(Sounds.CalcSoundVolume(new Vector2?(this._pos), this.custom_distance));
  }
}
