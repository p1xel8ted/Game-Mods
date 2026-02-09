// Decompiled with JetBrains decompiler
// Type: DoorRoomLocationManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[DefaultExecutionOrder(-50)]
public class DoorRoomLocationManager : LocationManager
{
  public static DoorRoomLocationManager Instance;
  public Transform DoorPosition;
  public Animator SkyAnimator;
  [SerializeField]
  public GameObject chainDoorObject;
  [SerializeField]
  public GameObject mysticShopObject;
  public Material uberShaderParticleMat;

  public override FollowerLocation Location => FollowerLocation.DoorRoom;

  public override Transform UnitLayer => this.transform;

  public override Transform StructureLayer => this.transform;

  public override void Awake()
  {
    DoorRoomLocationManager.Instance = this;
    base.Awake();
  }

  public override void OnEnable()
  {
    base.OnEnable();
    if (!SettingsManager.Settings.Game.PerformanceMode)
      return;
    if ((UnityEngine.Object) this.uberShaderParticleMat == (UnityEngine.Object) null)
      this.uberShaderParticleMat = GameObject.Find("Grass Tufty").GetComponent<SpriteRenderer>().sharedMaterial;
    this.uberShaderParticleMat.SetFloat("_FloorHeight", -1.1f);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    if (!SettingsManager.Settings.Game.PerformanceMode || !((UnityEngine.Object) this.uberShaderParticleMat != (UnityEngine.Object) null))
      return;
    this.uberShaderParticleMat.SetFloat("_FloorHeight", 0.0f);
  }

  public override void Start()
  {
    base.Start();
    this.chainDoorObject.SetActive(!DataManager.Instance.DeathCatBeaten || !DataManager.Instance.OnboardedMysticShop);
    this.mysticShopObject.SetActive(DataManager.Instance.DeathCatBeaten);
  }

  public override void Update()
  {
    base.Update();
    this.SkyAnimator.SetBool("BloodMoon", FollowerBrainStats.IsBloodMoon);
    this.SkyAnimator.SetBool("AuroraBorealis", FollowerBrainStats.IsWarmthRitual);
  }

  public override Vector3 GetStartPosition(FollowerLocation prevLocation)
  {
    return DoorRoomLocationManager.Instance.DoorPosition.position;
  }

  public override Vector3 GetExitPosition(FollowerLocation destLocation)
  {
    return DoorRoomLocationManager.Instance.DoorPosition.position;
  }

  public void DeathCatRelicSequence()
  {
    this.StartCoroutine((IEnumerator) this.DeathCatRelicSequenceIE());
  }

  public IEnumerator DeathCatRelicSequenceIE()
  {
    DoorRoomLocationManager roomLocationManager = this;
    while (LocationManager.GetLocationState(roomLocationManager.Location) != LocationState.Active)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    List<Vector3> vector3List = new List<Vector3>()
    {
      new Vector3(2f, 32f, -1.25f),
      new Vector3(-2f, 32f, -1.25f),
      new Vector3(3f, 33f, -1.25f),
      new Vector3(-3f, 33f, -1.25f)
    };
    List<FollowerManager.SpawnedFollower> otherBishops = new List<FollowerManager.SpawnedFollower>();
    List<int> intList = new List<int>();
    foreach (FollowerInfo follower1 in DataManager.Instance.Followers)
    {
      if (follower1.ID != 666 && FollowerManager.BishopIDs.Contains(follower1.ID) && !intList.Contains(follower1.ID))
      {
        FollowerManager.SpawnedFollower spawnedFollower = FollowerManager.SpawnCopyFollower(follower1, vector3List[otherBishops.Count], roomLocationManager.transform, FollowerLocation.DoorRoom);
        double num = (double) spawnedFollower.Follower.SetBodyAnimation("idle", true);
        otherBishops.Add(spawnedFollower);
        intList.Add(follower1.ID);
      }
    }
    PlayerFarming.Instance.transform.position = new Vector3(-0.75f, 23.15f, -1.25f);
    FollowerManager.SpawnedFollower follower = FollowerManager.SpawnCopyFollower(FollowerInfo.GetInfoByID(666), new Vector3(0.75f, 23.15f, -1.25f), roomLocationManager.transform, FollowerLocation.DoorRoom);
    follower.Follower.SpeedMultiplier = 1.5f;
    follower.Follower.OverridingEmotions = true;
    follower.Follower.SetFaceAnimation("Emotions/emotion-normal", true);
    bool waiting = true;
    Vector3 zero = Vector3.zero;
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      PlayerFarming playerFarming = player;
      playerFarming.GoToAndStop(new Vector3(-1f, 31.5f, -1.25f) + zero, GoToCallback: (System.Action) (() => playerFarming.state.facingAngle = playerFarming.state.LookAngle = Utils.GetAngle(playerFarming.transform.position, follower.Follower.transform.position)));
      zero += Vector3.down / 2f;
    }
    yield return (object) new WaitForEndOfFrame();
    MMConversation.ClearEventListenerSFX(follower.Follower.gameObject, "VO/talk short nice");
    MMConversation.ClearEventListenerSFX(follower.Follower.gameObject, "VO/talk short hate");
    follower.Follower.GoTo(new Vector3(1f, 31.5f, -1.25f), (System.Action) (() => waiting = false));
    foreach (FollowerManager.SpawnedFollower spawnedFollower in otherBishops)
    {
      spawnedFollower.Follower.FacePosition(new Vector3(0.0f, 32f, 0.0f));
      spawnedFollower.Follower.OverridingEmotions = true;
      string animName = "Emotions/emotion-happy";
      if (spawnedFollower.FollowerBrain.Info.ID == 99990)
        animName = "Emotions/emotion-normal";
      else if (spawnedFollower.FollowerBrain.Info.ID == 99991)
        animName = "Emotions/emotion-angry";
      else if (spawnedFollower.FollowerBrain.Info.ID == 99993)
        animName = "Emotions/emotion-unhappy";
      spawnedFollower.Follower.SetFaceAnimation(animName, true);
    }
    while (waiting)
      yield return (object) null;
    follower.Follower.FacePosition(PlayerFarming.Instance.transform.position);
    foreach (PlayerFarming player in PlayerFarming.players)
      player.state.facingAngle = player.state.LookAngle = Utils.GetAngle(player.transform.position, follower.Follower.transform.position);
    yield return (object) new WaitForSeconds(0.5f);
    List<ConversationEntry> Entries = new List<ConversationEntry>()
    {
      new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/DeathCat/Relic/Second/0"),
      new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/DeathCat/Relic/Second/1"),
      new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/DeathCat/Relic/Second/2"),
      new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/DeathCat/Relic/Second/3"),
      new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/DeathCat/Relic/Second/4")
    };
    for (int index = 0; index < Entries.Count; ++index)
    {
      Entries[index].CharacterName = follower.FollowerFakeInfo.Name;
      Entries[index].Animation = (index >= 3 ? "Conversations/talk-nice" : "Conversations/talk-mean") + UnityEngine.Random.Range(1, 4).ToString();
      Entries[index].LoopAnimation = true;
      Entries[index].soundPath = " ";
      Entries[index].SetZoom = true;
      Entries[index].Zoom = UnityEngine.Random.Range(4f, 5f);
      Entries[index].followerID = 666;
    }
    MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) null), false);
    while (MMConversation.isPlaying)
      yield return (object) null;
    double num1 = (double) follower.Follower.SetBodyAnimation("idle", true);
    follower.Follower.SetFaceAnimation("Emotions/emotion-normal", true);
    waiting = true;
    GameObject Speaker = RelicCustomTarget.Create(follower.Follower.transform.position, PlayerFarming.Instance.transform.position, 1f, RelicType.KillNonBossEnemies, (System.Action) (() => waiting = false));
    GameManager.GetInstance().OnConversationNext(Speaker, 5f);
    while (waiting)
      yield return (object) null;
    follower.FollowerBrain.AddTrait(FollowerTrait.TraitType.BishopOfCult, true);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject, 5f);
    follower.Follower.GoTo(Vector3.zero, (System.Action) null);
    foreach (FollowerManager.SpawnedFollower spawnedFollower1 in otherBishops)
    {
      FollowerManager.SpawnedFollower spawnedFollower = spawnedFollower1;
      roomLocationManager.StartCoroutine((IEnumerator) roomLocationManager.PlaySoundDelay(spawnedFollower.Follower.gameObject));
      spawnedFollower.Follower.TimedAnimation("spawn-out", 0.8666667f, (System.Action) (() => FollowerManager.CleanUpCopyFollower(spawnedFollower)));
      yield return (object) new WaitForSeconds(0.1f);
    }
    yield return (object) new WaitForSeconds(2f);
    FollowerManager.CleanUpCopyFollower(follower);
    foreach (PlayerFarming player in PlayerFarming.players)
      player.unitObject.SpeedMultiplier = 1f;
    GameManager.GetInstance().OnConversationEnd();
  }

  public IEnumerator PlaySoundDelay(GameObject spawnedFollower)
  {
    yield return (object) new WaitForSeconds(0.566666663f);
    AudioManager.Instance.PlayOneShot("event:/followers/pop_in", spawnedFollower);
  }
}
