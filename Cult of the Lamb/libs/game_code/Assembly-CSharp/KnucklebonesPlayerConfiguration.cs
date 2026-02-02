// Decompiled with JetBrains decompiler
// Type: KnucklebonesPlayerConfiguration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public string _opponentName;
  [SerializeField]
  public DataManager.Variables _variableToShow;
  [SerializeField]
  public DataManager.Variables _variableToChangeOnWin;
  [SerializeField]
  [EventRef]
  public string _soundToPlay;
  [SerializeField]
  [EventRef]
  public string _victoryAudio;
  [SerializeField]
  [EventRef]
  public string _defeatAudio;
  [SerializeField]
  public TarotCards.Card _reward;
  [SerializeField]
  public SkeletonDataAsset _spine;
  [SerializeField]
  public string _initialSkinName;
  [SerializeField]
  [Range(0.0f, 10f)]
  public int _difficulty;
  [SerializeField]
  public int _maxBet;
  [SerializeField]
  public Vector2 _scale;
  [SerializeField]
  public Vector2 _positionOffset;

  public string OpponentName
  {
    get => this._opponentName;
    set => this._opponentName = value;
  }

  public DataManager.Variables VariableToShow => this._variableToShow;

  public DataManager.Variables VariableToChangeOnWin => this._variableToChangeOnWin;

  public string SoundToPlay => this._soundToPlay;

  public string VictoryAudio => this._victoryAudio;

  public string DefeatAudio => this._defeatAudio;

  public TarotCards.Card Reward => this._reward;

  public SkeletonDataAsset Spine => this._spine;

  public string InitialSkinName
  {
    get
    {
      return this.Spine.name.Contains("Klunko") && DataManager.Instance.TookBopToTailor ? "Just-Head-NoBop" : this._initialSkinName;
    }
  }

  public int Difficulty
  {
    get => this._difficulty;
    set => this._difficulty = value;
  }

  public int MaxBet => this._maxBet;

  public Vector2 Scale => this._scale;

  public Vector2 PositionOffset => this._positionOffset;

  public KBOpponentAI CreateAI() => new KBOpponentAI((float) this._difficulty);
}
