// Decompiled with JetBrains decompiler
// Type: src.UI.Testing.MockedPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace src.UI.Testing;

[RequireComponent(typeof (HealthPlayer), typeof (Interactor), typeof (PlayerAmmo))]
[RequireComponent(typeof (PlayerRelic), typeof (PlayerSpells), typeof (StateMachine))]
public class MockedPlayer : PlayerFarming
{
  [SerializeField]
  public MockedPlayer.Character _character;

  public new virtual void Awake()
  {
    this.health = this.GetComponent<HealthPlayer>();
    this.interactor = this.GetComponent<Interactor>();
    this.isLamb = this._character == MockedPlayer.Character.Lamb;
    this.IsGoat = this._character == MockedPlayer.Character.GoatSolo;
    if (!((Object) PlayerFarming.Instance == (Object) null) || this._character == MockedPlayer.Character.GoatCoop)
      return;
    PlayerFarming.Instance = (PlayerFarming) this;
  }

  public new virtual void OnEnable()
  {
    if (PlayerFarming.players.Contains((PlayerFarming) this))
      return;
    if (this._character == MockedPlayer.Character.GoatCoop)
    {
      PlayerFarming.players.Add((PlayerFarming) this);
    }
    else
    {
      PlayerFarming.players.Insert(0, (PlayerFarming) this);
      if ((Object) PlayerFarming.Instance == (Object) null)
        PlayerFarming.Instance = (PlayerFarming) this;
    }
    PlayerFarming.RefreshPlayersCount(false);
  }

  public new virtual void Start()
  {
  }

  public new virtual void Update()
  {
  }

  public new virtual void LateUpdate()
  {
  }

  public virtual void OnDisable()
  {
    if (PlayerFarming.players.Contains((PlayerFarming) this))
    {
      PlayerFarming.players.Remove((PlayerFarming) this);
      PlayerFarming.RefreshPlayersCount(false);
    }
    if (!((Object) PlayerFarming.Instance == (Object) this))
      return;
    PlayerFarming.Instance = PlayerFarming.players.Count <= 0 || !(PlayerFarming.players[0] is MockedPlayer player) || player._character == MockedPlayer.Character.GoatCoop ? (PlayerFarming) null : PlayerFarming.players[0];
  }

  public new virtual void OnDestroy()
  {
  }

  public enum Character
  {
    Lamb,
    GoatSolo,
    GoatCoop,
  }
}
