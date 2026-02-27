// Decompiled with JetBrains decompiler
// Type: SimpleVFX
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using MMBiomeGeneration;
using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[ExecuteAlways]
public class SimpleVFX : BaseMonoBehaviour
{
  public bool justPlayParticles;
  public bool setRotation = true;
  public float Rotation;
  public SkeletonAnimation Spine;
  public MeshRenderer mesh;
  [SpineAnimation("", "Spine", true, false)]
  public List<string> animationsToPlay = new List<string>();
  [SpineSlot("", "Spine", false, true, false)]
  public List<string> Slots = new List<string>();
  public List<ParticleSystem> particlesToPlay = new List<ParticleSystem>();
  public bool useCustomEndTime;
  public float customEndTime;

  private void OnEnable()
  {
    BiomeGenerator.OnBiomeChangeRoom += new BiomeGenerator.BiomeAction(this.BiomeGenerator_OnBiomeChangeRoom);
  }

  private void BiomeGenerator_OnBiomeChangeRoom()
  {
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.BiomeGenerator_OnBiomeChangeRoom);
    if ((UnityEngine.Object) this.Spine != (UnityEngine.Object) null && this.Spine.AnimationState != null)
      this.Spine.AnimationState.Complete -= new Spine.AnimationState.TrackEntryDelegate(this.AnimationState_Complete);
    if (this.useCustomEndTime && (UnityEngine.Object) this.mesh != (UnityEngine.Object) null)
      this.mesh.enabled = false;
    else if (ObjectPool.IsSpawned(this.gameObject))
      this.gameObject.Recycle();
    else
      this.gameObject.SetActive(false);
  }

  private void EnableSpine(TrackEntry entry)
  {
  }

  public void Play()
  {
    this.StopAllCoroutines();
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.FrameDelay((System.Action) (() =>
    {
      this.Spine.AnimationState.Start += new Spine.AnimationState.TrackEntryDelegate(this.EnableSpine);
      if (!this.justPlayParticles)
      {
        float num = 0.0f;
        this.Spine.AnimationState.SetAnimation(0, this.animationsToPlay[UnityEngine.Random.Range(0, this.animationsToPlay.Count - 1)], false);
        this.mesh.enabled = true;
        this.gameObject.SetActive(true);
        this.Spine.gameObject.SetActive(true);
        this.Spine.AnimationState.Complete += new Spine.AnimationState.TrackEntryDelegate(this.AnimationState_Complete);
        if (this.useCustomEndTime)
          this.StartCoroutine((IEnumerator) this.customEndTimer());
        if (this.setRotation)
          this.Rotation = num + (float) UnityEngine.Random.Range(-10, 10);
      }
      if (this.particlesToPlay.Count <= 0)
        return;
      if ((UnityEngine.Object) this.particlesToPlay[0] != (UnityEngine.Object) null)
        this.particlesToPlay[0].gameObject.SetActive(true);
      int index = UnityEngine.Random.Range(0, this.particlesToPlay.Count - 1);
      this.particlesToPlay[index].Stop();
      this.particlesToPlay[index].Play();
    })));
  }

  public void Play(Vector3 Position, float Angle = 0.0f)
  {
    this.StopAllCoroutines();
    this.gameObject.SetActive(true);
    this.gameObject.transform.position = Position;
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.FrameDelay((System.Action) (() =>
    {
      if (!this.justPlayParticles)
      {
        this.mesh.enabled = false;
        this.Spine.AnimationState.SetAnimation(0, this.animationsToPlay[UnityEngine.Random.Range(0, this.animationsToPlay.Count - 1)], false);
        this.mesh.enabled = true;
        this.Spine.gameObject.SetActive(true);
        this.Spine.AnimationState.Complete += new Spine.AnimationState.TrackEntryDelegate(this.AnimationState_Complete);
        if (this.useCustomEndTime)
          this.StartCoroutine((IEnumerator) this.customEndTimer());
        if (this.setRotation)
          this.Rotation = Angle + (float) UnityEngine.Random.Range(-10, 10);
      }
      if (this.particlesToPlay.Count <= 0)
        return;
      if ((UnityEngine.Object) this.particlesToPlay[0] != (UnityEngine.Object) null)
        this.particlesToPlay[0].gameObject.SetActive(true);
      this.transform.position = Position;
      int index = UnityEngine.Random.Range(0, this.particlesToPlay.Count - 1);
      this.particlesToPlay[index].Stop();
      this.particlesToPlay[index].Play();
    })));
  }

  public void Play(Vector3 Position, float Angle, string animation)
  {
    this.StopAllCoroutines();
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.FrameDelay((System.Action) (() =>
    {
      if (!this.justPlayParticles)
      {
        this.mesh.enabled = false;
        this.transform.position = Position;
        this.Spine.AnimationState.SetAnimation(0, animation, false);
        this.mesh.enabled = true;
        this.Spine.gameObject.SetActive(true);
        this.gameObject.SetActive(true);
        this.Spine.AnimationState.Complete += new Spine.AnimationState.TrackEntryDelegate(this.AnimationState_Complete);
        if (this.useCustomEndTime)
          this.StartCoroutine((IEnumerator) this.customEndTimer());
        if (this.setRotation)
          this.Rotation = Angle + (float) UnityEngine.Random.Range(-10, 10);
      }
      if (this.particlesToPlay.Count <= 0)
        return;
      if ((UnityEngine.Object) this.particlesToPlay[0] != (UnityEngine.Object) null)
        this.particlesToPlay[0].gameObject.SetActive(true);
      this.transform.position = Position;
      int index = UnityEngine.Random.Range(0, this.particlesToPlay.Count - 1);
      this.particlesToPlay[index].Stop();
      this.particlesToPlay[index].Play();
    })));
  }

  private IEnumerator FrameDelay(System.Action callback)
  {
    yield return (object) new WaitForEndOfFrame();
    System.Action action = callback;
    if (action != null)
      action();
  }

  private IEnumerator customEndTimer()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    SimpleVFX simpleVfx = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      simpleVfx.gameObject.Recycle();
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(simpleVfx.customEndTime);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private void PlayAnimation() => this.Play(this.transform.position, (float) UnityEngine.Random.Range(0, 360));

  private void AnimationState_Complete(TrackEntry trackEntry)
  {
    this.Spine.AnimationState.Complete -= new Spine.AnimationState.TrackEntryDelegate(this.AnimationState_Complete);
    if (this.useCustomEndTime)
      this.mesh.enabled = false;
    else
      this.gameObject.Recycle();
  }

  private void OnDisable()
  {
    if ((UnityEngine.Object) this.Spine != (UnityEngine.Object) null && this.Spine.AnimationState != null)
    {
      this.Spine.AnimationState.Start -= new Spine.AnimationState.TrackEntryDelegate(this.EnableSpine);
      this.Spine.AnimationState.Complete -= new Spine.AnimationState.TrackEntryDelegate(this.AnimationState_Complete);
    }
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.BiomeGenerator_OnBiomeChangeRoom);
  }

  private void Update()
  {
    if (this.justPlayParticles || !this.setRotation)
      return;
    this.transform.eulerAngles = new Vector3(-60f, 0.0f, this.Rotation);
  }
}
