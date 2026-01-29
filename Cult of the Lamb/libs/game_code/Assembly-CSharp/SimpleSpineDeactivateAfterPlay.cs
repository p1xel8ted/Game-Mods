// Decompiled with JetBrains decompiler
// Type: SimpleSpineDeactivateAfterPlay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Spine;
using Spine.Unity;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class SimpleSpineDeactivateAfterPlay : BaseMonoBehaviour
{
  public SkeletonAnimation Spine;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string Animation;
  [CompilerGenerated]
  public bool \u003CInit\u003Ek__BackingField;
  public static AsyncOperationHandle<Material> addrHandle;
  public static Material material_low;
  public static Sprite[] spikes_low;
  public static Spine.Animation spikesAnimation;
  public GameObject lowQualitySpike;
  public float timer;
  public bool useLowQualitySpikes;
  public Vector3 spikes_low_position;
  public float initialTimer;
  public float stage1Anim;
  public float stage2Anim;
  public float stage3Anim;

  public bool Init
  {
    get => this.\u003CInit\u003Ek__BackingField;
    set => this.\u003CInit\u003Ek__BackingField = value;
  }

  public void UseLowQualitySpikes(SimpleSpineDeactivateAfterPlay.SpikeType spikeType)
  {
    if (this.useLowQualitySpikes)
      return;
    switch (spikeType)
    {
      case SimpleSpineDeactivateAfterPlay.SpikeType.EnemyWormBoss:
        if (SimpleSpineDeactivateAfterPlay.spikes_low == null)
          SimpleSpineDeactivateAfterPlay.spikes_low = Resources.LoadAll<Sprite>("PerformanceMode/spikes_low");
        this.spikes_low_position = new Vector3(0.0f, 0.645f, -0.744f);
        break;
      case SimpleSpineDeactivateAfterPlay.SpikeType.EnemyJellySpikerMiniBoss:
        if (SimpleSpineDeactivateAfterPlay.spikes_low == null)
          SimpleSpineDeactivateAfterPlay.spikes_low = Resources.LoadAll<Sprite>("PerformanceMode/burrowing_spikes_low");
        this.spikes_low_position = new Vector3(0.0f, 0.242f, -0.435f);
        break;
    }
    if ((Object) SimpleSpineDeactivateAfterPlay.material_low == (Object) null)
    {
      SimpleSpineDeactivateAfterPlay.addrHandle = Addressables.LoadAssetAsync<Material>((object) "Assets/Art/Shaders/AmplifyShaderEditor/Environment/Sprites-Default.mat");
      SimpleSpineDeactivateAfterPlay.addrHandle.WaitForCompletion();
      SimpleSpineDeactivateAfterPlay.material_low = SimpleSpineDeactivateAfterPlay.addrHandle.Result;
    }
    if (SimpleSpineDeactivateAfterPlay.spikesAnimation == null)
      SimpleSpineDeactivateAfterPlay.spikesAnimation = this.Spine.Skeleton.Data.FindAnimation(this.Animation);
    GameObject gameObject = new GameObject("spine_low");
    SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
    spriteRenderer.sprite = SimpleSpineDeactivateAfterPlay.spikes_low[Random.Range(0, SimpleSpineDeactivateAfterPlay.spikes_low.Length)];
    spriteRenderer.material = SimpleSpineDeactivateAfterPlay.material_low;
    gameObject.transform.Rotate(new Vector3(-60f, 0.0f, 0.0f));
    gameObject.transform.parent = this.transform;
    this.lowQualitySpike = gameObject;
    this.lowQualitySpike.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
    this.useLowQualitySpikes = true;
    this.initialTimer = SimpleSpineDeactivateAfterPlay.spikesAnimation.Duration;
    this.Spine.gameObject.SetActive(false);
    this.Init = false;
    this.stage1Anim = this.initialTimer / 2f;
    this.stage2Anim = this.initialTimer * 0.25f;
    this.stage3Anim = (float) ((double) this.initialTimer / 2.0 + (double) this.initialTimer * 0.25);
  }

  public void Update()
  {
    this.HandleFrozenTime();
    if (!this.Init && (this.useLowQualitySpikes || this.Spine.AnimationState != null))
    {
      this.Init = true;
      if (this.useLowQualitySpikes)
      {
        this.timer = this.initialTimer;
        this.lowQualitySpike.transform.localPosition = this.spikes_low_position;
        this.lowQualitySpike.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        this.lowQualitySpike.transform.DOScale(new Vector3(1f, 1f, 1f), this.stage1Anim).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBounce);
        this.lowQualitySpike.transform.DOScale(0.0f, this.stage2Anim).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBounce).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(this.stage3Anim);
      }
      else
      {
        this.Spine.AnimationState.SetAnimation(0, this.Animation, true);
        this.Spine.AnimationState.Complete += new Spine.AnimationState.TrackEntryDelegate(this.AnimationState_Complete);
      }
    }
    if (!this.useLowQualitySpikes || (double) (this.timer -= Time.deltaTime) > 0.0)
      return;
    this.AnimationState_Complete((TrackEntry) null);
    this.Init = false;
  }

  public void OnDisable()
  {
    this.Init = false;
    if (this.useLowQualitySpikes || !((Object) this.Spine != (Object) null) || this.Spine.AnimationState == null)
      return;
    this.Spine.AnimationState.Complete -= new Spine.AnimationState.TrackEntryDelegate(this.AnimationState_Complete);
  }

  public void AnimationState_Complete(TrackEntry trackEntry) => this.gameObject.SetActive(false);

  public void HandleFrozenTime()
  {
    if ((Object) this.Spine == (Object) null)
      return;
    if (PlayerRelic.TimeFrozen)
      this.Spine.timeScale = 0.0001f;
    else
      this.Spine.timeScale = 1f;
  }

  public static void UnloadResources()
  {
    SimpleSpineDeactivateAfterPlay.material_low = (Material) null;
    if (SimpleSpineDeactivateAfterPlay.addrHandle.IsValid())
      Addressables.Release<Material>(SimpleSpineDeactivateAfterPlay.addrHandle);
    SimpleSpineDeactivateAfterPlay.spikes_low = (Sprite[]) null;
    SimpleSpineDeactivateAfterPlay.spikesAnimation = (Spine.Animation) null;
  }

  public enum SpikeType
  {
    EnemyWormBoss,
    EnemyJellySpikerMiniBoss,
  }
}
