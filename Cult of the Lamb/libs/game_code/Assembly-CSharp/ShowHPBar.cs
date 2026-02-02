// Decompiled with JetBrains decompiler
// Type: ShowHPBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (StateMachine))]
[RequireComponent(typeof (Health))]
public class ShowHPBar : BaseMonoBehaviour
{
  public StateMachine state;
  public float zOffset = 1f;
  public bool WideBar;
  public HPBar hpBar;
  public SpriteRenderer[] barSprites;
  public float alphaHold;
  public float prevHP;
  public Health health;
  public bool OnlyShowOnHit = true;
  public bool DestroyOnDeath = true;
  public static GameObject HPBarTeam1;
  public static GameObject HPBarTeam2;
  public static GameObject HPBarTeam2Wide;
  public bool initialised;
  public bool initialisationRecovered;
  public Coroutine cTweenBar;

  public float StasisXOffset => this.WideBar ? 0.8f : 0.5f;

  public void Start() => this.Init();

  public static void Preload(int count)
  {
    if ((Object) ShowHPBar.HPBarTeam1 == (Object) null)
    {
      ShowHPBar.HPBarTeam1 = Resources.Load("Prefabs/HPBar - Team 1") as GameObject;
      ShowHPBar.HPBarTeam1.CreatePool(count);
    }
    if ((Object) ShowHPBar.HPBarTeam2Wide == (Object) null)
    {
      ShowHPBar.HPBarTeam2Wide = Resources.Load("Prefabs/HPBar - Team 2 Wide") as GameObject;
      ShowHPBar.HPBarTeam2Wide.CreatePool(count);
    }
    if (!((Object) ShowHPBar.HPBarTeam2 == (Object) null))
      return;
    ShowHPBar.HPBarTeam2 = Resources.Load("Prefabs/HPBar - Team 2") as GameObject;
    ShowHPBar.HPBarTeam2.CreatePool(count);
  }

  public void Init()
  {
    if (this.initialised)
      return;
    this.initialised = true;
    this.health = this.GetComponent<Health>();
    this.InstantiateHpBar();
    this.health.OnHit += new Health.HitAction(this.OnHit);
    this.health.OnDie += new Health.DieAction(this.OnDie);
    this.health.OnCharmed += new Health.StasisEvent(this.OnStasisEvent);
    this.health.OnIced += new Health.StasisEvent(this.OnStasisEvent);
    this.health.OnPoisoned += new Health.StasisEvent(this.OnStasisEvent);
  }

  public void InstantiateHpBar()
  {
    if (!this.initialised)
      return;
    switch (this.health.team)
    {
      case Health.Team.Team2:
      case Health.Team.KillAll:
        if (this.WideBar)
        {
          if ((Object) ShowHPBar.HPBarTeam2Wide == (Object) null)
            ShowHPBar.HPBarTeam2Wide = Resources.Load("Prefabs/HPBar - Team 2 Wide") as GameObject;
          this.hpBar = ShowHPBar.HPBarTeam2Wide.SpawnUI(this.transform.parent).GetComponent<HPBar>();
          break;
        }
        if ((Object) ShowHPBar.HPBarTeam2 == (Object) null)
          ShowHPBar.HPBarTeam2 = Resources.Load("Prefabs/HPBar - Team 2") as GameObject;
        this.hpBar = ShowHPBar.HPBarTeam2.SpawnUI(this.transform.parent).GetComponent<HPBar>();
        break;
      default:
        if ((Object) ShowHPBar.HPBarTeam1 == (Object) null)
          ShowHPBar.HPBarTeam1 = Resources.Load("Prefabs/HPBar - Team 1") as GameObject;
        this.hpBar = ShowHPBar.HPBarTeam1.SpawnUI(this.transform.parent).GetComponent<HPBar>();
        break;
    }
    this.barSprites = this.hpBar.GetComponentsInChildren<SpriteRenderer>();
    if (this.OnlyShowOnHit)
    {
      foreach (SpriteRenderer barSprite in this.barSprites)
        barSprite.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    }
    Transform transform1 = this.hpBar.barInstant.transform;
    Transform transform2 = this.hpBar.barTween.transform;
    Vector3 vector3_1 = new Vector3(this.health.HP / this.health.totalHP, 1f);
    Vector3 vector3_2 = vector3_1;
    transform2.localScale = vector3_2;
    Vector3 vector3_3 = vector3_1;
    transform1.localScale = vector3_3;
    this.hpBar.transform.Rotate(new Vector3(-45f, 0.0f, 0.0f), Space.Self);
  }

  public void OnStasisEvent()
  {
    this.alphaHold = 5f;
    if ((Object) this.hpBar != (Object) null && (Object) this.health != (Object) null)
    {
      if (!this.hpBar.gameObject.activeInHierarchy)
        this.hpBar.gameObject.SetActive(true);
      this.hpBar.barInstant.transform.localScale = new Vector3(this.health.HP / this.health.totalHP, 1f);
    }
    else if (this.initialised && !this.initialisationRecovered)
    {
      this.InstantiateHpBar();
      this.initialisationRecovered = true;
    }
    if (this.cTweenBar != null)
      this.StopCoroutine(this.cTweenBar);
    if (!((Object) this.hpBar != (Object) null) || !((Object) this.hpBar.barTween != (Object) null))
      return;
    this.cTweenBar = this.StartCoroutine((IEnumerator) this.TweenBar());
  }

  public void Hide() => this.alphaHold = 0.0f;

  public void Update()
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

  public void LateUpdate()
  {
    if (!(bool) (Object) this.hpBar)
      return;
    this.hpBar.transform.position = this.transform.position - new Vector3(0.0f, -0.5f, this.zOffset);
  }

  public void OnEnable()
  {
    if (!((Object) this.health != (Object) null) || !this.initialised)
      return;
    this.health.OnDie -= new Health.DieAction(this.OnDie);
    this.health.OnHit -= new Health.HitAction(this.OnHit);
    this.health.OnCharmed -= new Health.StasisEvent(this.OnStasisEvent);
    this.health.OnIced -= new Health.StasisEvent(this.OnStasisEvent);
    this.health.OnPoisoned -= new Health.StasisEvent(this.OnStasisEvent);
    this.health.OnHit += new Health.HitAction(this.OnHit);
    this.health.OnDie += new Health.DieAction(this.OnDie);
    this.health.OnCharmed += new Health.StasisEvent(this.OnStasisEvent);
    this.health.OnIced += new Health.StasisEvent(this.OnStasisEvent);
    this.health.OnPoisoned += new Health.StasisEvent(this.OnStasisEvent);
  }

  public void OnDisable()
  {
    if ((Object) this.hpBar != (Object) null)
      this.hpBar.defence.SetActive(false);
    if (!((Object) this.health != (Object) null))
      return;
    this.health.OnDie -= new Health.DieAction(this.OnDie);
    this.health.OnHit -= new Health.HitAction(this.OnHit);
    this.health.OnCharmed -= new Health.StasisEvent(this.OnStasisEvent);
    this.health.OnIced -= new Health.StasisEvent(this.OnStasisEvent);
    this.health.OnPoisoned -= new Health.StasisEvent(this.OnStasisEvent);
  }

  public void OnDestroy()
  {
    if (!((Object) this.hpBar != (Object) null) || !((Object) this.hpBar.gameObject != (Object) null) || !this.hpBar.gameObject.activeInHierarchy)
      return;
    if (this.DestroyOnDeath)
      this.DestroyHPBar();
    else
      this.alphaHold = 0.0f;
  }

  public void ShowHPBarShield() => this.hpBar.defence.SetActive(true);

  public void HideHPBarShield() => this.hpBar.defence.SetActive(false);

  public void DestroyHPBar()
  {
    if (this.cTweenBar != null)
      this.StopCoroutine(this.cTweenBar);
    if (!((Object) this.hpBar != (Object) null) || !((Object) this.hpBar.gameObject != (Object) null))
      return;
    if ((Object) this.hpBar.groupIndicator != (Object) null)
      this.hpBar.groupIndicator.gameObject.SetActive(false);
    if ((Object) this.hpBar.defence != (Object) null)
      this.hpBar.defence.gameObject.SetActive(false);
    ObjectPool.Recycle(this.hpBar.gameObject);
  }

  public void OnDie(
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
    {
      if (!this.hpBar.gameObject.activeInHierarchy)
        this.hpBar.gameObject.SetActive(true);
      this.hpBar.barInstant.transform.localScale = new Vector3(this.health.HP / this.health.totalHP, 1f);
    }
    else if (this.initialised && !this.initialisationRecovered)
    {
      this.InstantiateHpBar();
      this.initialisationRecovered = true;
    }
    if (this.cTweenBar != null)
      this.StopCoroutine(this.cTweenBar);
    if (!((Object) this.hpBar != (Object) null) || !((Object) this.hpBar.barTween != (Object) null))
      return;
    this.cTweenBar = this.StartCoroutine((IEnumerator) this.TweenBar());
  }

  public IEnumerator TweenBar(float delay = 0.4f)
  {
    yield return (object) new WaitForSeconds(delay);
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

  public void ForceUpdate()
  {
    this.hpBar.barInstant.transform.localScale = new Vector3(this.health.HP / this.health.totalHP, 1f);
  }
}
