// Decompiled with JetBrains decompiler
// Type: EnemyChaser
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using FMODUnity;
using UnityEngine;

#nullable disable
public class EnemyChaser : UnitObject
{
  [SerializeField]
  protected SimpleSpineFlash simpleSpineFlash;
  [SerializeField]
  protected ColliderEvents damageColliderEvents;
  [EventRef]
  public string onHitSoundPath = string.Empty;
  [EventRef]
  public string onDeathSoundPath = string.Empty;
  [Space]
  [SerializeField]
  private float checkPlayerInterval;
  [SerializeField]
  private float targetTrackingOffset;
  [SerializeField]
  private float directTargetDistance;
  [SerializeField]
  private bool canLoseRange;
  [SerializeField]
  private bool damageOnTouch;
  [SerializeField]
  protected float knockbackMultiplier;
  protected Health targetObject;
  protected GameManager gm;
  protected bool inRange;
  private Vector3 offset;
  private float checkPlayerTimestamp;

  public override void Awake()
  {
    base.Awake();
    if (!((Object) this.damageColliderEvents != (Object) null))
      return;
    this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    this.damageColliderEvents.SetActive(false);
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.offset = (Vector3) (Random.insideUnitCircle * this.targetTrackingOffset);
    this.damageColliderEvents.SetActive(this.damageOnTouch);
  }

  protected virtual void Start()
  {
    if (!((Object) this.gm == (Object) null))
      return;
    this.gm = GameManager.GetInstance();
  }

  public override void Update()
  {
    base.Update();
    if ((Object) this.targetObject == (Object) null)
      this.targetObject = this.GetClosestTarget();
    if (this.inRange)
      this.UpdateMoving();
    if (!((Object) this.targetObject != (Object) null))
      return;
    float num = Vector3.Distance(this.transform.position, this.targetObject.transform.position);
    if ((double) num < (double) this.directTargetDistance)
      this.offset = Vector3.zero;
    else if (this.offset == Vector3.zero)
      this.offset = (Vector3) (Random.insideUnitCircle * this.targetTrackingOffset);
    if (this.inRange && (!this.inRange || !this.canLoseRange))
      return;
    this.inRange = (double) num < (double) this.VisionRange;
  }

  protected virtual void UpdateMoving()
  {
    if ((double) this.gm.TimeSince(this.checkPlayerTimestamp) <= (double) this.checkPlayerInterval || !GameManager.RoomActive)
      return;
    this.targetObject = this.GetClosestTarget();
    this.checkPlayerTimestamp = this.gm.CurrentTime + this.checkPlayerInterval;
    this.givePath(this.targetObject.transform.position + this.offset);
    this.LookAtAngle(Utils.GetAngle(this.transform.position, this.targetObject.transform.position + this.offset));
  }

  protected void LookAtAngle(float angle)
  {
    this.state.facingAngle = angle;
    this.state.LookAngle = angle;
  }

  protected virtual void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((Object) component != (Object) null) || component.team == this.health.team && this.health.team != Health.Team.PlayerTeam)
      return;
    component.DealDamage(component.team == Health.Team.PlayerTeam ? 1f : (float) int.MaxValue, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    if (string.IsNullOrEmpty(this.onDeathSoundPath))
      return;
    AudioManager.Instance.PlayOneShot(this.onDeathSoundPath, this.transform.position);
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    this.simpleSpineFlash.FlashFillRed();
    this.targetObject = (Health) null;
    if ((double) this.knockbackMultiplier != 0.0)
      this.DoKnockBack(Attacker, this.knockbackMultiplier, 1f);
    if (string.IsNullOrEmpty(this.onHitSoundPath))
      return;
    AudioManager.Instance.PlayOneShot(this.onHitSoundPath, this.transform.position);
  }
}
