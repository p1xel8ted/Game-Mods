// Decompiled with JetBrains decompiler
// Type: SkeletonAnimationLODGlobalManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

#nullable disable
public class SkeletonAnimationLODGlobalManager : MonoBehaviour
{
  public const float DELAY_BETWEEN_CHECKS = 0.0f;
  public static SkeletonAnimationLODGlobalManager Instance;
  public Plane[] cameraFrustum;
  public Camera mainCamera;
  public float timer;
  public Dictionary<Transform, SkeletonAnimationLODManager> skeletons = new Dictionary<Transform, SkeletonAnimationLODManager>();
  public float closeDistanceThreshold = 4f;
  public float mediumDistanceThreshold = 8f;
  public static bool UseLODSytem;
  public Vector3 camCenterpoint;

  public void Initialize()
  {
    if ((UnityEngine.Object) SkeletonAnimationLODGlobalManager.Instance != (UnityEngine.Object) null)
      return;
    SkeletonAnimationLODGlobalManager.Instance = this;
    SkeletonAnimation.OnInitialized += new Action<Transform, SkeletonAnimation, bool>(this.AddLOD);
    SceneManager.activeSceneChanged += new UnityAction<Scene, Scene>(this.SceneManager_activeSceneChanged);
  }

  public void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
  {
    SkeletonAnimationLODGlobalManager.UseLODSytem = arg1.name == "Base Biome 1";
  }

  public void Update()
  {
    if ((double) (this.timer -= Time.deltaTime) > 0.0)
      return;
    this.timer = 0.0f;
    if ((UnityEngine.Object) this.mainCamera == (UnityEngine.Object) null || !this.mainCamera.gameObject.activeInHierarchy)
      this.mainCamera = Camera.main;
    if ((UnityEngine.Object) this.mainCamera == (UnityEngine.Object) null)
      return;
    this.cameraFrustum = GeometryUtility.CalculateFrustumPlanes(this.mainCamera);
    if (!SkeletonAnimationLODGlobalManager.UseLODSytem || (UnityEngine.Object) CameraFollowTarget.Instance == (UnityEngine.Object) null)
      return;
    float fps = DynamicResolutionManager._fps;
    float threshold = PerformanceModeManager.GetFrameRate() == 60 ? 50f : 25f;
    this.camCenterpoint = CameraFollowTarget.Instance.GetCentrePoint();
    foreach (KeyValuePair<Transform, SkeletonAnimationLODManager> skeleton in this.skeletons)
    {
      if ((UnityEngine.Object) skeleton.Value != (UnityEngine.Object) null)
      {
        SkeletonAnimationLODManager animationLodManager = skeleton.Value;
        Vector3 position = animationLodManager.transform.position;
        Renderer component = animationLodManager.GetComponent<Renderer>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null && !GeometryUtility.TestPlanesAABB(this.cameraFrustum, component.bounds))
        {
          animationLodManager.ChangeAnimationQualityIfNeeded(ManualSkeletonUpdater.AnimationQuality.None);
        }
        else
        {
          float distance = Vector3.Distance(this.camCenterpoint, position);
          ManualSkeletonUpdater.AnimationQuality animationQuality = this.DetermineAnimationQuality(fps, threshold, distance);
          animationLodManager.ChangeAnimationQualityIfNeeded(animationQuality);
        }
      }
    }
  }

  public ManualSkeletonUpdater.AnimationQuality DetermineAnimationQuality(
    float fps,
    float threshold,
    float distance)
  {
    bool flag = (double) fps < (double) threshold - 1.0;
    if ((double) distance <= (flag ? (double) this.closeDistanceThreshold / 2.0 : (double) this.closeDistanceThreshold))
      return ManualSkeletonUpdater.AnimationQuality.High;
    return (double) distance <= (flag ? (double) this.closeDistanceThreshold : (double) this.mediumDistanceThreshold) ? ManualSkeletonUpdater.AnimationQuality.Normal : ManualSkeletonUpdater.AnimationQuality.Low;
  }

  public void AddLOD(Transform parent, SkeletonAnimation spine, bool lowestQuality)
  {
    if ((UnityEngine.Object) parent == (UnityEngine.Object) null || (UnityEngine.Object) spine == (UnityEngine.Object) null || !Application.isPlaying || this.skeletons.ContainsKey(spine.transform) || parent.gameObject.TryGetComponent<SkeletonAnimationLODManager>(out SkeletonAnimationLODManager _))
      return;
    SkeletonAnimationLODManager animationLodManager = parent.gameObject.AddComponent<SkeletonAnimationLODManager>();
    if (lowestQuality)
    {
      animationLodManager.Initialise(spine, ManualSkeletonUpdater.AnimationQuality.Low);
      animationLodManager.ReduceQualityToMinimum();
    }
    else
      animationLodManager.Initialise(spine);
    this.skeletons.Add(spine.transform, animationLodManager);
  }

  public void AddMinimumQualityLOD(Transform parent, SkeletonAnimation spine)
  {
    if ((UnityEngine.Object) parent == (UnityEngine.Object) null || (UnityEngine.Object) spine == (UnityEngine.Object) null || !Application.isPlaying || this.skeletons.ContainsKey(spine.transform) || parent.gameObject.TryGetComponent<SkeletonAnimationLODManager>(out SkeletonAnimationLODManager _))
      return;
    SkeletonAnimationLODManager animationLodManager = parent.gameObject.AddComponent<SkeletonAnimationLODManager>();
    animationLodManager.Initialise(spine, ManualSkeletonUpdater.AnimationQuality.Low);
    animationLodManager.ReduceQualityToMinimum();
    spine.InitializeLOD();
    this.skeletons.Add(spine.transform, animationLodManager);
  }

  public SkeletonAnimationLODManager AddCustomLOD(
    Transform parent,
    SkeletonAnimation spine,
    ManualSkeletonUpdater.AnimationQuality quality = ManualSkeletonUpdater.AnimationQuality.Normal)
  {
    if ((UnityEngine.Object) parent == (UnityEngine.Object) null || (UnityEngine.Object) spine == (UnityEngine.Object) null)
      return (SkeletonAnimationLODManager) null;
    if (!Application.isPlaying)
      return (SkeletonAnimationLODManager) null;
    if (this.skeletons.ContainsKey(spine.transform))
      return this.skeletons[spine.transform];
    SkeletonAnimationLODManager component;
    if (!parent.gameObject.TryGetComponent<SkeletonAnimationLODManager>(out component))
    {
      component = parent.gameObject.AddComponent<SkeletonAnimationLODManager>();
      component.Initialise(spine, quality);
      spine.InitializeLOD();
      this.skeletons.Add(spine.transform, component);
    }
    return component;
  }

  public void DisableCulling(Transform spineTransform, SkeletonAnimation spine)
  {
    if ((UnityEngine.Object) spineTransform == (UnityEngine.Object) null || (UnityEngine.Object) spine == (UnityEngine.Object) null)
      return;
    SkeletonAnimationLODManager animationLodManager = this.AddCustomLOD(spineTransform, spine);
    if ((UnityEngine.Object) animationLodManager == (UnityEngine.Object) null)
      animationLodManager = spineTransform.gameObject.GetComponent<SkeletonAnimationLODManager>();
    if ((UnityEngine.Object) animationLodManager != (UnityEngine.Object) null)
      animationLodManager.IgnoreCulling = true;
    else
      Debug.LogWarning((object) ("No LOD was found! " + ((object) spine.gameObject)?.ToString()));
  }

  public void RemoveLOD(Transform parent)
  {
    if (!this.skeletons.ContainsKey(parent))
      return;
    this.skeletons.Remove(parent);
  }

  public void OnDrawGizmos()
  {
    if ((UnityEngine.Object) CameraFollowTarget.Instance == (UnityEngine.Object) null)
      return;
    Gizmos.color = Color.green;
    Gizmos.DrawWireSphere(this.camCenterpoint, this.closeDistanceThreshold);
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(this.camCenterpoint, this.mediumDistanceThreshold);
  }
}
