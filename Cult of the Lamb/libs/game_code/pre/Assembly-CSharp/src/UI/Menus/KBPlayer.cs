// Decompiled with JetBrains decompiler
// Type: src.UI.Menus.KBPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using KnuckleBones;
using Spine.Unity;
using src.UINavigator;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace src.UI.Menus;

public class KBPlayer : KBPlayerBase
{
  [SerializeField]
  private SkeletonGraphic _lambSpine;
  private MMButton[] _tubSelectables;

  protected override string _playDiceAnimation => "knucklebones/play-dice";

  protected override string _playerIdleAnimation => "knucklebones/idle";

  protected override string _playerTakeDiceAnimation => "knucklebones/take-dice";

  protected override string _playerLostDiceAnimation => "knucklebones/lose-dice";

  protected override string _playerWonAnimation => "knucklebones/win-game";

  protected override string _playerWonLoop => "knucklebones/win-game-loop";

  protected override string _playerLostAnimation => "knucklebones/lose-game";

  protected override string _playerLostLoop => "knucklebones/lose-game-loop";

  protected override SkeletonGraphic _spine => this._lambSpine;

  public override void Configure(Vector2 contentOffscreenOffset, Vector2 tubOffscreenOffset)
  {
    this._playerName = this.GetLocalizedName();
    this._tubSelectables = new MMButton[this._diceTubs.Count];
    for (int index = 0; index < this._diceTubs.Count; ++index)
    {
      this._tubSelectables[index] = this._diceTubs[index].GetComponent<MMButton>();
      this._tubSelectables[index].interactable = false;
    }
    base.Configure(contentOffscreenOffset, tubOffscreenOffset);
  }

  public override IEnumerator SelectTub(Dice dice)
  {
    KBPlayer kbPlayer = this;
    KBDiceTub diceTub = (KBDiceTub) null;
    foreach (MMButton tubSelectable in kbPlayer._tubSelectables)
    {
      tubSelectable.interactable = true;
      tubSelectable.onClick.AddListener(new UnityAction(Choose));
    }
    MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) kbPlayer._tubSelectables[1]);
    bool chosen = false;
    while (!chosen)
      yield return (object) null;
    foreach (MMButton tubSelectable in kbPlayer._tubSelectables)
    {
      tubSelectable.interactable = false;
      tubSelectable.onClick.RemoveListener(new UnityAction(Choose));
    }
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    yield return (object) kbPlayer.FinishTubSelection(dice, diceTub);

    void Choose()
    {
      int index = ((IMMSelectable[]) this._tubSelectables).IndexOf<IMMSelectable>(MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable);
      if (!this._diceTubs[index].TrySelectTub())
        return;
      diceTub = this._diceTubs[index];
      chosen = true;
    }
  }

  public override string GetLocalizedName() => ScriptLocalization.NAMES_Knucklebones.Player;
}
