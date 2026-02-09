// Decompiled with JetBrains decompiler
// Type: UIDragDropContainer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("NGUI/Interaction/Drag and Drop Container")]
public class UIDragDropContainer : MonoBehaviour
{
  public Transform reparentTarget;

  public virtual void Start()
  {
    if (!((Object) this.reparentTarget == (Object) null))
      return;
    this.reparentTarget = this.transform;
  }
}
