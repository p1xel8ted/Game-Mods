// Decompiled with JetBrains decompiler
// Type: Rewired.Integration.UnityUI.PlayerPointerEventData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Rewired.UI;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine.EventSystems;

#nullable disable
namespace Rewired.Integration.UnityUI;

public class PlayerPointerEventData : PointerEventData
{
  [CompilerGenerated]
  public int \u003CplayerId\u003Ek__BackingField;
  [CompilerGenerated]
  public int \u003CinputSourceIndex\u003Ek__BackingField;
  [CompilerGenerated]
  public IMouseInputSource \u003CmouseSource\u003Ek__BackingField;
  [CompilerGenerated]
  public ITouchInputSource \u003CtouchSource\u003Ek__BackingField;
  [CompilerGenerated]
  public PointerEventType \u003CsourceType\u003Ek__BackingField;
  [CompilerGenerated]
  public int \u003CbuttonIndex\u003Ek__BackingField;

  public int playerId
  {
    get => this.\u003CplayerId\u003Ek__BackingField;
    set => this.\u003CplayerId\u003Ek__BackingField = value;
  }

  public int inputSourceIndex
  {
    get => this.\u003CinputSourceIndex\u003Ek__BackingField;
    set => this.\u003CinputSourceIndex\u003Ek__BackingField = value;
  }

  public IMouseInputSource mouseSource
  {
    get => this.\u003CmouseSource\u003Ek__BackingField;
    set => this.\u003CmouseSource\u003Ek__BackingField = value;
  }

  public ITouchInputSource touchSource
  {
    get => this.\u003CtouchSource\u003Ek__BackingField;
    set => this.\u003CtouchSource\u003Ek__BackingField = value;
  }

  public PointerEventType sourceType
  {
    get => this.\u003CsourceType\u003Ek__BackingField;
    set => this.\u003CsourceType\u003Ek__BackingField = value;
  }

  public int buttonIndex
  {
    get => this.\u003CbuttonIndex\u003Ek__BackingField;
    set => this.\u003CbuttonIndex\u003Ek__BackingField = value;
  }

  public PlayerPointerEventData(EventSystem eventSystem)
    : base(eventSystem)
  {
    this.playerId = -1;
    this.inputSourceIndex = -1;
    this.buttonIndex = -1;
  }

  public override string ToString()
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.AppendLine("<b>Player Id</b>: " + this.playerId.ToString());
    stringBuilder.AppendLine("<b>Mouse Source</b>: " + this.mouseSource?.ToString());
    stringBuilder.AppendLine("<b>Input Source Index</b>: " + this.inputSourceIndex.ToString());
    stringBuilder.AppendLine("<b>Touch Source/b>: " + this.touchSource?.ToString());
    stringBuilder.AppendLine("<b>Source Type</b>: " + this.sourceType.ToString());
    stringBuilder.AppendLine("<b>Button Index</b>: " + this.buttonIndex.ToString());
    stringBuilder.Append(base.ToString());
    return stringBuilder.ToString();
  }
}
