// Decompiled with JetBrains decompiler
// Type: Lamb.UI.SettingsMenu.AudioSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace Lamb.UI.SettingsMenu;

public class AudioSettings : UISubmenuBase
{
  [Header("Audio Settings")]
  [SerializeField]
  public MMSlider _masterVolumeSlider;
  [SerializeField]
  public MMSlider _musicVolumeSlider;
  [SerializeField]
  public MMSlider _sfxVolumeSlider;
  [SerializeField]
  public MMSlider _voVolumeSlider;

  public void Start()
  {
    if (SettingsManager.Settings == null)
      return;
    this.Configure(SettingsManager.Settings.Audio);
    this._masterVolumeSlider.onValueChanged.AddListener(new UnityAction<float>(this.OnMasterVolumeChanged));
    this._sfxVolumeSlider.onValueChanged.AddListener(new UnityAction<float>(this.OnSFXVolumeChanged));
    this._musicVolumeSlider.onValueChanged.AddListener(new UnityAction<float>(this.OnMusicVolumeChanged));
    this._voVolumeSlider.onValueChanged.AddListener(new UnityAction<float>(this.OnVOVolumeChanged));
  }

  public void Configure(SettingsData.AudioSettings audioSettings)
  {
    this._masterVolumeSlider.value = audioSettings.MasterVolume * 100f;
    this._sfxVolumeSlider.value = audioSettings.SFXVolume * 100f;
    this._musicVolumeSlider.value = audioSettings.MusicVolume * 100f;
    this._voVolumeSlider.value = audioSettings.VOVolume * 100f;
  }

  public void Reset() => SettingsManager.Settings.Audio = new SettingsData.AudioSettings();

  public void OnMasterVolumeChanged(float volume)
  {
    volume /= 100f;
    Debug.Log((object) $"AudioSettings - Master Volume changed to {volume}".Colour(Color.yellow));
    AudioManager.Instance.SetMasterBusVolume(volume);
    SettingsManager.Settings.Audio.MasterVolume = volume;
  }

  public void OnSFXVolumeChanged(float volume)
  {
    volume /= 100f;
    Debug.Log((object) $"AudioSettings - SFX Volume changed to {volume}".Colour(Color.yellow));
    AudioManager.Instance.SetSFXBusVolume(volume);
    SettingsManager.Settings.Audio.SFXVolume = volume;
  }

  public void OnMusicVolumeChanged(float volume)
  {
    volume /= 100f;
    Debug.Log((object) $"AudioSettings - Music Volume changed to {volume}".Colour(Color.yellow));
    AudioManager.Instance.SetMusicBusVolume(volume);
    SettingsManager.Settings.Audio.MusicVolume = volume;
  }

  public void OnVOVolumeChanged(float volume)
  {
    volume /= 100f;
    Debug.Log((object) $"AudioSettings - VO Volume changed to {volume}".Colour(Color.yellow));
    AudioManager.Instance.SetVOBusVolume(volume);
    SettingsManager.Settings.Audio.VOVolume = volume;
  }
}
