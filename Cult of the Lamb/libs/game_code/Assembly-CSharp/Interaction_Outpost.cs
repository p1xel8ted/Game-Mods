// Decompiled with JetBrains decompiler
// Type: Interaction_Outpost
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using UnityEngine;

#nullable disable
public class Interaction_Outpost : Interaction
{
  public Structure Structure;
  public GameObject ReceiveSoulPosition;
  public SpriteRenderer ProgressBar;
  public string assignString;
  public string collectString;
  public bool _hasFollowers;
  public bool _hasSouls;
  public GameObject Player;
  public bool CollectingSouls;
  public float Delay;
  public float DistanceToTriggerDeposits = 5f;

  public Structures_Shrine StructureBrain => this.Structure.Brain as Structures_Shrine;

  public void Start()
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
    Structures_Shrine structureBrain = this.StructureBrain;
    structureBrain.OnSoulsGained = structureBrain.OnSoulsGained - new System.Action<int>(this.OnSoulsGained);
  }

  public void OnStructuresPlaced()
  {
    Structures_Shrine structureBrain = this.StructureBrain;
    structureBrain.OnSoulsGained = structureBrain.OnSoulsGained + new System.Action<int>(this.OnSoulsGained);
    this.UpdateBar();
  }

  public void OnSoulsGained(int count) => this.UpdateBar();

  public void UpdateBar()
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
    {
      if (LocalizeIntegration.IsArabic())
        this.Label = $"{this.collectString} <sprite name=\"icon_spirits\"> x{LocalizeIntegration.ReverseText(this.StructureBrain.SoulMax.ToString())}/{LocalizeIntegration.ReverseText(this.StructureBrain.SoulCount.ToString())}";
      else
        this.Label = $"{this.collectString} {"<sprite name=\"icon_spirits\">"} x{this.StructureBrain.SoulCount}/{this.StructureBrain.SoulMax}";
    }
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

  public bool HasFollowingFollowers()
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

  public override void Update()
  {
    base.Update();
    if ((UnityEngine.Object) (this.Player = GameObject.FindWithTag("Player")) == (UnityEngine.Object) null)
      return;
    this.GetLabel();
    if (this.CollectingSouls && (this.StructureBrain.SoulCount <= 0 || InputManager.Gameplay.GetInteractButtonUp() || (double) Vector3.Distance(this.transform.position, this.Player.transform.position) > (double) this.DistanceToTriggerDeposits))
      this.CollectingSouls = false;
    if ((double) (this.Delay -= Time.deltaTime) >= 0.0 || !this.CollectingSouls)
      return;
    if ((GameManager.HasUnlockAvailable() ? 1 : (DataManager.Instance.DeathCatBeaten ? 1 : 0)) != 0)
      SoulCustomTarget.Create(this.playerFarming.gameObject, this.ReceiveSoulPosition.transform.position, Color.white, new System.Action(this.GivePlayerSoul));
    else
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, this.transform.position + Vector3.back, 0.0f).SetInitialSpeedAndDiraction(8f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
    --this.StructureBrain.SoulCount;
    this.Delay = 0.2f;
    this.UpdateBar();
  }

  public void GivePlayerSoul() => this.playerFarming.GetSoul(1);
}
