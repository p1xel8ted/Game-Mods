// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.UIElementInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
namespace Rewired.UI.ControlMapper;

[AddComponentMenu("")]
public abstract class UIElementInfo : MonoBehaviour, ISelectHandler, IEventSystemHandler
{
  public string identifier;
  public int intData;
  public TMP_Text text;

  public event Action<GameObject> OnSelectedEvent;

  public void OnSelect(BaseEventData eventData)
  {
    if (this.OnSelectedEvent == null)
      return;
    this.OnSelectedEvent(this.gameObject);
  }
}
