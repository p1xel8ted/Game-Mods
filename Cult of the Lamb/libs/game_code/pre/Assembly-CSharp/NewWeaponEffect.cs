// Decompiled with JetBrains decompiler
// Type: NewWeaponEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using UnityEngine;

#nullable disable
public class NewWeaponEffect : MonoBehaviour
{
  public GameObject WeaponContainer;
  public Interaction_WeaponSelectionPodium WeaponInteraction;
  [SerializeField]
  private GameObject distortionObject;
  [SerializeField]
  private float lifeTime = 2f;
  public float LifeTimeMultiplier = 0.5f;
  private float timer;
  private EventInstance LoopInstance;
  private bool createdLoop;
  private bool MusicStopped;

  private void OnDisable()
  {
    AudioManager.Instance.StopLoop(this.LoopInstance);
    Object.Destroy((Object) this.gameObject);
  }

  private void Start()
  {
    this.distortionObject.transform.DOScale(9f, 0.9f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear);
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

  private void OnDestroy() => AudioManager.Instance.StopLoop(this.LoopInstance);

  private void Update()
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
}
