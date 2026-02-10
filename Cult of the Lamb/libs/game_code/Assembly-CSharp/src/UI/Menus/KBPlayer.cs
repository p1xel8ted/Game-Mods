// Decompiled with JetBrains decompiler
// Type: src.UI.Menus.KBPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using KnuckleBones;
using Spine;
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
  public SkeletonGraphic _lambSpine;
  [SerializeField]
  public KBPlayer _otherCoopPlayer;
  [SerializeField]
  public KBOpponent _twitchChat;
  public MMButton[] _tubSelectables;
  public PlayerFarming playerFarming;

  public override string _playDiceAnimation
  {
    get => !this.isLamb ? "knucklebones/knucklebones-goat/play-dice" : "knucklebones/play-dice";
  }

  public override string _playerIdleAnimation
  {
    get => !this.isLamb ? "knucklebones/knucklebones-goat/idle" : "knucklebones/idle";
  }

  public override string _playerTakeDiceAnimation
  {
    get => !this.isLamb ? "knucklebones/knucklebones-goat/take-dice" : "knucklebones/take-dice";
  }

  public override string _playerLostDiceAnimation
  {
    get => !this.isLamb ? "knucklebones/knucklebones-goat/lose-dice" : "knucklebones/lose-dice";
  }

  public override string _playerWonAnimation
  {
    get => !this.isLamb ? "knucklebones/knucklebones-goat/win-game" : "knucklebones/win-game";
  }

  public override string _playerWonLoop
  {
    get
    {
      return !this.isLamb ? "knucklebones/knucklebones-goat/win-game-loop" : "knucklebones/win-game-loop";
    }
  }

  public override string _playerLostAnimation
  {
    get => !this.isLamb ? "knucklebones/knucklebones-goat/lose-game" : "knucklebones/lose-game";
  }

  public override string _playerLostLoop
  {
    get
    {
      return !this.isLamb ? "knucklebones/knucklebones-goat/lose-game-loop" : "knucklebones/lose-game-loop";
    }
  }

  public override SkeletonGraphic _spine => this._lambSpine;

  public bool isLamb => this.playerFarming.isLamb && !this.playerFarming.IsGoat;

  public override void Configure(Vector2 contentOffscreenOffset, Vector2 tubOffscreenOffset)
  {
    this._playerName = this.GetLocalizedName();
    this._tubSelectables = new MMButton[this._diceTubs.Count];
    for (int index = 0; index < this._diceTubs.Count; ++index)
    {
      this._tubSelectables[index] = this._diceTubs[index].GetComponent<MMButton>();
      this._tubSelectables[index].interactable = false;
    }
    if (this.playerFarming.isLamb && !this.playerFarming.IsGoat)
    {
      if (this._otherCoopPlayer.gameObject.activeSelf)
      {
        for (int index = 0; index < this._diceTubs.Count; ++index)
          this._diceTubs[index].OpponentTub = this._otherCoopPlayer._diceTubs[index];
      }
      else if (this._twitchChat.gameObject.activeSelf)
      {
        for (int index = 0; index < this._diceTubs.Count; ++index)
          this._diceTubs[index].OpponentTub = this._twitchChat.DiceTubs[index];
      }
    }
    else
    {
      Skin newSkin = new Skin("skin");
      newSkin.AddSkin(this._spine.Skeleton.Data.FindSkin("JustHead"));
      newSkin.AddSkin(this._spine.Skeleton.Data.FindSkin("Goat"));
      this._spine.Skeleton.SetSkin(newSkin);
    }
    base.Configure(contentOffscreenOffset, tubOffscreenOffset);
  }

  public void Configure(
    PlayerFarming playerFarming,
    Vector2 contentOffscreenOffset,
    Vector2 tubOffscreenOffset)
  {
    this.playerFarming = playerFarming;
    this.Configure(contentOffscreenOffset, tubOffscreenOffset);
  }

  public override IEnumerator SelectTub(Dice dice)
  {
    KBPlayer kbPlayer = this;
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    KBPlayer.\u003C\u003Ec__DisplayClass27_0 cDisplayClass270 = new KBPlayer.\u003C\u003Ec__DisplayClass27_0();
    // ISSUE: reference to a compiler-generated field
    cDisplayClass270.\u003C\u003E4__this = this;
    MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer = kbPlayer.playerFarming;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass270.diceTub = (KBDiceTub) null;
    foreach (MMButton tubSelectable in kbPlayer._tubSelectables)
    {
      MMButton button = tubSelectable;
      button.interactable = true;
      // ISSUE: reference to a compiler-generated method
      button.onClick.AddListener((UnityAction) (() => cDisplayClass270.\u003CSelectTub\u003Eg__Choose\u007C0(this._tubSelectables.IndexOf<MMButton>(button))));
    }
    MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) kbPlayer._tubSelectables[1]);
    // ISSUE: reference to a compiler-generated field
    cDisplayClass270.chosen = false;
    // ISSUE: reference to a compiler-generated field
    while (!cDisplayClass270.chosen)
      yield return (object) null;
    foreach (MMButton tubSelectable in kbPlayer._tubSelectables)
    {
      tubSelectable.interactable = false;
      tubSelectable.onClick.RemoveAllListeners();
    }
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    // ISSUE: reference to a compiler-generated field
    yield return (object) kbPlayer.FinishTubSelection(dice, cDisplayClass270.diceTub);
  }

  public override string GetLocalizedName()
  {
    return !this.playerFarming.isLamb || this.playerFarming.IsGoat ? LocalizationManager.GetTranslation("NAMES/Knucklebones/Goat") : ScriptLocalization.NAMES_Knucklebones.Player;
  }

  public void OnDisable() => this.HideTubs();

  public void HideTubs() => this._diceTubs[0].transform.parent.gameObject.SetActive(false);
}
