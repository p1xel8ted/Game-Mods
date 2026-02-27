// Decompiled with JetBrains decompiler
// Type: Interaction_TempleBossDoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using MMBiomeGeneration;
using MMTools;
using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_TempleBossDoor : Interaction
{
  public ParticleSystem doorParticles;
  public ParticleSystem doorParticlesLong;
  public GameObject PlayerPosition;
  public SimpleSetCamera SimpleSetCamera;
  public GameObject DoorToMove;
  private Vector3 OpenDoorPosition = new Vector3(0.0f, -2.5f, 4.5f);
  public string SceneName;
  public GameObject TeleportToBase;
  public SkeletonAnimation SealSpine;
  public SimpleSpineFlash SimpleSpineFlash;
  private Coroutine sealCoroutine;
  [SerializeField]
  private SkeletonRendererCustomMaterials spineMaterialOverride;
  [SerializeField]
  private ParticleSystem doorcloseParticleSystem;
  private bool spawnedExtraHearts;
  public BoxCollider2D CollideForDoor;
  private bool Unlocked;
  private bool Used;
  private Tween moveTween;

  private void Start()
  {
    this.UpdateLocalisation();
    if (!(bool) (UnityEngine.Object) this.TeleportToBase)
      return;
    this.TeleportToBase.SetActive(!DataManager.Instance.DungeonBossFight || this.Unlocked);
  }

  public void OpenTheDoor()
  {
    this.Unlocked = true;
    this.OpenDoor();
    this.Interactable = true;
  }

  protected override void OnEnable()
  {
    base.OnEnable();
    this.Interactable = false;
    this.Used = false;
    this.spineMaterialOverride.enabled = false;
    Debug.Log((object) $"DataManager.Instance.UnlockedBossTempleDoor.Contains(MMBiomeGeneration.BiomeGenerator.Instance.DungeonLocation) {DataManager.Instance.UnlockedBossTempleDoor.Contains(BiomeGenerator.Instance.DungeonLocation).ToString()}  {(object) BiomeGenerator.Instance.DungeonLocation}");
    this.Unlocked = DataManager.Instance.UnlockedBossTempleDoor.Contains(BiomeGenerator.Instance.DungeonLocation);
    if (this.Unlocked && DataManager.Instance.ShownInitialTempleDoorSeal || BiomeGenerator.Instance.DungeonLocation == FollowerLocation.Boss_1 || BiomeGenerator.Instance.DungeonLocation == FollowerLocation.Boss_2 || BiomeGenerator.Instance.DungeonLocation == FollowerLocation.Boss_3 || BiomeGenerator.Instance.DungeonLocation == FollowerLocation.Boss_4)
    {
      this.Unlocked = true;
      this.DoorToMove.transform.localPosition = this.OpenDoorPosition;
      this.DoorToMove.SetActive(false);
      this.StartCoroutine((IEnumerator) this.SpawnHeartOnPreviousDeaths());
    }
    else
    {
      this.DoorToMove.transform.localPosition = Vector3.zero;
      if ((UnityEngine.Object) this.SealSpine != (UnityEngine.Object) null)
        this.sealCoroutine = this.StartCoroutine((IEnumerator) this.SealRoutine());
    }
    this.OpenDoor();
    BiomeGenerator.OnBiomeChangeRoom += new BiomeGenerator.BiomeAction(this.OnBiomeChangeRoom);
  }

  private IEnumerator SpawnHeartOnPreviousDeaths()
  {
    Interaction_TempleBossDoor interactionTempleBossDoor = this;
    yield return (object) new WaitForEndOfFrame();
    if (!interactionTempleBossDoor.spawnedExtraHearts && interactionTempleBossDoor.gameObject.activeInHierarchy)
    {
      if (DifficultyManager.AssistModeEnabled && DataManager.Instance.playerDeathsInARowFightingLeader > 2)
      {
        switch (DifficultyManager.PrimaryDifficulty)
        {
          case DifficultyManager.Difficulty.Easy:
            InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLUE_HEART, 2, interactionTempleBossDoor.TeleportToBase.gameObject.transform.position);
            break;
          case DifficultyManager.Difficulty.Medium:
            InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLUE_HEART, 1, interactionTempleBossDoor.TeleportToBase.gameObject.transform.position);
            break;
        }
      }
      interactionTempleBossDoor.spawnedExtraHearts = true;
    }
  }

  public override void GetLabel()
  {
    base.GetLabel();
    this.Label = !this.Unlocked ? ScriptLocalization.Interactions.Locked : "";
  }

  private IEnumerator SealRoutine()
  {
    Interaction_TempleBossDoor interactionTempleBossDoor = this;
    int dungeonLayer = GameManager.CurrentDungeonLayer;
    string anim = (dungeonLayer - 1).ToString();
    while (interactionTempleBossDoor.SealSpine.AnimationState == null)
      yield return (object) null;
    interactionTempleBossDoor.SealSpine.AnimationState.SetAnimation(0, anim, true);
    if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null && (GameManager.CurrentDungeonLayer == 4 || !DataManager.Instance.ShownInitialTempleDoorSeal))
    {
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject);
      float t = 0.0f;
      while ((double) (t += Time.deltaTime) < 1.0)
      {
        PlayerFarming.Instance.GoToAndStop(new Vector3(-0.5f, 0.5f, 0.0f));
        yield return (object) null;
      }
      while (PlayerFarming.Instance.GoToAndStopping)
        yield return (object) null;
      yield return (object) new WaitForEndOfFrame();
      PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      interactionTempleBossDoor.spineMaterialOverride.enabled = true;
      GameManager.GetInstance().OnConversationNext(interactionTempleBossDoor.SealSpine.gameObject, 7f);
      yield return (object) new WaitForSeconds(2.5f);
    }
    else
      yield return (object) new WaitForSeconds(1.5f);
    anim = dungeonLayer.ToString() + "-activate";
    if (anim == "4-activate")
      AudioManager.Instance.PlayOneShot("event:/door/boss_door_sequence", interactionTempleBossDoor.gameObject);
    else
      AudioManager.Instance.PlayOneShot("event:/door/boss_door_piece", interactionTempleBossDoor.gameObject);
    interactionTempleBossDoor.SealSpine.AnimationState.SetAnimation(0, anim, false);
    anim = dungeonLayer.ToString();
    if (GameManager.CurrentDungeonLayer != 4)
      interactionTempleBossDoor.SealSpine.AnimationState.AddAnimation(0, anim, true, 0.0f);
    interactionTempleBossDoor.doorParticles.Play();
    interactionTempleBossDoor.SimpleSpineFlash.FlashFillRed();
    if (GameManager.CurrentDungeonLayer == 4)
    {
      interactionTempleBossDoor.doorParticlesLong.Play();
      yield return (object) new WaitForSeconds(4.5f);
      interactionTempleBossDoor.doorParticles.Play();
      interactionTempleBossDoor.OnInteract((StateMachine) null);
      if (!DataManager.Instance.UnlockedBossTempleDoor.Contains(BiomeGenerator.Instance.DungeonLocation))
      {
        Debug.Log((object) ("ADD ME! " + (object) BiomeGenerator.Instance.DungeonLocation));
        DataManager.Instance.UnlockedBossTempleDoor.Add(BiomeGenerator.Instance.DungeonLocation);
      }
    }
    else if (!DataManager.Instance.ShownInitialTempleDoorSeal)
    {
      yield return (object) new WaitForSeconds(3f);
      GameManager.GetInstance().OnConversationEnd();
    }
    DataManager.Instance.ShownInitialTempleDoorSeal = true;
    interactionTempleBossDoor.TeleportToBase.GetComponent<Interaction_TeleportHome>().HasChanged = true;
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.OnBiomeChangeRoom);
    if (this.sealCoroutine == null)
      return;
    this.StopCoroutine(this.sealCoroutine);
  }

  private void OnBiomeChangeRoom()
  {
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.OnBiomeChangeRoom);
  }

  private void OpenDoor()
  {
    if (this.Unlocked)
      this.CollideForDoor.enabled = true;
    else
      this.CollideForDoor.enabled = false;
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.SimpleSetCamera.Play();
    this.StartCoroutine((IEnumerator) this.EnterTemple());
  }

  private IEnumerator EnterTemple()
  {
    Interaction_TempleBossDoor interactionTempleBossDoor = this;
    if (GameManager.CurrentDungeonLayer != 4)
      AudioManager.Instance.PlayOneShot("event:/door/boss_door_piece", interactionTempleBossDoor.gameObject);
    yield return (object) new WaitForSeconds(1f);
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.5f, 0.3f);
    yield return (object) new WaitForSeconds(0.2f);
    AudioManager.Instance.PlayOneShot("event:/door/door_lower", interactionTempleBossDoor.gameObject);
    interactionTempleBossDoor.doorcloseParticleSystem.Play();
    float Progress = 0.0f;
    float Duration = 3f;
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.5f, Duration);
    Vector3 StartingPosition = interactionTempleBossDoor.DoorToMove.transform.position;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      interactionTempleBossDoor.DoorToMove.transform.position = Vector3.Lerp(StartingPosition, StartingPosition + interactionTempleBossDoor.OpenDoorPosition, Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
      yield return (object) null;
    }
    AudioManager.Instance.PlayOneShot("event:/door/door_done", interactionTempleBossDoor.gameObject);
    interactionTempleBossDoor.doorcloseParticleSystem.Stop();
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.5f, 0.3f);
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().OnConversationEnd();
    interactionTempleBossDoor.SimpleSetCamera.Reset();
    interactionTempleBossDoor.Unlocked = true;
    interactionTempleBossDoor.OpenDoor();
    interactionTempleBossDoor.OnBiomeChangeRoom();
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (!(collision.gameObject.tag == "Player") || this.Used)
      return;
    this.Used = true;
    MMTransition.StopCurrentTransition();
    MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 0.5f, "", new System.Action(this.ChangeRoom));
  }

  private void ChangeRoom()
  {
    Debug.Log((object) "HIT");
    if (BiomeGenerator.Instance.DungeonLocation == FollowerLocation.Boss_1 || BiomeGenerator.Instance.DungeonLocation == FollowerLocation.Boss_2 || BiomeGenerator.Instance.DungeonLocation == FollowerLocation.Boss_3 || BiomeGenerator.Instance.DungeonLocation == FollowerLocation.Boss_4)
      BiomeGenerator.ChangeRoom(0, 1);
    else
      BiomeGenerator.ChangeRoom(BiomeGenerator.BossCoords.x, BiomeGenerator.BossCoords.y);
  }

  private void FadeSave()
  {
  }
}
