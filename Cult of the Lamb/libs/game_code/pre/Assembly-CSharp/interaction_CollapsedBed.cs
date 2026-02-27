// Decompiled with JetBrains decompiler
// Type: interaction_CollapsedBed
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System.Collections;
using UnityEngine;

#nullable disable
public class interaction_CollapsedBed : Interaction
{
  [SerializeField]
  private Interaction_Bed bed;
  private float GoldCost = 1f;
  private float WoodCost = 3f;
  private string sRepair;

  private void Start() => this.UpdateLocalisation();

  private string GetAffordColor()
  {
    return (double) Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.LOG) >= (double) this.WoodCost ? "<color=#f4ecd3>" : "<color=red>";
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sRepair = ScriptLocalization.Interactions.Repair;
  }

  public override void GetLabel()
  {
    this.Label = string.Join(" ", this.sRepair, CostFormatter.FormatCost(InventoryItem.ITEM_TYPE.LOG, (int) this.WoodCost));
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if ((double) Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.LOG) >= (double) this.WoodCost)
      this.StartCoroutine((IEnumerator) this.InteractRoutine());
    else
      MonoSingleton<Indicator>.Instance.PlayShake();
  }

  private IEnumerator InteractRoutine()
  {
    interaction_CollapsedBed interactionCollapsedBed = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone, 6f);
    yield return (object) new WaitForEndOfFrame();
    float i = interactionCollapsedBed.WoodCost;
    while ((double) --i >= 0.0)
    {
      AudioManager.Instance.PlayOneShot("event:/followers/pop_in", interactionCollapsedBed.gameObject);
      ResourceCustomTarget.Create(interactionCollapsedBed.gameObject, PlayerFarming.Instance.CameraBone.transform.position, InventoryItem.ITEM_TYPE.LOG, (System.Action) null);
      yield return (object) new WaitForSeconds((float) (0.10000000149011612 - 0.10000000149011612 * ((double) i / (double) interactionCollapsedBed.WoodCost)));
    }
    interactionCollapsedBed.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    Vector3 position = interactionCollapsedBed.transform.position;
    interactionCollapsedBed.state.facingAngle = Utils.GetAngle(interactionCollapsedBed.state.transform.position, position);
    CameraManager.instance.ShakeCameraForDuration(0.1f, 0.2f, 0.5f);
    AudioManager.Instance.PlayOneShot("event:/material/dirt_dig", position);
    yield return (object) new WaitForSeconds(1.5f);
    AudioManager.Instance.PlayOneShot("event:/building/finished_wood", position);
    GameManager.GetInstance().OnConversationEnd();
    Inventory.ChangeItemQuantity(1, (int) -(double) interactionCollapsedBed.WoodCost);
    interactionCollapsedBed.bed.StructureBrain.Rebuild();
    interactionCollapsedBed.bed.UpdateBed();
  }
}
