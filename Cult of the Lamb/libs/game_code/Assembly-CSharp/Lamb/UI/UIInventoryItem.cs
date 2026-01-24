// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIInventoryItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using Lamb.UI.Assets;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public abstract class UIInventoryItem : BaseMonoBehaviour
{
  [SerializeField]
  public Image _icon;
  [SerializeField]
  public RectTransform _rectTransform;
  [SerializeField]
  public CanvasGroup _canvasGroup;
  [SerializeField]
  public InventoryIconMapping _iconMapping;
  [SerializeField]
  public TextMeshProUGUI _amountText;
  [SerializeField]
  public MMButton _button;
  [SerializeField]
  public RectTransform _container;
  public InventoryItem _item;
  public int _quantity;
  public bool _showQuantity;
  public Vector2 _containerOrigin;
  public bool IgnoreSingles;

  public MMButton Button => this._button;

  public InventoryItem.ITEM_TYPE Type => (InventoryItem.ITEM_TYPE) this._item.type;

  public int Quantity => this._quantity;

  public RectTransform RectTransform => this._rectTransform;

  public CanvasGroup CanvasGroup => this._canvasGroup;

  public virtual void Configure(InventoryItem.ITEM_TYPE type, bool showQuantity = true)
  {
    this.Configure(Inventory.GetItemByType(type) ?? new InventoryItem(type, 0), showQuantity);
  }

  public virtual void Configure(InventoryItem item, bool showQuantity = true)
  {
    this._item = item;
    this._showQuantity = showQuantity;
    if ((UnityEngine.Object) this._amountText != (UnityEngine.Object) null)
      this._amountText.gameObject.SetActive(this._showQuantity);
    this._iconMapping.GetImage(this.Type, this._icon);
    this._containerOrigin = this._container.anchoredPosition;
    this.UpdateQuantity();
  }

  public void FadeIn(float delay, System.Action andThen = null)
  {
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.DoFade(delay, andThen));
  }

  public IEnumerator DoFade(float delay, System.Action andThen)
  {
    this._canvasGroup.alpha = 0.0f;
    yield return (object) new WaitForSecondsRealtime(delay);
    float progress = 0.0f;
    float duration = 0.2f;
    while ((double) (progress += Time.unscaledDeltaTime) < (double) duration)
    {
      this._canvasGroup.alpha = progress / duration;
      yield return (object) null;
    }
    this._canvasGroup.alpha = 1f;
    System.Action action = andThen;
    if (action != null)
      action();
  }

  public void Shake()
  {
    this._container.transform.DOKill();
    this._container.anchoredPosition = this._containerOrigin;
    this._container.transform.DOShakePosition(1f, new Vector3(10f, 0.0f)).SetUpdate<Tweener>(true);
  }

  public virtual void UpdateQuantity()
  {
    this._quantity = this._item.quantity;
    if (this._quantity == 1 && this.IgnoreSingles)
      this._amountText.text = "";
    else if (this._showQuantity)
    {
      this._icon.color = new Color(this._quantity <= 0 ? 0.0f : 1f, 1f, 1f, 1f);
      this._amountText.color = this._quantity <= 0 ? StaticColors.RedColor : StaticColors.OffWhiteColor;
      this._amountText.text = this._quantity.ToString();
    }
    else
      this._amountText.text = "";
  }
}
