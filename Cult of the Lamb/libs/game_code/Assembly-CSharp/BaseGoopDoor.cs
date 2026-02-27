// Decompiled with JetBrains decompiler
// Type: BaseGoopDoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MMTools;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class BaseGoopDoor : Interaction
{
  public static List<BaseGoopDoor> doors = new List<BaseGoopDoor>();
  public static List<PlayerFarming> doorUpContributors = new List<PlayerFarming>();
  public static BaseGoopDoor MainDoor;
  public static BaseGoopDoor DLCDoor;
  public static BaseGoopDoor WoolhavenDoor;
  public static bool IsOpen = true;
  [CompilerGenerated]
  public static bool \u003CLockDoor\u003Ek__BackingField = false;
  public bool IsMainDoor = true;
  public bool IsDLCDoor;
  public bool IsWoolhavenDoor;
  public GameObject BlockingCollider;
  public Animator Animator;
  public string doorLockedLabel = "";
  public bool unlocked = true;
  public bool revealed = true;
  public string sIndoctrinateFollower;
  public SimpleSetCamera SimpleSetCamera;

  public static bool LockDoor
  {
    get => BaseGoopDoor.\u003CLockDoor\u003Ek__BackingField;
    set => BaseGoopDoor.\u003CLockDoor\u003Ek__BackingField = value;
  }

  public int woolhavenFollowerCountRequirement
  {
    get
    {
      if (DataManager.Instance.BeatenYngya)
        return 0;
      if (DataManager.Instance.DLCDungeonNodesCompleted.Count > 16 /*0x10*/)
        return 12;
      if (DataManager.Instance.DLCDungeonNodesCompleted.Count > 13)
        return 11;
      if (DataManager.Instance.DLCDungeonNodesCompleted.Count > 11)
        return 10;
      if (DataManager.Instance.DLCDungeonNodesCompleted.Count > 9)
        return 9;
      if (DataManager.Instance.DLCDungeonNodesCompleted.Count > 6)
        return 7;
      return DataManager.Instance.DLCDungeonNodesCompleted.Count > 4 ? 5 : 0;
    }
  }

  public void Awake()
  {
    BaseGoopDoor.doors.Add(this);
    if (this.IsMainDoor)
      BaseGoopDoor.MainDoor = this;
    else if (this.IsDLCDoor)
    {
      BaseGoopDoor.DLCDoor = this;
    }
    else
    {
      if (!this.IsWoolhavenDoor)
        return;
      BaseGoopDoor.WoolhavenDoor = this;
    }
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    BaseGoopDoor.doors.Remove(this);
    BaseGoopDoor.doorUpContributors.Clear();
  }

  public void Start()
  {
    this.Interactable = false;
    this.UpdateLocalisation();
    if (!this.IsWoolhavenDoor)
    {
      this.OnNewRecruit();
      if (DataManager.Instance.BaseGoopDoorLocked)
      {
        this.doorLockedLabel = DataManager.Instance.BaseGoopDoorLoc;
        BaseGoopDoor.BlockGoopDoor();
      }
    }
    if (this.IsWoolhavenDoor)
      return;
    if (this.IsDLCDoor)
    {
      this.unlocked = true;
      this.revealed = DataManager.Instance.OnboardedDLCEntrance;
      this.BlockingCollider.SetActive(!this.revealed);
    }
    if (!this.unlocked && BaseGoopDoor.IsOpen && this.revealed && !this.IsWoolhavenDoor)
    {
      this.SetDoorUp();
    }
    else
    {
      if (DataManager.Instance.BaseGoopDoorLocked)
        return;
      BaseGoopDoor.UnblockGoopDoor();
    }
  }

  public override void OnEnable()
  {
    base.OnEnable();
    if (!this.IsWoolhavenDoor)
      return;
    this.CheckWoolhavenDoor();
  }

  public void CheckWoolhavenDoor()
  {
    if (this.CanWoolhavenDoorOpen())
      BaseGoopDoor.DoorDown();
    else
      this.SetDoorUp();
  }

  public bool CanWoolhavenDoorOpen()
  {
    return !PlayerFarming.IsAnyPlayerInInteractionWithRanchable() && (DataManager.Instance.OnboardedFindLostSouls || !DataManager.Instance.OnboardedAddFuelToFurnace) && Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.YNGYA_GHOST) <= 0 && !ObjectiveManager.HasCustomObjectiveOfType(Objectives.CustomQuestTypes.LightFurnace) && !ObjectiveManager.HasBuildStructureObjective(StructureBrain.TYPES.FURNACE_1) && Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.SPECIAL_WOOL_RANCHER) <= 0 && DataManager.Instance.Followers.Count >= this.woolhavenFollowerCountRequirement;
  }

  public static void BlockGoopDoor(string loc = "")
  {
    if (DataManager.Instance.OnboardingFinished)
      return;
    BaseGoopDoor.DoorUp();
    DataManager.Instance.BaseGoopDoorLoc = loc;
    DataManager.Instance.BaseGoopDoorLocked = true;
    Debug.Log((object) "BlockGoopDoor()".Colour(Color.red));
    foreach (BaseGoopDoor door in BaseGoopDoor.doors)
    {
      BaseGoopDoor.LockDoor = true;
      if (!string.IsNullOrEmpty(loc))
        door.doorLockedLabel = loc;
    }
  }

  public static void UnblockGoopDoor()
  {
    Debug.Log((object) "BlockGoopDoor()".Colour(Color.green));
    foreach (BaseGoopDoor door in BaseGoopDoor.doors)
      BaseGoopDoor.LockDoor = false;
    BaseGoopDoor.DoorDown();
    DataManager.Instance.BaseGoopDoorLocked = false;
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    FollowerRecruit.OnNewRecruit += new System.Action(this.OnNewRecruit);
    if (!(bool) (UnityEngine.Object) BiomeBaseManager.Instance)
      return;
    BiomeBaseManager.Instance.OnNewRecruitRevealed += new System.Action(this.OnNewRecruit);
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    FollowerRecruit.OnNewRecruit -= new System.Action(this.OnNewRecruit);
    if (!(bool) (UnityEngine.Object) BiomeBaseManager.Instance)
      return;
    BiomeBaseManager.Instance.OnNewRecruitRevealed -= new System.Action(this.OnNewRecruit);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    if (!this.IsWoolhavenDoor || BaseGoopDoor.IsOpen)
      return;
    BaseGoopDoor.IsOpen = true;
  }

  public void OnNewRecruit()
  {
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sIndoctrinateFollower = ScriptLocalization.Interactions.IndoctrinateBeforeLeaving;
  }

  public override void GetLabel()
  {
    this.Interactable = false;
    if (this.IsWoolhavenDoor)
    {
      if (DataManager.Instance.Followers.Count < this.woolhavenFollowerCountRequirement)
      {
        if (LocalizeIntegration.IsArabic())
        {
          string str;
          if (!this.BlockingCollider.activeSelf)
            str = "";
          else
            str = $"{ScriptLocalization.Interactions.Requires} {this.woolhavenFollowerCountRequirement.ToString()} / <color=red> {DataManager.Instance.Followers.Count.ToString()}</color> {FontImageNames.GetIconByType(InventoryItem.ITEM_TYPE.FOLLOWERS)}";
          this.Label = str;
        }
        else
        {
          string str;
          if (!this.BlockingCollider.activeSelf)
            str = "";
          else
            str = $"{ScriptLocalization.Interactions.Requires}<color=red> {DataManager.Instance.Followers.Count.ToString()}</color> / {this.woolhavenFollowerCountRequirement.ToString()} {FontImageNames.GetIconByType(InventoryItem.ITEM_TYPE.FOLLOWERS)}";
          this.Label = str;
        }
      }
      else
      {
        string str = LocalizationManager.GetTranslation("Interactions/RequiresDepositYngyaGhost");
        if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.YNGYA_GHOST) <= 0)
          str = Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.SPECIAL_WOOL_RANCHER) <= 0 ? (!DataManager.Instance.BuiltFurnace ? string.Format(ScriptLocalization.Objectives.BuildStructure, (object) StructuresData.LocalizedName(StructureBrain.TYPES.FURNACE_1)) : LocalizationManager.GetTranslation("Objectives/Custom/LightFurnace")) : LocalizationManager.GetTranslation("Objectives/Custom/BuryLostGhost");
        this.Label = this.BlockingCollider.activeSelf ? str : "";
      }
    }
    else if (BaseGoopDoor.LockDoor)
    {
      if (DataManager.Instance.InTutorial && ObjectiveManager.AllObjectiveGroupIDs().Count == 1)
      {
        foreach (ObjectivesData objectivesData in ObjectiveManager.GetAllObjectivesOfGroup(ObjectiveManager.AllObjectiveGroupIDs()[0]))
        {
          if (DataManager.Instance.Objectives.Contains(objectivesData))
          {
            this.Label = objectivesData.Text;
            break;
          }
        }
      }
      else
        this.Label = string.IsNullOrEmpty(this.doorLockedLabel) ? "" : LocalizationManager.GetTranslation(this.doorLockedLabel);
    }
    else if (!BaseGoopDoor.IsOpen && BaseGoopDoor.doorUpContributors.Count > 0)
      this.Label = string.IsNullOrEmpty(this.doorLockedLabel) ? "" : LocalizationManager.GetTranslation(this.doorLockedLabel);
    else
      this.Label = !BaseGoopDoor.IsOpen ? this.sIndoctrinateFollower : "";
  }

  public override void IndicateHighlighted(PlayerFarming playerFarming)
  {
  }

  public override void EndIndicateHighlighted(PlayerFarming playerFarming)
  {
  }

  public static void DoorUp(string label = "", PlayerFarming contributor = null)
  {
    if ((UnityEngine.Object) contributor != (UnityEngine.Object) null)
      BaseGoopDoor.doorUpContributors.Add(contributor);
    foreach (BaseGoopDoor door in BaseGoopDoor.doors)
    {
      if (door.revealed)
      {
        if (BaseGoopDoor.LockDoor)
          return;
        if (BaseGoopDoor.IsOpen)
          door.SetDoorUp(label);
      }
    }
    BaseGoopDoor.IsOpen = false;
  }

  public void SetDoorUp(string label = "")
  {
    this.BlockingCollider.SetActive(true);
    this.doorLockedLabel = label;
    this.Animator.Play("GoopWallIntro");
    if (this.IsWoolhavenDoor)
      BaseGoopDoor.IsOpen = false;
    else if (this.IsDLCDoor)
    {
      foreach (PlayerFarming player in PlayerFarming.players)
      {
        if ((double) player.transform.position.x > (double) this.transform.position.x && (double) player.transform.position.y > (double) this.transform.position.y - 2.0)
          player.transform.position = this.BlockingCollider.transform.position - Vector3.right * 2f;
      }
    }
    else
    {
      if (PlayerFarming.Location != FollowerLocation.Base)
        return;
      foreach (PlayerFarming player in PlayerFarming.players)
      {
        if ((double) player.transform.position.y > (double) this.transform.position.y)
          player.transform.position = this.BlockingCollider.transform.position - Vector3.up * 2f;
      }
    }
  }

  public static void DoorDown(PlayerFarming contributor = null)
  {
    if ((UnityEngine.Object) contributor != (UnityEngine.Object) null)
      BaseGoopDoor.doorUpContributors.Remove(contributor);
    foreach (BaseGoopDoor door in BaseGoopDoor.doors)
    {
      if (door.unlocked && door.revealed)
      {
        if (BaseGoopDoor.LockDoor || BaseGoopDoor.doorUpContributors.Count > 0)
          return;
        door.Animator.Play("GoopWallDown");
        door.BlockingCollider.SetActive(false);
        door.doorLockedLabel = "";
      }
    }
    BaseGoopDoor.IsOpen = true;
  }

  public void OnTriggerEnter2D(Collider2D collision)
  {
    if ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null || !this.IsPlayerGameObject(collision.gameObject) || BaseGoopDoor.IsOpen || !this.revealed)
      return;
    this.Animator.Play("GoopWallColliding");
    AudioManager.Instance.PlayOneShot("event:/Stings/generic_negative", this.gameObject);
  }

  public bool IsPlayerGameObject(GameObject collisionObject)
  {
    bool flag = false;
    foreach (Component player in PlayerFarming.players)
    {
      if ((UnityEngine.Object) player.gameObject == (UnityEngine.Object) collisionObject)
        flag = true;
    }
    return flag;
  }

  public void PlayOpenDoorSequence(System.Action Callback)
  {
    this.StartCoroutine(this.PlayOpenDoorSequenceRoutine(Callback));
  }

  public IEnumerator PlayOpenDoorSequenceRoutine(System.Action Callback)
  {
    BaseGoopDoor baseGoopDoor = this;
    while (MMConversation.CURRENT_CONVERSATION != null)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNew();
    baseGoopDoor.SimpleSetCamera.Play();
    yield return (object) new WaitForSeconds(2.5f);
    BaseGoopDoor.UnblockGoopDoor();
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.5f, 0.3f);
    AudioManager.Instance.PlayOneShot("event:/door/door_done", baseGoopDoor.gameObject);
    yield return (object) new WaitForSeconds(2f);
    baseGoopDoor.SimpleSetCamera.Reset();
    System.Action action = Callback;
    if (action != null)
      action();
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().OnConversationEnd();
  }

  public void SetReveal(bool revealed) => this.revealed = revealed;
}
