// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.TouchButtonExample
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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

  public bool isPressed { get; private set; }

  private void Awake()
  {
    if (SystemInfo.deviceType != DeviceType.Handheld)
      return;
    this.allowMouseControl = false;
  }

  private void Restart() => this.isPressed = false;

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

  private static bool IsMousePointerId(int id) => id == -1 || id == -2 || id == -3;
}
