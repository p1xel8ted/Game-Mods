// Decompiled with JetBrains decompiler
// Type: Lamb.UI.SettingsMenu.AudioSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace Lamb.UI.SettingsMenu;

public class AudioSettings : UISubmenuBase
{
  [Header("Audio Settings")]
  [SerializeField]
  private MMSlider _masterVolumeSlider;
  [SerializeField]
  private MMSlider _musicVolumeSlider;
  [SerializeField]
  private MMSlider _sfxVolumeSlider;
  [SerializeField]
  private MMSlider _voVolumeSlider;

  private void Start()
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

  private void OnMasterVolumeChanged(float volume)
  {
    volume /= 100f;
    Debug.Log((object) $"AudioSettings - Master Volume changed to {volume}".Colour(Color.yellow));
    AudioManager.Instance.SetMasterBusVolume(volume);
    SettingsManager.Settings.Audio.MasterVolume = volume;
  }

  private void OnSFXVolumeChanged(float volume)
  {
    volume /= 100f;
    Debug.Log((object) $"AudioSettings - SFX Volume changed to {volume}".Colour(Color.yellow));
    AudioManager.Instance.SetSFXBusVolume(volume);
    SettingsManager.Settings.Audio.SFXVolume = volume;
  }

  private void OnMusicVolumeChanged(float volume)
  {
    volume /= 100f;
    Debug.Log((object) $"AudioSettings - Music Volume changed to {volume}".Colour(Color.yellow));
    AudioManager.Instance.SetMusicBusVolume(volume);
    SettingsManager.Settings.Audio.MusicVolume = volume;
  }

  private void OnVOVolumeChanged(float volume)
  {
    volume /= 100f;
    Debug.Log((object) $"AudioSettings - VO Volume changed to {volume}".Colour(Color.yellow));
    AudioManager.Instance.SetVOBusVolume(volume);
    SettingsManager.Settings.Audio.VOVolume = volume;
  }
}
