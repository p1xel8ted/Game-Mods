// Decompiled with JetBrains decompiler
// Type: PointerRouter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
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
