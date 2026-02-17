// Decompiled with JetBrains decompiler
// Type: BreakableObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BreakableObject : BaseMonoBehaviour
{
  public bool destroyOnWalk = true;
  public bool destroyOnRoll;
  public bool destroyOnAttack = true;
  public bool shakeCamera = true;
  public bool emitSmokeExplosion = true;
  public bool emitGroundSmashDecal;
  public float Radius = 1f;
  public Vector3 RadiusOffset = Vector3.zero;
  public bool Seperate = true;
  public List<GameObject> gameObjectsToDisable = new List<GameObject>();
  public SoundConstants.SoundMaterial soundMaterial;
  public string customHitSFX = "";
  public string customDestroySFX = "";
  public BiomeConstants.TypeOfParticle Type;
  public float zSpawn = 0.5f;
  public float CameraShake = 2f;
  public int maxParticles = 20;
  public float RandomVariation = 0.5f;
  public float VelocityMultiplyer = 1f;
  public float ParticleMultiplyer = 1f;
  public static List<BreakableObject> BreakableObjects = new List<BreakableObject>();
  public bool Activated;
  public float CheckDistance;
  public Vector3 SeperatorVelocity;
  public bool hasHealth;
  public int HP;
  public int TotalHP = 6;
  public bool ShowGizmos;
  public bool TESTDEBUG = true;
  public Vector2 Velocity;
  public bool ShakeObject = true;
  public float shakeDuration = 0.5f;
  public Vector3 shakeStrength = new Vector3(0.5f, 0.5f, 0.01f);
  [Range(0.0f, 30f)]
  public int vibrato = 10;
  [Range(0.0f, 180f)]
  public float randomness = 90f;
  public bool ActivatedHit;

  public void OnEnable() => BreakableObject.BreakableObjects.Add(this);

  public void OnDisable() => BreakableObject.BreakableObjects.Remove(this);

  public void OnDrawGizmos()
  {
    if (!this.ShowGizmos)
      return;
    Utils.DrawCircleXY(this.transform.position + this.RadiusOffset, this.Radius, Color.green);
  }

  public void Start() => this.HP = this.TotalHP;

  public void Update()
  {
    if (this.Activated || this.ActivatedHit)
      return;
    foreach (Health allUnit in Health.allUnits)
    {
      if ((allUnit.team == Health.Team.PlayerTeam || allUnit.team == Health.Team.Team2) && !((UnityEngine.Object) allUnit == (UnityEngine.Object) null))
      {
        this.CheckDistance = Vector3.Distance(allUnit.transform.position, this.transform.position + this.RadiusOffset);
        if ((double) this.CheckDistance <= (double) this.Radius && this.destroyOnWalk)
          this.Collide(allUnit);
        else if ((UnityEngine.Object) allUnit.state != (UnityEngine.Object) null && (double) this.CheckDistance <= (double) this.Radius && allUnit.state.CURRENT_STATE == StateMachine.State.Dodging && this.destroyOnRoll)
          this.Collide(allUnit);
        else if ((UnityEngine.Object) allUnit.state != (UnityEngine.Object) null && (allUnit.state.CURRENT_STATE == StateMachine.State.RecoverFromAttack || allUnit.state.CURRENT_STATE == StateMachine.State.Attacking) && (double) this.CheckDistance <= (double) this.Radius + 1.0 && (double) Mathf.Abs(Utils.GetAngle(allUnit.transform.position, this.transform.position + this.RadiusOffset) - allUnit.state.facingAngle) < 90.0 && this.destroyOnAttack)
          this.Collide(allUnit);
      }
    }
    if (!this.Seperate)
      return;
    this.SeperateObject();
  }

  public void SeperateObject()
  {
    this.SeperatorVelocity = Vector3.zero;
    foreach (BreakableObject breakableObject in BreakableObject.BreakableObjects)
    {
      if (!((UnityEngine.Object) breakableObject == (UnityEngine.Object) null) && !((UnityEngine.Object) breakableObject == (UnityEngine.Object) this))
      {
        float num = Vector2.Distance((Vector2) (breakableObject.transform.position + breakableObject.RadiusOffset), (Vector2) (this.transform.position + this.RadiusOffset));
        if ((double) num < (double) this.Radius + (double) breakableObject.Radius)
        {
          float f = Utils.GetAngle(breakableObject.transform.position + breakableObject.RadiusOffset, this.transform.position + this.RadiusOffset) * ((float) Math.PI / 180f);
          if (this.TESTDEBUG)
          {
            this.SeperatorVelocity.x += (breakableObject.Radius + this.Radius - num) * Mathf.Cos(f);
            this.SeperatorVelocity.y += (breakableObject.Radius + this.Radius - num) * Mathf.Sin(f);
          }
          else
          {
            this.SeperatorVelocity.x += (breakableObject.Radius - num) * Mathf.Cos(f);
            this.SeperatorVelocity.y += (breakableObject.Radius - num) * Mathf.Sin(f);
          }
        }
      }
    }
    this.transform.position = this.transform.position + this.SeperatorVelocity;
  }

  public void Collide(Health h)
  {
    this.StopAllCoroutines();
    if (!this.hasHealth)
      this.StartCoroutine((IEnumerator) this.CollideRoutine(h));
    else if (this.HP <= 0)
    {
      this.StartCoroutine((IEnumerator) this.CollideRoutine(h));
    }
    else
    {
      --this.HP;
      this.StartCoroutine((IEnumerator) this.HitRoutine(h));
    }
  }

  public IEnumerator HitRoutine(Health h)
  {
    BreakableObject breakableObject = this;
    breakableObject.ActivatedHit = true;
    if ((UnityEngine.Object) h != (UnityEngine.Object) null)
    {
      Vector3 LastPos = h.transform.position;
      yield return (object) null;
      breakableObject.Velocity = (Vector2) ((h.transform.position - LastPos) * 50f);
      LastPos = new Vector3();
    }
    if (breakableObject.ShakeObject)
    {
      foreach (GameObject gameObject in breakableObject.gameObjectsToDisable)
      {
        gameObject.transform.DORestart();
        gameObject.transform.DOShakePosition(breakableObject.shakeDuration, breakableObject.shakeStrength, breakableObject.vibrato, breakableObject.randomness);
      }
    }
    if (!string.IsNullOrEmpty(breakableObject.customHitSFX))
      AudioManager.Instance.PlayOneShot(breakableObject.customHitSFX, breakableObject.transform.position);
    else if (breakableObject.soundMaterial != SoundConstants.SoundMaterial.None)
      AudioManager.Instance.PlayOneShot(SoundConstants.GetImpactSoundPathForMaterial(breakableObject.soundMaterial), breakableObject.transform.position);
    if (breakableObject.shakeCamera)
      CameraManager.shakeCamera(breakableObject.CameraShake / 1.5f, Utils.GetAngle(breakableObject.transform.position, breakableObject.transform.position), false);
    BiomeConstants.Instance.EmitParticleChunk(breakableObject.Type, breakableObject.transform.position, (Vector3) breakableObject.Velocity, breakableObject.maxParticles / 4, breakableObject.ParticleMultiplyer);
    yield return (object) new WaitForSeconds(breakableObject.shakeDuration);
    breakableObject.ActivatedHit = false;
  }

  public IEnumerator CollideRoutine(Health h)
  {
    BreakableObject breakableObject = this;
    breakableObject.Activated = true;
    Vector3 LastPos = h.transform.position;
    yield return (object) null;
    breakableObject.Velocity = (Vector2) ((h.transform.position - LastPos) * 50f);
    foreach (GameObject gameObject in breakableObject.gameObjectsToDisable)
      gameObject.SetActive(false);
    if (!string.IsNullOrEmpty(breakableObject.customDestroySFX))
      AudioManager.Instance.PlayOneShot(breakableObject.customDestroySFX, breakableObject.transform.position);
    else if (breakableObject.soundMaterial != SoundConstants.SoundMaterial.None)
      AudioManager.Instance.PlayOneShot(SoundConstants.GetBreakSoundPathForMaterial(breakableObject.soundMaterial), breakableObject.transform.position);
    if (breakableObject.shakeCamera)
      CameraManager.shakeCamera(breakableObject.CameraShake, Utils.GetAngle(breakableObject.transform.position, breakableObject.transform.position), false);
    if (breakableObject.emitSmokeExplosion)
      BiomeConstants.Instance.EmitSmokeExplosionVFX(breakableObject.transform.position + Vector3.back * breakableObject.zSpawn);
    if (breakableObject.emitGroundSmashDecal)
      BiomeConstants.Instance.EmitGroundSmashVFXParticles(breakableObject.transform.position);
    BiomeConstants.Instance.EmitParticleChunk(breakableObject.Type, breakableObject.transform.position, (Vector3) (breakableObject.Velocity * breakableObject.VelocityMultiplyer), breakableObject.maxParticles, breakableObject.ParticleMultiplyer);
  }
}
