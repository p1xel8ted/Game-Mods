// Decompiled with JetBrains decompiler
// Type: Interaction_SacrificeFollowerToGold
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using MMTools;
using Spine.Unity;
using src.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Interaction_SacrificeFollowerToGold : Interaction
{
  public List<Interaction_SimpleConversation> StatueConversations = new List<Interaction_SimpleConversation>();
  public SkeletonAnimation Spine;
  public List<GameObject> Statues = new List<GameObject>();
  private string sLabel;
  public Interaction_KeyPiece KeyPiecePrefab;

  private void Start()
  {
    this.UpdateLocalisation();
    int index = -1;
    while (++index < this.Statues.Count)
      this.Statues[index].SetActive(index < DataManager.Instance.MidasFollowerStatueCount);
    this.Spine.gameObject.SetActive(false);
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sLabel = ScriptLocalization.Interactions.SacrificeFollower;
  }

  public override void GetLabel()
  {
    if (DataManager.Instance.MidasFollowerStatueCount < 4)
      this.Label = this.sLabel;
    else
      this.Label = "";
  }

  public override void OnInteract(StateMachine state)
  {
    if (FollowerBrain.AllBrains.Count > 0)
    {
      base.OnInteract(state);
      PlayerFarming.Instance.GoToAndStop(this.transform.position + new Vector3(-1f, -2.5f), this.gameObject, GoToCallback: (System.Action) (() => this.StartCoroutine((IEnumerator) this.SacrificeFollowerRoutine())));
    }
    else
      MonoSingleton<Indicator>.Instance.PlayShake();
  }

  private IEnumerator SacrificeFollowerRoutine()
  {
    Interaction_SacrificeFollowerToGold sacrificeFollowerToGold = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(sacrificeFollowerToGold.state.gameObject);
    yield return (object) new WaitForSeconds(0.5f);
    UIFollowerSelectMenuController followerSelectInstance = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
    followerSelectInstance.Show(FollowerBrain.AllAvailableFollowerBrains(), (List<FollowerBrain>) null, false, UpgradeSystem.Type.Count, true, true, true);
    UIFollowerSelectMenuController selectMenuController1 = followerSelectInstance;
    selectMenuController1.OnFollowerSelected = selectMenuController1.OnFollowerSelected + (System.Action<FollowerInfo>) (followerInfo =>
    {
      AudioManager.Instance.PlayOneShot("event:/ritual_sacrifice/select_follower", PlayerFarming.Instance.gameObject);
      this.StartCoroutine((IEnumerator) this.SpawnFollower(followerInfo.ID));
    });
    UIFollowerSelectMenuController selectMenuController2 = followerSelectInstance;
    selectMenuController2.OnHidden = selectMenuController2.OnHidden + (System.Action) (() => followerSelectInstance = (UIFollowerSelectMenuController) null);
    UIFollowerSelectMenuController selectMenuController3 = followerSelectInstance;
    selectMenuController3.OnCancel = selectMenuController3.OnCancel + (System.Action) (() => GameManager.GetInstance().OnConversationEnd());
  }

  private IEnumerator SpawnFollower(int ID)
  {
    Interaction_SacrificeFollowerToGold sacrificeFollowerToGold = this;
    while (MMConversation.isPlaying)
      yield return (object) null;
    AudioManager.Instance.SetMusicRoomID(1, "drum_layer");
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(sacrificeFollowerToGold.state.gameObject);
    sacrificeFollowerToGold.Spine.gameObject.SetActive(true);
    while (sacrificeFollowerToGold.Spine.AnimationState == null)
      yield return (object) null;
    sacrificeFollowerToGold.Spine.AnimationState.SetAnimation(0, "enter", false);
    sacrificeFollowerToGold.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    yield return (object) new WaitForSeconds(1f);
    FollowerManager.SpawnedFollower spawnedFollower = FollowerManager.SpawnCopyFollower(FollowerManager.FindFollowerInfo(ID), sacrificeFollowerToGold.transform.position, sacrificeFollowerToGold.transform.parent, PlayerFarming.Location);
    spawnedFollower.Follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    spawnedFollower.Follower.Spine.AnimationState.SetAnimation(1, "spawn-in", false);
    spawnedFollower.Follower.Spine.AnimationState.AddAnimation(1, "Reactions/react-scared-long", false, 0.0f);
    spawnedFollower.Follower.Spine.AnimationState.AddAnimation(1, "idle-sad", true, 0.0f);
    FollowerManager.RemoveFollowerBrain(ID);
    GameManager.GetInstance().OnConversationNext(spawnedFollower.Follower.gameObject);
    yield return (object) new WaitForSeconds(2f);
    sacrificeFollowerToGold.Spine.AnimationState.SetAnimation(0, "warning", true);
    float Progress = 0.0f;
    float Duration = 3.75f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      GameManager.GetInstance().CameraSetTargetZoom(Mathf.Lerp(9f, 4f, Mathf.SmoothStep(0.0f, 1f, Progress / Duration)));
      CameraManager.shakeCamera((float) (0.30000001192092896 + 0.5 * ((double) Progress / (double) Duration)));
      yield return (object) null;
    }
    CameraManager.instance.ShakeCameraForDuration(0.5f, 0.7f, 0.3f);
    FollowerManager.CleanUpCopyFollower(spawnedFollower);
    AudioManager.Instance.PlayOneShot("event:/followers/turn_to_gold_sequence", sacrificeFollowerToGold.transform.position);
    GameObject s = sacrificeFollowerToGold.Statues[DataManager.Instance.MidasFollowerStatueCount];
    s.SetActive(true);
    s.transform.localScale = Vector3.one * 1.2f;
    s.transform.DOScale(Vector3.one, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    Vector3 TargetPosition = s.transform.position;
    s.transform.position = sacrificeFollowerToGold.transform.position;
    BiomeConstants.Instance.EmitSmokeInteractionVFX(s.transform.position, new Vector3(2f, 2f, 1f));
    sacrificeFollowerToGold.Spine.AnimationState.SetAnimation(0, "idle", true);
    GameManager.GetInstance().OnConversationNext(s, 5f);
    GameManager.GetInstance().HitStop();
    yield return (object) new WaitForSeconds(2f);
    GameManager.GetInstance().OnConversationNext(s, 8f);
    sacrificeFollowerToGold.Spine.AnimationState.SetAnimation(0, "warning", true);
    s.transform.DOMove(sacrificeFollowerToGold.transform.position + Vector3.back, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    yield return (object) new WaitForSeconds(0.5f);
    s.transform.DOMove(TargetPosition + Vector3.back, 2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    yield return (object) new WaitForSeconds(1.5f);
    s.transform.DOMove(TargetPosition, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      BiomeConstants.Instance.EmitSmokeInteractionVFX(s.transform.position, new Vector3(2f, 2f, 1f));
      CameraManager.instance.ShakeCameraForDuration(0.5f, 0.7f, 0.3f);
      this.Spine.AnimationState.SetAnimation(0, "idle", true);
    }));
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().OnConversationEnd();
    sacrificeFollowerToGold.StatueConversations[DataManager.Instance.MidasFollowerStatueCount].gameObject.SetActive(true);
    sacrificeFollowerToGold.StatueConversations[DataManager.Instance.MidasFollowerStatueCount].Callback.AddListener((UnityAction) (() =>
    {
      this.HasChanged = true;
      ++DataManager.Instance.MidasFollowerStatueCount;
      this.enabled = true;
      this.StartCoroutine((IEnumerator) this.GiveKeyPieceRoutine());
    }));
    sacrificeFollowerToGold.enabled = false;
    yield return (object) new WaitForEndOfFrame();
    HUD_Manager.Instance.Hide(true);
  }

  private IEnumerator GiveKeyPieceRoutine()
  {
    Interaction_SacrificeFollowerToGold sacrificeFollowerToGold = this;
    while (MMConversation.isPlaying)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNew(SnapLetterBox: true);
    Interaction_KeyPiece KeyPiece = UnityEngine.Object.Instantiate<Interaction_KeyPiece>(sacrificeFollowerToGold.KeyPiecePrefab, sacrificeFollowerToGold.Spine.transform.position + Vector3.back * 0.75f, Quaternion.identity, sacrificeFollowerToGold.transform.parent);
    KeyPiece.transform.localScale = Vector3.zero;
    KeyPiece.transform.DOScale(Vector3.one, 2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    GameManager.GetInstance().OnConversationNext(KeyPiece.gameObject, 6f);
    yield return (object) new WaitForSeconds(0.5f);
    yield return (object) new WaitForSeconds(1f);
    KeyPiece.transform.DOMove(PlayerFarming.Instance.transform.position + Vector3.back * 0.5f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
    yield return (object) new WaitForSeconds(1f);
    yield return (object) null;
    sacrificeFollowerToGold.Spine.AnimationState.SetAnimation(0, "exit", false);
    GameManager.GetInstance().OnConversationEnd(false);
    KeyPiece.OnInteract(PlayerFarming.Instance.state);
  }
}
