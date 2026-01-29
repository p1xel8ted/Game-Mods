// Decompiled with JetBrains decompiler
// Type: HUD_Base
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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
  public string FaceVeryHappy = "\uF599";
  public string FaceHappy = "\uF118";
  public string FaceMeh = "\uF11A";
  public string FaceSad = "\uF119";
  public string FaceVerySad = "\uF5B4";
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
  public TextMeshProUGUI WoolAmount;
  public GameObject WoolParent;
  public GameObject warmthBarContainer;
  public WarmthBar warmthBar;
  public List<GameObject> heartSpacers = new List<GameObject>();
  public bool _offscreen;
  public Vector3 StartPos;
  public Vector3 MovePos;
  public UI_Transitions UITransition;
  public bool HidHappiness;
  public bool HidHunger;
  public bool HidFollowerCount;
  public int DiscipleCount;
  public bool showing;
  public Coroutine cLerpBarRoutine;

  public void Start()
  {
    this._offscreen = DataManager.Instance.Followers.Count <= 0;
    this.StartCoroutine((IEnumerator) this.ShakeIcon());
  }

  public void OnEnable()
  {
    this.DiscipleParent.SetActive(false);
    this.FollowerCount.SetActive(DataManager.Instance.Followers.Count > 0);
    this.FollowerAmount.text = DataManager.Instance.Followers.Count.ToString();
    this.CoinsParent.SetActive(Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD) > 0);
    this.CoinsAmount.text = Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD).ToString();
    this.WoolParent.SetActive(Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.WOOL) > 0);
    this.WoolAmount.text = Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.WOOL).ToString();
    if (!DataManager.Instance.HappinessEnabled)
    {
      this.HideHappiness();
    }
    else
    {
      int num = this.HidHappiness ? 1 : 0;
    }
  }

  public void UpdateDiscipleCount(bool Shake)
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

  public void OnDisable() => this.StopAllCoroutines();

  public void OnHappinessStateChanged(
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

  public void HideHappiness()
  {
    this.HidHappiness = true;
    this.HappinessBar.SetActive(false);
  }

  public void ShowHappiness() => this.StartCoroutine((IEnumerator) this.ShowHappinessRoutine());

  public IEnumerator ShowHappinessRoutine()
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

  public void HideHunger()
  {
    this.HidHappiness = true;
    this.HappinessBar.SetActive(false);
  }

  public void ShowHunger() => this.StartCoroutine((IEnumerator) this.ShowHungerRoutine());

  public IEnumerator ShowHungerRoutine()
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

  public IEnumerator ShakeIcon()
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

  public void UpdateFace()
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

  public void Update()
  {
    if (this.showing && (SeasonsManager.CurrentSeason != SeasonsManager.Season.Winter || DataManager.Instance.Followers.Count < 1) && (!DataManager.Instance.HasWeatherVane || (double) SeasonsManager.SEASON_NORMALISED_PROGRESS < 0.89999997615814209))
    {
      this.showing = false;
      this.warmthBarContainer.gameObject.SetActive(this.showing);
      foreach (GameObject heartSpacer in this.heartSpacers)
      {
        if (heartSpacer.activeSelf)
          heartSpacer.SetActive(this.showing);
      }
    }
    else if (!this.showing && DataManager.Instance.Followers.Count > 0 && (SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter || DataManager.Instance.HasWeatherVane && (double) SeasonsManager.SEASON_NORMALISED_PROGRESS >= 0.89999997615814209))
    {
      this.showing = true;
      this.warmthBarContainer.gameObject.SetActive(this.showing);
      foreach (GameObject heartSpacer in this.heartSpacers)
      {
        if (heartSpacer.activeSelf)
          heartSpacer.SetActive(this.showing);
      }
    }
    int itemQuantity1 = Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD);
    this.CoinsParent.SetActive(itemQuantity1 > 0);
    this.CoinsAmount.text = itemQuantity1.ToString();
    this.FollowerAmount.text = DataManager.Instance.Followers.Count.ToString();
    this.FollowerCount.SetActive(DataManager.Instance.Followers.Count > 0);
    int itemQuantity2 = Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.WOOL);
    this.WoolParent.SetActive(itemQuantity2 > 0 && (PlayerFarming.Location == FollowerLocation.DLC_ShrineRoom || PlayerFarming.Location == FollowerLocation.Blacksmith_Inside || PlayerFarming.Location == FollowerLocation.DecorationShop_Inside || PlayerFarming.Location == FollowerLocation.Flockade_Inside || PlayerFarming.Location == FollowerLocation.TarotShop_Inside));
    this.WoolAmount.text = itemQuantity2.ToString();
  }

  public IEnumerator LerpBarRoutine()
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
