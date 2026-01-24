// Decompiled with JetBrains decompiler
// Type: ProjectileVerticalSlash
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Ara;
using FMODUnity;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class ProjectileVerticalSlash : BaseMonoBehaviour
{
  public Transform t;
  public ColliderEvents damageColliderEvents;
  public Health.Team Team = Health.Team.Team2;
  public SpriteRenderer ShadowSpriteRenderer;
  public AraTrail trail;
  public TrailRenderer lowQualityTrail;
  [CompilerGenerated]
  public float \u003CAngle\u003Ek__BackingField;
  [CompilerGenerated]
  public float \u003CSpeed\u003Ek__BackingField;
  [EventRef]
  public string ImpactSFX = "event:/enemy/spit_gross_projectile";
  public Vector3 NewPosition;
  public GameObject owner;
  public bool isTravelling;
  public float time;
  public const string WALL_LAYER = "Island";

  public float Angle
  {
    get => this.\u003CAngle\u003Ek__BackingField;
    set => this.\u003CAngle\u003Ek__BackingField = value;
  }

  public float Speed
  {
    get => this.\u003CSpeed\u003Ek__BackingField;
    set => this.\u003CSpeed\u003Ek__BackingField = value;
  }

  public void OnEnable()
  {
    if ((bool) (UnityEngine.Object) this.trail)
    {
      if ((bool) (UnityEngine.Object) this.lowQualityTrail)
        this.lowQualityTrail.enabled = false;
      this.trail.enabled = true;
      this.trail.Clear();
      this.trail.emit = true;
    }
    this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    this.damageColliderEvents.SetActive(false);
  }

  public void OnDisable()
  {
    this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    this.damageColliderEvents.SetActive(false);
  }

  public void Play(Vector3 startPosition, float Angle, float Speed, Health.Team team = Health.Team.Team2)
  {
    this.trail.Clear();
    this.transform.position = new Vector3(startPosition.x, startPosition.y, -0.5f);
    this.Angle = Angle * ((float) Math.PI / 180f);
    this.Speed = Speed;
    this.Team = team;
    this.t.gameObject.SetActive(true);
    this.ShadowSpriteRenderer.enabled = true;
    this.StartCoroutine((IEnumerator) this.MoveRoutine());
  }

  public void ResetPosition() => this.transform.position = Vector3.zero;

  public IEnumerator MoveRoutine()
  {
    ProjectileVerticalSlash projectileVerticalSlash = this;
    projectileVerticalSlash.damageColliderEvents.SetActive(true);
    projectileVerticalSlash.isTravelling = true;
    while (projectileVerticalSlash.isTravelling)
    {
      if (!PlayerRelic.TimeFrozen)
      {
        projectileVerticalSlash.NewPosition = new Vector3(projectileVerticalSlash.Speed * Mathf.Cos(projectileVerticalSlash.Angle), projectileVerticalSlash.Speed * Mathf.Sin(projectileVerticalSlash.Angle)) * Time.fixedDeltaTime;
        projectileVerticalSlash.transform.position += projectileVerticalSlash.NewPosition;
        if ((double) projectileVerticalSlash.t.position.z > 0.0)
          break;
        yield return (object) new WaitForFixedUpdate();
      }
      else
        yield return (object) null;
    }
  }

  public void DoWallCollision()
  {
    this.StartCoroutine((IEnumerator) this.DestroyAfterWait(1f));
    this.isTravelling = false;
    this.t.gameObject.SetActive(false);
    this.ShadowSpriteRenderer.enabled = false;
    GameObject gameObject = BiomeConstants.Instance.GrenadeBulletImpact_A.Spawn();
    gameObject.transform.position = this.t.transform.position;
    gameObject.transform.rotation = Quaternion.identity;
    gameObject.transform.localScale = Vector3.one * 0.5f;
    CameraManager.shakeCamera(0.5f, false);
    AudioManager.Instance.PlayOneShot(this.ImpactSFX);
  }

  public IEnumerator DestroyAfterWait(float Delay)
  {
    ProjectileVerticalSlash projectileVerticalSlash = this;
    float num = 0.0f;
    if ((bool) (UnityEngine.Object) projectileVerticalSlash.trail)
    {
      num = projectileVerticalSlash.trail.time;
      projectileVerticalSlash.trail.emit = false;
    }
    yield return (object) new WaitForSeconds(Delay + num);
    projectileVerticalSlash.gameObject.Recycle();
  }

  public void OnDamageTriggerEnter(Collider2D collider)
  {
    int layer1 = collider.gameObject.layer;
    Health component = collider.GetComponent<Health>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.team != this.Team)
      component.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
    int layer2 = LayerMask.NameToLayer("Island");
    if (layer1 != layer2)
      return;
    this.DoWallCollision();
  }
}
