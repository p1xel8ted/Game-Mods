// Decompiled with JetBrains decompiler
// Type: CameraWeatherVFX
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CameraWeatherVFX : MonoBehaviour
{
  [Header("Follow Settings")]
  [SerializeField]
  public bool enableFollowEffect = true;
  [Tooltip("Will auto-populate this if unset")]
  [SerializeField]
  public CameraWeatherVFX_Follower followerTarget;
  [Header("Weather VFX Slots")]
  [SerializeField]
  public List<CameraWeatherVFX.WeatherVFXSlot> weatherSlots = new List<CameraWeatherVFX.WeatherVFXSlot>();

  public void Start()
  {
    this.InitializeSlots();
    if (!((UnityEngine.Object) this.followerTarget == (UnityEngine.Object) null))
      return;
    this.followerTarget = UnityEngine.Object.FindObjectOfType<CameraWeatherVFX_Follower>();
    if (!((UnityEngine.Object) this.followerTarget == (UnityEngine.Object) null) || !this.enableFollowEffect)
      return;
    Debug.LogWarning((object) $"CameraWeatherVFX: No CameraWeatherVFX_Follower found in scene for '{this.gameObject.name}'!", (UnityEngine.Object) this);
  }

  public void InitializeSlots()
  {
    for (int index = 0; index < this.weatherSlots.Count; ++index)
    {
      CameraWeatherVFX.WeatherVFXSlot weatherSlot = this.weatherSlots[index];
      if ((UnityEngine.Object) weatherSlot.particleSystem == (UnityEngine.Object) null)
      {
        Debug.LogWarning((object) $"CameraWeatherVFX: Slot {index} has no particle system assigned!", (UnityEngine.Object) this);
      }
      else
      {
        ParticleSystemRenderer component = weatherSlot.particleSystem.GetComponent<ParticleSystemRenderer>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null && (UnityEngine.Object) component.sharedMaterial != (UnityEngine.Object) null)
        {
          if ((UnityEngine.Object) weatherSlot.particleMaterialInstance == (UnityEngine.Object) null)
            weatherSlot.particleMaterialInstance = component.sharedMaterial;
          if ((UnityEngine.Object) weatherSlot.particleRenderer == (UnityEngine.Object) null)
            weatherSlot.particleRenderer = component;
          if (weatherSlot.propertyBlock == null)
          {
            weatherSlot.propertyBlock = new MaterialPropertyBlock();
            component.SetPropertyBlock(weatherSlot.propertyBlock);
          }
        }
        else
          Debug.LogWarning((object) $"CameraWeatherVFX: Slot {index} particle system '{weatherSlot.particleSystem.name}' has no material assigned!", (UnityEngine.Object) this);
      }
    }
  }

  public void Update()
  {
    if (this.enableFollowEffect && (UnityEngine.Object) this.followerTarget != (UnityEngine.Object) null)
      this.transform.position = this.followerTarget.CurrentLocation;
    this.UpdateAllSlotMaterials();
  }

  public void UpdateAllSlotMaterials()
  {
    foreach (CameraWeatherVFX.WeatherVFXSlot weatherSlot in this.weatherSlots)
    {
      if (!((UnityEngine.Object) weatherSlot.particleSystem == (UnityEngine.Object) null) && !((UnityEngine.Object) weatherSlot.particleMaterialInstance == (UnityEngine.Object) null) && weatherSlot.particleSystem.gameObject.activeInHierarchy && weatherSlot.warpParticles)
        this.UpdateSlotMaterialProperties(weatherSlot);
    }
  }

  public void UpdateSlotMaterialProperties(CameraWeatherVFX.WeatherVFXSlot slot)
  {
    Vector3 vector3_1;
    Vector3 vector3_2;
    if (slot.useFrustumScale && (UnityEngine.Object) this.followerTarget != (UnityEngine.Object) null)
    {
      Bounds frustumBounds = this.followerTarget.FrustumBounds;
      vector3_1 = frustumBounds.center;
      vector3_2 = frustumBounds.size;
      this.UpdateParticleSystemShape(slot, frustumBounds);
    }
    else
    {
      ParticleSystem.ShapeModule shape = slot.particleSystem.shape;
      vector3_1 = slot.particleSystem.transform.position + shape.position;
      vector3_2 = shape.scale;
    }
    if (!((UnityEngine.Object) slot.particleRenderer != (UnityEngine.Object) null) || !((UnityEngine.Object) slot.particleMaterialInstance != (UnityEngine.Object) null))
      return;
    slot.particleRenderer.GetPropertyBlock(slot.propertyBlock);
    slot.propertyBlock.SetVector("_WeatherVFXBoxPosition", (Vector4) vector3_1);
    slot.propertyBlock.SetVector("_WeatherVFXBoxScale", (Vector4) vector3_2);
    slot.particleRenderer.SetPropertyBlock(slot.propertyBlock);
  }

  public void AutoPopulateFromChildren()
  {
    this.weatherSlots.Clear();
    foreach (ParticleSystem componentsInChild in this.GetComponentsInChildren<ParticleSystem>())
      this.weatherSlots.Add(new CameraWeatherVFX.WeatherVFXSlot()
      {
        particleSystem = componentsInChild,
        warpParticles = true
      });
    Debug.Log((object) $"Auto-populated {this.weatherSlots.Count} weather VFX slots from child particle systems.");
  }

  public void ForceUpdateMaterialProperties() => this.ForceUpdateMaterialProperties(false);

  public void ForceUpdateMaterialProperties(bool editorMode)
  {
    this.UpdateFollowerTarget();
    this.UpdateFollowPosition();
    this.InitializeSlots();
    if (editorMode)
    {
      foreach (CameraWeatherVFX.WeatherVFXSlot weatherSlot in this.weatherSlots)
      {
        if (!((UnityEngine.Object) weatherSlot.particleSystem == (UnityEngine.Object) null) && weatherSlot.particleSystem.gameObject.activeInHierarchy && weatherSlot.warpParticles)
          this.UpdateSlotMaterialProperties(weatherSlot);
      }
    }
    else
      this.UpdateAllSlotMaterials();
    Debug.Log((object) $"Force updated material properties for {this.weatherSlots.Count} weather VFX slots (Editor mode: {editorMode}). Follower target: {((UnityEngine.Object) this.followerTarget != (UnityEngine.Object) null ? (object) this.followerTarget.name : (object) "None")}. Position: {this.transform.position}");
  }

  public void UpdateFollowerTarget()
  {
    if (!((UnityEngine.Object) this.followerTarget == (UnityEngine.Object) null) || !this.enableFollowEffect)
      return;
    this.followerTarget = UnityEngine.Object.FindObjectOfType<CameraWeatherVFX_Follower>();
    if ((UnityEngine.Object) this.followerTarget != (UnityEngine.Object) null)
      Debug.Log((object) $"CameraWeatherVFX: Found and assigned follower target '{this.followerTarget.name}'", (UnityEngine.Object) this);
    else
      Debug.LogWarning((object) $"CameraWeatherVFX: No CameraWeatherVFX_Follower found in scene for '{this.gameObject.name}'!", (UnityEngine.Object) this);
  }

  public void UpdateFollowPosition()
  {
    if (!this.enableFollowEffect || !((UnityEngine.Object) this.followerTarget != (UnityEngine.Object) null))
      return;
    Vector3 position = this.transform.position;
    this.transform.position = this.followerTarget.CurrentLocation;
  }

  public void UpdateParticleSystemShape(CameraWeatherVFX.WeatherVFXSlot slot, Bounds frustumBounds)
  {
    if ((UnityEngine.Object) slot.particleSystem == (UnityEngine.Object) null)
      return;
    ParticleSystem.ShapeModule shape = slot.particleSystem.shape with
    {
      enabled = true,
      shapeType = ParticleSystemShapeType.Box
    };
    Vector3 vector3 = slot.particleSystem.transform.InverseTransformPoint(frustumBounds.center);
    shape.position = vector3;
    shape.scale = frustumBounds.size;
  }

  [Serializable]
  public class WeatherVFXSlot
  {
    [Tooltip("The particle system for this weather effect")]
    public ParticleSystem particleSystem;
    [Tooltip("Calculate and send material values for particle warping")]
    public bool warpParticles = true;
    [Tooltip("Use camera frustum bounds for scale instead of particle system shape scale")]
    public bool useFrustumScale;
    [NonSerialized]
    public Material particleMaterialInstance;
    [NonSerialized]
    public MaterialPropertyBlock propertyBlock;
    [NonSerialized]
    public ParticleSystemRenderer particleRenderer;
  }
}
