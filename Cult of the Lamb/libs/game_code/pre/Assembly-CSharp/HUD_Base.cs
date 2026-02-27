// Decompiled with JetBrains decompiler
// Type: HUD_Base
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class HUD_Base : BaseMonoBehaviour
{
  public RectTransform Container;
  public GameObject HungerBar;
  public Image HungerProgressRing;
  public TextMeshProUGUI HungerIcon;
  public Image HungerRedGlow;
  public float LerpSpeed = 1f;
  public GameObject HappinessBar;
  public Image ProgressRingHappiness;
  public Image HappinessRedGlow;
  public TextMeshProUGUI HappinessIcon;
  private string FaceVeryHappy = "\uF599";
  private string FaceHappy = "\uF118";
  private string FaceMeh = "\uF11A";
  private string FaceSad = "\uF119";
  private string FaceVerySad = "\uF5B4";
  public GameObject FaithBar;
  public Image ProgressFaithBar;
  public Image FaithRedGlow;
  public TextMeshProUGUI FaithIcon;
  public Image FaithFlashRed;
  public Image FaithFlashGreen;
  public float FaithProgress;
  public Transform FaithBarBottomPosition;
  public Transform FaithBarTopPosition;
  public GameObject FollowerCount;
  public TextMeshProUGUI FollowerAmount;
  public TextMeshProUGUI FollowerIcon;
  public TextMeshProUGUI DisciplesAmount;
  public GameObject DiscipleParent;
  public TextMeshProUGUI CoinsAmount;
  public GameObject CoinsParent;
  private bool _offscreen;
  private Vector3 StartPos;
  private Vector3 MovePos;
  public UI_Transitions UITransition;
  private bool HidHappiness;
  private bool HidHunger;
  private bool HidFollowerCount;
  private int DiscipleCount;
  private Coroutine cLerpBarRoutine;

  private void Start()
  {
    this._offscreen = DataManager.Instance.Followers.Count <= 0;
    this.StartCoroutine((IEnumerator) this.ShakeIcon());
  }

  private void OnEnable()
  {
    this.DiscipleParent.SetActive(false);
    this.FollowerCount.SetActive(DataManager.Instance.Followers.Count > 0);
    this.FollowerAmount.text = DataManager.Instance.Followers.Count.ToString();
    this.CoinsParent.SetActive(Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD) > 0);
    this.CoinsAmount.text = Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD).ToString();
    if (!DataManager.Instance.HappinessEnabled)
    {
      this.HideHappiness();
    }
    else
    {
      int num = this.HidHappiness ? 1 : 0;
    }
  }

  private void UpdateDiscipleCount(bool Shake)
  {
    this.DiscipleCount = FollowerBrain.DiscipleCount();
    if (this.DiscipleCount > 0)
    {
      this.DiscipleParent.SetActive(true);
      if (Shake)
        this.DisciplesAmount.transform.DOShakeScale(1f);
      this.DisciplesAmount.text = this.DiscipleCount.ToString();
    }
    else
      this.DiscipleParent.SetActive(false);
  }

  private void OnDisable() => this.StopAllCoroutines();

  private void OnHappinessStateChanged(
    int followerID,
    float newValue,
    float oldValue,
    float change)
  {
    Debug.Log((object) "CALLED HAPPINESS");
    float num1 = 0.0f;
    float num2 = 0.0f;
    float num3 = 0.0f;
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      num1 += allBrain.Stats.Happiness;
      num2 += allBrain.Stats.Satiation + (75f - allBrain.Stats.Starvation);
      ++num3;
    }
    this.FaithProgress = num1 / (100f * num3);
    if (this.cLerpBarRoutine != null)
      this.StopCoroutine(this.cLerpBarRoutine);
    this.cLerpBarRoutine = this.StartCoroutine((IEnumerator) this.LerpBarRoutine());
    this.FaithRedGlow.color = new Color(1f, 0.0f, 0.0f, (float) ((double) this.FaithProgress * -1.0 + 0.5));
  }

  private void HideHappiness()
  {
    this.HidHappiness = true;
    this.HappinessBar.SetActive(false);
  }

  private void ShowHappiness() => this.StartCoroutine((IEnumerator) this.ShowHappinessRoutine());

  private IEnumerator ShowHappinessRoutine()
  {
    float Progress = 0.0f;
    float Duration = 1f;
    this.HappinessBar.SetActive(true);
    this.HappinessBar.transform.localScale = Vector3.zero;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      this.HappinessBar.transform.localScale = Vector3.one * Mathf.SmoothStep(0.0f, 1f, Progress / Duration);
      yield return (object) null;
    }
  }

  private void HideHunger()
  {
    this.HidHappiness = true;
    this.HappinessBar.SetActive(false);
  }

  private void ShowHunger() => this.StartCoroutine((IEnumerator) this.ShowHungerRoutine());

  private IEnumerator ShowHungerRoutine()
  {
    float Progress = 0.0f;
    float Duration = 1f;
    this.HungerBar.SetActive(true);
    this.HungerBar.transform.localScale = Vector3.zero;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      this.HungerBar.transform.localScale = Vector3.one * Mathf.SmoothStep(0.0f, 1f, Progress / Duration);
      yield return (object) null;
    }
  }

  private IEnumerator ShakeIcon()
  {
    while (true)
    {
      yield return (object) new WaitForSeconds(1f);
      this.UpdateFace();
      if ((double) this.HungerProgressRing.fillAmount < 0.25)
      {
        this.HungerIcon.transform.DOKill();
        this.HungerIcon.transform.DOShakeScale(0.75f, 0.5f);
        yield return (object) new WaitForSeconds(1f);
        this.HungerIcon.transform.localScale = Vector3.one;
      }
      if ((double) this.ProgressRingHappiness.fillAmount < 0.25)
      {
        this.HappinessIcon.transform.DOKill();
        this.HappinessIcon.transform.DOShakeScale(0.75f, 0.5f);
        yield return (object) new WaitForSeconds(1f);
        this.HappinessIcon.transform.localScale = Vector3.one;
      }
      yield return (object) null;
    }
  }

  private void UpdateFace()
  {
    string text1 = this.HappinessIcon.text;
    if ((double) this.ProgressRingHappiness.fillAmount < 0.15000000596046448)
      this.HappinessIcon.text = this.FaceVerySad;
    else if ((double) this.ProgressRingHappiness.fillAmount < 0.33000001311302185)
      this.HappinessIcon.text = this.FaceSad;
    else if ((double) this.ProgressRingHappiness.fillAmount < 0.44999998807907104)
      this.HappinessIcon.text = this.FaceMeh;
    else if ((double) this.ProgressRingHappiness.fillAmount < 0.85000002384185791)
      this.HappinessIcon.text = this.FaceHappy;
    else if ((double) this.ProgressRingHappiness.fillAmount < 1.0)
      this.HappinessIcon.text = this.FaceVeryHappy;
    string text2 = this.HappinessIcon.text;
    if (!(text1 != text2))
      return;
    this.HappinessIcon.transform.DOKill();
    this.HappinessIcon.transform.DOShakeScale(1f);
  }

  private void Update()
  {
    int itemQuantity = Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD);
    this.CoinsParent.SetActive(itemQuantity > 0);
    this.CoinsAmount.text = itemQuantity.ToString();
    this.FollowerAmount.text = DataManager.Instance.Followers.Count.ToString();
    this.FollowerCount.SetActive(DataManager.Instance.Followers.Count > 0);
  }

  private IEnumerator LerpBarRoutine()
  {
    Debug.Log((object) "Played Coroutine");
    yield return (object) new WaitForSeconds(0.2f);
    float Progress = 0.0f;
    float Duration = 0.2f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      this.ProgressFaithBar.rectTransform.localPosition = Vector3.Lerp(this.FaithBarBottomPosition.localPosition, this.FaithBarTopPosition.localPosition, Mathf.SmoothStep(0.0f, 1f, this.FaithProgress));
      yield return (object) null;
    }
  }
}
