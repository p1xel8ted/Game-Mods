// Decompiled with JetBrains decompiler
// Type: NGTools.EditorExitBehaviour
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

#nullable disable
namespace NGTools;

[ExecuteInEditMode]
public class EditorExitBehaviour : MonoBehaviour
{
  public Action callback;

  public virtual void OnDestroy()
  {
    if (this.callback == null)
      return;
    this.callback();
  }
}
