// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.PlayerPointerEventHandlerExample
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Rewired.Integration.UnityUI;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
namespace Rewired.Demos;

[AddComponentMenu("")]
public sealed class PlayerPointerEventHandlerExample : 
  MonoBehaviour,
  IPointerEnterHandler,
  IEventSystemHandler,
  IPointerExitHandler,
  IPointerUpHandler,
  IPointerDownHandler,
  IPointerClickHandler,
  IScrollHandler,
  IBeginDragHandler,
  IDragHandler,
  IEndDragHandler
{
  public UnityEngine.UI.Text text;
  private const int logLength = 10;
  private List<string> log = new List<string>();

  private void Log(string o)
  {
    this.log.Add(o);
    if (this.log.Count <= 10)
      return;
    this.log.RemoveAt(0);
  }

  private void Update()
  {
    if (!((Object) this.text != (Object) null))
      return;
    StringBuilder stringBuilder = new StringBuilder();
    foreach (string str in this.log)
      stringBuilder.AppendLine(str);
    this.text.text = stringBuilder.ToString();
  }

  public void OnPointerEnter(PointerEventData eventData)
  {
    if (!(eventData is PlayerPointerEventData))
      return;
    PlayerPointerEventData playerEventData = (PlayerPointerEventData) eventData;
    this.Log($"OnPointerEnter:  Player = {(object) playerEventData.playerId}, Pointer Index = {(object) playerEventData.inputSourceIndex}, Source = {PlayerPointerEventHandlerExample.GetSourceName(playerEventData)}");
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    if (!(eventData is PlayerPointerEventData))
      return;
    PlayerPointerEventData playerEventData = (PlayerPointerEventData) eventData;
    this.Log($"OnPointerExit:  Player = {(object) playerEventData.playerId}, Pointer Index = {(object) playerEventData.inputSourceIndex}, Source = {PlayerPointerEventHandlerExample.GetSourceName(playerEventData)}");
  }

  public void OnPointerUp(PointerEventData eventData)
  {
    if (!(eventData is PlayerPointerEventData))
      return;
    PlayerPointerEventData playerEventData = (PlayerPointerEventData) eventData;
    this.Log($"OnPointerUp:  Player = {(object) playerEventData.playerId}, Pointer Index = {(object) playerEventData.inputSourceIndex}, Source = {PlayerPointerEventHandlerExample.GetSourceName(playerEventData)}, Button Index = {(object) playerEventData.buttonIndex}");
  }

  public void OnPointerDown(PointerEventData eventData)
  {
    if (!(eventData is PlayerPointerEventData))
      return;
    PlayerPointerEventData playerEventData = (PlayerPointerEventData) eventData;
    this.Log($"OnPointerDown:  Player = {(object) playerEventData.playerId}, Pointer Index = {(object) playerEventData.inputSourceIndex}, Source = {PlayerPointerEventHandlerExample.GetSourceName(playerEventData)}, Button Index = {(object) playerEventData.buttonIndex}");
  }

  public void OnPointerClick(PointerEventData eventData)
  {
    if (!(eventData is PlayerPointerEventData))
      return;
    PlayerPointerEventData playerEventData = (PlayerPointerEventData) eventData;
    this.Log($"OnPointerClick:  Player = {(object) playerEventData.playerId}, Pointer Index = {(object) playerEventData.inputSourceIndex}, Source = {PlayerPointerEventHandlerExample.GetSourceName(playerEventData)}, Button Index = {(object) playerEventData.buttonIndex}");
  }

  public void OnScroll(PointerEventData eventData)
  {
    if (!(eventData is PlayerPointerEventData))
      return;
    PlayerPointerEventData playerEventData = (PlayerPointerEventData) eventData;
    this.Log($"OnScroll:  Player = {(object) playerEventData.playerId}, Pointer Index = {(object) playerEventData.inputSourceIndex}, Source = {PlayerPointerEventHandlerExample.GetSourceName(playerEventData)}");
  }

  public void OnBeginDrag(PointerEventData eventData)
  {
    if (!(eventData is PlayerPointerEventData))
      return;
    PlayerPointerEventData playerEventData = (PlayerPointerEventData) eventData;
    this.Log($"OnBeginDrag:  Player = {(object) playerEventData.playerId}, Pointer Index = {(object) playerEventData.inputSourceIndex}, Source = {PlayerPointerEventHandlerExample.GetSourceName(playerEventData)}, Button Index = {(object) playerEventData.buttonIndex}");
  }

  public void OnDrag(PointerEventData eventData)
  {
    if (!(eventData is PlayerPointerEventData))
      return;
    PlayerPointerEventData playerEventData = (PlayerPointerEventData) eventData;
    this.Log($"OnDrag:  Player = {(object) playerEventData.playerId}, Pointer Index = {(object) playerEventData.inputSourceIndex}, Source = {PlayerPointerEventHandlerExample.GetSourceName(playerEventData)}, Button Index = {(object) playerEventData.buttonIndex}");
  }

  public void OnEndDrag(PointerEventData eventData)
  {
    if (!(eventData is PlayerPointerEventData))
      return;
    PlayerPointerEventData playerEventData = (PlayerPointerEventData) eventData;
    this.Log($"OnEndDrag:  Player = {(object) playerEventData.playerId}, Pointer Index = {(object) playerEventData.inputSourceIndex}, Source = {PlayerPointerEventHandlerExample.GetSourceName(playerEventData)}, Button Index = {(object) playerEventData.buttonIndex}");
  }

  private static string GetSourceName(PlayerPointerEventData playerEventData)
  {
    if (playerEventData.sourceType == PointerEventType.Mouse)
    {
      if (playerEventData.mouseSource is Behaviour)
        return (playerEventData.mouseSource as Behaviour).name;
    }
    else if (playerEventData.sourceType == PointerEventType.Touch && playerEventData.touchSource is Behaviour)
      return (playerEventData.touchSource as Behaviour).name;
    return (string) null;
  }
}
