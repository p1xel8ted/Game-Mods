// Decompiled with JetBrains decompiler
// Type: Vomit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
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
public class Vomit : Interaction
{
  public Structure structure;
  public static List<Vomit> Vomits = new List<Vomit>();
  [SerializeField]
  public GameObject normal;
  [SerializeField]
  public GameObject mutated;
  [SerializeField]
  public Transform vomitSpriteTransform;
  [SerializeField]
  public Transform mutatedVomitSpriteTransform;
  public string sString;
  public SkeletonAnimation skeletonAnimation;
  public bool EventListenerActive;
  public bool playedSfx;
  public bool hasFancyRobes;

  public StructuresData StructureInfo => this.structure.Structure_Info;

  public StructureBrain StructureBrain => this.structure.Brain;

  public void Start() => this.UpdateLocalisation();

  public bool Activating
  {
    get => this.StructureBrain != null && this.StructureBrain.ReservedByPlayer;
    set => this.StructureBrain.ReservedByPlayer = value;
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    Vomit.Vomits.Add(this);
    this.structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    Vomit.Vomits.Remove(this);
    this.structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
  }

  public void OnBrainAssigned()
  {
    if (!this.StructureInfo.Picked)
    {
      this.StructureInfo.Picked = true;
      this.transform.localScale = Vector3.one;
      this.transform.DOPunchScale(Vector3.one * 0.25f, 0.25f);
      this.vomitSpriteTransform.transform.localPosition = new Vector3(0.0f, 0.0f, UnityEngine.Random.Range(-0.02f, 0.0f));
      this.mutatedVomitSpriteTransform.transform.localPosition = new Vector3(0.0f, 0.0f, UnityEngine.Random.Range(-0.02f, 0.0f));
    }
    if ((UnityEngine.Object) TownCentre.Instance != (UnityEngine.Object) null && (double) Vector3.Distance(this.transform.position, TownCentre.Instance.transform.position) < 2.0)
      this.StructureBrain.Remove();
    this.normal.gameObject.SetActive(this.StructureInfo.VariantIndex == 0);
    this.mutated.gameObject.SetActive(this.StructureInfo.VariantIndex == 1);
    this.vomitSpriteTransform.gameObject.SetActive(this.StructureInfo.VariantIndex == 0);
    this.mutatedVomitSpriteTransform.gameObject.SetActive(this.StructureInfo.VariantIndex == 1);
    this.OutlineTarget = this.StructureInfo.VariantIndex == 0 ? this.vomitSpriteTransform.gameObject : this.mutatedVomitSpriteTransform.gameObject;
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
    this.StartCoroutine((IEnumerator) this.DoClean());
    this.skeletonAnimation = this.playerFarming.Spine;
    if (this.EventListenerActive)
      return;
    this.skeletonAnimation.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
    this.EventListenerActive = true;
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
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

  public void HandleEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (!(e.Data.Name == "sfxTrigger"))
      return;
    CameraManager.shakeCamera(0.05f, Utils.GetAngle(this.playerFarming.gameObject.transform.position, this.transform.position));
    MMVibrate.Haptic(MMVibrate.HapticTypes.SoftImpact, this.playerFarming, coroutineSupport: (MonoBehaviour) GameManager.GetInstance());
    if (this.playedSfx)
      return;
    AudioManager.Instance.PlayOneShot("event:/player/sweep", this.transform.position);
    this.playedSfx = true;
  }

  public IEnumerator DoClean()
  {
    Vomit vomit = this;
    vomit.Activating = true;
    vomit.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    Vector3 position = vomit.transform.position;
    vomit.state.facingAngle = Utils.GetAngle(vomit.state.transform.position, position);
    yield return (object) new WaitForEndOfFrame();
    vomit._playerFarming.playerChoreXPBarController.AddChoreXP(vomit.playerFarming);
    vomit.playerFarming.simpleSpineAnimator.Animate("cleaning", 0, true);
    AudioManager.Instance.PlayOneShot("event:/player/sweep", vomit.transform.position);
    float choreDuration = DataManager.GetChoreDuration(vomit.playerFarming);
    Debug.Log((object) ("Chore duration: " + choreDuration.ToString()));
    yield return (object) new WaitForSeconds(choreDuration);
    vomit.transform.DOKill();
    vomit.transform.DOScale(0.0f, 0.15f).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(new TweenCallback(vomit.\u003CDoClean\u003Eb__25_0));
  }

  public static void SpawnLoot(Vector3 position)
  {
    int num = UnityEngine.Random.Range(0, 100);
    if (num < 87)
      return;
    if (num < 90)
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, position);
    else if (num < 93)
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BONE, 1, position);
    else if (num < 95)
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.MEAT, 1, position);
    else if (num < 98)
    {
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BERRY, 3, position);
    }
    else
    {
      if (num >= 100)
        return;
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.Necklace_4, 1, position);
    }
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
  public void \u003CDoClean\u003Eb__25_0()
  {
    AudioManager.Instance.PlayOneShot("event:/followers/poop_pop", this.transform.position);
    List<StructureBrain> structuresOfType = StructureManager.GetAllStructuresOfType(in this.StructureInfo.Location, StructureBrain.TYPES.VOMIT);
    for (int index = structuresOfType.Count - 1; index >= 0; --index)
    {
      if ((double) Vector3.Distance(structuresOfType[index].Data.Position, this.transform.position) < 0.5)
        structuresOfType[index].Remove();
    }
    ++DataManager.Instance.itemsCleaned;
    if (!this.hasFancyRobes)
      this.CheckItemsCleaned();
    this.skeletonAnimation.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
    this.EventListenerActive = false;
    if (this.StructureInfo.VariantIndex == 1)
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.SOOT, UnityEngine.Random.Range(2, 6), this.transform.position);
    else
      Vomit.SpawnLoot(this.transform.position);
    BiomeConstants.Instance.EmitBloodSplatter(this.transform.position, Vector3.back, Color.green);
    BiomeConstants.Instance.EmitBloodDieEffect(this.transform.position, Vector3.back, Color.green);
    System.Action onCrownReturn = this.playerFarming.OnCrownReturn;
    if (onCrownReturn != null)
      onCrownReturn();
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this.Activating = false;
    if (!DataManager.Instance.ShowCultIllness && DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Illness))
    {
      UITutorialOverlayController overlayController = MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Illness);
      overlayController.OnHide = overlayController.OnHide + new System.Action(IllnessBar.Instance.Reveal);
    }
    this.StructureBrain.Remove();
  }
}
