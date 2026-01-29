// Decompiled with JetBrains decompiler
// Type: Interaction_DeathNPC
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_DeathNPC : Interaction
{
  public SkeletonAnimation ShopKeeperSpine;
  public SkeletonAnimation FollowerSpine;
  public bool Activated;
  public FollowerInfo _followerInfo;
  public FollowerOutfit _outfit;
  public Interaction_SimpleConversation Conversation;
  public static Interaction_DeathNPC Instance;
  public GameObject Notification;

  public override void OnEnableInteraction()
  {
    if (DataManager.Instance.FollowerTokens > 0 && !DataManager.Instance.firstRecruit)
      this.Notification.SetActive(true);
    else
      this.Notification.SetActive(false);
    base.OnEnableInteraction();
    Interaction_DeathNPC.Instance = this;
  }

  public void Start()
  {
    if (!DataManager.Instance.BlackSoulsEnabled)
      this.enabled = false;
    else
      UnityEngine.Object.Destroy((UnityEngine.Object) this.Conversation);
    this.NewRecruitable();
  }

  public void NewRecruitable()
  {
    this._followerInfo = FollowerInfo.NewCharacter(FollowerLocation.Church);
    this._outfit = new FollowerOutfit(this._followerInfo);
    this._outfit.SetOutfit(this.FollowerSpine, false);
  }

  public override void GetLabel()
  {
    this.Label = this.Activated ? "" : "<sprite name=\"icon_Followers\"> x1";
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (DataManager.Instance.FollowerTokens >= 1)
      this.StartCoroutine((IEnumerator) this.Purchase());
    else
      this.SpiderAnimationCantAfford();
  }

  public IEnumerator Purchase()
  {
    Interaction_DeathNPC interactionDeathNpc = this;
    interactionDeathNpc.Activated = true;
    GameManager.GetInstance().OnConversationNew(false);
    GameManager.GetInstance().OnConversationNext(interactionDeathNpc.gameObject, 4f);
    GameManager.GetInstance().AddPlayerToCamera();
    interactionDeathNpc.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    for (int i = 0; i < 1; ++i)
    {
      AudioManager.Instance.PlayOneShot("event:/followers/pop_in", interactionDeathNpc.state.transform.position);
      ResourceCustomTarget.Create(interactionDeathNpc.gameObject, interactionDeathNpc.state.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, (System.Action) null);
      yield return (object) new WaitForSeconds(0.1f);
    }
    --Inventory.FollowerTokens;
    interactionDeathNpc.SpiderAnimationBoughtItem();
    yield return (object) new WaitForSeconds(1f);
    interactionDeathNpc.Notification.SetActive(false);
    GameManager.GetInstance().OnConversationNext(ChurchFollowerManager.Instance.RitualCenterPosition.gameObject, 4f);
    yield return (object) new WaitForSeconds(1f);
    CameraManager.shakeCamera(0.5f, (float) UnityEngine.Random.Range(0, 360));
    interactionDeathNpc.NewRecruitable();
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().OnConversationNext(interactionDeathNpc.gameObject, 6f);
    GameManager.GetInstance().AddPlayerToCamera();
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationEnd();
    yield return (object) new WaitForSeconds(0.5f);
    interactionDeathNpc.Activated = false;
  }

  public void SpiderAnimationBoughtItem()
  {
  }

  public void SpiderAnimationCantAfford()
  {
  }
}
