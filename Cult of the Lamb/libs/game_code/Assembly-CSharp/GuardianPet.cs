// Decompiled with JetBrains decompiler
// Type: GuardianPet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using MMBiomeGeneration;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class GuardianPet : UnitObject
{
  [SerializeField]
  public SkeletonAnimation spine;
  [SerializeField]
  public SimpleSpineFlash simpleSpineFlash;
  [SerializeField]
  public GameObject shadow;
  [SerializeField]
  public GameObject spawnVfx;
  [SerializeField]
  public GameObject deathVfx;
  public List<Behaviour> BehavioursToActivate = new List<Behaviour>();
  public GuardianPetController ParentPetController;
  public string GetHitVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_monster_small/gethit";
  public string DeathVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_monster_small/death";
  public string WarningVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_monster_small/warning";
  public bool HostHasDied;

  public virtual void LaunchPet(GuardianPetController TargetObject)
  {
    if (!string.IsNullOrEmpty(this.WarningVO))
      AudioManager.Instance.PlayOneShot(this.WarningVO, this.transform.position);
    this.ParentPetController = TargetObject;
    this.transform.eulerAngles = Vector3.zero;
    this.transform.localScale = Vector3.one * 1.5f;
    this.transform.parent = BiomeGenerator.Instance.CurrentRoom.generateRoom.transform;
    this.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    this.transform.DOMoveZ(-3f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    foreach (Behaviour behaviour in this.BehavioursToActivate)
      behaviour.enabled = true;
    this.Play();
    this.spawnVfx.gameObject.SetActive(true);
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    if (!string.IsNullOrEmpty(this.GetHitVO))
      AudioManager.Instance.PlayOneShot(this.GetHitVO, this.transform.position);
    this.simpleSpineFlash.FlashFillRed();
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    if (!string.IsNullOrEmpty(this.DeathVO))
      AudioManager.Instance.PlayOneShot(this.DeathVO, this.transform.position);
    this.deathVfx.transform.parent = (Transform) null;
    this.deathVfx.gameObject.SetActive(true);
    this.ParentPetController.RemoveActivePet(this);
  }

  public virtual void Play()
  {
    this.health.invincible = false;
    this.DisableForces = false;
    this.rb.simulated = true;
  }

  public void Return()
  {
    if (this.HostHasDied)
      return;
    this.moveVX = this.moveVY = this.speed = 0.0f;
    this.rb.velocity = Vector2.zero;
    this.rb.simulated = false;
    this.DisableForces = true;
    Debug.Log((object) "Return!".Colour(Color.yellow));
    foreach (Behaviour behaviour in this.BehavioursToActivate)
      behaviour.enabled = false;
    this.health.invincible = true;
    float Speed = 5f;
    float Distance = Vector3.Distance(this.ParentPetController.transform.position, this.transform.position);
    TweenerCore<Vector3, Vector3, VectorOptions> t = this.transform.DOMove(this.ParentPetController.transform.position, Distance / Speed);
    t.OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      this.transform.parent = this.ParentPetController.transform;
      this.transform.localPosition = Vector3.zero;
      this.transform.localScale = Vector3.one * 1.5f;
      this.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    }));
    t.OnUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      Distance = Vector3.Distance(this.ParentPetController.transform.position, this.transform.position);
      if ((double) Vector3.Distance(this.ParentPetController.transform.position, this.transform.position) <= 0.5)
        return;
      t.ChangeEndValue(this.ParentPetController.transform.position, Distance / Speed, true);
    }));
  }

  public override void Update()
  {
    base.Update();
    this.shadow.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0.0f);
  }
}
