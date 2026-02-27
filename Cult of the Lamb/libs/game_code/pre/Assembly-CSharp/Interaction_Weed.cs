// Decompiled with JetBrains decompiler
// Type: Interaction_Weed
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_Weed : Interaction
{
  public static List<Interaction_Weed> Weeds = new List<Interaction_Weed>();
  public Structure Structure;
  private Structures_Weeds _StructureBrain;
  public static System.Action<Interaction_Weed> PlayerActivatingStart;
  public static System.Action<Interaction_Weed> PlayerActivatingEnd;
  public GameObject BuildSiteProgressUIPrefab;
  private BuildSitePlotProgressUI ProgressUI;
  private string sString;
  private float ShowTimer;
  private bool SubscribedEvent;
  private Vector3 RandomRot;
  private bool EventListenerActive;

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public Structures_Weeds StructureBrain
  {
    get
    {
      if (this._StructureBrain == null && this.Structure.Brain != null)
        this._StructureBrain = this.Structure.Brain as Structures_Weeds;
      return this._StructureBrain;
    }
    set => this._StructureBrain = value;
  }

  public bool Activating
  {
    get => this.StructureBrain != null && this.StructureBrain.ReservedByPlayer;
    set => this.StructureBrain.ReservedByPlayer = value;
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    Interaction_Weed.Weeds.Add(this);
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    if (this.Structure.Brain == null)
      return;
    this.OnBrainAssigned();
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    Interaction_Weed.Weeds.Remove(this);
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null && (UnityEngine.Object) PlayerFarming.Instance.Spine != (UnityEngine.Object) null)
    {
      PlayerFarming.Instance.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
      this.SubscribedEvent = false;
    }
    if (this.StructureBrain != null && this.StructureBrain.Data != null)
    {
      this.StructureBrain.Data.Progress = 0.0f;
      this.StructureBrain.OnProgressChanged -= new System.Action(this.OnRemovalProgressChanged);
      this.StructureBrain.OnComplete -= new System.Action(this.WeedsPulled);
    }
    this.EventListenerActive = false;
    if (this.StructureInfo == null || !this.StructureInfo.Destroyed || this.StructureBrain == null || this.StructureBrain.ForceRemoved)
      return;
    AudioManager.Instance.PlayOneShot("event:/player/weed_done", this.transform.position);
    BiomeConstants.Instance?.EmitSmokeExplosionVFX(this.gameObject.transform.position);
    Vector3 Velocity = (this.gameObject.transform.position - PlayerFarming.Instance.transform.position) * 5f;
    BiomeConstants.Instance.EmitParticleChunk(BiomeConstants.TypeOfParticle.grass, this.transform.position, Velocity, 7);
    if (!this.StructureBrain.DropWeed)
      return;
    InventoryItem.Spawn(InventoryItem.ITEM_TYPE.GRASS, 1, this.transform.position);
  }

  private void OnBrainAssigned()
  {
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    this.OnRemovalProgressChanged();
    this.StructureBrain.OnProgressChanged += new System.Action(this.OnRemovalProgressChanged);
    this.StructureBrain.OnComplete += new System.Action(this.WeedsPulled);
  }

  private void HandleEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (!(e.Data.Name == "Chop"))
      return;
    this.EventListenerActive = true;
    AudioManager.Instance.PlayOneShot("event:/player/weed_pick", this.gameObject);
    this.StructureBrain.PickWeeds(2f);
  }

  private void Start()
  {
    this.OnRemovalProgressChanged();
    this.UpdateLocalisation();
    this.ContinuouslyHold = true;
  }

  public void ShowUI()
  {
    if ((UnityEngine.Object) this.ProgressUI != (UnityEngine.Object) null && !this.ProgressUI.gameObject.activeSelf)
      this.ProgressUI.Show();
    this.ShowTimer = 3f;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sString = ScriptLocalization.Interactions.ClearWeeds;
  }

  public override void GetLabel()
  {
    if (this.StructureInfo != null && !this.StructureInfo.PrioritisedAsBuildingObstruction)
      this.Label = "";
    else
      this.Label = this.sString;
  }

  private new void Update()
  {
    if ((double) this.ShowTimer <= 0.0)
      return;
    this.ShowTimer -= Time.deltaTime;
    if ((double) this.ShowTimer > 0.0)
      return;
    this.ProgressUI.Hide();
  }

  private void OnRemovalProgressChanged()
  {
    if (this.StructureBrain == null || this.StructureBrain.Data == null)
      return;
    if ((double) this.StructureBrain.Data.Progress == 0.0)
    {
      if (!((UnityEngine.Object) this.ProgressUI != (UnityEngine.Object) null))
        return;
      this.ProgressUI.gameObject.SetActive(false);
    }
    else
    {
      if ((UnityEngine.Object) this.ProgressUI == (UnityEngine.Object) null)
        this.ProgressUI = UnityEngine.Object.Instantiate<GameObject>(CanvasConstants.instance.BuildSiteProgressUIPrefab, CanvasConstants.instance.transform).GetComponent<BuildSitePlotProgressUI>();
      this.ShowUI();
      if (!((UnityEngine.Object) this.ProgressUI != (UnityEngine.Object) null))
        return;
      this.ProgressUI.UpdateProgress(this.StructureBrain.Data.Progress / this.StructureBrain.Data.ProgressTarget);
    }
  }

  private void LateUpdate()
  {
    if (!((UnityEngine.Object) this.ProgressUI != (UnityEngine.Object) null) || (double) this.ProgressUI.canvasGroup.alpha <= 0.0)
      return;
    this.ProgressUI.SetPosition(this.transform.position);
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.Activating)
      return;
    base.OnInteract(state);
    if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null && !this.SubscribedEvent)
    {
      PlayerFarming.Instance.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
      this.SubscribedEvent = true;
    }
    this.StartCoroutine((IEnumerator) this.DoBuild());
  }

  private IEnumerator DoBuild()
  {
    Interaction_Weed interactionWeed = this;
    interactionWeed.Activating = true;
    System.Action<Interaction_Weed> playerActivatingStart = Interaction_Weed.PlayerActivatingStart;
    if (playerActivatingStart != null)
      playerActivatingStart(interactionWeed);
    interactionWeed.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    interactionWeed.state.facingAngle = Utils.GetAngle(interactionWeed.state.transform.position, interactionWeed.transform.position);
    yield return (object) new WaitForEndOfFrame();
    PlayerFarming.Instance.simpleSpineAnimator.Animate("actions/collect-berries", 0, true);
    yield return (object) new WaitForSeconds(0.933333337f);
    while (InputManager.Gameplay.GetInteractButtonHeld())
      yield return (object) null;
    if (interactionWeed.EventListenerActive)
      PlayerFarming.Instance.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(interactionWeed.HandleEvent);
    interactionWeed.SubscribedEvent = false;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.Idle;
    interactionWeed.StopAllCoroutines();
    interactionWeed.Activating = false;
    interactionWeed.Interactable = true;
  }

  private void WeedsPulled()
  {
    if ((UnityEngine.Object) this.state == (UnityEngine.Object) PlayerFarming.Instance._state)
      RumbleManager.Instance.Rumble();
    this.StructureBrain.Remove();
    if (this.Activating)
      this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.Activating = false;
    System.Action<Interaction_Weed> playerActivatingEnd = Interaction_Weed.PlayerActivatingEnd;
    if (playerActivatingEnd == null)
      return;
    playerActivatingEnd(this);
  }

  private new void OnDestroy()
  {
    if ((UnityEngine.Object) this.ProgressUI != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.ProgressUI.gameObject);
    if (this.StructureInfo != null || this.StructureBrain != null)
      return;
    if (this.StructureInfo != null && this.StructureBrain != null && this.StructureInfo.Destroyed && !this.StructureBrain.ForceRemoved)
    {
      AudioManager.Instance.PlayOneShot("event:/player/weed_done", this.transform.position);
      BiomeConstants.Instance?.EmitSmokeExplosionVFX(this.gameObject.transform.position);
      if (this.StructureBrain.DropWeed)
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.GRASS, 1, this.transform.position);
    }
    if (this.Activating)
    {
      this.StopAllCoroutines();
      this.state.CURRENT_STATE = StateMachine.State.Idle;
    }
    if (this.StructureBrain == null)
      return;
    this.StructureBrain.OnProgressChanged -= new System.Action(this.OnRemovalProgressChanged);
    this.StructureBrain.OnComplete -= new System.Action(this.WeedsPulled);
  }
}
