// Decompiled with JetBrains decompiler
// Type: Interaction_EntranceShrine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using MMBiomeGeneration;
using UnityEngine;

#nullable disable
public class Interaction_EntranceShrine : Interaction
{
  public GameObject ParentContainer;
  public DevotionCounterOverlay devotionCounterOverlay;
  public GameObject ReceiveSoulPosition;
  public Health health;
  public SpriteXPBar XPBar;
  private string sString;
  private int SoulCount = 20;
  private int SoulMax = 20;
  public GameObject[] Dummys;
  private GameObject Player;
  private bool Activating;
  private float Delay;
  private float Distance;
  public float DistanceToTriggerDeposits = 5f;

  private void Start()
  {
    this.UpdateLocalisation();
    this.ContinuouslyHold = true;
    BiomeGenerator.OnBiomeChangeRoom += new BiomeGenerator.BiomeAction(this.OnChangeRoom);
    switch (PlayerFarming.Location)
    {
      case FollowerLocation.Dungeon1_1:
        this.SoulMax = 7;
        break;
      case FollowerLocation.Dungeon1_2:
        this.SoulMax = 14;
        break;
      case FollowerLocation.Dungeon1_3:
        this.SoulMax = 20;
        break;
      case FollowerLocation.Dungeon1_4:
        this.SoulMax = 30;
        break;
    }
    this.SoulCount = this.SoulMax;
    if (GameManager.CurrentDungeonFloor == 1 && GameManager.InitialDungeonEnter || !DataManager.Instance.HasBuiltShrine1)
      this.ParentContainer.gameObject.SetActive(false);
    else
      this.HideDummys();
  }

  public void HideDummys()
  {
    foreach (GameObject dummy in this.Dummys)
    {
      if ((bool) (UnityEngine.Object) dummy)
        dummy.gameObject.SetActive(false);
    }
  }

  public void Die()
  {
    for (int index = 0; (double) index < (double) this.SoulCount * 1.25; ++index)
    {
      if (GameManager.HasUnlockAvailable())
        SoulCustomTarget.Create(PlayerFarming.Instance.gameObject, this.ReceiveSoulPosition.transform.position, Color.white, new System.Action(this.GivePlayerSoul));
      else
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, this.transform.position + Vector3.back, 0.0f).SetInitialSpeedAndDiraction(8f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
    }
    FaithAmmo.Reload();
    this.SoulCount = 0;
    this.UpdateBar();
  }

  public override void OnEnableInteraction()
  {
    this.ActivateDistance = 3f;
    base.OnEnableInteraction();
    this.UpdateBar();
  }

  protected override void OnDestroy()
  {
    base.OnDestroy();
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.OnChangeRoom);
  }

  private void OnChangeRoom()
  {
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.OnChangeRoom);
    if (GameManager.CurrentDungeonFloor > 1 || !GameManager.InitialDungeonEnter)
      RoomLockController.RoomCompleted();
    else
      HUD_Manager.Instance.ShowTopRight();
  }

  public override void GetLabel()
  {
    if (this.SoulCount > 0)
    {
      int num = GameManager.HasUnlockAvailable() ? 1 : 0;
      string str = num != 0 ? "<sprite name=\"icon_spirits\">" : "<sprite name=\"icon_blackgold\">";
      if (num == 0)
        this.sString = ScriptLocalization.Interactions.Collect;
      this.Label = $"{this.sString} {str} x{(object) this.SoulCount}/{(object) this.SoulMax}";
    }
    else
      this.Label = "";
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.Activating)
      return;
    base.OnInteract(state);
    this.Activating = true;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sString = ScriptLocalization.Interactions.StealDevotion;
  }

  private void UpdateBar()
  {
    if ((UnityEngine.Object) this.XPBar == (UnityEngine.Object) null)
      return;
    this.XPBar.UpdateBar(Mathf.Clamp((float) this.SoulCount / (float) this.SoulMax, 0.0f, 1f));
  }

  private new void Update()
  {
    if ((UnityEngine.Object) (this.Player = GameObject.FindWithTag("Player")) == (UnityEngine.Object) null)
      return;
    this.GetLabel();
    this.Distance = Vector3.Distance(this.transform.position, this.Player.transform.position);
    if (this.Activating && (this.SoulCount <= 0 || InputManager.Gameplay.GetInteractButtonUp() || (double) this.Distance > (double) this.DistanceToTriggerDeposits))
      this.Activating = false;
    if ((double) (this.Delay -= Time.deltaTime) >= 0.0 || !this.Activating)
      return;
    if (GameManager.HasUnlockAvailable())
      SoulCustomTarget.Create(PlayerFarming.Instance.gameObject, this.ReceiveSoulPosition.transform.position, Color.white, new System.Action(this.GivePlayerSoul));
    else
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, this.transform.position + Vector3.back, 0.0f).SetInitialSpeedAndDiraction(8f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
    --this.SoulCount;
    this.Delay = 0.1f;
    this.UpdateBar();
  }

  private void GivePlayerSoul() => PlayerFarming.Instance.GetSoul(1);
}
