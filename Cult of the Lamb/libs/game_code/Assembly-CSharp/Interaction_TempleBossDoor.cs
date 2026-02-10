// Decompiled with JetBrains decompiler
// Type: Interaction_TempleBossDoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using MMBiomeGeneration;
using MMTools;
using Spine.Unity;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_TempleBossDoor : Interaction
{
  public static Interaction_TempleBossDoor Instance;
  public ParticleSystem doorParticles;
  public ParticleSystem doorParticlesLong;
  public GameObject PlayerPosition;
  public SimpleSetCamera SimpleSetCamera;
  public GameObject DoorToMove;
  public Vector3 OpenDoorPosition = new Vector3(0.0f, -2.5f, 4.5f);
  public string SceneName;
  public GameObject TeleportToBase;
  public SkeletonAnimation SealSpine;
  public SimpleSpineFlash SimpleSpineFlash;
  public Coroutine sealCoroutine;
  [SerializeField]
  public SkeletonRendererCustomMaterials spineMaterialOverride;
  [SerializeField]
  public ParticleSystem doorcloseParticleSystem;
  public bool spawnedExtraHearts;
  public BoxCollider2D CollideForDoor;
  public bool Unlocked;
  [CompilerGenerated]
  public bool \u003CUsed\u003Ek__BackingField;
  public Tween moveTween;

  public void Start()
  {
    Interaction_TempleBossDoor.Instance = this;
    this.UpdateLocalisation();
    if ((bool) (UnityEngine.Object) this.TeleportToBase)
    {
      this.TeleportToBase.SetActive(!DataManager.Instance.DungeonBossFight || this.Unlocked);
      if (DungeonSandboxManager.Active)
      {
        this.TeleportToBase.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
      }
    }
    if (!DataManager.Instance.DungeonBossFight || PlayerFarming.Location != FollowerLocation.Dungeon1_5 && PlayerFarming.Location != FollowerLocation.Dungeon1_6)
      return;
    this.TeleportToBase.gameObject.SetActive(false);
  }

  public void OpenTheDoor()
  {
    this.Unlocked = true;
    this.OpenDoor();
    this.Interactable = true;
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.Interactable = false;
    this.Used = false;
    this.spineMaterialOverride.enabled = false;
    Debug.Log((object) $"DataManager.Instance.UnlockedBossTempleDoor.Contains(MMBiomeGeneration.BiomeGenerator.Instance.DungeonLocation) {DataManager.Instance.UnlockedBossTempleDoor.Contains(BiomeGenerator.Instance.DungeonLocation).ToString()}  {BiomeGenerator.Instance.DungeonLocation.ToString()}");
    this.Unlocked = DataManager.Instance.UnlockedBossTempleDoor.Contains(BiomeGenerator.Instance.DungeonLocation) || DungeonSandboxManager.Active;
    if (DataManager.Instance.CurrentDLCNodeType == DungeonWorldMapIcon.NodeType.Dungeon5_Boss || DataManager.Instance.CurrentDLCNodeType == DungeonWorldMapIcon.NodeType.Dungeon6_Boss)
      this.Unlocked = true;
    if (this.Unlocked && (DataManager.Instance.ShownInitialTempleDoorSeal || DungeonSandboxManager.Active) || BiomeGenerator.Instance.DungeonLocation == FollowerLocation.Boss_1 || BiomeGenerator.Instance.DungeonLocation == FollowerLocation.Boss_2 || BiomeGenerator.Instance.DungeonLocation == FollowerLocation.Boss_3 || BiomeGenerator.Instance.DungeonLocation == FollowerLocation.Boss_4 || DataManager.Instance.CurrentDLCNodeType == DungeonWorldMapIcon.NodeType.Dungeon5_Boss || DataManager.Instance.CurrentDLCNodeType == DungeonWorldMapIcon.NodeType.Dungeon6_Boss)
    {
      this.Unlocked = true;
      this.DoorToMove.transform.localPosition = this.OpenDoorPosition;
      this.DoorToMove.SetActive(false);
      this.StartCoroutine((IEnumerator) this.SpawnHeartOnPreviousDeaths());
      this.TeleportToBase.gameObject.SetActive(false);
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

  public IEnumerator SpawnHeartOnPreviousDeaths()
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

  public IEnumerator SealRoutine()
  {
    Interaction_TempleBossDoor interactionTempleBossDoor = this;
    Debug.Log((object) "AAA".Colour(Color.red));
    int dungeonLayer = GameManager.CurrentDungeonLayer;
    if (GameManager.Layer2)
    {
      dungeonLayer = DataManager.GetGodTearNotches(PlayerFarming.Location);
      if (DataManager.Instance.DungeonBossFight)
        dungeonLayer = 4;
    }
    string anim = Mathf.Clamp(dungeonLayer - 1, 0, int.MaxValue).ToString();
    while (interactionTempleBossDoor.SealSpine.AnimationState == null)
      yield return (object) null;
    interactionTempleBossDoor.SealSpine.AnimationState.SetAnimation(0, anim, true);
    if ((UnityEngine.Object) interactionTempleBossDoor.playerFarming != (UnityEngine.Object) null && (dungeonLayer == 4 || !DataManager.Instance.ShownInitialTempleDoorSeal))
    {
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(interactionTempleBossDoor.playerFarming.gameObject);
      float t = 0.0f;
      while ((double) (t += Time.deltaTime) < 1.0)
      {
        interactionTempleBossDoor.playerFarming.GoToAndStop(new Vector3(-0.5f, 0.5f, 0.0f), groupAction: true);
        yield return (object) null;
      }
      while (interactionTempleBossDoor.playerFarming.GoToAndStopping)
        yield return (object) null;
      yield return (object) new WaitForEndOfFrame();
      PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive, PlayerNotToInclude: interactionTempleBossDoor.playerFarming);
      interactionTempleBossDoor.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
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
    if (dungeonLayer != 4)
      interactionTempleBossDoor.SealSpine.AnimationState.AddAnimation(0, anim, true, 0.0f);
    interactionTempleBossDoor.doorParticles.Play();
    interactionTempleBossDoor.SimpleSpineFlash.FlashFillRed();
    if (dungeonLayer == 4)
    {
      interactionTempleBossDoor.doorParticlesLong.Play();
      yield return (object) new WaitForSeconds(4.5f);
      interactionTempleBossDoor.doorParticles.Play();
      interactionTempleBossDoor.OnInteract(interactionTempleBossDoor.playerFarming.state);
      if (!DataManager.Instance.UnlockedBossTempleDoor.Contains(BiomeGenerator.Instance.DungeonLocation))
      {
        Debug.Log((object) ("ADD ME! " + BiomeGenerator.Instance.DungeonLocation.ToString()));
        DataManager.Instance.UnlockedBossTempleDoor.Add(BiomeGenerator.Instance.DungeonLocation);
      }
    }
    else if (!DataManager.Instance.ShownInitialTempleDoorSeal)
    {
      yield return (object) new WaitForSeconds(3f);
      PlayerFarming.SetStateForAllPlayers();
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

  public void OnBiomeChangeRoom()
  {
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.OnBiomeChangeRoom);
  }

  public void OpenDoor()
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

  public IEnumerator EnterTemple()
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

  public bool Used
  {
    get => this.\u003CUsed\u003Ek__BackingField;
    set => this.\u003CUsed\u003Ek__BackingField = value;
  }

  public void OnTriggerEnter2D(Collider2D collision)
  {
    if (!collision.gameObject.CompareTag("Player") || this.Used)
      return;
    this.Used = true;
    MMTransition.StopCurrentTransition();
    MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 0.5f, "", new System.Action(this.ChangeRoom));
  }

  public void ChangeRoom()
  {
    Debug.Log((object) "HIT");
    if (BiomeGenerator.Instance.DungeonLocation == FollowerLocation.Boss_1 || BiomeGenerator.Instance.DungeonLocation == FollowerLocation.Boss_2 || BiomeGenerator.Instance.DungeonLocation == FollowerLocation.Boss_3 || BiomeGenerator.Instance.DungeonLocation == FollowerLocation.Boss_4)
      BiomeGenerator.ChangeRoom(0, 1);
    else
      BiomeGenerator.ChangeRoom(BiomeGenerator.BossCoords.x, BiomeGenerator.BossCoords.y);
  }

  public void FadeSave()
  {
  }
}
