// Decompiled with JetBrains decompiler
// Type: HealingRoomSecret
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using UnityEngine;

#nullable disable
public class HealingRoomSecret : MonoBehaviour
{
  public const int Spirit = 5;
  public const int Collector = 3;
  [SerializeField]
  private Interaction_SimpleConversation wrongHeartConvo;
  [SerializeField]
  private Interaction_SimpleConversation rightHeartConvo;
  [SerializeField]
  private Interaction_SimpleConversation gaveHeartConvo;
  [SerializeField]
  private SimpleBark bark;
  [SerializeField]
  private SimpleBark barkAlternate;
  [SerializeField]
  private Transform _moveTarget;
  [SerializeField]
  private RatooCheckSkin _ratooCheckSkin;
  private bool _gotCollector;
  private bool _gotSpirit;
  private Demon_Collector demon;

  private void OnEnable()
  {
    this.CheckStatus();
    if (DataManager.Instance.EncounteredHealingRoom)
    {
      if ((bool) (UnityEngine.Object) this.barkAlternate)
        this.barkAlternate.gameObject.SetActive(DataManager.Instance.RatauKilled && (bool) (UnityEngine.Object) this.barkAlternate);
      if (!(bool) (UnityEngine.Object) this.bark)
        return;
      this.bark.gameObject.SetActive(!DataManager.Instance.RatauKilled || !(bool) (UnityEngine.Object) this.barkAlternate);
    }
    else
    {
      if ((bool) (UnityEngine.Object) this.bark)
        this.bark.gameObject.SetActive(false);
      if (!(bool) (UnityEngine.Object) this.barkAlternate)
        return;
      this.barkAlternate.gameObject.SetActive(false);
    }
  }

  private void OnDisable()
  {
    if (!(bool) (UnityEngine.Object) this.demon)
      return;
    this.demon.CanCollect = true;
  }

  private void CheckStatus()
  {
    if (DataManager.Instance.RatooGivenHeart)
      return;
    foreach (GameObject demon in Demon_Arrows.Demons)
    {
      if ((bool) (UnityEngine.Object) demon.GetComponent<Demon_Collector>())
      {
        this.demon = demon.GetComponent<Demon_Collector>();
        this.demon.CanCollect = false;
      }
    }
    if (!DataManager.Instance.EncounteredHealingRoom)
      return;
    this.UpdateStatus();
  }

  public void UpdateStatus()
  {
    if (DataManager.Instance.RatooGivenHeart)
      return;
    foreach (GameObject demon in Demon_Arrows.Demons)
    {
      if ((bool) (UnityEngine.Object) demon.GetComponent<Demon_Collector>())
      {
        this._gotCollector = true;
        break;
      }
      if ((bool) (UnityEngine.Object) demon.GetComponent<Demon_Spirit>() && !this._gotCollector)
        this._gotSpirit = true;
    }
    if (this._gotCollector && !DataManager.Instance.RatooGivenHeart)
    {
      this.rightHeartConvo.gameObject.SetActive(true);
      if ((bool) (UnityEngine.Object) this.bark)
        this.bark.gameObject.SetActive(false);
      if ((bool) (UnityEngine.Object) this.barkAlternate)
        this.barkAlternate.gameObject.SetActive(false);
    }
    if (!this._gotSpirit || this._gotCollector || DataManager.Instance.RatooMentionedWrongHeart)
      return;
    this.wrongHeartConvo.gameObject.SetActive(true);
    if ((bool) (UnityEngine.Object) this.bark)
      this.bark.gameObject.SetActive(false);
    if (!(bool) (UnityEngine.Object) this.barkAlternate)
      return;
    this.barkAlternate.gameObject.SetActive(false);
  }

  public void GiveDemon()
  {
    foreach (GameObject demon in Demon_Arrows.Demons)
    {
      Demon_Collector component = demon.GetComponent<Demon_Collector>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        this.StartCoroutine((IEnumerator) this.GiveDemonRoutine(component));
        break;
      }
    }
  }

  public void GiveReward() => this.StartCoroutine((IEnumerator) this.GiveHeartRoutine());

  private IEnumerator GiveHeartRoutine()
  {
    yield return (object) new WaitForEndOfFrame();
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject);
    bool waiting = true;
    PermanentHeart_CustomTarget.Create(this._moveTarget.transform.position, PlayerFarming.Instance.gameObject.transform.position, 2f, (System.Action) (() => waiting = false));
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.InActive;
    while (waiting)
      yield return (object) null;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.Idle;
    GameManager.GetInstance().OnConversationEnd();
    yield return (object) new WaitForSeconds(1f);
    if (DataManager.Instance.RatauKilled && (bool) (UnityEngine.Object) this.barkAlternate)
      this.barkAlternate.gameObject.SetActive(true);
    else if ((bool) (UnityEngine.Object) this.bark)
      this.bark.gameObject.SetActive(true);
  }

  private IEnumerator GiveDemonRoutine(Demon_Collector demon)
  {
    HealingRoomSecret healingRoomSecret = this;
    yield return (object) new WaitForEndOfFrame();
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(healingRoomSecret.gameObject);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    int count = 0;
    foreach (int followersDemonsType in DataManager.Instance.Followers_Demons_Types)
    {
      if (followersDemonsType == 3)
      {
        int followersDemonsId = DataManager.Instance.Followers_Demons_IDs[count];
        DataManager.Instance.Followers_Demons_Types.RemoveAt(count);
        DataManager.Instance.Followers_Demons_IDs.RemoveAt(count);
        FollowerManager.RemoveFollowerBrain(followersDemonsId);
        break;
      }
      ++count;
      yield return (object) null;
    }
    demon.enabled = false;
    yield return (object) demon.spine.state.SetAnimation(0, "action", false);
    demon.spine.transform.DOMove(healingRoomSecret._moveTarget.position + Vector3.back * 0.5f, 3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuart);
    yield return (object) new WaitForSeconds(3f);
    BiomeConstants.Instance.EmitSmokeExplosionVFX(healingRoomSecret._moveTarget.position);
    BiomeConstants.Instance.ShakeCamera();
    CameraManager.instance.ShakeCameraForDuration(0.0f, 1f, 0.5f);
    demon.gameObject.SetActive(false);
    DataManager.Instance.RatooGivenHeart = true;
    healingRoomSecret._ratooCheckSkin.CheckSkin();
    GameManager.GetInstance().OnConversationEnd();
    yield return (object) new WaitForEndOfFrame();
    healingRoomSecret.gaveHeartConvo.gameObject.SetActive(true);
    healingRoomSecret.gaveHeartConvo.OnInteract(PlayerFarming.Instance.state);
  }
}
