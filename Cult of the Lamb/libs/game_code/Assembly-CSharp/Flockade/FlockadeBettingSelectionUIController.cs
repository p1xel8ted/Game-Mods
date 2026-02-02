// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeBettingSelectionUIController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
namespace Flockade;

public class FlockadeBettingSelectionUIController : UIMenuBase
{
  public const int kBetIncrement = 1;
  public static string _BET_FORMAT = "<sprite name=\"icon_Wool\"> {0} / {1}";
  [Header("Betting")]
  [SerializeField]
  public MMSelectable_HorizontalSelector _horizontalSelectable;
  [SerializeField]
  [TermsPopup("")]
  public string _noCoins;
  [SerializeField]
  public TextMeshProUGUI _winningsText;
  public FlockadeOpponentConfiguration _opponent;

  public event Action<int> BetConfirmed;

  public int MaxBet
  {
    get
    {
      FlockadeNpcConfiguration npcConfiguration = this._opponent.NpcConfiguration;
      return npcConfiguration == null ? 0 : npcConfiguration.MaxBet;
    }
  }

  public override void Awake()
  {
    base.Awake();
    this._canvasGroup.alpha = 0.0f;
  }

  public virtual void Start()
  {
    List<string> content = new List<string>()
    {
      LocalizationManager.GetTranslation(this._noCoins)
    };
    for (int index = 1; index <= this.MaxBet / 1; ++index)
      content.Add(string.Format(FlockadeBettingSelectionUIController._BET_FORMAT, (object) index, (object) this.MaxBet));
    this._horizontalSelectable.Confirmable = true;
    MMSelectable_HorizontalSelector horizontalSelectable = this._horizontalSelectable;
    horizontalSelectable.OnConfirm = horizontalSelectable.OnConfirm + new System.Action(this.ConfirmBet);
    this._horizontalSelectable.HorizontalSelector.PrefillContent(content);
    this._horizontalSelectable.HorizontalSelector.OnSelectionChanged += new Action<int>(this.UpdateBetting);
    this.UpdateBetting(0);
  }

  public void Show(
    FlockadeOpponentConfiguration opponentConfiguration,
    bool immediate = false)
  {
    this._opponent = opponentConfiguration;
    this.Show(immediate);
  }

  public void UpdateBetting(int selectionIndex)
  {
    int num = Mathf.Clamp(selectionIndex, 1, int.MaxValue);
    this._winningsText.text = selectionIndex > 0 ? $"{ScriptLocalization.Interactions.Collect} {"<sprite name=\"icon_Wool\">"} {num}" : string.Empty;
    this._horizontalSelectable.Confirmable = Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.WOOL) >= num;
    this._horizontalSelectable.HorizontalSelector.Color = this._horizontalSelectable.Confirmable ? StaticColors.OffWhiteColor : StaticColors.RedColor;
  }

  public void ConfirmBet()
  {
    Action<int> betConfirmed = this.BetConfirmed;
    if (betConfirmed != null)
      betConfirmed(this._horizontalSelectable.HorizontalSelector.ContentIndex);
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
    FlockadeBettingSelectionUIController selectionUiController = this;
    while ((double) selectionUiController._canvasGroup.alpha < 1.0)
    {
      selectionUiController._canvasGroup.alpha += Time.unscaledDeltaTime * 2f;
      yield return (object) null;
    }
  }

  public override IEnumerator DoHideAnimation()
  {
    FlockadeBettingSelectionUIController selectionUiController = this;
    while ((double) selectionUiController._canvasGroup.alpha > 0.0)
    {
      selectionUiController._canvasGroup.alpha -= Time.unscaledDeltaTime * 2f;
      yield return (object) null;
    }
  }
}
