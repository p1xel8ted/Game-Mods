// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UpgradeMenu.RatauHomeShop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using I2.Loc;
using src.Extensions;
using src.UI.Overlays.TutorialOverlay;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

#nullable disable
namespace Lamb.UI.UpgradeMenu;

public class RatauHomeShop : Interaction
{
  [FormerlySerializedAs("shopMenuController")]
  [FormerlySerializedAs("shopMenu")]
  [SerializeField]
  public UIUpgradeShopMenuController _shopMenuControllerTemplate;
  public Transform DevotionSpawnPosition;
  public string sBuyCrownAbility;
  public EventInstance receiveLoop;

  public void Start() => this.UpdateLocalisation();

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sBuyCrownAbility = ScriptLocalization.Interactions.BuyCrownAbility;
  }

  public override void GetLabel() => this.Label = this.sBuyCrownAbility;

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.Play();
  }

  public void Play()
  {
    Time.timeScale = 0.0f;
    HUD_Manager.Instance.Hide(false, 0);
    UIUpgradeShopMenuController shopMenuController = this._shopMenuControllerTemplate.Instantiate<UIUpgradeShopMenuController>();
    shopMenuController.Show();
    shopMenuController.OnHide = shopMenuController.OnHide + (System.Action) (() => HUD_Manager.Instance.Show(0));
    shopMenuController.OnHidden = shopMenuController.OnHidden + (System.Action) (() =>
    {
      Time.timeScale = 1f;
      GameManager.GetInstance().StartCoroutine((IEnumerator) UpgradeSystem.ListOfUnlocksRoutine());
    });
    shopMenuController.OnUpgradeChosen += (System.Action<UpgradeSystem.Type>) (type =>
    {
      UpgradeSystem.UnlockAbility(type);
      this.StartCoroutine((IEnumerator) this.GetAbilityRoutine(type));
    });
  }

  public IEnumerator GetAbilityRoutine(UpgradeSystem.Type Type)
  {
    RatauHomeShop ratauHomeShop = this;
    yield return (object) new WaitForEndOfFrame();
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(ratauHomeShop.playerFarming.CameraBone, 6f);
    ratauHomeShop.playerFarming.GoToAndStop(new Vector3(ratauHomeShop.DevotionSpawnPosition.position.x, ratauHomeShop.DevotionSpawnPosition.position.y - 2f, 0.0f));
    while (ratauHomeShop.playerFarming.GoToAndStopping)
      yield return (object) null;
    GameManager.GetInstance().AddToCamera(ratauHomeShop.DevotionSpawnPosition.gameObject);
    ratauHomeShop.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    ratauHomeShop.playerFarming.Spine.AnimationState.SetAnimation(0, "gameover-fast", true);
    AudioManager.Instance.PlayOneShot("event:/player/receive_animation_start", ratauHomeShop.playerFarming.gameObject);
    ratauHomeShop.receiveLoop = AudioManager.Instance.CreateLoop("event:/player/receive_animation_loop", ratauHomeShop.playerFarming.gameObject, true);
    float Progress = 0.0f;
    float Duration = 3.66666675f;
    float StartingZoom = GameManager.GetInstance().CamFollowTarget.distance;
    while ((double) (Progress += Time.deltaTime) < (double) Duration - 0.5)
    {
      GameManager.GetInstance().CameraSetZoom(Mathf.Lerp(StartingZoom, 4f, Progress / Duration));
      if (Time.frameCount % 10 == 0)
        SoulCustomTarget.Create(ratauHomeShop.playerFarming.gameObject, ratauHomeShop.DevotionSpawnPosition.position, Color.black, (System.Action) null, 0.2f);
      yield return (object) null;
    }
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationNext(ratauHomeShop.playerFarming.CameraBone, 4f);
    ratauHomeShop.playerFarming.Spine.AnimationState.SetAnimation(0, "specials/special-activate", false);
    yield return (object) new WaitForSeconds(1f);
    AudioManager.Instance.PlayOneShot("event:/player/receive_animation_end", ratauHomeShop.playerFarming.gameObject);
    int num = (int) ratauHomeShop.receiveLoop.stop(STOP_MODE.ALLOWFADEOUT);
    ratauHomeShop.playerFarming.Spine.AnimationState.SetAnimation(0, "idle", true);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    GameManager.GetInstance().OnConversationEnd();
    UITutorialOverlayController TutorialOverlay = (UITutorialOverlayController) null;
    switch (Type)
    {
      case UpgradeSystem.Type.Ability_Eat:
        if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.TheHunger))
        {
          TutorialOverlay = MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.TheHunger);
          break;
        }
        break;
      case UpgradeSystem.Type.Ability_Resurrection:
        if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Resurrection))
        {
          TutorialOverlay = MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Resurrection);
          break;
        }
        break;
      case UpgradeSystem.Type.Ability_BlackHeart:
        if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.DarknessWithin))
        {
          TutorialOverlay = MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.DarknessWithin);
          break;
        }
        break;
      case UpgradeSystem.Type.Ability_TeleportHome:
        if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Omnipresence))
        {
          TutorialOverlay = MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Omnipresence);
          break;
        }
        break;
    }
    while ((UnityEngine.Object) TutorialOverlay != (UnityEngine.Object) null)
      yield return (object) null;
    yield return (object) new WaitForSeconds(0.5f);
  }

  [CompilerGenerated]
  public void \u003CPlay\u003Eb__7_2(UpgradeSystem.Type type)
  {
    UpgradeSystem.UnlockAbility(type);
    this.StartCoroutine((IEnumerator) this.GetAbilityRoutine(type));
  }
}
