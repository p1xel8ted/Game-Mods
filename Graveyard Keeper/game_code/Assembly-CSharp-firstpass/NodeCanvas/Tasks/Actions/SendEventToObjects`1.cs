// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.SendEventToObjects`1
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
public class SendEventToObjects<T> : ActionTask
{
  [RequiredField]
  public BBParameter<List<GameObject>> targetObjects;
  [RequiredField]
  public BBParameter<string> eventName;
  public BBParameter<T> eventValue;

  public override string info
  {
    get => $"Send Event [{this.eventName}]({this.eventValue}) to {this.targetObjects}";
  }

  public override void OnExecute()
  {
    foreach (GameObject gameObject in this.targetObjects.value)
    {
      GraphOwner component = gameObject.GetComponent<GraphOwner>();
      if ((Object) component != (Object) null)
        component.SendEvent<T>(this.eventName.value, this.eventValue.value);
    }
    this.EndAction();
  }
}
