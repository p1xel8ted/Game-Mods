// Decompiled with JetBrains decompiler
// Type: VideoProgressBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
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
