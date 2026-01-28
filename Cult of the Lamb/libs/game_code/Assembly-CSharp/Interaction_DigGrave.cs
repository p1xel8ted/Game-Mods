// Decompiled with JetBrains decompiler
// Type: Interaction_DigGrave
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Spine;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class Interaction_DigGrave : Interaction
{
  public GameObject GraveMound;
  public GameObject GraveHole;
  public AssetReferenceGameObject ZombiePrefab;
  [SerializeField]
  public bool forceReward;
  [SerializeField]
  public InventoryItem.ITEM_TYPE forcedReward;
  public bool Activated;
  public string sString;

  public void Start() => this.UpdateLocalisation();

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sString = ScriptLocalization.Interactions.DigGrave;
  }

  public override void GetLabel()
  {
    this.Label = this.Activated || !this.Interactable ? "" : this.sString;
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.Activated = true;
    GameManager.GetInstance().OnConversationNew(false);
    GameManager.GetInstance().OnConversationNext(this.playerFarming.CameraBone, 6f);
    this.playerFarming.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
    this.playerFarming.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
    this.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    this.playerFarming.TimedAction(1.5f, (System.Action) (() => { }), "actions/dig");
    this.playerFarming.health.invincible = true;
    this.Interactable = false;
  }

  public override void Update()
  {
    base.Update();
    if (!this.Activated || this.playerFarming.state.CURRENT_STATE == StateMachine.State.CustomAction0)
      return;
    this.Activated = false;
    GameManager.GetInstance().OnConversationEnd(false);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    if (!((UnityEngine.Object) this.playerFarming != (UnityEngine.Object) null))
      return;
    this.playerFarming.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
  }

  public void HandleEvent(TrackEntry trackentry, Spine.Event e)
  {
    if (!(e.Data.Name == "dig"))
      return;
    AudioManager.Instance.PlayOneShot("event:/material/dirt_dig", this.gameObject);
    MMVibrate.Haptic(MMVibrate.HapticTypes.LightImpact, this.playerFarming);
    CameraManager.instance.ShakeCameraForDuration(0.1f, 0.3f, 1f);
    this.StartCoroutine((IEnumerator) this.InteractRoutine());
  }

  public IEnumerator InteractRoutine()
  {
    Interaction_DigGrave interactionDigGrave = this;
    BiomeConstants.Instance.EmitSmokeExplosionVFX(interactionDigGrave.gameObject.transform.position);
    CameraManager.instance.ShakeCameraForDuration(0.1f, 0.3f, 1f);
    GameManager.GetInstance().OnConversationNext(interactionDigGrave.playerFarming.CameraBone, 6f);
    interactionDigGrave.playerFarming.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(interactionDigGrave.HandleEvent);
    if (!interactionDigGrave.forceReward)
    {
      switch (UnityEngine.Random.Range(0, 7))
      {
        case 0:
        case 1:
          switch (UnityEngine.Random.Range(0, 7))
          {
            case 0:
              InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLUE_HEART, 1, interactionDigGrave.GraveHole.transform.position, 0.0f);
              break;
            case 1:
            case 2:
              InventoryItem.Spawn(InventoryItem.ITEM_TYPE.HALF_BLUE_HEART, 1, interactionDigGrave.GraveHole.transform.position, 0.0f);
              break;
            case 3:
              InventoryItem.Spawn(InventoryItem.ITEM_TYPE.RED_HEART, 1, interactionDigGrave.GraveHole.transform.position, 0.0f);
              break;
            case 4:
            case 5:
              InventoryItem.Spawn(InventoryItem.ITEM_TYPE.HALF_HEART, 1, interactionDigGrave.GraveHole.transform.position, 0.0f);
              break;
          }
          break;
        case 2:
        case 3:
          InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BONE, UnityEngine.Random.Range(3, 6), interactionDigGrave.GraveHole.transform.position);
          break;
        case 4:
        case 5:
        case 6:
          int num = UnityEngine.Random.Range(1, 4);
          for (int index = 0; index < num; ++index)
            Addressables.InstantiateAsync((object) interactionDigGrave.ZombiePrefab, interactionDigGrave.transform.position + (Vector3) (UnityEngine.Random.insideUnitCircle * 0.5f), Quaternion.identity, interactionDigGrave.transform.parent).Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj => obj.Result.GetComponent<EnemySwordsman>().GraveSpawn());
          break;
      }
    }
    else if (!LoreSystem.LoreAvailable(10) || !LoreSystem.LoreAvailable(11) || !LoreSystem.LoreAvailable(12))
    {
      PickUp pickUp = InventoryItem.Spawn(interactionDigGrave.forcedReward, 1, interactionDigGrave.GraveHole.transform.position);
      if (interactionDigGrave.forcedReward == InventoryItem.ITEM_TYPE.LORE_STONE)
      {
        LoreStone component = pickUp.GetComponent<LoreStone>();
        if (!LoreSystem.LoreAvailable(10))
          component.SetLore(10, true);
        else if (!LoreSystem.LoreAvailable(11))
          component.SetLore(11, true);
        else if (!LoreSystem.LoreAvailable(12))
          component.SetLore(12, true);
        else
          Debug.Log((object) "Have unlocked all the grave lores");
      }
    }
    else
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.HALF_BLUE_HEART, 1, interactionDigGrave.GraveHole.transform.position, 0.0f);
    InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BONE, UnityEngine.Random.Range(3, 6), interactionDigGrave.GraveHole.transform.position);
    interactionDigGrave.Interactable = false;
    interactionDigGrave.Label = "";
    interactionDigGrave.playerFarming.health.invincible = false;
    interactionDigGrave.GraveMound.SetActive(false);
    interactionDigGrave.GraveHole.SetActive(true);
    GameManager.GetInstance().OnConversationEnd();
    yield return (object) new WaitForSeconds(0.1f);
  }
}
