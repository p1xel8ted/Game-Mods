// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.FindWithName
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("GameObject")]
public class FindWithName : ActionTask
{
  [RequiredField]
  public BBParameter<string> gameObjectName;
  [BlackboardOnly]
  public BBParameter<GameObject> saveAs;

  public override string info
  {
    get => $"Find Object {this.gameObjectName?.ToString()} as {this.saveAs?.ToString()}";
  }

  public override void OnExecute()
  {
    this.saveAs.value = GameObject.Find(this.gameObjectName.value);
    this.EndAction();
  }
}
