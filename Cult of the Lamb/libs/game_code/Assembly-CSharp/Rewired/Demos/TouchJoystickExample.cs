// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.TouchJoystickExample
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
namespace Rewired.Demos;

[AddComponentMenu("")]
[RequireComponent(typeof (Image))]
public class TouchJoystickExample : 
  MonoBehaviour,
  IPointerDownHandler,
  IEventSystemHandler,
  IPointerUpHandler,
  IDragHandler
{
  public bool allowMouseControl = true;
  public int radius = 50;
  public Vector2 origAnchoredPosition;
  public Vector3 origWorldPosition;
  public Vector2 origScreenResolution;
  public ScreenOrientation origScreenOrientation;
  [NonSerialized]
  public bool hasFinger;
  [NonSerialized]
  public int lastFingerId;
  [CompilerGenerated]
  public Vector2 \u003Cposition\u003Ek__BackingField;

  public Vector2 position
  {
    get => this.\u003Cposition\u003Ek__BackingField;
    set => this.\u003Cposition\u003Ek__BackingField = value;
  }

  public void Start()
  {
    if (SystemInfo.deviceType == DeviceType.Handheld)
      this.allowMouseControl = false;
    this.StoreOrigValues();
  }

  public void Update()
  {
    if ((double) Screen.width == (double) this.origScreenResolution.x && (double) Screen.height == (double) this.origScreenResolution.y && Screen.orientation == this.origScreenOrientation)
      return;
    this.Restart();
    this.StoreOrigValues();
  }

  public void Restart()
  {
    this.hasFinger = false;
    (this.transform as RectTransform).anchoredPosition = this.origAnchoredPosition;
    this.position = Vector2.zero;
  }

  public void StoreOrigValues()
  {
    this.origAnchoredPosition = (this.transform as RectTransform).anchoredPosition;
    this.origWorldPosition = this.transform.position;
    this.origScreenResolution = new Vector2((float) Screen.width, (float) Screen.height);
    this.origScreenOrientation = Screen.orientation;
  }

  public void UpdateValue(Vector3 value)
  {
    Vector3 vector3_1 = this.origWorldPosition - value;
    vector3_1.y = -vector3_1.y;
    Vector3 vector3_2 = vector3_1 / (float) this.radius;
    this.position = new Vector2(-vector3_2.x, vector3_2.y);
  }

  void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
  {
    if (this.hasFinger || !this.allowMouseControl && TouchJoystickExample.IsMousePointerId(eventData.pointerId))
      return;
    this.hasFinger = true;
    this.lastFingerId = eventData.pointerId;
  }

  void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
  {
    if (eventData.pointerId != this.lastFingerId || !this.allowMouseControl && TouchJoystickExample.IsMousePointerId(eventData.pointerId))
      return;
    this.Restart();
  }

  void IDragHandler.OnDrag(PointerEventData eventData)
  {
    if (!this.hasFinger || eventData.pointerId != this.lastFingerId)
      return;
    Vector3 vector3 = this.origWorldPosition + Vector3.ClampMagnitude(new Vector3(eventData.position.x - this.origWorldPosition.x, eventData.position.y - this.origWorldPosition.y), (float) this.radius);
    this.transform.position = vector3;
    this.UpdateValue(vector3);
  }

  public static bool IsMousePointerId(int id) => id == -1 || id == -2 || id == -3;
}
