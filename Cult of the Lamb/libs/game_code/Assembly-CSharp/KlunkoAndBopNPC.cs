// Decompiled with JetBrains decompiler
// Type: KlunkoAndBopNPC
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Spine.Unity;
using src.UINavigator;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class KlunkoAndBopNPC : MonoBehaviour
{
  [SerializeField]
  public Interaction_SimpleConversation talkToBop;
  [SerializeField]
  public Transform bopStartPosition;
  [SerializeField]
  public SkeletonAnimation spine;

  public void Start()
  {
    if (DataManager.Instance.MAJOR_DLC && ObjectiveManager.GroupExists("Objectives/GroupTitles/BerithAndBop") && !DataManager.Instance.TookBopToTailor)
    {
      this.talkToBop.Callback.AddListener(new UnityAction(this.GiveBop));
      this.talkToBop.gameObject.SetActive(true);
    }
    if (!DataManager.Instance.TookBopToTailor)
      return;
    this.spine.Skeleton.Skin.AddSkin(this.spine.Skeleton.Data.FindSkin("Normal-NoBop"));
    this.spine.Skeleton.SetSlotsToSetupPose();
  }

  public void GiveBop()
  {
    PickUp bopItem = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BOP, 1, this.bopStartPosition.position);
    bopItem.enabled = false;
    Vector3 endValue = new Vector3(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer.simpleInventory.ItemImage.transform.position.x, MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer.simpleInventory.ItemImage.transform.position.y, -1f);
    GameManager.GetInstance().OnConversationNext(bopItem.gameObject, 7f);
    this.spine.Skeleton.Skin.AddSkin(this.spine.Skeleton.Data.FindSkin("Normal-NoBop"));
    this.spine.Skeleton.SetSlotsToSetupPose();
    TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = bopItem.transform.DOMove(endValue, 0.7f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    tweenerCore.onComplete = tweenerCore.onComplete + (TweenCallback) (() =>
    {
      MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer.state.CURRENT_STATE = StateMachine.State.FoundItem;
      Inventory.AddItem(InventoryItem.ITEM_TYPE.BOP, 1);
      DataManager.Instance.TookBopToTailor = true;
      ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/BerithAndBop", Objectives.CustomQuestTypes.BringBopToBerith), isDLCObjective: true);
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.VisitKlunkoAndBop);
      GameManager.GetInstance().WaitForSeconds(0.8f, (System.Action) (() =>
      {
        UnityEngine.Object.Destroy((UnityEngine.Object) bopItem.gameObject);
        GameManager.GetInstance().OnConversationEnd();
      }));
    });
  }
}
