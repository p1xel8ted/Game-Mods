// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.SendMessageToType`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("✫ Script Control/Common")]
[Description("Send a Unity message to all game objects with a component of the specified type.\nNotice: This is slow and should not be called per-fame.")]
public class SendMessageToType<T> : ActionTask where T : Component
{
  [RequiredField]
  public BBParameter<string> message;
  [BlackboardOnly]
  public BBParameter<object> argument;

  public override string info
  {
    get => $"Message {this.message}({this.argument}) to all {typeof (T).Name}s";
  }

  public override void OnExecute()
  {
    T[] objectsOfType = Object.FindObjectsOfType<T>();
    if (objectsOfType.Length == 0)
    {
      this.EndAction(false);
    }
    else
    {
      foreach (T obj in objectsOfType)
        obj.gameObject.SendMessage(this.message.value, this.argument.value);
      this.EndAction(true);
    }
  }
}
