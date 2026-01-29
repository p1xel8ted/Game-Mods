// Decompiled with JetBrains decompiler
// Type: PlayerReturnToBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using Lamb.UI;
using Lamb.UI.DeathScreen;
using src.UINavigator;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class PlayerReturnToBase : BaseMonoBehaviour
{
  [SerializeField]
  public CoopIndicatorIcon coopIndicatorIcon;
  public float Duration = 4f;
  public float Timer;
  public Image ProgressRing;
  public bool Active;
  public static bool s_Disabled;
  public EventInstance loopingSoundInstance;
  public string holdTime = "hold_time";
  public PlayerFarming currentMeditator;

  public static bool Disabled
  {
    get => PlayerReturnToBase.s_Disabled;
    set
    {
      Debug.Log((object) $"Omnipresence state change: Disabled = {value}");
      PlayerReturnToBase.s_Disabled = value;
    }
  }

  public void Start()
  {
    this.ProgressRing.fillAmount = 0.0f;
    this.coopIndicatorIcon.SetIcon(CoopIndicatorIcon.CoopIcon.Lamb);
  }

  public void Update()
  {
    if (PlayerReturnToBase.Disabled || (Object) PlayerFarming.Instance == (Object) null || this.Active || DungeonSandboxManager.Active || PlayerFarming.Location == FollowerLocation.Base || (Object) RespawnRoomManager.Instance != (Object) null && RespawnRoomManager.Instance.gameObject.activeSelf)
      return;
    PlayerFarming player = PlayerFarming.players[0];
    if (player.IsBurrowing || !((Object) this.currentMeditator == (Object) null) && !((Object) this.currentMeditator == (Object) player))
      return;
    switch (player.state.CURRENT_STATE)
    {
      case StateMachine.State.Idle:
      case StateMachine.State.Moving:
        if (InputManager.Gameplay.GetReturnToBaseButtonHeld(player) && !PlayerFarming.AnyPlayerGotoAndStopping() && !player.IsKnockedOut)
        {
          if (!player.returnToBaseMeditating)
            this.loopingSoundInstance = AudioManager.Instance.CreateLoop("event:/ui/hold_button_loop", this.gameObject, true);
          player.returnToBaseMeditating = true;
          this.currentMeditator = player;
          player.state.CURRENT_STATE = StateMachine.State.Meditate;
          GameManager.GetInstance().CameraSetTargetZoom(15f);
          break;
        }
        break;
      case StateMachine.State.Meditate:
        this.Timer += Time.deltaTime;
        this.ProgressRing.fillAmount = this.Timer / this.Duration;
        if (this.loopingSoundInstance.isValid())
        {
          int num = (int) this.loopingSoundInstance.setParameterByName(this.holdTime, this.Timer / this.Duration);
        }
        if (!InputManager.Gameplay.GetReturnToBaseButtonHeld(player))
        {
          if ((double) this.Timer > 0.30000001192092896)
          {
            player.state.CURRENT_STATE = StateMachine.State.Idle;
            break;
          }
          break;
        }
        if ((double) this.Timer / (double) this.Duration >= 1.0 && (Object) UIDeathScreenOverlayController.Instance == (Object) null)
        {
          AudioManager.Instance.StopLoop(this.loopingSoundInstance);
          if (player.isLamb)
          {
            MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer = player;
            MonoSingleton<UIManager>.Instance.ShowDeathScreenOverlay(UIDeathScreenOverlayController.Results.Escaped);
            AudioManager.Instance.PlayOneShot("event:/pentagram_platform/pentagram_platform_curse");
            AudioManager.Instance.PlayOneShot("event:/ui/heretics_defeated");
            AudioManager.Instance.PlayMusic("event:/music/game_over/game_over");
          }
          this.Active = true;
          break;
        }
        break;
    }
    if (!(bool) (Object) player || player.state.CURRENT_STATE == StateMachine.State.Meditate)
      return;
    if ((double) this.Timer > 0.0)
    {
      this.Timer -= Time.deltaTime * 3f;
      if ((double) this.Timer <= 0.0)
        this.Timer = 0.0f;
    }
    this.ProgressRing.fillAmount = this.Timer / this.Duration;
    if (!player.returnToBaseMeditating)
      return;
    AudioManager.Instance.StopLoop(this.loopingSoundInstance);
    GameManager.GetInstance().CameraResetTargetZoom();
    player.returnToBaseMeditating = false;
    this.currentMeditator = (PlayerFarming) null;
  }
}
