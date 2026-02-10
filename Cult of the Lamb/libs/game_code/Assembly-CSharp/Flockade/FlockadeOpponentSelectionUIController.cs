// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeOpponentSelectionUIController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Flockade;

public class FlockadeOpponentSelectionUIController : UIMenuBase
{
  [Header("Opponent Selection")]
  [SerializeField]
  public Image _difficultyBarFill;
  [SerializeField]
  public MMSelectable_HorizontalSelector _horizontalSelectable;
  [SerializeField]
  public GameObject _maxBetContainer;
  [SerializeField]
  public TextMeshProUGUI _maxBetText;
  [SerializeField]
  public MMPageIndicators _pageIndicators;
  public FlockadeOpponentConfiguration[] _opponents;

  public event System.Action OpponentConfirmed;

  public event Action<int, FlockadeOpponentConfiguration> SelectedOpponentChanged;

  public override void Awake()
  {
    base.Awake();
    this._canvasGroup.alpha = 0.0f;
  }

  public virtual void Start()
  {
    List<string> list = ((IEnumerable<FlockadeOpponentConfiguration>) this._opponents).Where<FlockadeOpponentConfiguration>((Func<FlockadeOpponentConfiguration, bool>) (opponent => opponent.NpcConfiguration != null)).Select<FlockadeOpponentConfiguration, string>((Func<FlockadeOpponentConfiguration, string>) (opponent => LocalizationManager.GetTranslation(opponent.NpcConfiguration.Name))).ToList<string>();
    this._horizontalSelectable.Confirmable = true;
    MMSelectable_HorizontalSelector horizontalSelectable = this._horizontalSelectable;
    horizontalSelectable.OnConfirm = horizontalSelectable.OnConfirm + new System.Action(this.ConfirmOpponent);
    this._horizontalSelectable.HorizontalSelector.PrefillContent(list);
    this._horizontalSelectable.HorizontalSelector.ContentIndex = list.Count - 1;
    this._horizontalSelectable.HorizontalSelector.OnSelectionChanged += new Action<int>(this.UpdateSelectedOpponent);
    this._pageIndicators.SetNumPages(list.Count);
    this._pageIndicators.gameObject.SetActive(list.Count > 1);
    this.UpdateSelectedOpponent(list.Count - 1);
  }

  public void Show(
    FlockadeOpponentConfiguration[] opponentConfigurations,
    bool immediate = false)
  {
    this._opponents = opponentConfigurations;
    this.Show(immediate);
  }

  public void UpdateSelectedOpponent(int selectionIndex)
  {
    FlockadeOpponentConfiguration opponent = this._opponents[selectionIndex];
    Action<int, FlockadeOpponentConfiguration> selectedOpponentChanged = this.SelectedOpponentChanged;
    if (selectedOpponentChanged != null)
      selectedOpponentChanged(selectionIndex, opponent);
    this._difficultyBarFill.fillAmount = (float) (((double) opponent.NpcConfiguration?.Difficulty ?? 0.0) / 10.0);
    if (this._pageIndicators.gameObject.activeSelf)
      this._pageIndicators.SetPage(selectionIndex);
    this._maxBetContainer.SetActive(opponent.NpcConfiguration != null && DataManager.Instance.GetVariable(opponent.NpcConfiguration.VariableToChangeOnWin));
    TextMeshProUGUI maxBetText = this._maxBetText;
    FlockadeNpcConfiguration npcConfiguration = opponent.NpcConfiguration;
    string str = $"{"<sprite name=\"icon_Wool\">"} {(npcConfiguration != null ? npcConfiguration.MaxBet : 0)}";
    maxBetText.text = str;
  }

  public void ConfirmOpponent()
  {
    System.Action opponentConfirmed = this.OpponentConfirmed;
    if (opponentConfirmed != null)
      opponentConfirmed();
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
    FlockadeOpponentSelectionUIController selectionUiController = this;
    while ((double) selectionUiController._canvasGroup.alpha < 1.0)
    {
      selectionUiController._canvasGroup.alpha += Time.unscaledDeltaTime * 2f;
      yield return (object) null;
    }
  }

  public override IEnumerator DoHideAnimation()
  {
    FlockadeOpponentSelectionUIController selectionUiController = this;
    while ((double) selectionUiController._canvasGroup.alpha > 0.0)
    {
      selectionUiController._canvasGroup.alpha -= Time.unscaledDeltaTime * 2f;
      yield return (object) null;
    }
  }
}
