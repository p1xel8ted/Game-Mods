// Decompiled with JetBrains decompiler
// Type: PlayerReturnToBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using Lamb.UI;
using Lamb.UI.DeathScreen;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class PlayerReturnToBase : BaseMonoBehaviour
{
  private float Duration = 4f;
  private float Timer;
  public Image ProgressRing;
  private bool Meditating;
  private bool Active;
  private EventInstance loopingSoundInstance;
  private string holdTime = "hold_time";

  private void Start() => this.ProgressRing.fillAmount = 0.0f;

  private void Update()
  {
    if ((Object) PlayerFarming.Instance == (Object) null || this.Active || PlayerFarming.Location == FollowerLocation.Base || (Object) RespawnRoomManager.Instance != (Object) null && RespawnRoomManager.Instance.gameObject.activeSelf)
      return;
    switch (PlayerFarming.Instance.state.CURRENT_STATE)
    {
      case StateMachine.State.Idle:
      case StateMachine.State.Moving:
        if (InputManager.Gameplay.GetReturnToBaseButtonHeld() && !PlayerFarming.Instance.GoToAndStopping)
        {
          if (!this.Meditating)
            this.loopingSoundInstance = AudioManager.Instance.CreateLoop("event:/ui/hold_button_loop", this.gameObject, true);
          this.Meditating = true;
          PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.Meditate;
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
        if (!InputManager.Gameplay.GetReturnToBaseButtonHeld())
        {
          if ((double) this.Timer > 0.30000001192092896)
          {
            PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.Idle;
            break;
          }
          break;
        }
        if ((double) this.Timer / (double) this.Duration >= 1.0 && (Object) UIDeathScreenOverlayController.Instance == (Object) null)
        {
          AudioManager.Instance.StopLoop(this.loopingSoundInstance);
          MonoSingleton<UIManager>.Instance.ShowDeathScreenOverlay(UIDeathScreenOverlayController.Results.Escaped);
          AudioManager.Instance.PlayOneShot("event:/pentagram_platform/pentagram_platform_curse");
          AudioManager.Instance.PlayOneShot("event:/ui/heretics_defeated");
          AudioManager.Instance.PlayMusic("event:/music/game_over/game_over");
          this.Active = true;
          break;
        }
        break;
    }
    if (!(bool) (Object) PlayerFarming.Instance || PlayerFarming.Instance.state.CURRENT_STATE == StateMachine.State.Meditate)
      return;
    if ((double) this.Timer > 0.0)
    {
      this.Timer -= Time.deltaTime * 3f;
      if ((double) this.Timer <= 0.0)
        this.Timer = 0.0f;
    }
    this.ProgressRing.fillAmount = this.Timer / this.Duration;
    if (!this.Meditating)
      return;
    AudioManager.Instance.StopLoop(this.loopingSoundInstance);
    GameManager.GetInstance().CameraResetTargetZoom();
    this.Meditating = false;
  }
}
