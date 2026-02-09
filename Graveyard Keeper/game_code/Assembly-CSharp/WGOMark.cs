// Decompiled with JetBrains decompiler
// Type: WGOMark
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class WGOMark : MonoBehaviour
{
  public SpriteRenderer spr;
  public UnityEngine.Sprite s_can_be_removed;
  public UnityEngine.Sprite s_is_removing;

  public void Draw(WGOMark.MarkType type)
  {
    switch (type)
    {
      case WGOMark.MarkType.None:
      case WGOMark.MarkType.CanBeRemoved:
        this.spr.sprite = (UnityEngine.Sprite) null;
        break;
      case WGOMark.MarkType.IsRemoving:
        this.spr.sprite = this.s_is_removing;
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof (type), (object) type, (string) null);
    }
  }

  public enum MarkType
  {
    None,
    CanBeRemoved,
    IsRemoving,
  }
}
