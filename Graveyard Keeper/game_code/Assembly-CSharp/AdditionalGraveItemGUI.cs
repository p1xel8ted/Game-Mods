// Decompiled with JetBrains decompiler
// Type: AdditionalGraveItemGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class AdditionalGraveItemGUI : MonoBehaviour
{
  public UILabel skulls_counter;
  public bool _initialized;

  public void Init()
  {
    if (this._initialized)
      return;
    this._initialized = true;
  }

  public void Clear()
  {
    if (!this._initialized)
      this.Init();
    this.skulls_counter.text = "-";
  }

  public void Draw(Item item)
  {
  }
}
