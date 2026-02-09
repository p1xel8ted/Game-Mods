// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.ImageEffects.GlobalFog
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

#nullable disable
namespace UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
[AddComponentMenu("Image Effects/Rendering/Global Fog")]
public class GlobalFog : PostEffectsBase
{
  [Tooltip("Apply distance-based fog?")]
  public bool distanceFog = true;
  [Tooltip("Exclude far plane pixels from distance-based fog? (Skybox or clear color)")]
  public bool excludeFarPixels = true;
  [Tooltip("Distance fog is based on radial distance from camera when checked")]
  public bool useRadialDistance;
  [Tooltip("Apply height-based fog?")]
  public bool heightFog = true;
  [Tooltip("Fog top Y coordinate")]
  public float height = 1f;
  [Range(0.001f, 10f)]
  public float heightDensity = 2f;
  [Tooltip("Push fog away from the camera by this amount")]
  public float startDistance;
  public Shader fogShader;
  public Material fogMaterial;

  public override bool CheckResources()
  {
    this.CheckSupport(true);
    this.fogMaterial = this.CheckShaderAndCreateMaterial(this.fogShader, this.fogMaterial);
    if (!this.isSupported)
      this.ReportAutoDisable();
    return this.isSupported;
  }

  [ImageEffectOpaque]
  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (!this.CheckResources() || !this.distanceFog && !this.heightFog)
    {
      Graphics.Blit((Texture) source, destination);
    }
    else
    {
      Camera component = this.GetComponent<Camera>();
      Transform transform = component.transform;
      float nearClipPlane = component.nearClipPlane;
      float farClipPlane = component.farClipPlane;
      float fieldOfView = component.fieldOfView;
      float aspect = component.aspect;
      Matrix4x4 identity = Matrix4x4.identity;
      float num1 = fieldOfView * 0.5f;
      Vector3 vector3_1 = transform.right * nearClipPlane * Mathf.Tan(num1 * ((float) Math.PI / 180f)) * aspect;
      Vector3 vector3_2 = transform.up * nearClipPlane * Mathf.Tan(num1 * ((float) Math.PI / 180f));
      Vector3 row1 = transform.forward * nearClipPlane - vector3_1 + vector3_2;
      float num2 = row1.magnitude * farClipPlane / nearClipPlane;
      row1.Normalize();
      row1 *= num2;
      Vector3 vector3_3 = transform.forward * nearClipPlane + vector3_1 + vector3_2;
      vector3_3.Normalize();
      Vector3 row2 = vector3_3 * num2;
      Vector3 vector3_4 = transform.forward * nearClipPlane + vector3_1 - vector3_2;
      vector3_4.Normalize();
      Vector3 row3 = vector3_4 * num2;
      Vector3 vector3_5 = transform.forward * nearClipPlane - vector3_1 - vector3_2;
      vector3_5.Normalize();
      Vector3 row4 = vector3_5 * num2;
      identity.SetRow(0, (Vector4) row1);
      identity.SetRow(1, (Vector4) row2);
      identity.SetRow(2, (Vector4) row3);
      identity.SetRow(3, (Vector4) row4);
      Vector3 position = transform.position;
      float y1 = position.y - this.height;
      float z = (double) y1 <= 0.0 ? 1f : 0.0f;
      float y2 = this.excludeFarPixels ? 1f : 2f;
      this.fogMaterial.SetMatrix("_FrustumCornersWS", identity);
      this.fogMaterial.SetVector("_CameraWS", (Vector4) position);
      this.fogMaterial.SetVector("_HeightParams", new Vector4(this.height, y1, z, this.heightDensity * 0.5f));
      this.fogMaterial.SetVector("_DistanceParams", new Vector4(-Mathf.Max(this.startDistance, 0.0f), y2, 0.0f, 0.0f));
      FogMode fogMode = RenderSettings.fogMode;
      float fogDensity = RenderSettings.fogDensity;
      float fogStartDistance = RenderSettings.fogStartDistance;
      float fogEndDistance = RenderSettings.fogEndDistance;
      bool flag = fogMode == FogMode.Linear;
      float f = flag ? fogEndDistance - fogStartDistance : 0.0f;
      float num3 = (double) Mathf.Abs(f) > 9.9999997473787516E-05 ? 1f / f : 0.0f;
      Vector4 vector4;
      vector4.x = fogDensity * 1.2011224f;
      vector4.y = fogDensity * 1.442695f;
      vector4.z = flag ? -num3 : 0.0f;
      vector4.w = flag ? fogEndDistance * num3 : 0.0f;
      this.fogMaterial.SetVector("_SceneFogParams", vector4);
      this.fogMaterial.SetVector("_SceneFogMode", new Vector4((float) fogMode, this.useRadialDistance ? 1f : 0.0f, 0.0f, 0.0f));
      int passNr = !this.distanceFog || !this.heightFog ? (!this.distanceFog ? 2 : 1) : 0;
      GlobalFog.CustomGraphicsBlit(source, destination, this.fogMaterial, passNr);
    }
  }

  public static void CustomGraphicsBlit(
    RenderTexture source,
    RenderTexture dest,
    Material fxMaterial,
    int passNr)
  {
    RenderTexture.active = dest;
    fxMaterial.SetTexture("_MainTex", (Texture) source);
    GL.PushMatrix();
    GL.LoadOrtho();
    fxMaterial.SetPass(passNr);
    GL.Begin(7);
    GL.MultiTexCoord2(0, 0.0f, 0.0f);
    GL.Vertex3(0.0f, 0.0f, 3f);
    GL.MultiTexCoord2(0, 1f, 0.0f);
    GL.Vertex3(1f, 0.0f, 2f);
    GL.MultiTexCoord2(0, 1f, 1f);
    GL.Vertex3(1f, 1f, 1f);
    GL.MultiTexCoord2(0, 0.0f, 1f);
    GL.Vertex3(0.0f, 1f, 0.0f);
    GL.End();
    GL.PopMatrix();
  }
}
