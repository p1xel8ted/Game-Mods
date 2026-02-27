// Decompiled with JetBrains decompiler
// Type: RatauGiveSpells
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using Lamb.UI;
using Spine.Unity;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class RatauGiveSpells : BaseMonoBehaviour
{
  public Interaction_SimpleConversation FollowUpConversation;
  public CanvasGroup ControlsHUD;
  public spineChangeAnimationSimple ChangeAnimation;
  public SkeletonAnimation RatauSpine;
  public static RatauGiveSpells Instance;
  private int ShootCount;
  private EventInstance receiveLoop;
  public int DummyCount;
  public static System.Action OnDummyShot;

  private void OnEnable()
  {
    this.ControlsHUD.alpha = 0.0f;
    this.FollowUpConversation.gameObject.SetActive(false);
    RatauGiveSpells.Instance = this;
  }

  private void OnDisable()
  {
    if (!((UnityEngine.Object) RatauGiveSpells.Instance == (UnityEngine.Object) this))
      return;
    RatauGiveSpells.Instance = (RatauGiveSpells) null;
  }

  private void Start() => DataManager.Instance.EnabledSword = false;

  public void GiveSpells()
  {
    DataManager.Instance.EnabledSpells = true;
    HUD_Manager.Instance.Show(0);
    this.StartCoroutine((IEnumerator) this.WaitForShooting());
  }

  private void OnObjectiveComplete(string GroupID)
  {
    Debug.Log((object) ("Group ID: " + GroupID));
    if (!(GroupID == "Objectives/GroupTitles/RatauGiveCurse"))
      return;
    ObjectiveManager.OnObjectiveGroupCompleted -= new Action<string>(this.OnObjectiveComplete);
    this.StartCoroutine((IEnumerator) this.EndSequenceRoutine());
  }

  private IEnumerator WaitForShooting()
  {
    RatauGiveSpells ratauGiveSpells = this;
    yield return (object) new WaitForEndOfFrame();
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone, 6f);
    yield return (object) null;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "gameover-fast", true);
    ratauGiveSpells.RatauSpine.AnimationState.SetAnimation(0, "warning", true);
    AudioManager.Instance.PlayOneShot("event:/dialogue/ratau/ratau_song", ratauGiveSpells.RatauSpine.transform.position);
    AudioManager.Instance.PlayOneShot("event:/player/receive_animation_start", PlayerFarming.Instance.gameObject);
    ratauGiveSpells.receiveLoop = AudioManager.Instance.CreateLoop("event:/player/receive_animation_loop", PlayerFarming.Instance.gameObject, true);
    float Progress = 0.0f;
    float Duration = 3.66666675f;
    float StartingZoom = GameManager.GetInstance().CamFollowTarget.distance;
    while ((double) (Progress += Time.deltaTime) < (double) Duration - 0.5)
    {
      GameManager.GetInstance().CameraSetZoom(Mathf.Lerp(StartingZoom, 4f, Progress / Duration));
      if (Time.frameCount % 10 == 0)
        SoulCustomTarget.Create(PlayerFarming.Instance.gameObject, ratauGiveSpells.gameObject.transform.position, Color.black, (System.Action) null, 0.2f);
      yield return (object) null;
    }
    yield return (object) new WaitForSeconds(0.5f);
    ratauGiveSpells.RatauSpine.AnimationState.SetAnimation(0, "idle", true);
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone, 4f);
    PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "specials/special-activate", false);
    yield return (object) new WaitForSeconds(1f);
    AudioManager.Instance.PlayOneShot("event:/player/receive_animation_end", PlayerFarming.Instance.gameObject);
    int num = (int) ratauGiveSpells.receiveLoop.stop(STOP_MODE.ALLOWFADEOUT);
    PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "idle", true);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    GameManager.GetInstance().OnConversationEnd();
    PlayerFarming.Instance.playerSpells.SetSpell(EquipmentType.Fireball, 1);
    ++DataManager.Instance.CurrentRunCurseLevel;
    Progress = 0.0f;
    Duration = 0.5f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      ratauGiveSpells.ControlsHUD.alpha = Mathf.Lerp(0.0f, 1f, Progress / Duration);
      yield return (object) null;
    }
    AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.SpecialCombat);
    ObjectiveManager.OnObjectiveGroupCompleted += new Action<string>(ratauGiveSpells.OnObjectiveComplete);
    ObjectiveManager.Add((ObjectivesData) new Objectives_ShootDummy("Objectives/GroupTitles/RatauGiveCurse"));
    PlayerSpells.OnCastSpell += new System.Action(ratauGiveSpells.OnCastSpell);
  }

  public void DummyDestroyed()
  {
    ++this.DummyCount;
    System.Action onDummyShot = RatauGiveSpells.OnDummyShot;
    if (onDummyShot == null)
      return;
    onDummyShot();
  }

  private IEnumerator EndSequenceRoutine()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    RatauGiveSpells ratauGiveSpells = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      ratauGiveSpells.ControlsHUD.alpha = 0.0f;
      AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.StandardAmbience);
      ratauGiveSpells.FollowUpConversation.gameObject.SetActive(true);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    PlayerSpells.OnCastSpell -= new System.Action(ratauGiveSpells.OnCastSpell);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(0.5f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void EndConversation()
  {
    DataManager.Instance.EnabledSword = true;
    DataManager.Instance.RatauToGiveCurseNextRun = false;
    RoomLockController.RoomCompleted();
    FaithAmmo.Reload();
    this.ChangeAnimation.Play();
    if (!DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Fervor))
      return;
    MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Fervor);
  }

  private void OnCastSpell()
  {
    this.StartCoroutine((IEnumerator) this.DelayReload());
    this.RatauSpine.AnimationState.SetAnimation(0, "warning", false);
    this.RatauSpine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
  }

  private IEnumerator DelayReload()
  {
    yield return (object) new WaitForSeconds(0.3f);
    if ((double) FaithAmmo.Ammo < (double) FaithAmmo.Total * 0.30000001192092896)
      FaithAmmo.Reload();
  }

  private void OnDestroy()
  {
    PlayerSpells.OnCastSpell -= new System.Action(this.OnCastSpell);
    ObjectiveManager.OnObjectiveGroupCompleted -= new Action<string>(this.OnObjectiveComplete);
  }
}
