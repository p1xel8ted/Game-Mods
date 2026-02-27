// Decompiled with JetBrains decompiler
// Type: DoubleUIBossHUD
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class DoubleUIBossHUD : BaseMonoBehaviour
{
  public static DoubleUIBossHUD Instance;
  [SerializeField]
  public TMP_Text bossNameText1;
  [SerializeField]
  public TMP_Text bossNameText2;
  [SerializeField]
  public Image healthBar1;
  [SerializeField]
  public Image healthFlashBar1;
  [SerializeField]
  public Image healthBar2;
  [SerializeField]
  public Image healthFlashBar2;
  [SerializeField]
  public CanvasGroup ailmentIconCanvasGroup1;
  [SerializeField]
  public CanvasGroup ailmentIconCanvasGroup2;
  [SerializeField]
  public Image ailmentIcon1;
  [SerializeField]
  public Image ailmentIcon2;
  [SerializeField]
  public Sprite poisonIcon;
  [SerializeField]
  public Sprite icedIcon;
  public float targetHealthAmount1;
  public float targetHealthAmount2;
  public Coroutine healthRoutine1;
  public Coroutine healthRoutine2;
  public Health boss1;
  public Health boss2;
  public Color targetColor1;
  public Color targetColor2;
  public Tween ailmentTween1;
  public Tween ailmentTween2;

  public static void Play(Health boss1, string name1, Health boss2, string name2)
  {
    if ((Object) DoubleUIBossHUD.Instance == (Object) null)
      DoubleUIBossHUD.Instance = Object.Instantiate<DoubleUIBossHUD>(UnityEngine.Resources.Load<DoubleUIBossHUD>("Prefabs/UI/Double UI Boss HUD"), GameObject.FindWithTag("Canvas").transform);
    HUD_Manager.Instance.XPBarTransitions.gameObject.SetActive(false);
    DoubleUIBossHUD.Instance.boss1 = boss1;
    DoubleUIBossHUD.Instance.boss2 = boss2;
    DoubleUIBossHUD.Instance.boss1.OnHit += new Health.HitAction(DoubleUIBossHUD.Instance.OnBoss1Hit);
    DoubleUIBossHUD.Instance.boss2.OnHit += new Health.HitAction(DoubleUIBossHUD.Instance.OnBoss2Hit);
    DoubleUIBossHUD.Instance.boss1.OnHitForceBossHUD += new Health.HitAction(DoubleUIBossHUD.Instance.OnBoss1DeadHit);
    DoubleUIBossHUD.Instance.boss2.OnHitForceBossHUD += new Health.HitAction(DoubleUIBossHUD.Instance.OnBoss2DeadHit);
    DoubleUIBossHUD.Instance.bossNameText1.text = name1;
    DoubleUIBossHUD.Instance.bossNameText2.text = name2;
  }

  public static void Hide()
  {
    if ((Object) DoubleUIBossHUD.Instance != (Object) null)
    {
      Object.Destroy((Object) DoubleUIBossHUD.Instance.gameObject);
      DoubleUIBossHUD.Instance = (DoubleUIBossHUD) null;
    }
    HUD_Manager.Instance.XPBarTransitions.gameObject.SetActive(true);
  }

  public void OnDisable()
  {
    if ((bool) (Object) this.boss1)
    {
      this.boss1.OnHit -= new Health.HitAction(this.OnBoss1Hit);
      this.boss1.OnHitForceBossHUD -= new Health.HitAction(this.OnBoss1DeadHit);
      this.ForceHealthAmount1(0.0f);
    }
    if (!(bool) (Object) this.boss2)
      return;
    this.boss2.OnHit -= new Health.HitAction(this.OnBoss2Hit);
    this.boss2.OnHitForceBossHUD -= new Health.HitAction(this.OnBoss2DeadHit);
    this.ForceHealthAmount2(0.0f);
  }

  public void Update()
  {
    if ((double) this.ailmentIconCanvasGroup1.alpha == 0.0 && (this.boss1.IsPoisoned || this.boss1.IsIced))
    {
      if (this.ailmentTween1 != null && this.ailmentTween1.active)
        this.ailmentTween1.Complete();
      this.ailmentIconCanvasGroup1.alpha = 0.0f;
      this.ailmentTween1 = (Tween) this.ailmentIconCanvasGroup1.DOFade(1f, 0.2f);
    }
    else if ((double) this.ailmentIconCanvasGroup1.alpha == 1.0 && !this.boss1.IsPoisoned && !this.boss1.IsIced)
    {
      if (this.ailmentTween1 != null && this.ailmentTween1.active)
        this.ailmentTween1.Complete();
      this.ailmentIconCanvasGroup1.alpha = 1f;
      this.ailmentTween1 = (Tween) this.ailmentIconCanvasGroup1.DOFade(0.0f, 0.2f);
    }
    this.targetColor1 = Color.red;
    if (this.boss1.IsPoisoned)
    {
      this.targetColor1 = Color.green;
      this.ailmentIcon1.sprite = this.poisonIcon;
    }
    else if (this.boss1.IsIced)
    {
      this.targetColor1 = Color.cyan;
      this.ailmentIcon1.sprite = this.icedIcon;
    }
    this.healthBar1.color = Color.Lerp(this.healthBar1.color, this.targetColor1, 5f * Time.deltaTime);
    this.ailmentIcon1.color = this.targetColor1;
    if ((double) this.ailmentIconCanvasGroup2.alpha == 0.0 && (this.boss2.IsPoisoned || this.boss2.IsIced))
    {
      if (this.ailmentTween2 != null && this.ailmentTween2.active)
        this.ailmentTween2.Complete();
      this.ailmentIconCanvasGroup2.alpha = 0.0f;
      this.ailmentTween2 = (Tween) this.ailmentIconCanvasGroup2.DOFade(1f, 0.2f);
    }
    else if ((double) this.ailmentIconCanvasGroup2.alpha == 1.0 && !this.boss2.IsPoisoned && !this.boss2.IsIced)
    {
      if (this.ailmentTween2 != null && this.ailmentTween2.active)
        this.ailmentTween2.Complete();
      this.ailmentIconCanvasGroup2.alpha = 1f;
      this.ailmentTween2 = (Tween) this.ailmentIconCanvasGroup2.DOFade(0.0f, 0.2f);
    }
    this.targetColor2 = Color.red;
    if (this.boss2.IsPoisoned)
    {
      this.targetColor2 = Color.green;
      this.ailmentIcon2.sprite = this.poisonIcon;
    }
    else if (this.boss2.IsIced)
    {
      this.targetColor2 = Color.cyan;
      this.ailmentIcon2.sprite = this.icedIcon;
    }
    this.healthBar2.color = Color.Lerp(this.healthBar2.color, this.targetColor2, 5f * Time.deltaTime);
    this.ailmentIcon2.color = this.targetColor2;
  }

  public void UpdateName(string name1, string name2)
  {
    this.bossNameText1.text = name1;
    this.bossNameText2.text = name2;
  }

  public void OnBoss1Hit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    if (!(bool) (Object) this.boss1)
      return;
    this.HealthUpdated1(this.boss1.HP / this.boss1.totalHP);
  }

  public void OnBoss2Hit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    if (!(bool) (Object) this.boss2)
      return;
    this.HealthUpdated2(this.boss2.HP / this.boss2.totalHP);
  }

  public void OnBoss1DeadHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    this.ForceHealthAmount1(0.0f);
  }

  public void OnBoss2DeadHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    this.ForceHealthAmount2(0.0f);
  }

  public void HealthUpdated1(float normHealthAmount)
  {
    if (!this.gameObject.activeInHierarchy || (double) normHealthAmount == (double) this.targetHealthAmount1)
      return;
    if (this.healthRoutine1 != null)
    {
      this.StopCoroutine(this.healthRoutine1);
      this.ForceHealthAmount1(this.targetHealthAmount1);
    }
    this.targetHealthAmount1 = normHealthAmount;
    this.healthRoutine1 = this.StartCoroutine(this.HealthBarUpdated1(normHealthAmount));
  }

  public void HealthUpdated1WithTime(float normHealthAmount, float time)
  {
    if (!this.gameObject.activeInHierarchy || (double) normHealthAmount == (double) this.targetHealthAmount1)
      return;
    if (this.healthRoutine1 != null)
    {
      this.StopCoroutine(this.healthRoutine1);
      this.ForceHealthAmount1(this.targetHealthAmount1);
    }
    this.targetHealthAmount1 = normHealthAmount;
    this.healthRoutine1 = this.StartCoroutine(this.HealthBarUpdated1WithTime(normHealthAmount, time));
  }

  public void HealthUpdated2(float normHealthAmount)
  {
    if (!this.gameObject.activeInHierarchy || (double) normHealthAmount == (double) this.targetHealthAmount1)
      return;
    if (this.healthRoutine2 != null)
    {
      this.StopCoroutine(this.healthRoutine2);
      this.ForceHealthAmount2(this.targetHealthAmount2);
    }
    this.targetHealthAmount2 = normHealthAmount;
    this.healthRoutine2 = this.StartCoroutine(this.HealthBarUpdated2(normHealthAmount));
  }

  public void HealthUpdated2WithTime(float normHealthAmount, float time)
  {
    if (!this.gameObject.activeInHierarchy || (double) normHealthAmount == (double) this.targetHealthAmount1)
      return;
    if (this.healthRoutine2 != null)
    {
      this.StopCoroutine(this.healthRoutine2);
      this.ForceHealthAmount2(this.targetHealthAmount2);
    }
    this.targetHealthAmount2 = normHealthAmount;
    this.healthRoutine2 = this.StartCoroutine(this.HealthBarUpdated2WithTime(normHealthAmount, time));
  }

  public void ForceHealthAmount1(float normHealthAmount)
  {
    if (this.healthRoutine1 != null)
      this.StopCoroutine(this.healthRoutine1);
    this.healthBar1.fillAmount = normHealthAmount;
    this.healthFlashBar1.fillAmount = normHealthAmount;
    this.targetHealthAmount1 = normHealthAmount;
  }

  public void ForceHealthAmount2(float normHealthAmount)
  {
    if (this.healthRoutine2 != null)
      this.StopCoroutine(this.healthRoutine2);
    this.healthBar2.fillAmount = normHealthAmount;
    this.healthFlashBar2.fillAmount = normHealthAmount;
    this.targetHealthAmount2 = normHealthAmount;
  }

  public IEnumerator HealthBarUpdated1(float normAmount)
  {
    this.healthBar1.fillAmount = normAmount;
    yield return (object) new WaitForSeconds(0.3f);
    float fromAmount = this.healthFlashBar1.fillAmount;
    float t = 0.0f;
    while ((double) t < 0.25)
    {
      float t1 = t / 0.25f;
      this.healthFlashBar1.fillAmount = Mathf.Lerp(fromAmount, normAmount, t1);
      t += Time.deltaTime;
      yield return (object) null;
    }
    this.healthFlashBar1.fillAmount = normAmount;
    this.healthRoutine1 = (Coroutine) null;
  }

  public IEnumerator HealthBarUpdated1WithTime(float normAmount, float time)
  {
    this.healthBar1.fillAmount = normAmount;
    yield return (object) new WaitForSeconds(0.3f);
    float fromAmount = this.healthFlashBar1.fillAmount;
    float t = 0.0f;
    while ((double) t < (double) time)
    {
      float t1 = t / time;
      this.healthFlashBar1.fillAmount = Mathf.Lerp(fromAmount, normAmount, t1);
      t += Time.deltaTime;
      yield return (object) null;
    }
    this.healthFlashBar1.fillAmount = normAmount;
    this.healthRoutine1 = (Coroutine) null;
  }

  public IEnumerator HealthBarUpdated2(float normAmount)
  {
    this.healthBar2.fillAmount = normAmount;
    yield return (object) new WaitForSeconds(0.3f);
    float fromAmount = this.healthFlashBar2.fillAmount;
    float t = 0.0f;
    while ((double) t < 0.25)
    {
      float t1 = t / 0.25f;
      this.healthFlashBar2.fillAmount = Mathf.Lerp(fromAmount, normAmount, t1);
      t += Time.deltaTime;
      yield return (object) null;
    }
    this.healthFlashBar2.fillAmount = normAmount;
    this.healthRoutine2 = (Coroutine) null;
  }

  public IEnumerator HealthBarUpdated2WithTime(float normAmount, float time)
  {
    this.healthBar2.fillAmount = normAmount;
    yield return (object) new WaitForSeconds(0.3f);
    float fromAmount = this.healthFlashBar2.fillAmount;
    float t = 0.0f;
    while ((double) t < (double) time)
    {
      float t1 = t / time;
      this.healthFlashBar2.fillAmount = Mathf.Lerp(fromAmount, normAmount, t1);
      t += Time.deltaTime;
      yield return (object) null;
    }
    this.healthFlashBar2.fillAmount = normAmount;
    this.healthRoutine2 = (Coroutine) null;
  }

  public void OnBoss1Phase() => this.HealthUpdated1WithTime(1f, 2f);

  public void OnBoss2Phase() => this.HealthUpdated2WithTime(1f, 2f);
}
