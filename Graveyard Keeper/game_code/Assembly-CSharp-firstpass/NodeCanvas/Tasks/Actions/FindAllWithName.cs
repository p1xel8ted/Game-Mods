// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.FindAllWithName
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("GameObject")]
[Description("Note that this is slow.\nAction will end in Failure if no objects are found")]
public class FindAllWithName : ActionTask
{
  [RequiredField]
  public BBParameter<string> searchName = (BBParameter<string>) "GameObject";
  [BlackboardOnly]
  public BBParameter<List<GameObject>> saveAs;

  public override string info
  {
    get => $"GetObjects '{this.searchName?.ToString()}' as {this.saveAs?.ToString()}";
  }

  public override void OnExecute()
  {
    List<GameObject> gameObjectList = new List<GameObject>();
    foreach (GameObject gameObject in Object.FindObjectsOfType<GameObject>())
    {
      if (gameObject.name == this.searchName.value)
        gameObjectList.Add(gameObject);
    }
    this.saveAs.value = gameObjectList;
    this.EndAction(gameObjectList.Count != 0);
  }
}
