// Decompiled with JetBrains decompiler
// Type: MidasStealGold
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using MMTools;
using Spine.Unity;
using System.Collections;
using Unify;
using UnityEngine;

#nullable disable
public class MidasStealGold : MonoBehaviour
{
  [SerializeField]
  public SkeletonAnimation spine;
  [SerializeField]
  public SimpleSpineFlash simpleSpineFlash;
  [SerializeField]
  public Interaction_SimpleConversation[] conversations;
  [SerializeField]
  public SimpleBark bark;
  public bool goldStolen;
  public int stolenGold = -1;

  public void Awake()
  {
    this.GetComponent<Health>().OnDamaged += new Health.HealthEvent(this.MidasStealGold_OnDamaged);
    for (int index = 0; index < this.conversations.Length; ++index)
      this.conversations[index].gameObject.SetActive(DataManager.Instance.MidasSpecialEncounter == index);
    DataManager.Instance.ShowSpecialMidasRoom = false;
  }

  public void MidasStealGold_OnDamaged(
    GameObject attacker,
    Vector3 attackLocation,
    float damage,
    Health.AttackTypes attackType,
    Health.AttackFlags attackFlag)
  {
    this.simpleSpineFlash.FlashFillRed();
    if (this.stolenGold == -1)
      this.stolenGold = DataManager.Instance.MidasTotalGoldStolen;
    if (DataManager.Instance.MidasTotalGoldStolen > 0)
    {
      if (this.spine.AnimationState.GetCurrent(0).Animation.Name != "drop-money")
      {
        this.spine.AnimationState.SetAnimation(0, "drop-money", false);
        this.spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
        foreach (ConversationEntry entry in this.bark.Entries)
        {
          entry.Animation = "drop-money";
          entry.LoopAnimation = false;
        }
      }
      int num = Mathf.Min(25, DataManager.Instance.MidasTotalGoldStolen);
      DataManager.Instance.MidasTotalGoldStolen -= Mathf.RoundToInt((float) this.stolenGold / 4f);
      for (int index = 0; index < num; ++index)
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, this.transform.position, 0.0f).SetInitialSpeedAndDiraction(5f, (float) Random.Range(0, 360));
    }
    else
    {
      DataManager.Instance.MidasBeaten = true;
      AchievementsWrapper.UnlockAchievement(Achievements.Instance.Lookup("BEAT_UP_MIDAS"));
      Debug.Log((object) "ACHIEVEMENT GOT : BEAT_UP_MIDAS");
      this.spine.Skeleton.SetSkin("Beaten");
      foreach (ConversationEntry entry in this.bark.Entries)
      {
        entry.Animation = "talk";
        entry.LoopAnimation = false;
      }
    }
    if (!this.bark.gameObject.activeSelf)
    {
      this.bark.gameObject.SetActive(true);
    }
    else
    {
      if (MMConversation.CURRENT_CONVERSATION != null)
        return;
      this.bark.Show();
    }
  }

  public void StealGold()
  {
    if (this.goldStolen)
      return;
    this.goldStolen = true;
    this.StartCoroutine((IEnumerator) this.StealGoldIE());
  }

  public IEnumerator StealGoldIE()
  {
    MidasStealGold midasStealGold = this;
    DataManager.Instance.MidasSpecialEncounteredLocations.Add(PlayerFarming.Location);
    ++DataManager.Instance.MidasSpecialEncounter;
    midasStealGold.spine.AnimationState.SetAnimation(0, "steal-money", true);
    Vector3 startingPosition = midasStealGold.transform.position;
    midasStealGold.transform.DOMove(PlayerFarming.Instance.transform.position + Vector3.right * 1.5f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine);
    yield return (object) new WaitForSeconds(1f);
    yield return (object) new WaitForSeconds(0.25f);
    midasStealGold.transform.DOMove(PlayerFarming.Instance.transform.position, 0.15f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear);
    AudioManager.Instance.PlayOneShot("event:/player/gethit", midasStealGold.transform.position);
    BiomeConstants.Instance.EmitHitVFX(PlayerFarming.Instance.transform.position, Quaternion.identity.z, "HitFX_Blocked");
    PlayerFarming.Instance.simpleSpineAnimator.FlashRedTint();
    CameraManager.instance.ShakeCameraForDuration(1.5f, 1.7f, 0.2f);
    PlayerFarming.Instance.state.facingAngle = Utils.GetAngle(PlayerFarming.Instance.transform.position, midasStealGold.transform.position);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.HitThrown;
    yield return (object) new WaitForSeconds(0.15f);
    midasStealGold.transform.DOMove(PlayerFarming.Instance.transform.position + Vector3.right * 1.5f, 0.15f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    PlayerFarming.Instance.Spine.AnimationState.AddAnimation(0, "reactions/react-angry", false, 0.0f);
    PlayerFarming.Instance.Spine.AnimationState.AddAnimation(0, "idle", false, 0.0f);
    yield return (object) new WaitForSeconds(0.15f);
    int itemQuantity = Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD);
    int num = itemQuantity >= 1000 ? Mathf.CeilToInt((float) itemQuantity / 1.5f) : Mathf.CeilToInt((float) itemQuantity / 10f);
    DataManager.Instance.MidasTotalGoldStolen += num;
    Inventory.ChangeItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD, -num);
    for (int i = 0; i < 5; ++i)
    {
      PickUp pickUp = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, PlayerFarming.Instance.transform.position, 0.0f);
      pickUp.SetInitialSpeedAndDiraction(5f, (float) Random.Range(0, 360));
      pickUp.Player = midasStealGold.gameObject;
      pickUp.MagnetToPlayer = true;
      pickUp.AddToInventory = false;
      yield return (object) new WaitForSeconds(0.05f);
    }
    AudioManager.Instance.PlayOneShot("event:/dialogue/midas/standard_midas", midasStealGold.transform.position);
    AudioManager.Instance.PlayOneShot("event:/Stings/gamble_lose", midasStealGold.transform.position);
    yield return (object) new WaitForSeconds(0.5f);
    midasStealGold.spine.transform.localScale = new Vector3(-1f, 1f, 1f);
    midasStealGold.spine.AnimationState.SetAnimation(0, "run", true);
    midasStealGold.transform.DOMove(startingPosition, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear);
    yield return (object) new WaitForSeconds(0.5f);
    if (DataManager.Instance.MidasSpecialEncounter >= 3)
    {
      midasStealGold.spine.transform.localScale = new Vector3(1f, 1f, 1f);
      midasStealGold.spine.AnimationState.SetAnimation(0, "searching", true);
      midasStealGold.GetComponent<Health>().enabled = true;
    }
    else
    {
      midasStealGold.spine.AnimationState.SetAnimation(0, "exit", false);
      yield return (object) new WaitForSeconds(1f);
      midasStealGold.spine.gameObject.SetActive(false);
    }
    GameManager.GetInstance().OnConversationEnd();
  }
}
