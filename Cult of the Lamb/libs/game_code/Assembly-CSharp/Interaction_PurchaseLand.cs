// Decompiled with JetBrains decompiler
// Type: Interaction_PurchaseLand
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MMTools;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_PurchaseLand : Interaction
{
  [SerializeField]
  public SkeletonAnimation spine;
  [SerializeField]
  public int landIndex;
  [SerializeField]
  public InventoryItem.ITEM_TYPE costType;
  [SerializeField]
  public int cost;
  [SerializeField]
  public float revealDistance;
  [SerializeField]
  public Vector3 revealOffset;
  [SerializeField]
  public Vector3 postBuildOffset;
  [SerializeField]
  public GameObject cameraTarget;
  [SerializeField]
  public Interaction_SimpleConversation introConvo;
  [SerializeField]
  public Interaction_SimpleConversation builtConvo;
  [SerializeField]
  public SkeletonAnimation[] workers;
  public bool isRevealed;
  [CompilerGenerated]
  public bool \u003CForceShown\u003Ek__BackingField;

  public bool ForceShown
  {
    get => this.\u003CForceShown\u003Ek__BackingField;
    set => this.\u003CForceShown\u003Ek__BackingField = value;
  }

  public void Start()
  {
    if (this.landIndex <= DataManager.Instance.LandPurchased)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    else
      this.introConvo.gameObject.SetActive(DataManager.Instance.LandConvoProgress == 0);
    this.spine.gameObject.SetActive(false);
  }

  public override void GetLabel()
  {
    base.GetLabel();
    if (DataManager.Instance.LandConvoProgress >= 2)
    {
      this.Interactable = false;
      this.Label = "";
    }
    else
    {
      this.Interactable = true;
      this.Label = ScriptLocalization.Interactions.Give + InventoryItem.CapacityString(this.costType, this.cost);
    }
  }

  public override void Update()
  {
    base.Update();
    bool flag = false;
    foreach (Component player in PlayerFarming.players)
    {
      if ((double) Vector3.Distance(player.transform.position, this.transform.position + this.revealOffset) < (double) this.revealDistance)
        flag = true;
    }
    if (this.ForceShown)
    {
      flag = true;
      if (DataManager.Instance.LandConvoProgress > 0)
        this.ForceShown = false;
    }
    if (flag && !this.isRevealed)
    {
      this.spine.gameObject.SetActive(true);
      AudioManager.Instance.PlayOneShot("event:/dlc/env/gofernon/burrow_in", this.spine.transform.position);
      this.spine.AnimationState.SetAnimation(0, "dig_up", false);
      this.spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
      this.isRevealed = true;
    }
    else
    {
      if (flag || !this.isRevealed)
        return;
      this.spine.AnimationState.SetAnimation(0, "dig_down", false);
      this.isRevealed = false;
    }
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (Inventory.GetItemQuantity(this.costType) >= this.cost)
    {
      state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      this.playerFarming.state.facingAngle = this.playerFarming.state.LookAngle = Utils.GetAngle(this.playerFarming.transform.position, this.spine.transform.position);
      this.Interactable = false;
      this.StartCoroutine((IEnumerator) this.GiveResources());
    }
    else
      state.GetComponent<PlayerFarming>().indicator.PlayShake();
  }

  public IEnumerator GiveResources()
  {
    Interaction_PurchaseLand interactionPurchaseLand = this;
    GameManager.GetInstance().OnConversationNew();
    for (int i = 0; i < interactionPurchaseLand.cost; ++i)
    {
      Inventory.ChangeItemQuantity(interactionPurchaseLand.costType, -1);
      ResourceCustomTarget.Create(interactionPurchaseLand.spine.gameObject, interactionPurchaseLand._playerFarming.transform.position, interactionPurchaseLand.costType, (System.Action) null);
      ObjectiveManager.GiveItem(interactionPurchaseLand.costType);
      yield return (object) new WaitForSeconds(0.15f);
    }
    interactionPurchaseLand.HasChanged = true;
    ++DataManager.Instance.LandConvoProgress;
    GameManager.GetInstance().StartCoroutine((IEnumerator) interactionPurchaseLand.UnlockSequenceIE());
  }

  public IEnumerator UnlockSequenceIE()
  {
    Interaction_PurchaseLand interactionPurchaseLand = this;
    if (DataManager.Instance.LandPurchased == -1)
    {
      List<ConversationEntry> Entries = new List<ConversationEntry>()
      {
        new ConversationEntry(interactionPurchaseLand.gameObject, "Conversation_NPC/BaseExpansion/First/GivenResources/0")
      };
      foreach (ConversationEntry conversationEntry in Entries)
        conversationEntry.CharacterName = LocalizationManager.GetTranslation("NAMES/BaseExpansionNPC");
      MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) null), false);
      yield return (object) null;
      while (MMConversation.isPlaying)
        yield return (object) null;
    }
    ++DataManager.Instance.LandPurchased;
    DataManager.Instance.LandConvoProgress = 0;
    DataManager.Instance.LandResourcesGiven = 0;
    interactionPurchaseLand.spine.AnimationState.SetAnimation(0, "dig_down", false);
    AudioManager.Instance.PlayOneShot("event:/dlc/env/gofernon/burrow_out", interactionPurchaseLand.spine.transform.position);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionPurchaseLand.cameraTarget, 15f);
    interactionPurchaseLand.transform.parent = (Transform) null;
    yield return (object) new WaitForSeconds(1f);
    CameraManager.instance.ShakeCameraForDuration(0.1f, 0.2f, 3f);
    AudioManager.Instance.PlayOneShot("event:/dlc/env/gofernon/base_expand", interactionPurchaseLand.transform.position);
    foreach (SkeletonAnimation worker in interactionPurchaseLand.workers)
    {
      worker.transform.position = new Vector3(interactionPurchaseLand.cameraTarget.transform.position.x, interactionPurchaseLand.cameraTarget.transform.position.y) + (Vector3) UnityEngine.Random.insideUnitCircle * 5f;
      interactionPurchaseLand.StartCoroutine((IEnumerator) interactionPurchaseLand.WorkerRoutine(worker, UnityEngine.Random.Range(0.0f, 2f), 3f));
    }
    yield return (object) new WaitForSeconds(3f);
    CameraManager.instance.ShakeCameraForDuration(0.7f, 0.9f, 2f);
    yield return (object) new WaitForSeconds(2f);
    bool waiting = true;
    MMTransition.Play(MMTransition.TransitionType.FadeAndCallBack, MMTransition.Effect.BlackFade, "", 1f, "", (System.Action) (() => waiting = false));
    while (waiting)
      yield return (object) null;
    DLCLandController.LandSlot landSlot = DLCLandController.Instance.GetLandSlot(DataManager.Instance.LandPurchased);
    DLCLandController.Instance.ShowSlot(DataManager.Instance.LandPurchased);
    PlacementRegion.Instance.Grid.Clear();
    PlacementRegion.Instance.GridTileLookup.Clear();
    PlacementRegion.Instance.CreateFloodFill();
    foreach (Structure structure in Structure.Structures)
    {
      if ((UnityEngine.Object) structure != (UnityEngine.Object) null && structure.Brain != null && !structure.Brain.Data.IgnoreGrid && !structure.Brain.Data.DoesNotOccupyGrid)
        PlacementRegion.Instance.structureBrain.AddStructureToGrid(structure.Brain.Data);
    }
    Interaction_PurchaseLand.OccupyBridgeArea();
    interactionPurchaseLand.spine.AnimationState.SetAnimation(0, "dig_up", false);
    interactionPurchaseLand.spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    float num = 1f;
    if (DataManager.Instance.LandPurchased > 0)
      num = 0.75f;
    StructureManager.CreateWeeds(FollowerLocation.Base, new List<Structures_PlacementRegion>()
    {
      PlacementRegion.Instance.structureBrain
    }, landSlot.SegmentCollider, (int) ((double) UnityEngine.Random.Range(100, 150) * (double) num));
    StructureManager.PlaceRubble(FollowerLocation.Base, landSlot.ResourcesToPlace, PlacementRegion.Instance.structureBrain, landSlot.SegmentCollider);
    for (int index = 0; index < PlayerFarming.players.Count; ++index)
      PlayerFarming.players[index].transform.position = interactionPurchaseLand.transform.position + Vector3.up * ((float) (index + 1) / 2f);
    interactionPurchaseLand.transform.position += interactionPurchaseLand.postBuildOffset;
    foreach (Component worker in interactionPurchaseLand.workers)
      worker.gameObject.SetActive(false);
    yield return (object) new WaitForSeconds(2f);
    MMTransition.ResumePlay();
    GameManager.GetInstance().OnConversationNew();
    yield return (object) new WaitForSeconds(1f);
    interactionPurchaseLand.builtConvo.Play();
    while (MMConversation.isPlaying)
      yield return (object) null;
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.ExpandBase);
    interactionPurchaseLand.enabled = false;
    if (interactionPurchaseLand.landIndex >= 2)
      interactionPurchaseLand.GiveSkin();
    yield return (object) new WaitForSeconds(2f);
    UnityEngine.Object.Destroy((UnityEngine.Object) interactionPurchaseLand.gameObject);
  }

  public IEnumerator WorkerRoutine(SkeletonAnimation worker, float delay, float totalWait)
  {
    yield return (object) new WaitForSeconds(delay);
    worker.gameObject.SetActive(true);
    AudioManager.Instance.PlayOneShot("event:/dlc/env/gofernon/base_expand_mole_popup");
    worker.AnimationState.SetAnimation(0, "dig-up", false);
    worker.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    yield return (object) new WaitForSeconds(totalWait - delay);
    for (int i = 0; i < 10; ++i)
    {
      yield return (object) new WaitForSeconds(0.25f);
      BiomeConstants.Instance.EmitSmokeInteractionVFX(worker.transform.position, Vector3.one * (UnityEngine.Random.value + 1f));
    }
  }

  public void GiveObjective()
  {
    ++DataManager.Instance.LandConvoProgress;
    ObjectiveManager.Add((ObjectivesData) new Objectives_GiveItem("Objectives/GroupTitles/ExpandBase", "NAMES/BaseExpansionNPC", this.cost, this.costType), isDLCObjective: true);
  }

  public override void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position + this.revealOffset, this.revealDistance, Color.white);
  }

  public void GiveSkin()
  {
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(this.gameObject, 6f);
    FollowerSkinCustomTarget.Create(this.spine.transform.position, this.playerFarming.transform.position, 2f, "Mole", (System.Action) (() =>
    {
      GameManager.GetInstance().OnConversationEnd();
      GameManager.GetInstance().AddPlayerToCamera();
    }));
  }

  public static void OccupyBridgeArea()
  {
    for (int x = -9; x < -5; ++x)
    {
      for (int y = -8; y < -4; ++y)
      {
        PlacementRegion.TileGridTile tileGridTile = PlacementRegion.Instance.GetTileGridTile(new Vector2Int(x, y));
        if (tileGridTile != null)
        {
          tileGridTile.Occupied = true;
          tileGridTile.ReservedForWaste = false;
        }
      }
    }
    foreach (Vector2Int Position in new List<Vector2Int>()
    {
      new Vector2Int(-10, -7),
      new Vector2Int(-8, -9),
      new Vector2Int(-7, -4),
      new Vector2Int(-5, -6)
    })
    {
      PlacementRegion.TileGridTile tileGridTile = PlacementRegion.Instance.GetTileGridTile(Position);
      if (tileGridTile != null)
      {
        tileGridTile.Occupied = true;
        tileGridTile.ReservedForWaste = false;
      }
    }
  }
}
