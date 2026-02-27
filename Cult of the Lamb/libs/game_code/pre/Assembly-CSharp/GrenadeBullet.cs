// Decompiled with JetBrains decompiler
// Type: GrenadeBullet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Ara;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class GrenadeBullet : BaseMonoBehaviour
{
  public Transform t;
  private float GravSpeed = 0.2f;
  public ColliderEvents damageColliderEvents;
  private Health.Team Team = Health.Team.Team2;
  public SpriteRenderer ShadowSpriteRenderer;
  [SerializeField]
  private SpriteRenderer indicatorIcon;
  public AraTrail Trail;
  private float time;
  private Color indicatorColor = Color.white;
  private Vector3 NewPosition;
  private Coroutine damageColliderRoutine;

  private void OnEnable()
  {
    this.Trail = this.indicatorIcon.transform.parent.Find("GrenadeBullet_Tail").GetComponent<AraTrail>();
    this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    this.damageColliderEvents.SetActive(false);
    if (!(bool) (UnityEngine.Object) this.Trail)
      return;
    this.Trail.Clear();
    this.Trail.emit = true;
  }

  private void OnDisable()
  {
    this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    this.damageColliderEvents.SetActive(false);
  }

  public float Angle { get; set; }

  public float Speed { get; set; }

  public float Grav { get; set; }

  public void Play(float StartingHeight, float Angle, float Speed, float Grav, Health.Team team = Health.Team.Team2)
  {
    this.Trail.Clear();
    this.t.localPosition = Vector3.forward * StartingHeight;
    this.time = Grav;
    this.Angle = Angle * ((float) Math.PI / 180f);
    this.Speed = Speed;
    this.Grav = Grav * 0.0166666675f;
    this.Team = team;
    this.indicatorIcon.gameObject.SetActive(true);
    this.t.gameObject.SetActive(true);
    this.ShadowSpriteRenderer.enabled = true;
    this.StartCoroutine((IEnumerator) this.MoveRoutine());
  }

  private void ResetPosition() => this.transform.position = Vector3.zero;

  private void Update()
  {
    if ((double) Time.timeScale == 0.0 || !this.indicatorIcon.gameObject.activeSelf || Time.frameCount % 5 != 0)
      return;
    this.indicatorColor = this.indicatorColor == Color.white ? Color.red : Color.white;
    this.indicatorIcon.material.SetColor("_Color", this.indicatorColor);
  }

  private IEnumerator MoveRoutine()
  {
    GrenadeBullet grenadeBullet = this;
    Vector3 vector2 = (Vector3) Utils.RadianToVector2(grenadeBullet.Angle);
    float num1 = 5.99999952f + grenadeBullet.t.localPosition.z / 2f - (1f - Mathf.Sqrt(Mathf.Abs(grenadeBullet.t.localPosition.z)));
    float num2 = Mathf.Abs(grenadeBullet.time) / num1;
    grenadeBullet.indicatorIcon.gameObject.SetActive(true);
    Vector3 targetPosition = grenadeBullet.transform.position + vector2 * (Mathf.Abs(grenadeBullet.Speed) * num2);
    while (true)
    {
      grenadeBullet.NewPosition = new Vector3(grenadeBullet.Speed * Mathf.Cos(grenadeBullet.Angle), grenadeBullet.Speed * Mathf.Sin(grenadeBullet.Angle)) * Time.fixedDeltaTime;
      grenadeBullet.transform.position = grenadeBullet.transform.position + grenadeBullet.NewPosition;
      grenadeBullet.Grav += grenadeBullet.GravSpeed * Time.fixedDeltaTime;
      grenadeBullet.t.localPosition += Vector3.forward * grenadeBullet.Grav;
      grenadeBullet.indicatorIcon.transform.position = targetPosition + Vector3.back * 0.03f;
      if ((double) grenadeBullet.t.position.z <= 0.0)
        yield return (object) new WaitForFixedUpdate();
      else
        break;
    }
    grenadeBullet.indicatorIcon.gameObject.SetActive(false);
    grenadeBullet.t.gameObject.SetActive(false);
    grenadeBullet.ShadowSpriteRenderer.enabled = false;
    if (grenadeBullet.damageColliderRoutine != null)
      grenadeBullet.StopCoroutine(grenadeBullet.damageColliderRoutine);
    grenadeBullet.damageColliderRoutine = grenadeBullet.StartCoroutine((IEnumerator) grenadeBullet.TurnOnDamageColliderForDuration(0.2f));
    GameObject gameObject = BiomeConstants.Instance.GrenadeBulletImpact_A.Spawn();
    gameObject.transform.position = grenadeBullet.t.transform.position;
    gameObject.transform.rotation = Quaternion.identity;
    gameObject.transform.localScale = Vector3.one * 0.5f;
    CameraManager.shakeCamera(0.5f, false);
    AudioManager.Instance.PlayOneShot("event:/enemy/spit_gross_projectile");
    grenadeBullet.StartCoroutine((IEnumerator) grenadeBullet.DestroyAfterWait(1f));
  }

  private IEnumerator TurnOnDamageColliderForDuration(float duration)
  {
    this.damageColliderEvents.SetActive(true);
    yield return (object) new WaitForSeconds(duration);
    this.damageColliderEvents.SetActive(false);
  }

  private IEnumerator DestroyAfterWait(float Delay)
  {
    GrenadeBullet grenadeBullet = this;
    float num = 0.0f;
    if ((bool) (UnityEngine.Object) grenadeBullet.Trail)
    {
      num = grenadeBullet.Trail.time;
      grenadeBullet.Trail.emit = false;
    }
    yield return (object) new WaitForSeconds(Delay + num);
    grenadeBullet.gameObject.Recycle();
  }

  private void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.Team)
      return;
    component.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
  }
}
