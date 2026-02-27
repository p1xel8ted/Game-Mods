// Decompiled with JetBrains decompiler
// Type: TriggerCallback
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class TriggerCallback : BaseMonoBehaviour
{
  public UnityEvent Callback;
  private bool Activated;

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (this.Activated || !collision.gameObject.CompareTag("Player"))
      return;
    this.Activated = true;
    this.Callback?.Invoke();
  }
}
