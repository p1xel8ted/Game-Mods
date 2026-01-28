// Decompiled with JetBrains decompiler
// Type: MMButtonClick
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

#nullable disable
public class MMButtonClick : MMSelectable, IPointerClickHandler, IEventSystemHandler
{
  public UnityEvent OnClick = new UnityEvent();

  public new void Start() => this.Interactable = false;

  public virtual void OnPointerClick(PointerEventData eventData) => this.OnClick.Invoke();
}
