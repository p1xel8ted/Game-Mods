// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.CreateGameObject
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("GameObject")]
public class CreateGameObject : ActionTask
{
  public BBParameter<string> objectName;
  public BBParameter<Vector3> position;
  public BBParameter<Vector3> rotation;
  [BlackboardOnly]
  public BBParameter<GameObject> saveAs;

  public override void OnExecute()
  {
    this.saveAs.value = new GameObject(this.objectName.value)
    {
      transform = {
        position = this.position.value,
        eulerAngles = this.rotation.value
      }
    };
    this.EndAction();
  }
}
