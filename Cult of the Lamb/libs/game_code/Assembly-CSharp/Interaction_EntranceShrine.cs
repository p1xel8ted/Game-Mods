// Decompiled with JetBrains decompiler
// Type: Interaction_EntranceShrine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MMBiomeGeneration;
using MMTools;
using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_EntranceShrine : Interaction
{
  public GameObject ParentContainer;
  public DevotionCounterOverlay devotionCounterOverlay;
  public GameObject ReceiveSoulPosition;
  public Health health;
  [SerializeField]
  public bool showInEntrance;
  public SpriteXPBar XPBar;
  public string sString;
  public int SoulCount = 20;
  public int SoulMax = 20;
  public GameObject[] Dummys;
  public GameObject Player;
  public bool Activating;
  public float Delay;
  public float Distance;
  public float DistanceToTriggerDeposits = 5f;

  public void Start()
  {
    if (DungeonSandboxManager.Active)
    {
      GameManager.GetInstance().StartCoroutine((IEnumerator) this.TimedDelay(0.5f, (System.Action) (() => RoomLockController.RoomCompleted())));
      UnityEngine.Object.Destroy((UnityEngine.Object) this.XPBar.gameObject);
      UnityEngine.Object.Destroy((UnityEngine.Object) this);
    }
    this.UpdateLocalisation();
    this.ContinuouslyHold = true;
    BiomeGenerator.OnBiomeChangeRoom += new BiomeGenerator.BiomeAction(this.OnChangeRoom);
    switch (PlayerFarming.Location)
    {
      case FollowerLocation.Dungeon1_1:
        this.SoulMax = GameManager.Layer2 ? 40 : 7;
        break;
      case FollowerLocation.Dungeon1_2:
        this.SoulMax = GameManager.Layer2 ? 45 : 14;
        break;
      case FollowerLocation.Dungeon1_3:
        this.SoulMax = GameManager.Layer2 ? 50 : 20;
        break;
      case FollowerLocation.Dungeon1_4:
        this.SoulMax = GameManager.Layer2 ? 55 : 30;
        break;
    }
    this.SoulCount = this.SoulMax;
    if (GameManager.CurrentDungeonFloor == 1 && GameManager.InitialDungeonEnter || !DataManager.Instance.HasBuiltShrine1)
    {
      if (this.showInEntrance)
      {
        this.SoulCount = 0;
        this.XPBar.gameObject.SetActive(false);
      }
      else
        this.ParentContainer.gameObject.SetActive(false);
    }
    else
      this.HideDummys();
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.StartCoroutine((IEnumerator) this.ReloadStatue());
  }

  public IEnumerator ReloadStatue()
  {
    yield return (object) new WaitForEndOfFrame();
    while (MMTransition.IsPlaying || MMConversation.isPlaying)
      yield return (object) null;
    SkeletonAnimation componentInChildren = this.health.GetComponentInChildren<SkeletonAnimation>();
    if ((UnityEngine.Object) componentInChildren != (UnityEngine.Object) null)
    {
      componentInChildren.SkeletonDataAsset.Clear();
      componentInChildren.SkeletonDataAsset.GetSkeletonData(true);
      componentInChildren.Initialize(true);
      componentInChildren.LateUpdate();
    }
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
      if ((GameManager.HasUnlockAvailable() ? 1 : (DataManager.Instance.DeathCatBeaten ? 1 : 0)) != 0)
        SoulCustomTarget.Create(this.playerFarming.gameObject, this.ReceiveSoulPosition.transform.position, Color.white, new System.Action(this.GivePlayerSoul));
      else
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, this.transform.position + Vector3.back, 0.0f).SetInitialSpeedAndDiraction(8f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
    }
    PlayerFarming.ReloadAllFaith();
    this.SoulCount = 0;
    this.UpdateBar();
  }

  public override void OnEnableInteraction()
  {
    this.ActivateDistance = 3f;
    base.OnEnableInteraction();
    this.UpdateBar();
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.OnChangeRoom);
  }

  public void OnChangeRoom()
  {
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.OnChangeRoom);
    if (GameManager.CurrentDungeonFloor > 1 || !GameManager.InitialDungeonEnter)
      RoomLockController.RoomCompleted();
    else
      HUD_Manager.Instance.ShowTopRight();
    if (BiomeGenerator.Instance.DungeonLocation != FollowerLocation.Dungeon1_5)
      return;
    this.Interactable = false;
  }

  public override void GetLabel()
  {
    if (this.SoulCount > 0 && this.Interactable)
    {
      int num = GameManager.HasUnlockAvailable() ? 1 : (DataManager.Instance.DeathCatBeaten ? 1 : 0);
      string str = num != 0 ? "<sprite name=\"icon_spirits\">" : "<sprite name=\"icon_blackgold\">";
      if (num == 0)
        this.sString = ScriptLocalization.Interactions.Collect;
      if (LocalizeIntegration.IsArabic())
        this.Label = $"{this.sString} {str} x{LocalizeIntegration.ReverseText(this.SoulMax.ToString())}/{LocalizeIntegration.ReverseText(this.SoulCount.ToString())}";
      else
        this.Label = $"{this.sString} {str} x{this.SoulCount.ToString()}/{this.SoulMax.ToString()}";
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

  public override void IndicateHighlighted(PlayerFarming playerFarming = null)
  {
    base.IndicateHighlighted(playerFarming);
    if (!(bool) (UnityEngine.Object) playerFarming || PlayerFarming.Location != FollowerLocation.Dungeon1_5)
      return;
    playerFarming.indicator.ShowTopInfo(LocalizationManager.GetTranslation("Conversation_NPC/WolfStatue"));
  }

  public override void EndIndicateHighlightedUpdate()
  {
    base.EndIndicateHighlightedUpdate();
    this.playerFarming.indicator.HideTopInfo();
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sString = ScriptLocalization.Interactions.StealDevotion;
  }

  public void UpdateBar()
  {
    if ((UnityEngine.Object) this.XPBar == (UnityEngine.Object) null)
      return;
    this.XPBar.UpdateBar(Mathf.Clamp((float) this.SoulCount / (float) this.SoulMax, 0.0f, 1f));
  }

  public override void Update()
  {
    base.Update();
    if ((UnityEngine.Object) (this.Player = GameObject.FindWithTag("Player")) == (UnityEngine.Object) null)
      return;
    this.GetLabel();
    this.Distance = Vector3.Distance(this.transform.position, this.Player.transform.position);
    if (this.Activating && (this.SoulCount <= 0 || InputManager.Gameplay.GetInteractButtonUp() || (double) this.Distance > (double) this.DistanceToTriggerDeposits))
      this.Activating = false;
    if ((double) (this.Delay -= Time.deltaTime) >= 0.0 || !this.Activating)
      return;
    if ((GameManager.HasUnlockAvailable() ? 1 : (DataManager.Instance.DeathCatBeaten ? 1 : 0)) != 0)
      SoulCustomTarget.Create(this.playerFarming.gameObject, this.ReceiveSoulPosition.transform.position, Color.white, new System.Action(this.GivePlayerSoul));
    else
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, this.transform.position + Vector3.back, 0.0f).SetInitialSpeedAndDiraction(8f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
    --this.SoulCount;
    this.Delay = 0.1f;
    this.UpdateBar();
  }

  public IEnumerator TimedDelay(float delay, System.Action callback)
  {
    yield return (object) new WaitForSeconds(delay);
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void GivePlayerSoul() => this.playerFarming.GetSoul(1);

  public void SetInteractable()
  {
    this.Interactable = true;
    this.UpdateBar();
  }

  public void DisableInteraction()
  {
    this.Interactable = false;
    this.enabled = false;
  }
}
