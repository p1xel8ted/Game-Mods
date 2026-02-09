// Decompiled with JetBrains decompiler
// Type: BaseInputController
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BaseInputController
{
  public List<GameKey> holded_keys = new List<GameKey>();
  public List<GameKey> pressed_keys = new List<GameKey>();
  public List<GameKey> released_keys = new List<GameKey>();
  public Vector2 dir = Vector2.zero;
  public Vector2 dir2 = Vector2.zero;

  public Vector2 direction => this.dir;

  public Vector2 direction2 => this.dir2;

  public virtual void Update()
  {
    this.holded_keys.Clear();
    this.pressed_keys.Clear();
  }

  public virtual bool IsActive()
  {
    return this.holded_keys.Count > 0 || (double) this.dir.magnitude > 0.0 || (double) this.dir2.magnitude > 0.0;
  }
}
