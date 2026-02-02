// Decompiled with JetBrains decompiler
// Type: RoomSwapManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMRoomGeneration;
using MMTools;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class RoomSwapManager : BaseMonoBehaviour
{
  public static bool Testing = true;
  public static bool EnterTemple = false;
  public GameObject ChurchEntrance;
  public GameObject RunResults;
  public GameObject Church;
  public AssetReferenceGameObject Church_Addr;
  public GameObject ChurchParent;
  public EnterBuilding enterBuilding;
  public bool ChurchUpdatePathfindingAndCollisions;
  public bool ChurchUpadteGenerateRoomPathfinding = true;
  public GameObject Room;
  public GameObject Room_1;
  public GameObject Room_2;
  public bool RoomUpdatePathfindingAndCollisions;
  public bool RoomUpadteGenerateRoomPathfinding = true;
  public static bool WalkedBack = false;
  public int TransitionInRoomId = -1;
  public int TransitionOutRoomId = -1;
  public string SoundParam = "";
  public string AtmosSoundParam = "";
  public bool setParamFalseOnDisable = true;
  public UnityEvent CallbackOnRoom;
  public UnityEvent CallbackOnChurch;
  public AsyncOperationHandle<GameObject> addrHandle;
  public bool ControlWeather;

  public void Awake()
  {
    if (!((Object) this.Church != (Object) null) || this.Church.activeSelf)
      return;
    this.Church.SetActive(true);
  }

  public void ToggleChurch()
  {
    MMTransition.ResumePlay();
    if ((Object) this.Church != (Object) null && this.Church.activeSelf)
      this.ActivateRoom();
    else
      this.StartCoroutine((IEnumerator) this.ActivateChurch());
  }

  public IEnumerator ActivateChurch()
  {
    RoomSwapManager roomSwapManager = this;
    if (roomSwapManager.ControlWeather)
      WeatherSystemController.Instance.EnteredBuilding();
    AudioManager.Instance.ToggleFilter("inside", true);
    if (roomSwapManager.AtmosSoundParam != "")
      AudioManager.Instance.AdjustAtmosParameter(roomSwapManager.AtmosSoundParam, 1f);
    if (roomSwapManager.SoundParam != "")
    {
      Debug.Log((object) "Set music ID");
      AudioManager.Instance.SetMusicRoomID(roomSwapManager.TransitionInRoomId, roomSwapManager.SoundParam);
    }
    AudioManager.Instance.PlayOneShot("event:/enter_leave_buildings/enter_building", PlayerFarming.Instance.gameObject);
    if (SeasonsManager.CurrentWeatherEvent == SeasonsManager.WeatherEvent.Blizzard)
      AudioManager.Instance.StopCurrentAtmos();
    yield return (object) roomSwapManager.StartCoroutine((IEnumerator) roomSwapManager.LoadAddressableAsset());
    yield return (object) null;
    roomSwapManager.Church.SetActive(true);
    if (roomSwapManager.ChurchUpdatePathfindingAndCollisions && roomSwapManager.ChurchUpadteGenerateRoomPathfinding)
      GenerateRoom.Instance.SetColliderAndUpdatePathfinding();
    roomSwapManager.Room.gameObject.SetActive(false);
    if ((Object) roomSwapManager.Room_1 != (Object) null)
      roomSwapManager.Room_1.gameObject.SetActive(false);
    if ((Object) roomSwapManager.Room_2 != (Object) null)
      roomSwapManager.Room_2.gameObject.SetActive(false);
    if (roomSwapManager.ChurchUpdatePathfindingAndCollisions && !roomSwapManager.ChurchUpadteGenerateRoomPathfinding)
      GameManager.RecalculatePaths(true, false);
    roomSwapManager.CallbackOnChurch?.Invoke();
  }

  public void OnDisable()
  {
    if (!this.setParamFalseOnDisable)
      return;
    AudioManager.Instance.ToggleFilter("inside", false);
  }

  public IEnumerator LoadAddressableAsset()
  {
    RoomSwapManager roomSwapManager = this;
    if (roomSwapManager.Church_Addr.RuntimeKeyIsValid() && !roomSwapManager.addrHandle.IsValid())
    {
      roomSwapManager.addrHandle = Addressables.LoadAssetAsync<GameObject>((object) roomSwapManager.Church_Addr);
      yield return (object) roomSwapManager.addrHandle.WaitForCompletion();
      roomSwapManager.Church = Object.Instantiate<GameObject>(roomSwapManager.addrHandle.Result, roomSwapManager.ChurchParent.transform);
      roomSwapManager.enterBuilding = roomSwapManager.Church.GetComponentInChildren<EnterBuilding>();
      roomSwapManager.enterBuilding.Trigger.AddListener(new UnityAction(roomSwapManager.\u003CLoadAddressableAsset\u003Eb__29_0));
      RevealJobBoard componentInChildren = roomSwapManager.Church.GetComponentInChildren<RevealJobBoard>();
      if ((Object) componentInChildren != (Object) null)
      {
        DecoJobBoardManager firstObjectByType = Object.FindFirstObjectByType<DecoJobBoardManager>(FindObjectsInactive.Include);
        if ((Object) firstObjectByType != (Object) null)
        {
          componentInChildren.SetJobBoard(firstObjectByType.gameObject);
          componentInChildren.Initialize();
        }
      }
    }
  }

  public void UnloadAddressableAsset()
  {
    if (!this.addrHandle.IsValid())
      return;
    if ((Object) this.enterBuilding != (Object) null)
      this.enterBuilding.Trigger.RemoveAllListeners();
    Object.Destroy((Object) this.Church.gameObject);
    Addressables.Release<GameObject>(this.addrHandle);
  }

  public void ActivateRoom()
  {
    if (this.ControlWeather)
      WeatherSystemController.Instance.ExitedBuilding();
    AudioManager.Instance.ToggleFilter("inside", false);
    if (this.AtmosSoundParam != "")
      AudioManager.Instance.AdjustAtmosParameter(this.AtmosSoundParam, 0.0f);
    else if (SeasonsManager.CurrentWeatherEvent == SeasonsManager.WeatherEvent.Blizzard)
      AudioManager.Instance.PlayCurrentAtmos();
    if (this.SoundParam != "")
    {
      Debug.Log((object) "Set music ID");
      AudioManager.Instance.SetMusicRoomID(this.TransitionOutRoomId, this.SoundParam);
    }
    AudioManager.Instance.PlayOneShot("event:/enter_leave_buildings/leave_building", PlayerFarming.Instance.gameObject);
    this.Room.gameObject.SetActive(true);
    if ((Object) this.Room_1 != (Object) null)
      this.Room_1.gameObject.SetActive(true);
    if ((Object) this.Room_2 != (Object) null)
      this.Room_2.gameObject.SetActive(true);
    this.Church.SetActive(false);
    if (this.RoomUpdatePathfindingAndCollisions)
    {
      if (this.RoomUpadteGenerateRoomPathfinding)
        GenerateRoom.Instance.SetColliderAndUpdatePathfinding();
      else
        GameManager.RecalculatePaths(true, false);
    }
    this.CallbackOnRoom?.Invoke();
    this.UnloadAddressableAsset();
  }

  [CompilerGenerated]
  public void \u003CLoadAddressableAsset\u003Eb__29_0() => this.ToggleChurch();
}
