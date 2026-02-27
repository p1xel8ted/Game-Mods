// Decompiled with JetBrains decompiler
// Type: Interaction_ChestBossRoom
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_ChestBossRoom : Interaction
{
  public bool StartRevealed;
  public InventoryItem.ITEM_TYPE ItemType = InventoryItem.ITEM_TYPE.LOG;
  public InventoryItemDisplay Item;
  public GameObject PlayerPosition;
  public SkeletonAnimation Spine;
  private string sLabel;
  private float Timer;
  private Interaction_ChestBossRoom.State MyState;
  public int MaxToGive = 12;
  public int MinToGive = 8;
  public static Interaction_Chest.ChestEvent OnChestRevealed;
  private InventoryItem.ITEM_TYPE Reward;
  private HealthPlayer healthPlayer;
  public static int RewardCount = -1;
  public static int RevealCount = -1;
  private bool Loop;

  private new void Update()
  {
    if (this.MyState != Interaction_ChestBossRoom.State.Closed)
      return;
    this.Timer += Time.deltaTime;
  }

  private void Start()
  {
    this.Item.gameObject.SetActive(false);
    this.UpdateLocalisation();
    if (this.MyState == Interaction_ChestBossRoom.State.Hidden)
      this.Spine.gameObject.SetActive(false);
    else
      this.Spine.AnimationState.SetAnimation(0, "closed", true);
    if (!this.StartRevealed)
      return;
    this.Reveal();
  }

  public void Reveal()
  {
    this.Spine.gameObject.SetActive(true);
    this.Spine.AnimationState.SetAnimation(0, "reveal", true);
    this.Spine.AnimationState.AddAnimation(0, "closed", true, 0.0f);
    this.MyState = Interaction_ChestBossRoom.State.Closed;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sLabel = ScriptLocalization.Interactions.OpenChest;
  }

  public override void GetLabel()
  {
    this.Label = this.MyState != Interaction_ChestBossRoom.State.Closed || (double) this.Timer <= 0.5 ? "" : this.sLabel;
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.MyState != Interaction_ChestBossRoom.State.Closed)
      return;
    base.OnInteract(state);
    this.StartCoroutine((IEnumerator) this.InteractionRoutine());
  }

  private IEnumerator InteractionRoutine()
  {
    Interaction_ChestBossRoom interactionChestBossRoom = this;
    interactionChestBossRoom.Loop = false;
    GameManager.GetInstance().OnConversationNew(false);
    GameManager.GetInstance().OnConversationNext(interactionChestBossRoom.state.gameObject, 6f);
    PlayerFarming.Instance.GoToAndStop(interactionChestBossRoom.PlayerPosition, interactionChestBossRoom.gameObject);
    while (PlayerFarming.Instance.GoToAndStopping)
      yield return (object) null;
    yield return (object) new WaitForSeconds(0.2f);
    interactionChestBossRoom.Spine.AnimationState.SetAnimation(0, "open", false);
    interactionChestBossRoom.Spine.AnimationState.AddAnimation(0, "opened", true, 0.0f);
    interactionChestBossRoom.MyState = Interaction_ChestBossRoom.State.Open;
    for (int i = 0; i < Random.Range(interactionChestBossRoom.MinToGive, interactionChestBossRoom.MaxToGive); ++i)
    {
      InventoryItem.Spawn(interactionChestBossRoom.ItemType, 1, interactionChestBossRoom.transform.position);
      yield return (object) new WaitForSeconds(0.1f);
    }
    InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, interactionChestBossRoom.transform.position);
    GameManager.GetInstance().OnConversationEnd();
    GameManager.GetInstance().CameraResetTargetZoom();
    interactionChestBossRoom.state.CURRENT_STATE = StateMachine.State.Idle;
    Interaction_Chest.ChestEvent onChestRevealed = Interaction_ChestBossRoom.OnChestRevealed;
    if (onChestRevealed != null)
      onChestRevealed();
  }

  public enum State
  {
    Hidden,
    Closed,
    Open,
  }
}
