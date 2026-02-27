// Decompiled with JetBrains decompiler
// Type: KnucklebonesPlayerConfiguration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using FMODUnity;
using I2.Loc;
using Spine.Unity;
using UnityEngine;

#nullable disable
[CreateAssetMenu(fileName = "KnucklebonesPlayerConfiguration", menuName = "COTL/Knucklebones/KnucklebonesPlayerConfiguration")]
public class KnucklebonesPlayerConfiguration : ScriptableObject
{
  [SerializeField]
  [TermsPopup("")]
  private string _opponentName;
  [SerializeField]
  private DataManager.Variables _variableToShow;
  [SerializeField]
  private DataManager.Variables _variableToChangeOnWin;
  [SerializeField]
  [EventRef]
  private string _soundToPlay;
  [SerializeField]
  [EventRef]
  private string _victoryAudio;
  [SerializeField]
  [EventRef]
  private string _defeatAudio;
  [SerializeField]
  private TarotCards.Card _reward;
  [SerializeField]
  private SkeletonDataAsset _spine;
  [SerializeField]
  private string _initialSkinName;
  [SerializeField]
  [Range(0.0f, 10f)]
  private int _difficulty;
  [SerializeField]
  private int _maxBet;
  [SerializeField]
  private Vector2 _scale;
  [SerializeField]
  private Vector2 _positionOffset;

  public string OpponentName => this._opponentName;

  public DataManager.Variables VariableToShow => this._variableToShow;

  public DataManager.Variables VariableToChangeOnWin => this._variableToChangeOnWin;

  public string SoundToPlay => this._soundToPlay;

  public string VictoryAudio => this._victoryAudio;

  public string DefeatAudio => this._defeatAudio;

  public TarotCards.Card Reward => this._reward;

  public SkeletonDataAsset Spine => this._spine;

  public string InitialSkinName => this._initialSkinName;

  public int Difficulty => this._difficulty;

  public int MaxBet => this._maxBet;

  public Vector2 Scale => this._scale;

  public Vector2 PositionOffset => this._positionOffset;

  public KBOpponentAI CreateAI() => new KBOpponentAI((float) this._difficulty);
}
