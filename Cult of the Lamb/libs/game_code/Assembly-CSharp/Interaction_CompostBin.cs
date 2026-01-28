// Decompiled with JetBrains decompiler
// Type: Interaction_CompostBin
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class Interaction_CompostBin : Interaction
{
  public List<GameObject> PoopProgress;
  public List<GameObject> GrassProgress;
  [SerializeField]
  public Canvas _progressCanvas;
  [SerializeField]
  public Image _progressImage;
  public Structure Structure;
  public Structures_CompostBin _StructureInfo;
  public bool Activating;
  public Vector3 previousPosition;
  public string sString;
  public string sCollect;
  public string sComposting;

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
    this.UpdateLocalisation();
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    if (this.Structure.Brain == null)
      return;
    this.OnBrainAssigned();
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    if (this.StructureBrain == null)
      return;
    Structures_CompostBin structureBrain = this.StructureBrain;
    structureBrain.OnItemRemoved = structureBrain.OnItemRemoved - new System.Action(this.UpdateImages);
    this.StructureBrain.UpdateCompostState -= new System.Action(this.UpdateImages);
  }

  public void OnBrainAssigned()
  {
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    Structures_CompostBin structureBrain = this.StructureBrain;
    structureBrain.OnItemRemoved = structureBrain.OnItemRemoved + new System.Action(this.UpdateImages);
    this.StructureBrain.UpdateCompostState += new System.Action(this.UpdateImages);
    this.UpdateImages();
  }

  public void UpdateImages()
  {
    if (this.StructureBrain == null)
      return;
    int index1 = -1;
    while (++index1 < this.PoopProgress.Count)
    {
      if ((UnityEngine.Object) this.PoopProgress[index1] != (UnityEngine.Object) null)
        this.PoopProgress[index1].SetActive(false);
    }
    if (this.StructureBrain.PoopCount > 0 && (UnityEngine.Object) this.PoopProgress[this.PoopProgress.Count - 1] != (UnityEngine.Object) null)
      this.PoopProgress[this.PoopProgress.Count - 1].SetActive(true);
    int index2 = -1;
    while (++index2 < this.GrassProgress.Count)
    {
      if ((UnityEngine.Object) this.GrassProgress[index2] != (UnityEngine.Object) null)
        this.GrassProgress[index2].SetActive(false);
    }
    if (this.StructureBrain.CompostCount <= 0 || !((UnityEngine.Object) this.GrassProgress[this.GrassProgress.Count - 1] != (UnityEngine.Object) null))
      return;
    this.GrassProgress[this.GrassProgress.Count - 1].SetActive(true);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    if (this.StructureBrain == null)
      return;
    this.StructureBrain.UpdateCompostState -= new System.Action(this.UpdateImages);
    Structures_CompostBin structureBrain = this.StructureBrain;
    structureBrain.OnItemRemoved = structureBrain.OnItemRemoved - new System.Action(this.UpdateImages);
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sString = ScriptLocalization.Interactions_Bank.Deposit;
    this.sCollect = ScriptLocalization.Interactions.Collect;
    this.sComposting = ScriptLocalization.Interactions.Composting;
    this.previousPosition = this.transform.position;
  }

  public string GetAffordColor()
  {
    return Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.GRASS) > 0 ? "<color=#f4ecd3>" : "<color=red>";
  }

  public override void GetLabel()
  {
    if (this.Activating || this.StructureBrain == null)
      this.Label = "";
    else if (this.StructureBrain.CompostCount <= 0 && this.StructureBrain.PoopCount <= 0)
    {
      this.Label = $"{this.sString} {CostFormatter.FormatCost(InventoryItem.ITEM_TYPE.GRASS, this.StructureBrain.CompostCost)}";
      this.Interactable = true;
    }
    else if (this.StructureBrain.CompostCount > 0 && this.StructureBrain.PoopCount <= 0)
    {
      this.Label = this.sComposting;
      this.Interactable = false;
    }
    else
    {
      if (this.StructureBrain.PoopCount <= 0)
        return;
      this.Label = $"{this.sCollect} <sprite name=\"icon_Poop\"> x{LocalizeIntegration.ReverseText(this.StructureBrain.PoopCount.ToString())}";
      this.Interactable = true;
    }
  }

  public override void OnInteract(StateMachine state)
  {
    this.state = state;
    if (this.Activating)
      return;
    if (this.StructureBrain.CompostCount <= 0 && this.StructureBrain.PoopCount <= 0)
    {
      if (Inventory.GetItemQuantity(35) >= this.StructureBrain.CompostCost)
      {
        base.OnInteract(state);
        this.StartCoroutine((IEnumerator) this.DepositGrassRoutine());
      }
      else
        this.playerFarming.indicator.PlayShake();
    }
    else
    {
      if (this.StructureBrain.PoopCount <= 0)
        return;
      this.StartCoroutine((IEnumerator) this.GivePoopRoutine());
    }
  }

  public IEnumerator DepositGrassRoutine()
  {
    Interaction_CompostBin interactionCompostBin = this;
    interactionCompostBin.Activating = true;
    int i = -1;
    while (++i < 10)
    {
      AudioManager.Instance.PlayOneShot("event:/material/footstep_bush", interactionCompostBin.transform.position);
      ResourceCustomTarget.Create(interactionCompostBin.gameObject, interactionCompostBin.state.transform.position, InventoryItem.ITEM_TYPE.GRASS, (System.Action) null);
      yield return (object) new WaitForSeconds(0.1f);
    }
    interactionCompostBin.StructureBrain.AddGrass();
    Inventory.ChangeItemQuantity(35, -interactionCompostBin.StructureBrain.CompostCost);
    interactionCompostBin.Activating = false;
  }

  public IEnumerator GivePoopRoutine()
  {
    Interaction_CompostBin interactionCompostBin = this;
    interactionCompostBin.Activating = true;
    int Count = interactionCompostBin.StructureBrain.PoopCount;
    interactionCompostBin.StructureBrain.CollectPoop();
    int i = -1;
    while (++i < Count)
    {
      AudioManager.Instance.PlayOneShot("event:/followers/pop_in", interactionCompostBin.transform.position);
      ResourceCustomTarget.Create(interactionCompostBin.playerFarming.gameObject, interactionCompostBin.transform.position, InventoryItem.ITEM_TYPE.POOP, (System.Action) (() => Inventory.AddItem(39, 1)));
      yield return (object) new WaitForSeconds(0.1f);
    }
    interactionCompostBin.Activating = false;
  }

  public void ClearAll()
  {
    this.StructureBrain.SetGrass(0);
    this.StructureBrain.SetPoop(0);
    this.UpdateImages();
  }

  public override void Update()
  {
    base.Update();
    if (this.StructureBrain == null)
      return;
    if ((double) this.StructureBrain.Data.Progress > 0.0)
    {
      float num = (TimeManager.TotalElapsedGameTime - this.StructureBrain.Data.Progress) / this.StructureBrain.COMPOST_DURATION;
      this._progressCanvas.gameObject.SetActive(false);
      this._progressCanvas.gameObject.SetActive(true);
      this._progressImage.fillAmount = num;
      if ((double) num < 1.0)
        return;
      this.StructureBrain.AddPoop();
      this.StructureBrain.Data.Progress = 0.0f;
      this._progressCanvas.gameObject.SetActive(false);
    }
    else
      this._progressCanvas.gameObject.SetActive(false);
  }

  public Vector3 GetCurrentUIProgressIndicatorPosition()
  {
    return this.transform.position + Vector3.up * 1.5f + Vector3.back * 1.5f - BiomeConstants.Instance.transform.position;
  }
}
