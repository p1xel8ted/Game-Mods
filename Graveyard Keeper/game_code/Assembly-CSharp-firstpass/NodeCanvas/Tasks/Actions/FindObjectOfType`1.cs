// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.FindObjectOfType`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("GameObject")]
[Description("Note that this is very slow")]
public class FindObjectOfType<T> : ActionTask where T : Component
{
  [BlackboardOnly]
  public BBParameter<T> saveComponentAs;
  [BlackboardOnly]
  public BBParameter<GameObject> saveGameObjectAs;

  public override void OnExecute()
  {
    T objectOfType = Object.FindObjectOfType<T>();
    if ((Object) objectOfType != (Object) null)
    {
      this.saveComponentAs.value = objectOfType;
      this.saveGameObjectAs.value = objectOfType.gameObject;
      this.EndAction(true);
    }
    else
      this.EndAction(false);
  }
}
