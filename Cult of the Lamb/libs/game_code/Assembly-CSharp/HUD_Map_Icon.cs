// Decompiled with JetBrains decompiler
// Type: HUD_Map_Icon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class HUD_Map_Icon : BaseMonoBehaviour
{
  public Sprite Home;
  public Sprite EntranceHallway;
  public Sprite RoomSprite;
  public Sprite Door;
  public Sprite PointOfInterest;
  public Image image;
  public float Delay;
  public Room Room;
  public static List<HUD_Map_Icon> Icons = new List<HUD_Map_Icon>();
  public float scale;
  public float scaleSpeed;
  public float TargetScale = 1f;

  public void SetImage(HUD_Map_Icon.RoomType type, float Delay, Room Room)
  {
    this.Delay = Delay;
    switch (type)
    {
      case HUD_Map_Icon.RoomType.HOME:
        this.image.sprite = this.Home;
        break;
      case HUD_Map_Icon.RoomType.ENTRANCE_HALLWAY:
        this.image.sprite = this.EntranceHallway;
        break;
      case HUD_Map_Icon.RoomType.ROOM:
        this.image.sprite = this.RoomSprite;
        break;
      case HUD_Map_Icon.RoomType.DOOR:
        this.image.sprite = this.Door;
        break;
      case HUD_Map_Icon.RoomType.POINT_OF_INTEREST:
        this.image.sprite = this.PointOfInterest;
        break;
    }
    this.image.SetNativeSize();
    this.Room = Room;
  }

  public static HUD_Map_Icon GetIconByRoom(Room room)
  {
    foreach (HUD_Map_Icon icon in HUD_Map_Icon.Icons)
    {
      if ((Object) icon.Room == (Object) room)
        return icon;
    }
    return (HUD_Map_Icon) null;
  }

  public void OnEnable()
  {
    this.transform.localScale = Vector3.zero;
    HUD_Map_Icon.Icons.Add(this);
  }

  public void OnDisable()
  {
    this.Room = (Room) null;
    HUD_Map_Icon.Icons.Remove(this);
  }

  public void Update()
  {
    if ((double) (this.Delay -= Time.deltaTime) > 0.0)
      return;
    this.scaleSpeed += (float) (((double) this.TargetScale - (double) this.scale) * 0.30000001192092896);
    this.scale += (this.scaleSpeed *= 0.7f);
    this.transform.localScale = new Vector3(this.scale, this.scale, 1f);
  }

  public enum RoomType
  {
    HOME,
    ENTRANCE_HALLWAY,
    ROOM,
    DOOR,
    POINT_OF_INTEREST,
  }
}
