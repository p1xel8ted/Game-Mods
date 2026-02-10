// Decompiled with JetBrains decompiler
// Type: UIChoiceCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UIChoiceCard : BaseMonoBehaviour
{
  public TextMeshProUGUI TitleText;
  public TextMeshProUGUI SubtitleText;
  public CanvasGroup canvasGroup;
  public ChoiceReward Reward;
  public SkeletonGraphic FollowerSpine;
  public InventoryItemDisplay ItemDisplay;
  public TextMeshProUGUI PositiveTraitText;
  public TextMeshProUGUI PositiveTraitDescription;
  public TextMeshProUGUI NegativeTraitText;
  public TextMeshProUGUI NegativeTraitDescription;
  public TextMeshProUGUI CostText;
  public RectTransform CostContainer;
  public Image CostPrefab;
  public Vector3 InitPosition;
  public List<Image> Costs;
  public float xSpeed;
  public float xOffset;
  public Coroutine cShake;

  public void Play(ChoiceReward Reward, float Delay)
  {
    this.Reward = Reward;
    this.TitleText.text = Reward.Title;
    this.SubtitleText.text = Reward.Locked ? "" : Reward.SubTitle;
    this.SetUpSpine();
    this.SetItemDisplay();
    this.SetTraits();
    this.SetUpCosts();
    this.StartCoroutine((IEnumerator) this.PlayRoutine(Delay));
    this.InitPosition = this.CostText.transform.localPosition;
  }

  public void OnHighlighted() => this.SetCostColours(true);

  public void OnDehighlighted() => this.SetCostColours(false);

  public void SetUpCosts()
  {
    this.CostText.text = this.Reward.Locked ? this.Reward.SubTitle : (this.Reward.Cost == 0 ? "" : $"{FontImageNames.GetIconByType(this.Reward.Currency)}x{this.Reward.Cost.ToString()}");
    if (Inventory.GetItemQuantity((int) this.Reward.Currency) >= this.Reward.Cost)
      return;
    this.CostText.color = Color.red;
  }

  public void SetCostColours(bool PreviewPlay)
  {
  }

  public void SetItemDisplay()
  {
    if (this.Reward.Type == ChoiceReward.RewardType.Resource)
      this.ItemDisplay.SetImage(this.Reward.ItemType);
    else
      this.ItemDisplay.gameObject.SetActive(false);
  }

  public void SetTraits()
  {
    if (this.Reward.Type == ChoiceReward.RewardType.Follower)
    {
      this.PositiveTraitText.text = FollowerTrait.GetLocalizedTitle(this.Reward.FollowerInfo.Traits[0]);
      this.PositiveTraitDescription.text = FollowerTrait.GetLocalizedDescription(this.Reward.FollowerInfo.Traits[0]);
      this.NegativeTraitText.text = FollowerTrait.GetLocalizedTitle(this.Reward.FollowerInfo.Traits[1]);
      this.NegativeTraitDescription.text = FollowerTrait.GetLocalizedDescription(this.Reward.FollowerInfo.Traits[1]);
      this.PositiveTraitText.gameObject.SetActive(true);
      this.PositiveTraitDescription.gameObject.SetActive(true);
      this.NegativeTraitText.gameObject.SetActive(true);
      this.NegativeTraitDescription.gameObject.SetActive(true);
    }
    else
    {
      this.PositiveTraitText.gameObject.SetActive(false);
      this.PositiveTraitDescription.gameObject.SetActive(false);
      this.NegativeTraitText.gameObject.SetActive(false);
      this.NegativeTraitDescription.gameObject.SetActive(false);
    }
  }

  public void SetUpSpine()
  {
    if (this.Reward.Type == ChoiceReward.RewardType.Follower)
    {
      this.FollowerSpine.Skeleton.SetSkin(this.Reward.FollowerInfo.SkinName);
      this.FollowerSpine.timeScale = UnityEngine.Random.Range(0.9f, 1.2f);
      WorshipperData.SkinAndData colourData = WorshipperData.Instance.GetColourData(this.Reward.FollowerInfo.SkinName);
      if (colourData != null)
      {
        foreach (WorshipperData.SlotAndColor slotAndColour in colourData.SlotAndColours[Mathf.Clamp(this.Reward.FollowerInfo.SkinColour, 0, colourData.SlotAndColours.Count - 1)].SlotAndColours)
        {
          Slot slot = this.FollowerSpine.Skeleton.FindSlot(slotAndColour.Slot);
          if (slot != null)
            slot.SetColor(slotAndColour.color);
        }
      }
      this.FollowerSpine.AnimationState.SetAnimation(0, "spawn-in", true);
      if ((double) UnityEngine.Random.value < 0.5)
        this.FollowerSpine.AnimationState.AddAnimation(0, "wave", true, 0.0f);
      else
        this.FollowerSpine.AnimationState.AddAnimation(0, "worship", true, 0.0f);
    }
    else
      this.FollowerSpine.gameObject.SetActive(false);
  }

  public IEnumerator PlayRoutine(float Delay)
  {
    this.canvasGroup.alpha = 0.0f;
    yield return (object) new WaitForSecondsRealtime(Delay);
    float Progress = 0.0f;
    float Duration = 0.5f;
    while ((double) (Progress += Time.unscaledDeltaTime) < 0.5)
    {
      this.canvasGroup.alpha = Progress / Duration;
      yield return (object) null;
    }
    this.canvasGroup.alpha = 1f;
  }

  public bool CanAfford()
  {
    return Inventory.GetItemQuantity((int) this.Reward.Currency) >= this.Reward.Cost;
  }

  public void Shake()
  {
    if (this.cShake != null)
      this.StopCoroutine(this.cShake);
    this.cShake = this.StartCoroutine((IEnumerator) this.ShakeRoutine());
  }

  public IEnumerator ShakeRoutine()
  {
    Debug.Log((object) "SHAKE!");
    this.xSpeed = 10f;
    this.CostText.transform.localPosition = this.InitPosition;
    Vector3 Direction = (double) UnityEngine.Random.value < 0.5 ? Vector3.left : -Vector3.right;
    while (true)
    {
      this.xSpeed += (float) ((0.0 - (double) this.xOffset) * 0.20000000298023224);
      this.xOffset += (this.xSpeed *= 0.8f);
      this.CostText.transform.localPosition = this.InitPosition + Direction * (this.xOffset * (Time.unscaledDeltaTime * 60f));
      yield return (object) null;
    }
  }

  public bool Play(System.Action CancelCallBack) => true;
}
