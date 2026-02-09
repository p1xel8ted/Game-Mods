// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.InstantiateGameObject
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("GameObject")]
public class InstantiateGameObject : ActionTask<Transform>
{
  public BBParameter<Transform> parent;
  public BBParameter<Vector3> clonePosition;
  public BBParameter<Vector3> cloneRotation;
  [BlackboardOnly]
  public BBParameter<GameObject> saveCloneAs;

  public override string info
  {
    get
    {
      return $"Instantiate {this.agentInfo} under {((bool) (Object) this.parent.value ? this.parent.ToString() : "World")} at {this.clonePosition?.ToString()} as {this.saveCloneAs?.ToString()}";
    }
  }

  public override void OnExecute()
  {
    GameObject gameObject = Object.Instantiate<GameObject>(this.agent.gameObject, this.parent.value, false);
    gameObject.transform.position = this.clonePosition.value;
    gameObject.transform.eulerAngles = this.cloneRotation.value;
    this.saveCloneAs.value = gameObject;
    this.EndAction();
  }
}
