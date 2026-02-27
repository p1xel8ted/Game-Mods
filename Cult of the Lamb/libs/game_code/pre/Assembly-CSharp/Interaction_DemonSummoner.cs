// Decompiled with JetBrains decompiler
// Type: Interaction_DemonSummoner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using Spine.Unity;
using src.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class Interaction_DemonSummoner : Interaction
{
  public Canvas UICanvas;
  public Image UIProgress;
  [SerializeField]
  private SkeletonAnimation[] demons;
  private Structures_Demon_Summoner _StructureInfo;
  private FollowerBrain follower;
  public GameObject OnEffects;
  public static string[] DemonSkins = new string[6]
  {
    "Projectile",
    "Chomp",
    "Arrows",
    "Heart",
    "Explode",
    "Spirit"
  };

  public Structure Structure { get; private set; }

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public Structures_Demon_Summoner structureBrain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_Demon_Summoner;
      return this._StructureInfo;
    }
    set => this._StructureInfo = value;
  }

  private void Start() => this.UpdateLocalisation();

  protected override void OnDestroy()
  {
    base.OnDestroy();
    if (!(bool) (UnityEngine.Object) this.Structure)
      return;
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    this.Structure.OnBrainRemoved -= new System.Action(this.OnBrainRemoved);
  }

  private void OnBrainAssigned()
  {
    foreach (Component demon in this.demons)
      demon.gameObject.SetActive(false);
    for (int index1 = this.StructureInfo.MultipleFollowerIDs.Count - 1; index1 >= 0; --index1)
    {
      if (this.StructureInfo.MultipleFollowerIDs[index1] != -1 && DataManager.Instance.Followers_Demons_IDs.Contains(this.StructureInfo.MultipleFollowerIDs[index1]))
      {
        this.OnEffects.SetActive(true);
        this.demons[index1].gameObject.SetActive(true);
        for (int index2 = 0; index2 < DataManager.Instance.Followers_Demons_IDs.Count; ++index2)
        {
          if (DataManager.Instance.Followers_Demons_IDs[index2] == this.StructureInfo.MultipleFollowerIDs[index1])
          {
            string skinName = Interaction_DemonSummoner.DemonSkins[DataManager.Instance.Followers_Demons_Types[index2]] + (FollowerInfo.GetInfoByID(DataManager.Instance.Followers_Demons_IDs[index2]).XPLevel > 1 ? "+" : "");
            this.demons[index1].Skeleton.SetSkin(skinName);
            break;
          }
        }
      }
    }
    this.DemonPreserved(false);
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    if (this.StructureInfo == null)
      return;
    for (int index = this.StructureInfo.MultipleFollowerIDs.Count - 1; index >= 0; --index)
    {
      if (DataManager.Instance.Followers_Transitioning_IDs.Contains(this.StructureInfo.MultipleFollowerIDs[index]))
      {
        Follower followerById = FollowerManager.FindFollowerByID(this.StructureInfo.MultipleFollowerIDs[index]);
        int demonType = DemonModel.GetDemonType(followerById.Brain._directInfoAccess);
        this.demons[index].gameObject.SetActive(true);
        string skinName = Interaction_DemonSummoner.DemonSkins[demonType] + (followerById.Brain.Stats.HasLevelledUp ? "+" : "");
        this.demons[index].Skeleton.SetSkin(skinName);
        this.demons[index].AnimationState.SetAnimation(0, "reveal", false);
        this.demons[index].AnimationState.AddAnimation(0, "idle", true, 0.0f);
        followerById.Brain.CompleteCurrentTask();
        followerById.Brain.HardSwapToTask((FollowerTask) new FollowerTask_IsDemon());
        followerById.Brain.CurrentTask.Arrive();
        NotificationCentre.Instance.PlayFollowerNotification(NotificationCentre.NotificationType.DemonConverted, followerById.Brain.Info, NotificationFollower.Animation.Normal);
        DataManager.Instance.Followers_Demons_IDs.Add(followerById.Brain.Info.ID);
        DataManager.Instance.Followers_Demons_Types.Add(demonType);
        DataManager.Instance.Followers_Transitioning_IDs.Remove(followerById.Brain.Info.ID);
      }
    }
  }

  private void OnBrainRemoved()
  {
    DataManager.Instance.Followers_Demons_IDs.Clear();
    DataManager.Instance.Followers_Demons_Types.Clear();
    this.DemonPreserved(true);
  }

  private void DemonPreserved(bool structureRemoved)
  {
    for (int index = this.StructureInfo.MultipleFollowerIDs.Count - 1; index >= 0; --index)
    {
      if (this.StructureInfo.MultipleFollowerIDs[index] != -1 && !DataManager.Instance.Followers_Demons_IDs.Contains(this.StructureInfo.MultipleFollowerIDs[index]) && !DataManager.Instance.Followers_Transitioning_IDs.Contains(this.StructureInfo.MultipleFollowerIDs[index]))
      {
        FollowerInfo infoById = FollowerInfo.GetInfoByID(this.StructureInfo.MultipleFollowerIDs[index]);
        if (infoById != null)
        {
          FollowerBrain brain = FollowerBrain.GetOrCreateBrain(infoById);
          GameManager.GetInstance().StartCoroutine((IEnumerator) this.SetFollowerPosition(brain));
          NotificationCentre.Instance.PlayFollowerNotification(NotificationCentre.NotificationType.DemonPreserved, brain.Info, NotificationFollower.Animation.Normal);
          if (!structureRemoved)
          {
            brain.MakeExhausted();
            brain._directInfoAccess.MissionaryExhaustion += 3f;
          }
        }
        this.StructureInfo.MultipleFollowerIDs.RemoveAt(index);
      }
    }
  }

  private IEnumerator SetFollowerPosition(FollowerBrain brain)
  {
    Vector3 pos = this.transform.position;
    while ((double) Time.timeScale == 0.0)
      yield return (object) null;
    brain.HardSwapToTask((FollowerTask) new FollowerTask_Idle());
    brain.CurrentTask.Arrive();
    while ((UnityEngine.Object) FollowerManager.FindFollowerByID(brain.Info.ID) == (UnityEngine.Object) null)
      yield return (object) null;
    Follower followerById = FollowerManager.FindFollowerByID(brain.Info.ID);
    if ((bool) (UnityEngine.Object) followerById)
    {
      brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
      followerById.transform.position = pos;
      brain.CompleteCurrentTask();
    }
  }

  public override void OnEnableInteraction()
  {
    this.Structure = this.GetComponentInParent<Structure>();
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    this.Structure.OnBrainRemoved += new System.Action(this.OnBrainRemoved);
    if (this.Structure.Brain != null)
      this.OnBrainAssigned();
    base.OnEnableInteraction();
  }

  public override void GetLabel()
  {
    if (!this.AtDemonLimit())
      this.Label = ScriptLocalization.Interactions.SummonDemon;
    else
      this.Label = "";
  }

  public override void GetSecondaryLabel()
  {
    if (this.StructureInfo.MultipleFollowerIDs.Count > 0)
      this.SecondaryLabel = ScriptLocalization.UI_Settings_Controls.Interact;
    else
      this.SecondaryLabel = "";
  }

  public override void OnBecomeCurrent()
  {
    this.Interactable = !this.AtDemonLimit();
    this.HasSecondaryInteraction = this.StructureInfo.MultipleFollowerIDs.Count > 0;
    base.OnBecomeCurrent();
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (this.AtDemonLimit())
      return;
    GameManager.GetInstance().OnConversationNew();
    Time.timeScale = 0.0f;
    HUD_Manager.Instance.Hide(false, 0);
    UIDemonSummonMenuController demonSummonMenuInstance = MonoSingleton<UIManager>.Instance.DemonSummonTemplate.Instantiate<UIDemonSummonMenuController>();
    demonSummonMenuInstance.Show(DemonModel.AvailableFollowersForDemonConversion(), (List<Follower>) null, false, UpgradeSystem.Type.Count, true, true, true);
    demonSummonMenuInstance.UpdateDemonCounts(this.StructureInfo.MultipleFollowerIDs);
    UIDemonSummonMenuController summonMenuController1 = demonSummonMenuInstance;
    summonMenuController1.OnFollowerSelected = summonMenuController1.OnFollowerSelected + new System.Action<FollowerInfo>(this.OnFollowerChosenForConversion);
    UIDemonSummonMenuController summonMenuController2 = demonSummonMenuInstance;
    summonMenuController2.OnHidden = summonMenuController2.OnHidden + (System.Action) (() =>
    {
      demonSummonMenuInstance = (UIDemonSummonMenuController) null;
      Time.timeScale = 1f;
      HUD_Manager.Instance.Show();
      GameManager.GetInstance().OnConversationEnd();
    });
  }

  public override void OnSecondaryInteract(StateMachine state)
  {
    base.OnSecondaryInteract(state);
    GameManager.GetInstance().OnConversationNew();
    UIDemonMenuController demonMenuInstance = MonoSingleton<UIManager>.Instance.DemonMenuTemplate.Instantiate<UIDemonMenuController>();
    demonMenuInstance.Show(this.StructureInfo.MultipleFollowerIDs);
    UIDemonMenuController demonMenuController = demonMenuInstance;
    demonMenuController.OnHidden = demonMenuController.OnHidden + (System.Action) (() =>
    {
      demonMenuInstance = (UIDemonMenuController) null;
      Time.timeScale = 1f;
      HUD_Manager.Instance.Show();
      GameManager.GetInstance().OnConversationEnd();
    });
  }

  private bool AtDemonLimit()
  {
    return this.StructureInfo.MultipleFollowerIDs.Count >= this.structureBrain.DemonSlots;
  }

  private void OnFollowerChosenForConversion(FollowerInfo followerInfo)
  {
    this.StructureInfo.MultipleFollowerIDs.Add(followerInfo.ID);
    FollowerBrain brain = FollowerBrain.GetOrCreateBrain(followerInfo);
    Follower followerById = FollowerManager.FindFollowerByID(followerInfo.ID);
    FollowerTask_ManualControl nextTask = new FollowerTask_ManualControl();
    brain.HardSwapToTask((FollowerTask) nextTask);
    followerById.transform.position = this.transform.position;
    DataManager.Instance.Followers_Transitioning_IDs.Add(followerInfo.ID);
    this.StartCoroutine((IEnumerator) this.SentFollowerIE(followerById));
  }

  private IEnumerator SentFollowerIE(Follower follower)
  {
    follower.HideAllFollowerIcons();
    int demonType = DemonModel.GetDemonType(follower.Brain._directInfoAccess);
    yield return (object) new WaitForSeconds(0.5f);
    follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    this.OnEffects.SetActive(true);
    double num = (double) follower.SetBodyAnimation("summon-demon", false);
    AudioManager.Instance.PlayOneShot("event:/followers/follower_to_demon_sequence", follower.transform.position);
    yield return (object) new WaitForSeconds(3f);
    int index1 = 0;
    for (int index2 = 0; index2 < this.StructureInfo.MultipleFollowerIDs.Count; ++index2)
    {
      if (this.StructureInfo.MultipleFollowerIDs[index2] == follower.Brain.Info.ID)
      {
        index1 = index2;
        break;
      }
    }
    this.demons[index1].gameObject.SetActive(true);
    string skinName = Interaction_DemonSummoner.DemonSkins[demonType] + (follower.Brain.Info.XPLevel > 1 ? "+" : "");
    this.demons[index1].Skeleton.SetSkin(skinName);
    this.demons[index1].AnimationState.SetAnimation(0, "reveal", false);
    this.demons[index1].AnimationState.AddAnimation(0, "idle", true, 0.0f);
    follower.Brain.CompleteCurrentTask();
    follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_IsDemon());
    follower.Brain.CurrentTask.Arrive();
    NotificationCentre.Instance.PlayFollowerNotification(NotificationCentre.NotificationType.DemonConverted, follower.Brain.Info, NotificationFollower.Animation.Normal);
    DataManager.Instance.Followers_Demons_IDs.Add(follower.Brain.Info.ID);
    DataManager.Instance.Followers_Demons_Types.Add(demonType);
    DataManager.Instance.Followers_Transitioning_IDs.Remove(follower.Brain.Info.ID);
    yield return (object) new WaitForSeconds(0.8f);
  }
}
