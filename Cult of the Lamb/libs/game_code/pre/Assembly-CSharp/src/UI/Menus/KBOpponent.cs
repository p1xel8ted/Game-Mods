// Decompiled with JetBrains decompiler
// Type: src.UI.Menus.KBOpponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using KnuckleBones;
using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
namespace src.UI.Menus;

public class KBOpponent : KBPlayerBase
{
  [SerializeField]
  private SkeletonGraphic _opponentSpine;
  private KBOpponentAI _ai;
  private KnucklebonesOpponent _opponent;

  protected override string _playDiceAnimation => "knucklebones/play-dice";

  protected override string _playerIdleAnimation => "animation";

  protected override string _playerTakeDiceAnimation => "knucklebones/take-dice";

  protected override string _playerLostDiceAnimation => "knucklebones/lose-dice";

  protected override string _playerWonAnimation => "knucklebones/win-game";

  protected override string _playerWonLoop => "knucklebones/win-game-loop";

  protected override string _playerLostAnimation => "knucklebones/lose-game";

  protected override string _playerLostLoop => "knucklebones/lose-game-loop";

  protected override SkeletonGraphic _spine => this._opponentSpine;

  public void Configure(
    KnucklebonesOpponent opponent,
    Vector2 contentOffscreenOffset,
    Vector2 tubOffscreenOffset)
  {
    this._opponent = opponent;
    this._ai = this._opponent.Config.CreateAI();
    this._playerName = this.GetLocalizedName();
    this._opponentSpine.skeletonDataAsset = opponent.Config.Spine;
    this._opponentSpine.initialSkinName = string.Empty;
    this._opponentSpine.startingAnimation = string.Empty;
    this._opponentSpine.Initialize(true);
    this._opponentSpine.Skeleton.SetSkin(opponent.Config.InitialSkinName);
    this._opponentSpine.AnimationState.SetAnimation(0, this._playerIdleAnimation, true);
    this._opponentSpine.transform.localPosition = (Vector3) opponent.Config.PositionOffset;
    this._opponentSpine.transform.localScale = (Vector3) opponent.Config.Scale;
    this.Configure(contentOffscreenOffset, tubOffscreenOffset);
  }

  public override IEnumerator SelectTub(Dice dice)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    KBOpponent kbOpponent = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    int index = kbOpponent._ai.Evaluate(kbOpponent._diceTubs, dice);
    KBDiceTub diceTub = kbOpponent._diceTubs[index];
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) kbOpponent.FinishTubSelection(dice, diceTub);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void PlayEndGameVoiceover(bool victory)
  {
    if (victory)
      AudioManager.Instance.PlayOneShot(this._opponent.Config.DefeatAudio);
    else
      AudioManager.Instance.PlayOneShot(this._opponent.Config.VictoryAudio);
  }

  public override string GetLocalizedName()
  {
    return LocalizationManager.GetTranslation(this._opponent.Config.OpponentName);
  }
}
