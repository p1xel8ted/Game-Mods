// Decompiled with JetBrains decompiler
// Type: KBDiceTub
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class KBDiceTub : MonoBehaviour, ISelectHandler, IEventSystemHandler, IDeselectHandler
{
  public System.Action OnDiceLost;
  public System.Action OnDiceMatched;
  [SerializeField]
  private RectTransform _rectTransform;
  [SerializeField]
  private KBDiceTub _opponentTub;
  [SerializeField]
  private Image _highlight;
  [SerializeField]
  private TextMeshProUGUI _scoreText;
  [SerializeField]
  private List<RectTransform> _diceContainers = new List<RectTransform>();
  private int _score;
  private List<KnuckleBones.Dice> _dice = new List<KnuckleBones.Dice>();
  private RectTransform _highlightRectTransform;
  private Vector3 _highlightOriginScale;
  private Vector3 _positionOrigin;

  public int Score => this._score;

  public List<KnuckleBones.Dice> Dice => this._dice;

  public KBDiceTub OpponentTub => this._opponentTub;

  private void Start()
  {
    this._highlightRectTransform = this._highlight.GetComponent<RectTransform>();
    this._highlightOriginScale = this._highlightRectTransform.localScale;
    this._highlight.enabled = false;
    this._scoreText.text = "";
    this._positionOrigin = (Vector3) this._rectTransform.anchoredPosition;
  }

  public bool TrySelectTub()
  {
    if (this._dice.Count < 3)
      return true;
    AudioManager.Instance.PlayOneShot("event:/ui/negative_feedback");
    this._rectTransform.DOKill();
    this._rectTransform.anchoredPosition = (Vector2) this._positionOrigin;
    this._rectTransform.DOShakeAnchorPos(0.75f, 5f).SetUpdate<Tweener>(true);
    return false;
  }

  public IEnumerator AddDice(KnuckleBones.Dice dice)
  {
    yield return (object) dice.GoToLocationRoutine((Vector3) this.GetNextPosition());
    this._dice.Add(dice);
    this.UpdateScore((KnuckleBones.Dice) null);
    yield return (object) this._opponentTub.CheckDice(dice);
  }

  private IEnumerator CheckDice(KnuckleBones.Dice dice)
  {
    bool flag = false;
    int i = -1;
    while (++i < this.Dice.Count)
    {
      if (this.Dice[i].Num == dice.Num)
      {
        AudioManager.Instance.PlayOneShot("event:/material/stained_glass_impact");
        yield return (object) this.Dice[i].StartCoroutine((IEnumerator) this.Dice[i].ShakeRoutine());
        UnityEngine.Object.Destroy((UnityEngine.Object) this.Dice[i].gameObject);
        this.Dice.RemoveAt(i);
        flag = true;
        --i;
      }
    }
    this.UpdateScore(dice);
    if (flag)
    {
      System.Action onDiceLost = this.OnDiceLost;
      if (onDiceLost != null)
        onDiceLost();
      i = -1;
      while (++i < this._dice.Count)
      {
        if (!this._dice[i].transform.position.Equals((Vector3) this.GetPosition(i)))
          yield return (object) this._dice[i].GoToLocationRoutine((Vector3) this.GetPosition(i));
      }
    }
  }

  private void UpdateScore(KnuckleBones.Dice dice)
  {
    this._score = 0;
    foreach (KnuckleBones.Dice die in this._dice)
    {
      int num = this.NumMatchingDice(die.Num);
      this._score += die.Num * num;
      if (num > 1)
      {
        die.image.color = (Color) (num == 2 ? new Color32((byte) 246, (byte) 235, (byte) 152, byte.MaxValue) : new Color32((byte) 117, (byte) 189, byte.MaxValue, byte.MaxValue));
        if (!die.matched)
        {
          Debug.Log((object) "Match Dice");
          die.matched = true;
          System.Action onDiceMatched = this.OnDiceMatched;
          if (onDiceMatched != null)
            onDiceMatched();
          AudioManager.Instance.PlayOneShot("event:/ui/open_menu");
        }
        if ((UnityEngine.Object) dice != (UnityEngine.Object) null && die.Num == dice.Num)
          die.Scale();
      }
      else
        die.image.color = Color.white;
    }
    if (this.Score == 0)
    {
      this._scoreText.text = "";
    }
    else
    {
      this._scoreText.text = this.Score.ToString();
      this._scoreText.transform.DOKill();
      this._scoreText.transform.DOPunchPosition(new Vector3(5f, 0.0f), 1f).SetUpdate<Tweener>(true);
    }
  }

  private void Highlight()
  {
    this._highlight.enabled = true;
    this._highlightRectTransform.localScale = this._highlightOriginScale * 0.8f;
    this._highlightRectTransform.DOKill();
    this._highlightRectTransform.DOScale(this._highlightOriginScale, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    DOTweenModuleUI.DOFade(this._highlight, 1f, 0.5f).SetEase<TweenerCore<Color, Color, ColorOptions>>(Ease.OutQuart).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    this._highlight.color = this._dice.Count < 3 ? Color.red : Color.black;
  }

  private void UnHighlight() => this._highlight.enabled = false;

  public void OnSelect(BaseEventData eventData) => this.Highlight();

  public void OnDeselect(BaseEventData eventData) => this.UnHighlight();

  public int NumMatchingDice(int number)
  {
    int num = 0;
    foreach (KnuckleBones.Dice die in this._dice)
    {
      if (die.Num == number)
        ++num;
    }
    return num;
  }

  private Vector2 GetNextPosition() => (Vector2) this._diceContainers[this._dice.Count].position;

  private Vector2 GetPosition(int index) => (Vector2) this._diceContainers[index].position;

  public void FinalizeScore()
  {
    int score1 = this.Score;
    int score2 = this._opponentTub.Score;
  }
}
