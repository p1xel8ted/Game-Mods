// Decompiled with JetBrains decompiler
// Type: Interaction_CompostBinDeadBody
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class Interaction_CompostBinDeadBody : Interaction
{
  public static List<Interaction_CompostBinDeadBody> DeadBodyCompost = new List<Interaction_CompostBinDeadBody>();
  public List<GameObject> PoopProgress;
  public List<GameObject> GrassProgress;
  [SerializeField]
  public Canvas _progressCanvas;
  [SerializeField]
  public Image _progressImage;
  public Structure Structure;
  public Structures_DeadBodyCompost _StructureInfo;
  public bool Activating;
  public Vector3 previousPosition;
  public string sString;
  public string sCollect;
  public string sComposting;
  public string sPlaceDeadFollower;
  public bool Activated;

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public Structures_DeadBodyCompost StructureBrain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_DeadBodyCompost;
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
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    if (this.Structure.Brain != null)
      this.OnBrainAssigned();
    Interaction_CompostBinDeadBody.DeadBodyCompost.Add(this);
  }

  public void OnBrainAssigned()
  {
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    Structures_DeadBodyCompost structureBrain1 = this.StructureBrain;
    structureBrain1.UpdateCompostState = structureBrain1.UpdateCompostState - new System.Action(this.UpdateImages);
    Structures_DeadBodyCompost structureBrain2 = this.StructureBrain;
    structureBrain2.UpdateCompostState = structureBrain2.UpdateCompostState + new System.Action(this.UpdateImages);
    Structures_DeadBodyCompost structureBrain3 = this.StructureBrain;
    structureBrain3.OnItemRemoved = structureBrain3.OnItemRemoved + new System.Action(this.UpdateImages);
    Structures_DeadBodyCompost structureBrain4 = this.StructureBrain;
    structureBrain4.OnItemRemoved = structureBrain4.OnItemRemoved - new System.Action(this.UpdateImages);
    this.UpdateImages();
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    if (this.StructureBrain != null)
    {
      Structures_DeadBodyCompost structureBrain1 = this.StructureBrain;
      structureBrain1.OnItemRemoved = structureBrain1.OnItemRemoved - new System.Action(this.UpdateImages);
      Structures_DeadBodyCompost structureBrain2 = this.StructureBrain;
      structureBrain2.UpdateCompostState = structureBrain2.UpdateCompostState - new System.Action(this.UpdateImages);
    }
    Interaction_CompostBinDeadBody.DeadBodyCompost.Remove(this);
  }

  public void UpdateImages()
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

  public override void OnDestroy()
  {
    base.OnDestroy();
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    if (this.StructureBrain == null)
      return;
    Structures_DeadBodyCompost structureBrain1 = this.StructureBrain;
    structureBrain1.UpdateCompostState = structureBrain1.UpdateCompostState - new System.Action(this.UpdateImages);
    Structures_DeadBodyCompost structureBrain2 = this.StructureBrain;
    structureBrain2.OnItemRemoved = structureBrain2.OnItemRemoved - new System.Action(this.UpdateImages);
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sString = ScriptLocalization.Interactions.DepositCompost;
    this.sCollect = ScriptLocalization.Interactions.Collect;
    this.sComposting = ScriptLocalization.Interactions.Composting;
    this.sPlaceDeadFollower = ScriptLocalization.Interactions.PlaceBodyToCompost;
    this.previousPosition = this.transform.position;
  }

  public override void GetLabel()
  {
    if (!this.Activating && this.StructureBrain.CompostCount > 0 && this.StructureBrain.PoopCount <= 0)
    {
      this.Label = this.sComposting;
      this.Interactable = false;
    }
    else if (!this.Activating && this.StructureBrain.PoopCount > 0)
    {
      this.Label = $"{this.sCollect} <sprite name=\"icon_Poop\"> x{LocalizeIntegration.ReverseText(this.StructureBrain.PoopCount.ToString())}";
      this.Interactable = true;
    }
    else
    {
      this.Label = this.sPlaceDeadFollower;
      this.Interactable = false;
    }
  }

  public override void OnInteract(StateMachine state)
  {
    this.state = state;
    if (this.StructureBrain.PoopCount <= 0)
      return;
    this.StartCoroutine((IEnumerator) this.GivePoopRoutine());
  }

  public void BuryBody()
  {
    Debug.Log((object) "BURY BDDY!");
    this.StructureBrain.AddGrass();
  }

  public IEnumerator DepositGrassRoutine()
  {
    Interaction_CompostBinDeadBody compostBinDeadBody = this;
    int i = -1;
    while (++i < compostBinDeadBody.StructureBrain.CompostCost)
    {
      AudioManager.Instance.PlayOneShot("event:/material/footstep_grass", compostBinDeadBody.gameObject);
      ResourceCustomTarget.Create(compostBinDeadBody.gameObject, compostBinDeadBody.state.transform.position, InventoryItem.ITEM_TYPE.GRASS, (System.Action) (() => { }));
      yield return (object) new WaitForSeconds(0.05f);
    }
    compostBinDeadBody.StructureBrain.AddGrass();
    Inventory.ChangeItemQuantity(35, -compostBinDeadBody.StructureBrain.CompostCost);
    compostBinDeadBody.Activated = false;
  }

  public IEnumerator GivePoopRoutine()
  {
    Interaction_CompostBinDeadBody compostBinDeadBody = this;
    compostBinDeadBody.Activating = true;
    int Count = compostBinDeadBody.StructureBrain.PoopCount;
    compostBinDeadBody.StructureBrain.CollectPoop();
    int i = -1;
    while (++i < Count)
    {
      AudioManager.Instance.PlayOneShot("event:/followers/poop_pop", compostBinDeadBody.gameObject);
      ResourceCustomTarget.Create(compostBinDeadBody.playerFarming.gameObject, compostBinDeadBody.transform.position, InventoryItem.ITEM_TYPE.POOP, (System.Action) (() => Inventory.AddItem(39, 1)));
      yield return (object) new WaitForSeconds(0.1f);
    }
    compostBinDeadBody.Activating = false;
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
