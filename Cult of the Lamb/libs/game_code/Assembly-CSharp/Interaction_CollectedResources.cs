// Decompiled with JetBrains decompiler
// Type: Interaction_CollectedResources
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_CollectedResources : Interaction
{
  public GameObject ChestNoItems;
  public GameObject ChestWithItems;
  public Structure structure;
  public Structures_CollectedResourceChest _Structure_Info;
  public bool Activating;
  public Vector3 PunchScale = new Vector3(0.1f, 0.1f, 0.1f);

  public StructuresData StructureInfo => this.structure.Structure_Info;

  public Structures_CollectedResourceChest StructureBrain
  {
    get
    {
      if (this._Structure_Info == null)
        this._Structure_Info = this.structure.Brain as Structures_CollectedResourceChest;
      return this._Structure_Info;
    }
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.ActivateDistance = 2f;
    if ((UnityEngine.Object) this.structure != (UnityEngine.Object) null && this.structure.Brain != null)
    {
      if (this.StructureInfo.Inventory.Count > 0 && this.ChestNoItems.activeSelf)
        AudioManager.Instance.PlayOneShot("event:/chests/chest_small_open");
      this.OnBrainAssigned();
    }
    else
    {
      if (!(bool) (UnityEngine.Object) this.structure)
        return;
      this.structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    }
  }

  public void OnBrainAssigned()
  {
    this.UpdateImage();
    this.structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    this.structure.Brain.OnItemDeposited += new System.Action(this.OnItemDeposited);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if (!((UnityEngine.Object) this.structure != (UnityEngine.Object) null) || this.structure.Brain == null)
      return;
    this.structure.Brain.OnItemDeposited -= new System.Action(this.OnItemDeposited);
  }

  public void OnItemDeposited()
  {
    if ((UnityEngine.Object) this == (UnityEngine.Object) null)
      return;
    this.UpdateImage();
    this.transform.DOKill();
    this.transform.localScale = Vector3.one;
    this.transform.DOPunchScale(new Vector3(0.2f, 0.1f), 1f, 5);
  }

  public void UpdateImage()
  {
    if (this.StructureInfo == null || this.StructureInfo.Inventory == null)
      return;
    if (this.StructureInfo.Inventory.Count > 0)
    {
      if ((UnityEngine.Object) this.ChestNoItems != (UnityEngine.Object) null)
        this.ChestNoItems.SetActive(false);
      if ((UnityEngine.Object) this.ChestWithItems != (UnityEngine.Object) null)
        this.ChestWithItems.SetActive(true);
      this.OutlineTarget = this.ChestWithItems;
      this.ActivatorOffset = new Vector3(2.5f, -1f);
      this.ActivateDistance = 3f;
    }
    else
    {
      if ((UnityEngine.Object) this.ChestNoItems != (UnityEngine.Object) null)
        this.ChestNoItems.SetActive(true);
      if ((UnityEngine.Object) this.ChestWithItems != (UnityEngine.Object) null)
        this.ChestWithItems.SetActive(false);
      this.OutlineTarget = this.ChestNoItems;
      this.ActivatorOffset = Vector3.zero;
      this.ActivateDistance = 2f;
    }
  }

  public override void OnDisableInteraction() => base.OnDisableInteraction();

  public override void Update()
  {
    base.Update();
    this.AutomaticallyInteract = (UnityEngine.Object) this.structure != (UnityEngine.Object) null && this.structure.Structure_Info != null && this.structure.Structure_Info.Inventory.Count > 0;
    this.Interactable = this.AutomaticallyInteract;
  }

  public override void GetLabel()
  {
    if (this.StructureInfo == null)
      return;
    if (this.Activating)
      this.Label = "";
    else
      this.Label = this.Label = ScriptLocalization.Interactions.BaseChest;
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.Activating || this.structure.Structure_Info.Inventory.Count <= 0)
      return;
    this.Activating = true;
    base.OnInteract(state);
    this.StartCoroutine((IEnumerator) this.GiveResourcesRoutine());
  }

  public IEnumerator GiveResourcesRoutine()
  {
    Interaction_CollectedResources collectedResources = this;
    if ((UnityEngine.Object) collectedResources.structure != (UnityEngine.Object) null && collectedResources.structure.Structure_Info != null && collectedResources.structure.Structure_Info.Inventory != null && collectedResources.structure.Structure_Info.Inventory.Count > 0)
    {
      collectedResources.gameObject.transform.DOKill();
      collectedResources.gameObject.transform.DOPunchScale(collectedResources.PunchScale, 1f);
      for (int t = collectedResources.structure.Structure_Info.Inventory.Count - 1; t >= 0; --t)
      {
        int Target = Mathf.Min(5, collectedResources.structure.Structure_Info.Inventory[t].quantity);
        for (int i = 0; i < Target; ++i)
        {
          ResourceCustomTarget.Create(collectedResources.state.gameObject, collectedResources.transform.position, (InventoryItem.ITEM_TYPE) collectedResources.structure.Structure_Info.Inventory[t].type, (System.Action) null);
          AudioManager.Instance.PlayOneShot("event:/chests/chest_item_spawn", collectedResources.transform.position);
          yield return (object) new WaitForSeconds(0.05f);
        }
        Inventory.AddItem(collectedResources.structure.Structure_Info.Inventory[t].type, collectedResources.structure.Structure_Info.Inventory[t].quantity);
      }
      yield return (object) new WaitForSeconds(1f);
      collectedResources.gameObject.transform.DOKill();
      collectedResources.gameObject.transform.DOPunchScale(collectedResources.PunchScale * 2f, 0.2f, 1);
      AudioManager.Instance.PlayOneShot("event:/chests/chest_small_land", collectedResources.transform.position);
      collectedResources.ChestNoItems.SetActive(true);
      collectedResources.ChestWithItems.SetActive(false);
      collectedResources.OutlineTarget = collectedResources.ChestNoItems;
      yield return (object) new WaitForSeconds(1f);
      collectedResources.structure.Structure_Info.Inventory.Clear();
      collectedResources.Activating = false;
    }
  }
}
