// Decompiled with JetBrains decompiler
// Type: CustomScript
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public abstract class CustomScript : MonoBehaviour
{
  public bool started;
  public string script_name;
  public bool is_global;
  public WorldGameObject current_interractor;

  public virtual void TerminateMe()
  {
    this.enabled = false;
    NGUITools.Destroy((Object) this.gameObject);
    Debug.Log((object) $"<color=orange>Terminating Script:</color> {this.script_name}, is_global = {this.is_global.ToString()}", (Object) this);
  }

  public void ForceStart() => this.started = true;
}
