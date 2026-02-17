// Decompiled with JetBrains decompiler
// Type: UIKnucklebonesBettingSelectionController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
public class UIKnucklebonesBettingSelectionController : UIMenuBase
{
  public const int kBetIncrement = 5;
  public Action<int> OnConfirmBet;
  [Header("Betting")]
  [SerializeField]
  public MMSelectable_HorizontalSelector _horizontalSelectable;
  [SerializeField]
  public MMHorizontalSelector _horizontalSelector;
  [SerializeField]
  public TextMeshProUGUI _winningsText;
  public KnucklebonesPlayerConfiguration _opponent;

  public int _maxBet => this._opponent.MaxBet;

  public int _playerCoins => Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD);

  public override void Awake()
  {
    base.Awake();
    this._canvasGroup.alpha = 0.0f;
  }

  public void Start()
  {
    List<string> content = new List<string>();
    content.Add(KnucklebonesModel.GetLocalizedString("NoCoins"));
    for (int index = 1; index <= this._maxBet / 5; ++index)
      content.Add($"{StringUtilities.Sprite("icon_blackgold")} {index * 5} / {this._maxBet}");
    this._horizontalSelectable.Confirmable = true;
    this._horizontalSelector.PrefillContent(content);
    this._horizontalSelector.OnSelectionChanged += new Action<int>(this.OnSelectionChanged);
    MMSelectable_HorizontalSelector horizontalSelectable = this._horizontalSelectable;
    horizontalSelectable.OnConfirm = horizontalSelectable.OnConfirm + new System.Action(this.ConfirmBet);
    this.OnSelectionChanged(0);
  }

  public void Show(
    KnucklebonesPlayerConfiguration opponentConfiguration,
    bool instant = false)
  {
    this._opponent = opponentConfiguration;
    this.Show(instant);
  }

  public void OnSelectionChanged(int selection)
  {
    if (selection > 0)
      this._winningsText.text = $"{ScriptLocalization.Interactions.Collect} {StringUtilities.Sprite("icon_blackgold")} {selection * 5}";
    else
      this._winningsText.text = "";
    this._horizontalSelectable.Confirmable = this._playerCoins >= selection * 5;
    if (this._horizontalSelectable.Confirmable)
      this._horizontalSelectable.HorizontalSelector.Color = StaticColors.OffWhiteColor;
    else
      this._horizontalSelectable.HorizontalSelector.Color = StaticColors.RedColor;
  }

  public void ConfirmBet()
  {
    Action<int> onConfirmBet = this.OnConfirmBet;
    if (onConfirmBet != null)
      onConfirmBet(this._horizontalSelector.ContentIndex);
    this.Hide();
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable)
      return;
    this.Hide();
  }

  public override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

  public override IEnumerator DoShowAnimation()
  {
    UIKnucklebonesBettingSelectionController selectionController = this;
    while ((double) selectionController._canvasGroup.alpha < 1.0)
    {
      selectionController._canvasGroup.alpha += Time.unscaledDeltaTime * 2f;
      yield return (object) null;
    }
  }

  public override IEnumerator DoHideAnimation()
  {
    UIKnucklebonesBettingSelectionController selectionController = this;
    while ((double) selectionController._canvasGroup.alpha > 0.0)
    {
      selectionController._canvasGroup.alpha -= Time.unscaledDeltaTime * 2f;
      yield return (object) null;
    }
  }
}
