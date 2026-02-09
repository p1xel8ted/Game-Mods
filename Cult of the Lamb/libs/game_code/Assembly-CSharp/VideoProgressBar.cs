// Decompiled with JetBrains decompiler
// Type: VideoProgressBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
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
