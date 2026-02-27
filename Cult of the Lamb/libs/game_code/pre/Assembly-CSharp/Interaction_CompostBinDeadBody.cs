// Decompiled with JetBrains decompiler
// Type: Interaction_CompostBinDeadBody
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_CompostBinDeadBody : Interaction
{
  public static List<Interaction_CompostBinDeadBody> DeadBodyCompost = new List<Interaction_CompostBinDeadBody>();
  public List<GameObject> PoopProgress;
  public List<GameObject> GrassProgress;
  public Structure Structure;
  private Structures_DeadBodyCompost _StructureInfo;
  private bool Activating;
  private UIProgressIndicator _uiProgressIndicator;
  private string sString;
  private string sCollect;
  private string sComposting;
  private string sPlaceDeadFollower;
  private bool Activated;

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

  private void OnBrainAssigned()
  {
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    this.StructureBrain.UpdateCompostState += new System.Action(this.UpdateImages);
    this.UpdateImages();
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    if (this.StructureBrain != null)
      this.StructureBrain.UpdateCompostState -= new System.Action(this.UpdateImages);
    Interaction_CompostBinDeadBody.DeadBodyCompost.Remove(this);
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
    this.sString = ScriptLocalization.Interactions.DepositCompost;
    this.sCollect = ScriptLocalization.Interactions.Collect;
    this.sComposting = ScriptLocalization.Interactions.Composting;
    this.sPlaceDeadFollower = ScriptLocalization.Interactions.PlaceBodyToCompost;
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
      this.Label = $"{this.sCollect} <sprite name=\"icon_Poop\"> x{(object) this.StructureBrain.PoopCount}";
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
    if (this.StructureBrain.PoopCount <= 0)
      return;
    this.StartCoroutine((IEnumerator) this.GivePoopRoutine());
  }

  public void BuryBody()
  {
    Debug.Log((object) "BURY BDDY!");
    this.StructureBrain.AddGrass();
  }

  private IEnumerator DepositGrassRoutine()
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

  private IEnumerator GivePoopRoutine()
  {
    Interaction_CompostBinDeadBody compostBinDeadBody = this;
    compostBinDeadBody.Activating = true;
    int Count = compostBinDeadBody.StructureBrain.PoopCount;
    compostBinDeadBody.StructureBrain.CollectPoop();
    int i = -1;
    while (++i < Count)
    {
      AudioManager.Instance.PlayOneShot("event:/followers/poop_pop", compostBinDeadBody.gameObject);
      ResourceCustomTarget.Create(PlayerFarming.Instance.gameObject, compostBinDeadBody.transform.position, InventoryItem.ITEM_TYPE.POOP, (System.Action) (() => Inventory.AddItem(39, 1)));
      yield return (object) new WaitForSeconds(0.1f);
    }
    compostBinDeadBody.Activating = false;
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
