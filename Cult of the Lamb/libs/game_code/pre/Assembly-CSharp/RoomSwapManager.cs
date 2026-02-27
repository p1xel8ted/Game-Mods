// Decompiled with JetBrains decompiler
// Type: RoomSwapManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using MMRoomGeneration;
using MMTools;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class RoomSwapManager : BaseMonoBehaviour
{
  public static bool Testing = true;
  public static bool EnterTemple = false;
  public GameObject ChurchEntrance;
  public GameObject RunResults;
  public GameObject Church;
  public bool ChurchUpdatePathfindingAndCollisions;
  public GameObject Room;
  public GameObject Room_1;
  public GameObject Room_2;
  public bool RoomUpdatePathfindingAndCollisions;
  public static bool WalkedBack = false;
  public int TransitionInRoomId = -1;
  public int TransitionOutRoomId = -1;
  public string SoundParam = "";
  public string AtmosSoundParam = "";
  public UnityEvent CallbackOnRoom;
  public UnityEvent CallbackOnChurch;
  public bool ControlWeather;

  private void Awake()
  {
    if (!((Object) this.Church != (Object) null) || this.Church.activeSelf)
      return;
    this.Church.SetActive(true);
  }

  public void ToggleChurch()
  {
    MMTransition.ResumePlay();
    if (this.Church.activeSelf)
      this.ActivateRoom();
    else
      this.ActivateChurch();
  }

  private void ActivateChurch()
  {
    if (this.ControlWeather)
    {
      WeatherController.InsideBuilding = true;
      WeatherController.Instance.CheckWeather();
    }
    AudioManager.Instance.ToggleFilter("inside", true);
    if (this.AtmosSoundParam != "")
      AudioManager.Instance.AdjustAtmosParameter(this.AtmosSoundParam, 1f);
    if (this.SoundParam != "")
    {
      Debug.Log((object) "Set music ID");
      AudioManager.Instance.SetMusicRoomID(this.TransitionInRoomId, this.SoundParam);
    }
    AudioManager.Instance.PlayOneShot("event:/enter_leave_buildings/enter_building", PlayerFarming.Instance.gameObject);
    this.Church.SetActive(true);
    if (this.ChurchUpdatePathfindingAndCollisions)
      GenerateRoom.Instance.SetColliderAndUpdatePathfinding();
    this.Room.gameObject.SetActive(false);
    if ((Object) this.Room_1 != (Object) null)
      this.Room_1.gameObject.SetActive(false);
    if ((Object) this.Room_2 != (Object) null)
      this.Room_2.gameObject.SetActive(false);
    this.CallbackOnChurch?.Invoke();
  }

  private void OnDisable() => AudioManager.Instance.ToggleFilter("inside", false);

  private void ActivateRoom()
  {
    if (this.ControlWeather)
    {
      WeatherController.InsideBuilding = false;
      WeatherController.Instance.CheckWeather();
    }
    AudioManager.Instance.ToggleFilter("inside", false);
    if (this.AtmosSoundParam != "")
      AudioManager.Instance.AdjustAtmosParameter(this.AtmosSoundParam, 0.0f);
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
    if (this.RoomUpdatePathfindingAndCollisions)
      GenerateRoom.Instance.SetColliderAndUpdatePathfinding();
    this.Church.SetActive(false);
    this.CallbackOnRoom?.Invoke();
  }
}
