// Decompiled with JetBrains decompiler
// Type: Interaction_ReapSouls
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using Spine.Unity;
using src.Extensions;
using src.UINavigator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_ReapSouls : Interaction
{
  public const int SOULS_REQUIRED = 3;
  [SerializeField]
  public SkeletonAnimation[] spines;
  [SerializeField]
  public GameObject blade;
  [SerializeField]
  public GameObject cameraPosition;
  [SerializeField]
  public GameObject sinPrefab;
  [SerializeField]
  public GameObject symbols;
  [SerializeField]
  public GameObject sinPosition;
  [SerializeField]
  public GameObject playerPosition;
  [SerializeField]
  public GameObject groundPosition;
  [SerializeField]
  public MeshRenderer meshVFX;
  public Material _meshMaterial;
  public List<Transform> headPos = new List<Transform>();
  public List<Vector3> headPosStart = new List<Vector3>();
  public Vector3 bladeStartingPosition;
  public string SRequires;
  public string SReap;
  public string SDeadFollowers;
  public string _pitchParameter = "follower_pitch";
  public string _vibratoParameter = "follower_vibrato";

  public void Start()
  {
    this.bladeStartingPosition = this.blade.transform.localPosition;
    foreach (Component spine in this.spines)
      spine.gameObject.SetActive(false);
    this._meshMaterial = this.meshVFX.material;
    this._meshMaterial.DOColor(new Color(1f, 1f, 1f, 0.0f), "_Color", 0.0f);
    for (int index = 0; index < this.spines.Length - 1; ++index)
      this.headPosStart[index] = this.spines[index].transform.position;
    this.headPos.Shuffle<Transform>();
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.SRequires = ScriptLocalization.Interactions.Requires;
    this.SReap = LocalizationManager.GetTranslation("Interactions/ReapSouls");
    this.SDeadFollowers = LocalizationManager.GetTranslation("UI/DeadFollowers");
  }

  public override void GetLabel()
  {
    base.GetLabel();
    string str = DataManager.Instance.Followers_Dead.Count >= 3 ? "<color=green> " : "<color=red> ";
    if (LocalizeIntegration.IsArabic())
      this.Label = $"{this.SReap} | {this.SDeadFollowers} {3.ToString()} / {str}{DataManager.Instance.Followers_Dead.Count.ToString()}</color>";
    else
      this.Label = $"{this.SReap} | {this.SDeadFollowers} {str}{DataManager.Instance.Followers_Dead.Count.ToString()}</color> / {3.ToString()}";
  }

  public override void OnInteract(StateMachine state)
  {
    if (DataManager.Instance.Followers_Dead.Count < 3)
    {
      this._playerFarming.indicator.PlayShake();
    }
    else
    {
      base.OnInteract(state);
      GameManager.GetInstance().OnConversationNew();
      Time.timeScale = 0.0f;
      List<FollowerSelectEntry> followerSelectEntries = new List<FollowerSelectEntry>();
      foreach (FollowerInfo followerInfo in DataManager.Instance.Followers_Dead)
        followerSelectEntries.Add(new FollowerSelectEntry(followerInfo));
      UIReapSoulsMenuController followerSelectInstance = MonoSingleton<UIManager>.Instance.ReapSoulsMenuTemplate.Instantiate<UIReapSoulsMenuController>();
      followerSelectInstance.VotingType = TwitchVoting.VotingType.REAP_SOULS;
      MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer = this.playerFarming;
      followerSelectInstance.Show(followerSelectEntries, false, UpgradeSystem.Type.Count, false, true, true, false, true);
      UIReapSoulsMenuController soulsMenuController1 = followerSelectInstance;
      soulsMenuController1.OnFollowerSelected = soulsMenuController1.OnFollowerSelected + (System.Action<FollowerInfo>) (info => AudioManager.Instance.PlayOneShotAndSetParametersValue("event:/dialogue/followers/enlightened", this._pitchParameter, info.follower_pitch, this._vibratoParameter, info.follower_vibrato, PlayerFarming.Instance.transform));
      followerSelectInstance.OnFollowersChosen += (System.Action<List<FollowerInfo>>) (followers =>
      {
        Time.timeScale = 1f;
        this.StartCoroutine((IEnumerator) this.ReapSoul(followers));
      });
      UIReapSoulsMenuController soulsMenuController2 = followerSelectInstance;
      soulsMenuController2.OnCancel = soulsMenuController2.OnCancel + (System.Action) (() =>
      {
        GameManager.GetInstance().OnConversationEnd();
        Time.timeScale = 1f;
      });
      UIReapSoulsMenuController soulsMenuController3 = followerSelectInstance;
      soulsMenuController3.OnHidden = soulsMenuController3.OnHidden + (System.Action) (() => followerSelectInstance = (UIReapSoulsMenuController) null);
    }
  }

  public IEnumerator ReapSoul(List<FollowerInfo> followerInfos)
  {
    Interaction_ReapSouls interactionReapSouls = this;
    PlayerFarming.Instance.GoToAndStop(interactionReapSouls.playerPosition.transform.position, interactionReapSouls.sinPosition.gameObject);
    GameManager.GetInstance().OnConversationNext(interactionReapSouls.spines[0].gameObject, 4f);
    yield return (object) new WaitForSeconds(1f);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "rituals/sin-onboarding-start", true);
    PlayerFarming.Instance.Spine.AnimationState.AddAnimation(0, "rituals/sin-onboarding-loop", true, 0.0f);
    yield return (object) new WaitForSeconds(1f);
    interactionReapSouls.symbols.SetActive(true);
    yield return (object) new WaitForSeconds(0.25f);
    for (int i = 0; i < interactionReapSouls.spines.Length; ++i)
    {
      interactionReapSouls.spines[i].gameObject.SetActive(true);
      FollowerBrain.SetFollowerCostume(interactionReapSouls.spines[i].Skeleton, followerInfos[i], forceUpdate: true);
      BiomeConstants.Instance.EmitSmokeInteractionVFX(interactionReapSouls.spines[i].transform.position, Vector3.one);
      AudioManager.Instance.PlayOneShot("event:/followers/pop_in", interactionReapSouls.spines[i].transform.position);
      yield return (object) new WaitForSeconds(0.33f);
    }
    interactionReapSouls._meshMaterial.DOColor(Color.white, "_Color", 2f);
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationNext(interactionReapSouls.cameraPosition, 6f);
    yield return (object) new WaitForSeconds(1f);
    interactionReapSouls.blade.transform.DOShakeRotation(0.5f, new Vector3(0.0f, 0.0f, 3f), 50).SetEase<Tweener>(Ease.Linear);
    yield return (object) new WaitForSeconds(1f);
    interactionReapSouls.blade.transform.DOLocalMoveY(0.3f, 0.15f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear);
    yield return (object) new WaitForSeconds(0.15f);
    BloodSplatterScreenOverlay.instance.PlayRoutine();
    DeviceLightingManager.TransitionLighting(Color.red, Color.red, 4f);
    BiomeConstants.Instance.EmitBloodSplatter(interactionReapSouls.groundPosition.transform.position - Vector3.back, Vector3.back, Color.black);
    BiomeConstants.Instance.EmitBloodSplatterGroundParticles(interactionReapSouls.groundPosition.transform.position - Vector3.back, Vector3.back, Color.black);
    BiomeConstants.Instance.EmitBloodImpact(interactionReapSouls.groundPosition.transform.position - Vector3.back, 0.0f, "red", "BloodImpact_Large_0");
    MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact);
    BiomeConstants.Instance.EmitBloodDieEffect(interactionReapSouls.groundPosition.transform.position - Vector3.back, Vector3.back, Color.black);
    BiomeConstants.Instance.ImpactFrameForDuration();
    AudioManager.Instance.PlayOneShot("event:/Stings/sins_snake_sting", PlayerFarming.Instance.transform.position);
    AudioManager.Instance.PlayOneShot("event:/weapon/axe_heavy/catch_axe", interactionReapSouls.groundPosition.transform.position - Vector3.back);
    AudioManager.Instance.PlayOneShot("event:/material/wood_impact", interactionReapSouls.groundPosition.transform.position - Vector3.back);
    int index1 = 0;
    foreach (SkeletonAnimation spine1 in interactionReapSouls.spines)
    {
      SkeletonAnimation spine = spine1;
      BiomeConstants.Instance.EmitBloodSplatter(spine.transform.position + new Vector3(0.0f, -0.1f, -0.75f), Vector3.one, Color.black);
      spine.AnimationState.SetAnimation(0, "Avatars/Death", false);
      spine.transform.DOMoveZ(2f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InCirc).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => spine.transform.DOMoveZ(0.0f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBounce)));
      spine.transform.DOMoveX(interactionReapSouls.headPos[index1].transform.position.x, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutCirc);
      spine.transform.DOMoveY(interactionReapSouls.headPos[index1].transform.position.y, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutCirc);
      ++index1;
    }
    for (int index2 = 0; index2 < followerInfos.Count; ++index2)
    {
      DataManager.Instance.Followers_Dead.Remove(followerInfos[index2]);
      DataManager.Instance.Followers_Dead_IDs.Remove(followerInfos[index2].ID);
    }
    GameObject godTear = UnityEngine.Object.Instantiate<GameObject>(interactionReapSouls.sinPrefab, interactionReapSouls.sinPosition.transform.position, Quaternion.identity, interactionReapSouls.transform.parent);
    godTear.transform.localScale = Vector3.zero;
    godTear.transform.DOScale(Vector3.one, 1.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
    AudioManager.Instance.PlayOneShot("event:/Stings/global_faith_up", interactionReapSouls.gameObject);
    AudioManager.Instance.PlayOneShot("event:/Stings/generic_positive", interactionReapSouls.gameObject);
    AudioManager.Instance.PlayOneShot("event:/player/float_follower", interactionReapSouls.gameObject);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    AudioManager.Instance.PlayOneShot("event:/player/collect_sin_from_fol", PlayerFarming.Instance.gameObject);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("Sin/collect", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(1.5f);
    PlayerSimpleInventory simpleInventory = PlayerFarming.Instance.simpleInventory;
    Vector3 endValue = new Vector3(simpleInventory.ItemImage.transform.position.x, simpleInventory.ItemImage.transform.position.y - 0.5f, -1f) - Vector3.forward;
    GameManager.GetInstance().OnConversationNext(godTear, 8f);
    godTear.transform.DOMove(endValue, 2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => godTear.transform.DOScale(0.0f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) godTear.gameObject);
      GameManager.GetInstance().OnConversationEnd();
      Inventory.AddItem(154, 1);
    }))));
    interactionReapSouls.blade.transform.DOLocalMove(interactionReapSouls.bladeStartingPosition, 3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear);
    yield return (object) new WaitForSeconds(0.5f);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.FoundItem;
    AudioManager.Instance.PlayOneShot("event:/Stings/sins_snake_sting", interactionReapSouls.gameObject);
    interactionReapSouls._meshMaterial.DOColor(new Color(1f, 1f, 1f, 0.0f), "_Color", 3f);
    yield return (object) new WaitForSeconds(2f);
    int num = 0;
    while (num < interactionReapSouls.spines.Length)
      ++num;
    yield return (object) new WaitForSeconds(1f);
    for (int index3 = 0; index3 < interactionReapSouls.spines.Length; ++index3)
    {
      interactionReapSouls.spines[index3].transform.position = interactionReapSouls.headPosStart[index3];
      interactionReapSouls.spines[index3].gameObject.SetActive(false);
    }
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.Idle;
    interactionReapSouls.symbols.SetActive(false);
  }
}
