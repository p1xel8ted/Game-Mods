// Decompiled with JetBrains decompiler
// Type: HUDTechPointsBar
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class HUDTechPointsBar : AnimatedGUIPanel
{
  public int hidden_coord = 30;
  public int shown_coord = -1;
  public UILabel label;
  public GameObject pos_r;
  public GameObject pos_g;
  public GameObject pos_b;

  public void Init()
  {
    this.Init(this.hidden_coord, this.shown_coord, AnimatedGUIPanel.AnimationType.AnimateX);
  }

  public override void Redraw() => this.label.text = PlayerComponent.GetTechPointsString();

  public Vector2 GetTechPointsCounterPosition(string tech_type)
  {
    switch (tech_type)
    {
      case "r":
        return (Vector2) this.pos_r.transform.position;
      case "g":
        return (Vector2) this.pos_g.transform.position;
      case "b":
        return (Vector2) this.pos_b.transform.position;
      default:
        return Vector2.zero;
    }
  }
}
