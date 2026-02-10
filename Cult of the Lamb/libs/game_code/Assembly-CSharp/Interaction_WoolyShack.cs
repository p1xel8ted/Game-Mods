// Decompiled with JetBrains decompiler
// Type: Interaction_WoolyShack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using UnityEngine;

#nullable disable
public class Interaction_WoolyShack : Interaction
{
  public static Interaction_WoolyShack Instance;
  public Structures_WoolyShack _StructureInfo;
  public Structure Structure;
  [SerializeField]
  public GameObject trunk;
  public string sLabel;
  public const int CRAFTING_COST = 5;

  public Structures_WoolyShack Brain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_WoolyShack;
      return this._StructureInfo;
    }
    set => this._StructureInfo = value;
  }

  public GameObject Trunk => this.trunk;

  public void Start()
  {
    Interaction_WoolyShack.Instance = this;
    this.UpdateLocalisation();
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sLabel = ScriptLocalization.Interactions.Craft;
  }

  public override void GetLabel()
  {
    base.GetLabel();
    int num = 0;
    foreach (FollowerInfo follower in DataManager.Instance.Followers)
    {
      FollowerBrain brain = FollowerBrain.GetOrCreateBrain(follower);
      if (brain != null && brain.CanFreezeExcludingScarf())
        ++num;
    }
    if (this.Brain.Data.Inventory.Count >= num)
    {
      this.Interactable = false;
      this.Label = ScriptLocalization.Interactions.Full;
    }
    else
    {
      this.Interactable = true;
      this.Label = $"{this.sLabel}: {InventoryItem.CapacityString(InventoryItem.ITEM_TYPE.WOOL, 5)}";
    }
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.WOOL) >= 5)
    {
      Inventory.ChangeItemQuantity(InventoryItem.ITEM_TYPE.WOOL, -5);
      this.Craft();
      this.HasChanged = true;
    }
    else
      this.playerFarming.indicator.PlayShake();
  }

  public void Craft()
  {
    this.trunk.transform.localScale = Vector3.one * 0.5f;
    this.trunk.transform.DOKill();
    this.trunk.transform.DOPunchScale(Vector3.one * 0.1f, 0.5f);
    this.Brain.Data.Inventory.Add(new InventoryItem(InventoryItem.ITEM_TYPE.NONE));
    if (SeasonsManager.CurrentSeason != SeasonsManager.Season.Winter)
      return;
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.CanFreeze() && allBrain.CurrentTaskType != FollowerTaskType.CollectScarf && !FollowerManager.FollowerLocked(allBrain.Info.ID) && this.Brain.ScarfAvailable())
        allBrain.HardSwapToTask((FollowerTask) new FollowerTask_CollectScarf());
    }
  }
}
