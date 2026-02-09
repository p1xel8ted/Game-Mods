// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.FindObjectsOfType`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("GameObject")]
[Description("Note that this is very slow")]
public class FindObjectsOfType<T> : ActionTask where T : Component
{
  [BlackboardOnly]
  public BBParameter<List<GameObject>> saveGameObjects;
  [BlackboardOnly]
  public BBParameter<List<T>> saveComponents;

  public override void OnExecute()
  {
    T[] objectsOfType = UnityEngine.Object.FindObjectsOfType<T>();
    if (objectsOfType != null && objectsOfType.Length != 0)
    {
      this.saveGameObjects.value = ((IEnumerable<T>) objectsOfType).Select<T, GameObject>((Func<T, GameObject>) (o => o.gameObject)).ToList<GameObject>();
      this.saveComponents.value = ((IEnumerable<T>) objectsOfType).ToList<T>();
      this.EndAction(true);
    }
    else
      this.EndAction(false);
  }
}
