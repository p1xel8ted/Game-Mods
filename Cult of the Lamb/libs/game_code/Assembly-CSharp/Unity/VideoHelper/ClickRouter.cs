// Decompiled with JetBrains decompiler
// Type: Unity.VideoHelper.ClickRouter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
namespace Unity.VideoHelper;

public class ClickRouter : Selectable, IPointerClickHandler, IEventSystemHandler
{
  public UnityEvent OnClick = new UnityEvent();
  public UnityEvent OnDoubleClick = new UnityEvent();

  public virtual void OnPointerClick(PointerEventData eventData)
  {
    if (eventData.clickCount == 2)
      this.OnDoubleClick.Invoke();
    else
      this.OnClick.Invoke();
  }
}
