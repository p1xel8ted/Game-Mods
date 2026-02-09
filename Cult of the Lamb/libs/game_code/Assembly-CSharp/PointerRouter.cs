// Decompiled with JetBrains decompiler
// Type: PointerRouter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
public class PointerRouter : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler
{
  public event System.Action OnEnter;

  public void OnPointerEnter(PointerEventData eventData)
  {
    if (this.OnEnter == null)
      return;
    this.OnEnter();
  }
}
