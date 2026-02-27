// Decompiled with JetBrains decompiler
// Type: Interaction_CompostBin
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_CompostBin : Interaction
{
  public List<GameObject> PoopProgress;
  public List<GameObject> GrassProgress;
  public Structure Structure;
  private UIProgressIndicator _uiProgressIndicator;
  private Structures_CompostBin _StructureInfo;
  private bool Activating;
  private string sString;
  private string sCollect;
  private string sComposting;

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

  private void OnBrainAssigned()
  {
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    this.StructureBrain.UpdateCompostState += new System.Action(this.UpdateImages);
    this.UpdateImages();
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    if (this.StructureBrain == null)
      return;
    this.StructureBrain.UpdateCompostState -= new System.Action(this.UpdateImages);
  }

  private void UpdateImages()
  {
    int index1 = -1;
    while (++index1 < this.PoopProgress.Count)
      this.PoopProgress[index1].SetActive(false);
    if (this.StructureBrain.PoopCount > 0)
      this.PoopProgress[this.PoopProgress.Count - 1].SetActive(true);
    int index2 = -1;
    while (++index2 < this.GrassProgress.Count)
      this.GrassProgress[index2].SetActive(false);
    if (this.StructureBrain.CompostCount <= 0)
      return;
    this.GrassProgress[this.GrassProgress.Count - 1].SetActive(true);
  }

  protected override void OnDestroy()
  {
    base.OnDestroy();
    if ((UnityEngine.Object) this._uiProgressIndicator != (UnityEngine.Object) null)
    {
      this._uiProgressIndicator.Recycle<UIProgressIndicator>();
      this._uiProgressIndicator = (UIProgressIndicator) null;
    }
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sString = ScriptLocalization.Interactions_Bank.Deposit;
    this.sCollect = ScriptLocalization.Interactions.Collect;
    this.sComposting = ScriptLocalization.Interactions.Composting;
  }

  private string GetAffordColor()
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
      this.Label = $"{this.sCollect} <sprite name=\"icon_Poop\"> x{(object) this.StructureBrain.PoopCount}";
      this.Interactable = true;
    }
  }

  public override void OnInteract(StateMachine state)
  {
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
        MonoSingleton<Indicator>.Instance.PlayShake();
    }
    else
    {
      if (this.StructureBrain.PoopCount <= 0)
        return;
      this.StartCoroutine((IEnumerator) this.GivePoopRoutine());
    }
  }

  private IEnumerator DepositGrassRoutine()
  {
    Interaction_CompostBin interactionCompostBin = this;
    interactionCompostBin.Activating = true;
    int i = -1;
    while (++i < interactionCompostBin.StructureBrain.CompostCost)
    {
      AudioManager.Instance.PlayOneShot("event:/material/footstep_bush", interactionCompostBin.transform.position);
      ResourceCustomTarget.Create(interactionCompostBin.gameObject, interactionCompostBin.state.transform.position, InventoryItem.ITEM_TYPE.GRASS, (System.Action) null);
      yield return (object) new WaitForSeconds(0.02f);
    }
    interactionCompostBin.StructureBrain.AddGrass();
    Inventory.ChangeItemQuantity(35, -interactionCompostBin.StructureBrain.CompostCost);
    interactionCompostBin.Activating = false;
  }

  private IEnumerator GivePoopRoutine()
  {
    Interaction_CompostBin interactionCompostBin = this;
    interactionCompostBin.Activating = true;
    int Count = interactionCompostBin.StructureBrain.PoopCount;
    interactionCompostBin.StructureBrain.CollectPoop();
    int i = -1;
    while (++i < Count)
    {
      AudioManager.Instance.PlayOneShot("event:/followers/pop_in", interactionCompostBin.transform.position);
      ResourceCustomTarget.Create(PlayerFarming.Instance.gameObject, interactionCompostBin.transform.position, InventoryItem.ITEM_TYPE.POOP, (System.Action) (() => Inventory.AddItem(39, 1)));
      yield return (object) new WaitForSeconds(0.1f);
    }
    interactionCompostBin.Activating = false;
  }

  private void ClearAll()
  {
    this.StructureBrain.CompostCount = 0;
    this.StructureBrain.PoopCount = 0;
    this.UpdateImages();
  }

  protected override void Update()
  {
    base.Update();
    if (this.StructureBrain == null || this.StructureBrain.DepositTimes.Count <= 0)
      return;
    float progress = (TimeManager.TotalElapsedGameTime - this.StructureBrain.DepositTimes[0]) / Structures_CompostBin.COMPOST_DURATION;
    if ((UnityEngine.Object) this._uiProgressIndicator == (UnityEngine.Object) null)
    {
      this._uiProgressIndicator = BiomeConstants.Instance.ProgressIndicatorTemplate.Spawn<UIProgressIndicator>(BiomeConstants.Instance.transform, this.transform.position + Vector3.up * 1.5f + Vector3.back * 1.5f - BiomeConstants.Instance.transform.position);
      this._uiProgressIndicator.Show(progress);
      this._uiProgressIndicator.OnHidden += (System.Action) (() => this._uiProgressIndicator = (UIProgressIndicator) null);
    }
    else
    {
      this._uiProgressIndicator.SetProgress(progress);
      if ((double) progress < 1.0)
        return;
      this.StructureBrain.AddPoop();
      this.StructureBrain.DepositTimes.Clear();
      this._uiProgressIndicator.Hide();
    }
  }
}
