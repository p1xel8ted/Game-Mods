// Decompiled with JetBrains decompiler
// Type: SimpleVFX
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMBiomeGeneration;
using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

  public void OnEnable()
  {
    BiomeGenerator.OnBiomeChangeRoom += new BiomeGenerator.BiomeAction(this.BiomeGenerator_OnBiomeChangeRoom);
  }

  public void BiomeGenerator_OnBiomeChangeRoom()
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

  public void EnableSpine(TrackEntry entry)
  {
  }

  public void Play()
  {
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.FrameDelay((System.Action) (() =>
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
      else if (this.useCustomEndTime && ObjectPool.IsSpawned(this.gameObject))
        this.StartCoroutine((IEnumerator) this.customEndTimer());
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
    this.StartCoroutine((IEnumerator) this.FrameDelay((System.Action) (() =>
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
      else if (this.useCustomEndTime && ObjectPool.IsSpawned(this.gameObject))
        this.StartCoroutine((IEnumerator) this.customEndTimer());
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
    this.StartCoroutine((IEnumerator) this.FrameDelay((System.Action) (() =>
    {
      if (!this.justPlayParticles)
      {
        this.mesh.enabled = false;
        this.transform.position = Position;
        if ((UnityEngine.Object) this.Spine != (UnityEngine.Object) null)
        {
          this.Spine.gameObject.SetActive(true);
          if (this.Spine.AnimationState != null)
          {
            this.Spine.AnimationState.SetAnimation(0, animation, false);
            this.Spine.AnimationState.Complete += new Spine.AnimationState.TrackEntryDelegate(this.AnimationState_Complete);
          }
        }
        this.mesh.enabled = true;
        this.gameObject.SetActive(true);
        if (this.useCustomEndTime)
          this.StartCoroutine((IEnumerator) this.customEndTimer());
        if (this.setRotation)
          this.Rotation = Angle + (float) UnityEngine.Random.Range(-10, 10);
      }
      else if (this.useCustomEndTime && ObjectPool.IsSpawned(this.gameObject))
        this.StartCoroutine((IEnumerator) this.customEndTimer());
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

  public IEnumerator FrameDelay(System.Action callback)
  {
    yield return (object) new WaitForEndOfFrame();
    System.Action action = callback;
    if (action != null)
      action();
  }

  public IEnumerator customEndTimer()
  {
    SimpleVFX simpleVfx = this;
    yield return (object) new WaitForSeconds(simpleVfx.customEndTime);
    if (simpleVfx.gameObject.activeInHierarchy)
      simpleVfx.gameObject.Recycle();
  }

  public void PlayAnimation() => this.Play(this.transform.position, (float) UnityEngine.Random.Range(0, 360));

  public void AnimationState_Complete(TrackEntry trackEntry)
  {
    this.Spine.AnimationState.Complete -= new Spine.AnimationState.TrackEntryDelegate(this.AnimationState_Complete);
    if (this.useCustomEndTime)
    {
      this.mesh.enabled = false;
    }
    else
    {
      if (!this.gameObject.activeInHierarchy)
        return;
      this.gameObject.Recycle();
    }
  }

  public void OnDisable()
  {
    if ((UnityEngine.Object) this.Spine != (UnityEngine.Object) null && this.Spine.AnimationState != null)
    {
      this.Spine.AnimationState.Start -= new Spine.AnimationState.TrackEntryDelegate(this.EnableSpine);
      this.Spine.AnimationState.Complete -= new Spine.AnimationState.TrackEntryDelegate(this.AnimationState_Complete);
    }
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.BiomeGenerator_OnBiomeChangeRoom);
  }

  public void Update()
  {
    if (this.justPlayParticles || !this.setRotation)
      return;
    this.transform.eulerAngles = new Vector3(-60f, 0.0f, this.Rotation);
  }

  [CompilerGenerated]
  public void \u003CPlay\u003Eb__13_0()
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
    else if (this.useCustomEndTime && ObjectPool.IsSpawned(this.gameObject))
      this.StartCoroutine((IEnumerator) this.customEndTimer());
    if (this.particlesToPlay.Count <= 0)
      return;
    if ((UnityEngine.Object) this.particlesToPlay[0] != (UnityEngine.Object) null)
      this.particlesToPlay[0].gameObject.SetActive(true);
    int index = UnityEngine.Random.Range(0, this.particlesToPlay.Count - 1);
    this.particlesToPlay[index].Stop();
    this.particlesToPlay[index].Play();
  }
}
