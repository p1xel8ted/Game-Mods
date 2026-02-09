// Decompiled with JetBrains decompiler
// Type: UIDragDropRoot
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("NGUI/Interaction/Drag and Drop Root")]
public class UIDragDropRoot : MonoBehaviour
{
  public static Transform root;

  public void OnEnable() => UIDragDropRoot.root = this.transform;

  public void OnDisable()
  {
    if (!((Object) UIDragDropRoot.root == (Object) this.transform))
      return;
    UIDragDropRoot.root = (Transform) null;
  }
}
