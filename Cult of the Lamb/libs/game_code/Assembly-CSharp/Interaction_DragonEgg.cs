// Decompiled with JetBrains decompiler
// Type: Interaction_DragonEgg
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using I2.Loc;
using MMTools;
using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_DragonEgg : Interaction
{
  [SerializeField]
  public SkeletonAnimation egg;
  [SerializeField]
  public SkeletonAnimation portalSpine;
  [SerializeField]
  public GameObject podium;
  [SerializeField]
  public GameObject purchaseBark;
  [SerializeField]
  public Interaction_SimpleConversation introConvo;
  [SerializeField]
  public Interaction_SimpleConversation finalConvo;
  public InventoryItem.ITEM_TYPE costType;
  public static List<string> DragonSkins = new List<string>()
  {
    "Dragon",
    "DragonTwo",
    "DragonThree",
    "DragonFour",
    "DragonFive"
  };
  public EventInstance receiveLoop;

  public int cost
  {
    get
    {
      return DataManager.Instance.DragonEggsCollected < 5 ? 10 * (DataManager.Instance.DragonEggsCollected + 1) : 20;
    }
  }

  public void Start()
  {
    this.costType = (double) UnityEngine.Random.value < 0.5 ? InventoryItem.ITEM_TYPE.FLOWER_PURPLE : InventoryItem.ITEM_TYPE.FLOWER_WHITE;
    if (!DataManager.Instance.DragonIntrod)
      this.introConvo.gameObject.SetActive(true);
    if (this.egg.AnimationState == null)
      return;
    this.egg.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.AnimationState_Event);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if ((UnityEngine.Object) AudioManager.Instance != (UnityEngine.Object) null)
      AudioManager.Instance.StopLoop(this.receiveLoop);
    if (this.egg.AnimationState == null)
      return;
    this.egg.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.AnimationState_Event);
  }

  public override void GetLabel()
  {
    base.GetLabel();
    this.Label = $"{string.Format(ScriptLocalization.Interactions.Buy, (object) "")}: {InventoryItem.CapacityString(this.costType, this.cost)}";
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (Inventory.GetItemQuantity(this.costType) >= this.cost)
    {
      AudioManager.Instance.PlayOneShot("event:/shop/buy");
      this.StartCoroutine((IEnumerator) this.GiveEggIE());
    }
    else
      state.GetComponent<PlayerFarming>().indicator.PlayShake();
  }

  public IEnumerator GiveEggIE()
  {
    Interaction_DragonEgg interactionDragonEgg = this;
    MMConversation.ClearConversation();
    Inventory.ChangeItemQuantity(interactionDragonEgg.costType, -interactionDragonEgg.cost);
    string ForceSkin = "";
    if (DataManager.Instance.DragonEggsCollected < Interaction_DragonEgg.DragonSkins.Count)
    {
      ForceSkin = Interaction_DragonEgg.DragonSkins[DataManager.Instance.DragonEggsCollected];
    }
    else
    {
      foreach (string dragonSkin in Interaction_DragonEgg.DragonSkins)
      {
        if (!DataManager.Instance.FollowerSkinsUnlocked.Contains(dragonSkin))
        {
          ForceSkin = dragonSkin;
          break;
        }
      }
      if (string.IsNullOrEmpty(ForceSkin))
        ForceSkin = Interaction_DragonEgg.DragonSkins[UnityEngine.Random.Range(0, Interaction_DragonEgg.DragonSkins.Count)];
    }
    FollowerInfo followerInfo = FollowerInfo.NewCharacter(FollowerLocation.Base, ForceSkin);
    followerInfo.FromDragonNPC = true;
    followerInfo.Special = FollowerSpecialType.Gold;
    DataManager.Instance.Followers_Recruit.Add(followerInfo);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionDragonEgg.egg.gameObject, 4f);
    for (int i = 0; i < 5; ++i)
    {
      ResourceCustomTarget.Create(interactionDragonEgg.gameObject, PlayerFarming.Instance.transform.position, interactionDragonEgg.costType, (System.Action) null);
      yield return (object) new WaitForSeconds(0.2f);
    }
    PlayerFarming.Instance.GoToAndStop(PlayerFarming.Instance.transform.position + Vector3.right * 1.5f, interactionDragonEgg.gameObject);
    yield return (object) new WaitForSeconds(1f);
    interactionDragonEgg.egg.transform.parent = (Transform) null;
    AudioManager.Instance.PlayOneShot("event:/dialogue/followers/positive_acknowledge", interactionDragonEgg.gameObject);
    AudioManager.Instance.PlayOneShot("event:/player/receive_animation_start", interactionDragonEgg.egg.gameObject);
    interactionDragonEgg.receiveLoop = AudioManager.Instance.CreateLoop("event:/player/receive_animation_loop", interactionDragonEgg.egg.gameObject, true);
    interactionDragonEgg.egg.AnimationState.SetAnimation(0, "Egg/convert", false);
    interactionDragonEgg.portalSpine.gameObject.SetActive(true);
    interactionDragonEgg.portalSpine.AnimationState.SetAnimation(0, "convert-short", false);
    interactionDragonEgg.podium.gameObject.SetActive(false);
    CameraManager.shakeCamera(0.5f, (float) UnityEngine.Random.Range(0, 360));
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForSeconds(PlayerFarming.Instance.simpleSpineAnimator.Animate("specials/special-activate-long", 0, true).Animation.Duration - 1f);
    AudioManager.Instance.PlayOneShot("event:/player/receive_animation_end", interactionDragonEgg.egg.gameObject);
    int num = (int) interactionDragonEgg.receiveLoop.stop(STOP_MODE.ALLOWFADEOUT);
    interactionDragonEgg.egg.gameObject.SetActive(false);
    GameManager.GetInstance().OnConversationEnd();
    if (DataManager.Instance.DragonEggsCollected + 1 == Interaction_DragonEgg.DragonSkins.Count)
      interactionDragonEgg.finalConvo.Play();
    else
      interactionDragonEgg.purchaseBark.gameObject.SetActive(true);
    ++DataManager.Instance.DragonEggsCollected;
    interactionDragonEgg.gameObject.SetActive(false);
  }

  public void AnimationState_Event(TrackEntry trackEntry, Spine.Event e)
  {
    if (!(e.Data.Name == "Audio/egg_bounce"))
      return;
    AudioManager.Instance.PlayOneShot("event:/material/egg_bounce", this.egg.gameObject);
  }
}
