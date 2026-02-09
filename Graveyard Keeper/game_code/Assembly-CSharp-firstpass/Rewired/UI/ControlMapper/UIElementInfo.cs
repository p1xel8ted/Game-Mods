// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.UIElementInfo
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
namespace Rewired.UI.ControlMapper;

[AddComponentMenu("")]
public abstract class UIElementInfo : MonoBehaviour, ISelectHandler, IEventSystemHandler
{
  public string identifier;
  public int intData;
  public Text text;

  public event Action<GameObject> OnSelectedEvent;

  public void OnSelect(BaseEventData eventData)
  {
    if (this.OnSelectedEvent == null)
      return;
    this.OnSelectedEvent(this.gameObject);
  }
}
