// Decompiled with JetBrains decompiler
// Type: MillipedeBodyPart
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class MillipedeBodyPart : BaseMonoBehaviour
{
  public SkeletonAnimation Spine;
  [SerializeField]
  private SimpleSpineFlash simpleSpineFlash;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string idleAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string attackAnticipationAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string attackAnimation;
  [Space]
  [SerializeField]
  private float anticipation;
  [SerializeField]
  private float cooldown;
  [SerializeField]
  private float attackRadius = 2f;
  [SerializeField]
  private ColliderEvents damageCollider;
  private EnemyMillipede millipede;
  private Health health;
  private bool dropped;
  private bool spiking;

  private void Awake()
  {
    this.millipede = this.GetComponentInParent<EnemyMillipede>();
    this.health = this.GetComponent<Health>();
    this.health.OnDamaged += new Health.HealthEvent(this.OnDamaged);
  }

  private void OnDestroy()
  {
    if (!(bool) (Object) this.health)
      return;
    this.health.OnDamaged -= new Health.HealthEvent(this.OnDamaged);
  }

  private void Update()
  {
    if (!this.dropped || this.spiking || !(bool) (Object) PlayerFarming.Instance || (double) Vector3.Distance(this.transform.position, PlayerFarming.Instance.transform.position) >= (double) this.attackRadius)
      return;
    this.StartCoroutine((IEnumerator) this.SpikeIE());
  }

  private IEnumerator SpikeIE()
  {
    this.spiking = true;
    this.Spine.AnimationState.SetAnimation(0, this.attackAnticipationAnimation, true);
    float t = 0.0f;
    while ((double) t < (double) this.anticipation)
    {
      this.simpleSpineFlash.FlashWhite(t / this.anticipation);
      t += Time.deltaTime;
      yield return (object) null;
    }
    yield return (object) new WaitForEndOfFrame();
    this.simpleSpineFlash.FlashWhite(false);
    this.Spine.AnimationState.AddAnimation(0, this.attackAnimation, false, 0.0f);
    this.Spine.AnimationState.AddAnimation(0, this.idleAnimation, true, 0.0f);
    this.damageCollider.SetActive(true);
    yield return (object) new WaitForSeconds(0.1f);
    this.damageCollider.SetActive(false);
    yield return (object) new WaitForSeconds(this.cooldown);
    this.spiking = false;
  }

  private void OnDamaged(GameObject attacker, Vector3 attackLocation, float damage)
  {
    if (!((Object) attacker != (Object) this.millipede.gameObject) || this.dropped)
      return;
    this.millipede.DamageFromBody(attacker, attackLocation, damage);
  }

  public void DroppedPart() => this.dropped = true;
}
