// Decompiled with JetBrains decompiler
// Type: ReflectionCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class ReflectionCamera : MonoBehaviour
{
  [SerializeField]
  public Camera mainCamera;
  [SerializeField]
  public RenderTexture renderTexture;
  [SerializeField]
  public Vector3 planePos = Vector3.zero;
  [SerializeField]
  public Vector3 planeNormal = Vector3.up;
  [SerializeField]
  public Vector3 rotationOffset = Vector3.zero;
  public Camera reflectionCamera;
  [CompilerGenerated]
  public static ReflectionCamera \u003CInstance\u003Ek__BackingField;
  public static System.Action OnPreReflectionRender;
  public static System.Action OnPostReflectionRender;

  public static ReflectionCamera Instance
  {
    get => ReflectionCamera.\u003CInstance\u003Ek__BackingField;
    set => ReflectionCamera.\u003CInstance\u003Ek__BackingField = value;
  }

  public Camera ReflectionCameraInstance => this.reflectionCamera;

  public void Awake()
  {
    ReflectionCamera.Instance = this;
    this.reflectionCamera = this.GetComponent<Camera>();
    this.reflectionCamera.enabled = false;
  }

  public void OnDisable() => this.renderTexture.Release();

  public void Update()
  {
    this.reflectionCamera.ResetProjectionMatrix();
    this.reflectionCamera.ResetWorldToCameraMatrix();
    int graphicsPreset = SettingsManager.Settings.Graphics.GraphicsPreset;
    MonoBehaviour.print((object) $"Current Quality Level: {graphicsPreset}");
    if (graphicsPreset <= 1)
    {
      this.renderTexture.Release();
    }
    else
    {
      Vector3 position = this.mainCamera.transform.position;
      float num = Vector3.Dot(this.planeNormal, position - Vector3.zero);
      Vector3 vector3 = position - 2f * num * this.planeNormal + this.planePos;
      Vector3 forward1 = this.mainCamera.transform.forward;
      Vector3 up = this.mainCamera.transform.up;
      Vector3 planeNormal = this.planeNormal;
      Vector3 forward2 = Vector3.Reflect(forward1, planeNormal);
      Vector3 upwards = Vector3.Reflect(up, this.planeNormal);
      this.reflectionCamera.transform.position = vector3;
      this.reflectionCamera.transform.rotation = Quaternion.LookRotation(forward2, upwards);
      this.reflectionCamera.transform.rotation *= Quaternion.Euler(this.rotationOffset);
      this.reflectionCamera.orthographic = this.mainCamera.orthographic;
      this.reflectionCamera.orthographicSize = this.mainCamera.orthographicSize;
      this.reflectionCamera.fieldOfView = this.mainCamera.fieldOfView;
      Matrix4x4 projectionMatrix = this.mainCamera.projectionMatrix;
      projectionMatrix[0, 0] *= -1f;
      this.reflectionCamera.projectionMatrix = projectionMatrix;
      this.reflectionCamera.targetTexture = this.renderTexture;
      System.Action reflectionRender1 = ReflectionCamera.OnPreReflectionRender;
      if (reflectionRender1 != null)
        reflectionRender1();
      this.reflectionCamera.Render();
      System.Action reflectionRender2 = ReflectionCamera.OnPostReflectionRender;
      if (reflectionRender2 == null)
        return;
      reflectionRender2();
    }
  }
}
