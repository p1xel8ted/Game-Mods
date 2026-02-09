// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.ICustomSelectable
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

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
