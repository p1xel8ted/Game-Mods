// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.NewGameObject
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using System;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Obsolete]
[Category("Utilities/Constructors")]
public class NewGameObject : CallableFunctionNode<GameObject, string, Vector3, Quaternion>
{
  public override GameObject Invoke(string name, Vector3 position, Quaternion rotation)
  {
    return new GameObject(name)
    {
      transform = {
        position = position,
        rotation = rotation
      }
    };
  }
}
