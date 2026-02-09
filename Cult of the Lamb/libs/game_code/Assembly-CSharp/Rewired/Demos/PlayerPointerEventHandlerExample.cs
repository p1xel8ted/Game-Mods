// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.PlayerPointerEventHandlerExample
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public const int logLength = 10;
  public List<string> log = new List<string>();

  public void Log(string o)
  {
    this.log.Add(o);
    if (this.log.Count <= 10)
      return;
    this.log.RemoveAt(0);
  }

  public void Update()
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
    this.Log($"OnPointerEnter:  Player = {playerEventData.playerId.ToString()}, Pointer Index = {playerEventData.inputSourceIndex.ToString()}, Source = {PlayerPointerEventHandlerExample.GetSourceName(playerEventData)}");
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    if (!(eventData is PlayerPointerEventData))
      return;
    PlayerPointerEventData playerEventData = (PlayerPointerEventData) eventData;
    this.Log($"OnPointerExit:  Player = {playerEventData.playerId.ToString()}, Pointer Index = {playerEventData.inputSourceIndex.ToString()}, Source = {PlayerPointerEventHandlerExample.GetSourceName(playerEventData)}");
  }

  public void OnPointerUp(PointerEventData eventData)
  {
    if (!(eventData is PlayerPointerEventData))
      return;
    PlayerPointerEventData playerEventData = (PlayerPointerEventData) eventData;
    this.Log($"OnPointerUp:  Player = {playerEventData.playerId.ToString()}, Pointer Index = {playerEventData.inputSourceIndex.ToString()}, Source = {PlayerPointerEventHandlerExample.GetSourceName(playerEventData)}, Button Index = {playerEventData.buttonIndex.ToString()}");
  }

  public void OnPointerDown(PointerEventData eventData)
  {
    if (!(eventData is PlayerPointerEventData))
      return;
    PlayerPointerEventData playerEventData = (PlayerPointerEventData) eventData;
    this.Log($"OnPointerDown:  Player = {playerEventData.playerId.ToString()}, Pointer Index = {playerEventData.inputSourceIndex.ToString()}, Source = {PlayerPointerEventHandlerExample.GetSourceName(playerEventData)}, Button Index = {playerEventData.buttonIndex.ToString()}");
  }

  public void OnPointerClick(PointerEventData eventData)
  {
    if (!(eventData is PlayerPointerEventData))
      return;
    PlayerPointerEventData playerEventData = (PlayerPointerEventData) eventData;
    this.Log($"OnPointerClick:  Player = {playerEventData.playerId.ToString()}, Pointer Index = {playerEventData.inputSourceIndex.ToString()}, Source = {PlayerPointerEventHandlerExample.GetSourceName(playerEventData)}, Button Index = {playerEventData.buttonIndex.ToString()}");
  }

  public void OnScroll(PointerEventData eventData)
  {
    if (!(eventData is PlayerPointerEventData))
      return;
    PlayerPointerEventData playerEventData = (PlayerPointerEventData) eventData;
    this.Log($"OnScroll:  Player = {playerEventData.playerId.ToString()}, Pointer Index = {playerEventData.inputSourceIndex.ToString()}, Source = {PlayerPointerEventHandlerExample.GetSourceName(playerEventData)}");
  }

  public void OnBeginDrag(PointerEventData eventData)
  {
    if (!(eventData is PlayerPointerEventData))
      return;
    PlayerPointerEventData playerEventData = (PlayerPointerEventData) eventData;
    this.Log($"OnBeginDrag:  Player = {playerEventData.playerId.ToString()}, Pointer Index = {playerEventData.inputSourceIndex.ToString()}, Source = {PlayerPointerEventHandlerExample.GetSourceName(playerEventData)}, Button Index = {playerEventData.buttonIndex.ToString()}");
  }

  public void OnDrag(PointerEventData eventData)
  {
    if (!(eventData is PlayerPointerEventData))
      return;
    PlayerPointerEventData playerEventData = (PlayerPointerEventData) eventData;
    this.Log($"OnDrag:  Player = {playerEventData.playerId.ToString()}, Pointer Index = {playerEventData.inputSourceIndex.ToString()}, Source = {PlayerPointerEventHandlerExample.GetSourceName(playerEventData)}, Button Index = {playerEventData.buttonIndex.ToString()}");
  }

  public void OnEndDrag(PointerEventData eventData)
  {
    if (!(eventData is PlayerPointerEventData))
      return;
    PlayerPointerEventData playerEventData = (PlayerPointerEventData) eventData;
    this.Log($"OnEndDrag:  Player = {playerEventData.playerId.ToString()}, Pointer Index = {playerEventData.inputSourceIndex.ToString()}, Source = {PlayerPointerEventHandlerExample.GetSourceName(playerEventData)}, Button Index = {playerEventData.buttonIndex.ToString()}");
  }

  public static string GetSourceName(PlayerPointerEventData playerEventData)
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
