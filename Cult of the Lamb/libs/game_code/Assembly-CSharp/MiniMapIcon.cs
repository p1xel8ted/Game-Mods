// Decompiled with JetBrains decompiler
// Type: MiniMapIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMBiomeGeneration;
using MMRoomGeneration;
using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class MiniMapIcon : BaseMonoBehaviour
{
  public Text text;
  public RectTransform RT;
  public GameObject QuestionMark;
  public RectTransform IconObject;
  public GameObject North;
  public GameObject East;
  public GameObject South;
  public GameObject West;
  public Image outlineImage;
  public Image image;
  public int X;
  public int Y;
  public RectTransform rectTransform;
  public BiomeRoom room;
  public Image Icon;
  public GameObject IconContainer;

  public bool HasTeleporter => false;

  public void ShowIcon(float opacity = 1f)
  {
    this.SetIcons();
    this.IconObject.gameObject.SetActive(true);
    this.QuestionMark.SetActive(false);
    this.image.color = this.image.color with { a = opacity };
  }

  public void SetSelfToQuestionMark()
  {
    this.IconObject.gameObject.SetActive(false);
    if (this.room.IsRespawnRoom)
      return;
    try
    {
      if ((this.room.N_Room == null || this.room.N_Room.Room == null || !this.room.N_Room.Connected || !this.room.N_Room.Room.Visited || this.room.N_Room.Room.S_Room.Room != this.room || this.room.Visited) && (this.room.E_Room == null || this.room.E_Room.Room == null || !this.room.E_Room.Connected || !this.room.E_Room.Room.Visited || this.room.E_Room.Room.W_Room.Room != this.room || this.room.Visited) && (this.room.S_Room == null || this.room.S_Room.Room == null || !this.room.S_Room.Connected || !this.room.S_Room.Room.Visited || this.room.S_Room.Room.N_Room.Room != this.room || this.room.Visited) && (this.room.W_Room == null || this.room.W_Room.Room == null || !this.room.W_Room.Connected || !this.room.W_Room.Room.Visited || this.room.W_Room.Room.E_Room.Room != this.room || this.room.Visited) || this.room.Hidden)
        return;
      this.QuestionMark.SetActive(true);
    }
    catch (Exception ex)
    {
    }
  }

  public Vector2 Init(BiomeRoom room, Sprite icon, float Scale, Vector3 Position)
  {
    this.room = room;
    this.X = room.x;
    this.Y = room.y;
    this.SetIcons();
    this.outlineImage.enabled = false;
    this.RT.localScale = new Vector3(Scale, Scale);
    this.image.sprite = icon;
    this.RT.localPosition = Position;
    this.rectTransform = this.RT;
    return this.RT.rect.size * (Vector2) this.RT.localScale;
  }

  public void SetIcons()
  {
    this.text.text = "";
    if ((UnityEngine.Object) this.room.generateRoom.MapIcon != (UnityEngine.Object) null)
    {
      this.Icon.gameObject.SetActive(true);
      this.Icon.sprite = this.room.generateRoom.MapIcon;
    }
    else
      this.Icon.gameObject.SetActive(false);
    if (this.room.IsRespawnRoom)
      return;
    this.North.SetActive(this.room.N_Room == null || this.room.N_Room.Connected && (this.room.N_Room.Room == null || !this.room.N_Room.Room.Hidden) && this.room.N_Room.ConnectionType != GenerateRoom.ConnectionTypes.NextLayer);
    this.East.SetActive(this.room.E_Room == null || this.room.E_Room.Connected && (this.room.E_Room.Room == null || !this.room.E_Room.Room.Hidden));
    this.South.SetActive(this.room.S_Room == null || this.room.S_Room.Connected && (this.room.S_Room.Room == null || !this.room.S_Room.Room.Hidden));
    this.West.SetActive(this.room.W_Room == null || this.room.W_Room.Connected && (this.room.W_Room.Room == null || !this.room.W_Room.Room.Hidden));
  }

  public void ShowTeleporter()
  {
  }
}
