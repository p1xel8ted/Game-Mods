// Decompiled with JetBrains decompiler
// Type: HUDTechTrashCan
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class HUDTechTrashCan : AnimatedGUIPanel
{
  public int hidden_y = 30;
  public int shown_y = -1;
  public GameObject pos;

  public void Init() => this.Init(this.hidden_y, this.shown_y);
}
