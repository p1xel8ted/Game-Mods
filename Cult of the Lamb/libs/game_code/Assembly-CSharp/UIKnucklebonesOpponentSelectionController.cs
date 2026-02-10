// Decompiled with JetBrains decompiler
// Type: UIKnucklebonesOpponentSelectionController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UIKnucklebonesOpponentSelectionController : UIMenuBase
{
  public System.Action OnConfirmOpponent;
  public Action<int> OnOpponentSelectionChanged;
  [Header("Opponent Selection")]
  [SerializeField]
  public MMSelectable_HorizontalSelector _horizontalSelectable;
  [SerializeField]
  public MMHorizontalSelector _horizontalSelector;
  [SerializeField]
  public Image _difficultyFill;
  [SerializeField]
  public MMPageIndicators _mmPageIndicators;
  [SerializeField]
  public GameObject _maxBetContainer;
  [SerializeField]
  public TextMeshProUGUI _maxBetText;
  public KnucklebonesPlayerConfiguration[] _opponents;

  public override void Awake()
  {
    base.Awake();
    this._canvasGroup.alpha = 0.0f;
  }

  public void Show(KnucklebonesPlayerConfiguration[] opponents, bool instant = false)
  {
    this._opponents = opponents;
    this.Show(instant);
  }

  public void Start()
  {
    List<string> content = new List<string>();
    foreach (KnucklebonesPlayerConfiguration opponent in this._opponents)
      content.Add(LocalizationManager.GetTermTranslation(opponent.OpponentName));
    this._horizontalSelectable.Confirmable = true;
    this._horizontalSelector.PrefillContent(content);
    this._horizontalSelector.ContentIndex = content.Count - 1;
    this._horizontalSelector.OnSelectionChanged += new Action<int>(this.OnSelectionChanged);
    MMSelectable_HorizontalSelector horizontalSelectable = this._horizontalSelectable;
    horizontalSelectable.OnConfirm = horizontalSelectable.OnConfirm + new System.Action(this.ConfirmOpponent);
    this._mmPageIndicators.SetNumPages(content.Count);
    this._mmPageIndicators.gameObject.SetActive(content.Count > 1);
    this.OnSelectionChanged(content.Count - 1);
  }

  public void OnSelectionChanged(int selection)
  {
    Action<int> selectionChanged = this.OnOpponentSelectionChanged;
    if (selectionChanged != null)
      selectionChanged(selection);
    this._difficultyFill.fillAmount = (float) this._opponents[selection].Difficulty / 10f;
    if (this._mmPageIndicators.gameObject.activeSelf)
      this._mmPageIndicators.SetPage(selection);
    this._maxBetContainer.SetActive(DataManager.Instance.GetVariable(this._opponents[selection].VariableToChangeOnWin));
    this._maxBetText.text = $"{"<sprite name=\"icon_blackgold\">"} {this._opponents[selection].MaxBet}";
  }

  public void ConfirmOpponent()
  {
    System.Action onConfirmOpponent = this.OnConfirmOpponent;
    if (onConfirmOpponent != null)
      onConfirmOpponent();
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
    UIKnucklebonesOpponentSelectionController selectionController = this;
    while ((double) selectionController._canvasGroup.alpha < 1.0)
    {
      selectionController._canvasGroup.alpha += Time.unscaledDeltaTime * 2f;
      yield return (object) null;
    }
  }

  public override IEnumerator DoHideAnimation()
  {
    UIKnucklebonesOpponentSelectionController selectionController = this;
    while ((double) selectionController._canvasGroup.alpha > 0.0)
    {
      selectionController._canvasGroup.alpha -= Time.unscaledDeltaTime * 2f;
      yield return (object) null;
    }
  }
}
