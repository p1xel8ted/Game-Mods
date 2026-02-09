// Decompiled with JetBrains decompiler
// Type: Interaction_WolfTrap
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using MMTools;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_WolfTrap : Interaction
{
  public static List<Interaction_WolfTrap> Traps = new List<Interaction_WolfTrap>();
  public const float EFFECTIVE_DISTANCE = 7f;
  [SerializeField]
  public Structure structure;
  [SerializeField]
  public GameObject trapOn;
  [SerializeField]
  public GameObject trapOff;
  [SerializeField]
  public GameObject trapSet;
  [SerializeField]
  public SkeletonAnimation spine;
  [SerializeField]
  public GameObject gold;
  [SerializeField]
  public GameObject meat;
  [SerializeField]
  public GameObject followerMeat;
  [SerializeField]
  public SpriteRenderer rangeSprite;
  public Color fadeOut = new Color(1f, 1f, 1f, 0.0f);
  public float DistanceRadius = 1f;
  public int UpdateInterval = 2;
  public bool distanceChanged;
  public Vector3 updatePos;
  public List<InventoryItem> toDeposit = new List<InventoryItem>();

  public Structure Structure => this.structure;

  public override void OnEnable()
  {
    base.OnEnable();
    this.structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    if (this.structure.Brain != null)
      this.OnBrainAssigned();
    Interaction_WolfTrap.Traps.Add(this);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    this.structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    Interaction_WolfTrap.Traps.Remove(this);
  }

  public override void Update()
  {
    base.Update();
    if (Time.frameCount % this.UpdateInterval != 0 || (UnityEngine.Object) this.playerFarming == (UnityEngine.Object) null)
      return;
    if (!GameManager.overridePlayerPosition)
    {
      this.updatePos = this.playerFarming.transform.position;
      this.DistanceRadius = 1f;
    }
    else
    {
      this.updatePos = PlacementRegion.Instance.PlacementPosition;
      this.DistanceRadius = 7f;
    }
    if ((double) Vector3.Distance(this.updatePos, this.transform.position) < (double) this.DistanceRadius)
    {
      this.rangeSprite.gameObject.SetActive(true);
      this.rangeSprite.DOKill();
      this.rangeSprite.DOColor(StaticColors.OffWhiteColor, 0.5f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      this.distanceChanged = true;
    }
    else
    {
      if (!this.distanceChanged)
        return;
      this.rangeSprite.DOKill();
      this.rangeSprite.DOColor(this.fadeOut, 0.5f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true).OnComplete<TweenerCore<Color, Color, ColorOptions>>((TweenCallback) (() => this.rangeSprite.gameObject.SetActive(false)));
      this.distanceChanged = false;
    }
  }

  public void OnBrainAssigned()
  {
    this.structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    this.UpdateVisuals();
  }

  public override void GetLabel()
  {
    base.GetLabel();
    this.Interactable = true;
    if (this.structure.Brain.Data.HasBird)
      this.Label = ScriptLocalization.Interactions.OpenTrap;
    else if (this.structure.Brain.Data.Inventory.Count <= 0 && this.toDeposit.Count <= 0)
    {
      this.Label = LocalizationManager.GetTranslation("Interactions/SetTrap");
    }
    else
    {
      this.Interactable = false;
      this.Label = "";
    }
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    if ((UnityEngine.Object) this.GetComponentInParent<PlacementObject>() == (UnityEngine.Object) null)
      this.rangeSprite.DOColor(this.fadeOut, 0.0f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true).OnComplete<TweenerCore<Color, Color, ColorOptions>>((TweenCallback) (() => this.rangeSprite.gameObject.SetActive(false)));
    this.rangeSprite.size = new Vector2(7f, 7f);
  }

  public void UpdateVisuals()
  {
    this.trapSet.gameObject.SetActive(this.structure.Brain.Data.Inventory.Count > 0 && !this.structure.Brain.Data.HasBird);
    this.trapOn.gameObject.SetActive(this.structure.Brain.Data.HasBird);
    this.trapOff.gameObject.SetActive(this.structure.Brain.Data.Inventory.Count <= 0 && !this.structure.Brain.Data.HasBird);
    if (this.structure.Brain.Data.Inventory.Count <= 0)
      return;
    InventoryItem.ITEM_TYPE type = (InventoryItem.ITEM_TYPE) this.structure.Brain.Data.Inventory[0].type;
    this.gold.gameObject.SetActive(type == InventoryItem.ITEM_TYPE.GOLD_REFINED);
    this.meat.gameObject.SetActive(type == InventoryItem.ITEM_TYPE.MEAT);
    this.followerMeat.gameObject.SetActive(type == InventoryItem.ITEM_TYPE.FOLLOWER_MEAT);
    this.spine.Skeleton.SetSkin(type == InventoryItem.ITEM_TYPE.GOLD_REFINED ? "midas" : "meat");
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (this.structure.Brain.Data.HasBird)
    {
      this.structure.Brain.Data.HasBird = false;
      this.UpdateVisuals();
      for (int index = 0; index < UnityEngine.Random.Range(2, 6); ++index)
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.MEAT, 1, this.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle);
      AudioManager.Instance.PlayOneShot("event:/dlc/env/dog/trap_open", this.transform.position);
    }
    else
    {
      this.Interactable = false;
      state.CURRENT_STATE = StateMachine.State.InActive;
      state.facingAngle = Utils.GetAngle(state.transform.position, this.transform.position);
      List<InventoryItem.ITEM_TYPE> items = new List<InventoryItem.ITEM_TYPE>()
      {
        InventoryItem.ITEM_TYPE.MEAT,
        InventoryItem.ITEM_TYPE.FOLLOWER_MEAT
      };
      if (DataManager.Instance.HasMidasHiding)
      {
        Debug.Log((object) "Can add gold to the trap!");
        items.Add(InventoryItem.ITEM_TYPE.GOLD_REFINED);
      }
      UIItemSelectorOverlayController itemSelector = MonoSingleton<UIManager>.Instance.ShowItemSelector(this.playerFarming, items, new ItemSelector.Params()
      {
        Key = "trap",
        Context = ItemSelector.Context.Add,
        Offset = new Vector2(0.0f, 250f),
        RequiresDiscovery = true,
        HideOnSelection = false,
        ShowEmpty = true,
        DontCache = true
      });
      itemSelector.OnItemChosen += (System.Action<InventoryItem.ITEM_TYPE>) (chosenItem =>
      {
        Debug.Log((object) $"ItemToDeposit {chosenItem}".Colour(Color.yellow));
        InventoryItem item = new InventoryItem(chosenItem, 1);
        Inventory.ChangeItemQuantity((int) chosenItem, -1);
        AudioManager.Instance.PlayOneShot("event:/dlc/env/dog/trap_bait", this.transform.position);
        this.toDeposit.Add(item);
        ResourceCustomTarget.Create(this.gameObject, this.playerFarming.transform.position, chosenItem, (System.Action) (() =>
        {
          if (item.type == 86)
            ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.SetMidasTrap);
          this.toDeposit.Remove(item);
          this.structure.Brain.Data.Inventory.Add(item);
          this.UpdateVisuals();
        }));
        this.Interactable = false;
        itemSelector.Hide();
        this.HasChanged = true;
      });
      UIItemSelectorOverlayController overlayController = itemSelector;
      overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() =>
      {
        state.CURRENT_STATE = LetterBox.IsPlaying || MMConversation.isPlaying ? StateMachine.State.InActive : StateMachine.State.Idle;
        itemSelector = (UIItemSelectorOverlayController) null;
        this.Interactable = true;
        this.HasChanged = true;
      });
    }
  }

  public void TrappedMidas()
  {
    this.spine.Skeleton.SetSkin("midas");
    this.trapOn.gameObject.SetActive(false);
    this.trapOff.gameObject.SetActive(false);
    this.trapSet.gameObject.SetActive(false);
    AudioManager.Instance.PlayOneShot("event:/dlc/env/dog/trap_trigger", this.transform.position);
  }

  public void TrappedWolf(float delay)
  {
    this.StartCoroutine((IEnumerator) this.TrappedWolfIE(delay));
  }

  public IEnumerator TrappedWolfIE(float delay)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Interaction_WolfTrap interactionWolfTrap = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      interactionWolfTrap.structure.Brain.Data.Inventory.Clear();
      interactionWolfTrap.structure.Brain.Data.HasBird = true;
      interactionWolfTrap.spine.Skeleton.SetSkin("meat");
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    interactionWolfTrap.trapSet.gameObject.SetActive(false);
    interactionWolfTrap.trapOff.gameObject.SetActive(false);
    interactionWolfTrap.trapOn.gameObject.SetActive(true);
    interactionWolfTrap.spine.Skeleton.SetSkin("midas");
    AudioManager.Instance.PlayOneShot("event:/dlc/env/dog/trap_trigger", interactionWolfTrap.transform.position);
    AudioManager.Instance.PlayOneShot("event:/dlc/dungeon05/enemy/vocals_shared/dog_basic_small/death", interactionWolfTrap.transform.position);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(delay);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  [CompilerGenerated]
  public void \u003CUpdate\u003Eb__21_0() => this.rangeSprite.gameObject.SetActive(false);

  [CompilerGenerated]
  public void \u003COnEnableInteraction\u003Eb__24_0()
  {
    this.rangeSprite.gameObject.SetActive(false);
  }
}
