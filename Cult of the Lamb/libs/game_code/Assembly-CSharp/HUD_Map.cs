// Decompiled with JetBrains decompiler
// Type: HUD_Map
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class HUD_Map : BaseMonoBehaviour
{
  public Transform IconParent;
  public GameObject TargetRoom;
  public GameObject TargetRoomPointer;
  public HUD_Map_Icon CurrentRoom;
  public HUD_Map_Icon CurrentTarget;
  public GameObject RoomIcon;
  public GameObject DoorIcon;
  public GameObject PlayerIcon;
  public float Timer;
  public List<GameObject> icons;
  public bool OnlyShowVisited;

  public void OnEnable()
  {
    this.icons = new List<GameObject>();
    if (WorldGen.rooms == null)
      return;
    foreach (Room room in WorldGen.rooms)
    {
      HUD_Map_Icon component = Object.Instantiate<GameObject>(this.RoomIcon, this.IconParent, true).GetComponent<HUD_Map_Icon>();
      component.gameObject.GetComponent<RectTransform>().localPosition = this.RoomPosition(room.x, room.y);
      float Delay = Vector3.Distance(this.RoomPosition(RoomManager.CurrentX, RoomManager.CurrentY), this.RoomPosition(room.x, room.y));
      if (room.isHome)
        component.SetImage(HUD_Map_Icon.RoomType.HOME, Delay, room);
      else if (room.isEntranceHall)
        component.SetImage(HUD_Map_Icon.RoomType.ENTRANCE_HALLWAY, Delay, room);
      else if (room.pointOfInterest)
        component.SetImage(HUD_Map_Icon.RoomType.POINT_OF_INTEREST, Delay, room);
      else
        component.SetImage(HUD_Map_Icon.RoomType.ROOM, Delay, room);
      if (this.OnlyShowVisited && !room.visited)
        component.gameObject.SetActive(false);
      this.icons.Add(component.gameObject);
    }
    foreach (Room room in WorldGen.rooms)
    {
      if ((Object) room.N_Link != (Object) null)
      {
        HUD_Map_Icon component = Object.Instantiate<GameObject>(this.DoorIcon, this.transform, true).GetComponent<HUD_Map_Icon>();
        component.gameObject.GetComponent<RectTransform>().localPosition = new Vector3((float) (room.x * 205 - WorldGen.WIDTH * 100), (float) (room.y * 155 - WorldGen.HEIGHT * 75 + 80 /*0x50*/));
        float Delay = Vector2.Distance(new Vector2((float) NavigateRooms.CurrentX, (float) NavigateRooms.CurrentY), new Vector2((float) room.x, (float) room.y)) * 0.05f;
        component.SetImage(HUD_Map_Icon.RoomType.DOOR, Delay, (Room) null);
        this.icons.Add(component.gameObject);
        if (this.OnlyShowVisited && !room.visited)
          component.gameObject.SetActive(false);
      }
      if ((Object) room.E_Link != (Object) null)
      {
        HUD_Map_Icon component = Object.Instantiate<GameObject>(this.DoorIcon, this.transform, true).GetComponent<HUD_Map_Icon>();
        component.gameObject.GetComponent<RectTransform>().localPosition = new Vector3((float) (room.x * 205 - WorldGen.WIDTH * 100 + 100), (float) (room.y * 155 - WorldGen.HEIGHT * 75));
        float Delay = Vector2.Distance(new Vector2((float) NavigateRooms.CurrentX, (float) NavigateRooms.CurrentY), new Vector2((float) room.x, (float) room.y)) * 0.05f;
        component.SetImage(HUD_Map_Icon.RoomType.DOOR, Delay, (Room) null);
        this.icons.Add(component.gameObject);
        if (this.OnlyShowVisited && !room.visited)
          component.gameObject.SetActive(false);
      }
      if ((Object) room.S_Link != (Object) null)
      {
        HUD_Map_Icon component = Object.Instantiate<GameObject>(this.DoorIcon, this.transform, true).GetComponent<HUD_Map_Icon>();
        component.gameObject.GetComponent<RectTransform>().localPosition = new Vector3((float) (room.x * 205 - WorldGen.WIDTH * 100), (float) (room.y * 155 - WorldGen.HEIGHT * 75 - 80 /*0x50*/));
        float Delay = Vector2.Distance(new Vector2((float) NavigateRooms.CurrentX, (float) NavigateRooms.CurrentY), new Vector2((float) room.x, (float) room.y)) * 0.05f;
        component.SetImage(HUD_Map_Icon.RoomType.DOOR, Delay, (Room) null);
        this.icons.Add(component.gameObject);
        if (this.OnlyShowVisited && !room.visited)
          component.gameObject.SetActive(false);
      }
      if ((Object) room.W_Link != (Object) null)
      {
        HUD_Map_Icon component = Object.Instantiate<GameObject>(this.DoorIcon, this.transform, true).GetComponent<HUD_Map_Icon>();
        component.gameObject.GetComponent<RectTransform>().localPosition = new Vector3((float) (room.x * 205 - WorldGen.WIDTH * 100 - 100), (float) (room.y * 155 - WorldGen.HEIGHT * 75));
        float Delay = Vector2.Distance(new Vector2((float) NavigateRooms.CurrentX, (float) NavigateRooms.CurrentY), new Vector2((float) room.x, (float) room.y)) * 0.05f;
        component.SetImage(HUD_Map_Icon.RoomType.DOOR, Delay, (Room) null);
        this.icons.Add(component.gameObject);
        if (this.OnlyShowVisited && !room.visited)
          component.gameObject.SetActive(false);
      }
    }
    GameObject gameObject = Object.Instantiate<GameObject>(this.PlayerIcon, this.transform, true);
    gameObject.GetComponent<RectTransform>().localPosition = new Vector3((float) (RoomManager.CurrentX * 205 - WorldGen.WIDTH * 100), (float) (RoomManager.CurrentY * 155 - WorldGen.HEIGHT * 75));
    this.icons.Add(gameObject.gameObject);
    this.CurrentRoom = HUD_Map_Icon.GetIconByRoom(RoomManager.r);
    this.TargetRoom.transform.localPosition = this.RoomPosition(this.CurrentRoom.Room.x, this.CurrentRoom.Room.y);
  }

  public Vector3 RoomPosition(int x, int y)
  {
    return new Vector3((float) (x * 205 - WorldGen.WIDTH * 100), (float) (y * 155 - WorldGen.HEIGHT * 75));
  }

  public void OnDisable()
  {
    foreach (GameObject icon in this.icons)
      Object.Destroy((Object) icon.gameObject);
    this.icons.Clear();
    this.icons = (List<GameObject>) null;
  }

  public void Update()
  {
    if (!InputManager.Gameplay.GetAttackButtonUp())
      return;
    this.gameObject.SetActive(false);
  }
}
