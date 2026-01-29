// Decompiled with JetBrains decompiler
// Type: Interaction_IndulgenceShop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using I2.Loc;
using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_IndulgenceShop : Interaction
{
  public bool ForceSpecificSkin;
  [SpineSkin("", "", true, false, false)]
  public string ForceSkin = "";
  public GameObject ShopKeeper;
  public SkeletonAnimation ShopKeeperSpine;
  public ParticleSystem indulgenceParticles;
  public int Cost;
  public EventInstance receiveLoop;
  public System.Action BoughtIndulgenceCallback;
  public System.Action FollowerCreated;
  public string skin;
  public Transform playerMovePos;
  public bool IsDungeon;
  public IndulgenceDecoration[] indulgenceDecorations;
  public IndulgenceDecoration currentIndulgenceDecoration;
  public string saleText;
  public string off;
  public bool Activated;
  public GameObject Player;
  public StateMachine CompanionState;
  public GameObject ConversionBone;
  [SerializeField]
  public GameObject normalBark;
  [SerializeField]
  public GameObject buyBark;
  [SerializeField]
  public GameObject cantAffordBark;

  public void Start() => this.UpdateLocalisation();

  public override void OnEnableInteraction() => base.OnEnableInteraction();

  public int GetCost()
  {
    int cost = this.currentIndulgenceDecoration.cost;
    if (this.currentIndulgenceDecoration.isFree)
      cost = 0;
    return cost;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.off = ScriptLocalization.UI_Generic.Off;
  }

  public override void GetLabel()
  {
    this.currentIndulgenceDecoration = this.indulgenceDecorations[DataManager.Instance.TempleLevel];
    this.saleText = !this.currentIndulgenceDecoration.isFree ? (string) null : string.Format(this.off, (object) 100);
    string str1 = LocalizationManager.GetTranslation(this.currentIndulgenceDecoration.nameString).ToString();
    string str2 = string.Empty;
    if (!this.Activated)
    {
      str2 = string.Format(ScriptLocalization.UI_ItemSelector_Context.Buy, (object) str1, (object) CostFormatter.FormatCost(InventoryItem.ITEM_TYPE.BLACK_GOLD, this.GetCost()));
      if (!string.IsNullOrEmpty(this.saleText))
        str2 = string.Join(" ", str2, this.saleText.Colour(Color.yellow));
    }
    this.Label = str2;
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (Inventory.GetItemQuantity(20) >= this.GetCost() || CheatConsole.BuildingsFree)
      this.StartCoroutine((IEnumerator) this.Purchase());
    else
      this.ShopkeeperAnimationCantAfford();
  }

  public string GetAffordColor()
  {
    return this.GetCost() == 0 || Inventory.GetItemQuantity(20) >= this.GetCost() ? "<color=#f4ecd3>" : "<color=red>";
  }

  public IEnumerator Purchase()
  {
    Interaction_IndulgenceShop interactionIndulgenceShop = this;
    interactionIndulgenceShop.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    interactionIndulgenceShop.playerFarming.GoToAndStop(interactionIndulgenceShop.playerMovePos.position, interactionIndulgenceShop.transform.gameObject);
    while (interactionIndulgenceShop.playerFarming.GoToAndStopping)
      yield return (object) null;
    AudioManager.Instance.PlayOneShot("event:/shop/buy", interactionIndulgenceShop.playerFarming.transform.position);
    interactionIndulgenceShop.ShopKeeperSpine.AnimationState.SetAnimation(0, "buy", false);
    interactionIndulgenceShop.ShopKeeperSpine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    if (interactionIndulgenceShop.GetCost() != 0)
    {
      for (int i = 0; i < interactionIndulgenceShop.Cost; ++i)
      {
        ResourceCustomTarget.Create(interactionIndulgenceShop.ShopKeeper, interactionIndulgenceShop.playerFarming.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, (System.Action) null, interactionIndulgenceShop.transform);
        yield return (object) new WaitForSeconds(0.1f);
      }
      Inventory.ChangeItemQuantity(20, -interactionIndulgenceShop.GetCost());
    }
    interactionIndulgenceShop.currentIndulgenceDecoration.purchaseGameObjectToActivate.SetActive(true);
    interactionIndulgenceShop.BoughtIndulgenceCallback();
    interactionIndulgenceShop.StartCoroutine((IEnumerator) interactionIndulgenceShop.BoughtIndulgence());
    interactionIndulgenceShop.Activated = true;
    yield return (object) new WaitForSeconds(2f);
    Debug.Log((object) "TIME TO TRY AGAIN!!!!!");
    interactionIndulgenceShop.Activated = false;
  }

  public override void Update() => base.Update();

  public void ShopkeeperAnimationBoughtItem() => this.buyBark.SetActive(true);

  public void ShopkeeperAnimationCantAfford()
  {
    AudioManager.Instance.PlayOneShot("event:/ui/negative_feedback", this.playerFarming.transform.position);
    this.playerFarming.indicator.PlayShake();
    AudioManager.Instance.PlayOneShot("event:/dialogue/midas/standard_midas", AudioManager.Instance.Listener.transform.position);
    this.cantAffordBark.SetActive(true);
    this.buyBark.SetActive(false);
    this.ShopKeeperSpine.AnimationState.SetAnimation(0, "idle", false);
  }

  public IEnumerator BoughtIndulgence()
  {
    Interaction_IndulgenceShop interactionIndulgenceShop = this;
    interactionIndulgenceShop.ShopkeeperAnimationBoughtItem();
    interactionIndulgenceShop.Interactable = false;
    HealthPlayer h = interactionIndulgenceShop.state.GetComponent<HealthPlayer>();
    h.untouchable = true;
    interactionIndulgenceShop.state.GetComponentInChildren<SimpleSpineAnimator>();
    AudioManager.Instance.PlayOneShot("event:/dialogue/midas_statues/laugh_midas_statues", AudioManager.Instance.Listener.transform.position);
    CameraManager.shakeCamera(0.5f, (float) UnityEngine.Random.Range(0, 360));
    yield return (object) new WaitForSeconds(interactionIndulgenceShop.ShopKeeperSpine.AnimationState.SetAnimation(0, "steal-money", false).Animation.Duration);
    interactionIndulgenceShop.ShopKeeperSpine.AnimationState.SetAnimation(0, "idle", false);
    interactionIndulgenceShop.indulgenceParticles.Play();
    interactionIndulgenceShop.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    CameraManager.shakeCamera(0.5f, (float) UnityEngine.Random.Range(0, 360));
    int num = (int) interactionIndulgenceShop.receiveLoop.stop(STOP_MODE.ALLOWFADEOUT);
    if ((UnityEngine.Object) BiomeBaseManager.Instance != (UnityEngine.Object) null && BiomeBaseManager.Instance.SpawnExistingRecruits && DataManager.Instance.Followers_Recruit.Count <= 0)
      BiomeBaseManager.Instance.SpawnExistingRecruits = false;
    FollowerThoughts.GetData((double) UnityEngine.Random.value >= 0.699999988079071 ? ((double) UnityEngine.Random.value > 0.30000001192092896 ? Thought.InstantBelieverRescued : Thought.ResentfulRescued) : Thought.GratefulRecued).Init();
    GameManager.GetInstance().OnConversationEnd();
    yield return (object) new WaitForSeconds(0.75f);
    h.untouchable = false;
  }
}
