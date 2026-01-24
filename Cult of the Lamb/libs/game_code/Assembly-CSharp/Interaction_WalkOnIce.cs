// Decompiled with JetBrains decompiler
// Type: Interaction_WalkOnIce
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_WalkOnIce : Interaction
{
  [SerializeField]
  public GameObject upSprite;
  [SerializeField]
  public GameObject downSprite;
  public bool onIce;
  public string jumpOntoSeaIceSFX = "event:/dlc/player/mv_seaice_jump_down";
  public string jumpOffSeaIceSFX = "event:/dlc/player/mv_seaice_jump_up";

  public override void GetLabel()
  {
    base.GetLabel();
    if (this.onIce)
      this.Label = this.Interactable ? LocalizationManager.GetTranslation("Interactions/JumpUp") : "";
    else
      this.Label = this.Interactable ? LocalizationManager.GetTranslation("Interactions/JumpDown") : "";
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (this.onIce)
    {
      SeasonsManager.WinterEndingDisabled = false;
      if (PlayerFarming.players.Count > 1)
      {
        this.Interactable = false;
        this.HasChanged = true;
      }
      GameManager.GetInstance().CameraResetTargetZoom();
      this.StopAllCoroutines();
      PlayerFarming.Instance.TimedAction(0.4f, (System.Action) (() => PlayerFarming.Instance.unitObject.LockToGround = true), "oldstuff/roll-up-JUMPY");
      PlayerFarming.Instance.transform.DOMove(PlayerFarming.Instance.transform.position + Vector3.up * 3f + Vector3.back * 0.8f, 0.4f);
      if (SeasonsManager.CurrentWeatherEvent != SeasonsManager.WeatherEvent.Blizzard)
        SeasonsManager.SetSeasonDefaultWeather(SeasonsManager.CurrentSeason);
      foreach (PlayerFarming player1 in PlayerFarming.players)
      {
        PlayerFarming player = player1;
        player.playerController.ResetSpecialMovingAnimations(StateMachine.State.Idle_Winter);
        AudioManager.Instance.PlayOneShot(this.jumpOffSeaIceSFX, player.gameObject);
        if ((UnityEngine.Object) player != (UnityEngine.Object) PlayerFarming.Instance)
          player.GoToAndStop(this.transform.position + Vector3.up, GoToCallback: (System.Action) (() =>
          {
            player.TimedAction(0.4f, (System.Action) (() =>
            {
              player.unitObject.LockToGround = true;
              this.Interactable = true;
              this.HasChanged = true;
            }), "oldstuff/roll-up-JUMPY");
            player.transform.DOMove(player.transform.position + Vector3.up * 3f + Vector3.back * 0.8f, 0.4f);
          }));
      }
    }
    else
    {
      SeasonsManager.WinterEndingDisabled = true;
      if (PlayerFarming.players.Count > 1)
      {
        this.Interactable = false;
        this.HasChanged = true;
      }
      PlayerFarming.Instance.unitObject.LockToGround = false;
      PlayerFarming.Instance.TimedAction(0.4f, (System.Action) null, "oldstuff/roll-down-JUMPY");
      PlayerFarming.Instance.transform.DOMove(PlayerFarming.Instance.transform.position + Vector3.down * 3f + Vector3.forward * 0.8f, 0.4f);
      GameManager.GetInstance().CameraSetZoom(7.5f);
      foreach (PlayerFarming player2 in PlayerFarming.players)
      {
        PlayerFarming player = player2;
        AudioManager.Instance.PlayOneShot(this.jumpOntoSeaIceSFX, player.gameObject);
        if ((UnityEngine.Object) player != (UnityEngine.Object) PlayerFarming.Instance)
          player.GoToAndStop(this.transform.position + Vector3.down, GoToCallback: (System.Action) (() =>
          {
            player.unitObject.LockToGround = false;
            player.TimedAction(0.4f, (System.Action) (() =>
            {
              this.Interactable = true;
              this.HasChanged = true;
            }), "oldstuff/roll-up-JUMPY");
            player.transform.DOMove(player.transform.position + Vector3.down * 3f + Vector3.forward * 0.8f, 0.4f);
          }));
      }
      this.StartCoroutine((IEnumerator) this.WaitForSeconds(3f, (System.Action) (() =>
      {
        if (SeasonsManager.CurrentWeatherEvent != SeasonsManager.WeatherEvent.Blizzard)
          WeatherSystemController.Instance.SetWeather(WeatherSystemController.WeatherType.Snowing, WeatherSystemController.WeatherStrength.Extreme, 7f);
        foreach (PlayerFarming player in PlayerFarming.players)
          player.playerController.SetSpecialMovingAnimations("blizzard/idle", "blizzard/run-up", "blizzard/run-down", "blizzard/run", "blizzard/run-up-diagonal", "blizzard/run-horizontal", StateMachine.State.Idle_Winter);
        this.StartCoroutine((IEnumerator) this.WaitForSeconds(5f, (System.Action) (() =>
        {
          foreach (PlayerFarming player in PlayerFarming.players)
            player.state.CURRENT_STATE = player.state.CURRENT_STATE != StateMachine.State.Moving ? StateMachine.State.Idle_Winter : StateMachine.State.Moving_Winter;
        })));
      })));
    }
    this.onIce = !this.onIce;
    this.OutlineTarget = this.onIce ? this.upSprite : this.downSprite;
  }

  public IEnumerator WaitForSeconds(float duration, System.Action callback)
  {
    yield return (object) new UnityEngine.WaitForSeconds(duration);
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void SetPlayerPositionLow()
  {
    foreach (PlayerFarming player in PlayerFarming.players)
      player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, 0.8f);
    GameManager.GetInstance().CameraSetZoom(7.5f);
  }

  public void SetPlayerPositionNormal()
  {
    foreach (PlayerFarming player in PlayerFarming.players)
      player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, 0.0f);
    GameManager.GetInstance().CameraResetTargetZoom();
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__6_5()
  {
    this.Interactable = true;
    this.HasChanged = true;
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__6_1()
  {
    if (SeasonsManager.CurrentWeatherEvent != SeasonsManager.WeatherEvent.Blizzard)
      WeatherSystemController.Instance.SetWeather(WeatherSystemController.WeatherType.Snowing, WeatherSystemController.WeatherStrength.Extreme, 7f);
    foreach (PlayerFarming player in PlayerFarming.players)
      player.playerController.SetSpecialMovingAnimations("blizzard/idle", "blizzard/run-up", "blizzard/run-down", "blizzard/run", "blizzard/run-up-diagonal", "blizzard/run-horizontal", StateMachine.State.Idle_Winter);
    this.StartCoroutine((IEnumerator) this.WaitForSeconds(5f, (System.Action) (() =>
    {
      foreach (PlayerFarming player in PlayerFarming.players)
        player.state.CURRENT_STATE = player.state.CURRENT_STATE != StateMachine.State.Moving ? StateMachine.State.Idle_Winter : StateMachine.State.Moving_Winter;
    })));
  }
}
