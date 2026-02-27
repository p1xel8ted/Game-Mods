// Decompiled with JetBrains decompiler
// Type: Interaction_FeastTable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_FeastTable : Interaction
{
  public static List<Interaction_FeastTable> FeastTables = new List<Interaction_FeastTable>();
  public Structure Structure;
  private Structures_FeastTable _StructureInfo;
  public AudioClip FeastSong;
  public GameObject Center;
  public GameObject PlayerPosition;
  public GameObject Container;
  public GameObject[] Seats;
  private bool IsEating;
  private float createdMealTimer;
  private float feastingMembersCheckTimer;
  private List<Follower> feasters = new List<Follower>();
  public bool IsFeastActive;
  private GameObject Player;
  private bool GiveThought;

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public Structures_FeastTable brain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_FeastTable;
      return this._StructureInfo;
    }
    set => this._StructureInfo = value;
  }

  private void Start() => this.UpdateLocalisation();

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.ContinuouslyHold = true;
    Interaction_FeastTable.FeastTables.Add(this);
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    if (this.Structure.Brain == null)
    {
      this.Structure.Brain = StructureBrain.GetOrCreateBrain(StructuresData.GetInfoByType(StructureBrain.TYPES.FEAST_TABLE, 0));
      this.GetFeasters();
    }
    StructureManager.OnStructureAdded += new StructureManager.StructureChanged(this.OnStructureAdded);
  }

  private void OnBrainAssigned()
  {
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    if (!this.StructureInfo.IsGatheringActive)
      this.StructureInfo.GivenHealth = false;
    this.GetFeasters();
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    StructureManager.OnStructureAdded -= new StructureManager.StructureChanged(this.OnStructureAdded);
  }

  private void OnStructureAdded(StructuresData structure)
  {
    if (structure.ID != this.StructureInfo.ID)
      return;
    StructureManager.OnStructureAdded -= new StructureManager.StructureChanged(this.OnStructureAdded);
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (!this.StructureInfo.IsGatheringActive)
      return;
    if (!this.IsEating)
    {
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject, 6f);
      GameManager.GetInstance().AddToCamera(this.gameObject);
      this.StartCoroutine((IEnumerator) this.EatIE());
    }
    else
    {
      if (!this.IsEating || PlayerFarming.Instance.GoToAndStopping)
        return;
      this.CancelEat();
      PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.Idle;
    }
  }

  private IEnumerator EatIE()
  {
    PlayerFarming.Instance.GoToAndStop(this.PlayerPosition);
    while (PlayerFarming.Instance.GoToAndStopping)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    this.IsEating = true;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.Dancing;
    yield return (object) new WaitForEndOfFrame();
    PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "eat", true);
  }

  private void CancelEat()
  {
    GameManager.GetInstance().OnConversationEnd();
    GameManager.GetInstance().RemoveFromCamera(this.gameObject);
    this.IsEating = false;
    if (!this.StructureInfo.GivenHealth)
      this.StartCoroutine((IEnumerator) this.CancelEatIE());
    this.StructureInfo.GivenHealth = true;
  }

  private IEnumerator CancelEatIE()
  {
    yield return (object) new WaitForSeconds(1f);
    PlayerFarming.Instance.health.BlueHearts += 4f;
  }

  private new void OnDestroy() => Interaction_FeastTable.FeastTables.Remove(this);

  public override void GetLabel()
  {
    string str = this.IsEating ? ScriptLocalization.Interactions.Cancel : ScriptLocalization.Interactions.JoinDance;
    this.Label = this.brain.Data.IsGatheringActive ? str : "";
    if (!this.StructureInfo.GivenHealth)
      return;
    this.Label = "";
    this.Interactable = false;
  }

  private new void Update()
  {
    if ((UnityEngine.Object) (this.Player = GameObject.FindWithTag("Player")) == (UnityEngine.Object) null || this.StructureInfo == null)
      return;
    if (this.IsEating && !PlayerFarming.Instance.GoToAndStopping && (!this.StructureInfo.IsGatheringActive || InputManager.General.GetAnyButton()))
      this.CancelEat();
    if (!this.IsFeastActive)
      return;
    if ((double) Time.time > (double) this.createdMealTimer && this.feasters.Count > 0)
    {
      Follower feaster = this.feasters[UnityEngine.Random.Range(0, this.feasters.Count)];
      if (feaster.Brain.CurrentTask != null && feaster.Brain.CurrentTask is FollowerTask_EatFeastTable)
      {
        this.createdMealTimer = Time.time + UnityEngine.Random.Range(0.25f, 0.75f);
        ResourceCustomTarget.Create(feaster.gameObject, this.Center.transform.position + Vector3.up * UnityEngine.Random.Range(-1f, 1f), this.GetRandomMeal(), (System.Action) null);
      }
    }
    else if (this.feasters.Count == 0)
      this.GetFeasters();
    this.GiveThought = true;
  }

  private void GetFeasters()
  {
    this.feasters.Clear();
    foreach (Follower follower in FollowerManager.FollowersAtLocation(FollowerLocation.Church))
      this.feasters.Add(follower);
    foreach (Follower follower in FollowerManager.FollowersAtLocation(FollowerLocation.Base))
    {
      if (!this.feasters.Contains(follower) && !FollowerManager.FollowerLocked(follower.Brain.Info.ID))
        this.feasters.Add(follower);
    }
  }

  private InventoryItem.ITEM_TYPE GetRandomMeal()
  {
    InventoryItem.ITEM_TYPE[] allMeals = CookingData.GetAllMeals();
    return allMeals[UnityEngine.Random.Range(0, allMeals.Length)];
  }

  public Vector3 GetEatPosition(Follower follower)
  {
    this.GetFeasters();
    int index = this.feasters.IndexOf(follower);
    if (index != -1 && index < this.Seats.Length - 1)
      return this.Seats[index].transform.position;
    Vector3 eatPosition = this.transform.position + Vector3.up * 3f;
    eatPosition.x += UnityEngine.Random.Range(0, 2) == 0 ? UnityEngine.Random.Range(-1f, -1.75f) : UnityEngine.Random.Range(1f, 1.75f);
    eatPosition.y += UnityEngine.Random.Range(-3f, 2.5f);
    return eatPosition;
  }
}
