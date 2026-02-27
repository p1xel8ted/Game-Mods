// Decompiled with JetBrains decompiler
// Type: TrapBellAvalanche
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class TrapBellAvalanche : BaseMonoBehaviour
{
  public TrapBellAvalanche.LightningType TrapType;
  [SerializeField]
  public Transform[] trapAvalanches;
  [SerializeField]
  public GameObject avalanche;
  [SerializeField]
  public float cooldown;
  [SerializeField]
  public float radius;
  [SerializeField]
  public float dropDelay;
  [SerializeField]
  public float dropSpeed;
  [SerializeField]
  public SkeletonAnimation spine;
  [SpineAnimation("", "", true, false, dataField = "spine")]
  public string idleAnimation;
  [SpineAnimation("", "", true, false, dataField = "spine")]
  public string hitAnimation;
  [SerializeField]
  public float dropHitOffset = -0.4f;
  [SerializeField]
  public SimpleSpineFlash flash;
  [Header("FX")]
  [SerializeField]
  public GameObject aoeParticles;
  public float coolDownTimer;
  public Health health;

  public bool CanBeActivated => (double) this.coolDownTimer <= 0.0;

  public void Awake() => this.health = this.GetComponent<Health>();

  public void Update() => this.RefreshCooldownTimer();

  public void OnEnable() => this.health.OnHit += new Health.HitAction(this.OnHit);

  public void OnDisable() => this.health.OnHit -= new Health.HitAction(this.OnHit);

  public void ActivateTrap()
  {
    if (!this.CanBeActivated)
      return;
    AudioManager.Instance.PlayOneShot("event:/building/building_bell_ring", this.transform.position);
    this.DropAvalanches();
    this.ResetCooldownTimer();
    this.ActivateCloseBells();
  }

  public void ActivateTrapDelay(float delay) => this.StartCoroutine(this.ActivateTrapIE(delay));

  public IEnumerator ActivateTrapIE(float delay)
  {
    yield return (object) new WaitForSeconds(delay);
    this.ActivateTrap();
  }

  public void DropAvalanches()
  {
    foreach (Transform trapAvalanch in this.trapAvalanches)
    {
      TrapAvalanche component = ObjectPool.Spawn(this.avalanche, trapAvalanch.position, Quaternion.identity).GetComponent<TrapAvalanche>();
      this.AnimateHit();
      component.DropDelay = this.dropDelay;
      component.DropSpeed = this.dropSpeed;
      component.onLand.AddListener(new UnityAction(this.AnimateHit));
      component.Drop();
    }
  }

  public void RefreshCooldownTimer()
  {
    if ((double) this.coolDownTimer <= 0.0)
      return;
    this.coolDownTimer -= Time.deltaTime;
  }

  public void ResetCooldownTimer() => this.coolDownTimer = this.cooldown;

  public void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    this.ActivateTrap();
    this.health.HP = float.MaxValue;
  }

  public void ActivateCloseBells()
  {
    BellMechanics.ActivateCloseBells(this.aoeParticles, this.transform.position, this.radius);
  }

  public void AnimateHit()
  {
    this.spine.AnimationState.SetAnimation(0, this.hitAnimation, false);
    this.spine.AnimationState.AddAnimation(0, this.idleAnimation, true, 0.0f);
  }

  public enum LightningType
  {
    Cross,
    Diagonal,
    Ring,
  }
}
