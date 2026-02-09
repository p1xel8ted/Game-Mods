// Decompiled with JetBrains decompiler
// Type: Grave
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Grave : Interaction
{
  public static List<Grave> Graves = new List<Grave>();
  public Structure Structure;
  public Structures_Grave _StructureInfo;
  public GameObject EmptyGraveGameObject;
  public GameObject FullGraveGameObject;
  public GameObject Flowers;
  public SpriteXPBar XPBar;
  public string sString;
  public bool PlayerGotBody;
  public bool Activating;
  public float DistanceToTriggerDeposits = 3f;
  public float Delay;

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public Structures_Grave structureBrain
  {
    set => this._StructureInfo = value;
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_Grave;
      return this._StructureInfo;
    }
  }

  public override void OnEnableInteraction()
  {
    this.XPBar.gameObject.SetActive(false);
    Grave.Graves.Add(this);
    this.SetGameObjects();
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    StructureManager.OnStructureRemoved += new StructureManager.StructureChanged(this.OnStructureRemoved);
    TimeManager.OnNewPhaseStarted += new System.Action(this.UpdateStructure);
    this.HasThirdInteraction = true;
    this.ThirdInteractable = true;
    if (this.StructureInfo == null || FollowerManager.GetDeadFollowerInfoByID(this.StructureInfo.FollowerID) == null || !FollowerManager.GetDeadFollowerInfoByID(this.StructureInfo.FollowerID).HadFuneral)
      return;
    if (this.structureBrain != null && this.structureBrain.Data != null && (double) this.structureBrain.Data.LastPrayTime == -1.0)
    {
      this.structureBrain.SoulCount = this.structureBrain.SoulMax;
      this.structureBrain.Data.LastPrayTime = TimeManager.TotalElapsedGameTime + this.structureBrain.TimeBetweenSouls;
    }
    this.UpdateBar();
  }

  public void UpdateBar()
  {
    if ((UnityEngine.Object) this.XPBar == (UnityEngine.Object) null || this.structureBrain == null)
      return;
    this.XPBar.gameObject.SetActive(true);
    this.XPBar.UpdateBar(Mathf.Clamp((float) this.structureBrain.SoulCount / (float) this.structureBrain.SoulMax, 0.0f, 1f));
  }

  public void OnBrainAssigned()
  {
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    if (this.Structure.Type == StructureBrain.TYPES.GRAVE2)
      this.structureBrain.UpgradedGrave = true;
    if (this.StructureInfo.FollowerID != -1 && FollowerManager.GetDeadFollowerInfoByID(this.StructureInfo.FollowerID) == null)
      this.StructureInfo.FollowerID = -1;
    if (FollowerManager.GetDeadFollowerInfoByID(this.StructureInfo.FollowerID) != null && FollowerManager.GetDeadFollowerInfoByID(this.StructureInfo.FollowerID).HadFuneral)
    {
      Structures_Grave structureBrain = this.structureBrain;
      structureBrain.OnSoulsGained = structureBrain.OnSoulsGained + new System.Action<int>(this.OnSoulsGained);
      if ((double) this.structureBrain.Data.LastPrayTime == -1.0)
      {
        this.structureBrain.SoulCount = this.structureBrain.SoulMax;
        this.structureBrain.Data.LastPrayTime = TimeManager.TotalElapsedGameTime + this.structureBrain.TimeBetweenSouls;
      }
      this.UpdateBar();
      this.UpdateStructure();
    }
    this.SetGameObjects();
  }

  public void OnSoulsGained(int count) => this.UpdateBar();

  public void OnStructureRemoved(StructuresData structure)
  {
    StructureManager.OnStructureRemoved -= new StructureManager.StructureChanged(this.OnStructureRemoved);
  }

  public override void OnDisableInteraction()
  {
    Grave.Graves.Remove(this);
    StructureManager.OnStructureRemoved -= new StructureManager.StructureChanged(this.OnStructureRemoved);
    TimeManager.OnNewPhaseStarted -= new System.Action(this.UpdateStructure);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if (this.structureBrain == null)
      return;
    Structures_Grave structureBrain = this.structureBrain;
    structureBrain.OnSoulsGained = structureBrain.OnSoulsGained - new System.Action<int>(this.OnSoulsGained);
  }

  public void Start()
  {
    this.SetGameObjects();
    this.UpdateLocalisation();
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sString = ScriptLocalization.Interactions.HereLies;
  }

  public void SetGameObjects()
  {
    if (this.StructureInfo == null)
      return;
    if (this.StructureInfo.FollowerID == -1 || FollowerManager.GetDeadFollowerInfoByID(this.StructureInfo.FollowerID) == null)
    {
      this.EmptyGraveGameObject.SetActive(true);
      this.FullGraveGameObject.SetActive(false);
      this.XPBar.gameObject.SetActive(false);
    }
    else
    {
      this.EmptyGraveGameObject.SetActive(false);
      this.FullGraveGameObject.SetActive(true);
      if (FollowerManager.GetDeadFollowerInfoByID(this.StructureInfo.FollowerID) != null && FollowerManager.GetDeadFollowerInfoByID(this.StructureInfo.FollowerID).HadFuneral)
      {
        if (!this.Flowers.activeSelf)
          this.Flowers.transform.DOPunchScale(Vector3.one * 0.2f, 0.25f);
        this.Flowers.SetActive(true);
      }
      else
        this.Flowers.SetActive(false);
    }
  }

  public override void GetLabel()
  {
    this.Interactable = false;
    this.SecondaryInteractable = false;
    this.HasSecondaryInteraction = false;
    this.HasThirdInteraction = false;
    this.ThirdInteractable = false;
    if (this.Activating)
      this.Label = string.Empty;
    else if (this.StructureInfo.FollowerID == -1)
    {
      if (this.playerFarming.state.CURRENT_STATE == StateMachine.State.Idle_CarryingBody || this.playerFarming.state.CURRENT_STATE == StateMachine.State.Moving_CarryingBody)
      {
        this.Label = ScriptLocalization.Interactions.BuryBody;
        this.PlayerGotBody = true;
      }
      else
      {
        this.Label = string.Empty;
        this.SecondaryLabel = string.Empty;
        this.ThirdLabel = string.Empty;
        this.PlayerGotBody = false;
      }
    }
    else
    {
      FollowerInfo followerInfoById = FollowerManager.GetDeadFollowerInfoByID(this.StructureInfo.FollowerID);
      if (followerInfoById == null)
        return;
      this.Label = "";
      this.ThirdLabel = LocalizationManager.GetTranslation("Interactions/DigGrave");
      this.HasThirdInteraction = true;
      this.ThirdInteractable = true;
      if (!followerInfoById.HadFuneral)
        return;
      this.XPBar.gameObject.SetActive(true);
      string str = (GameManager.HasUnlockAvailable() ? 1 : (DataManager.Instance.DeathCatBeaten ? 1 : 0)) != 0 ? "<sprite name=\"icon_spirits\">" : "<sprite name=\"icon_blackgold\">";
      string receiveDevotion = ScriptLocalization.Interactions.ReceiveDevotion;
      if (LocalizeIntegration.IsArabic())
        this.SecondaryLabel = $"{receiveDevotion} {str} {LocalizeIntegration.ReverseText(this.structureBrain.SoulMax.ToString())} / {LocalizeIntegration.ReverseText(this._StructureInfo.SoulCount.ToString())}{StaticColors.GreyColorHex}";
      else
        this.SecondaryLabel = $"{receiveDevotion} {str} {this._StructureInfo.SoulCount.ToString()}{StaticColors.GreyColorHex} / {this.structureBrain.SoulMax.ToString()}";
      this.SecondaryInteractable = true;
      this.HasSecondaryInteraction = true;
    }
  }

  public override void OnBecomeCurrent(PlayerFarming playerFarming)
  {
    base.OnBecomeCurrent(playerFarming);
    FollowerInfo followerInfoById = FollowerManager.GetDeadFollowerInfoByID(this.StructureInfo.FollowerID);
    if (followerInfoById == null)
      playerFarming.indicator.ShowTopInfo(string.Empty);
    else
      playerFarming.indicator.ShowTopInfo(this.HereLiesText(followerInfoById));
  }

  public string HereLiesText(FollowerInfo f)
  {
    return $"<sprite name=\"img_SwirleyLeft\"> {this.sString} <color=yellow>{f.Name}</color> <sprite name=\"img_SwirleyRight\">";
  }

  public override void OnBecomeNotCurrent(PlayerFarming playerFarming)
  {
    base.OnBecomeNotCurrent(playerFarming);
    playerFarming.indicator.HideTopInfo();
  }

  public override void OnThirdInteract(StateMachine state)
  {
    base.OnThirdInteract(state);
    if (this.Activating)
      return;
    this.StartCoroutine((IEnumerator) this.DigUpBodyIE());
  }

  public IEnumerator DigUpBodyIE()
  {
    Grave grave = this;
    grave.structureBrain.SoulCount = 0;
    grave.Activating = true;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(grave.playerFarming.gameObject, 6f);
    grave.playerFarming.GoToAndStop(grave.transform.position);
    yield return (object) new WaitForSeconds(0.5f);
    grave.playerFarming.EndGoToAndStop();
    grave.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForEndOfFrame();
    grave.playerFarming.Spine.AnimationState.SetAnimation(0, "actions/dig", false);
    grave.playerFarming.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    yield return (object) new WaitForSeconds(0.5835f);
    CameraManager.shakeCamera(1f, Utils.GetAngle(grave.playerFarming.gameObject.transform.position, grave.transform.position));
    grave.SpawnDeadBody();
    CultFaithManager.AddThought(TimeManager.CurrentPhase == DayPhase.Night ? Thought.Cult_DigUpBody_Night : Thought.Cult_DigUpBody_Day);
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().OnConversationEnd();
    grave.Activating = false;
  }

  public void SpawnDeadBody()
  {
    BiomeConstants.Instance.EmitSmokeInteractionVFX(this.transform.position, Vector3.one);
    StructuresData infoByType = StructuresData.GetInfoByType(StructureBrain.TYPES.DEAD_WORSHIPPER, 0);
    infoByType.BodyWrapped = true;
    infoByType.FollowerID = this.StructureInfo.FollowerID;
    StructureManager.BuildStructure(FollowerLocation.Base, infoByType, this.transform.position, Vector2Int.one, false, (System.Action<GameObject>) (g =>
    {
      DeadWorshipper component = g.GetComponent<DeadWorshipper>();
      component.WrapBody();
      Collider2D collider2D = Physics2D.OverlapCircle((Vector2) this.transform.position, 1f, LayerMask.GetMask("Island"));
      if ((bool) (UnityEngine.Object) collider2D)
        component.BounceOutFromPosition(UnityEngine.Random.Range(3f, 5f), (collider2D.transform.position - this.transform.position).normalized);
      else
        component.BounceOutFromPosition(UnityEngine.Random.Range(3f, 5f));
      PlacementRegion.TileGridTile tileAtWorldPosition = PlacementRegion.Instance.GetFreeClosestTileGridTileAtWorldPosition(component.transform.position);
      if (tileAtWorldPosition == null || !tileAtWorldPosition.CanPlaceStructure)
        return;
      component.Structure.Brain.AddToGrid(tileAtWorldPosition.Position);
    }));
    this.StructureInfo.FollowerID = -1;
    this.SetGameObjects();
  }

  public override void OnSecondaryInteract(StateMachine state)
  {
    base.OnSecondaryInteract(state);
    if (this.structureBrain.SoulCount > 0)
    {
      if (this.Activating)
        return;
      this.StartCoroutine((IEnumerator) this.GiveReward());
    }
    else
      this.playerFarming.indicator.PlayShake();
  }

  public IEnumerator GiveReward()
  {
    Grave grave = this;
    Debug.Log((object) ("_StructureInfo.SoulCount: " + grave._StructureInfo.SoulCount.ToString().Colour(Color.yellow)));
    grave.Activating = true;
    for (int i = 0; i < grave._StructureInfo.SoulCount; ++i)
    {
      if ((GameManager.HasUnlockAvailable() ? 1 : (DataManager.Instance.DeathCatBeaten ? 1 : 0)) != 0)
        SoulCustomTarget.Create(grave.playerFarming.gameObject, grave.transform.position, Color.white, new System.Action(grave.\u003CGiveReward\u003Eb__32_0));
      else
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, grave.transform.position + Vector3.back, 0.0f).SetInitialSpeedAndDiraction(8f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
      float num = Mathf.Clamp((float) (grave._StructureInfo.SoulCount - i) / (float) grave.structureBrain.SoulMax, 0.0f, 1f);
      grave.XPBar.UpdateBar(num);
      yield return (object) new WaitForSeconds(0.1f);
    }
    grave._StructureInfo.SoulCount = 0;
    grave.XPBar.UpdateBar(0.0f);
    grave.Activating = false;
    grave.HasChanged = true;
  }

  public void UpdateStructure()
  {
    if (this.structureBrain == null || (double) this.structureBrain.Data.LastPrayTime == -1.0 || (double) TimeManager.TotalElapsedGameTime <= (double) this.structureBrain.Data.LastPrayTime || this.structureBrain.SoulCount >= this.structureBrain.SoulMax)
      return;
    this.HasChanged = true;
    this.structureBrain.SoulCount = Mathf.Clamp(this.structureBrain.SoulCount + (1 + Mathf.FloorToInt((TimeManager.TotalElapsedGameTime - this.structureBrain.Data.LastPrayTime) / this.structureBrain.TimeBetweenSouls)), 0, this.structureBrain.SoulMax);
    this.structureBrain.Data.LastPrayTime = TimeManager.TotalElapsedGameTime + this.structureBrain.TimeBetweenSouls;
  }

  public void SpawnZombie(FollowerInfo deadBody)
  {
    if (deadBody == null)
      return;
    this.StartCoroutine((IEnumerator) this.SpawnZombieIE(deadBody));
  }

  public IEnumerator SpawnZombieIE(FollowerInfo f)
  {
    yield return (object) new WaitForSeconds(UnityEngine.Random.Range(5f, 10f));
    FollowerBrain resurrectingFollower = FollowerBrain.GetOrCreateBrain(f);
    resurrectingFollower.ResetStats();
    resurrectingFollower.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    resurrectingFollower.Location = FollowerLocation.Base;
    resurrectingFollower.DesiredLocation = FollowerLocation.Base;
    resurrectingFollower.LastPosition = this.StructureInfo.Position;
    resurrectingFollower.CurrentTask.Arrive();
    Follower revivedFollower = FollowerManager.CreateNewFollower(resurrectingFollower._directInfoAccess, this.StructureInfo.Position);
    revivedFollower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    revivedFollower.Spine.gameObject.SetActive(false);
    yield return (object) new WaitForSeconds(1f);
    revivedFollower.Spine.gameObject.SetActive(true);
    revivedFollower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    double num = (double) revivedFollower.SetBodyAnimation("Sermons/sermon-heal", false);
    revivedFollower.AddBodyAnimation("Insane/be-weird", false, 0.0f);
    revivedFollower.AddBodyAnimation("Insane/idle-insane", true, 0.0f);
    this.StructureInfo.FollowerID = -1;
    this.SetGameObjects();
    yield return (object) new WaitForSeconds(4.5f);
    revivedFollower.Brain.ApplyCurseState(Thought.Zombie);
    resurrectingFollower.HardSwapToTask((FollowerTask) new FollowerTask_Zombie());
    yield return (object) new WaitForSeconds(2f);
  }

  [CompilerGenerated]
  public void \u003CSpawnDeadBody\u003Eb__30_0(GameObject g)
  {
    DeadWorshipper component = g.GetComponent<DeadWorshipper>();
    component.WrapBody();
    Collider2D collider2D = Physics2D.OverlapCircle((Vector2) this.transform.position, 1f, LayerMask.GetMask("Island"));
    if ((bool) (UnityEngine.Object) collider2D)
      component.BounceOutFromPosition(UnityEngine.Random.Range(3f, 5f), (collider2D.transform.position - this.transform.position).normalized);
    else
      component.BounceOutFromPosition(UnityEngine.Random.Range(3f, 5f));
    PlacementRegion.TileGridTile tileAtWorldPosition = PlacementRegion.Instance.GetFreeClosestTileGridTileAtWorldPosition(component.transform.position);
    if (tileAtWorldPosition == null || !tileAtWorldPosition.CanPlaceStructure)
      return;
    component.Structure.Brain.AddToGrid(tileAtWorldPosition.Position);
  }

  [CompilerGenerated]
  public void \u003CGiveReward\u003Eb__32_0() => this.playerFarming.GetSoul(1);
}
