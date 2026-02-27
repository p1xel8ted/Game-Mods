// Decompiled with JetBrains decompiler
// Type: IntroRoomPlayerReturns
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using MMBiomeGeneration;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class IntroRoomPlayerReturns : BaseMonoBehaviour
{
  public IntroRoomMusicController musicController;
  public SimpleSetCamera SimpleSetCamera;
  public GameObject PlayerPrefab;
  private SimpleSpineAnimator simpleSpineAnimator;
  public AnimateOnCallBack GoatSpine;
  public CanvasGroup ControlsHUD;
  public List<GameObject> DestroyObjects = new List<GameObject>();
  public List<SkeletonAnimation> AnimateCultists = new List<SkeletonAnimation>();
  public GameObject ActivateEnemies;
  public GameObject InvisibleEnemyWall;
  public GameObject LightingOverride;

  private void Start()
  {
    this.ActivateEnemies.SetActive(false);
    this.ControlsHUD.alpha = 0.0f;
  }

  public void Play() => this.StartCoroutine((IEnumerator) this.PlayRoutine());

  private IEnumerator PlayRoutine()
  {
    IntroRoomPlayerReturns roomPlayerReturns = this;
    BiomeGenerator.Instance.CurrentRoom.generateRoom.SetColliderAndUpdatePathfinding();
    roomPlayerReturns.musicController.PlayAmbientNature();
    foreach (Object destroyObject in roomPlayerReturns.DestroyObjects)
      Object.Destroy(destroyObject);
    roomPlayerReturns.ActivateEnemies.SetActive(true);
    GameObject Player = GameObject.FindWithTag("Player");
    if ((Object) Player != (Object) null)
      Object.Destroy((Object) Player);
    GameObject NewPlayer = Object.Instantiate<GameObject>(roomPlayerReturns.PlayerPrefab, new Vector3(0.0f, Player.transform.position.y, Player.transform.position.z), Quaternion.identity, GameObject.FindGameObjectWithTag("Unit Layer").transform);
    BiomeGenerator.Instance.Player = NewPlayer;
    NewPlayer.GetComponent<Health>().DamageModifier = 0.0f;
    StateMachine component = NewPlayer.GetComponent<StateMachine>();
    component.facingAngle = Player.GetComponent<StateMachine>().facingAngle;
    component.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForEndOfFrame();
    GameManager.GetInstance().AddToCamera(NewPlayer);
    roomPlayerReturns.SimpleSetCamera.Reset();
    GameManager.GetInstance().CamFollowTarget.TargetCamera = (Camera) null;
    GameManager.GetInstance().AddPlayerToCamera();
    GameManager.GetInstance().OnConversationNew(false, true);
    GameManager.GetInstance().OnConversationNext(NewPlayer, 4f);
    GameManager.GetInstance().CameraSetZoom(4f);
    roomPlayerReturns.simpleSpineAnimator = NewPlayer.GetComponentInChildren<SimpleSpineAnimator>();
    roomPlayerReturns.simpleSpineAnimator.Animate("intro/dead", 0, true).MixDuration = 0.0f;
    roomPlayerReturns.simpleSpineAnimator.SetSkin("Lamb_Intro");
    Object.Destroy((Object) Player);
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationNext(NewPlayer, 5f);
    CameraManager.shakeCamera(0.3f);
    roomPlayerReturns.simpleSpineAnimator.OnSpineEvent += new SimpleSpineAnimator.SpineEvent(roomPlayerReturns.OnSpineEvent);
    roomPlayerReturns.simpleSpineAnimator.Animate("intro/rebirth", 0, false);
    roomPlayerReturns.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(3f);
    roomPlayerReturns.LightingOverride.SetActive(false);
    yield return (object) new WaitForSeconds(1.5f);
    GameManager.GetInstance().OnConversationNext(NewPlayer.gameObject, 6f);
    PlayerFarming.Instance.playerWeapon.SetWeapon(EquipmentType.Sword, 0);
    yield return (object) new WaitForSeconds(2.5f);
    GameManager.GetInstance().OnConversationEnd();
    GameManager.GetInstance().AddPlayerToCamera();
    HUD_Manager.Instance.Hide(true);
    foreach (Behaviour behaviour in Object.FindObjectsOfType<EnemySwordsman>())
      behaviour.enabled = true;
    foreach (FormationFighter formationFighter in Object.FindObjectsOfType<FormationFighter>())
    {
      formationFighter.transform.localScale = Vector3.one;
      formationFighter.enabled = true;
    }
    Object.FindObjectOfType<IntroGuards>().Wall.SetActive(false);
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().CameraSetZoom(12f);
    GameManager.GetInstance().CachedZoom = 12f;
    float Progress = 0.0f;
    float Duration = 0.5f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      roomPlayerReturns.ControlsHUD.alpha = Mathf.Lerp(0.0f, 1f, Progress / Duration);
      yield return (object) null;
    }
    while ((double) NewPlayer.transform.position.y > (double) roomPlayerReturns.InvisibleEnemyWall.transform.position.y)
      yield return (object) null;
    Progress = 0.0f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      roomPlayerReturns.ControlsHUD.alpha = Mathf.Lerp(1f, 0.0f, Progress / Duration);
      yield return (object) null;
    }
  }

  private void OnSpineEvent(string EventName)
  {
    switch (EventName)
    {
      case "change-skin":
        this.simpleSpineAnimator.SetSkin("Lamb_0");
        GameManager.GetInstance().CameraSetTargetZoom(7f);
        CameraManager.shakeCamera(0.5f);
        foreach (SkeletonAnimation animateCultist in this.AnimateCultists)
        {
          AudioManager.Instance.PlayOneShot("event:/enemy/vocals/humanoid/warning", animateCultist.gameObject);
          animateCultist.AnimationState.SetAnimation(0, "scared", false);
          animateCultist.AnimationState.AddAnimation(0, "scared-loop", true, 0.0f);
        }
        this.musicController.PlayCombatMusic();
        break;
      case "sfxTrigger":
        AudioManager.Instance.PlayOneShot("event:/player/resurrect", PlayerFarming.Instance.gameObject);
        break;
      case "intro-sword":
        AudioManager.Instance.PlayOneShot("event:/player/weapon_unlocked", PlayerFarming.Instance.gameObject);
        break;
    }
  }

  private void OnDisable()
  {
    if (!((Object) this.simpleSpineAnimator != (Object) null))
      return;
    this.simpleSpineAnimator.OnSpineEvent -= new SimpleSpineAnimator.SpineEvent(this.OnSpineEvent);
  }
}
