// Decompiled with JetBrains decompiler
// Type: RatauGiveHealing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class RatauGiveHealing : BaseMonoBehaviour
{
  public Interaction_SimpleConversation FollowUpConversation;
  public CanvasGroup ControlsHUD;
  public spineChangeAnimationSimple ChangeAnimation;
  public SkeletonAnimation RatauSpine;
  public EventInstance receiveLoop;

  public void OnEnable()
  {
    this.ControlsHUD.alpha = 0.0f;
    this.FollowUpConversation.gameObject.SetActive(false);
  }

  public void Start()
  {
    DataManager.Instance.EnabledSword = false;
    DataManager.Instance.EnabledSpells = false;
  }

  public void GiveHealing()
  {
    DataManager.Instance.EnabledHealing = true;
    this.StartCoroutine((IEnumerator) this.WaitForHealing());
  }

  public IEnumerator WaitForHealing()
  {
    RatauGiveHealing ratauGiveHealing = this;
    yield return (object) new WaitForEndOfFrame();
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone, 6f);
    yield return (object) null;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "gameover-fast", true);
    ratauGiveHealing.RatauSpine.AnimationState.SetAnimation(0, "warning", true);
    AudioManager.Instance.PlayOneShot("event:/player/receive_animation_start", PlayerFarming.Instance.gameObject);
    ratauGiveHealing.receiveLoop = AudioManager.Instance.CreateLoop("event:/player/receive_animation_loop", PlayerFarming.Instance.gameObject, true);
    float Progress = 0.0f;
    float Duration = 3.66666675f;
    float StartingZoom = GameManager.GetInstance().CamFollowTarget.distance;
    while ((double) (Progress += Time.deltaTime) < (double) Duration - 0.5)
    {
      GameManager.GetInstance().CameraSetZoom(Mathf.Lerp(StartingZoom, 4f, Progress / Duration));
      if (Time.frameCount % 10 == 0)
        SoulCustomTarget.Create(PlayerFarming.Instance.gameObject, ratauGiveHealing.gameObject.transform.position, Color.black, (System.Action) null, 0.2f);
      yield return (object) null;
    }
    yield return (object) new WaitForSeconds(0.5f);
    ratauGiveHealing.RatauSpine.AnimationState.SetAnimation(0, "idle", true);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone, 4f);
    PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "specials/special-activate", false);
    yield return (object) new WaitForSeconds(1f);
    UIAbilityUnlock.Play(UIAbilityUnlock.Ability.Heal);
    yield return (object) new WaitForSeconds(0.5f);
    PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "idle", true);
    AudioManager.Instance.PlayOneShot("event:/player/receive_animation_end", PlayerFarming.Instance.gameObject);
    int num1 = (int) ratauGiveHealing.receiveLoop.stop(STOP_MODE.ALLOWFADEOUT);
    GameManager.GetInstance().OnConversationEnd();
    HUD_Manager.Instance.Show(0);
    Progress = 0.0f;
    Duration = 0.5f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      ratauGiveHealing.ControlsHUD.alpha = Mathf.Lerp(0.0f, 1f, Progress / Duration);
      yield return (object) null;
    }
    HealthPlayer healthPlayer = PlayerFarming.Instance.GetComponent<HealthPlayer>();
    float num2 = healthPlayer.HP + healthPlayer.SpiritHearts;
    string str1 = num2.ToString();
    num2 = healthPlayer.totalHP + healthPlayer.TotalSpiritHearts;
    string str2 = num2.ToString();
    Debug.Log((object) $"{str1} - {str2}");
    num2 = healthPlayer.TotalSpiritHearts;
    if ((double) num2 != 0.0)
    {
      if ((double) num2 == 1.0)
      {
        healthPlayer.SpiritHearts = healthPlayer.TotalSpiritHearts - 1f;
        healthPlayer.HP = healthPlayer.totalHP - 1f;
      }
      else
      {
        healthPlayer.SpiritHearts = healthPlayer.TotalSpiritHearts - 2f;
        healthPlayer.HP = healthPlayer.totalHP;
      }
    }
    else
      healthPlayer.HP = healthPlayer.totalHP - 2f;
    while ((double) healthPlayer.HP + (double) healthPlayer.SpiritHearts < (double) healthPlayer.totalHP + (double) healthPlayer.TotalSpiritHearts || PlayerFarming.Instance.state.CURRENT_STATE == StateMachine.State.Heal)
    {
      PlayerFarming.ReloadAllFaith();
      yield return (object) null;
    }
    yield return (object) new WaitForSeconds(0.5f);
    ratauGiveHealing.ControlsHUD.alpha = 0.0f;
    ratauGiveHealing.FollowUpConversation.gameObject.SetActive(true);
  }

  public void EndConversation()
  {
    DataManager.Instance.EnabledSword = true;
    DataManager.Instance.EnabledSpells = true;
    RoomLockController.RoomCompleted();
    PlayerFarming.ReloadAllFaith();
    this.ChangeAnimation.Play();
  }
}
