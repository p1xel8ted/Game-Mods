// Decompiled with JetBrains decompiler
// Type: FrogBossTongue
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using UnityEngine;

#nullable disable
public class FrogBossTongue : UnitObject
{
  [SerializeField]
  public Transform tongueTip;
  [SerializeField]
  public Transform tongueActualTip;
  [SerializeField]
  public Transform tongueBase;
  [SerializeField]
  public ColliderEvents tongueTipCollider;
  [SerializeField]
  public GameObject targetObject;
  [SerializeField]
  public AnimationCurve moveInCurve;
  [SerializeField]
  public AnimationCurve moveOutCurve;
  [SerializeField]
  public Renderer[] renderers;
  [SerializeField]
  public ParticleSystem tongueAOEParticles;
  public EnemyFrogBoss boss;
  public LineRenderer lineRenderer;
  public CircleCollider2D tipCollider;
  public Color[] originalColors;

  public override void Awake()
  {
    base.Awake();
    this.lineRenderer = this.GetComponent<LineRenderer>();
    this.boss = this.GetComponentInParent<EnemyFrogBoss>();
    this.originalColors = new Color[this.renderers.Length];
    this.tipCollider = this.tongueTipCollider.GetComponent<CircleCollider2D>();
    for (int index = 0; index < this.renderers.Length; ++index)
      this.originalColors[index] = this.renderers[index].material.color;
    this.tongueTipCollider.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnTriggerEnterEvent);
  }

  public new void LateUpdate()
  {
    this.lineRenderer.SetPosition(0, this.boss.TonguePosition.transform.position);
    this.lineRenderer.SetPosition(1, this.tongueTip.position);
  }

  public IEnumerator SpitTongueIE(
    Vector3 targetPosition,
    float delay,
    float moveDuration,
    float waitDelay,
    float retrieveDuration)
  {
    FrogBossTongue frogBossTongue = this;
    Vector3 dir = (targetPosition - frogBossTongue.boss.TonguePosition.transform.position).normalized with
    {
      z = 0.0f
    };
    frogBossTongue.tongueActualTip.transform.localPosition = new Vector3(0.0f, 0.0f, -0.6f);
    frogBossTongue.tongueActualTip.transform.localScale = Vector3.zero;
    frogBossTongue.gameObject.SetActive(true);
    frogBossTongue.targetObject.SetActive(true);
    frogBossTongue.targetObject.transform.position = targetPosition;
    frogBossTongue.tongueTip.transform.position = frogBossTongue.boss.TonguePosition.transform.position;
    frogBossTongue.tongueTip.gameObject.SetActive(false);
    yield return (object) new WaitForSeconds(delay);
    frogBossTongue.tongueTip.gameObject.SetActive(true);
    frogBossTongue.tongueActualTip.transform.DOScale(1f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    float t = 0.0f;
    while ((double) t < (double) moveDuration + 0.10000000149011612)
    {
      float time = t / (moveDuration + 0.1f);
      frogBossTongue.tongueTip.transform.position = Vector3.Lerp(frogBossTongue.boss.TonguePosition.transform.position, targetPosition + dir * 0.3f, frogBossTongue.moveInCurve.Evaluate(time));
      t += Time.deltaTime;
      yield return (object) null;
    }
    frogBossTongue.targetObject.SetActive(false);
    frogBossTongue.StartCoroutine(frogBossTongue.TurnOnDamageColliderForDuration(frogBossTongue.tongueTipCollider.gameObject, 0.25f));
    frogBossTongue.tongueAOEParticles.Play();
    CameraManager.instance.ShakeCameraForDuration(1f, 1f, 0.2f);
    AudioManager.Instance.PlayOneShot("event:/boss/frog/tongue_impact");
    yield return (object) new WaitForSeconds(waitDelay);
    frogBossTongue.tongueActualTip.transform.DOScale(0.0f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
    t = 0.0f;
    while ((double) t < (double) retrieveDuration)
    {
      float time = t / retrieveDuration;
      frogBossTongue.tongueTip.transform.position = Vector3.Lerp(frogBossTongue.tongueTip.transform.position, frogBossTongue.boss.TonguePosition.transform.position, frogBossTongue.moveOutCurve.Evaluate(time));
      t += Time.deltaTime;
      yield return (object) null;
    }
    frogBossTongue.gameObject.SetActive(false);
  }

  public void DealDamageToBoss(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    PlayerFarming farmingComponent = PlayerFarming.GetPlayerFarmingComponent(Attacker);
    this.boss.health.DealDamage(PlayerWeapon.GetDamage(1f, farmingComponent.currentWeaponLevel, farmingComponent) * 1.5f, Attacker, AttackLocation, AttackType: AttackType);
    for (int index = 0; index < this.renderers.Length; ++index)
    {
      this.renderers[index].material.color = Color.red;
      this.renderers[index].material.DOColor(this.originalColors[index], 0.25f);
    }
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    this.DealDamageToBoss(Attacker, AttackLocation, AttackType, FromBehind);
  }

  public IEnumerator TurnOnDamageColliderForDuration(GameObject collider, float duration)
  {
    collider.SetActive(true);
    foreach (Component component1 in Physics2D.OverlapCircleAll((Vector2) this.tipCollider.transform.position, this.tipCollider.radius))
    {
      UnitObject component2 = component1.GetComponent<UnitObject>();
      if ((bool) (Object) component2 && component2.health.team == Health.Team.Team2 && (Object) component2 != (Object) this.boss)
        component2.DoKnockBack(collider.gameObject, 1f, 0.5f);
    }
    yield return (object) new WaitForSeconds(duration);
    collider.SetActive(false);
  }

  public void OnTriggerEnterEvent(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((Object) component != (Object) null) || component.team == this.health.team)
      return;
    component.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
  }
}
