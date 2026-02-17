// Decompiled with JetBrains decompiler
// Type: NewWeaponEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class NewWeaponEffect : MonoBehaviour
{
  public GameObject WeaponContainer;
  public Interaction_WeaponSelectionPodium WeaponInteraction;
  [SerializeField]
  public GameObject distortionObject;
  [SerializeField]
  public float lifeTime = 2f;
  public float LifeTimeMultiplier = 0.5f;
  public float timer;
  public EventInstance LoopInstance;
  public bool createdLoop;
  public bool MusicStopped;

  public void OnDisable()
  {
    AudioManager.Instance.StopLoop(this.LoopInstance);
    Object.Destroy((Object) this.gameObject);
  }

  public void Start()
  {
    this.distortionObject.transform.DOScale(9f, 0.9f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear);
    this.distortionObject.GetComponent<SpriteRenderer>().DOFade(0.0f, 0.9f).OnComplete<TweenerCore<Color, Color, ColorOptions>>((TweenCallback) (() => Object.Destroy((Object) this.distortionObject)));
    AudioManager.Instance.PlayOneShot(" event:/player/Curses/vortex_start", this.gameObject);
    if (!this.createdLoop)
    {
      this.LoopInstance = AudioManager.Instance.CreateLoop("event:/player/Curses/vortex_loop", this.gameObject, true);
      this.createdLoop = true;
    }
    this.lifeTime *= this.LifeTimeMultiplier;
    ParticleSystem[] componentsInChildren = this.GetComponentsInChildren<ParticleSystem>();
    foreach (ParticleSystem particleSystem in componentsInChildren)
    {
      particleSystem.Stop();
      ParticleSystem.MainModule main = particleSystem.main;
      ParticleSystem.MinMaxCurve startLifetime = main.startLifetime;
      startLifetime.constant *= this.LifeTimeMultiplier;
      main.startLifetime = startLifetime;
    }
    foreach (ParticleSystem particleSystem in componentsInChildren)
      particleSystem.Play();
    this.WeaponContainer.transform.localScale = Vector3.zero;
    this.WeaponContainer.SetActive(false);
    this.WeaponInteraction.enabled = false;
    DG.Tweening.Sequence sequence = DOTween.Sequence();
    sequence.AppendInterval(2f);
    sequence.AppendCallback((TweenCallback) (() =>
    {
      this.WeaponInteraction.enabled = true;
      this.WeaponContainer.SetActive(true);
    }));
    sequence.Append((Tween) this.WeaponContainer.transform.DOScale(Vector3.one, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack));
    sequence.Play<DG.Tweening.Sequence>();
  }

  public void OnDestroy() => AudioManager.Instance.StopLoop(this.LoopInstance);

  public void Update()
  {
    if ((double) this.timer > 3.0 && !this.MusicStopped)
    {
      this.MusicStopped = true;
      AudioManager.Instance.StopLoop(this.LoopInstance);
      AudioManager.Instance.PlayOneShot("event:/player/Curses/vortex_end", this.gameObject);
    }
    if ((double) (this.timer += Time.deltaTime) <= (double) this.lifeTime)
      return;
    Object.Destroy((Object) this.gameObject);
  }

  [CompilerGenerated]
  public void \u003CStart\u003Eb__9_0() => Object.Destroy((Object) this.distortionObject);

  [CompilerGenerated]
  public void \u003CStart\u003Eb__9_1()
  {
    this.WeaponInteraction.enabled = true;
    this.WeaponContainer.SetActive(true);
  }
}
