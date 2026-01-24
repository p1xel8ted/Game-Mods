// Decompiled with JetBrains decompiler
// Type: LightShaft.Scripts.CustomTools.SliderDrag
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
namespace LightShaft.Scripts.CustomTools;

[RequireComponent(typeof (Slider))]
public class SliderDrag : MonoBehaviour, IPointerUpHandler, IEventSystemHandler, IPointerDownHandler
{
  public UnityEvent onSliderStartDrag;
  public SliderDragEvent onSliderEndDrag;

  public float SliderValue => this.gameObject.GetComponent<Slider>().value;

  public void OnPointerDown(PointerEventData eventData) => this.onSliderStartDrag.Invoke();

  public void OnPointerUp(PointerEventData data) => this.onSliderEndDrag.Invoke(this.SliderValue);
}
