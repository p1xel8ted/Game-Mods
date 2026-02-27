// Decompiled with JetBrains decompiler
// Type: TeleportMap
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class TeleportMap : MiniMap
{
  public RectTransform TargetRoom;
  public RectTransform TargetRoomPointer;
  public MiniMapIcon TargetIcon;
  public RectTransform CurrentRoomIcon;
  public float TargetAngle;
  public RectTransform Line;
  public Canvas canvas;
  public static TeleportMap Instance;
  public System.Action Callback;
  public System.Action CallbackCancel;

  public new void Awake()
  {
    TeleportMap.Instance = this;
    this.gameObject.SetActive(false);
  }

  public override void StartMap() => this.TargetAngle = 0.0f;

  public void CenterMap()
  {
    this.IconContainerRect = this.IconContainer.GetComponent<RectTransform>();
    foreach (MiniMapIcon icon in this.Icons)
    {
      if (icon.X == WorldGen.WIDTH / 2 && icon.Y == WorldGen.HEIGHT / 2)
        this.IconContainerRect.localPosition = -icon.rectTransform.localPosition;
      if (icon.X == RoomManager.CurrentX && icon.Y == RoomManager.CurrentY)
        this.CurrentIcon = icon;
    }
    this.TargetRoom.position = -this.CurrentIcon.rectTransform.position;
    this.Line.position = this.CurrentIcon.rectTransform.position;
    this.Line.gameObject.SetActive(false);
    this.CurrentRoomIcon.position = this.CurrentIcon.rectTransform.position;
  }

  public static void Show(System.Action Callback, System.Action CallbackCancel)
  {
    Time.timeScale = 0.0f;
    TeleportMap.Instance.gameObject.SetActive(true);
    TeleportMap.Instance.CenterMap();
    TeleportMap.Instance.Callback = Callback;
    TeleportMap.Instance.CallbackCancel = CallbackCancel;
  }

  public static void Hide()
  {
    Time.timeScale = 1f;
    TeleportMap.Instance.gameObject.SetActive(false);
  }

  public void Update()
  {
    if ((double) Mathf.Abs(InputManager.UI.GetHorizontalAxis()) > 0.20000000298023224 || (double) Mathf.Abs(InputManager.UI.GetVerticalAxis()) > 0.20000000298023224)
    {
      this.TargetAngle = Utils.GetAngle(Vector3.zero, new Vector3(InputManager.UI.GetHorizontalAxis(), InputManager.UI.GetHorizontalAxis()));
      this.TargetRoomPointer.position = this.CurrentIcon.rectTransform.position + new Vector3(100f * Mathf.Cos(this.TargetAngle * ((float) Math.PI / 180f)), 100f * Mathf.Sin(this.TargetAngle * ((float) Math.PI / 180f)));
      this.GetClosestIcon();
      if ((UnityEngine.Object) this.TargetIcon != (UnityEngine.Object) null)
      {
        this.TargetRoom.position = this.TargetIcon.rectTransform.position;
        this.Line.gameObject.SetActive(true);
        this.Line.eulerAngles = new Vector3(0.0f, 0.0f, Utils.GetAngle(this.CurrentIcon.rectTransform.position, this.TargetRoom.position));
        this.Line.sizeDelta = new Vector2(Vector3.Distance(this.CurrentIcon.rectTransform.position, this.TargetRoom.position) / this.canvas.scaleFactor, this.Line.sizeDelta.y);
      }
    }
    if ((UnityEngine.Object) this.TargetIcon != (UnityEngine.Object) null && InputManager.UI.GetAcceptButtonDown())
    {
      RoomManager.CurrentX = this.TargetIcon.X;
      RoomManager.CurrentY = this.TargetIcon.Y;
      if (this.Callback != null)
        this.Callback();
      TeleportMap.Hide();
    }
    if (!InputManager.UI.GetCancelButtonUp())
      return;
    if (this.CallbackCancel != null)
      this.CallbackCancel();
    TeleportMap.Hide();
  }

  public void GetClosestIcon()
  {
    foreach (MiniMapIcon icon in this.Icons)
    {
      Mathf.Abs(Utils.GetAngle(this.CurrentIcon.rectTransform.position, icon.rectTransform.position) - this.TargetAngle);
      if (icon.gameObject.activeSelf && (UnityEngine.Object) icon != (UnityEngine.Object) this.CurrentIcon)
        this.TargetIcon = icon;
    }
  }
}
