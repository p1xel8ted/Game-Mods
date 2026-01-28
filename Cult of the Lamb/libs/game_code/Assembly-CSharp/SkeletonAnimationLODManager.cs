// Decompiled with JetBrains decompiler
// Type: SkeletonAnimationLODManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using UnityEngine;

#nullable disable
public class SkeletonAnimationLODManager : MonoBehaviour
{
  public const bool DISABLE_LOD_MANAGER = false;
  [SerializeField]
  public SkeletonAnimation skeletonAnimation;
  public ManualSkeletonUpdater skeletonUpdater;
  public SimpleSpineAnimator simpleSpineAnimator;
  public MeshRenderer meshRenderer;
  public bool doUpdate = true;
  public float randomSprayBetweemCheck;
  public bool isInitialized;
  public float timer;
  public int FrameIntervalOffset;
  public bool IgnoreCulling;
  public bool forceMainThread;
  public bool MinimumQuality;
  public ManualSkeletonUpdater.AnimationQuality currentQuality;

  public SkeletonAnimation SkeletonAnimation => this.skeletonAnimation;

  public void SetSkeletonAnimation(SkeletonAnimation anim) => this.skeletonAnimation = anim;

  public bool DoUpdate
  {
    get => this.doUpdate;
    set
    {
      if (!((Object) this.skeletonAnimation != (Object) null))
        return;
      this.skeletonAnimation.enabled = value;
    }
  }

  public void Start()
  {
    if (this.isInitialized || (Object) this.skeletonAnimation == (Object) null)
      return;
    if (this.skeletonUpdater == null)
      this.skeletonUpdater = new ManualSkeletonUpdater(this.skeletonAnimation);
    this.skeletonAnimation.enabled = false;
    this.randomSprayBetweemCheck = Random.Range(0.0f, 0.0f);
    this.skeletonUpdater.ForceFirstUpdate();
    this.skeletonUpdater.Visible = false;
    this.OnInvisible();
  }

  public void OnDestroy()
  {
    if (!((Object) SkeletonAnimationLODGlobalManager.Instance != (Object) null))
      return;
    SkeletonAnimationLODGlobalManager.Instance.RemoveLOD(this.skeletonAnimation.transform);
  }

  public void Initialise(
    SkeletonAnimation skeletonAnimation,
    ManualSkeletonUpdater.AnimationQuality quality = ManualSkeletonUpdater.AnimationQuality.Normal)
  {
    this.SetSkeletonAnimation(skeletonAnimation);
    this.ManualEnable();
    if (this.skeletonUpdater == null)
      return;
    this.skeletonUpdater.CanChangeAnimationQuality = true;
    this.skeletonUpdater.ChangeAnimationQuality(quality);
    this.OnVisibilityChange(true);
  }

  public void ManualEnable()
  {
    this.Start();
    this.OnEnable();
    this.isInitialized = true;
  }

  public void DisableLODManager(bool state)
  {
    this.doUpdate = !state;
    this.skeletonAnimation.enabled = state;
  }

  public void OnEnable()
  {
    if (this.isInitialized || !((Object) this.simpleSpineAnimator == (Object) null) || !((Object) this.skeletonAnimation != (Object) null))
      return;
    this.simpleSpineAnimator = this.skeletonAnimation.gameObject.GetComponent<SimpleSpineAnimator>();
    if (this.skeletonUpdater == null)
      this.skeletonUpdater = new ManualSkeletonUpdater(this.skeletonAnimation);
    if (!((Object) this.meshRenderer == (Object) null) || !((Object) this.skeletonAnimation != (Object) null))
      return;
    this.meshRenderer = this.skeletonAnimation.GetComponent<MeshRenderer>();
  }

  public void ChangeAnimationQualityIfNeeded(ManualSkeletonUpdater.AnimationQuality newQuality)
  {
    if (this.IgnoreCulling || this.currentQuality == newQuality)
      return;
    this.currentQuality = newQuality;
    if (this.skeletonUpdater != null)
    {
      if (this.MinimumQuality)
        return;
      switch (newQuality)
      {
        case ManualSkeletonUpdater.AnimationQuality.None:
          this.skeletonUpdater.QualityToOff();
          break;
        case ManualSkeletonUpdater.AnimationQuality.High:
          this.skeletonUpdater.QualityToHigh();
          break;
        case ManualSkeletonUpdater.AnimationQuality.Normal:
          this.skeletonUpdater.QualityToNormal();
          break;
        case ManualSkeletonUpdater.AnimationQuality.Low:
          this.skeletonUpdater.QualityToMinimum();
          break;
      }
    }
    else
      Debug.LogWarning((object) "Skeleton updater is not initialized, cannot change animation quality.");
  }

  public void Update()
  {
    if ((Object) this.skeletonAnimation == (Object) null || this.skeletonUpdater == null || SkeletonAnimationLODGlobalManager.Instance.cameraFrustum == null || (Object) this.meshRenderer == (Object) null || (Object) this.skeletonAnimation.gameObject != (Object) null && !this.skeletonAnimation.gameObject.activeInHierarchy || !this.doUpdate)
      return;
    if ((double) (this.timer -= Time.deltaTime) <= 0.0)
    {
      if (this.skeletonAnimation.enabled)
        this.skeletonAnimation.enabled = false;
      this.timer = 0.0f + this.randomSprayBetweemCheck;
      if (this.IgnoreCulling || GeometryUtility.TestPlanesAABB(SkeletonAnimationLODGlobalManager.Instance.cameraFrustum, this.meshRenderer.bounds))
        this.OnVisibilityChange(true);
      else
        this.OnVisibilityChange(false);
    }
    this.skeletonUpdater.Update();
  }

  public void LateUpdate()
  {
    if (!this.doUpdate)
      return;
    this.skeletonUpdater.LateUpdate();
  }

  public void OnVisibilityChange(bool visible)
  {
    if (visible)
      this.OnVisible();
    else
      this.OnInvisible();
    this.skeletonUpdater.Visible = visible;
  }

  public void OnVisible()
  {
    if (this.skeletonUpdater.Visible)
      return;
    this.skeletonUpdater.OnScreenUpdate = true;
    this.skeletonUpdater.StopUpdates = false;
  }

  public void OnInvisible()
  {
    this.skeletonUpdater.StopUpdates = false;
    if ((Object) this.simpleSpineAnimator != (Object) null)
    {
      switch (this.simpleSpineAnimator.GetCurrentState)
      {
        case StateMachine.State.Idle:
        case StateMachine.State.Moving:
        case StateMachine.State.Worshipping:
        case StateMachine.State.Sleeping:
        case StateMachine.State.Meditate:
          this.skeletonUpdater.StopUpdates = true;
          break;
      }
    }
    else
      this.skeletonUpdater.StopUpdates = true;
    this.skeletonUpdater.OnScreenUpdate = false;
  }

  public void ReduceQualityToMinimum()
  {
    this.skeletonUpdater.QualityToMinimum();
    this.MinimumQuality = true;
  }
}
