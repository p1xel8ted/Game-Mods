// Decompiled with JetBrains decompiler
// Type: SimpleTriggerEvents
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class SimpleTriggerEvents : MonoBehaviour
{
  public SimpleTriggerEvents.Collider2DEvent onEnter;
  public SimpleTriggerEvents.Collider2DEvent onStay;
  public SimpleTriggerEvents.Collider2DEvent onExit;

  public void OnTriggerEnter2D(Collider2D collider) => this.onEnter.Invoke(collider);

  public void OnTriggerExit2D(Collider2D collider) => this.onExit.Invoke(collider);

  public void OnTriggerStay2D(Collider2D collider) => this.onStay.Invoke(collider);

  [Serializable]
  public class Collider2DEvent : UnityEvent<Collider2D>
  {
  }
}
