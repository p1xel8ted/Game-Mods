// Decompiled with JetBrains decompiler
// Type: Interaction_OfferingShrine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_OfferingShrine : Interaction
{
  public Structure Structure;
  public Structures_OfferingShrine _StructureInfo;
  public static List<Interaction_OfferingShrine> Shrines = new List<Interaction_OfferingShrine>();
  public AnimationCurve bounceCurve;
  public GameObject EmptyObject;
  public GameObject FullObject;
  public PauseInventoryItem Item;
  public Vector3 IconPosition;
  [SerializeField]
  public Transform _itemTransform;
  public string sCollectOffering;

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public Structures_OfferingShrine StructureBrain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_OfferingShrine;
      return this._StructureInfo;
    }
    set => this._StructureInfo = value;
  }

  public override void OnEnableInteraction()
  {
    this.ActivateDistance = 2f;
    base.OnEnableInteraction();
    Interaction_OfferingShrine.Shrines.Add(this);
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    this.ContinuouslyHold = true;
    this.IconPosition = this.Item.transform.position;
    if (this.Structure.Brain == null)
      return;
    this.OnBrainAssigned();
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    Interaction_OfferingShrine.Shrines.Remove(this);
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    if (this.StructureBrain == null)
      return;
    this.StructureBrain.OnCompleteOfferingShrine -= new System.Action<Vector3>(this.OnCompleteRefining);
  }

  public void OnCompleteRefining(Vector3 FollowerPosition)
  {
    AudioManager.Instance.PlayOneShot("event:/followers/pop_in", FollowerPosition);
    ResourceCustomTarget.Create(this.Item.gameObject, FollowerPosition, (InventoryItem.ITEM_TYPE) this.StructureInfo.Inventory[0].type, new System.Action(this.ShowItem));
  }

  public void OnBrainAssigned()
  {
    this.StructureBrain.OnCompleteOfferingShrine += new System.Action<Vector3>(this.OnCompleteRefining);
    this.ShowItem();
  }

  public void ShowItem()
  {
    if (this.StructureInfo.Inventory.Count <= 0)
    {
      this.EmptyObject.SetActive(true);
      this.FullObject.SetActive(false);
    }
    else
    {
      this.EmptyObject.SetActive(false);
      this.FullObject.SetActive(true);
      this.Item.Init((InventoryItem.ITEM_TYPE) this.StructureInfo.Inventory[0].type, this.StructureInfo.Inventory[0].quantity);
    }
  }

  public override void Update()
  {
    base.Update();
    this.Item.transform.parent.transform.localPosition = new Vector3(0.0f, 0.25f * this.bounceCurve.Evaluate((float) ((double) Time.time * 0.5 % 1.0)));
    this._itemTransform.localScale = this.transform.localScale;
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (this.StructureInfo.Inventory.Count <= 0)
      return;
    InventoryItem.ITEM_TYPE type = (InventoryItem.ITEM_TYPE) this.StructureInfo.Inventory[0].type;
    int quantity = this.StructureInfo.Inventory[0].quantity;
    for (int index = 0; index < quantity; ++index)
      InventoryItem.Spawn(type, 1, this.Item.transform.position, 0.0f).SetInitialSpeedAndDiraction(4f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
    this.StructureInfo.Inventory.Clear();
    this.StructureInfo.LastPrayTime = TimeManager.TotalElapsedGameTime;
    this.ShowItem();
    AudioManager.Instance.PlayOneShot("event:/Stings/generic_positive", this.transform.position);
    MMVibrate.Haptic(MMVibrate.HapticTypes.LightImpact, this.playerFarming);
  }

  public void Start() => this.UpdateLocalisation();

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sCollectOffering = ScriptLocalization.Interactions.Collect;
  }

  public override void GetLabel()
  {
    if (this.StructureInfo.Inventory.Count > 0)
      this.Label = this.sCollectOffering;
    else
      this.Label = "";
  }
}
