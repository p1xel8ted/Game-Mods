// Decompiled with JetBrains decompiler
// Type: Interaction_Ranch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.RanchSelect;
using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;

#nullable disable
public class Interaction_Ranch : Interaction
{
  public static List<Interaction_Ranch> Ranches = new List<Interaction_Ranch>();
  public Structures_Ranch _StructureInfo;
  public Structure Structure;
  [SerializeField]
  public GameObject[] connectionPoints;
  [SerializeField]
  public GameObject ritualActiveVFX;
  public List<InventoryItem> toDeposit = new List<InventoryItem>();
  public UIRanchMenuController ranchMenu;
  [CompilerGenerated]
  public bool \u003CIsValid\u003Ek__BackingField;

  public Structures_Ranch Brain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_Ranch;
      return this._StructureInfo;
    }
    set => this._StructureInfo = value;
  }

  public bool IsValid
  {
    get => this.\u003CIsValid\u003Ek__BackingField;
    set => this.\u003CIsValid\u003Ek__BackingField = value;
  }

  public bool IsOvercrowded => this.Brain.IsOvercrowded;

  public bool IsFull => this.Brain.Data.Animals.Count + this.toDeposit.Count >= this.Brain.Capacity;

  public bool IsTooSmall
  {
    get
    {
      return (double) (this.Brain.Data.Animals.Count + this.toDeposit.Count) >= (double) this.Brain.RanchingTiles.Count / 2.0;
    }
  }

  public static event Interaction_Ranch.RanchEvent OnRanchUpdated;

  public void Awake()
  {
    Interaction_Ranch.Ranches.Add(this);
    this.ritualActiveVFX.gameObject.SetActive(false);
  }

  public void Start()
  {
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    if (this.Brain != null)
      this.OnBrainAssigned();
    this.HasSecondaryInteraction = true;
  }

  public override void OnEnable()
  {
    base.OnEnable();
    AstarPath.OnPostScan += new OnScanDelegate(this.UpdateRanchNodesPenalty);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    AstarPath.OnPostScan -= new OnScanDelegate(this.UpdateRanchNodesPenalty);
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    if (FollowerBrainStats.IsRanchHarvest || FollowerBrainStats.IsRanchMeat)
      this.ritualActiveVFX.gameObject.SetActive(true);
    else
      this.ritualActiveVFX.gameObject.SetActive(false);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    Interaction_Ranch.Ranches.Remove(this);
    StructureManager.OnStructureMoved -= new StructureManager.StructureChanged(this.CheckValidEnclosure);
    StructureManager.OnStructureRemoved -= new StructureManager.StructureChanged(this.CheckValidEnclosure);
    StructureManager.OnStructureAddedToGrid -= new StructureManager.StructureChanged(this.CheckValidEnclosure);
    StructureManager.OnStructuresPlaced -= new StructureManager.StructuresPlaced(this.CheckValidEnclosure);
    TimeManager.OnNewPhaseStarted -= new System.Action(this.OnNewPhaseStarted);
    if ((UnityEngine.Object) this.Structure != (UnityEngine.Object) null)
      this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    if (this.Brain == null)
      return;
    this.Brain.OnAnimalAdded -= new Structures_Ranch.AnimalEvent(this.Brain_OnAnimalAdded);
  }

  public void OnBrainAssigned()
  {
    StructureManager.OnStructureMoved += new StructureManager.StructureChanged(this.CheckValidEnclosure);
    StructureManager.OnStructureRemoved += new StructureManager.StructureChanged(this.CheckValidEnclosure);
    StructureManager.OnStructureAddedToGrid += new StructureManager.StructureChanged(this.CheckValidEnclosure);
    StructureManager.OnStructuresPlaced += new StructureManager.StructuresPlaced(this.CheckValidEnclosure);
    TimeManager.OnNewPhaseStarted += new System.Action(this.OnNewPhaseStarted);
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    this.Brain.OnAnimalAdded += new Structures_Ranch.AnimalEvent(this.Brain_OnAnimalAdded);
    this.CheckValidEnclosure((StructuresData) null);
    if (!((UnityEngine.Object) PlacementRegion.Instance != (UnityEngine.Object) null) || PlacementRegion.Instance.structureBrain == null)
      return;
    PlacementRegion.TileGridTile tileAtWorldPosition = PlacementRegion.Instance.GetClosestTileGridTileAtWorldPosition(this.transform.position);
    if (tileAtWorldPosition == null || tileAtWorldPosition.ObjectID == this.Brain.Data.ID)
      return;
    this.Brain.Data.GridTilePosition = tileAtWorldPosition.Position;
    PlacementRegion.Instance.structureBrain.AddStructureToGrid(this.Brain.Data, tileAtWorldPosition.Position);
  }

  public int GetAnimalCount()
  {
    int animalCount = 0;
    foreach (StructuresData.Ranchable_Animal animal in this.Brain.Data.Animals)
    {
      if (animal.State != Interaction_Ranchable.State.Dead)
        ++animalCount;
    }
    return animalCount;
  }

  public override void GetLabel()
  {
    base.GetLabel();
    if (this.IsValid)
      this.Label = LocalizationManager.GetTranslation("Interactions/View");
    else
      this.Label = "";
  }

  public override void GetSecondaryLabel()
  {
    base.GetSecondaryLabel();
    this.SecondaryLabel = LocalizationManager.GetTranslation("Interactions/PlacePosts");
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (!this.IsValid)
      return;
    this.ShowRanchMenu();
  }

  public static void RemoveAnimalFromRanch(RanchSelectEntry animal)
  {
    foreach (Interaction_Ranch ranch in Interaction_Ranch.Ranches)
    {
      foreach (StructuresData.Ranchable_Animal animal1 in ranch.Brain.Data.Animals)
      {
        if (animal1.ID == animal.AnimalInfo.ID)
        {
          AnimalData.RemoveAnimal(animal1, ranch.Brain.Data.Animals);
          break;
        }
      }
    }
  }

  public static void RemoveSpawnedAnimalID(int ID) => BiomeBaseManager.SpawnedAnimalIDs.Remove(ID);

  public override void OnSecondaryInteract(StateMachine state)
  {
    base.OnSecondaryInteract(state);
    this.ShowPlacementRegion();
  }

  public void ShowRanchMenu()
  {
    GameManager.GetInstance().OnConversationNew();
    this.ranchMenu = MonoSingleton<UIManager>.Instance.ShowRanchMenu(this._StructureInfo);
    this.Interactable = false;
    this.ranchMenu.OnAddAnimal += (System.Action<StructuresData.Ranchable_Animal>) (newAnimal =>
    {
      AudioManager.Instance.PlayOneShot("event:/dlc/animal/shared/confirm_placement", this.transform.position);
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.PlaceAnimalInsideRanch);
      Inventory.ChangeItemQuantity(newAnimal.Type, -1);
      ++DataManager.Instance.RanchingAnimalsAdded;
      this.ranchMenu.Hide();
      this.AddAnimal(newAnimal, (System.Action) (() => this.ShowRanchMenu()));
    });
    UIRanchMenuController ranchMenu1 = this.ranchMenu;
    ranchMenu1.OnHidden = ranchMenu1.OnHidden + (System.Action) (() =>
    {
      this.ranchMenu = (UIRanchMenuController) null;
      DOVirtual.Float(0.0f, 1f, 0.1f, (TweenCallback<float>) (value => this.Interactable = (double) value > 0.5)).OnComplete<Tweener>((TweenCallback) (() => this.Interactable = true));
    });
    UIRanchMenuController ranchMenu2 = this.ranchMenu;
    ranchMenu2.OnCancel = ranchMenu2.OnCancel + (System.Action) (() => GameManager.GetInstance().OnConversationEnd());
  }

  public void ShowPlacementRegion()
  {
    PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive);
    PlacementRegion.Instance.PlacementGameObject = TypeAndPlacementObjects.GetByType(StructureBrain.TYPES.RANCH_FENCE).PlacementObject;
    PlacementRegion.Instance.StructureType = StructureBrain.TYPES.RANCH_FENCE;
    PlacementRegion.Instance.Play(this.Brain.Data.ID);
  }

  public void AddAnimal()
  {
    InventoryItem.ITEM_TYPE type = (InventoryItem.ITEM_TYPE) this.toDeposit[0].type;
    this.toDeposit.RemoveAt(0);
    float num = UnityEngine.Random.Range(-0.5f, 0.0f);
    switch (type)
    {
      case InventoryItem.ITEM_TYPE.ANIMAL_SPIDER:
        num = UnityEngine.Random.Range(-0.2f, 0.0f);
        break;
      case InventoryItem.ITEM_TYPE.ANIMAL_SNAIL:
        num = UnityEngine.Random.Range(-0.5f, -0.3f);
        break;
    }
    ++DataManager.Instance.AnimalID;
    StructuresData.Ranchable_Animal animal = new StructuresData.Ranchable_Animal()
    {
      Age = 1,
      ID = DataManager.Instance.AnimalID,
      Type = type,
      Ears = UnityEngine.Random.Range(1, 6),
      Head = UnityEngine.Random.Range(1, 6),
      Horns = UnityEngine.Random.Range(1, 6),
      Colour = UnityEngine.Random.Range(0, 10),
      Speed = num,
      TimeSinceLastWash = TimeManager.TotalElapsedGameTime + 4800f + UnityEngine.Random.Range(-120f, 120f),
      TimeSincePoop = TimeManager.TotalElapsedGameTime + 1920f + UnityEngine.Random.Range(-120f, 120f)
    };
    List<PlacementRegion.TileGridTile> tileGridTileList = new List<PlacementRegion.TileGridTile>((IEnumerable<PlacementRegion.TileGridTile>) this.Brain.RanchingTiles);
    for (int index = 0; index < Interaction_Ranchable.Ranchables.Count; ++index)
    {
      if (Interaction_Ranchable.Ranchables[index].CurrentTile != null && tileGridTileList.Contains(Interaction_Ranchable.Ranchables[index].CurrentTile))
        tileGridTileList.Remove(Interaction_Ranchable.Ranchables[index].CurrentTile);
    }
    Vector3 position = this.transform.position - Vector3.forward;
    if (tileGridTileList.Count > 0)
      position = tileGridTileList[UnityEngine.Random.Range(0, tileGridTileList.Count)].WorldPosition;
    this.Brain.AnimalAdded(animal, position, (System.Action) null);
  }

  public List<Interaction_Ranchable> GetAnimals()
  {
    List<Interaction_Ranchable> animals = new List<Interaction_Ranchable>();
    foreach (StructuresData.Ranchable_Animal animal in this.Brain.Data.Animals)
      animals.Add(Interaction_Ranchable.GetAnimal(animal));
    return animals;
  }

  public void AddAnimal(StructuresData.Ranchable_Animal animal, System.Action callback)
  {
    float num = UnityEngine.Random.Range(-0.5f, 0.0f);
    if (animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_SNAIL)
      num = UnityEngine.Random.Range(-0.5f, -0.3f);
    else if (animal.Type == InventoryItem.ITEM_TYPE.ANIMAL_SPIDER)
      num = UnityEngine.Random.Range(-0.2f, 0.0f);
    ++DataManager.Instance.AnimalID;
    StructuresData.Ranchable_Animal animal1 = new StructuresData.Ranchable_Animal()
    {
      Age = 1,
      ID = DataManager.Instance.AnimalID,
      Type = animal.Type,
      Ears = animal.Ears,
      Head = animal.Head,
      Horns = animal.Horns,
      Colour = animal.Colour,
      Speed = num,
      FavouriteFood = Interaction_Ranchable.DrawFavouriteFood(),
      TimeSinceLastWash = TimeManager.TotalElapsedGameTime + 4800f + UnityEngine.Random.Range(-120f, 120f),
      TimeSincePoop = TimeManager.TotalElapsedGameTime + 1920f + UnityEngine.Random.Range(-120f, 120f)
    };
    List<PlacementRegion.TileGridTile> tileGridTileList = new List<PlacementRegion.TileGridTile>((IEnumerable<PlacementRegion.TileGridTile>) this.Brain.RanchingTiles);
    for (int index = 0; index < Interaction_Ranchable.Ranchables.Count; ++index)
    {
      if (Interaction_Ranchable.Ranchables[index].CurrentTile != null && tileGridTileList.Contains(Interaction_Ranchable.Ranchables[index].CurrentTile))
        tileGridTileList.Remove(Interaction_Ranchable.Ranchables[index].CurrentTile);
    }
    if (tileGridTileList.Count == 0)
      tileGridTileList = new List<PlacementRegion.TileGridTile>((IEnumerable<PlacementRegion.TileGridTile>) this.Brain.RanchingTiles);
    Vector3 position = this.transform.position - Vector3.forward;
    if (tileGridTileList.Count > 0)
      position = tileGridTileList[UnityEngine.Random.Range(0, tileGridTileList.Count)].WorldPosition;
    this.Brain.AnimalAdded(animal1, position, callback);
  }

  public IEnumerator AddAnimalSequenceIE(
    StructuresData.Ranchable_Animal animal,
    Vector3 position,
    System.Action callback = null)
  {
    Interaction_Ranch interactionRanch = this;
    GameManager.GetInstance().OnConversationNew();
    GameObject temp = new GameObject();
    temp.transform.position = interactionRanch.transform.position + new Vector3(0.0f, -0.2f);
    GameManager.GetInstance().OnConversationNext(temp, 4f);
    Interaction_Ranchable ranchable = (Interaction_Ranchable) null;
    BiomeBaseManager.Instance.SpawnAnimal(animal, position, false, interactionRanch, BaseLocationManager.Instance.StructureLayer, (System.Action<Interaction_Ranchable>) (r =>
    {
      ranchable = r;
      r.transform.position = this.transform.position + new Vector3(0.0f, -0.2f);
      r.gameObject.SetActive(false);
    }));
    yield return (object) new WaitForSeconds(1f);
    ranchable.gameObject.SetActive(true);
    ranchable.CurrentState = Interaction_Ranchable.State.Animating;
    ranchable.Spine.AnimationState.SetAnimation(0, "appear", false);
    ranchable.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    yield return (object) null;
    AudioManager.Instance.PlayOneShot("event:/dlc/animal/shared/appear", ranchable.gameObject);
    ranchable.PlayAppearVO();
    yield return (object) null;
    yield return (object) new WaitForSeconds(1f);
    if (interactionRanch.IsOvercrowded)
      ranchable.NotifyOvercrowded(interactionRanch);
    yield return (object) new WaitForSeconds(0.3f);
    ranchable.CurrentState = Interaction_Ranchable.State.Default;
    yield return (object) null;
    ranchable.Spine.AnimationState.SetAnimation(0, "walk", true);
    ranchable.UnitObject.givePath(position, ranchable.gameObject);
    if (interactionRanch.IsOvercrowded)
      yield return (object) new WaitForSeconds(0.7f);
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationEnd();
    UnityEngine.Object.Destroy((UnityEngine.Object) temp);
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void CheckValidEnclosure() => this.CheckValidEnclosure(this.Brain.Data);

  public void CheckValidEnclosure(StructuresData structure)
  {
    if (structure != null && !StructureManager.RanchingStructures.Contains(structure.Type) || this.Structure.Brain == null || (UnityEngine.Object) this.gameObject == (UnityEngine.Object) null || !this.gameObject.activeInHierarchy)
      return;
    if (structure != null)
    {
      GameManager.GetInstance().WaitForSecondsRealtime(0.5f, (System.Action) (() => this.CheckValidEnclosure((StructuresData) null)));
    }
    else
    {
      this.IsValid = this.Brain != null && this.Brain.HasValidEnclosure();
      if (this.Brain != null && this.Structure.Brain != null)
      {
        if (this.IsValid)
        {
          this.HasSecondaryInteraction = true;
          foreach (Interaction_Ranchable ranchable in Interaction_Ranchable.Ranchables)
          {
            foreach (StructuresData.Ranchable_Animal animal in this.Brain.Data.Animals)
            {
              if (ranchable.Animal == animal)
              {
                ranchable.CheckBreakingOut();
                break;
              }
            }
          }
          foreach (PlacementRegion.TileGridTile ranchingTile in this.Brain.GetRanchingTiles())
          {
            if (ranchingTile.ObjectOnTile != StructureBrain.TYPES.NONE && !StructureManager.IndestructibleByRanchStructures.Contains(ranchingTile.ObjectOnTile) && !StructureManager.OnlyPlaceableInRanch.Contains(ranchingTile.ObjectOnTile))
            {
              StructureBrain structureById = StructureManager.GetStructureByID<StructureBrain>(ranchingTile.ObjectID);
              if ((structureById.Data.Type != StructureBrain.TYPES.BUILD_SITE && structureById.Data.Type != StructureBrain.TYPES.BUILDSITE_BUILDINGPROJECT || !StructureManager.IndestructibleByRanchStructures.Contains(structureById.Data.ToBuildType) && !StructureManager.RanchingStructures.Contains(structureById.Data.ToBuildType)) && structureById != null)
              {
                if (PlayerFarming.Location == FollowerLocation.Base)
                {
                  foreach (Structure structure1 in Structure.Structures)
                  {
                    if ((UnityEngine.Object) structure1 != (UnityEngine.Object) null && structure1.Brain != null && structure1.Brain.Data != null && structure1.Brain.Data.ID == structureById.Data.ID)
                    {
                      BiomeConstants.Instance.EmitSmokeExplosionVFX(structure1.transform.position - Vector3.forward);
                      break;
                    }
                  }
                }
                if (structureById.Data.Type == StructureBrain.TYPES.FARM_PLOT)
                {
                  InventoryItem plantedSeed = ((Structures_FarmerPlot) structureById).GetPlantedSeed();
                  if (plantedSeed != null)
                  {
                    if (PlayerFarming.Location == FollowerLocation.Base)
                    {
                      InventoryItem.Spawn((InventoryItem.ITEM_TYPE) plantedSeed.type, 1, structureById.Data.Position);
                    }
                    else
                    {
                      List<Structures_CollectedResourceChest> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_CollectedResourceChest>(in PlayerFarming.Location);
                      if (structuresOfType.Count > 0)
                        structuresOfType[0].DepositItem((InventoryItem.ITEM_TYPE) plantedSeed.type, plantedSeed.quantity);
                    }
                  }
                }
                if (structureById.Data.Type == StructureBrain.TYPES.RANCH_FENCE)
                  Inventory.AddItem(InventoryItem.ITEM_TYPE.LOG_REFINED, 1);
                structureById.Remove();
              }
            }
            else
              PlacementRegion.Instance.MarkObstructionsForClearing(ranchingTile.Position, Vector2Int.one, (StructuresData) null);
          }
        }
        else
          this.Brain.RanchingTiles.Clear();
      }
      this.UpdateAllFences();
      Interaction_Ranch.RanchEvent onRanchUpdated = Interaction_Ranch.OnRanchUpdated;
      if (onRanchUpdated == null)
        return;
      onRanchUpdated();
    }
  }

  public void UpdateAllFences()
  {
    List<RanchFence> ranchFenceList = new List<RanchFence>((IEnumerable<RanchFence>) RanchFence.Fences);
    foreach (Structures_RanchFence structuresRanchFence in StructureManager.GetAllStructuresOfType<Structures_RanchFence>())
    {
      for (int index1 = 0; index1 < ranchFenceList.Count; ++index1)
      {
        if (ranchFenceList[index1].Structure.Brain != null && structuresRanchFence.Data.ID == ranchFenceList[index1].Structure.Brain.Data.ID)
        {
          bool flag = false;
          for (int index2 = 0; index2 < structuresRanchFence.ConnectedRanchIDs.Count; ++index2)
          {
            for (int index3 = 0; index3 < Interaction_Ranch.Ranches.Count; ++index3)
            {
              if (Interaction_Ranch.Ranches[index3].Brain != null && Interaction_Ranch.Ranches[index3].Brain.Data.ID == structuresRanchFence.ConnectedRanchIDs[index2] && Interaction_Ranch.Ranches[index3].IsValid)
              {
                ranchFenceList[index1].ConfigureFence();
                flag = true;
              }
            }
          }
          if (!flag)
            ranchFenceList[index1].DisableFence();
          ranchFenceList.RemoveAt(index1);
          break;
        }
      }
    }
    for (int index = 0; index < ranchFenceList.Count; ++index)
      ranchFenceList[index].DisableFence();
  }

  public void Brain_OnAnimalAdded(
    StructuresData.Ranchable_Animal animal,
    Vector3 position,
    System.Action spawnAnimalCallback)
  {
    if (DataManager.Instance.RanchingAnimalsAdded <= 1)
      this.StartCoroutine((IEnumerator) this.AddAnimalSequenceIE(animal, position, (System.Action) (() =>
      {
        ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/RanchingAnimal", Objectives.CustomQuestTypes.FeedAnimal), true, true);
        ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/RanchingAnimal", Objectives.CustomQuestTypes.WaitForAnimalToGrowUp), true, true);
        ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/RanchingAnimal", Objectives.CustomQuestTypes.SheerAnimal), true, true);
        System.Action action = spawnAnimalCallback;
        if (action == null)
          return;
        action();
      })));
    else
      this.StartCoroutine((IEnumerator) this.AddAnimalSequenceIE(animal, position, spawnAnimalCallback));
  }

  public GameObject GetClosestConnectionPoint(Vector3 position)
  {
    GameObject connectionPoint = this.connectionPoints[0];
    for (int index = 1; index < this.connectionPoints.Length; ++index)
    {
      if ((double) Vector3.Distance(this.connectionPoints[index].transform.position, position) < (double) Vector3.Distance(connectionPoint.transform.position, position))
        connectionPoint = this.connectionPoints[index];
    }
    return connectionPoint;
  }

  public void OnNewPhaseStarted()
  {
    Structures_Ranch brain = this.Brain;
  }

  public static Interaction_Ranchable GetAnimal(StructuresData.Ranchable_Animal animal)
  {
    foreach (Interaction_Ranchable ranchable in Interaction_Ranchable.Ranchables)
    {
      if (ranchable.Animal.ID == animal.ID)
        return ranchable;
    }
    foreach (Interaction_Ranchable deadRanchable in Interaction_Ranchable.DeadRanchables)
    {
      if (deadRanchable.Animal.ID == animal.ID)
        return deadRanchable;
    }
    return (Interaction_Ranchable) null;
  }

  public static Interaction_Ranch GetRanch(int ID)
  {
    foreach (Interaction_Ranch ranch in Interaction_Ranch.Ranches)
    {
      if (ranch.Brain.Data.ID == ID)
        return ranch;
    }
    return (Interaction_Ranch) null;
  }

  public void StarveAnimals()
  {
    foreach (StructuresData.Ranchable_Animal animal in this.Brain.Data.Animals)
      animal.Satiation = 0.0f;
  }

  public void UpdateRanchNodesPenalty(AstarPath script)
  {
    if (!this.IsValid)
      return;
    List<GraphNode> graphNodeList = new List<GraphNode>();
    Bounds bounds = new Bounds();
    bounds.size = Vector3.one * 1.5f;
    foreach (Structures_Ranch.FenceData fence in this.Brain.Fences)
    {
      bounds.center = fence.Tile.WorldPosition;
      graphNodeList.AddRange((IEnumerable<GraphNode>) AstarPath.active.data.gridGraph.GetNodesInRegion(bounds));
    }
    bounds.center = this.transform.position;
    graphNodeList.AddRange((IEnumerable<GraphNode>) AstarPath.active.data.gridGraph.GetNodesInRegion(bounds));
    foreach (GraphNode graphNode in graphNodeList)
      graphNode.Penalty = 10000U;
    graphNodeList.Clear();
  }

  [CompilerGenerated]
  public void \u003CShowRanchMenu\u003Eb__39_0(StructuresData.Ranchable_Animal newAnimal)
  {
    AudioManager.Instance.PlayOneShot("event:/dlc/animal/shared/confirm_placement", this.transform.position);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.PlaceAnimalInsideRanch);
    Inventory.ChangeItemQuantity(newAnimal.Type, -1);
    ++DataManager.Instance.RanchingAnimalsAdded;
    this.ranchMenu.Hide();
    this.AddAnimal(newAnimal, (System.Action) (() => this.ShowRanchMenu()));
  }

  [CompilerGenerated]
  public void \u003CShowRanchMenu\u003Eb__39_1() => this.ShowRanchMenu();

  [CompilerGenerated]
  public void \u003CShowRanchMenu\u003Eb__39_2()
  {
    this.ranchMenu = (UIRanchMenuController) null;
    DOVirtual.Float(0.0f, 1f, 0.1f, (TweenCallback<float>) (value => this.Interactable = (double) value > 0.5)).OnComplete<Tweener>((TweenCallback) (() => this.Interactable = true));
  }

  [CompilerGenerated]
  public void \u003CShowRanchMenu\u003Eb__39_3(float value)
  {
    this.Interactable = (double) value > 0.5;
  }

  [CompilerGenerated]
  public void \u003CShowRanchMenu\u003Eb__39_4() => this.Interactable = true;

  [CompilerGenerated]
  public void \u003CCheckValidEnclosure\u003Eb__46_0()
  {
    this.CheckValidEnclosure((StructuresData) null);
  }

  [Serializable]
  public class Animal
  {
    public InventoryItem.ITEM_TYPE Type;
    public AssetReferenceGameObject Addressable;
  }

  public delegate void RanchEvent();
}
