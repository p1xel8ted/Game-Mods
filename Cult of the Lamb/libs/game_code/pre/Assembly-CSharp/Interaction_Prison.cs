// Decompiled with JetBrains decompiler
// Type: Interaction_Prison
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.PrisonerMenu;
using src.Extensions;
using src.UI.Menus;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_Prison : Interaction
{
  public Structure Structure;
  private Structures_Prison _StructureInfo;
  [SerializeField]
  private GameObject playerPosition;
  private EventInstance LoopInstance;
  private float count;
  private UIPrisonerMenuController prisonerMenu;

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public Structures_Prison structureBrain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_Prison;
      return this._StructureInfo;
    }
    set => this._StructureInfo = value;
  }

  public int PrisonerID => this.StructureInfo.FollowerID;

  private void Awake() => this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);

  private void Start()
  {
    this.ActivateDistance = 2f;
    this.UpdateLocalisation();
  }

  private void OnBrainAssigned()
  {
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    if (DataManager.Instance.Followers_Imprisoned_IDs.Contains(this.StructureInfo.FollowerID))
      return;
    this.StructureInfo.FollowerID = -1;
  }

  public override void GetLabel()
  {
    this.Label = this.StructureInfo == null || FollowerInfo.GetInfoByID(this.StructureInfo.FollowerID) == null && this.StructureInfo.FollowerID != -1 ? "" : ScriptLocalization.Structures.PRISON;
  }

  public override void GetSecondaryLabel()
  {
  }

  public override void OnSecondaryInteract(StateMachine state)
  {
  }

  private IEnumerator ReEducate()
  {
    Interaction_Prison interactionPrison = this;
    yield return (object) new WaitForSeconds(0.25f);
    interactionPrison.count = 0.0f;
    PlayerFarming.Instance.GoToAndStop(interactionPrison.playerPosition, interactionPrison.gameObject, true);
    while (PlayerFarming.Instance.GoToAndStopping)
    {
      if ((double) interactionPrison.count > 60.0)
        PlayerFarming.Instance.EndGoToAndStop();
      ++interactionPrison.count;
      yield return (object) null;
    }
    Follower prisoner = FollowerManager.FindFollowerByID(interactionPrison.StructureInfo.FollowerID);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    GameManager.GetInstance().OnConversationNext(interactionPrison.gameObject, 8f);
    AudioManager.Instance.PlayOneShot("event:/dialogue/followers/general_acknowledge", interactionPrison.gameObject);
    AudioManager.Instance.PlayOneShot("event:/sermon/book_pickup", interactionPrison.gameObject);
    PlayerFarming.Instance.Spine.state.SetAnimation(0, "build", true);
    yield return (object) new WaitForSeconds(0.3f);
    AudioManager.Instance.PlayOneShot("event:/sermon/sermon_speech_bubble", interactionPrison.gameObject);
    interactionPrison.LoopInstance = AudioManager.Instance.CreateLoop("event:/sermon/preach_loop", interactionPrison.gameObject);
    prisoner.Spine.state.SetAnimation(0, "Prison/stocks-reeducate", false);
    double num = (double) prisoner.SetBodyAnimation("Prison/stocks-reeducate", false);
    prisoner.AddBodyAnimation("Prison/stocks", false, 0.0f);
    yield return (object) new WaitForSeconds(2f);
    prisoner.SetFaceAnimation("Reactions/react-brainwashed", false);
    AudioManager.Instance.StopLoop(interactionPrison.LoopInstance);
    CameraManager.shakeCamera(1f, (float) UnityEngine.Random.Range(0, 360));
    AudioManager.Instance.PlayOneShot("event:/sermon/end_sermon", interactionPrison.gameObject);
    prisoner.Spine.state.AddAnimation(0, "Prison/stocks", true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/dialogue/followers/positive_acknowledge", interactionPrison.gameObject);
    AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", interactionPrison.gameObject.transform.position);
    BiomeConstants.Instance.EmitHeartPickUpVFX(prisoner.gameObject.transform.position, 0.0f, "black", "burst_big");
    interactionPrison.structureBrain.Reeducate(prisoner.Brain);
    prisoner.Brain.Stats.ReeducatedAction = true;
    interactionPrison.ReeducatedThoughts(prisoner);
    interactionPrison.ReeducatedThoughts(prisoner);
    yield return (object) new WaitForSeconds(2f);
    prisoner.SetFaceAnimation("Reactions/react-loved", false);
    GameManager.GetInstance().OnConversationEnd();
    AudioManager.Instance.PlayOneShot("event:/sermon/book_put_down", interactionPrison.gameObject);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    GameManager.GetInstance().OnConversationNew();
    Time.timeScale = 0.0f;
    HUD_Manager.Instance.Hide(false, 0);
    if (this.StructureInfo.FollowerID != -1)
    {
      this.prisonerMenu = MonoSingleton<UIManager>.Instance.PrisonerMenuTemplate.Instantiate<UIPrisonerMenuController>();
      this.prisonerMenu.Show(FollowerInfo.GetInfoByID(this.StructureInfo.FollowerID), this.StructureInfo);
      this.prisonerMenu.OnFollowerReleased += (System.Action<FollowerInfo>) (followerInfo => this.ReleaseFollower());
      this.prisonerMenu.OnReEducate += (System.Action<FollowerInfo>) (followerInfo =>
      {
        Follower followerById = FollowerManager.FindFollowerByID(followerInfo.ID);
        if (!((UnityEngine.Object) followerById != (UnityEngine.Object) null) || followerById.Brain.Stats.ReeducatedAction)
          return;
        this.prisonerMenu.Hide();
        this.StartCoroutine((IEnumerator) this.ReEducate());
      });
      UIPrisonerMenuController prisonerMenu1 = this.prisonerMenu;
      prisonerMenu1.OnHidden = prisonerMenu1.OnHidden + (System.Action) (() => Time.timeScale = 1f);
      UIPrisonerMenuController prisonerMenu2 = this.prisonerMenu;
      prisonerMenu2.OnCancel = prisonerMenu2.OnCancel + (System.Action) (() => GameManager.GetInstance().OnConversationEnd());
    }
    else
    {
      UIPrisonMenuController prisonMenuController = MonoSingleton<UIManager>.Instance.PrisonMenuTemplate.Instantiate<UIPrisonMenuController>();
      prisonMenuController.Show(Prison.ImprisonableFollowers(), followerSelectionType: UpgradeSystem.Type.Building_Prison);
      prisonMenuController.OnFollowerSelected = prisonMenuController.OnFollowerSelected + (System.Action<FollowerInfo>) (followerInfo =>
      {
        this.ImprisonFollower(FollowerBrain.GetOrCreateBrain(followerInfo));
        BiomeConstants.Instance?.EmitSmokeExplosionVFX(this.gameObject.transform.position);
        AudioManager.Instance.PlayOneShot("event:/dialogue/followers/general_acknowledge", this.gameObject);
      });
      prisonMenuController.OnHidden = prisonMenuController.OnHidden + (System.Action) (() => Time.timeScale = 1f);
      prisonMenuController.OnCancel = prisonMenuController.OnCancel + (System.Action) (() => GameManager.GetInstance().OnConversationEnd());
    }
  }

  private void ImprisonFollower(FollowerBrain follower)
  {
    this.StructureInfo.FollowerID = follower.Info.ID;
    this.StructureInfo.FollowerImprisonedTimestamp = TimeManager.TotalElapsedGameTime;
    this.StructureInfo.FollowerImprisonedFaith = follower.Stats.Reeducation;
    this.StartCoroutine((IEnumerator) this.ImprisonFollowerIE(follower));
  }

  public void ReleaseFollower() => this.StartCoroutine((IEnumerator) this.ReleaseFollowerIE());

  private IEnumerator ReleaseFollowerIE()
  {
    yield return (object) new WaitForEndOfFrame();
    if (this.StructureInfo != null)
    {
      FollowerInfo infoById = FollowerInfo.GetInfoByID(this.StructureInfo.FollowerID);
      if (infoById != null)
      {
        FollowerBrain brain = FollowerBrain.GetOrCreateBrain(infoById);
        if (brain != null)
        {
          Follower f = FollowerManager.FindFollowerByID(this.StructureInfo.FollowerID);
          if (!((UnityEngine.Object) f == (UnityEngine.Object) null))
          {
            GameManager.GetInstance().OnConversationNew();
            GameManager.GetInstance().OnConversationNext(f.gameObject, 5f);
            yield return (object) new WaitForSeconds(1f);
            AudioManager.Instance.PlayOneShot("event:/followers/imprison", f.transform.position);
            brain.CompleteCurrentTask();
            DataManager.Instance.Followers_Imprisoned_IDs.Remove(brain.Info.ID);
            this.StructureInfo.FollowerID = -1;
            yield return (object) new WaitForSeconds(1f);
            GameManager.GetInstance().OnConversationEnd();
          }
        }
      }
    }
  }

  private IEnumerator ImprisonFollowerIE(FollowerBrain follower)
  {
    Interaction_Prison interactionPrison = this;
    yield return (object) new WaitForEndOfFrame();
    Follower followerById = FollowerManager.FindFollowerByID(follower.Info.ID);
    follower.CompleteCurrentTask();
    follower.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    followerById.transform.position = interactionPrison.GetComponent<Prison>().PrisonerLocation.position;
    interactionPrison.ImprisonedThoughts(followerById);
    AudioManager.Instance.PlayOneShot("event:/followers/imprison", followerById.transform.position);
    yield return (object) new WaitForEndOfFrame();
    follower.CompleteCurrentTask();
    follower.HardSwapToTask((FollowerTask) new FollowerTask_Imprisoned(interactionPrison.StructureInfo.ID));
    if (!DataManager.Instance.Followers_Imprisoned_IDs.Contains(follower.Info.ID))
      DataManager.Instance.Followers_Imprisoned_IDs.Add(follower.Info.ID);
    if (follower.Info.CursedState != Thought.Dissenter)
    {
      bool flag = false;
      foreach (ObjectivesData objective in DataManager.Instance.Objectives)
      {
        if (objective.Type == Objectives.TYPES.CUSTOM && ((Objectives_Custom) objective).CustomQuestType == Objectives.CustomQuestTypes.SendFollowerToPrison && ((Objectives_Custom) objective).TargetFollowerID == follower.Info.ID)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
      {
        if (DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.Disciplinarian))
          CultFaithManager.AddThought(Thought.Cult_Imprison_Trait);
        else
          CultFaithManager.AddThought(Thought.Cult_Imprison);
      }
    }
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.SendFollowerToPrison, follower.Info.ID);
    GameManager.GetInstance().OnConversationEnd();
  }

  private void ImprisonedThoughts(Follower prisoner)
  {
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.Location == prisoner.Brain.Location && allBrain != prisoner.Brain)
      {
        if ((double) allBrain.Stats.Reeducation > 0.0)
        {
          if (allBrain.CurrentTaskType == FollowerTaskType.Sleep)
            allBrain.AddThought(Thought.DissenterImprisonedSleeping);
          else if (allBrain.HasTrait(FollowerTrait.TraitType.Libertarian))
            allBrain.AddThought(Thought.ImprisonedLibertarian);
          else
            allBrain.AddThought(Thought.DissenterImprisoned);
        }
        else if (allBrain.CurrentTaskType == FollowerTaskType.Sleep)
          allBrain.AddThought(Thought.InnocentImprisonedSleeping);
        else if (allBrain.HasTrait(FollowerTrait.TraitType.Disciplinarian))
          allBrain.AddThought(Thought.InnocentImprisonedDisciplinarian);
        else if (allBrain.HasTrait(FollowerTrait.TraitType.Libertarian))
          allBrain.AddThought(Thought.ImprisonedLibertarian);
        else
          allBrain.AddThought(Thought.InnocentImprisoned);
      }
    }
  }

  public void ReeducatedThoughts(Follower prisoner)
  {
    if ((double) prisoner.Brain.Stats.Reeducation > 0.0)
    {
      foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
      {
        if (allBrain != prisoner.Brain)
        {
          if (allBrain.HasTrait(FollowerTrait.TraitType.Cynical))
          {
            if (allBrain.CurrentTaskType == FollowerTaskType.Sleep)
              allBrain.AddThought(Thought.DissenterCultMemberReeducatedSleepingCynicalTrait);
            else
              allBrain.AddThought(Thought.DissenterCultMemberReeducatedCynicalTrait);
          }
          else if (allBrain.HasTrait(FollowerTrait.TraitType.Gullible))
          {
            if (allBrain.CurrentTaskType == FollowerTaskType.Sleep)
              allBrain.AddThought(Thought.DissenterCultMemberReeducatedSleepingGullibleTrait);
            else
              allBrain.AddThought(Thought.DissenterCultMemberReeducatedGullibleTrait);
          }
          else if (allBrain.CurrentTaskType == FollowerTaskType.Sleep)
            allBrain.AddThought(Thought.DissenterCultMemberReeducatedSleeping);
          else
            allBrain.AddThought(Thought.DissenterCultMemberReeducated);
        }
      }
    }
    else
    {
      JudgementMeter.ShowModify(1);
      prisoner.Brain.AddThought(Thought.InnocentReeducated);
      foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
      {
        if (allBrain != prisoner.Brain)
        {
          if (allBrain.HasTrait(FollowerTrait.TraitType.Cynical))
          {
            if (allBrain.CurrentTaskType == FollowerTaskType.Sleep)
              allBrain.AddThought(Thought.InnocentCultMemberReeducatedCynicalTraitSleeping);
            else
              allBrain.AddThought(Thought.InnocentCultMemberReeducatedCynicalTrait);
          }
          else if (allBrain.HasTrait(FollowerTrait.TraitType.Gullible))
          {
            if (allBrain.CurrentTaskType == FollowerTaskType.Sleep)
              allBrain.AddThought(Thought.InnocentCultMemberReeducatedGullibleTraitSleeping);
            else
              allBrain.AddThought(Thought.InnocentCultMemberReeducatedGullibleTrait);
          }
          else if (allBrain.CurrentTaskType == FollowerTaskType.Sleep)
            allBrain.AddThought(Thought.InnocentCultMemberReeducatedSleeping);
          else
            allBrain.AddThought(Thought.InnocentCultMemberReeducated);
        }
      }
    }
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    AudioManager.Instance.StopLoop(this.LoopInstance);
  }
}
