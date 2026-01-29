// Decompiled with JetBrains decompiler
// Type: src.UI.Menus.KBPlayerBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Febucci.UI;
using KnuckleBones;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
namespace src.UI.Menus;

public abstract class KBPlayerBase : MonoBehaviour
{
  [SerializeField]
  public Dice _dicePrefab;
  [SerializeField]
  public Transform _position;
  [SerializeField]
  public List<KBDiceTub> _diceTubs = new List<KBDiceTub>();
  [SerializeField]
  public TextMeshProUGUI _nameText;
  [SerializeField]
  public TextMeshProUGUI _scoreText;
  [SerializeField]
  public TextAnimator _textAnimator;
  [SerializeField]
  public CanvasGroup _turnOverlay;
  [SerializeField]
  public RectTransform _content;
  [SerializeField]
  public RectTransform _tubsRect;
  public string _playerName;
  public Vector2 _contentOriginPosition;
  public Vector2 _tubOriginPosition;
  public Vector2 _contentOffscreenOffset;
  public Vector2 _tubOffscreenOffset;
  public bool tookDice;

  public abstract string _playDiceAnimation { get; }

  public abstract string _playerIdleAnimation { get; }

  public abstract string _playerTakeDiceAnimation { get; }

  public abstract string _playerLostDiceAnimation { get; }

  public abstract string _playerWonAnimation { get; }

  public abstract string _playerWonLoop { get; }

  public abstract string _playerLostAnimation { get; }

  public abstract string _playerLostLoop { get; }

  public abstract SkeletonGraphic _spine { get; }

  public List<KBDiceTub> DiceTubs => this._diceTubs;

  public Dice DicePrefab => this._dicePrefab;

  public Transform Position => this._position;

  public int Score
  {
    get
    {
      int score = 0;
      foreach (KBDiceTub diceTub in this._diceTubs)
        score += diceTub.Score;
      return score;
    }
  }

  public virtual void Configure(Vector2 contentOffscreenOffset, Vector2 tubOffscreenOffset)
  {
    this._scoreText.text = "";
    this._contentOriginPosition = (Vector2) this._content.localPosition;
    this._tubOriginPosition = (Vector2) this._tubsRect.localPosition;
    this._contentOffscreenOffset = contentOffscreenOffset;
    this._tubOffscreenOffset = tubOffscreenOffset;
    foreach (KBDiceTub diceTub in this._diceTubs)
    {
      diceTub.OnDiceMatched += new System.Action(this.OnDiceMatched);
      diceTub.OnDiceLost += new System.Action(this.OnDiceLost);
      diceTub.OpponentTub.OnDiceLost += new System.Action(this.OnDiceMatched);
    }
    this._nameText.text = this._playerName.Wave();
    this.Hide(true);
  }

  public virtual void OnDiceMatched()
  {
    this.tookDice = true;
    this._spine.AnimationState.SetAnimation(0, this._playerTakeDiceAnimation, false);
    this._spine.AnimationState.AddAnimation(0, this._playerIdleAnimation, true, 0.0f);
  }

  public virtual void OnDiceLost()
  {
    this._scoreText.text = this.Score.ToString();
    this._spine.AnimationState.SetAnimation(0, this._playerLostDiceAnimation, false);
    this._spine.AnimationState.AddAnimation(0, this._playerIdleAnimation, true, 0.0f);
  }

  public void Show(bool instant = false)
  {
    if (instant)
    {
      this._content.localPosition = (Vector3) this._contentOriginPosition;
      this._tubsRect.localPosition = (Vector3) this._tubOriginPosition;
    }
    else
    {
      this._content.DOLocalMove((Vector3) this._contentOriginPosition, 1.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
      this._tubsRect.DOLocalMove((Vector3) this._tubOriginPosition, 1.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    }
  }

  public void Hide(bool instant = false)
  {
    if (instant)
    {
      this._content.localPosition = (Vector3) (this._contentOriginPosition + this._contentOffscreenOffset);
      this._tubsRect.localPosition = (Vector3) (this._tubOriginPosition + this._tubOffscreenOffset);
    }
    this._content.DOLocalMove((Vector3) (this._contentOriginPosition + this._contentOffscreenOffset), 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    this._tubsRect.DOLocalMove((Vector3) (this._tubOriginPosition + this._tubOffscreenOffset), 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
  }

  public void HighlightMe()
  {
    this._turnOverlay.DOFade(0.0f, 1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    this._textAnimator.enabled = true;
  }

  public void UnHighlightMe()
  {
    this._turnOverlay.DOFade(1f, 1f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    this._textAnimator.enabled = false;
  }

  public abstract IEnumerator SelectTub(Dice dice);

  public virtual IEnumerator FinishTubSelection(Dice dice, KBDiceTub diceTub)
  {
    yield return (object) diceTub.AddDice(dice);
    if (!this.tookDice)
    {
      this._spine.AnimationState.SetAnimation(0, this._playDiceAnimation, false);
      this._spine.AnimationState.AddAnimation(0, this._playerIdleAnimation, true, 0.0f);
    }
    this.tookDice = false;
    yield return (object) new WaitForSecondsRealtime(0.5f);
    this._scoreText.text = this.Score.ToString();
  }

  public bool CheckGameCompleted()
  {
    int num = 0;
    foreach (KBDiceTub diceTub in this._diceTubs)
    {
      if (diceTub.Dice.Count >= 3)
        ++num;
    }
    return num >= this._diceTubs.Count;
  }

  public void FinalizeScores()
  {
    this._scoreText.text = this.Score.ToString();
    foreach (KBDiceTub diceTub in this._diceTubs)
      diceTub.FinalizeScore();
  }

  public void SetWonLoop()
  {
    this._spine.AnimationState.SetAnimation(0, this._playerWonAnimation, false);
    this._spine.AnimationState.AddAnimation(0, this._playerWonLoop, true, 0.0f);
  }

  public void SetLostLoop()
  {
    this._spine.AnimationState.SetAnimation(0, this._playerLostAnimation, false);
    this._spine.AnimationState.AddAnimation(0, this._playerLostLoop, true, 0.0f);
  }

  public abstract string GetLocalizedName();

  public List<int> GetAllDicesNumbers()
  {
    List<int> allDicesNumbers = new List<int>();
    foreach (KBDiceTub diceTub in this._diceTubs)
    {
      for (int index = 0; index < diceTub.Dice.Count; ++index)
        allDicesNumbers.Add(diceTub.Dice[index].Num);
    }
    return allDicesNumbers;
  }

  public List<int> GetTubDicesNumbers(int index)
  {
    if (index < 0 || index > this._diceTubs.Count)
      return (List<int>) null;
    List<int> tubDicesNumbers = new List<int>();
    KBDiceTub diceTub = this._diceTubs[index];
    for (int index1 = 0; index1 < diceTub.Dice.Count; ++index1)
      tubDicesNumbers.Add(diceTub.Dice[index1].Num);
    return tubDicesNumbers;
  }

  public int GetTubScore(int index)
  {
    if (index < 0 || index > this._diceTubs.Count)
      return 0;
    int tubScore = 0;
    List<int> intList = new List<int>();
    KBDiceTub diceTub = this._diceTubs[index];
    for (int index1 = 0; index1 < diceTub.Dice.Count; ++index1)
      tubScore += diceTub.Dice[index1].Num;
    return tubScore;
  }

  public int GetDuplicateCount(int tubIndex, int dice)
  {
    if (tubIndex < 0 || tubIndex > this._diceTubs.Count)
      return 0;
    int duplicateCount = 0;
    List<int> intList = new List<int>();
    KBDiceTub diceTub = this._diceTubs[tubIndex];
    for (int index = 0; index < diceTub.Dice.Count; ++index)
      intList.Add(diceTub.Dice[index].Num);
    for (int index = 0; index < intList.Count; ++index)
    {
      if (intList[index] == intList[dice])
        ++duplicateCount;
    }
    return duplicateCount;
  }
}
