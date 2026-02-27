// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.UIElementInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
