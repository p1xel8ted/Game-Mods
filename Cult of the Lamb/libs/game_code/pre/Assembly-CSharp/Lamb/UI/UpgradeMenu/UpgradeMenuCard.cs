// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UpgradeMenu.UpgradeMenuCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI.UpgradeMenu;

public class UpgradeMenuCard : MonoBehaviour
{
  private const string kShownGenericAnimationState = "Shown";
  private const string kHiddenGenericAnimationState = "Hidden";
  private const string kShowTrigger = "Show";
  private const string kHideTrigger = "Hide";
  [SerializeField]
  private Animator _animator;
  [SerializeField]
  private CanvasGroup _canvasGroup;
  [Header("Copy")]
  [SerializeField]
  private TextMeshProUGUI _headerText;
  [SerializeField]
  private TextMeshProUGUI _descriptionText;
  [SerializeField]
  private TextMeshProUGUI _loreText;
  [Header("Costs")]
  [SerializeField]
  private TextMeshProUGUI _costText;
  [Header("Graphics")]
  [SerializeField]
  private Image _icon;

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

  private void ResetTriggers()
  {
    this._animator.ResetTrigger("Show");
    this._animator.ResetTrigger("Hide");
  }

  private string GetCostText(UpgradeSystem.Type type)
  {
    string costText = "";
    List<StructuresData.ItemCost> cost = UpgradeSystem.GetCost(type);
    for (int index = 0; index < cost.Count; ++index)
    {
      int itemQuantity = Inventory.GetItemQuantity((int) cost[index].CostItem);
      int costValue = cost[index].CostValue;
      costText = $"{costText + (costValue > itemQuantity ? "<color=#ff0000>" : "<color=#FEF0D3>") + FontImageNames.GetIconByType(cost[index].CostItem)}{(object) itemQuantity}</color> / {costValue.ToString()}  ";
    }
    return costText;
  }
}
