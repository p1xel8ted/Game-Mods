// Decompiled with JetBrains decompiler
// Type: SnowFootstepsController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMTools;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

#nullable disable
public class SnowFootstepsController : MonoBehaviour
{
  [SerializeField]
  public Camera _camera;
  [SerializeField]
  public ParticleSystem _particles;
  [SerializeField]
  public RenderTexture _snowFootstepsRT;
  public int _currentRenderTextureResolution = -1;
  public static int SnowIntensity = Shader.PropertyToID("_Snow_Intensity");
  public static int SnowRT = Shader.PropertyToID("_SnowRT");
  public static int GlobalFirstPlayerPos = Shader.PropertyToID("_GlobalFirstPlayerPos");
  public static bool FirstSnowFootstepController = false;
  public bool isFirstController;
  public float _snowIntensityDebug;
  public ParticleSystem.EmissionModule emission;
  public float localSnowIntensity;
  public static float MinSnowIntensity = 0.5f;

  public void OnEnable()
  {
    MMTransition.OnBeginTransition += new System.Action(this.ClearRenderTexture);
    if (!((UnityEngine.Object) CoopManager.Instance != (UnityEngine.Object) null))
      return;
    CoopManager.Instance.OnPlayerLeft += new System.Action(this.ClearRenderTexture);
  }

  public void OnDisable()
  {
    MMTransition.OnBeginTransition -= new System.Action(this.ClearRenderTexture);
    if (!((UnityEngine.Object) CoopManager.Instance != (UnityEngine.Object) null))
      return;
    CoopManager.Instance.OnPlayerLeft -= new System.Action(this.ClearRenderTexture);
  }

  public void Awake()
  {
    this.emission = this._particles.emission;
    this._particles.gameObject.SetActive(false);
    if (!SnowFootstepsController.FirstSnowFootstepController)
    {
      SnowFootstepsController.FirstSnowFootstepController = true;
      this.isFirstController = true;
    }
    else
    {
      this.isFirstController = false;
      this._camera.gameObject.SetActive(false);
    }
    if (!((UnityEngine.Object) GameManager.GetInstance() != (UnityEngine.Object) null))
      return;
    GameManager.GetInstance().StartCoroutine(this.WaitForPlayerPosition());
  }

  public IEnumerator WaitForPlayerPosition()
  {
    while ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null)
      yield return (object) new WaitForSeconds(0.1f);
    this._particles.gameObject.SetActive(true);
  }

  public void Update()
  {
    if (this.isFirstController)
    {
      this.UpdateRenderTextureResolution();
      Shader.SetGlobalVector(SnowFootstepsController.GlobalFirstPlayerPos, (Vector4) this.transform.position);
    }
    Scene activeScene;
    if (SettingsManager.Settings != null && (SettingsManager.Settings.Graphics.EnvironmentDetail == 0 || SettingsManager.Settings.Game.PerformanceMode))
    {
      activeScene = SceneManager.GetActiveScene();
      if (!activeScene.name.StartsWith("Dungeon"))
      {
        this.DisableObjects();
        return;
      }
    }
    this.localSnowIntensity = Shader.GetGlobalFloat(SnowFootstepsController.SnowIntensity);
    this._snowIntensityDebug = this.localSnowIntensity;
    if ((double) this.CheckSnowIntensity() > (double) SnowFootstepsController.MinSnowIntensity)
      return;
    activeScene = SceneManager.GetActiveScene();
    activeScene.name.StartsWith("Dungeon");
  }

  public float CheckSnowIntensity()
  {
    if ((double) this.localSnowIntensity > (double) SnowFootstepsController.MinSnowIntensity || SceneManager.GetActiveScene().name.StartsWith("Dungeon"))
      this.EnableObjects();
    else
      this.DisableObjects();
    return this.localSnowIntensity;
  }

  public void DisableObjects()
  {
    this.ClearRenderTexture();
    this._camera.gameObject.SetActive(false);
    this._particles.gameObject.SetActive(false);
  }

  public void EnableObjects()
  {
    if (this.isFirstController)
      this._camera.gameObject.SetActive(true);
    this._particles.gameObject.SetActive(true);
  }

  public void ClearRenderTexture()
  {
    if (!this.isFirstController || (UnityEngine.Object) this._snowFootstepsRT == (UnityEngine.Object) null)
      return;
    RenderTexture active = RenderTexture.active;
    RenderTexture.active = this._snowFootstepsRT;
    GL.Clear(true, true, Color.black);
    RenderTexture.active = active;
  }

  public void OnDestroy()
  {
    if (this.isFirstController)
      SnowFootstepsController.FirstSnowFootstepController = false;
    this.ClearRenderTexture();
    if (!((UnityEngine.Object) this._snowFootstepsRT != (UnityEngine.Object) null))
      return;
    this._snowFootstepsRT.Release();
  }

  public int GetRenderTextureResolution()
  {
    if (SettingsManager.Settings == null)
      return 1080;
    switch (SettingsManager.Settings.Graphics.GraphicsPreset)
    {
      case 0:
        return 256 /*0x0100*/;
      case 1:
        return 512 /*0x0200*/;
      case 2:
        return 1080;
      case 3:
        return 1080;
      default:
        return 1080;
    }
  }

  public void UpdateRenderTextureResolution()
  {
    if (!this.isFirstController)
      return;
    int textureResolution = this.GetRenderTextureResolution();
    if (textureResolution == 0)
    {
      if ((UnityEngine.Object) this._snowFootstepsRT != (UnityEngine.Object) null)
      {
        this._snowFootstepsRT.Release();
        this._snowFootstepsRT = (RenderTexture) null;
      }
      this._currentRenderTextureResolution = 0;
      this.DisableObjects();
    }
    else
    {
      if (textureResolution == this._currentRenderTextureResolution)
        return;
      this._currentRenderTextureResolution = textureResolution;
      if ((UnityEngine.Object) this._snowFootstepsRT != (UnityEngine.Object) null)
        this._snowFootstepsRT.Release();
      this._snowFootstepsRT = new RenderTexture(textureResolution, textureResolution, 0, RenderTextureFormat.ARGB32);
      this._snowFootstepsRT.filterMode = FilterMode.Bilinear;
      this._snowFootstepsRT.wrapMode = TextureWrapMode.Clamp;
      this._snowFootstepsRT.Create();
      Shader.SetGlobalTexture(SnowFootstepsController.SnowRT, (Texture) this._snowFootstepsRT);
      this._camera.targetTexture = this._snowFootstepsRT;
      this.ClearRenderTexture();
    }
  }
}
