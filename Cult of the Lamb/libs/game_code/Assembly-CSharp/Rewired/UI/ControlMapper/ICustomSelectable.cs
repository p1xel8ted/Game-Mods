// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.ICustomSelectable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

#nullable disable
namespace Rewired.UI.ControlMapper;

public interface ICustomSelectable : ICancelHandler, IEventSystemHandler
{
  Sprite disabledHighlightedSprite { get; set; }

  Color disabledHighlightedColor { get; set; }

  string disabledHighlightedTrigger { get; set; }

  bool autoNavUp { get; set; }

  bool autoNavDown { get; set; }

  bool autoNavLeft { get; set; }

  bool autoNavRight { get; set; }

  event UnityAction CancelEvent;
}
