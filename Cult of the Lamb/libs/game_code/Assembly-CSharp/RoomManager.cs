// Decompiled with JetBrains decompiler
// Type: RoomManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMRoomGeneration;
using MMTools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RoomManager : BaseMonoBehaviour
{
  public static Room r = (Room) null;
  public bool TestSingleRoom = true;
  public bool RandomFollowers = true;
  public GameObject North;
  public GameObject East;
  public GameObject South;
  public GameObject West;
  public static int CurrentX = -1;
  public static int CurrentY = -1;
  public static int PrevCurrentX = -1;
  public static int PrevCurrentY = -1;
  public bool EntryToPortalRoom = true;
  public bool IsTeleporting;
  public GameObject player;
  public CameraFollowTarget Camera;
  public static RoomManager _Instance;
  public bool LimitTime = true;
  public List<GameObject> TimeCops = new List<GameObject>();
  public float TimeCopTimer;
  public float ForestHeadTimer = 5f;
  public int NumHeads;
  public List<RoomInfo> RoomPrefabs;
  public RoomInfo CurrentRoomPrefab;

  public event RoomManager.InitEnemiesAction OnInitEnemies;

  public event RoomManager.ChangeRoomDelegate OnChangeRoom;

  public static RoomManager Instance
  {
    get => RoomManager._Instance;
    set => RoomManager._Instance = value;
  }

  public void OnEnable()
  {
    if (!Application.isEditor)
      this.TestSingleRoom = false;
    RoomManager.Instance = this;
    Health.OnDieAny += new Health.DieAllAction(this.OnDieAny);
  }

  public void OnDieAny(Health Victim)
  {
    if (Health.team2.Count > 0 || Victim.team != Health.Team.Team2 || RoomManager.r.cleared)
      return;
    this.UnlockDoors();
    RoomManager.r.cleared = true;
  }

  public void Start()
  {
    if (WorldGen.WorldGenerated)
      this.OnWorldGenerated();
    else
      WorldGen.Instance.OnWorldGenerated += new WorldGen.WorldGeneratedAction(this.OnWorldGenerated);
  }

  public void Update()
  {
    if (!this.LimitTime || !HUD_Timer.IsTimeUp || !HUD_Timer.TimerRunning)
      return;
    this.SpawnTimeCops();
  }

  public void SpawnTimeCops()
  {
    if (this.NumHeads <= 0)
      this.ForestHeadTimer -= Time.deltaTime;
    if (this.TimeCops.Count >= 10 || (double) (this.TimeCopTimer -= Time.deltaTime) > 0.0 || !((UnityEngine.Object) this.player != (UnityEngine.Object) null))
      return;
    float num = (float) UnityEngine.Random.Range(3, 6);
    float f = (float) UnityEngine.Random.Range(0, 360) * ((float) Math.PI / 180f);
    Vector3 vector3 = new Vector3(num * Mathf.Cos(f), num * Mathf.Sin(f));
    GameObject gameObject;
    if ((double) this.ForestHeadTimer < 0.0 && this.NumHeads <= 0)
    {
      ++this.NumHeads;
      vector3 = new Vector3(10f * Mathf.Cos(f), 10f * Mathf.Sin(f));
      gameObject = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Units/Forest Enemies/Enemy Forest Head"), this.player.transform.position + vector3, Quaternion.identity) as GameObject;
    }
    else
      gameObject = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Units/Forest Enemies/Enemy Forest Mushroom"), this.player.transform.position + vector3, Quaternion.identity) as GameObject;
    Health componentInChildren = gameObject.GetComponentInChildren<Health>();
    componentInChildren.OnDie += new Health.DieAction(this.RemoveTimeCop);
    this.TimeCops.Add(componentInChildren.gameObject);
    this.TimeCopTimer = 3f;
  }

  public void RemoveTimeCop(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    this.TimeCops.Remove(Victim.gameObject);
    Victim.OnDie -= new Health.DieAction(this.RemoveTimeCop);
  }

  public void OnDisable()
  {
    if ((UnityEngine.Object) WorldGen.Instance != (UnityEngine.Object) null)
      WorldGen.Instance.OnWorldGenerated -= new WorldGen.WorldGeneratedAction(this.OnWorldGenerated);
    Health.OnDieAny -= new Health.DieAllAction(this.OnDieAny);
  }

  public void OnWorldGenerated() => this.StartCoroutine((IEnumerator) this.DoWorldGeneration());

  public IEnumerator DoWorldGeneration()
  {
    RoomManager roomManager = this;
    if (RoomManager.CurrentX == -1 && RoomManager.CurrentY == -1)
    {
      RoomManager.CurrentX = WorldGen.startRoom.x;
      RoomManager.CurrentY = WorldGen.startRoom.y;
      CameraFollowTarget.Instance.distance = 30f;
    }
    roomManager.RoomPrefabs = new List<RoomInfo>();
    yield return (object) roomManager.StartCoroutine((IEnumerator) roomManager.CreateRooms());
    roomManager.ChangeRoom();
    MMTransition.ResumePlay();
  }

  public IEnumerator CreateRooms()
  {
    if (!this.TestSingleRoom)
    {
      int i = -1;
      while (++i < WorldGen.rooms.Count)
      {
        MMTransition.UpdateProgress((float) i / (float) WorldGen.rooms.Count);
        Room room = WorldGen.rooms[i];
        GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load(room.PrefabDir), Vector3.zero, Quaternion.identity) as GameObject;
        this.CurrentRoomPrefab = gameObject.GetComponent<RoomInfo>();
        this.RoomPrefabs.Add(this.CurrentRoomPrefab);
        this.CurrentRoomPrefab.ID = $"Room_{room.x.ToString()}_{room.y.ToString()}";
        this.CurrentRoomPrefab.Init();
        RoomManager.r = room;
        gameObject.GetComponent<GenerateRoom>();
        this.CurrentRoomPrefab.gameObject.SetActive(false);
        yield return (object) null;
      }
    }
  }

  public IEnumerator PlaceResources(Room RoomToPlace)
  {
    Transform RoomToPlaceTransform = RoomInfo.GetByID($"Room_{RoomToPlace.x.ToString()}_{RoomToPlace.y.ToString()}").transform;
    for (int i = 0; i < RoomToPlace.Structures.Count; ++i)
    {
      StructuresData structure1 = RoomToPlace.Structures[i];
      GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Resources.Load(structure1.PrefabPath) as GameObject, RoomToPlaceTransform, true);
      gameObject.transform.position = structure1.Position + structure1.Offset;
      Structure structure2 = gameObject.GetComponent<Structure>();
      if ((UnityEngine.Object) structure2 == (UnityEngine.Object) null)
        structure2 = gameObject.GetComponentInChildren<Structure>();
      if ((UnityEngine.Object) structure2 != (UnityEngine.Object) null)
        structure2.Brain = StructureBrain.GetOrCreateBrain(structure1);
      yield return (object) null;
    }
    RoomToPlace.ResourcesPlaced = true;
  }

  public void ChangeRoom()
  {
    if ((UnityEngine.Object) this.CurrentRoomPrefab != (UnityEngine.Object) null)
      this.CurrentRoomPrefab.gameObject.SetActive(false);
    Room r = RoomManager.r;
    RoomManager.r = WorldGen.getRoom(RoomManager.CurrentX, RoomManager.CurrentY);
    GameManager.GetInstance().RemoveAllFromCamera();
    if (this.TestSingleRoom)
    {
      this.CurrentRoomPrefab = UnityEngine.Object.FindObjectOfType<RoomInfo>();
      if (this.OnInitEnemies != null)
        this.OnInitEnemies();
    }
    else
    {
      this.CurrentRoomPrefab = RoomInfo.GetByID($"Room_{RoomManager.r.x.ToString()}_{RoomManager.r.y.ToString()}");
      this.CurrentRoomPrefab.gameObject.SetActive(true);
    }
    if ((UnityEngine.Object) this.CurrentRoomPrefab != (UnityEngine.Object) null && (UnityEngine.Object) this.CurrentRoomPrefab.Music != (UnityEngine.Object) null)
      AmbientMusicController.PlayTrack(this.CurrentRoomPrefab.Music, 3f);
    else if ((UnityEngine.Object) r == (UnityEngine.Object) null)
      AmbientMusicController.PlayAmbient(0.0f);
    else
      AmbientMusicController.StopTrackAndResturnToAmbient();
    this.GetDoors();
    if (!this.TestSingleRoom)
      this.OnInitEnemies();
    this.PlaceAndPositionPlayer();
    if (RoomManager.r.cleared && !this.TestSingleRoom)
      this.UnlockDoors();
    GameManager.RecalculatePaths();
    RoomManager.r.visited = true;
    if (this.OnChangeRoom == null)
      return;
    this.OnChangeRoom();
  }

  public void RemoveStructure(StructuresData structure)
  {
    RoomManager.r.Structures.Remove(structure);
  }

  public void PlaceAndPositionPlayer(bool ForceCentrePlayer = false)
  {
    if ((UnityEngine.Object) this.player == (UnityEngine.Object) null)
      this.player = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/Units/Player") as GameObject, GameObject.FindGameObjectWithTag("Unit Layer").transform, true);
    if (this.IsTeleporting)
    {
      if ((UnityEngine.Object) Interaction_Teleporter.Instance != (UnityEngine.Object) null)
        this.player.transform.position = Interaction_Teleporter.Instance.transform.position;
      if ((UnityEngine.Object) Interaction_TeleporterMap.Instance != (UnityEngine.Object) null)
        this.player.transform.position = Interaction_TeleporterMap.Instance.transform.position;
      this.player.GetComponent<PlayerFarming>().TimedAction(1f, (System.Action) null, "teleport-in");
      this.IsTeleporting = false;
    }
    else if (ForceCentrePlayer)
    {
      if ((UnityEngine.Object) Interaction_Teleporter.Instance != (UnityEngine.Object) null)
        this.player.transform.position = Interaction_Teleporter.Instance.transform.position;
    }
    else if (this.EntryToPortalRoom)
    {
      this.EntryToPortalRoom = false;
      this.player.GetComponent<StateMachine>().facingAngle = 90f;
      if ((UnityEngine.Object) this.South != (UnityEngine.Object) null)
        this.player.transform.position = this.South.transform.position + Vector3.up * 2.5f;
      if (!this.TestSingleRoom)
      {
        PlayerFarming component = this.player.GetComponent<PlayerFarming>();
        GameObject gameObject = new GameObject();
        if ((UnityEngine.Object) this.South != (UnityEngine.Object) null)
          gameObject.transform.position = this.South.transform.position + Vector3.up * 5f;
        GameObject TargetPosition = gameObject;
        component.GoToAndStop(TargetPosition, IdleOnEnd: true);
      }
    }
    else if (RoomManager.PrevCurrentX == -1 && RoomManager.PrevCurrentY == -1)
      this.player.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
    else if (RoomManager.PrevCurrentX < RoomManager.CurrentX)
    {
      this.player.transform.position = this.West.transform.position + Vector3.right * 0.5f;
      this.player.GetComponent<StateMachine>().facingAngle = 0.0f;
      foreach (GameObject timeCop in this.TimeCops)
        timeCop.transform.position = this.West.transform.position + Vector3.right * -5f + (Vector3) (UnityEngine.Random.insideUnitCircle * 3f);
    }
    else if (RoomManager.PrevCurrentX > RoomManager.CurrentX)
    {
      this.player.transform.position = this.East.transform.position + Vector3.left * 0.5f;
      this.player.GetComponent<StateMachine>().facingAngle = 180f;
      foreach (GameObject timeCop in this.TimeCops)
        timeCop.transform.position = this.East.transform.position + Vector3.left * -5f + (Vector3) (UnityEngine.Random.insideUnitCircle * 3f);
    }
    else if (RoomManager.PrevCurrentY > RoomManager.CurrentY)
    {
      this.player.transform.position = this.North.transform.position + Vector3.down * 0.5f;
      this.player.GetComponent<StateMachine>().facingAngle = 270f;
      foreach (GameObject timeCop in this.TimeCops)
        timeCop.transform.position = this.North.transform.position + Vector3.down * -5f + (Vector3) (UnityEngine.Random.insideUnitCircle * 3f);
    }
    else if (RoomManager.PrevCurrentY < RoomManager.CurrentY)
    {
      this.player.GetComponent<StateMachine>().facingAngle = 90f;
      this.player.transform.position = this.South.transform.position + Vector3.up * 0.5f;
      foreach (GameObject timeCop in this.TimeCops)
        timeCop.transform.position = this.South.transform.position + Vector3.up * -5f + (Vector3) (UnityEngine.Random.insideUnitCircle * 3f);
    }
    GameManager.GetInstance().CameraSnapToPosition(this.player.transform.position);
    GameManager.GetInstance().AddPlayerToCamera();
    if (this.RandomFollowers)
    {
      int count = DataManager.Instance.Followers_Recruit.Count;
      while (++count <= 4)
        ;
    }
    else if (RoomManager.PrevCurrentX == -1 && RoomManager.PrevCurrentY == -1)
    {
      int num = -1;
      do
        ;
      while (++num < DataManager.Instance.Followers_Recruit.Count);
      int index = -1;
      while (++index < DataManager.Instance.Followers_Demons_IDs.Count)
        UnityEngine.Object.Instantiate<GameObject>(Resources.Load(new List<string>()
        {
          "Prefabs/Units/Demons/Demon_Shooty",
          "Prefabs/Units/Demons/Demon_Chomp"
        }[DataManager.Instance.Followers_Demons_Types[index]]) as GameObject, GameObject.FindGameObjectWithTag("Unit Layer").transform, true).transform.position = this.player.transform.position;
    }
    RoomManager.PrevCurrentX = RoomManager.CurrentX;
    RoomManager.PrevCurrentY = RoomManager.CurrentY;
  }

  public void GetDoors()
  {
    this.North = GameObject.FindGameObjectWithTag("North Door");
    this.East = GameObject.FindGameObjectWithTag("East Door");
    this.South = GameObject.FindGameObjectWithTag("South Door");
    this.West = GameObject.FindGameObjectWithTag("West Door");
  }

  public void UnlockDoors() => HUD_Timer.TimerPaused = false;

  public void BlockDoors() => HUD_Timer.TimerPaused = true;

  public void UnbockDoors() => HUD_Timer.TimerPaused = false;

  public void ChangeRoom(Vector3Int Direction)
  {
    RoomManager.CurrentX += Direction.x;
    RoomManager.CurrentY += Direction.y;
    this.ChangeRoom();
  }

  public void ChangeRoom(int X, int Y)
  {
    RoomManager.CurrentX = X;
    RoomManager.CurrentY = Y;
    this.ChangeRoom();
  }

  public bool WithinRoom(Vector3 position)
  {
    return (double) position.x >= (double) this.West.transform.position.x && (double) position.x <= (double) this.East.transform.position.x && (double) position.y <= (double) this.North.transform.position.y && (double) position.y >= (double) this.South.transform.position.y;
  }

  public delegate void InitEnemiesAction();

  public delegate void ChangeRoomDelegate();
}
