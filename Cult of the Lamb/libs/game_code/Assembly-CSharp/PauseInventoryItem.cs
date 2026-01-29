// Decompiled with JetBrains decompiler
// Type: PauseInventoryItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI.Alerts;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class PauseInventoryItem : BaseMonoBehaviour, ISelectHandler, IEventSystemHandler
{
  public Image InventoryItemImage;
  public InventoryItemDisplay inventoryItemDisplay;
  public TextMeshProUGUI AmountText;
  public TextMeshProUGUI AmountText2;
  public InventoryItem.ITEM_TYPE Type;
  public Material normalMaterial;
  public Material bWMaterial;
  [Space]
  [SerializeField]
  public GameObject recipeObject;
  [SerializeField]
  public Image[] faithBars;
  [SerializeField]
  public Image[] hungerBars;
  [SerializeField]
  public MMButton _button;
  [SerializeField]
  public InventoryAlert _alert;
  [CompilerGenerated]
  public int \u003CQuantity\u003Ek__BackingField;
  [SerializeField]
  public CanvasGroup canvasGroup;

  public int Quantity
  {
    get => this.\u003CQuantity\u003Ek__BackingField;
    set => this.\u003CQuantity\u003Ek__BackingField = value;
  }

  public MMButton Button => this._button;

  public InventoryItem.ITEM_TYPE ItemType => this.Type;

  public void Start()
  {
  }

  public void Init(
    InventoryItem.ITEM_TYPE type,
    int Quantity,
    bool showQuantity = true,
    int Quantity2 = 0,
    bool showQuantity2 = false)
  {
    if (type != InventoryItem.ITEM_TYPE.NONE)
    {
      this.Type = type;
      this.InventoryItemImage.sprite = this.inventoryItemDisplay.GetImage(type);
      if ((UnityEngine.Object) this.AmountText != (UnityEngine.Object) null)
      {
        this.AmountText.gameObject.SetActive(showQuantity);
        this.AmountText.text = Quantity.ToString();
      }
      if ((UnityEngine.Object) this.AmountText2 != (UnityEngine.Object) null)
      {
        this.AmountText2.gameObject.SetActive(showQuantity2);
        this.AmountText2.text = $"({Quantity2.ToString()})";
      }
      this.InventoryItemImage.enabled = true;
      if (Quantity <= 0)
        this.SetGrey();
      else
        this.SetWhite();
    }
    else
    {
      this.Type = type;
      this.InventoryItemImage.enabled = false;
      this.AmountText.text = "";
      Selectable component = this.GetComponent<Selectable>();
      if ((bool) (UnityEngine.Object) component)
        component.interactable = false;
    }
    if ((bool) (UnityEngine.Object) this.recipeObject)
      this.recipeObject.SetActive(false);
    if ((UnityEngine.Object) this._alert != (UnityEngine.Object) null)
      this._alert.Configure(this.Type);
    this.Quantity = Quantity;
  }

  public void ShowQuantity2(string s)
  {
    this.AmountText2.gameObject.SetActive(true);
    this.AmountText2.text = s;
  }

  public void ShowRecipe()
  {
    this.recipeObject.SetActive(true);
    for (int index = 0; index < this.hungerBars.Length; ++index)
      this.hungerBars[index].enabled = index < CookingData.GetSatationAmount(this.Type);
    for (int index = 0; index < this.faithBars.Length; ++index)
      this.faithBars[index].enabled = index < CookingData.GetFaithAmount(this.Type);
  }

  public void SetGrey()
  {
    this.InventoryItemImage.material = this.bWMaterial;
    if (!((UnityEngine.Object) this.AmountText != (UnityEngine.Object) null))
      return;
    this.AmountText.color = StaticColors.RedColor;
  }

  public void SetWhite()
  {
    this.InventoryItemImage.material = this.normalMaterial;
    if (!((UnityEngine.Object) this.AmountText != (UnityEngine.Object) null))
      return;
    this.AmountText.color = StaticColors.OffWhiteColor;
  }

  public void OnSelect(BaseEventData eventData) => this._alert.TryRemoveAlert();

  public void FadeIn(float delay, System.Action andThen = null)
  {
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.DoFade(delay, andThen));
  }

  public IEnumerator DoFade(float delay, System.Action andThen)
  {
    this.canvasGroup.alpha = 0.0f;
    yield return (object) new WaitForSecondsRealtime(delay);
    float progress = 0.0f;
    float duration = 0.2f;
    while ((double) (progress += Time.unscaledDeltaTime) < (double) duration)
    {
      this.canvasGroup.alpha = progress / duration;
      yield return (object) null;
    }
    this.canvasGroup.alpha = 1f;
    System.Action action = andThen;
    if (action != null)
      action();
  }
}
