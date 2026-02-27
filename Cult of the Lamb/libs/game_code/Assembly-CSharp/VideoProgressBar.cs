// Decompiled with JetBrains decompiler
// Type: VideoProgressBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using LightShaft.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
public class VideoProgressBar : MonoBehaviour, IDragHandler, IEventSystemHandler, IPointerDownHandler
{
  public YoutubePlayer player;

  public void OnDrag(PointerEventData eventData) => this.player.TrySkip(eventData);

  public void OnPointerDown(PointerEventData eventData) => this.player.TrySkip(eventData);
}
