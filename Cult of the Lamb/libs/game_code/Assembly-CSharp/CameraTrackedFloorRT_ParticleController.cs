// Decompiled with JetBrains decompiler
// Type: CameraTrackedFloorRT_ParticleController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

#nullable disable
public class CameraTrackedFloorRT_ParticleController : MonoBehaviour
{
  [Header("Settings")]
  [SerializeField]
  public ParticleSystem _particles;
  [Header("Movement Settings")]
  [SerializeField]
  public float _movementSpeedThreshold = 0.1f;
  [Header("Debug")]
  public float currentEffectIntensity;
  public bool emissionEnabled;
  public float movementSpeed;
  public static int EffectIntensity = Shader.PropertyToID("_Snow_Intensity");
  public ParticleSystem.EmissionModule emission;
  public CameraTrackedFloorRT_Manager _manager;
  public Vector3 _lastPosition;

  public void Awake()
  {
    if ((Object) this._particles != (Object) null)
    {
      this.emission = this._particles.emission;
      this.emission.enabled = false;
    }
    this._lastPosition = this.transform.position;
  }

  public void Start()
  {
    this._manager = CameraTrackedFloorRT_Manager.Instance;
    if (!((Object) this._manager == (Object) null))
      return;
    Debug.LogWarning((object) "CameraTrackedFloorRT_ParticleController: Manager instance not found");
  }

  public void Update()
  {
    if (MonoSingleton<UIManager>.Instance.IsPaused)
      return;
    this.UpdateMovementSpeed();
    this.UpdateParticleEmission();
  }

  public void UpdateMovementSpeed()
  {
    this.movementSpeed = Vector3.Distance(this.transform.position, this._lastPosition) / Time.deltaTime;
    this._lastPosition = this.transform.position;
  }

  public void UpdateParticleEmission()
  {
    if ((Object) this._particles == (Object) null || (Object) this._manager == (Object) null)
      return;
    this.currentEffectIntensity = Shader.GetGlobalFloat(CameraTrackedFloorRT_ParticleController.EffectIntensity);
    bool flag = this.ShouldEmitParticles();
    if (this.emission.enabled == flag)
      return;
    this.emission.enabled = flag;
    this.emissionEnabled = flag;
  }

  public bool ShouldEmitParticles()
  {
    if (SettingsManager.Settings != null && (SettingsManager.Settings.Graphics.EnvironmentDetail == 0 || SettingsManager.Settings.Game.PerformanceMode) && !SceneManager.GetActiveScene().name.StartsWith("Dungeon"))
      return false;
    return SceneManager.GetActiveScene().name.StartsWith("Woolhaven") || (((double) this.currentEffectIntensity > (double) this._manager.MinSnowIntensity ? 1 : (SceneManager.GetActiveScene().name.StartsWith("Dungeon") ? 1 : 0)) & ((double) this.movementSpeed > (double) this._movementSpeedThreshold ? 1 : 0)) != 0;
  }
}
