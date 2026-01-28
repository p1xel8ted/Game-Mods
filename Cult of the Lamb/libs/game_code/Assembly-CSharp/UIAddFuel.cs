// Decompiled with JetBrains decompiler
// Type: UIAddFuel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using Lamb.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UIAddFuel : BaseMonoBehaviour
{
  [SerializeField]
  public Vector3 offset;
  [Space]
  [SerializeField]
  public Image fuelBar;
  [SerializeField]
  public Image fuelFlashBar;
  [Space]
  [SerializeField]
  public Color emptyBarColor;
  [SerializeField]
  public Color emptyFlashBarColor;
  [SerializeField]
  public Color halfBarColor;
  [SerializeField]
  public Color halfFlashBarColor;
  [SerializeField]
  public Color fullBarColor;
  [SerializeField]
  public Color fullFlashBarColor;
  public bool hiding = true;
  public Camera camera;
  public Canvas canvas;
  public CanvasGroup canvasGroup;
  public RectTransform rectTransform;
  public Interaction_AddFuel fuelInteraction;
  public Coroutine fuelIncreasedRoutine;
  public float targetFuelBarAmount;

  public bool IsShowing => !this.hiding;

  public void Awake()
  {
    this.canvasGroup = this.GetComponent<CanvasGroup>();
    this.canvas = this.GetComponentInParent<Canvas>();
    this.rectTransform = this.GetComponent<RectTransform>();
    this.canvasGroup.alpha = 0.0f;
    this.gameObject.SetActive(false);
  }

  public void Show(Interaction_AddFuel fuelInteraction)
  {
    Debug.Log((object) "SHOW ME!".Colour(Color.red));
    this.gameObject.SetActive(true);
    this.camera = Camera.main;
    this.fuelInteraction = fuelInteraction;
    this.fuelInteraction.OnFuelModified += new Interaction_AddFuel.FuelEvent(this.FuelUpdated);
    this.FuelUpdated((float) fuelInteraction.Structure.Structure_Info.Fuel / (float) fuelInteraction.Structure.Structure_Info.MaxFuel);
    this.hiding = false;
  }

  public void Hide()
  {
    this.hiding = true;
    if (!(bool) (Object) this.fuelInteraction)
      return;
    this.fuelInteraction.OnFuelModified -= new Interaction_AddFuel.FuelEvent(this.FuelUpdated);
  }

  public void LateUpdate()
  {
    if ((Object) this.fuelInteraction != (Object) null)
    {
      Vector3 vector3 = Vector3.zero;
      foreach (UIItemSelectorOverlayController selectorOverlay in UIItemSelectorOverlayController.SelectorOverlays)
      {
        if ((Object) selectorOverlay != (Object) null && (Object) selectorOverlay.playerFarming == (Object) this.fuelInteraction.playerFarming)
          vector3 = Vector3.up * 100f;
      }
      this.rectTransform.position = this.camera.WorldToScreenPoint(this.fuelInteraction.LockPosition.position) + (this.offset + vector3) * this.canvas.scaleFactor;
    }
    if (!this.hiding)
    {
      if ((double) this.canvasGroup.alpha >= 1.0)
        return;
      this.canvasGroup.alpha += Time.deltaTime * 5f;
    }
    else if ((Object) this.canvasGroup != (Object) null && (double) this.canvasGroup.alpha > 0.0)
    {
      this.canvasGroup.alpha -= 5f * Time.deltaTime;
    }
    else
    {
      if (!this.gameObject.activeSelf)
        return;
      this.gameObject.SetActive(false);
    }
  }

  public void FuelUpdated(float normFuelAmount)
  {
    if (!this.gameObject.activeInHierarchy || (double) normFuelAmount == (double) this.targetFuelBarAmount)
      return;
    if (this.fuelIncreasedRoutine != null)
    {
      this.StopCoroutine(this.fuelIncreasedRoutine);
      this.ForceFuelAmount(this.targetFuelBarAmount);
    }
    this.targetFuelBarAmount = normFuelAmount;
    this.fuelIncreasedRoutine = this.StartCoroutine((IEnumerator) this.FuelBarUpdated(normFuelAmount));
  }

  public void ForceFuelAmount(float normFuelAmount)
  {
    this.fuelBar.fillAmount = normFuelAmount;
    this.fuelFlashBar.fillAmount = normFuelAmount;
    this.SetBarColor(normFuelAmount, this.fuelBar, this.emptyBarColor, this.halfBarColor, this.fullBarColor, 0.0f);
    this.SetBarColor(normFuelAmount, this.fuelFlashBar, this.emptyFlashBarColor, this.halfFlashBarColor, this.fullFlashBarColor, 0.0f);
    this.targetFuelBarAmount = normFuelAmount;
  }

  public IEnumerator FuelBarUpdated(float normAmount)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    UIAddFuel uiAddFuel = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      uiAddFuel.fuelIncreasedRoutine = (Coroutine) null;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) uiAddFuel.StartCoroutine((IEnumerator) uiAddFuel.BarUpdated(normAmount, uiAddFuel.fuelBar, uiAddFuel.fuelFlashBar));
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator BarUpdated(float normAmount, Image bar, Image flashBar)
  {
    Image secondMovingBar;
    Image image;
    if ((double) normAmount < (double) bar.fillAmount)
    {
      image = bar;
      secondMovingBar = flashBar;
      this.SetBarColor(normAmount, bar, this.emptyBarColor, this.halfBarColor, this.fullBarColor);
    }
    else
    {
      image = flashBar;
      secondMovingBar = bar;
      this.SetBarColor(normAmount, flashBar, this.emptyFlashBarColor, this.halfFlashBarColor, this.fullFlashBarColor);
    }
    image.fillAmount = normAmount;
    yield return (object) new WaitForSeconds(0.3f);
    if ((Object) secondMovingBar == (Object) flashBar)
      this.SetBarColor(normAmount, flashBar, this.emptyFlashBarColor, this.halfFlashBarColor, this.fullFlashBarColor);
    else
      this.SetBarColor(normAmount, bar, this.emptyBarColor, this.halfBarColor, this.fullBarColor);
    float fromAmount = secondMovingBar.fillAmount;
    float t = 0.0f;
    while ((double) t < 0.25)
    {
      secondMovingBar.fillAmount = Mathf.Lerp(fromAmount, normAmount, t / 0.25f);
      t += Time.deltaTime;
      yield return (object) null;
    }
    secondMovingBar.fillAmount = normAmount;
  }

  public void SetBarColor(
    float normAmount,
    Image bar,
    Color color1,
    Color color2,
    Color color3,
    float duration = 0.3f)
  {
    Color endValue = (double) normAmount >= 0.5 ? Color.Lerp(color2, color3, normAmount) : Color.Lerp(color1, color2, normAmount);
    DOTweenModuleUI.DOColor(bar, endValue, duration);
  }
}
