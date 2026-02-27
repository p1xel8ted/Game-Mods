// Decompiled with JetBrains decompiler
// Type: ShowHPBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (StateMachine))]
[RequireComponent(typeof (Health))]
public class ShowHPBar : BaseMonoBehaviour
{
  private StateMachine state;
  public float zOffset = 1f;
  public bool WideBar;
  private HPBar hpBar;
  private SpriteRenderer[] barSprites;
  private float alphaHold;
  private float prevHP;
  private Health health;
  public bool OnlyShowOnHit = true;
  public bool DestroyOnDeath = true;
  private Coroutine cTweenBar;

  public float StasisXOffset => this.WideBar ? 0.8f : 0.5f;

  private void Start()
  {
    this.health = this.GetComponent<Health>();
    this.hpBar = this.health.team == Health.Team.Team2 ? (!this.WideBar ? Object.Instantiate<GameObject>(Resources.Load("Prefabs/HPBar - Team 2") as GameObject, this.transform.parent, true).GetComponent<HPBar>() : Object.Instantiate<GameObject>(Resources.Load("Prefabs/HPBar - Team 2 Wide") as GameObject, this.transform.parent, true).GetComponent<HPBar>()) : Object.Instantiate<GameObject>(Resources.Load("Prefabs/HPBar - Team 1") as GameObject, this.transform.parent, true).GetComponent<HPBar>();
    this.barSprites = this.hpBar.GetComponentsInChildren<SpriteRenderer>();
    if (this.OnlyShowOnHit)
    {
      foreach (SpriteRenderer barSprite in this.barSprites)
        barSprite.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    }
    this.health.OnHit += new Health.HitAction(this.OnHit);
    this.health.OnDie += new Health.DieAction(this.OnDie);
    this.health.OnCharmed += new Health.StasisEvent(this.OnStasisEvent);
    this.health.OnIced += new Health.StasisEvent(this.OnStasisEvent);
    this.health.OnPoisoned += new Health.StasisEvent(this.OnStasisEvent);
    Transform transform1 = this.hpBar.barInstant.transform;
    Transform transform2 = this.hpBar.barTween.transform;
    Vector3 vector3_1 = new Vector3(this.health.HP / this.health.totalHP, 1f);
    Vector3 vector3_2 = vector3_1;
    transform2.localScale = vector3_2;
    Vector3 vector3_3 = vector3_1;
    transform1.localScale = vector3_3;
  }

  private void OnStasisEvent()
  {
    this.alphaHold = 5f;
    if ((Object) this.hpBar != (Object) null && (Object) this.health != (Object) null)
      this.hpBar.barInstant.transform.localScale = new Vector3(this.health.HP / this.health.totalHP, 1f);
    if (this.cTweenBar != null)
      this.StopCoroutine(this.cTweenBar);
    if (!((Object) this.hpBar != (Object) null) || !((Object) this.hpBar.barTween != (Object) null))
      return;
    this.cTweenBar = this.StartCoroutine((IEnumerator) this.TweenBar());
  }

  public void Hide() => this.alphaHold = 0.0f;

  private void Update()
  {
    if (this.OnlyShowOnHit)
    {
      this.alphaHold -= Time.deltaTime;
      foreach (SpriteRenderer barSprite in this.barSprites)
      {
        if (!((Object) barSprite == (Object) null))
        {
          if ((double) this.alphaHold <= 0.0)
            barSprite.color += (new Color(0.0f, 0.0f, 0.0f, 0.0f) - barSprite.color) / 15f;
          else
            barSprite.color += (Color.white - barSprite.color) / 5f;
        }
      }
    }
    if (!((Object) this.health == (Object) null) && this.health.gameObject.activeSelf)
      return;
    this.DestroyHPBar();
  }

  private void LateUpdate()
  {
    if (!(bool) (Object) this.hpBar)
      return;
    this.hpBar.transform.position = this.transform.position - new Vector3(0.0f, -0.5f, this.zOffset);
  }

  private void OnDestroy()
  {
    if ((Object) this.hpBar != (Object) null && (Object) this.hpBar.gameObject != (Object) null)
      Object.Destroy((Object) this.hpBar.gameObject);
    if (!((Object) this.health != (Object) null))
      return;
    this.health.OnDie -= new Health.DieAction(this.OnDie);
    this.health.OnHit -= new Health.HitAction(this.OnHit);
    this.health.OnCharmed -= new Health.StasisEvent(this.OnStasisEvent);
    this.health.OnIced -= new Health.StasisEvent(this.OnStasisEvent);
    this.health.OnPoisoned -= new Health.StasisEvent(this.OnStasisEvent);
  }

  public void ShowHPBarShield() => this.hpBar.defence.SetActive(true);

  public void HideHPBarShield() => this.hpBar.defence.SetActive(false);

  public void DestroyHPBar()
  {
    if ((Object) this.hpBar != (Object) null && (Object) this.hpBar.gameObject != (Object) null)
      Object.Destroy((Object) this.hpBar.gameObject);
    Object.Destroy((Object) this);
  }

  private void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if (this.DestroyOnDeath)
      this.DestroyHPBar();
    else
      this.alphaHold = 0.0f;
  }

  public void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    this.alphaHold = 5f;
    if ((Object) this.hpBar != (Object) null && (Object) this.health != (Object) null)
      this.hpBar.barInstant.transform.localScale = new Vector3(this.health.HP / this.health.totalHP, 1f);
    if (this.cTweenBar != null)
      this.StopCoroutine(this.cTweenBar);
    if (!((Object) this.hpBar != (Object) null) || !((Object) this.hpBar.barTween != (Object) null))
      return;
    this.cTweenBar = this.StartCoroutine((IEnumerator) this.TweenBar());
  }

  private IEnumerator TweenBar()
  {
    yield return (object) new WaitForSeconds(0.4f);
    Vector3 Start = this.hpBar.barTween.transform.localScale;
    Vector3 Destination = this.hpBar.barInstant.transform.localScale;
    float Duration = 0.3f;
    float Progress = 0.0f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      this.hpBar.barTween.transform.localScale = Vector3.Lerp(Start, Destination, Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
      yield return (object) null;
    }
    this.hpBar.barInstant.transform.localScale = this.hpBar.barInstant.transform.localScale;
  }
}
