// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UpgradeMenu.UpgradeMenuCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI.UpgradeMenu;

public class UpgradeMenuCard : MonoBehaviour
{
  public const string kShownGenericAnimationState = "Shown";
  public const string kHiddenGenericAnimationState = "Hidden";
  public const string kShowTrigger = "Show";
  public const string kHideTrigger = "Hide";
  [SerializeField]
  public Animator _animator;
  [SerializeField]
  public CanvasGroup _canvasGroup;
  [Header("Copy")]
  [SerializeField]
  public TextMeshProUGUI _headerText;
  [SerializeField]
  public TextMeshProUGUI _descriptionText;
  [SerializeField]
  public TextMeshProUGUI _loreText;
  [Header("Costs")]
  [SerializeField]
  public TextMeshProUGUI _costText;
  [Header("Graphics")]
  [SerializeField]
  public Image _icon;

  public void Show(UpgradeSystem.Type Type, bool instant = false)
  {
    this._icon.sprite = UpgradeSystem.GetIcon(Type);
    this._headerText.text = UpgradeSystem.GetLocalizedName(Type);
    this._descriptionText.text = UpgradeSystem.GetLocalizedDescription(Type);
    this._loreText.text = "";
    this._costText.text = this.GetCostText(Type);
    this.ResetTriggers();
    if (instant)
      this._animator.Play("Shown");
    else
      this._animator.SetTrigger(nameof (Show));
    Debug.Log((object) "Show card");
  }

  public void Hide(bool instant = false)
  {
    this.ResetTriggers();
    if (instant)
      this._animator.Play("Hidden");
    else
      this._animator.SetTrigger(nameof (Hide));
  }

  public void ResetTriggers()
  {
    this._animator.ResetTrigger("Show");
    this._animator.ResetTrigger("Hide");
  }

  public string GetCostText(UpgradeSystem.Type type)
  {
    string costText = "";
    List<StructuresData.ItemCost> cost = UpgradeSystem.GetCost(type);
    for (int index = 0; index < cost.Count; ++index)
    {
      int itemQuantity = Inventory.GetItemQuantity((int) cost[index].CostItem);
      int costValue = cost[index].CostValue;
      costText = $"{costText + (costValue > itemQuantity ? "<color=#ff0000>" : "<color=#FEF0D3>") + FontImageNames.GetIconByType(cost[index].CostItem)}{itemQuantity.ToString()}</color> / {costValue.ToString()}  ";
    }
    return costText;
  }
}
