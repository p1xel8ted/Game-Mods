// Decompiled with JetBrains decompiler
// Type: CameraTrackedFloorRT_Manager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using MMTools;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

#nullable disable
public class CameraTrackedFloorRT_Manager : MonoBehaviour
{
  [Header("Render Texture Settings")]
  [SerializeField]
  public RenderTexture _cameraTrackedFloorRT;
  public int _currentRenderTextureResolution = -1;
  [Header("Camera Tracking Settings")]
  [SerializeField]
  public Camera _sourceCamera;
  [SerializeField]
  public float _planeZDepthOffset;
  [SerializeField]
  public float _worldPadding = 2f;
  [SerializeField]
  public float _movementThreshold = 0.5f;
  [Header("Performance Settings")]
  [SerializeField]
  public bool _enableInDungeons = true;
  [SerializeField]
  public float _minSnowIntensity = 0.5f;
  [Header("Debug")]
  public Vector3 frustumCenter = Vector3.zero;
  public Vector2 frustumSize = Vector2.zero;
  public float currentEffectIntensity;
  public bool isActive;
  public static int EffectIntensity = Shader.PropertyToID("_Snow_Intensity");
  public static int CameraTrackedFloorRT = Shader.PropertyToID("_CameraTrackedFloorRT");
  public static int GlobalCenterPos = Shader.PropertyToID("_GlobalFloorRTCamPos");
  public static int GlobalCamSize = Shader.PropertyToID("_GlobalFloorRTCamSize");
  [CompilerGenerated]
  public static CameraTrackedFloorRT_Manager \u003CInstance\u003Ek__BackingField;
  public Camera _floorCamera;
  public Vector3 _lastCameraPosition;
  public Quaternion _lastCameraRotation;
  public float _lastCameraFOV;
  public float _lastCameraSize;
  public bool _wasCameraPerspective;
  public RenderTexture _runtimeRenderTexture;

  public float MinSnowIntensity => this._minSnowIntensity;

  public static CameraTrackedFloorRT_Manager Instance
  {
    get => CameraTrackedFloorRT_Manager.\u003CInstance\u003Ek__BackingField;
    set => CameraTrackedFloorRT_Manager.\u003CInstance\u003Ek__BackingField = value;
  }

  public void Awake()
  {
    if ((UnityEngine.Object) CameraTrackedFloorRT_Manager.Instance == (UnityEngine.Object) null)
    {
      CameraTrackedFloorRT_Manager.Instance = this;
      this._floorCamera = this.GetComponent<Camera>();
      if ((UnityEngine.Object) this._floorCamera == (UnityEngine.Object) null)
        Debug.LogWarning((object) "CameraTrackedFloorRT_Manager: Camera component not found on same GameObject");
      if (!((UnityEngine.Object) this._sourceCamera == (UnityEngine.Object) null))
        return;
      Debug.LogWarning((object) "CameraTrackedFloorRT_Manager: Source camera reference not assigned");
    }
    else
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public void OnEnable()
  {
    MMTransition.OnBeginTransition += new System.Action(this.ClearRenderTexture);
    if ((UnityEngine.Object) CoopManager.Instance != (UnityEngine.Object) null)
      CoopManager.Instance.OnPlayerLeft += new System.Action(this.ClearRenderTexture);
    GraphicsSettingsUtilities.OnEnvironmentSettingsChanged += new System.Action(this.UpdateRenderTextureResolution);
  }

  public void OnDisable()
  {
    MMTransition.OnBeginTransition -= new System.Action(this.ClearRenderTexture);
    if (!((UnityEngine.Object) CoopManager.Instance != (UnityEngine.Object) null))
      return;
    CoopManager.Instance.OnPlayerLeft -= new System.Action(this.ClearRenderTexture);
  }

  public void Start()
  {
    if ((UnityEngine.Object) this._floorCamera != (UnityEngine.Object) null)
      this._floorCamera.enabled = false;
    if ((UnityEngine.Object) this._sourceCamera != (UnityEngine.Object) null)
    {
      this._lastCameraPosition = this._sourceCamera.transform.position;
      this._lastCameraRotation = this._sourceCamera.transform.rotation;
      this._lastCameraFOV = this._sourceCamera.fieldOfView;
      this._lastCameraSize = this._sourceCamera.orthographicSize;
      this._wasCameraPerspective = !this._sourceCamera.orthographic;
    }
    this.UpdateRenderTextureResolution();
  }

  public void Update()
  {
    if (MonoSingleton<UIManager>.Instance.IsPaused)
      return;
    if (!this.ShouldRunEffect())
    {
      this.DisableEffect();
    }
    else
    {
      this.currentEffectIntensity = Shader.GetGlobalFloat(CameraTrackedFloorRT_Manager.EffectIntensity);
      if ((double) this.currentEffectIntensity <= (double) this._minSnowIntensity && !this.IsInDungeon())
      {
        this.ClearRenderTexture();
      }
      else
      {
        if (!this.HasCameraMoved())
          return;
        this.UpdateFloorCameraFromSource();
      }
    }
  }

  public bool ShouldRunEffect()
  {
    if ((UnityEngine.Object) this._sourceCamera == (UnityEngine.Object) null || !this._sourceCamera.gameObject.activeInHierarchy)
    {
      Debug.LogWarning((object) "CameraTrackedFloorRT_Manager: Source camera is null or inactive");
      return false;
    }
    return SceneManager.GetActiveScene().name == "Woolhaven Intro" || (SettingsManager.Settings == null || SettingsManager.Settings.Graphics.EnvironmentDetail != 0 && !SettingsManager.Settings.Game.PerformanceMode || this.IsInDungeon()) && (this._enableInDungeons || !this.IsInDungeon());
  }

  public bool IsInDungeon() => SceneManager.GetActiveScene().name.StartsWith("Dungeon");

  public bool HasCameraMoved()
  {
    if ((UnityEngine.Object) this._sourceCamera == (UnityEngine.Object) null)
      return false;
    float num1 = Vector3.Distance(this._lastCameraPosition, this._sourceCamera.transform.position);
    double num2 = (double) Quaternion.Angle(this._lastCameraRotation, this._sourceCamera.transform.rotation);
    bool flag1 = (double) num1 > (double) this._movementThreshold;
    bool flag2 = num2 > 1.0;
    bool flag3 = this._wasCameraPerspective != !this._sourceCamera.orthographic;
    bool flag4 = !this._sourceCamera.orthographic ? (double) Mathf.Abs(this._lastCameraFOV - this._sourceCamera.fieldOfView) > 1.0 : (double) Mathf.Abs(this._lastCameraSize - this._sourceCamera.orthographicSize) > 0.10000000149011612;
    return flag1 | flag2 | flag4 | flag3;
  }

  public void UpdateFloorCameraFromSource()
  {
    if ((UnityEngine.Object) this._sourceCamera == (UnityEngine.Object) null || (UnityEngine.Object) this._floorCamera == (UnityEngine.Object) null)
      return;
    this._lastCameraPosition = this._sourceCamera.transform.position;
    this._lastCameraRotation = this._sourceCamera.transform.rotation;
    this._lastCameraFOV = this._sourceCamera.fieldOfView;
    this._lastCameraSize = this._sourceCamera.orthographicSize;
    this._wasCameraPerspective = !this._sourceCamera.orthographic;
    Vector3 center;
    Vector2 size;
    if (!this.CalculateFrustumIntersection(out center, out size))
      return;
    this.frustumCenter = center;
    this.frustumSize = size;
    float num = Mathf.Max(size.x, size.y) * 0.5f + this._worldPadding;
    this._floorCamera.transform.position = new Vector3(center.x, center.y, this._floorCamera.transform.position.z);
    this._floorCamera.orthographicSize = num;
    Shader.SetGlobalVector(CameraTrackedFloorRT_Manager.GlobalCenterPos, (Vector4) center);
    Shader.SetGlobalFloat(CameraTrackedFloorRT_Manager.GlobalCamSize, num);
    this.isActive = true;
  }

  public bool CalculateFrustumIntersection(out Vector3 center, out Vector2 size)
  {
    center = Vector3.zero;
    size = Vector2.zero;
    if ((UnityEngine.Object) this._sourceCamera == (UnityEngine.Object) null)
      return false;
    float planeZdepthOffset = this._planeZDepthOffset;
    if (this._sourceCamera.orthographic)
    {
      float orthographicSize = this._sourceCamera.orthographicSize;
      float aspect = this._sourceCamera.aspect;
      center = new Vector3(this._sourceCamera.transform.position.x, this._sourceCamera.transform.position.y, planeZdepthOffset);
      size = new Vector2(orthographicSize * 2f * aspect, orthographicSize * 2f);
      return true;
    }
    Vector3 position = this._sourceCamera.transform.position;
    Vector3 forward = this._sourceCamera.transform.forward;
    if ((double) Mathf.Abs(forward.z) < 1.0 / 1000.0)
      return false;
    float num1 = (planeZdepthOffset - position.z) / forward.z;
    if ((double) num1 <= 0.0)
      return false;
    Vector3 vector3 = position + forward * num1;
    float num2 = Mathf.Tan((float) ((double) this._sourceCamera.fieldOfView * 0.5 * (Math.PI / 180.0))) * num1;
    float num3 = num2 * this._sourceCamera.aspect;
    center = new Vector3(vector3.x, vector3.y, planeZdepthOffset);
    size = new Vector2(num3 * 2f, num2 * 2f);
    return true;
  }

  public void EnableEffect()
  {
    if (!((UnityEngine.Object) this._floorCamera != (UnityEngine.Object) null))
      return;
    this._floorCamera.enabled = true;
  }

  public void DisableEffect()
  {
    this.ClearRenderTexture();
    if ((UnityEngine.Object) this._floorCamera != (UnityEngine.Object) null)
      this._floorCamera.enabled = false;
    this.isActive = false;
  }

  public void ClearRenderTexture()
  {
    if ((UnityEngine.Object) this._cameraTrackedFloorRT == (UnityEngine.Object) null)
      return;
    RenderTexture active = RenderTexture.active;
    RenderTexture.active = this._cameraTrackedFloorRT;
    GL.Clear(true, true, Color.black);
    RenderTexture.active = active;
  }

  public void ClearRuntimeRenderTexture()
  {
    if ((UnityEngine.Object) this._runtimeRenderTexture == (UnityEngine.Object) null)
      return;
    RenderTexture active = RenderTexture.active;
    RenderTexture.active = this._runtimeRenderTexture;
    GL.Clear(true, true, Color.black);
    RenderTexture.active = active;
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
        return 1024 /*0x0400*/;
      case 3:
        return 1024 /*0x0400*/;
      default:
        return 1024 /*0x0400*/;
    }
  }

  public void UpdateRenderTextureResolution()
  {
    if ((UnityEngine.Object) this._cameraTrackedFloorRT == (UnityEngine.Object) null)
    {
      Debug.LogWarning((object) "CameraTrackedFloorRT_Manager: Render texture asset not assigned!");
      this.DisableEffect();
    }
    else
    {
      int textureResolution = this.GetRenderTextureResolution();
      if (textureResolution == 0)
      {
        this.DisableEffect();
      }
      else
      {
        if (textureResolution == this._currentRenderTextureResolution)
          return;
        this._currentRenderTextureResolution = textureResolution;
        this._cameraTrackedFloorRT.Release();
        this._cameraTrackedFloorRT.width = textureResolution;
        this._cameraTrackedFloorRT.height = textureResolution;
        this._cameraTrackedFloorRT.Create();
        Shader.SetGlobalTexture(CameraTrackedFloorRT_Manager.CameraTrackedFloorRT, (Texture) this._cameraTrackedFloorRT);
        if ((UnityEngine.Object) this._floorCamera != (UnityEngine.Object) null)
          this._floorCamera.targetTexture = this._cameraTrackedFloorRT;
        this.ClearRenderTexture();
        this.EnableEffect();
      }
    }
  }

  public void OnDestroy()
  {
    this.ClearRenderTexture();
    if ((UnityEngine.Object) this._cameraTrackedFloorRT != (UnityEngine.Object) null)
      this._cameraTrackedFloorRT.Release();
    if (!((UnityEngine.Object) CameraTrackedFloorRT_Manager.Instance == (UnityEngine.Object) this))
      return;
    CameraTrackedFloorRT_Manager.Instance = (CameraTrackedFloorRT_Manager) null;
  }

  public void OnDrawGizmosSelected()
  {
    if (!Application.isPlaying || !this.isActive)
      return;
    Gizmos.color = Color.green;
    Gizmos.DrawWireCube(new Vector3(this.frustumCenter.x, this.frustumCenter.y, this._planeZDepthOffset), new Vector3(this.frustumSize.x, this.frustumSize.y, 0.1f));
    Gizmos.color = Color.yellow;
    float num = Mathf.Max(this.frustumSize.x, this.frustumSize.y) * 0.5f + this._worldPadding;
    Gizmos.DrawWireCube(new Vector3(this.frustumCenter.x, this.frustumCenter.y, this._planeZDepthOffset), new Vector3(num * 2f, num * 2f, 0.1f));
  }
}
