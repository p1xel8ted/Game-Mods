// Decompiled with JetBrains decompiler
// Type: UIKitchenMealMenuIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class UIKitchenMealMenuIcon : 
  BaseMonoBehaviour,
  ISelectHandler,
  IEventSystemHandler,
  IDeselectHandler
{
  public CanvasGroup canvasGroup;
  public InventoryItem.ITEM_TYPE Type;
  public TextMeshProUGUI CostText;
  public Image Icon;
  public Image IconSelected;
  public Transform ShakeObject;
  public Material BlackAndWhiteMaterial;
  public GameObject NewIcon;
  public Image FlashIcon;
  public Image CookingRing;
  public Image HungerBar;
  public Image FaithBar;
  public GameObject requiresFuelText;
  [CompilerGenerated]
  public bool \u003CCanAfford\u003Ek__BackingField;
  [CompilerGenerated]
  public int \u003CIngredientVariant\u003Ek__BackingField;
  public Coroutine cShake;
  public Vector2 ShakeVector;
  public Coroutine cSelectionRoutine;

  public bool CanAfford
  {
    get => this.\u003CCanAfford\u003Ek__BackingField;
    set => this.\u003CCanAfford\u003Ek__BackingField = value;
  }

  public int IngredientVariant
  {
    get => this.\u003CIngredientVariant\u003Ek__BackingField;
    set => this.\u003CIngredientVariant\u003Ek__BackingField = value;
  }

  public void Play(
    InventoryItem.ITEM_TYPE Type,
    int ingredientVariant,
    float Delay,
    bool queued = false,
    int queuedIndex = 0,
    bool fadeIn = true)
  {
    this.IconSelected.enabled = false;
    this.gameObject.SetActive(true);
    this.Type = Type;
    this.IngredientVariant = ingredientVariant;
    this.Icon.sprite = CookingData.GetIcon(Type);
    this.CostText.text = this.GetCostText(Type, ingredientVariant);
    this.NewIcon.SetActive(false);
    this.FlashIcon.gameObject.SetActive(false);
    this.canvasGroup = this.GetComponent<CanvasGroup>();
    if (fadeIn)
      this.StartCoroutine(this.FadeIn(Delay));
    else
      this.canvasGroup.alpha = 1f;
    bool flag1 = queuedIndex == 0 & queued;
    bool flag2 = true;
    this.CanAfford = this.CheckCanAfford(Type, ingredientVariant) | queued;
    if (!this.CanAfford && !queued || queued && (!flag2 || !flag1))
    {
      this.Icon.material = this.BlackAndWhiteMaterial;
      this.Icon.material.SetFloat("_GrayscaleLerpFade", 1f);
      if ((bool) (Object) this.requiresFuelText)
        this.requiresFuelText.SetActive(!flag2);
    }
    if ((bool) (Object) this.HungerBar)
      this.HungerBar.fillAmount = (float) CookingData.GetSatationAmount(Type) / 100f;
    if (!(bool) (Object) this.FaithBar)
      return;
    this.FaithBar.fillAmount = (float) CookingData.GetFaithAmount(Type) / 12f;
  }

  public void UpdateCookingProgress(float normTime) => this.CookingRing.fillAmount = normTime;

  public IEnumerator FadeIn(float Delay)
  {
    this.canvasGroup.alpha = 0.0f;
    yield return (object) new WaitForSecondsRealtime(Delay);
    float Progress = 0.0f;
    float Duration = 0.2f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      this.canvasGroup.alpha = Progress / Duration;
      yield return (object) null;
    }
    this.canvasGroup.alpha = 1f;
  }

  public IEnumerator Reveal(float Delay)
  {
    UIKitchenMealMenuIcon kitchenMealMenuIcon = this;
    kitchenMealMenuIcon.canvasGroup = kitchenMealMenuIcon.GetComponent<CanvasGroup>();
    kitchenMealMenuIcon.canvasGroup.alpha = 0.0f;
    yield return (object) new WaitForSecondsRealtime(Delay * 2f);
    kitchenMealMenuIcon.StartCoroutine(kitchenMealMenuIcon.ShakeNewIcon());
    float Progress = 0.0f;
    float Duration = 0.3f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      kitchenMealMenuIcon.transform.localScale = Vector3.one * Mathf.Lerp(2.5f, 1f, Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
      kitchenMealMenuIcon.canvasGroup.alpha = Progress / Duration;
      yield return (object) null;
    }
    kitchenMealMenuIcon.FlashIcon.gameObject.SetActive(true);
    Progress = 0.0f;
    Duration = 0.3f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      kitchenMealMenuIcon.FlashIcon.color = Color.Lerp(Color.white, new Color(1f, 1f, 1f, 0.0f), Progress / Duration);
      yield return (object) null;
    }
    kitchenMealMenuIcon.FlashIcon.gameObject.SetActive(false);
  }

  public IEnumerator ShakeNewIcon()
  {
    this.NewIcon.SetActive(true);
    float Progress = 0.0f;
    float Duration = 0.3f;
    float TargetScale = this.NewIcon.transform.localScale.x;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      this.NewIcon.transform.localScale = Vector3.one * Mathf.Lerp(1f, TargetScale, Progress / Duration);
      yield return (object) null;
    }
    this.NewIcon.transform.localScale = Vector3.one * TargetScale;
    float Wobble = (float) Random.Range(0, 360);
    float WobbleSpeed = 4f;
    while (true)
    {
      this.NewIcon.transform.eulerAngles = new Vector3(0.0f, 0.0f, 15f * Mathf.Cos(Wobble += WobbleSpeed * Time.unscaledDeltaTime));
      yield return (object) null;
    }
  }

  public bool CheckCanAfford(InventoryItem.ITEM_TYPE type, int ingredientVariant)
  {
    int num = CheatConsole.BuildingsFree ? 1 : 0;
    return true;
  }

  public string GetCostText(InventoryItem.ITEM_TYPE type, int ingredientVariant) => "";

  public void Shake()
  {
    if (this.cShake != null)
      this.StopCoroutine(this.cShake);
    this.cShake = this.StartCoroutine(this.ShakeRoutine());
  }

  public IEnumerator ShakeRoutine()
  {
    float Progress = 0.0f;
    float Duration = 5f;
    this.ShakeVector.y = 1000f;
    this.ShakeObject.localPosition = Vector3.zero;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      this.ShakeVector.y += (float) ((0.0 - (double) this.ShakeVector.x) * 0.20000000298023224) / Time.unscaledDeltaTime;
      this.ShakeVector.x += (this.ShakeVector.y *= 0.7f) * Time.unscaledDeltaTime;
      this.ShakeObject.localPosition = Vector3.left * this.ShakeVector.x;
      yield return (object) null;
    }
    this.ShakeObject.localPosition = Vector3.zero;
  }

  public void OnSelect(BaseEventData eventData)
  {
    this.IconSelected.enabled = true;
    this.NewIcon.SetActive(false);
    if (this.cSelectionRoutine != null)
      this.StopCoroutine(this.cSelectionRoutine);
    this.cSelectionRoutine = this.StartCoroutine(this.Selected(this.transform.localScale.x, 1.2f));
  }

  public void OnDeselect(BaseEventData eventData)
  {
    this.IconSelected.enabled = false;
    if (this.cSelectionRoutine != null)
      this.StopCoroutine(this.cSelectionRoutine);
    this.cSelectionRoutine = this.StartCoroutine(this.DeSelected());
  }

  public IEnumerator Selected(float Starting, float Target)
  {
    UIKitchenMealMenuIcon kitchenMealMenuIcon = this;
    float Progress = 0.0f;
    float Duration = 0.1f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      float num = Mathf.SmoothStep(Starting, Target, Progress / Duration);
      kitchenMealMenuIcon.transform.localScale = Vector3.one * num;
      yield return (object) null;
    }
    float num1 = Target;
    kitchenMealMenuIcon.transform.localScale = Vector3.one * num1;
  }

  public IEnumerator DeSelected()
  {
    UIKitchenMealMenuIcon kitchenMealMenuIcon = this;
    float Progress = 0.0f;
    float Duration = 0.3f;
    float StartingScale = kitchenMealMenuIcon.transform.localScale.x;
    float TargetScale = 1f;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      float num = Mathf.SmoothStep(StartingScale, TargetScale, Progress / Duration);
      kitchenMealMenuIcon.transform.localScale = Vector3.one * num;
      yield return (object) null;
    }
    float num1 = TargetScale;
    kitchenMealMenuIcon.transform.localScale = Vector3.one * num1;
  }
}
