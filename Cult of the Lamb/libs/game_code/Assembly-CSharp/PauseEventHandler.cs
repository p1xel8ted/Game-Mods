// Decompiled with JetBrains decompiler
// Type: PauseEventHandler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using UnityEngine;

#nullable disable
public class PauseEventHandler : MonoBehaviour
{
  public bool paused;
  public bool uiManagerPause;
  public bool simPaused;
  public bool photoModePaused;

  public event System.Action OnPause;

  public event System.Action OnUnPause;

  public bool ShouldPause
  {
    get
    {
      UIManager instance = MonoSingleton<UIManager>.Instance;
      return (instance != null ? (instance.IsPaused ? 1 : 0) : 0) != 0 || this.simPaused || this.photoModePaused;
    }
  }

  public void OnEnable()
  {
    SimulationManager.onPause += new SimulationManager.OnPause(this.OnSimulationPause);
    SimulationManager.onUnPause += new SimulationManager.OnUnPause(this.OnSimulationUnPause);
    PhotoModeManager.OnPhotoModeEnabled += new PhotoModeManager.PhotoEvent(this.OnPhotoModeEnabled);
    PhotoModeManager.OnPhotoModeDisabled += new PhotoModeManager.PhotoEvent(this.OnPhotoModeDisabled);
  }

  public void Update()
  {
    if (!((UnityEngine.Object) MonoSingleton<UIManager>.Instance != (UnityEngine.Object) null) || this.uiManagerPause == MonoSingleton<UIManager>.Instance.IsPaused)
      return;
    this.uiManagerPause = MonoSingleton<UIManager>.Instance.IsPaused;
    this.UpdatePause();
  }

  public void OnPhotoModeDisabled()
  {
    this.photoModePaused = false;
    this.UpdatePause();
  }

  public void Unpause()
  {
    if (!this.paused)
      return;
    this.paused = false;
    System.Action onUnPause = this.OnUnPause;
    if (onUnPause == null)
      return;
    onUnPause();
  }

  public void OnPhotoModeEnabled()
  {
    this.photoModePaused = true;
    this.UpdatePause();
  }

  public void Pause()
  {
    if (this.paused)
      return;
    this.paused = true;
    System.Action onPause = this.OnPause;
    if (onPause == null)
      return;
    onPause();
  }

  public void OnSimulationUnPause()
  {
    this.simPaused = false;
    this.UpdatePause();
  }

  public void OnSimulationPause()
  {
    this.simPaused = true;
    this.UpdatePause();
  }

  public void UpdatePause()
  {
    if (this.paused == this.ShouldPause)
      return;
    if (this.ShouldPause)
      this.Pause();
    else
      this.Unpause();
  }

  public void OnDisable() => this.UnbindEvents();

  public void UnbindEvents()
  {
    SimulationManager.onPause -= new SimulationManager.OnPause(this.OnSimulationPause);
    SimulationManager.onUnPause -= new SimulationManager.OnUnPause(this.OnSimulationUnPause);
    PhotoModeManager.OnPhotoModeEnabled -= new PhotoModeManager.PhotoEvent(this.OnPhotoModeEnabled);
    PhotoModeManager.OnPhotoModeDisabled -= new PhotoModeManager.PhotoEvent(this.OnPhotoModeDisabled);
  }
}
