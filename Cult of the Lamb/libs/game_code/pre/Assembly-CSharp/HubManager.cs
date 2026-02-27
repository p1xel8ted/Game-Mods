// Decompiled with JetBrains decompiler
// Type: HubManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using FMODUnity;
using I2.Loc;
using MMBiomeGeneration;
using MMRoomGeneration;
using MMTools;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.U2D;

#nullable disable
public class HubManager : BaseMonoBehaviour
{
  public HubLocationManager HubLocationManager;
  public GenerateRoom Room;
  [TermsPopup("")]
  public string DisplayName;
  public bool GenerateOnLoad;
  public bool InitiateDoors;
  public bool DoFirstArrivalRoutine;
  public static HubManager Instance;
  public int hubMusicParameter = -1;
  [EventRef]
  public string hubMusicPath;
  [EventRef]
  public string hubAtmosPath;
  public bool ToggleInside;
  private EventInstance hubAtmosInstance;
  public Door North;
  public Door East;
  public Door South;
  public Door West;
  private GameObject Player;
  private StateMachine PlayerState;

  private void OnEnable() => HubManager.Instance = this;

  private void Start() => this.StartCoroutine((IEnumerator) this.PlaceAndPositionPlayer());

  private void InitSpriteShapes()
  {
    SpriteShapeRenderer[] objectsOfType = (SpriteShapeRenderer[]) UnityEngine.Object.FindObjectsOfType((System.Type) typeof (SpriteShapeRenderer));
    CommandBuffer buffer = new CommandBuffer();
    buffer.GetTemporaryRT(0, 256 /*0x0100*/, 256 /*0x0100*/, 0);
    buffer.SetRenderTarget((RenderTargetIdentifier) 0);
    foreach (SpriteShapeRenderer spriteShapeRenderer in objectsOfType)
    {
      SpriteShapeController component = spriteShapeRenderer.gameObject.GetComponent<SpriteShapeController>();
      if ((UnityEngine.Object) spriteShapeRenderer != (UnityEngine.Object) null && (UnityEngine.Object) component != (UnityEngine.Object) null && !spriteShapeRenderer.isVisible)
      {
        component.BakeMesh();
        buffer.DrawRenderer((Renderer) spriteShapeRenderer, spriteShapeRenderer.sharedMaterial);
      }
    }
    buffer.ReleaseTemporaryRT(0);
    Graphics.ExecuteCommandBuffer(buffer);
  }

  private void InitDoors()
  {
    this.North = GameObject.FindGameObjectWithTag("North Door")?.GetComponent<Door>();
    this.East = GameObject.FindGameObjectWithTag("East Door")?.GetComponent<Door>();
    this.South = GameObject.FindGameObjectWithTag("South Door")?.GetComponent<Door>();
    this.West = GameObject.FindGameObjectWithTag("West Door")?.GetComponent<Door>();
    if (this.Room.North != GenerateRoom.ConnectionTypes.False)
      this.North?.Init(this.Room.North);
    if (this.Room.East != GenerateRoom.ConnectionTypes.False)
      this.East?.Init(this.Room.East);
    if (this.Room.South != GenerateRoom.ConnectionTypes.False)
      this.South?.Init(this.Room.South);
    if (this.Room.West == GenerateRoom.ConnectionTypes.False)
      return;
    this.West?.Init(this.Room.West);
  }

  private IEnumerator PlaceAndPositionPlayer()
  {
    HubManager hubManager = this;
    while (LocationManager.GetLocationState(hubManager.HubLocationManager.Location) != LocationState.Active)
      yield return (object) null;
    hubManager.Room.SetColliderAndUpdatePathfinding();
    if ((UnityEngine.Object) BiomeGenerator.Instance == (UnityEngine.Object) null)
    {
      AudioManager.Instance.PlayMusic(hubManager.hubMusicPath);
      AudioManager.Instance.PlayAtmos(hubManager.hubAtmosPath);
      AudioManager.Instance.SetMusicCombatState(false);
      if (hubManager.hubMusicParameter == -1)
        AudioManager.Instance.SetMusicRoomID(hubManager.Room.roomMusicID);
      else
        AudioManager.Instance.SetMusicRoomID(hubManager.hubMusicParameter, "shore_id");
      if (hubManager.ToggleInside)
        AudioManager.Instance.ToggleFilter("inside", true);
    }
    hubManager.Player = hubManager.HubLocationManager.PlacePlayer();
    GameManager.GetInstance().CameraSnapToPosition(PlayerFarming.Instance.CameraBone.transform.position);
    yield return (object) new WaitForEndOfFrame();
    if (hubManager.GenerateOnLoad)
      hubManager.InitDoors();
    else if (hubManager.InitiateDoors)
      hubManager.InitDoors();
    if (hubManager.DoFirstArrivalRoutine)
    {
      GameManager.GetInstance().OnConversationNew(SnapLetterBox: true);
      GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone, 6f);
      GameManager.GetInstance().CameraSetZoom(6f);
      GameManager.GetInstance().CameraSetOffset(new Vector3(0.0f, 0.0f, -1f));
      hubManager.Player.transform.position = Door.GetEntranceDoor().PlayerPosition.position;
      hubManager.PlayerState = hubManager.Player.GetComponent<StateMachine>();
      hubManager.StartCoroutine((IEnumerator) hubManager.DelayPlayerGoToAndStop());
    }
    MMTransition.ResumePlay();
    SimulationManager.UnPause();
    yield return (object) new WaitForEndOfFrame();
    HUD_Manager.Instance.Hidden = false;
    HUD_Manager.Instance.Hide(true, 0, true);
  }

  private IEnumerator DelayPlayerGoToAndStop()
  {
    yield return (object) new WaitForSeconds(0.5f);
    Door door = Door.GetEntranceDoor();
    float entranceGoToDistance = door.EntranceGoToDistance;
    Vector3 TargetPosition = PlayerFarming.Instance.transform.position + door.GetDoorDirection() * entranceGoToDistance;
    PlayerFarming.Instance.GoToAndStop(TargetPosition, IdleOnEnd: true, GoToCallback: (System.Action) (() =>
    {
      door = Door.GetFirstNonEntranceDoor();
      if ((UnityEngine.Object) door != (UnityEngine.Object) null)
        this.PlayerState.facingAngle = Utils.GetAngle(this.PlayerState.transform.position, door.transform.position);
      this.StartCoroutine((IEnumerator) this.DelayEndConversation());
    }));
    yield return (object) new WaitForSeconds(0.5f);
  }

  private IEnumerator DelayEndConversation()
  {
    yield return (object) new WaitForSeconds(0.3f);
    GameManager.GetInstance().OnConversationEnd(false);
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
  }

  private void OnDestroy()
  {
    AudioManager.Instance.StopCurrentMusic();
    AudioManager.Instance.StopCurrentAtmos();
  }
}
