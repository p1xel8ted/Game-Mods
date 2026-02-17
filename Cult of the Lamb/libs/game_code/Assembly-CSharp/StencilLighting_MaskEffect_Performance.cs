// Decompiled with JetBrains decompiler
// Type: StencilLighting_MaskEffect_Performance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class StencilLighting_MaskEffect_Performance : MonoBehaviour
{
  public StencilLighting_MaskEffect _maskEffect;
  public Dictionary<int, IStencilLighting> allDecalObjects = new Dictionary<int, IStencilLighting>();
  public Dictionary<int, Dictionary<int, StencilLighting_MaskEffect_Performance.Decal>> Decals = new Dictionary<int, Dictionary<int, StencilLighting_MaskEffect_Performance.Decal>>();

  public void OnPerformanceModeChanged(bool value) => this.enabled = value;

  public void Awake()
  {
    this._maskEffect = StencilLighting_MaskEffect.Instance;
    PerformanceModeManager.OnPerformanceModeChanged += new Action<bool>(this.OnPerformanceModeChanged);
    this.enabled = PerformanceModeManager.IsPerformanceMode();
  }

  public void OnDestroy()
  {
    PerformanceModeManager.OnPerformanceModeChanged -= new Action<bool>(this.OnPerformanceModeChanged);
  }

  public void Add(IStencilLighting go)
  {
    int gameObjectId = go.GetGameObjectID();
    if (this.allDecalObjects.ContainsKey(gameObjectId))
      return;
    this.allDecalObjects.Add(gameObjectId, go);
    this.AddDecal(go);
  }

  public void Remove(IStencilLighting go)
  {
    int gameObjectId = go.GetGameObjectID();
    if (!this.allDecalObjects.ContainsKey(gameObjectId))
      return;
    this.allDecalObjects.Remove(gameObjectId);
    this.RemoveDecal(go, gameObjectId);
  }

  public void AddDecal(IStencilLighting go)
  {
    StencilLighting_MaskEffect_Performance.Decal decal = new StencilLighting_MaskEffect_Performance.Decal()
    {
      id = go.GetGameObjectID(),
      mesh = go.GetMeshFilter().sharedMesh,
      material = go.GetSharedMaterial(),
      transform = go.GetTransform()
    };
    if (!(bool) (UnityEngine.Object) decal.mesh || !(bool) (UnityEngine.Object) decal.material || !(bool) (UnityEngine.Object) decal.transform)
      return;
    int instanceId = decal.material.GetInstanceID();
    Dictionary<int, StencilLighting_MaskEffect_Performance.Decal> dictionary;
    if (this.Decals.TryGetValue(instanceId, out dictionary))
    {
      if (dictionary.ContainsKey(decal.id))
        return;
      dictionary.Add(decal.id, decal);
    }
    else
      this.Decals.Add(instanceId, new Dictionary<int, StencilLighting_MaskEffect_Performance.Decal>()
      {
        {
          decal.id,
          decal
        }
      });
  }

  public void RemoveDecal(IStencilLighting go, int goID)
  {
    Dictionary<int, StencilLighting_MaskEffect_Performance.Decal> dictionary;
    if (!this.Decals.TryGetValue(go.GetMaterialID(), out dictionary))
      return;
    int key = goID;
    dictionary.Remove(key);
  }

  public void Clear()
  {
    this.allDecalObjects.Clear();
    foreach (Dictionary<int, StencilLighting_MaskEffect_Performance.Decal> dictionary in this.Decals.Values)
      dictionary.Clear();
    this.Decals.Clear();
  }

  public void Update()
  {
    this._maskEffect.UpdateRenderTexture();
    Camera main = Camera.main;
    if ((UnityEngine.Object) main == (UnityEngine.Object) null)
      return;
    RenderTexture active = RenderTexture.active;
    RenderTexture.active = StencilLighting_MaskEffect._lightingRenderTexture;
    Matrix4x4 projectionMatrix = main.projectionMatrix;
    Matrix4x4 worldToCameraMatrix = main.worldToCameraMatrix;
    GL.Clear(false, true, Color.clear);
    foreach (Dictionary<int, StencilLighting_MaskEffect_Performance.Decal> dictionary in this.Decals.Values)
    {
      bool flag1 = false;
      bool flag2 = false;
      foreach (StencilLighting_MaskEffect_Performance.Decal decal in dictionary.Values)
      {
        if (!((UnityEngine.Object) decal.transform == (UnityEngine.Object) null) && !((UnityEngine.Object) decal.material == (UnityEngine.Object) null))
        {
          if (!flag1)
          {
            flag2 = decal.material.SetPass(0);
            GL.PushMatrix();
            GL.LoadProjectionMatrix(projectionMatrix);
            GL.modelview = worldToCameraMatrix;
            flag1 = true;
          }
          if (flag2)
            Graphics.DrawMeshNow(decal.mesh, decal.transform.localToWorldMatrix);
          else
            Debug.LogWarning((object) (decal.material.name + " : material can't be rendered!"));
        }
      }
      if (flag1)
        GL.PopMatrix();
    }
    RenderTexture.active = active;
    this._maskEffect.amplifyPostEffect.MaskTexture.value = StencilLighting_MaskEffect._lightingRenderTexture;
  }

  public struct Decal
  {
    public int id;
    public Material material;
    public Mesh mesh;
    public Transform transform;
  }
}
