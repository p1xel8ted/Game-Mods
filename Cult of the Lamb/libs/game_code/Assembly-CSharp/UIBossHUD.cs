// Decompiled with JetBrains decompiler
// Type: UIBossHUD
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UIBossHUD : BaseMonoBehaviour
{
  public static UIBossHUD Instance;
  [SerializeField]
  public TMP_Text bossNameText;
  [SerializeField]
  public Image healthBar;
  [SerializeField]
  public Image healthFlashBar;
  [SerializeField]
  public CanvasGroup ailmentIconCanvasGroup;
  [SerializeField]
  public Image ailmentIcon;
  [SerializeField]
  public Sprite poisonIcon;
  [SerializeField]
  public Sprite icedIcon;
  public float targetHealthAmount;
  public Coroutine healthRoutine;
  public Health boss;
  public Color targetColor;
  public Tween ailmentTween;
  public bool hitSubscribed;

  public static void Play(Health boss, string name)
  {
    if ((Object) UIBossHUD.Instance == (Object) null)
      UIBossHUD.Instance = Object.Instantiate<UIBossHUD>(UnityEngine.Resources.Load<UIBossHUD>("Prefabs/UI/UI Boss HUD"), GameObject.FindWithTag("Canvas").transform);
    HUD_Manager.Instance.XPBarTransitions.gameObject.SetActive(false);
    UIBossHUD.Instance.boss = boss;
    UIBossHUD.Instance.boss.OnHit -= new Health.HitAction(UIBossHUD.Instance.OnBossHit);
    UIBossHUD.Instance.boss.OnHit += new Health.HitAction(UIBossHUD.Instance.OnBossHit);
    UIBossHUD.Instance.hitSubscribed = true;
    UIBossHUD.Instance.bossNameText.text = name;
    UIBossHUD.Instance.bossNameText.isRightToLeftText = LocalizeIntegration.IsArabic();
  }

  public static void Hide()
  {
    if ((Object) UIBossHUD.Instance != (Object) null)
    {
      Object.Destroy((Object) UIBossHUD.Instance.gameObject);
      UIBossHUD.Instance = (UIBossHUD) null;
    }
    HUD_Manager.Instance.XPBarTransitions.gameObject.SetActive(true);
  }

  public void OnEnable()
  {
    if (!((Object) this.boss != (Object) null) || this.hitSubscribed)
      return;
    this.boss.OnHit -= new Health.HitAction(this.OnBossHit);
    this.boss.OnHit += new Health.HitAction(this.OnBossHit);
    this.hitSubscribed = true;
  }

  public void OnDisable()
  {
    if (!(bool) (Object) this.boss)
      return;
    this.boss.OnHit -= new Health.HitAction(this.OnBossHit);
    this.hitSubscribed = false;
  }

  public void Update()
  {
    if ((Object) this.boss != (Object) null && !this.hitSubscribed)
    {
      this.boss.OnHit -= new Health.HitAction(this.OnBossHit);
      this.boss.OnHit += new Health.HitAction(this.OnBossHit);
      this.hitSubscribed = true;
    }
    if ((double) this.ailmentIconCanvasGroup.alpha == 0.0 && (this.boss.IsPoisoned || this.boss.IsIced))
    {
      if (this.ailmentTween != null && this.ailmentTween.active)
        this.ailmentTween.Complete();
      this.ailmentIconCanvasGroup.alpha = 0.0f;
      this.ailmentTween = (Tween) this.ailmentIconCanvasGroup.DOFade(1f, 0.2f);
    }
    else if ((double) this.ailmentIconCanvasGroup.alpha == 1.0 && !this.boss.IsPoisoned && !this.boss.IsIced)
    {
      if (this.ailmentTween != null && this.ailmentTween.active)
        this.ailmentTween.Complete();
      this.ailmentIconCanvasGroup.alpha = 1f;
      this.ailmentTween = (Tween) this.ailmentIconCanvasGroup.DOFade(0.0f, 0.2f);
    }
    this.targetColor = Color.red;
    if (this.boss.IsPoisoned)
    {
      this.targetColor = Color.green;
      this.ailmentIcon.sprite = this.poisonIcon;
    }
    else if (this.boss.IsIced)
    {
      this.targetColor = Color.cyan;
      this.ailmentIcon.sprite = this.icedIcon;
    }
    this.healthBar.color = Color.Lerp(this.healthBar.color, this.targetColor, 5f * Time.deltaTime);
    this.ailmentIcon.color = this.targetColor;
  }

  public void UpdateName(string name)
  {
    this.bossNameText.text = name;
    UIBossHUD.Instance.bossNameText.isRightToLeftText = LocalizeIntegration.IsArabic();
  }

  public void OnBossHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    if (!(bool) (Object) this.boss)
      return;
    this.HealthUpdated(this.boss.HP / this.boss.totalHP);
  }

  public void HealthUpdated(float normHealthAmount)
  {
    if (!this.gameObject.activeInHierarchy || (double) normHealthAmount == (double) this.targetHealthAmount)
      return;
    if (this.healthRoutine != null)
    {
      this.StopCoroutine(this.healthRoutine);
      this.ForceHealthAmount(this.targetHealthAmount);
    }
    this.targetHealthAmount = normHealthAmount;
    this.healthRoutine = this.StartCoroutine((IEnumerator) this.HealthBarUpdated(normHealthAmount));
  }

  public void ForceHealthAmount(float normHealthAmount)
  {
    this.healthBar.fillAmount = normHealthAmount;
    this.healthFlashBar.fillAmount = normHealthAmount;
    this.targetHealthAmount = normHealthAmount;
  }

  public void ForceHealthAmount(float normHealthAmount, float updateTime)
  {
    if (!this.gameObject.activeInHierarchy)
      return;
    if (this.healthRoutine != null)
      this.StopCoroutine(this.healthRoutine);
    this.healthRoutine = this.StartCoroutine((IEnumerator) this.HealthBarUpdated(normHealthAmount, updateTime));
  }

  public IEnumerator HealthBarUpdated(float normAmount)
  {
    this.healthBar.fillAmount = normAmount;
    yield return (object) new WaitForSeconds(0.3f);
    float fromAmount = this.healthFlashBar.fillAmount;
    float t = 0.0f;
    while ((double) t < 0.25)
    {
      float t1 = t / 0.25f;
      this.healthFlashBar.fillAmount = Mathf.Lerp(fromAmount, normAmount, t1);
      t += Time.deltaTime;
      yield return (object) null;
    }
    this.healthFlashBar.fillAmount = normAmount;
    this.healthRoutine = (Coroutine) null;
  }

  public IEnumerator HealthBarUpdated(float normAmount, float duration)
  {
    float fromAmount = this.healthBar.fillAmount;
    float t = 0.0f;
    while ((double) t < (double) duration)
    {
      float t1 = t / duration;
      this.healthBar.fillAmount = Mathf.Lerp(fromAmount, normAmount, t1);
      t += Time.deltaTime;
      yield return (object) null;
    }
    this.healthFlashBar.fillAmount = normAmount;
    this.healthBar.fillAmount = normAmount;
    this.healthRoutine = (Coroutine) null;
  }

  public void Shake()
  {
    this.transform.DOKill();
    this.transform.DOShakePosition(1f, Vector3.right * 20f);
  }
}
