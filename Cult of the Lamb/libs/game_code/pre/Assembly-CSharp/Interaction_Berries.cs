// Decompiled with JetBrains decompiler
// Type: Interaction_Berries
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Interaction_Berries : Interaction
{
  public const int BERRY_MAX_HP = 10;
  public static List<Interaction_Berries> Berries = new List<Interaction_Berries>();
  public Structure Structure;
  private Structures_BerryBush _StructureBrain;
  private float BerryPickingTime = 2.5f;
  private float BerryPickingIncrements = 20f;
  public bool DisableOnDie;
  private string sLabelName;
  public bool Activated;
  public static System.Action<Interaction_Berries> PlayerActivatingStart;
  public static System.Action<Interaction_Berries> PlayerActivatingEnd;
  public int ReservedByFollowerID;
  public GameObject PlayerPositionLeft;
  public GameObject PlayerPositionRight;
  public UnityEvent callBackOnHarvest;
  public GameObject berryBush_Normal;
  public GameObject berryBush_Cut;
  public GameObject berryToShake;
  [Range(0.0f, 1f)]
  public float BreakCameraShake;
  public int maxParticles = 10;
  public List<Sprite> ParticleChunks;
  public GameObject particleSpawn;
  public float zSpawn;
  private SpriteRenderer spriteRenderer;
  public Color minColor = new Color(0.1764706f, 0.4039216f, 0.3294118f, 1f);
  public Color maxColor = new Color(0.1411765f, 0.2705882f, 0.2627451f, 1f);
  public UIProgressIndicator _uiProgressIndicator;
  private bool harvested;
  public InventoryItem.ITEM_TYPE BerryBushType = InventoryItem.ITEM_TYPE.BERRY;
  private float pickBerriesTime;
  public bool buttonDown;
  private float ShowTimer;
  public bool inTrigger;

  public StructuresData StructureInfo
  {
    get
    {
      return !((UnityEngine.Object) this.Structure != (UnityEngine.Object) null) ? (StructuresData) null : this.Structure.Structure_Info;
    }
  }

  public Structures_BerryBush StructureBrain
  {
    get
    {
      if (this._StructureBrain == null && (UnityEngine.Object) this.Structure != (UnityEngine.Object) null && this.Structure.Brain != null)
        this._StructureBrain = this.Structure.Brain as Structures_BerryBush;
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
    Interaction_Berries.Berries.Add(this);
    this.transform.localScale = Vector3.one;
    if ((bool) (UnityEngine.Object) this.Structure)
    {
      this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
      if (this.Structure.Brain != null)
        this.OnBrainAssigned();
    }
    this.harvested = false;
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    Interaction_Berries.Berries.Remove(this);
    if (this.StructureBrain == null)
      return;
    this.StructureBrain.OnTreeProgressChanged -= new System.Action(this.OnRemovalProgressChanged);
    this.StructureBrain.OnTreeComplete -= new System.Action<bool>(this.PickedBerries);
  }

  protected override void OnDestroy()
  {
    base.OnDestroy();
    if ((UnityEngine.Object) this._uiProgressIndicator != (UnityEngine.Object) null)
    {
      this._uiProgressIndicator.Recycle<UIProgressIndicator>();
      this._uiProgressIndicator = (UIProgressIndicator) null;
    }
    if (this.StructureBrain == null)
      return;
    this.StructureBrain.OnTreeComplete -= new System.Action<bool>(this.PickedBerries);
    this.StructureBrain.OnTreeProgressChanged -= new System.Action(this.OnRemovalProgressChanged);
    this.StructureBrain.OnTreeComplete -= new System.Action<bool>(this.PickedBerries);
  }

  private void OnBrainAssigned()
  {
    this.UpdateLocalisation();
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    if (this.StructureBrain != null)
    {
      if (this.StructureBrain.BerryPicked)
      {
        if (this.DisableOnDie)
        {
          this.gameObject.SetActive(false);
        }
        else
        {
          this.berryBush_Normal.SetActive(false);
          this.berryBush_Cut.SetActive(true);
          this.Interactable = false;
        }
      }
      else
      {
        this.StructureBrain.OnTreeProgressChanged += new System.Action(this.OnRemovalProgressChanged);
        this.StructureBrain.OnTreeComplete += new System.Action<bool>(this.PickedBerries);
      }
    }
    if ((UnityEngine.Object) this.GetComponentInParent<CropController>() != (UnityEngine.Object) null)
    {
      this.StructureBrain.IsCrop = true;
      this.StructureBrain.CropID = !((UnityEngine.Object) this.GetComponentInParent<FarmPlot>() != (UnityEngine.Object) null) || this.GetComponentInParent<FarmPlot>().StructureInfo == null ? -1 : this.GetComponentInParent<FarmPlot>().StructureInfo.ID;
    }
    else
      this.StructureBrain.IsCrop = false;
  }

  private void OnStructuresPlaced()
  {
    if (this.StructureInfo == null || !this.StructureBrain.BerryPicked)
      return;
    if (this.DisableOnDie)
    {
      this.gameObject.SetActive(false);
    }
    else
    {
      this.berryBush_Normal.SetActive(false);
      this.berryBush_Cut.SetActive(true);
      this.Interactable = false;
    }
  }

  private void Start() => this.UpdateLocalisation();

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    switch (this.BerryBushType)
    {
      case InventoryItem.ITEM_TYPE.BERRY:
        this.sLabelName = ScriptLocalization.Interactions.PickBerries;
        break;
      case InventoryItem.ITEM_TYPE.MUSHROOM_BIG:
        this.sLabelName = ScriptLocalization.Interactions.PickMushrooms.Replace("brown", "#D95B5C");
        break;
      case InventoryItem.ITEM_TYPE.PUMPKIN:
        this.sLabelName = ScriptLocalization.Interactions.PickPumpkins;
        break;
      case InventoryItem.ITEM_TYPE.FLOWER_RED:
        this.sLabelName = ScriptLocalization.Interactions.PickRedFlower;
        break;
      case InventoryItem.ITEM_TYPE.FLOWER_WHITE:
        this.sLabelName = "";
        break;
      case InventoryItem.ITEM_TYPE.BEETROOT:
        this.sLabelName = ScriptLocalization.Interactions.PickBeetroots;
        break;
      case InventoryItem.ITEM_TYPE.CAULIFLOWER:
        this.sLabelName = ScriptLocalization.Interactions.PickCauliflower;
        break;
    }
  }

  public override void GetLabel()
  {
    if (!this.Activated && this.StructureBrain != null)
      this.Label = this.StructureBrain.BerryPicked ? "" : this.sLabelName;
    else
      this.Label = "";
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.Activated)
      return;
    this.Activated = true;
    base.OnInteract(state);
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.PickBerries());
    this.Interactable = false;
  }

  private IEnumerator PickBerries()
  {
    Interaction_Berries interactionBerries = this;
    interactionBerries.buttonDown = true;
    interactionBerries.Activating = true;
    System.Action<Interaction_Berries> playerActivatingStart = Interaction_Berries.PlayerActivatingStart;
    if (playerActivatingStart != null)
      playerActivatingStart(interactionBerries);
    PlayerFarming.Instance.TimedAction(10f, (System.Action) null, "actions/collect-berries");
    PlayerFarming.Instance.state.facingAngle = Utils.GetAngle(interactionBerries.transform.position, interactionBerries.transform.position);
    interactionBerries.StartCoroutine((IEnumerator) interactionBerries.berryTimer());
    interactionBerries.pickBerriesTime = 0.5f + UpgradeSystem.Foraging;
    while (interactionBerries.buttonDown && !interactionBerries.StructureBrain.BerryPicked && PlayerFarming.Instance.state.CURRENT_STATE == StateMachine.State.TimedAction)
      yield return (object) null;
    interactionBerries.EndPicking();
    System.Action<Interaction_Berries> playerActivatingEnd = Interaction_Berries.PlayerActivatingEnd;
    if (playerActivatingEnd != null)
      playerActivatingEnd(interactionBerries);
  }

  private IEnumerator berryTimer()
  {
    while (!this.StructureBrain.BerryPicked)
    {
      yield return (object) new WaitForSeconds(this.BerryPickingTime / this.BerryPickingIncrements);
      this.BerryRummage();
    }
  }

  public void BerryRummage()
  {
    this.StructureBrain.PickBerries(this.StructureInfo.ProgressTarget / this.BerryPickingIncrements);
    AudioManager.Instance.PlayOneShot("event:/material/footstep_bush", this.transform.position);
    CameraManager.shakeCamera(0.1f, Utils.GetAngle(PlayerFarming.Instance.gameObject.transform.position, this.transform.position));
    int num = -1;
    if (this.ParticleChunks.Count > 0)
    {
      while (++num < this.maxParticles / 5)
      {
        float t = (float) (UnityEngine.Random.Range(0, 100) / 100);
        Particle_Chunk.AddNew(this.transform.position, Utils.GetAngle(PlayerFarming.Instance.gameObject.transform.position, this.transform.position) + (float) UnityEngine.Random.Range(-20, 20), Color.Lerp(this.minColor, this.maxColor, t), this.ParticleChunks);
      }
    }
    this.berryToShake.transform.DORestart();
    this.berryToShake.transform.DOShakePosition(0.033f, 0.033f, 13, 48.8f);
  }

  public void PickedBerries(bool dropLoot)
  {
    if (this.harvested)
      return;
    this.harvested = true;
    BiomeConstants.Instance.EmitSmokeExplosionVFX(this.transform.position);
    AudioManager.Instance.PlayOneShot("event:/player/weed_done", this.transform.position);
    int num = -1;
    if (this.ParticleChunks.Count > 0)
    {
      while (++num < this.maxParticles)
      {
        float t = (float) (UnityEngine.Random.Range(0, 100) / 100);
        Particle_Chunk.AddNew(this.transform.position, Utils.GetAngle((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null ? this.transform.position : PlayerFarming.Instance.gameObject.transform.position, this.transform.position) + (float) UnityEngine.Random.Range(-20, 20), Color.Lerp(this.minColor, this.maxColor, t), this.ParticleChunks);
      }
    }
    if (this.Activated)
      this.EndPicking();
    if (dropLoot)
      this.StructureBrain.DropBerries(this.transform.position, (UnityEngine.Object) this.GetComponentInParent<CropController>() != (UnityEngine.Object) null, InventoryItem.GetSeedType(this.BerryBushType));
    CameraManager.shakeCamera(1.5f, Utils.GetAngle((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null ? this.transform.position : PlayerFarming.Instance.gameObject.transform.position, this.transform.position));
    if (this.callBackOnHarvest != null)
      this.callBackOnHarvest.Invoke();
    if ((UnityEngine.Object) this.Structure != (UnityEngine.Object) null && this.StructureInfo != null && !this.StructureInfo.CanRegrow)
    {
      this.StructureBrain.Remove();
    }
    else
    {
      if (this.DisableOnDie)
      {
        this.gameObject.SetActive(false);
      }
      else
      {
        this.berryBush_Normal.SetActive(false);
        this.berryBush_Cut.SetActive(true);
        this.Interactable = false;
      }
      if (!((UnityEngine.Object) this._uiProgressIndicator != (UnityEngine.Object) null))
        return;
      this._uiProgressIndicator.Recycle<UIProgressIndicator>();
      this._uiProgressIndicator = (UIProgressIndicator) null;
    }
  }

  protected override void Update()
  {
    if (!this.Activated || (!InputManager.Gameplay.GetInteractButtonUp() || !SettingsManager.Settings.Accessibility.HoldActions) && (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null) || PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.Meditate))
      return;
    Debug.Log((object) "Up");
    this.buttonDown = false;
  }

  private void EndPicking()
  {
    Debug.Log((object) "END PICKING!");
    if (PlayerFarming.Instance.state.CURRENT_STATE == StateMachine.State.TimedAction)
      PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.Idle;
    this.StopAllCoroutines();
    this.Activated = false;
    this.Interactable = true;
    this.Activating = true;
  }

  private void UpdateBar()
  {
    if (LetterBox.IsPlaying || this.StructureBrain == null || this.StructureBrain.Data == null)
      return;
    float progress = this.StructureBrain.Data.Progress / this.StructureBrain.Data.ProgressTarget;
    if (!((UnityEngine.Object) BiomeConstants.Instance != (UnityEngine.Object) null))
      return;
    if ((UnityEngine.Object) this._uiProgressIndicator == (UnityEngine.Object) null)
    {
      this._uiProgressIndicator = BiomeConstants.Instance.ProgressIndicatorTemplate.Spawn<UIProgressIndicator>(BiomeConstants.Instance.transform, this.transform.position + Vector3.back * 1.5f - BiomeConstants.Instance.transform.position);
      this._uiProgressIndicator.Show(progress);
      this._uiProgressIndicator.OnHidden += (System.Action) (() => this._uiProgressIndicator = (UIProgressIndicator) null);
    }
    else
      this._uiProgressIndicator.SetProgress(progress, 0.1f);
  }

  private void OnRemovalProgressChanged() => this.UpdateBar();

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (this.inTrigger)
      return;
    this.inTrigger = true;
    this.StartCoroutine((IEnumerator) this.ShakeObjectTimer());
    this.transform.DOShakeScale(0.5f, new Vector3(-0.1f, 0.05f, 0.01f), randomness: 1f);
    AudioManager.Instance.PlayOneShot("event:/material/footstep_bush", collision.transform.position);
  }

  private IEnumerator ShakeObjectTimer()
  {
    yield return (object) new WaitForSeconds(1f);
    this.inTrigger = false;
  }

  private void OnTriggerExit2D(Collider2D collision)
  {
    if (this.inTrigger)
      return;
    this.inTrigger = true;
    if (this.gameObject.activeSelf)
      this.StartCoroutine((IEnumerator) this.ShakeObjectTimer());
    this.transform.DOShakeScale(0.5f, new Vector3(-0.1f, 0.05f, 0.01f), randomness: 1f);
    AudioManager.Instance.PlayOneShot("event:/material/footstep_bush", collision.transform.position);
  }
}
