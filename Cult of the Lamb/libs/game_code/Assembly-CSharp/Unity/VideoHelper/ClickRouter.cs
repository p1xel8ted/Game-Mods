// Decompiled with JetBrains decompiler
// Type: Unity.VideoHelper.ClickRouter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
