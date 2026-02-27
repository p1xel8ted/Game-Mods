// Decompiled with JetBrains decompiler
// Type: Interaction_FarmPlotSign
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using UnityEngine;

#nullable disable
public class Interaction_FarmPlotSign : Interaction
{
  public Structure structure;
  public InventoryItemDisplay icon;

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    this.structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
  }

  private void OnBrainAssigned()
  {
    if (this.structure.Structure_Info.SignPostItem == InventoryItem.ITEM_TYPE.NONE)
      return;
    this.icon.SetImage(this.structure.Structure_Info.SignPostItem, false);
  }

  public override void GetLabel() => this.Label = ScriptLocalization.Interactions.SetIcon;

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().AddPlayerToCamera();
    CameraFollowTarget cameraFollowTarget = CameraFollowTarget.Instance;
    cameraFollowTarget.SetOffset(new Vector3(0.0f, 2.5f, 2f));
    cameraFollowTarget.AddTarget(this.gameObject, 1f);
    state.CURRENT_STATE = StateMachine.State.CustomAction0;
    UIItemSelectorOverlayController itemSelector = MonoSingleton<UIManager>.Instance.ShowItemSelector(InventoryItem.AllPlantables, new ItemSelector.Params()
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
      state.CURRENT_STATE = StateMachine.State.Idle;
      itemSelector = (UIItemSelectorOverlayController) null;
      this.Interactable = true;
      this.HasChanged = true;
      GameManager.GetInstance().OnConversationEnd();
      cameraFollowTarget.RemoveTarget(this.gameObject);
      cameraFollowTarget.SetOffset((Vector3) Vector2.zero);
    });
  }
}
