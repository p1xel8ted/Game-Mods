// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.TouchButtonExample
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
namespace Rewired.Demos;

[AddComponentMenu("")]
[RequireComponent(typeof (Image))]
public class TouchButtonExample : 
  MonoBehaviour,
  IPointerDownHandler,
  IEventSystemHandler,
  IPointerUpHandler
{
  public bool allowMouseControl = true;
  [CompilerGenerated]
  public bool \u003CisPressed\u003Ek__BackingField;

  public bool isPressed
  {
    get => this.\u003CisPressed\u003Ek__BackingField;
    set => this.\u003CisPressed\u003Ek__BackingField = value;
  }

  public void Awake()
  {
    if (SystemInfo.deviceType != DeviceType.Handheld)
      return;
    this.allowMouseControl = false;
  }

  public void Restart() => this.isPressed = false;

  void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
  {
    if (!this.allowMouseControl && TouchButtonExample.IsMousePointerId(eventData.pointerId))
      return;
    this.isPressed = true;
  }

  void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
  {
    if (!this.allowMouseControl && TouchButtonExample.IsMousePointerId(eventData.pointerId))
      return;
    this.isPressed = false;
  }

  public static bool IsMousePointerId(int id) => id == -1 || id == -2 || id == -3;
}
