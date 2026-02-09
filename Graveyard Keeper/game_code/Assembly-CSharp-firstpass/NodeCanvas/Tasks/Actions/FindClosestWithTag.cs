// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.FindClosestWithTag
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("GameObject")]
[Description("Find the closest game object of tag to the agent")]
public class FindClosestWithTag : ActionTask<Transform>
{
  [TagField]
  [RequiredField]
  public BBParameter<string> searchTag;
  public BBParameter<bool> ignoreChildren;
  [BlackboardOnly]
  public BBParameter<GameObject> saveObjectAs;
  [BlackboardOnly]
  public BBParameter<float> saveDistanceAs;

  public override void OnExecute()
  {
    List<GameObject> list = ((IEnumerable<GameObject>) GameObject.FindGameObjectsWithTag(this.searchTag.value)).ToList<GameObject>();
    if (list.Count == 0)
    {
      this.saveObjectAs.value = (GameObject) null;
      this.saveDistanceAs.value = 0.0f;
      this.EndAction(false);
    }
    else
    {
      GameObject gameObject1 = (GameObject) null;
      float num1 = float.PositiveInfinity;
      foreach (GameObject gameObject2 in list)
      {
        if (!((Object) gameObject2.transform == (Object) this.agent) && (!this.ignoreChildren.value || !gameObject2.transform.IsChildOf(this.agent)))
        {
          float num2 = Vector3.Distance(gameObject2.transform.position, this.agent.position);
          if ((double) num2 < (double) num1)
          {
            num1 = num2;
            gameObject1 = gameObject2;
          }
        }
      }
      this.saveObjectAs.value = gameObject1;
      this.saveDistanceAs.value = num1;
      this.EndAction();
    }
  }
}
