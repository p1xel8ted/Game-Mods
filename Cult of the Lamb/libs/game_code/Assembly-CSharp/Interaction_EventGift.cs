// Decompiled with JetBrains decompiler
// Type: Interaction_EventGift
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.BuildMenu;
using Spine.Unity;
using src.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class Interaction_EventGift : Interaction
{
  public static List<Interaction_EventGift> Gifts = new List<Interaction_EventGift>();
  [SerializeField]
  public SeasonalEventType seasonalEventType;
  [SerializeField]
  public SkeletonAnimation spine;
  public SeasonalEventData data;

  public void Awake()
  {
    this.data = SeasonalEventManager.GetSeasonalEventData(this.seasonalEventType);
    Interaction_EventGift.Gifts.Add(this);
  }

  public void Start()
  {
    this.gameObject.SetActive(SeasonalEventManager.IsSeasonalEventActive(this.seasonalEventType) && !this.HasUnlockedAllItems());
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    Interaction_EventGift.Gifts.Remove(this);
  }

  public bool HasUnlockedAllItems()
  {
    foreach (StructureBrain.TYPES decoration in this.data.Decorations)
    {
      if (!StructuresData.HasRevealed(decoration))
        return false;
    }
    foreach (string skin in this.data.Skins)
    {
      if (!DataManager.Instance.FollowerSkinsUnlocked.Contains(skin))
        return false;
    }
    return true;
  }

  public override void GetLabel()
  {
    base.GetLabel();
    this.Label = ScriptLocalization.Interactions.OpenChest;
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.StartCoroutine((IEnumerator) this.OpenIE());
  }

  public IEnumerator OpenIE()
  {
    Interaction_EventGift interactionEventGift = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionEventGift.gameObject, 6f);
    interactionEventGift.playerFarming.GoToAndStop(interactionEventGift.transform.position + Vector3.right * 2f, interactionEventGift.gameObject);
    yield return (object) new WaitForSeconds(1f);
    interactionEventGift.spine.AnimationState.SetAnimation(0, "open", false);
    AudioManager.Instance.PlayOneShot("event:/chests/chest_big_open", interactionEventGift.gameObject);
    yield return (object) new WaitForSeconds(0.25f);
    AudioManager.Instance.PlayOneShot("event:/player/new_item_reveal", interactionEventGift.gameObject);
    BiomeConstants.Instance.EmitSmokeExplosionVFX(interactionEventGift.transform.position - Vector3.forward * 1.5f);
    BiomeConstants.Instance.EmitConfettiVFX(interactionEventGift.transform.position - Vector3.forward * 1.5f);
    yield return (object) new WaitForSeconds(0.25f);
    List<GameObject> decos = new List<GameObject>();
    List<GameObject> skinos = new List<GameObject>();
    int i;
    for (i = 0; i < interactionEventGift.data.Decorations.Length; ++i)
    {
      AudioManager.Instance.PlayOneShot("event:/chests/chest_item_spawn", interactionEventGift.transform.position);
      FoundItemPickUp component = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION, 1, interactionEventGift.transform.position).GetComponent<FoundItemPickUp>();
      component.DecorationType = interactionEventGift.data.Decorations[i];
      decos.Add(component.gameObject);
      yield return (object) new WaitForSeconds(0.1f);
    }
    for (i = 0; i < interactionEventGift.data.Skins.Length; ++i)
    {
      if (!DataManager.Instance.FollowerSkinsUnlocked.Contains(interactionEventGift.data.Skins[i]))
      {
        AudioManager.Instance.PlayOneShot("event:/chests/chest_item_spawn", interactionEventGift.transform.position);
        FoundItemPickUp component = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN, 1, interactionEventGift.transform.position).GetComponent<FoundItemPickUp>();
        component.DecorationType = interactionEventGift.data.Decorations[i];
        skinos.Add(component.gameObject);
        yield return (object) new WaitForSeconds(0.1f);
      }
    }
    yield return (object) new WaitForSeconds(1f);
    foreach (GameObject gameObject in decos)
    {
      GameObject deco = gameObject;
      deco.transform.DOMove(interactionEventGift.playerFarming.transform.position, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => UnityEngine.Object.Destroy((UnityEngine.Object) deco.gameObject)));
    }
    yield return (object) new WaitForSeconds(0.5f);
    for (int index = 0; index < interactionEventGift.data.Decorations.Length; ++index)
    {
      StructuresData.CompleteResearch(interactionEventGift.data.Decorations[index]);
      StructuresData.SetRevealed(interactionEventGift.data.Decorations[index]);
    }
    bool wait = true;
    if (interactionEventGift.data.Decorations.Length != 0)
    {
      Time.timeScale = 0.0f;
      UIBuildMenuController buildMenuController = MonoSingleton<UIManager>.Instance.BuildMenuTemplate.Instantiate<UIBuildMenuController>();
      buildMenuController.Show(((IEnumerable<StructureBrain.TYPES>) interactionEventGift.data.Decorations).ToList<StructureBrain.TYPES>());
      UIBuildMenuController buildMenuController1 = buildMenuController;
      buildMenuController1.OnHidden = buildMenuController1.OnHidden + (System.Action) (() =>
      {
        wait = false;
        buildMenuController = (UIBuildMenuController) null;
      });
      while (wait)
        yield return (object) null;
    }
    Time.timeScale = 1f;
    foreach (GameObject gameObject in skinos)
    {
      GameObject deco = gameObject;
      deco.transform.DOMove(interactionEventGift.playerFarming.transform.position, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => UnityEngine.Object.Destroy((UnityEngine.Object) deco.gameObject)));
    }
    yield return (object) new WaitForSeconds(0.5f);
    if (interactionEventGift.data.Skins.Length != 0)
    {
      for (i = 0; i < interactionEventGift.data.Skins.Length; ++i)
      {
        if (!DataManager.Instance.FollowerSkinsUnlocked.Contains(interactionEventGift.data.Skins[i]))
        {
          wait = true;
          UINewItemOverlayController overlayController = MonoSingleton<UIManager>.Instance.ShowNewItemOverlay();
          overlayController.Show(UINewItemOverlayController.TypeOfCard.FollowerSkin, interactionEventGift.transform.position, interactionEventGift.data.Skins[i]);
          overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() => wait = false);
          while (wait)
            yield return (object) null;
        }
      }
    }
    GameManager.GetInstance().OnConversationEnd();
    UnityEngine.Object.Destroy((UnityEngine.Object) interactionEventGift.gameObject);
  }

  public static Interaction_EventGift GetGift(SeasonalEventType eventType)
  {
    foreach (Interaction_EventGift gift in Interaction_EventGift.Gifts)
    {
      if (gift.seasonalEventType == eventType)
        return gift;
    }
    return (Interaction_EventGift) null;
  }
}
