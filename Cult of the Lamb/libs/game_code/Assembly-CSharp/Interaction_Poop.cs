// Decompiled with JetBrains decompiler
// Type: Interaction_Poop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using FMOD.Studio;
using I2.Loc;
using Lamb.UI;
using Spine;
using Spine.Unity;
using src.UI.Overlays.TutorialOverlay;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_Poop : Interaction
{
  public Structure structure;
  public static List<Interaction_Poop> Poops = new List<Interaction_Poop>();
  public bool spawnItem = true;
  public float Scale;
  public float ScaleSpeed;
  public string sString;
  public bool EventListenerActive;
  public bool playedSfx;
  public SkeletonAnimation skeletonAnimation;
  public bool hasFancyRobes;
  public static bool GivenOutfit = false;

  public StructuresData StructureInfo => this.structure.Structure_Info;

  public StructureBrain StructureBrain => this.structure.Brain;

  public bool Activating
  {
    get => this.StructureBrain != null && this.StructureBrain.ReservedByPlayer;
    set => this.StructureBrain.ReservedByPlayer = value;
  }

  public void Start()
  {
    this.FreezeCoopPlayersOnHoldToInteract = false;
    this.UpdateLocalisation();
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    Interaction_Poop.Poops.Add(this);
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    Interaction_Poop.Poops.Remove(this);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if (this.StructureInfo != null && this.StructureInfo.Destroyed && this.StructureInfo.LootCountToDrop != -1)
    {
      if (this.StructureInfo.Type == StructureBrain.TYPES.TOXIC_WASTE)
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.SOOT, UnityEngine.Random.Range(3, 6), this.transform.position);
      else if (this.spawnItem)
        InventoryItem.Spawn(InventoryItem.GetPoopTypeFromStructure(this.StructureInfo.Type), 1, this.transform.position);
      if (this.StructureInfo.Type == StructureBrain.TYPES.POOP_GOLD)
      {
        for (int index = 0; index < UnityEngine.Random.Range(5, 12); ++index)
          InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, this.transform.position);
      }
      else if (this.StructureInfo.Type == StructureBrain.TYPES.POOP_DEVOTION)
      {
        for (int index = 0; index < UnityEngine.Random.Range(5, 10); ++index)
          SoulCustomTarget.Create(this.playerFarming.gameObject, this.transform.position, Color.white, (System.Action) (() => this.playerFarming?.GetSoul(1)));
      }
      else if (this.StructureInfo.Type == StructureBrain.TYPES.POOP_MASSIVE)
      {
        for (int index = 0; index < UnityEngine.Random.Range(3, 6); ++index)
          InventoryItem.Spawn(InventoryItem.ITEM_TYPE.POOP, 1, this.transform.position);
        AudioManager.Instance.PlayOneShot("event:/followers/big_poop_cleaned", this.transform.position);
      }
    }
    if (this.Activating)
    {
      this.StopAllCoroutines();
      System.Action onCrownReturn = this.playerFarming.OnCrownReturn;
      if (onCrownReturn != null)
        onCrownReturn();
      this.state.CURRENT_STATE = StateMachine.State.Idle;
    }
    if (!((UnityEngine.Object) this.skeletonAnimation != (UnityEngine.Object) null) || this.skeletonAnimation.AnimationState == null)
      return;
    this.skeletonAnimation.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
  }

  public IEnumerator DevotionIE(float duration)
  {
    Interaction_Poop interactionPoop = this;
    int r = UnityEngine.Random.Range(10, 20);
    float timeBetween = duration / (float) r;
    for (int i = 0; i < r; ++i)
    {
      yield return (object) new WaitForSeconds(timeBetween);
      SoulCustomTarget.Create(interactionPoop.playerFarming.gameObject, interactionPoop.transform.position, Color.white, new System.Action(interactionPoop.\u003CDevotionIE\u003Eb__14_0));
    }
  }

  public IEnumerator GoldIE(float duration)
  {
    Interaction_Poop interactionPoop = this;
    int r = UnityEngine.Random.Range(10, 20);
    float timeBetween = duration / (float) r;
    for (int i = 0; i < r; ++i)
    {
      yield return (object) new WaitForSeconds(timeBetween);
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, interactionPoop.transform.position);
    }
  }

  public IEnumerator PoopIE(float duration)
  {
    Interaction_Poop interactionPoop = this;
    int r = UnityEngine.Random.Range(6, 11);
    float timeBetween = duration / (float) r;
    for (int i = 0; i < r; ++i)
    {
      yield return (object) new WaitForSeconds(timeBetween);
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.POOP, 1, interactionPoop.transform.position);
      AudioManager.Instance.PlayOneShot("event:/followers/poop_pop", interactionPoop.transform.position);
    }
  }

  public IEnumerator RevealPoopPetIE(Follower f)
  {
    Interaction_Poop interactionPoop = this;
    interactionPoop.spawnItem = false;
    interactionPoop.playerFarming.GoToAndStop(interactionPoop.transform.position + Vector3.right, interactionPoop.gameObject);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionPoop.gameObject, 5f);
    yield return (object) new WaitForSeconds(0.5f);
    foreach (Renderer componentsInChild in interactionPoop.GetComponentsInChildren<SpriteRenderer>())
      componentsInChild.enabled = false;
    BiomeConstants.Instance.EmitSmokeInteractionVFX(interactionPoop.transform.position, Vector3.one / 2f);
    AudioManager.Instance.PlayOneShot("event:/followers/poop_pop", interactionPoop.transform.position);
    f.CreatePet(FollowerPet.FollowerPetType.Poop, interactionPoop.transform.position, (System.Action<FollowerPet>) (pet =>
    {
      CritterSpider spider = pet.GetComponent<CritterSpider>();
      spider.enabled = false;
      pet.transform.DOPunchScale(Vector3.one * 0.25f, 0.25f, 1);
      GameManager.GetInstance().OnConversationNext(pet.gameObject, 5f);
      GameManager.GetInstance().WaitForSeconds(1f, (System.Action) (() => spider.enabled = true));
    }));
    yield return (object) new WaitForSeconds(1.5f);
    GameManager.GetInstance().OnConversationEnd();
  }

  public void Play()
  {
    this.Scale = 2f;
    this.StartCoroutine(this.ScaleRoutine());
  }

  public IEnumerator ScaleRoutine()
  {
    Interaction_Poop interactionPoop = this;
    float Progress = 0.0f;
    while ((double) (Progress += Time.deltaTime) < 5.0)
    {
      interactionPoop.ScaleSpeed += (float) ((1.0 - (double) interactionPoop.Scale) * 0.30000001192092896 * ((double) Time.deltaTime * 60.0));
      interactionPoop.Scale += (interactionPoop.ScaleSpeed *= 0.7f) * (Time.deltaTime * 60f);
      interactionPoop.transform.localScale = Vector3.one * interactionPoop.Scale;
      yield return (object) null;
    }
    interactionPoop.transform.localScale = Vector3.one;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sString = ScriptLocalization.Interactions.Clean;
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.Activating)
      return;
    base.OnInteract(state);
    this.Activating = true;
    this.skeletonAnimation = this.playerFarming.Spine;
    if (!this.EventListenerActive)
    {
      this.skeletonAnimation.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
      this.EventListenerActive = true;
    }
    this.StartCoroutine(this.DoClean());
  }

  public void HandleEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (!(e.Data.Name == "sfxTrigger"))
      return;
    CameraManager.shakeCamera(0.05f, Utils.GetAngle(this.playerFarming.gameObject.transform.position, this.transform.position));
    MMVibrate.Haptic(MMVibrate.HapticTypes.SoftImpact, this.playerFarming, coroutineSupport: (MonoBehaviour) GameManager.GetInstance());
    this.transform.DOKill();
    this.transform.DOPunchScale(Vector3.one * 0.15f, 0.25f);
    if (!this.playedSfx)
    {
      if (this.StructureInfo.Type != StructureBrain.TYPES.POOP_MASSIVE)
        AudioManager.Instance.PlayOneShot("event:/player/sweep", this.transform.position);
      this.playedSfx = true;
    }
    if (this.StructureInfo.Type != StructureBrain.TYPES.POOP_MASSIVE)
      return;
    AudioManager.Instance.PlayOneShot("event:/followers/poop_slime", this.transform.position);
  }

  public IEnumerator DoClean()
  {
    Interaction_Poop interactionPoop = this;
    interactionPoop.playedSfx = false;
    interactionPoop.Activating = true;
    interactionPoop.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    interactionPoop.state.facingAngle = Utils.GetAngle(interactionPoop.state.transform.position, interactionPoop.transform.position);
    yield return (object) new WaitForEndOfFrame();
    float multiplier = 1f;
    if (interactionPoop.StructureBrain.Data.Type == StructureBrain.TYPES.POOP_RAINBOW)
      multiplier = 3f;
    else if (interactionPoop.StructureBrain.Data.Type == StructureBrain.TYPES.POOP_MASSIVE)
      multiplier = 2f;
    interactionPoop.playerFarming.simpleSpineAnimator.Animate("cleaning", 0, true);
    EventInstance loop = new EventInstance();
    if (interactionPoop.StructureBrain.Data.Type == StructureBrain.TYPES.POOP_MASSIVE || interactionPoop.StructureBrain.Data.Type == StructureBrain.TYPES.TOXIC_WASTE)
      loop = AudioManager.Instance.CreateLoop("event:/player/sweep_loop", true);
    else
      AudioManager.Instance.PlayOneShot("event:/player/sweep", interactionPoop.transform.position);
    float ChoreDuration = DataManager.GetChoreDuration(interactionPoop.playerFarming);
    Debug.Log((object) ("Chore duration: " + ChoreDuration.ToString()));
    if (interactionPoop.StructureBrain.Data.Type == StructureBrain.TYPES.POOP_MASSIVE)
      ChoreDuration *= 3f;
    else if (interactionPoop.StructureBrain.Data.Type == StructureBrain.TYPES.TOXIC_WASTE)
      ChoreDuration *= 2f;
    if (interactionPoop.StructureBrain.Data.Type == StructureBrain.TYPES.POOP_DEVOTION)
      interactionPoop.StartCoroutine(interactionPoop.DevotionIE(ChoreDuration));
    else if (interactionPoop.StructureBrain.Data.Type == StructureBrain.TYPES.POOP_GOLD)
      interactionPoop.StartCoroutine(interactionPoop.GoldIE(ChoreDuration));
    else if (interactionPoop.StructureBrain.Data.Type == StructureBrain.TYPES.POOP_MASSIVE)
      interactionPoop.StartCoroutine(interactionPoop.PoopIE(ChoreDuration));
    yield return (object) new WaitForSeconds(ChoreDuration / 2f);
    if (interactionPoop.StructureInfo.Type != StructureBrain.TYPES.POOP_PET)
      interactionPoop._playerFarming.playerChoreXPBarController.AddChoreXP(interactionPoop.playerFarming, multiplier);
    yield return (object) new WaitForSeconds(ChoreDuration / 2f);
    Follower followerById = FollowerManager.FindFollowerByID(interactionPoop.StructureInfo.FollowerID);
    if ((UnityEngine.Object) followerById != (UnityEngine.Object) null)
    {
      if (interactionPoop.StructureInfo.Type == StructureBrain.TYPES.POOP_PET)
      {
        yield return (object) interactionPoop.StartCoroutine(interactionPoop.RevealPoopPetIE(followerById));
        interactionPoop._playerFarming.playerChoreXPBarController.AddChoreXP(interactionPoop.playerFarming, multiplier);
      }
      else if (interactionPoop.StructureInfo.Type == StructureBrain.TYPES.POOP_MASSIVE || interactionPoop.StructureInfo.Type == StructureBrain.TYPES.TOXIC_WASTE)
      {
        followerById.transform.parent = BaseLocationManager.Instance.UnitLayer;
        ++DataManager.Instance.FollowersTrappedInToxicWaste;
        if (interactionPoop.StructureInfo.Type == StructureBrain.TYPES.TOXIC_WASTE)
        {
          int cursedState = (int) followerById.Brain.Info.CursedState;
        }
      }
    }
    if (interactionPoop.StructureInfo.Type == StructureBrain.TYPES.POOP_MASSIVE || interactionPoop.StructureBrain.Data.Type == StructureBrain.TYPES.TOXIC_WASTE)
      AudioManager.Instance.StopLoop(loop);
    interactionPoop.StructureBrain.Remove();
    AudioManager.Instance.PlayOneShot("event:/followers/poop_pop", interactionPoop.transform.position);
    ++DataManager.Instance.itemsCleaned;
    if (!interactionPoop.hasFancyRobes)
      interactionPoop.CheckItemsCleaned();
    if (interactionPoop.StructureInfo.Type != StructureBrain.TYPES.POOP_PET)
      BiomeConstants.Instance.EmitSmokeExplosionVFX(interactionPoop.transform.position);
    interactionPoop.skeletonAnimation.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(interactionPoop.HandleEvent);
    interactionPoop.EventListenerActive = false;
    System.Action onCrownReturn = interactionPoop.playerFarming.OnCrownReturn;
    if (onCrownReturn != null)
      onCrownReturn();
    interactionPoop.state.CURRENT_STATE = StateMachine.State.Idle;
    interactionPoop.Activating = false;
    if (!DataManager.Instance.ShowCultIllness && DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Illness))
    {
      UITutorialOverlayController overlayController = MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Illness);
      overlayController.OnHide = overlayController.OnHide + new System.Action(IllnessBar.Instance.Reveal);
    }
    List<Structures_Poop> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_Poop>(in interactionPoop.StructureInfo.Location);
    for (int index = structuresOfType.Count - 1; index >= 0; --index)
    {
      if ((double) Vector3.Distance(structuresOfType[index].Data.Position, interactionPoop.transform.position) < 0.5)
      {
        structuresOfType[index].Data.LootCountToDrop = -1;
        structuresOfType[index].Remove();
      }
    }
  }

  public IEnumerator BecameMutatedIE(Follower follower)
  {
    Interaction_Poop interactionPoop = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionPoop.gameObject, 5f);
    foreach (Renderer componentsInChild in interactionPoop.GetComponentsInChildren<SpriteRenderer>())
      componentsInChild.enabled = false;
    follower.Brain.CompleteCurrentTask();
    yield return (object) new WaitForSeconds(2f);
    follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    yield return (object) new WaitForEndOfFrame();
    double num1 = (double) follower.SetBodyAnimation("Reactions/react-worried1", false);
    yield return (object) new WaitForSeconds(1f);
    double num2 = (double) follower.SetBodyAnimation("Reactions/react-scared", false);
    BiomeConstants.Instance.EmitSmokeExplosionVFX(follower.transform.position - Vector3.forward);
    AudioManager.Instance.PlayOneShot("event:/followers/poop_pop", interactionPoop.transform.position);
    follower.Brain.AddTrait(FollowerTrait.TraitType.Mutated, true);
    FollowerBrain.SetFollowerCostume(follower.Spine.Skeleton, follower.Brain._directInfoAccess, forceUpdate: true);
    yield return (object) new WaitForSeconds(1.86666667f);
    follower.Brain.CompleteCurrentTask();
    GameManager.GetInstance().OnConversationEnd();
  }

  public void CheckItemsCleaned()
  {
    if (DataManager.Instance.itemsCleaned <= DataManager.itemsCleanedNeeded || Interaction_Poop.GivenOutfit || !DataManager.Instance.TailorEnabled)
      return;
    foreach (FollowerClothingType followerClothingType in DataManager.Instance.UnlockedClothing)
    {
      if (followerClothingType == FollowerClothingType.Special_4)
        this.hasFancyRobes = true;
    }
    if (this.hasFancyRobes)
      return;
    InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FOUND_ITEM_OUTFIT, 1, this.transform.position).GetComponent<FoundItemPickUp>().clothingType = FollowerClothingType.Special_4;
    Interaction_Poop.GivenOutfit = true;
  }

  public override void GetLabel() => this.Label = this.sString;

  [CompilerGenerated]
  public void \u003COnDestroy\u003Eb__13_0() => this.playerFarming?.GetSoul(1);

  [CompilerGenerated]
  public void \u003CDevotionIE\u003Eb__14_0() => this.playerFarming?.GetSoul(1);
}
