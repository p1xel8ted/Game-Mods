// Decompiled with JetBrains decompiler
// Type: MMButtonClick
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
