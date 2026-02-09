// Decompiled with JetBrains decompiler
// Type: TriggerCallback
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class TriggerCallback : BaseMonoBehaviour
{
  public UnityEvent Callback;
  public bool Activated;

  public void OnTriggerEnter2D(Collider2D collision)
  {
    PlayerFarming component = collision.GetComponent<PlayerFarming>();
    if (this.Activated || !((Object) component != (Object) null))
      return;
    this.Activated = true;
    this.Callback?.Invoke();
  }

  public void DisableTrigger() => this.gameObject.GetComponent<Collider2D>().enabled = false;
}
