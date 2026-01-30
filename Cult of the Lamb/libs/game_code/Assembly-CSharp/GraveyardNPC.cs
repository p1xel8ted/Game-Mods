// Decompiled with JetBrains decompiler
// Type: GraveyardNPC
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using src.UINavigator;
using System;
using System.Collections;
using Unify;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class GraveyardNPC : GhostNPC
{
  [SerializeField]
  public Interaction_SimpleConversation ratauCloakReactionInteraction;
  [SerializeField]
  public SimpleBark ratauCloakReactionBark;

  public override void OnEnable()
  {
    base.OnEnable();
    if (DataManager.Instance.PlayerFleece != 11 && DataManager.Instance.PlayerFleece != 12 || !DataManager.Instance.GraveyardShopFixed)
      return;
    if (DataManager.Instance.RatauKilled && !DataManager.Instance.RatauStaffQuestAliveDead)
    {
      this.ratauCloakReactionInteraction.Callback.AddListener(new UnityAction(this.PlayGiveWeaponSequenece));
      this.ratauCloakReactionInteraction.gameObject.SetActive(true);
    }
    else
      this.ratauCloakReactionBark.gameObject.SetActive(true);
  }

  public new void OnDisable()
  {
    this.ratauCloakReactionInteraction.Callback.RemoveListener(new UnityAction(this.PlayGiveWeaponSequenece));
  }

  public void PlayGiveWeaponSequenece()
  {
    this.StartCoroutine((IEnumerator) this.GiveWeaponSequence());
  }

  public IEnumerator GiveWeaponSequence()
  {
    GraveyardNPC graveyardNpc = this;
    PlayerFarming player = MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer;
    while (player.state.CURRENT_STATE == StateMachine.State.InActive)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNew();
    PlayerSimpleInventory inventory = player.state.gameObject.GetComponent<PlayerSimpleInventory>();
    PickUp ratauStaff = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.RATAU_STAFF, 1, graveyardNpc.transform.position, 0.0f, (Action<PickUp>) (pickUp => pickUp.enabled = false));
    while ((UnityEngine.Object) ratauStaff == (UnityEngine.Object) null)
      yield return (object) null;
    Vector3 endValue = new Vector3(inventory.ItemImage.transform.position.x, inventory.ItemImage.transform.position.y, -1f);
    GameManager.GetInstance().OnConversationNext(ratauStaff.gameObject, 7f);
    bool isMoving = true;
    ratauStaff.transform.DOMove(endValue, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => isMoving = false));
    while (isMoving)
      yield return (object) null;
    player.state.CURRENT_STATE = StateMachine.State.FoundItem;
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", graveyardNpc.transform.position);
    yield return (object) new WaitForSeconds(1.5f);
    DataManager.Instance.ForcedStartingWeapon = EquipmentType.Sword_Ratau;
    DataManager.Instance.AddWeapon(EquipmentType.Sword_Ratau);
    NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("Notifications/LegendaryWeaponAddedToPool", EquipmentManager.GetWeaponData(EquipmentType.Sword_Ratau).GetLocalisedTitle());
    UnityEngine.Object.Destroy((UnityEngine.Object) ratauStaff.gameObject);
    GameManager.GetInstance().OnConversationEnd();
    AchievementsWrapper.UnlockAchievement(Achievements.Instance.Lookup(AchievementsWrapper.Tags.RATAU_END));
  }
}
