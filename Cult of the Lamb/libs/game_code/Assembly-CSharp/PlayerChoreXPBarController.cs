// Decompiled with JetBrains decompiler
// Type: PlayerChoreXPBarController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class PlayerChoreXPBarController : MonoBehaviour
{
  public GameObject BarContainer;
  public BarControllerNonUI BarController;
  public DG.Tweening.Sequence Sequence;
  [HideInInspector]
  public PlayerFarming playerFarming;

  public void AddChoreXP(PlayerFarming playerFarming, float multiplier = 1f)
  {
    this.Play(playerFarming, multiplier);
  }

  public void OnEnable() => this.Init();

  public void Init()
  {
    this.BarContainer.gameObject.SetActive(false);
    if (!this.playerFarming.isLamb)
    {
      this.RestoreTempChoreXP();
      Debug.Log((object) $"CHORE SET COOP CHORE: {DataManager.Instance.ChoreXP.ToString()}XP {DataManager.Instance.ChoreXPLevel.ToString()}LVL{Time.time.ToString()}");
      DataManager.Instance.ChoreXPLevel_Coop = DataManager.Instance.ChoreXPLevel;
      DataManager.Instance.ChoreXP_Coop = DataManager.Instance.ChoreXP;
    }
    else
    {
      if (!((UnityEngine.Object) CoopManager.Instance != (UnityEngine.Object) null))
        return;
      CoopManager.Instance.OnPlayerLeft += new System.Action(this.RestoreTempChoreXP);
    }
  }

  public void RestoreTempChoreXP()
  {
    if ((double) DataManager.Instance.ChoreXP_Coop_Temp_Gained <= 0.0)
      return;
    Debug.Log((object) ("CHORE ChoreXP_Coop_Temp_Gained: " + DataManager.Instance.ChoreXP_Coop_Temp_Gained.ToString()));
    DataManager.Instance.ChoreXP += DataManager.Instance.ChoreXP_Coop_Temp_Gained;
    DataManager.Instance.ChoreXP_Coop_Temp_Gained = 0.0f;
    while ((double) this.CurrentChoreXP > (double) this.TargetChoreXP)
    {
      this.CurrentChoreXP -= this.TargetChoreXP;
      ++DataManager.Instance.ChoreXPLevel;
    }
    PlayerFarming.players[0].SetSkin();
  }

  public void OnPlayerLeft()
  {
    if (!this.playerFarming.isLamb)
      return;
    this.RestoreTempChoreXP();
  }

  public float CurrentChoreXP
  {
    get
    {
      return this.playerFarming.isLamb ? DataManager.Instance.ChoreXP : DataManager.Instance.ChoreXP_Coop;
    }
    set
    {
      if (this.playerFarming.isLamb)
        DataManager.Instance.ChoreXP = value;
      else
        DataManager.Instance.ChoreXP_Coop = value;
    }
  }

  public float TargetChoreXP
  {
    get
    {
      return this.playerFarming.isLamb ? DataManager.TargetChoreXP[Mathf.Min(DataManager.Instance.ChoreXPLevel, DataManager.TargetChoreXP.Count - 1)] : DataManager.TargetChoreXP[Mathf.Min(DataManager.Instance.ChoreXPLevel_Coop, DataManager.TargetChoreXP.Count - 1)];
    }
  }

  public void Start()
  {
    this.transform.localScale = Vector3.zero;
    this.BarController.SetBarSize(this.CurrentChoreXP / this.TargetChoreXP, false);
  }

  public void OnDestroy()
  {
    if (this.Sequence != null && this.Sequence.active)
    {
      this.Sequence.Kill();
      this.Sequence = (DG.Tweening.Sequence) null;
    }
    CoopManager.Instance.OnPlayerLeft -= new System.Action(this.RestoreTempChoreXP);
    this.transform.DOKill();
  }

  public void Play(PlayerFarming playerFarming, float multiplier = 1f)
  {
    if (playerFarming.isLamb)
    {
      if (DataManager.Instance.ChoreXPLevel >= 9)
        return;
    }
    else if (DataManager.Instance.ChoreXPLevel_Coop >= 9)
      return;
    this.EnableBarContainerGameobject();
    this.transform.DOKill();
    this.BarController.SetBarSize(this.CurrentChoreXP / this.TargetChoreXP, false);
    this.transform.localScale = Vector3.zero;
    if (this.Sequence != null && this.Sequence.active)
      this.Sequence.Kill();
    this.Sequence = DOTween.Sequence();
    this.Sequence.Append((Tween) this.transform.DOScale(Vector3.one * 0.7f, 0.3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true));
    this.Sequence.AppendCallback((TweenCallback) (() =>
    {
      this.CurrentChoreXP += 1f * multiplier;
      if (!playerFarming.isLamb)
        DataManager.Instance.ChoreXP_Coop_Temp_Gained += 1f * multiplier;
      this.BarController.SetBarSize(this.CurrentChoreXP / this.TargetChoreXP, true);
    }));
    if ((double) this.CurrentChoreXP + 1.0 * (double) multiplier >= (double) this.TargetChoreXP)
    {
      this.StartCoroutine((IEnumerator) this.LevelUpChores(playerFarming));
      this.Sequence.AppendInterval(1f);
    }
    else
      this.Sequence.AppendInterval(1.2f);
    this.Sequence.Append((Tween) this.transform.DOScale(Vector3.zero, 0.3f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.BarContainer.gameObject.SetActive(false))));
  }

  public IEnumerator LevelUpChores(PlayerFarming playerFarming)
  {
    PlayerChoreXPBarController choreXpBarController = this;
    yield return (object) null;
    while (playerFarming.state.CURRENT_STATE == StateMachine.State.CustomAnimation)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNew(false);
    GameManager.GetInstance().OnConversationNext(playerFarming.state.gameObject, 4f);
    PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive, PlayerNotToInclude: playerFarming);
    yield return (object) null;
    AudioManager.Instance.PlayOneShot("event:/ui/broom_in_spin", choreXpBarController.transform.position);
    playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    playerFarming.simpleSpineAnimator.Animate("Mop/upgrade-mop-start", 0, false);
    playerFarming.simpleSpineAnimator.AddAnimate("Mop/upgrade-mop-loop", 0, true, 0.0f);
    playerFarming.simpleSpineAnimator.OnSpineEvent += new SimpleSpineAnimator.SpineEvent(choreXpBarController.OnChangeMop);
    yield return (object) new WaitForSeconds(3f);
    AudioManager.Instance.PlayOneShot("event:/ui/broom_away_spin", choreXpBarController.transform.position);
    yield return (object) new WaitForSeconds(playerFarming.simpleSpineAnimator.Animate("Mop/upgrade-mop-stop", 0, false).Animation.Duration - 0.1f);
    PlayerFarming.SetStateForAllPlayers();
    GameManager.GetInstance().OnConversationEnd();
    choreXpBarController.transform.DOScale(Vector3.zero, 0.3f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(new TweenCallback(choreXpBarController.\u003CLevelUpChores\u003Eb__17_0));
  }

  public void EnableBarContainerGameobject()
  {
    if ((UnityEngine.Object) this.BarContainer != (UnityEngine.Object) null)
      this.BarContainer.gameObject.SetActive(true);
    FaithCanvasOptimization componentInParent = this.GetComponentInParent<FaithCanvasOptimization>();
    if (!((UnityEngine.Object) componentInParent != (UnityEngine.Object) null))
      return;
    componentInParent.ActivateCanvas();
  }

  public void OnChangeMop(string EventName)
  {
    if (!(EventName == "MopSwap"))
      return;
    if (this.playerFarming.isLamb)
    {
      ++DataManager.Instance.ChoreXPLevel;
      DataManager.Instance.ChoreXP = 0.0f;
    }
    else
    {
      ++DataManager.Instance.ChoreXPLevel_Coop;
      DataManager.Instance.ChoreXP_Coop = 0.0f;
    }
    this.playerFarming.SetSkin();
    this.playerFarming.simpleSpineAnimator.OnSpineEvent -= new SimpleSpineAnimator.SpineEvent(this.OnChangeMop);
  }

  [CompilerGenerated]
  public void \u003CLevelUpChores\u003Eb__17_0() => this.BarContainer.gameObject.SetActive(false);
}
