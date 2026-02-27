// Decompiled with JetBrains decompiler
// Type: Interaction_FireDancePit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_FireDancePit : Interaction
{
  public static Interaction_FireDancePit Instance;
  public Structure Structure;
  private Structures_DancingFirePit _StructureInfo;
  public AudioClip DanceSong;
  public List<InventoryItem.ITEM_TYPE> AllowedFuel;
  public GameObject BonfireOn;
  public GameObject BonfireOff;
  public GameObject Center;
  public GameObject PlayerPosition;
  public Transform[] Positions;
  private bool isLit = true;
  private bool isDancing;
  [SerializeField]
  private GameObject _resourceTarget;
  private string sAddFuel;

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public Structures_DancingFirePit Brain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_DancingFirePit;
      return this._StructureInfo;
    }
    set => this._StructureInfo = value;
  }

  private void Awake() => Interaction_FireDancePit.Instance = this;

  private void Start()
  {
    this.UpdateLocalisation();
    this.StructureInfo.MaxFuel = InventoryItem.FuelWeight(InventoryItem.ITEM_TYPE.LOG) * 50;
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.ContinuouslyHold = true;
    StructureManager.OnStructureAdded += new StructureManager.StructureChanged(this.OnStructureAdded);
    StructureManager.OnStructuresPlaced += new StructureManager.StructuresPlaced(this.SetImages);
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    if (this.Structure.Brain == null)
      this.Structure.Brain = StructureBrain.GetOrCreateBrain(StructuresData.GetInfoByType(StructureBrain.TYPES.DANCING_FIREPIT, 0));
    if (this.Structure.Brain == null)
      return;
    this.SetImages();
  }

  private void OnBrainAssigned()
  {
    this.SetImages();
    if (this.StructureInfo.IsGatheringActive)
      return;
    this.StructureInfo.GivenHealth = false;
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    StructureManager.OnStructureAdded -= new StructureManager.StructureChanged(this.OnStructureAdded);
    StructureManager.OnStructuresPlaced -= new StructureManager.StructuresPlaced(this.SetImages);
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
  }

  private void OnStructureAdded(StructuresData structure)
  {
    if (structure.ID != this.StructureInfo.ID)
      return;
    this.SetImages();
    StructureManager.OnStructureAdded -= new StructureManager.StructureChanged(this.OnStructureAdded);
  }

  private void SetImages()
  {
    if (this.StructureInfo.IsGatheringActive)
    {
      this.BonfireOn.SetActive(true);
      this.BonfireOff.SetActive(false);
    }
    else
    {
      this.BonfireOn.SetActive(false);
      this.BonfireOff.SetActive(true);
    }
  }

  public Vector3 GetDancePosition(int followerId)
  {
    int index1 = 0;
    for (int index2 = 0; index2 < FollowerBrain.AllBrains.Count; ++index2)
    {
      if (FollowerBrain.AllBrains[index2].Info.ID == followerId)
      {
        index1 = index2;
        break;
      }
    }
    return index1 < this.Positions.Length ? this.Positions[index1].position : this.transform.position + Vector3.down * 2f + (Vector3) UnityEngine.Random.insideUnitCircle * UnityEngine.Random.Range(2f, 4f);
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (!this.StructureInfo.IsGatheringActive)
    {
      state.CURRENT_STATE = StateMachine.State.InActive;
      state.facingAngle = Utils.GetAngle(state.transform.position, this.transform.position);
      CameraFollowTarget cameraFollowTarget = CameraFollowTarget.Instance;
      cameraFollowTarget.SetOffset(new Vector3(0.0f, 4.5f, 2f));
      cameraFollowTarget.AddTarget(this.gameObject, 1f);
      UIItemSelectorOverlayController itemSelector = MonoSingleton<UIManager>.Instance.ShowItemSelector(InventoryItem.AllBurnableFuel, new ItemSelector.Params()
      {
        Key = "addfuel",
        Context = ItemSelector.Context.Add,
        Offset = new Vector2(0.0f, 125f),
        HideOnSelection = false,
        ShowEmpty = true
      });
      itemSelector.OnItemChosen += (System.Action<InventoryItem.ITEM_TYPE>) (chosenItem =>
      {
        Debug.Log((object) $"Deposit {chosenItem} to fuel fire pit".Colour(Color.yellow));
        AudioManager.Instance.PlayOneShot("event:/followers/pop_in", this.transform.position);
        ResourceCustomTarget.Create(this._resourceTarget, PlayerFarming.Instance.transform.position, chosenItem, (System.Action) null);
        Inventory.ChangeItemQuantity((int) chosenItem, -1);
        this.StructureInfo.Fuel = Mathf.Clamp(this.StructureInfo.Fuel + InventoryItem.FuelWeight(chosenItem), 0, this.StructureInfo.MaxFuel);
        if (this.RequiresFuel())
          return;
        itemSelector.Hide();
        this.BonfireLit();
      });
      UIItemSelectorOverlayController overlayController = itemSelector;
      overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() =>
      {
        state.CURRENT_STATE = StateMachine.State.Idle;
        itemSelector = (UIItemSelectorOverlayController) null;
        this.Interactable = true;
        this.HasChanged = true;
        cameraFollowTarget.SetOffset(Vector3.zero);
        cameraFollowTarget.RemoveTarget(this.gameObject);
      });
    }
    else if (!this.isDancing)
    {
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject, 6f);
      this.StartCoroutine((IEnumerator) this.DanceIE());
    }
    else
    {
      if (!this.isDancing)
        return;
      this.CancelDance();
      PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.Idle;
    }
  }

  private IEnumerator DanceIE()
  {
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForEndOfFrame();
    this.isDancing = true;
    PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "dance", true);
  }

  private void CancelDance()
  {
    GameManager.GetInstance().OnConversationEnd();
    this.isDancing = false;
    if (!this.StructureInfo.GivenHealth)
      this.StartCoroutine((IEnumerator) this.CancelDanceIE());
    this.StructureInfo.GivenHealth = true;
  }

  private IEnumerator CancelDanceIE()
  {
    yield return (object) new WaitForSeconds(1f);
    PlayerFarming.Instance.health.BlueHearts += 4f;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sAddFuel = "<sprite name=\"icon_wood\">";
  }

  public override void GetLabel()
  {
    if (this.RequiresFuel())
      this.Label = ScriptLocalization.Interactions.AddFuel;
    else if (this.isDancing)
      this.Label = ScriptLocalization.Interactions.Cancel;
    else
      this.Label = ScriptLocalization.Interactions.JoinDance;
    if (!this.StructureInfo.GivenHealth)
      return;
    this.Label = "";
    this.Interactable = false;
  }

  protected override void Update()
  {
    base.Update();
    if (this.isLit && !this.StructureInfo.IsGatheringActive)
    {
      this.SetImages();
      this.isLit = false;
      this.StructureInfo.GivenHealth = false;
      this.Interactable = true;
    }
    if (!this.isDancing || this.StructureInfo.IsGatheringActive && !InputManager.General.GetAnyButton())
      return;
    this.CancelDance();
  }

  private bool RequiresFuel() => this.StructureInfo.Fuel < this.StructureInfo.MaxFuel;

  private void BonfireLit()
  {
    this.StructureInfo.GatheringEndPhase = (int) (TimeManager.CurrentPhase + 2) % 5;
    this.SetImages();
    this.isLit = true;
    NotificationCentreScreen.Play(NotificationCentre.NotificationType.FirePitBegan);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.UseFirePit);
    foreach (Follower follower in FollowerManager.FollowersAtLocation(this.Brain.Data.Location))
    {
      if (!FollowerManager.FollowerLocked(follower.Brain.Info.ID) && follower.Brain.Info.CursedState != Thought.Dissenter && follower.Brain.Info.CursedState != Thought.Zombie)
      {
        follower.Brain.CompleteCurrentTask();
        follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_DanceFirePit(this.Brain.Data.ID));
      }
    }
  }
}
