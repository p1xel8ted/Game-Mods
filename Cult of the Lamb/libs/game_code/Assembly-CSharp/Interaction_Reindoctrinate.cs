// Decompiled with JetBrains decompiler
// Type: Interaction_Reindoctrinate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using Spine.Unity;
using src.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_Reindoctrinate : Interaction
{
  [SerializeField]
  public GameObject _spriteRenderer;
  [SerializeField]
  public bool _unlockedUpgrade;
  public string _reindoctrinateFollower;
  public bool Activating;
  public OriginalFollowerLookData originalFollowerLookData;
  public Follower sacrificeFollower;
  public FollowerTask_ManualControl Task;
  public BiomeLightingSettings LightingSettings;
  public OverrideLightingProperties overrideLightingProperties;
  public List<MeshRenderer> FollowersTurnedOff = new List<MeshRenderer>();

  public void Start()
  {
    this.UpdateLocalisation();
    if (!UpgradeSystem.UnlockedUpgrades.Contains(UpgradeSystem.Type.Building_UpgradedIndoctrination))
      return;
    this.Init();
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.Interactable = false;
    this._spriteRenderer.SetActive(false);
    this._unlockedUpgrade = false;
    UpgradeSystem.OnAbilityUnlocked += new System.Action<UpgradeSystem.Type>(this.OnAbilityUnlocked);
    if (!UpgradeSystem.UnlockedUpgrades.Contains(UpgradeSystem.Type.Building_UpgradedIndoctrination))
      return;
    this.Init();
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    UpgradeSystem.OnAbilityUnlocked -= new System.Action<UpgradeSystem.Type>(this.OnAbilityUnlocked);
  }

  public void OnAbilityUnlocked(UpgradeSystem.Type obj)
  {
    if (obj != UpgradeSystem.Type.Building_UpgradedIndoctrination)
      return;
    this.Init();
  }

  public void Init()
  {
    this._spriteRenderer.SetActive(true);
    this._unlockedUpgrade = true;
  }

  public override void Update()
  {
    base.Update();
    if (DataManager.Instance.followerRecruitWaiting)
      this.Interactable = false;
    else
      this.Interactable = true;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this._reindoctrinateFollower = ScriptLocalization.Interactions.ReIndoctrinateFollower;
  }

  public override void GetLabel()
  {
    if (FollowerManager.FollowersAtLocation(PlayerFarming.Location).Count <= 0 || !UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_UpgradedIndoctrination) || DataManager.Instance.Followers_Recruit.Count > 0)
    {
      this.Label = "";
      this.Interactable = false;
    }
    else
    {
      this.Label = $"{this._reindoctrinateFollower} {InventoryItem.CapacityString(InventoryItem.ITEM_TYPE.PLEASURE_POINT, 1)}";
      this.Interactable = true;
    }
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.PLEASURE_POINT) < 1)
    {
      this.playerFarming.indicator.PlayShake();
    }
    else
    {
      if (this.Activating)
        return;
      this.Activating = true;
      this.Interactable = false;
      GameManager.GetInstance().OnConversationNew(this.playerFarming);
      GameManager.GetInstance().OnConversationNext(this.playerFarming.CameraBone, 8f);
      if (FollowerManager.FollowersAtLocation(PlayerFarming.Location).Count <= 0 || DataManager.Instance.Followers.Count <= 0)
      {
        GameManager.GetInstance().OnConversationEnd();
        this.Activating = false;
      }
      else
        this.playerFarming.GoToAndStop(this.transform.position - new Vector3(2f, 0.0f, 0.0f), this.gameObject, true, GoToCallback: (System.Action) (() => GameManager.GetInstance().WaitForSeconds(0.2f, (System.Action) (() =>
        {
          Time.timeScale = 0.0f;
          state.CURRENT_STATE = StateMachine.State.InActive;
          List<FollowerSelectEntry> followerSelectEntries = new List<FollowerSelectEntry>();
          foreach (Follower follower in Follower.Followers)
          {
            if (follower.Brain._directInfoAccess.IsSnowman)
              followerSelectEntries.Add(new FollowerSelectEntry(follower, FollowerSelectEntry.Status.Unavailable));
            else
              followerSelectEntries.Add(new FollowerSelectEntry(follower, FollowerManager.GetFollowerAvailabilityStatus(follower.Brain)));
          }
          followerSelectEntries.Sort((Comparison<FollowerSelectEntry>) ((a, b) => b.FollowerInfo.XPLevel.CompareTo(a.FollowerInfo.XPLevel)));
          UIFollowerSelectMenuController followerSelectInstance = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
          followerSelectInstance.VotingType = TwitchVoting.VotingType.CONFESSION;
          followerSelectInstance.Show(followerSelectEntries, false, UpgradeSystem.Type.Count, true, true, true, false, true);
          followerSelectInstance.OnFollowerSelected += (System.Action<FollowerInfo>) (followerInfo =>
          {
            Inventory.ChangeItemQuantity(InventoryItem.ITEM_TYPE.PLEASURE_POINT, -1);
            SimulationManager.Pause();
            Time.timeScale = 1f;
            this.sacrificeFollower = FollowerManager.FindFollowerByID(followerInfo.ID);
            this.originalFollowerLookData = new OriginalFollowerLookData(followerInfo);
            if (TimeManager.IsNight && this.sacrificeFollower.Brain.CurrentTask != null && this.sacrificeFollower.Brain.CurrentTask.State == FollowerTaskState.Doing && (this.sacrificeFollower.Brain.CurrentTaskType == FollowerTaskType.Sleep || this.sacrificeFollower.Brain.CurrentTaskType == FollowerTaskType.SleepBedRest))
              CultFaithManager.AddThought(Thought.Cult_WokeUpFollower, this.sacrificeFollower.Brain.Info.ID);
            this.sacrificeFollower.Brain.CurrentTask?.Abort();
            this.Task = new FollowerTask_ManualControl();
            this.sacrificeFollower.Brain.HardSwapToTask((FollowerTask) this.Task);
            GameManager.GetInstance().OnConversationNext(this.sacrificeFollower.gameObject);
            this.sacrificeFollower.transform.position = this.transform.position;
            this.StartCoroutine((IEnumerator) this.SimpleNewRecruitRoutine(true));
            followerSelectInstance.Hide();
            GameManager.GetInstance().WaitForSeconds(0.0f, (System.Action) (() => this.sacrificeFollower.FacePosition(this.playerFarming.gameObject.transform.position)));
          });
          followerSelectInstance.OnCancel += (System.Action) (() =>
          {
            GameManager.GetInstance().OnConversationEnd();
            this.Activating = false;
            Time.timeScale = 1f;
            this.Activating = false;
          });
          followerSelectInstance.OnHidden += (System.Action) (() => followerSelectInstance = (UIFollowerSelectMenuController) null);
        }))));
    }
  }

  public IEnumerator PositionCharacters(GameObject Character, Vector3 TargetPosition)
  {
    float Progress = 0.0f;
    float Duration = 0.3f;
    Vector3 StartingPosition = Character.transform.position;
    while ((double) (Progress += Time.unscaledDeltaTime) < (double) Duration)
    {
      Character.transform.position = Vector3.Lerp(StartingPosition, TargetPosition, Progress / Duration);
      yield return (object) null;
    }
    Character.transform.position = TargetPosition;
  }

  public IEnumerator SimpleNewRecruitRoutine(bool customise)
  {
    Interaction_Reindoctrinate interactionReindoctrinate = this;
    GameManager.GetInstance().OnConversationNext(interactionReindoctrinate.sacrificeFollower.gameObject, 4f);
    interactionReindoctrinate.playerFarming.state.facingAngle = Utils.GetAngle(interactionReindoctrinate.transform.position, interactionReindoctrinate.sacrificeFollower.transform.position);
    yield return (object) new WaitForSeconds(0.3f);
    if (customise)
    {
      interactionReindoctrinate.FollowersTurnedOff.Clear();
      foreach (Follower locationFollower in FollowerManager.ActiveLocationFollowers())
      {
        SkeletonAnimation spine = locationFollower.Spine;
        if (spine.gameObject.activeSelf && (double) Vector3.Distance(spine.transform.position, interactionReindoctrinate.transform.position) < 1.0 && (double) spine.transform.position.y < (double) interactionReindoctrinate.transform.position.y)
        {
          Debug.Log((object) ("Turning off gameobject: " + spine.name));
          MeshRenderer component = spine.gameObject.GetComponent<MeshRenderer>();
          component.enabled = false;
          interactionReindoctrinate.FollowersTurnedOff.Add(component);
        }
      }
      GameManager.GetInstance().CameraSetOffset(new Vector3(-2f, 0.0f, 0.0f));
      UIFollowerIndoctrinationMenuController indoctrinationMenuInstance = MonoSingleton<UIManager>.Instance.ShowIndoctrinationMenu(interactionReindoctrinate.sacrificeFollower, interactionReindoctrinate.originalFollowerLookData);
      indoctrinationMenuInstance.OnIndoctrinationCompleted += new System.Action(interactionReindoctrinate.\u003CSimpleNewRecruitRoutine\u003Eb__20_0);
      UIFollowerIndoctrinationMenuController indoctrinationMenuController1 = indoctrinationMenuInstance;
      indoctrinationMenuController1.OnShown = indoctrinationMenuController1.OnShown + new System.Action(interactionReindoctrinate.\u003CSimpleNewRecruitRoutine\u003Eb__20_1);
      UIFollowerIndoctrinationMenuController indoctrinationMenuController2 = indoctrinationMenuInstance;
      indoctrinationMenuController2.OnHide = indoctrinationMenuController2.OnHide + new System.Action(interactionReindoctrinate.\u003CSimpleNewRecruitRoutine\u003Eb__20_2);
      UIFollowerIndoctrinationMenuController indoctrinationMenuController3 = indoctrinationMenuInstance;
      indoctrinationMenuController3.OnHidden = indoctrinationMenuController3.OnHidden + (System.Action) (() => indoctrinationMenuInstance = (UIFollowerIndoctrinationMenuController) null);
    }
    else
      interactionReindoctrinate.StartCoroutine((IEnumerator) interactionReindoctrinate.CharacterSetupCallback());
  }

  public IEnumerator CharacterSetupCallback()
  {
    Interaction_Reindoctrinate interactionReindoctrinate = this;
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    interactionReindoctrinate.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    interactionReindoctrinate.playerFarming.simpleSpineAnimator.Animate("recruit", 0, false);
    interactionReindoctrinate.playerFarming.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    interactionReindoctrinate.playerFarming.state.facingAngle = Utils.GetAngle(interactionReindoctrinate.transform.position, interactionReindoctrinate.sacrificeFollower.transform.position);
    yield return (object) new WaitForSeconds(0.5f);
    AudioManager.Instance.PlayOneShot("event:/dialogue/followers/positive_acknowledge", interactionReindoctrinate.sacrificeFollower.gameObject);
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", interactionReindoctrinate.playerFarming.gameObject);
    double num = (double) interactionReindoctrinate.sacrificeFollower.SetBodyAnimation("Indoctrinate/indoctrinate-finish", false);
    yield return (object) new WaitForSeconds(4f);
    interactionReindoctrinate.sacrificeFollower.SimpleAnimator?.ResetAnimationsToDefaults();
    yield return (object) new WaitForEndOfFrame();
    interactionReindoctrinate.state.CURRENT_STATE = StateMachine.State.Idle;
    GameManager.GetInstance().OnConversationEnd();
    yield return (object) null;
    SimulationManager.UnPause();
    interactionReindoctrinate.OnRecruitFinished();
  }

  public void OnRecruitFinished()
  {
    GameManager.GetInstance().OnConversationEnd();
    this.sacrificeFollower.Brain.CompleteCurrentTask();
    this.Activating = false;
    SimulationManager.UnPause();
    FollowerBrain.SetFollowerCostume(this.sacrificeFollower.Spine.Skeleton, this.sacrificeFollower.Brain._directInfoAccess, forceUpdate: true);
    TwitchFollowers.SendFollowers();
  }

  [CompilerGenerated]
  public void \u003CSimpleNewRecruitRoutine\u003Eb__20_0()
  {
    this.StartCoroutine((IEnumerator) this.CharacterSetupCallback());
  }

  [CompilerGenerated]
  public void \u003CSimpleNewRecruitRoutine\u003Eb__20_1()
  {
    LightingManager.Instance.inOverride = true;
    this.LightingSettings.overrideLightingProperties = this.overrideLightingProperties;
    LightingManager.Instance.overrideSettings = this.LightingSettings;
    LightingManager.Instance.transitionDurationMultiplier = 0.0f;
    LightingManager.Instance.UpdateLighting(true);
  }

  [CompilerGenerated]
  public void \u003CSimpleNewRecruitRoutine\u003Eb__20_2()
  {
    foreach (Renderer renderer in this.FollowersTurnedOff)
      renderer.enabled = true;
    this.FollowersTurnedOff.Clear();
    LightingManager.Instance.inOverride = false;
    LightingManager.Instance.overrideSettings = (BiomeLightingSettings) null;
    LightingManager.Instance.transitionDurationMultiplier = 1f;
    LightingManager.Instance.lerpActive = false;
    LightingManager.Instance.UpdateLighting(true);
  }
}
