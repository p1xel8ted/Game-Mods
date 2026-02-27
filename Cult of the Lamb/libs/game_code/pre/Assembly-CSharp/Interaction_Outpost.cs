// Decompiled with JetBrains decompiler
// Type: Interaction_Outpost
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using UnityEngine;

#nullable disable
public class Interaction_Outpost : Interaction
{
  public Structure Structure;
  public GameObject ReceiveSoulPosition;
  public SpriteRenderer ProgressBar;
  private string assignString;
  private string collectString;
  private bool _hasFollowers;
  private bool _hasSouls;
  private GameObject Player;
  private bool CollectingSouls;
  private float Delay;
  public float DistanceToTriggerDeposits = 5f;

  public Structures_Shrine StructureBrain => this.Structure.Brain as Structures_Shrine;

  private void Start()
  {
    this.UpdateLocalisation();
    this.Structure.Structure_Info.CanBeMoved = false;
    this.Structure.Structure_Info.CanBeRecycled = false;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.assignString = "Assign Follower";
    this.collectString = ScriptLocalization.Interactions.ReceiveDevotion;
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    StructureManager.OnStructuresPlaced += new StructureManager.StructuresPlaced(this.OnStructuresPlaced);
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    StructureManager.OnStructuresPlaced -= new StructureManager.StructuresPlaced(this.OnStructuresPlaced);
    if (this.StructureBrain == null)
      return;
    this.StructureBrain.OnSoulsGained -= new System.Action<int>(this.OnSoulsGained);
  }

  private void OnStructuresPlaced()
  {
    this.StructureBrain.OnSoulsGained += new System.Action<int>(this.OnSoulsGained);
    this.UpdateBar();
  }

  private void OnSoulsGained(int count) => this.UpdateBar();

  private void UpdateBar()
  {
    this.ProgressBar.transform.localScale = new Vector3(Mathf.Clamp((float) this.StructureBrain.SoulCount / (float) this.StructureBrain.SoulMax, 0.0f, 1f), this.ProgressBar.transform.localScale.y);
  }

  public override void GetLabel()
  {
    this._hasFollowers = this.HasFollowingFollowers();
    this._hasSouls = this.StructureBrain.SoulCount > 0;
    this.Interactable = this._hasFollowers || this._hasSouls;
    if (this._hasFollowers)
      this.Label = this.assignString;
    else if (this._hasSouls)
      this.Label = $"{this.collectString} {"<sprite name=\"icon_spirits\">"} x{this.StructureBrain.SoulCount}/{this.StructureBrain.SoulMax}";
    else
      this.Label = "";
  }

  public override void OnInteract(StateMachine state)
  {
    if (this._hasFollowers)
    {
      base.OnInteract(state);
      foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
      {
        if (allBrain.FollowingPlayer)
        {
          allBrain.SetNewHomeLocation(this.StructureBrain.Data.Location);
          allBrain.FollowingPlayer = false;
          allBrain.CompleteCurrentTask();
        }
      }
    }
    else
    {
      if (!this._hasSouls || this.CollectingSouls)
        return;
      base.OnInteract(state);
      this.CollectingSouls = true;
    }
  }

  private bool HasFollowingFollowers()
  {
    bool flag = false;
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.FollowingPlayer)
      {
        flag = true;
        break;
      }
    }
    return flag;
  }

  private new void Update()
  {
    if ((UnityEngine.Object) (this.Player = GameObject.FindWithTag("Player")) == (UnityEngine.Object) null)
      return;
    this.GetLabel();
    if (this.CollectingSouls && (this.StructureBrain.SoulCount <= 0 || InputManager.Gameplay.GetInteractButtonUp() || (double) Vector3.Distance(this.transform.position, this.Player.transform.position) > (double) this.DistanceToTriggerDeposits))
      this.CollectingSouls = false;
    if ((double) (this.Delay -= Time.deltaTime) >= 0.0 || !this.CollectingSouls)
      return;
    if (GameManager.HasUnlockAvailable())
      SoulCustomTarget.Create(PlayerFarming.Instance.gameObject, this.ReceiveSoulPosition.transform.position, Color.white, new System.Action(this.GivePlayerSoul));
    else
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, this.transform.position + Vector3.back, 0.0f).SetInitialSpeedAndDiraction(8f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
    --this.StructureBrain.SoulCount;
    this.Delay = 0.2f;
    this.UpdateBar();
  }

  private void GivePlayerSoul() => PlayerFarming.Instance.GetSoul(1);
}
