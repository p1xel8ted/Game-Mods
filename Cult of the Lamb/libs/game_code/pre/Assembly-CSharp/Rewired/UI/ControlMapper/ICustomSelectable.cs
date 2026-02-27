// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.ICustomSelectable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
