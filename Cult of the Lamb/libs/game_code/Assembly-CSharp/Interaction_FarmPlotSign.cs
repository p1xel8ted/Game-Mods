// Decompiled with JetBrains decompiler
// Type: Interaction_FarmPlotSign
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using MMTools;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_FarmPlotSign : Interaction
{
  public Color FadeOut = new Color(1f, 1f, 1f, 0.0f);
  public SpriteRenderer RangeSprite;
  public Structure structure;
  public InventoryItemDisplay icon;
  public Vector3 _updatePos;
  public float DistanceRadius = 1f;
  public int FrameIntervalOffset;
  public int UpdateInterval = 2;
  public bool distanceChanged;

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    if ((UnityEngine.Object) this.GetComponentInParent<PlacementObject>() == (UnityEngine.Object) null)
      this.RangeSprite.DOColor(this.FadeOut, 0.0f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
    this.RangeSprite.size = new Vector2(5f, 5f);
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    this.structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
  }

  public void OnBrainAssigned()
  {
    if (this.structure.Structure_Info.SignPostItem == InventoryItem.ITEM_TYPE.NONE)
      return;
    this.icon.SetImage(this.structure.Structure_Info.SignPostItem, false);
  }

  public override void GetLabel()
  {
    this.Label = LocalizationManager.GetTranslation(ScriptLocalization.Interactions.SetIcon);
  }

  public override void Update()
  {
    base.Update();
    if ((Time.frameCount + this.FrameIntervalOffset) % this.UpdateInterval != 0 || (UnityEngine.Object) this.playerFarming == (UnityEngine.Object) null)
      return;
    if (!GameManager.overridePlayerPosition)
    {
      this._updatePos = this.playerFarming.transform.position;
      this.DistanceRadius = 1f;
    }
    else
    {
      this._updatePos = PlacementRegion.Instance.PlacementPosition;
      this.DistanceRadius = 5f;
    }
    if ((double) Vector3.Distance(this._updatePos, this.transform.position) < (double) this.DistanceRadius)
    {
      this.RangeSprite.gameObject.SetActive(true);
      this.RangeSprite.DOKill();
      this.RangeSprite.DOColor(StaticColors.OffWhiteColor, 0.5f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      this.distanceChanged = true;
    }
    else
    {
      if (!this.distanceChanged)
        return;
      this.RangeSprite.DOKill();
      this.RangeSprite.DOColor(this.FadeOut, 0.5f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      this.distanceChanged = false;
    }
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    CameraFollowTarget cameraFollowTarget = CameraFollowTarget.Instance;
    cameraFollowTarget.SetOffset(new Vector3(0.0f, 2.5f, 2f));
    cameraFollowTarget.AddTarget(this.gameObject, 1f);
    state.CURRENT_STATE = StateMachine.State.CustomAction0;
    List<InventoryItem.ITEM_TYPE> allPlantables = InventoryItem.AllPlantables;
    allPlantables.Remove(InventoryItem.ITEM_TYPE.FLOWER_WHITE);
    allPlantables.Remove(InventoryItem.ITEM_TYPE.FLOWER_PURPLE);
    if (!GameManager.AuthenticateMajorDLC())
    {
      allPlantables.Remove(InventoryItem.ITEM_TYPE.CHILLI);
      allPlantables.Remove(InventoryItem.ITEM_TYPE.SNOW_FRUIT);
    }
    UIItemSelectorOverlayController itemSelector = MonoSingleton<UIManager>.Instance.ShowItemSelector(this.playerFarming, allPlantables, new ItemSelector.Params()
    {
      Key = "farm_plot",
      Context = ItemSelector.Context.SetLabel,
      Offset = new Vector2(0.0f, 150f),
      ShowEmpty = true,
      RequiresDiscovery = false,
      HideOnSelection = true,
      HideQuantity = true
    });
    itemSelector.OnItemChosen += (System.Action<InventoryItem.ITEM_TYPE>) (chosenItem =>
    {
      this.icon.SetImage(chosenItem, false);
      this.structure.Structure_Info.SignPostItem = chosenItem;
    });
    UIItemSelectorOverlayController overlayController = itemSelector;
    overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() =>
    {
      state.CURRENT_STATE = LetterBox.IsPlaying || MMConversation.isPlaying ? StateMachine.State.InActive : StateMachine.State.Idle;
      itemSelector = (UIItemSelectorOverlayController) null;
      this.Interactable = true;
      this.HasChanged = true;
      cameraFollowTarget.RemoveTarget(this.gameObject);
      cameraFollowTarget.SetOffset((Vector3) Vector2.zero);
    });
  }
}
