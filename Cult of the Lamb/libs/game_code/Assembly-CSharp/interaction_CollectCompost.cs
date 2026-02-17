// Decompiled with JetBrains decompiler
// Type: interaction_CollectCompost
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System.Collections;
using UnityEngine;

#nullable disable
public class interaction_CollectCompost : Interaction
{
  public Structure Structure;
  public Structures_CompostBin _StructureInfo;
  public string sCollect;
  public bool Activating;

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public Structures_CompostBin StructureBrain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_CompostBin;
      return this._StructureInfo;
    }
    set => this._StructureInfo = value;
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.ContinuouslyHold = true;
    this.HasSecondaryInteraction = false;
    this.UpdateLocalisation();
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sCollect = ScriptLocalization.Interactions.Collect;
  }

  public override void GetLabel()
  {
    this.Interactable = this.StructureBrain.PoopCount > 0;
    string str;
    if (!this.Activating)
      str = string.Join(" ", this.sCollect, CostFormatter.FormatCost(InventoryItem.ITEM_TYPE.POOP, this.StructureBrain.PoopCount, ignoreAffordability: true));
    else
      str = string.Empty;
    this.Label = str;
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.StructureBrain.PoopCount <= 0)
      return;
    base.OnInteract(state);
    this.StartCoroutine((IEnumerator) this.GivePoopRoutine());
  }

  public IEnumerator GivePoopRoutine()
  {
    interaction_CollectCompost interactionCollectCompost = this;
    interactionCollectCompost.Activating = true;
    int Count = interactionCollectCompost.StructureBrain.PoopCount;
    interactionCollectCompost.StructureBrain.CollectPoop();
    int i = -1;
    while (++i < Count)
    {
      AudioManager.Instance.PlayOneShot("event:/followers/poop_pop", interactionCollectCompost.transform.position);
      ResourceCustomTarget.Create(interactionCollectCompost.state.gameObject, interactionCollectCompost.transform.position, InventoryItem.ITEM_TYPE.POOP, (System.Action) (() => Inventory.AddItem(39, 1)));
      yield return (object) new WaitForSeconds(0.1f);
    }
    interactionCollectCompost.Activating = false;
  }
}
