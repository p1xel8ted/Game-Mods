// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.SendEventToObjects
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("✫ Utility")]
[Description("Send a Graph Event to multiple gameobjects which should have a GraphOwner component attached.")]
public class SendEventToObjects : ActionTask
{
  [RequiredField]
  public BBParameter<List<GameObject>> targetObjects;
  [RequiredField]
  public BBParameter<string> eventName;

  public override string info => $"Send Event [{this.eventName}] to {this.targetObjects}";

  public override void OnExecute()
  {
    foreach (GameObject gameObject in this.targetObjects.value)
    {
      if ((Object) gameObject != (Object) null)
      {
        GraphOwner component = gameObject.GetComponent<GraphOwner>();
        if ((Object) component != (Object) null)
          component.SendEvent(this.eventName.value);
      }
    }
    this.EndAction();
  }
}
