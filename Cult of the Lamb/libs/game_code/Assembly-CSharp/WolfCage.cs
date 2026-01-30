// Decompiled with JetBrains decompiler
// Type: WolfCage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class WolfCage : BaseMonoBehaviour
{
  [SerializeField]
  public bool doRattle = true;
  [SerializeField]
  public float rattleWaitRangeMin = 2f;
  public float rattleWaitRangeMax = 3f;
  [SerializeField]
  public float rattleStrength = 0.1f;
  [SerializeField]
  public float rattleDuration = 0.5f;
  [SerializeField]
  public GameObject chargedParticles;
  public ParticleSystem cageParticles;
  [SerializeField]
  public float explodeDuration = 0.5f;
  [SerializeField]
  public float explodeInterval = 0.3f;
  [SerializeField]
  public float explodeSize = 1.5f;
  [SerializeField]
  public int explodeCount = 3;
  [SerializeField]
  public SkeletonAnimation cageSpine;
  [SerializeField]
  public string CageShakeSFX = "event:/dlc/dungeon05/enemy/miniboss_dog/intro_cage_shake";
  public Vector3 _originalPosition;

  public void Start()
  {
    this.chargedParticles.SetActive(false);
    this._originalPosition = this.transform.position;
    this.StartCoroutine((IEnumerator) this.RattleRoutine());
  }

  public IEnumerator RattleRoutine()
  {
    while (true)
    {
      yield return (object) new WaitForSeconds(Random.Range(this.rattleWaitRangeMin, this.rattleWaitRangeMax));
      this.Rattle();
    }
  }

  public void Rattle()
  {
    this.transform.DOShakePosition(this.rattleDuration, new Vector3(this.rattleStrength, 0.0f, 0.0f), randomness: 0.0f);
    if (string.IsNullOrEmpty(this.CageShakeSFX))
      return;
    AudioManager.Instance.PlayOneShot(this.CageShakeSFX, this.transform.position);
  }

  public void Explode()
  {
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.ExplodeRoutine());
  }

  public IEnumerator ExplodeRoutine(bool playSFX = false)
  {
    WolfCage wolfCage = this;
    yield return (object) new WaitForSeconds(1f);
    wolfCage.cageSpine.AnimationState.SetAnimation(0, "break", true);
    if (playSFX)
      AudioManager.Instance.PlayOneShot("event:/dlc/dungeon05/enemy/miniboss_dog/intro_break_free_start_postdlc");
    yield return (object) new WaitForSeconds(1f);
    for (int i = 0; i < wolfCage.explodeCount; ++i)
    {
      float num = Mathf.Lerp(0.05f, 0.15f, (float) i / (float) wolfCage.explodeCount);
      float duration = 0.2f;
      wolfCage.transform.DOPunchScale(Vector3.one * num, duration, 1, 0.5f);
      wolfCage.transform.DOShakePosition(wolfCage.rattleDuration, wolfCage.rattleStrength);
      wolfCage.transform.DOShakeRotation(wolfCage.rattleDuration, wolfCage.rattleStrength, fadeOut: false);
      yield return (object) new WaitForSeconds(wolfCage.explodeInterval);
    }
    wolfCage.cageParticles.Play();
    Object.Destroy((Object) wolfCage.gameObject);
  }
}
