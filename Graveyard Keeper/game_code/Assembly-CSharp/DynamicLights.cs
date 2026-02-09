// Decompiled with JetBrains decompiler
// Type: DynamicLights
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DynamicLights : MonoBehaviour
{
  public static List<Light> _lights_ground = new List<Light>();
  public static List<DynamicSpritePreset> _dyn_lights_ground = new List<DynamicSpritePreset>();
  public static List<Light> _lights_default = new List<Light>();
  public static List<DynamicLight> _dyn_lights_default = new List<DynamicLight>();
  public static List<float> _lights_ground_k = new List<float>();
  public static List<float> _lights_default_k = new List<float>();
  public static List<DynamicLight> dyn_lights = new List<DynamicLight>();
  public static HashSet<ObjectDynamicShadow> shadows = new HashSet<ObjectDynamicShadow>();
  public static DynamicLights _me = (DynamicLights) null;
  public static float _intensity_ground = 2.3f;
  public static float _intensity_default = 2.6f;
  public const float MIN_INTENSITY = 0.2f;

  public static void Init()
  {
    DynamicLights._me = SingletonGameObjects.FindOrCreate<DynamicLights>();
  }

  public static void SearchForLightsInNewObject(GameObject go)
  {
    foreach (Light componentsInChild in go.GetComponentsInChildren<Light>(true))
    {
      GroundLight groundLight = componentsInChild.gameObject.GetComponentInParent<GroundLight>();
      if ((Object) groundLight == (Object) null)
        groundLight = componentsInChild.gameObject.transform.parent.GetComponent<GroundLight>();
      if ((Object) groundLight == (Object) null)
        groundLight = componentsInChild.gameObject.GetComponentInChildren<GroundLight>();
      if ((Object) groundLight != (Object) null)
      {
        DynamicLights._lights_ground.Add(componentsInChild);
        DynamicLights._lights_ground_k.Add(groundLight.intensity_k);
        DynamicLights._dyn_lights_ground.Add(groundLight.intensity_preset);
      }
      else
      {
        DynamicLight dynamicLight = componentsInChild.gameObject.GetComponentInParent<DynamicLight>() ?? componentsInChild.gameObject.transform.parent.GetComponent<DynamicLight>();
        if ((Object) dynamicLight == (Object) null)
          dynamicLight = componentsInChild.gameObject.GetComponentInChildren<DynamicLight>();
        if ((Object) dynamicLight == (Object) null)
        {
          Debug.LogError((object) ("Strange light, skipping. Name = " + componentsInChild.name), (Object) componentsInChild);
        }
        else
        {
          DynamicLights._lights_default.Add(componentsInChild);
          DynamicLights._dyn_lights_default.Add(dynamicLight);
          DynamicLights._lights_default_k.Add(dynamicLight.intensity_k);
        }
      }
    }
  }

  public static void SearchForLightsInDestroyedObject(GameObject go)
  {
    foreach (Light componentsInChild in go.GetComponentsInChildren<Light>(true))
    {
      int index1 = DynamicLights._lights_default.IndexOf(componentsInChild);
      if (index1 != -1)
      {
        DynamicLights._lights_default.RemoveAt(index1);
        DynamicLights._lights_default_k.RemoveAt(index1);
        DynamicLights._dyn_lights_default.RemoveAt(index1);
      }
      int index2 = DynamicLights._lights_ground.IndexOf(componentsInChild);
      if (index2 != -1)
      {
        DynamicLights._lights_ground.RemoveAt(index2);
        DynamicLights._lights_ground_k.RemoveAt(index2);
        DynamicLights._dyn_lights_ground.RemoveAt(index2);
      }
    }
  }

  public void Update()
  {
    float k = Mathf.Clamp(TimeOfDay.light_intensity_k, 0.2f, 1f);
    this.AdjustDefaultLightsIntensity(k);
    this.AdjustGroundLightsIntensity(k);
    foreach (DynamicLight dynLight in DynamicLights.dyn_lights)
      dynLight.CustomUpdate();
    foreach (ObjectDynamicShadow shadow in DynamicLights.shadows)
      shadow.CheckLightsRange(DynamicLights.dyn_lights);
  }

  public void AdjustDefaultLightsIntensity(float k)
  {
    for (int index = 0; index < DynamicLights._lights_default.Count; ++index)
    {
      Light light = DynamicLights._lights_default[index];
      if (!((Object) light == (Object) null) && !((Object) light.gameObject == (Object) null) && light.gameObject.activeInHierarchy)
      {
        DynamicLight dynamicLight = DynamicLights._dyn_lights_default[index];
        DynamicSpritePreset intensityPreset = (Object) dynamicLight == (Object) null ? (DynamicSpritePreset) null : dynamicLight.intensity_preset;
        float num = (Object) intensityPreset == (Object) null ? 1f : intensityPreset.EvaluateAlpha();
        light.intensity = DynamicLights._intensity_default * k * DynamicLights._lights_default_k[index] * num;
      }
    }
  }

  public void AdjustGroundLightsIntensity(float k)
  {
    for (int index = 0; index < DynamicLights._lights_ground.Count; ++index)
    {
      Light light = DynamicLights._lights_ground[index];
      if (!((Object) light == (Object) null) && !((Object) light.gameObject == (Object) null) && light.gameObject.activeInHierarchy)
      {
        DynamicSpritePreset dynamicSpritePreset = DynamicLights._dyn_lights_ground[index];
        float num = (Object) dynamicSpritePreset == (Object) null ? 1f : dynamicSpritePreset.EvaluateAlpha();
        light.intensity = DynamicLights._intensity_ground * k * DynamicLights._lights_ground_k[index] * num;
      }
    }
  }
}
