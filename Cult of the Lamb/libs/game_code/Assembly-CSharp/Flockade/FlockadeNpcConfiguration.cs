// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeNpcConfiguration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Spine.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Flockade;

[Serializable]
public class FlockadeNpcConfiguration
{
  public const int MAX_OPPONENT_DIFFICULTY = 10;
  [SerializeField]
  [TermsPopup("")]
  public string _name;
  [SerializeField]
  [Range(0.0f, 10f)]
  public int _difficulty;
  [SerializeField]
  public Color _highlight;
  [SerializeField]
  public int _maxBet;
  [SerializeField]
  public SkeletonDataAsset _skeleton;
  [SerializeField]
  [SpineSkin("", "_skeleton", true, false, false)]
  public string _skin;
  [SerializeField]
  [TermsPopup("")]
  public string _victorySentence;
  [SerializeField]
  [Tooltip("Rewarded on the NPCs first defeat only")]
  public List<FlockadePieceType> _rewardPieces = new List<FlockadePieceType>();
  [SerializeField]
  [SpineAnimation("", "_skeleton", true, false)]
  public string _idleAnimation;
  [SerializeField]
  [SpineAnimation("", "_skeleton", true, false)]
  public string _losePieceOrPointsAnimation;
  [SerializeField]
  [SpineAnimation("", "_skeleton", true, false)]
  public string _loseRoundAnimation;
  [SerializeField]
  [SpineAnimation("", "_skeleton", true, false)]
  public string _loseGameAnimation;
  [SerializeField]
  [SpineAnimation("", "_skeleton", true, false)]
  public string _playPieceAnimation;
  [SerializeField]
  [SpineAnimation("", "_skeleton", true, false)]
  public string _winPieceOrPointsAnimation;
  [SerializeField]
  [SpineAnimation("", "_skeleton", true, false)]
  public string _winRoundAnimation;
  [SerializeField]
  [SpineAnimation("", "_skeleton", true, false)]
  public string _winGameAnimation;
  [SerializeField]
  public string _selectedSound;
  [SerializeField]
  public string _winGameSound;
  [SerializeField]
  public string _loseGameSound;
  [SerializeField]
  public DataManager.Variables _variableToChangeOnWin;
  [SerializeField]
  public DataManager.Variables _variableForWoolWonCount;
  [SerializeField]
  public DataManager.Variables _variableToShow;
  [SerializeField]
  public DataManager.Variables _previousOpponentToBeat;

  public string IdleAnimation => this._idleAnimation;

  public string LosePieceOrPointsAnimation => this._losePieceOrPointsAnimation;

  public string LoseRoundAnimation => this._loseRoundAnimation;

  public string LoseGameAnimation => this._loseGameAnimation;

  public string PlayPieceAnimation => this._playPieceAnimation;

  public string WinPieceOrPointsAnimation => this._winPieceOrPointsAnimation;

  public string WinRoundAnimation => this._winRoundAnimation;

  public string WinGameAnimation => this._winGameAnimation;

  public string VictorySentence => this._victorySentence;

  public int Difficulty => this._difficulty;

  public Color Highlight => this._highlight;

  public int MaxBet => this._maxBet;

  public string Name => this._name;

  public SkeletonDataAsset Skeleton => this._skeleton;

  public string SelectedSound => this._selectedSound;

  public string WinGameSound => this._winGameSound;

  public string LoseGameSound => this._loseGameSound;

  public string Skin => this._skin;

  public IReadOnlyList<FlockadePieceType> RewardPieces
  {
    get => (IReadOnlyList<FlockadePieceType>) this._rewardPieces;
  }

  public DataManager.Variables VariableToChangeOnWin => this._variableToChangeOnWin;

  public DataManager.Variables VariableForWoolWonCount => this._variableForWoolWonCount;

  public DataManager.Variables VariableToShow => this._variableToShow;

  public DataManager.Variables PreviousOpponentToBeat => this._previousOpponentToBeat;

  public FlockadeNpcAI CreateAI(FlockadeGameBoard gameBoard, FlockadeGameBoardSide side)
  {
    return new FlockadeNpcAI(gameBoard, side, (float) (1.0 - (double) this._difficulty / 10.0));
  }
}
