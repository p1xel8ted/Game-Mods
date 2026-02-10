// Decompiled with JetBrains decompiler
// Type: Interaction_RanchHutch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using FMOD.Studio;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.RanchSelect;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_RanchHutch : Interaction
{
  public static List<Interaction_RanchHutch> Hutches = new List<Interaction_RanchHutch>();
  public Structures_RanchHutch _StructureInfo;
  public Structure Structure;
  [SerializeField]
  public GameObject occupied;
  [SerializeField]
  public GameObject occupiedEmitter;
  [SerializeField]
  public GameObject babyPosition;
  [SerializeField]
  public GameObject babyReady;
  public Interaction_Ranchable baby;
  public PauseEventHandler pauseHandler;
  public bool babySpawned;
  public EventInstance matingLoopSFX;
  public bool playingLoop;

  public Structures_RanchHutch Brain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_RanchHutch;
      return this._StructureInfo;
    }
    set => this._StructureInfo = value;
  }

  public PauseEventHandler PauseHandler
  {
    get => this.pauseHandler ?? (this.pauseHandler = this.GetComponent<PauseEventHandler>());
  }

  public void Start()
  {
    this.babyReady.gameObject.SetActive(false);
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    Interaction_RanchHutch.Hutches.Add(this);
    this.occupied.SetActive(false);
    this.occupiedEmitter.SetActive(false);
    if (!((UnityEngine.Object) this.PauseHandler != (UnityEngine.Object) null))
      return;
    this.PauseHandler.OnPause += new System.Action(this.OnPause);
    this.PauseHandler.OnUnPause += new System.Action(this.OnUnPause);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    Interaction_RanchHutch.Hutches.Remove(this);
    if ((UnityEngine.Object) this.Structure != (UnityEngine.Object) null)
      this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    AudioManager.Instance.StopLoop(this.matingLoopSFX);
  }

  public void OnBrainAssigned()
  {
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
  }

  public override void GetLabel()
  {
    base.GetLabel();
    this.Label = "";
    this.GetRanch();
    if (Interaction_WolfBase.WolvesActive)
    {
      this.Interactable = false;
    }
    else
    {
      this.Interactable = true;
      this.Label = $"{ScriptLocalization.FollowerInteractions.InteractAnimal} {InventoryItem.CapacityString(InventoryItem.ITEM_TYPE.PLEASURE_POINT, 1)}";
    }
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (!((UnityEngine.Object) this.GetRanch() != (UnityEngine.Object) null))
      return;
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.PLEASURE_POINT) < 1)
      this.playerFarming.indicator.PlayShake();
    else
      this.ShowMatingMenu();
  }

  public void ShowMatingMenu()
  {
    List<RanchSelectEntry> animals = new List<RanchSelectEntry>();
    Interaction_Ranch ranch = this.GetRanch();
    foreach (StructuresData.Ranchable_Animal animal in ranch.Brain.Data.Animals)
    {
      if (animal.State != Interaction_Ranchable.State.Dead)
      {
        RanchSelectEntry ranchSelectEntry = new RanchSelectEntry(animal, this.GetAnimalAvailablilityStatus(animal, ranch));
        animals.Add(ranchSelectEntry);
      }
    }
    MonoSingleton<UIManager>.Instance.ShowRanchMatingtMenu(animals, ranch.Brain.Capacity, this).OnAnimalChosen += (System.Action<StructuresData.Ranchable_Animal, StructuresData.Ranchable_Animal>) ((a, b) => this.StartCoroutine((IEnumerator) this.StartMating(a, b)));
  }

  public RanchSelectEntry.Status GetAnimalAvailablilityStatus(
    StructuresData.Ranchable_Animal animal,
    Interaction_Ranch ranch)
  {
    if (ranch.IsOvercrowded)
      return RanchSelectEntry.Status.UnavailableOvercrowded;
    if (animal.Age < 2)
      return RanchSelectEntry.Status.UnavailableTooYoung;
    if ((double) animal.Satiation <= 25.0)
      return RanchSelectEntry.Status.UnavailableHungry;
    if (animal.Ailment == Interaction_Ranchable.Ailment.Feral)
      return RanchSelectEntry.Status.UnavailableFeral;
    if (animal.Ailment == Interaction_Ranchable.Ailment.Stinky)
      return RanchSelectEntry.Status.UnavailableStinky;
    return animal.State != Interaction_Ranchable.State.Default && animal.State != Interaction_Ranchable.State.Sleeping ? RanchSelectEntry.Status.Unavailable : RanchSelectEntry.Status.Available;
  }

  public IEnumerator StartMating(
    StructuresData.Ranchable_Animal parent1,
    StructuresData.Ranchable_Animal parent2)
  {
    Interaction_RanchHutch interactionRanchHutch = this;
    interactionRanchHutch.playerFarming.GoToAndStop(interactionRanchHutch.transform.position + Vector3.right, interactionRanchHutch.gameObject);
    GameManager.GetInstance().OnConversationNew(true, true, interactionRanchHutch.playerFarming);
    GameManager.GetInstance().OnConversationNext(interactionRanchHutch.gameObject, 5f);
    Inventory.ChangeItemQuantity(InventoryItem.ITEM_TYPE.PLEASURE_POINT, -1);
    Interaction_Ranchable animal1 = Interaction_Ranchable.GetAnimal(parent1);
    Interaction_Ranchable animal2 = Interaction_Ranchable.GetAnimal(parent2);
    Vector3 vector3_1;
    if ((UnityEngine.Object) animal1 != (UnityEngine.Object) null)
    {
      Transform transform = animal1.transform;
      Vector3 position = interactionRanchHutch.transform.position;
      vector3_1 = Vector3.down + Vector3.left;
      Vector3 normalized = vector3_1.normalized;
      Vector3 vector3_2 = position + normalized;
      transform.position = vector3_2;
      parent1.State = Interaction_Ranchable.State.EnteringHutch;
      animal1.TargetHutch(interactionRanchHutch.Brain);
      animal1.OnEnterHutch += new System.Action<StructuresData.Ranchable_Animal, Structures_RanchHutch>(interactionRanchHutch.OnRanchableEnterHutch);
      animal1.OnEnteredHutch += new System.Action<StructuresData.Ranchable_Animal, Structures_RanchHutch>(interactionRanchHutch.OnRanchableEnteredHutch);
    }
    if ((UnityEngine.Object) animal2 != (UnityEngine.Object) null)
    {
      Transform transform = animal2.transform;
      Vector3 position = interactionRanchHutch.transform.position;
      vector3_1 = Vector3.down + Vector3.right;
      Vector3 normalized = vector3_1.normalized;
      Vector3 vector3_3 = position + normalized;
      transform.position = vector3_3;
      parent2.State = Interaction_Ranchable.State.EnteringHutch;
      animal2.TargetHutch(interactionRanchHutch.Brain);
      animal2.OnEnterHutch += new System.Action<StructuresData.Ranchable_Animal, Structures_RanchHutch>(interactionRanchHutch.OnRanchableEnterHutch);
      animal2.OnEnteredHutch += new System.Action<StructuresData.Ranchable_Animal, Structures_RanchHutch>(interactionRanchHutch.OnRanchableEnteredHutch);
    }
    yield return (object) new WaitForSeconds(2.5f);
    interactionRanchHutch.occupied.SetActive(true);
    interactionRanchHutch.occupiedEmitter.SetActive(true);
    interactionRanchHutch.matingLoopSFX = AudioManager.Instance.CreateLoop("event:/dlc/animal/shared/mating_loop", interactionRanchHutch.gameObject, true);
    yield return (object) new WaitForSeconds(2f);
    Interaction_Ranch ranch = (Interaction_Ranch) null;
    foreach (Interaction_Ranch ranch1 in Interaction_Ranch.Ranches)
    {
      if (ranch1.Brain != null)
      {
        foreach (PlacementRegion.TileGridTile ranchingTile in ranch1.Brain.RanchingTiles)
        {
          if (ranchingTile.ObjectID == interactionRanchHutch.Brain.Data.ID)
          {
            ranch = ranch1;
            break;
          }
        }
      }
    }
    ranch.Brain.EndMating(interactionRanchHutch._StructureInfo);
    AudioManager.Instance.StopLoop(interactionRanchHutch.matingLoopSFX);
    interactionRanchHutch.occupied.SetActive(false);
    interactionRanchHutch.occupiedEmitter.SetActive(false);
    BiomeConstants.Instance.EmitHeartPickUpVFX(interactionRanchHutch.transform.position, 0.0f, "red", "burst_big");
    CameraManager.instance.ShakeCameraForDuration(0.5f, 0.75f, 0.3f);
    GameManager.GetInstance().OnConversationNext(interactionRanchHutch.gameObject, 3f);
    yield return (object) new WaitForSeconds(1f);
    yield return (object) interactionRanchHutch.StartCoroutine((IEnumerator) interactionRanchHutch.BabyBornIE(ranch));
  }

  public void OnRanchableEnterHutch(
    StructuresData.Ranchable_Animal animal,
    Structures_RanchHutch hutch)
  {
    Interaction_Ranchable animal1 = Interaction_Ranch.GetAnimal(animal);
    if (!((UnityEngine.Object) animal1 != (UnityEngine.Object) null))
      return;
    animal1.OnEnterHutch -= new System.Action<StructuresData.Ranchable_Animal, Structures_RanchHutch>(this.OnRanchableEnterHutch);
    this.transform.DOKill();
    this.transform.DOPunchScale(Vector3.one * 0.2f, 0.5f).SetDelay<Tweener>(0.5f);
  }

  public void OnRanchableEnteredHutch(
    StructuresData.Ranchable_Animal animal,
    Structures_RanchHutch hutch)
  {
    animal.State = Interaction_Ranchable.State.InsideHutch;
    AnimalData.AddAnimal(animal, hutch.Data.Animals);
    Interaction_Ranchable animal1 = Interaction_Ranch.GetAnimal(animal);
    if ((UnityEngine.Object) animal1 != (UnityEngine.Object) null)
      animal1.OnEnteredHutch -= new System.Action<StructuresData.Ranchable_Animal, Structures_RanchHutch>(this.OnRanchableEnteredHutch);
    if (hutch.Data.Animals.Count < 2)
      return;
    hutch.Data.TargetPhase = TimeManager.CurrentPhase >= DayPhase.Night ? DayPhase.Dawn : TimeManager.CurrentPhase + 1;
  }

  public Interaction_Ranch GetRanch()
  {
    foreach (Interaction_Ranch ranch in Interaction_Ranch.Ranches)
    {
      foreach (PlacementRegion.TileGridTile ranchingTile in ranch.Brain.RanchingTiles)
      {
        if (ranchingTile.ObjectID == this.Brain.Data.ID)
          return ranch;
      }
    }
    return (Interaction_Ranch) null;
  }

  public override void OnDisable()
  {
    base.OnDisable();
    AudioManager.Instance.StopLoop(this.matingLoopSFX);
  }

  public IEnumerator BabyBornIE(Interaction_Ranch ranch)
  {
    Interaction_RanchHutch interactionRanchHutch = this;
    interactionRanchHutch._playerFarming.GoToAndStop(interactionRanchHutch.babyPosition.transform.position + Vector3.right, interactionRanchHutch.gameObject);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionRanchHutch.babyPosition, 3f);
    yield return (object) new WaitForSeconds(1f);
    Interaction_Ranchable r = (Interaction_Ranchable) null;
    UnityEngine.Object.Destroy((UnityEngine.Object) interactionRanchHutch.baby.gameObject);
    Interaction_Ranch.RemoveSpawnedAnimalID(interactionRanchHutch.baby.Animal.ID);
    Interaction_Ranchable.Ranchables.Remove(interactionRanchHutch.baby);
    interactionRanchHutch.Brain.Data.Animals[0].State = Interaction_Ranchable.State.Animating;
    interactionRanchHutch.Brain.Data.Animals[0].TimeSincePoop = TimeManager.TotalElapsedGameTime + 1920f + UnityEngine.Random.Range(-120f, 120f);
    interactionRanchHutch.Brain.Data.Animals[0].TimeSinceLastWash = TimeManager.TotalElapsedGameTime + 4800f + UnityEngine.Random.Range(-120f, 120f);
    AudioManager.Instance.PlayOneShot("event:/dlc/animal/shared/baby_emerge", interactionRanchHutch.transform.position);
    BiomeBaseManager.Instance.SpawnAnimal(interactionRanchHutch.Brain.Data.Animals[0], interactionRanchHutch.babyPosition.transform.position + Vector3.down * 0.1f, false, ranch, BaseLocationManager.Instance.StructureLayer, (System.Action<Interaction_Ranchable>) (result =>
    {
      result.PlayEmergeVO();
      r = result;
    }));
    AnimalData.RemoveAnimal(interactionRanchHutch.Brain.Data.Animals[0], ranch.Brain.Data.Animals);
    AnimalData.RemoveAnimal(interactionRanchHutch.Brain.Data.Animals[0], interactionRanchHutch.Brain.Data.Animals);
    while ((UnityEngine.Object) r == (UnityEngine.Object) null)
      yield return (object) null;
    int num1 = ranch.IsOvercrowded ? 1 : 0;
    AnimalData.AddAnimal(r.Animal, ranch.Brain.Data.Animals);
    int num2 = ranch.IsOvercrowded ? 1 : 0;
    if (num1 != num2)
      r.NotifyOvercrowded(ranch);
    r.ClearCurrentPath();
    r.CurrentState = Interaction_Ranchable.State.Animating;
    interactionRanchHutch.babyReady.gameObject.SetActive(false);
    AudioManager.Instance.PlayOneShot("event:/followers/pop_in", r.transform.position);
    r.Spine.AnimationState.SetAnimation(0, "appear", false);
    r.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    r.transform.DOMove(r.transform.position + Vector3.down / 3f, 0.33f);
    yield return (object) new WaitForSeconds(0.25f);
    AudioManager.Instance.PlayOneShot("event:/Stings/generic_positive", r.transform.position);
    yield return (object) new WaitForSeconds(0.75f);
    AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", r.transform.position);
    BiomeConstants.Instance.EmitHeartPickUpVFX(r.transform.position, 0.0f, "red", "burst_small");
    yield return (object) new WaitForSeconds(2f);
    r.CurrentState = Interaction_Ranchable.State.Default;
    GameManager.GetInstance().OnConversationEnd();
    interactionRanchHutch.babySpawned = false;
  }

  public void SpawnBaby()
  {
    this.babyReady.gameObject.SetActive(true);
    BiomeConstants.Instance.EmitHeartPickUpVFX(this.transform.position, 0.0f, "red", "burst_small");
    foreach (Interaction_Ranch ranch in Interaction_Ranch.Ranches)
    {
      if (ranch.Brain != null)
      {
        foreach (PlacementRegion.TileGridTile ranchingTile in ranch.Brain.RanchingTiles)
        {
          if (ranchingTile.ObjectID == this.Brain.Data.ID)
          {
            this.babySpawned = true;
            this.Brain.Data.Animals[0].State = Interaction_Ranchable.State.BabyInHutch;
            BiomeBaseManager.Instance.SpawnAnimal(this.Brain.Data.Animals[0], this.babyPosition.transform.position, false, ranch, BaseLocationManager.Instance.StructureLayer, (System.Action<Interaction_Ranchable>) (result =>
            {
              this.baby = result;
              this.baby.transform.parent = this.babyPosition.transform;
              this.baby.CurrentState = Interaction_Ranchable.State.BabyInHutch;
              this.baby.Animal.FavouriteFood = Interaction_Ranchable.DrawFavouriteFood();
            }));
          }
        }
      }
    }
  }

  public void OnPause()
  {
    ParticleSystem component;
    if (!((UnityEngine.Object) this.occupiedEmitter != (UnityEngine.Object) null) || !this.occupiedEmitter.TryGetComponent<ParticleSystem>(out component))
      return;
    component.Pause();
  }

  public void OnUnPause()
  {
    ParticleSystem component;
    if (!((UnityEngine.Object) this.occupiedEmitter != (UnityEngine.Object) null) || !this.occupiedEmitter.TryGetComponent<ParticleSystem>(out component))
      return;
    component.Play();
  }

  [CompilerGenerated]
  public void \u003CShowMatingMenu\u003Eb__22_0(
    StructuresData.Ranchable_Animal a,
    StructuresData.Ranchable_Animal b)
  {
    this.StartCoroutine((IEnumerator) this.StartMating(a, b));
  }

  [CompilerGenerated]
  public void \u003CSpawnBaby\u003Eb__30_0(Interaction_Ranchable result)
  {
    this.baby = result;
    this.baby.transform.parent = this.babyPosition.transform;
    this.baby.CurrentState = Interaction_Ranchable.State.BabyInHutch;
    this.baby.Animal.FavouriteFood = Interaction_Ranchable.DrawFavouriteFood();
  }
}
