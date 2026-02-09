// Decompiled with JetBrains decompiler
// Type: MMButtonClick
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
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
