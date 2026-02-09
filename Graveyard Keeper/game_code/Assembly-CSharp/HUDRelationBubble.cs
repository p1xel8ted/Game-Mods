// Decompiled with JetBrains decompiler
// Type: HUDRelationBubble
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class HUDRelationBubble : AnimatedGUIPanel
{
  public int hidden_x = 30;
  public int shown_x = -1;
  public UILabel label;

  public void Init()
  {
    this.Init(this.hidden_x, this.shown_x, AnimatedGUIPanel.AnimationType.AnimateX);
  }

  public static string GetRelationChangeString(int delta)
  {
    return delta > 0 ? "+(positive)" + Mathf.Abs(delta).ToString() : "-(negative)" + Mathf.Abs(delta).ToString();
  }

  public void OnChangedRelation(int delta)
  {
    this.label.text = $"{GJL.L("relation")}{GJL.L(":")} {HUDRelationBubble.GetRelationChangeString(delta)}";
    this.Show();
  }
}
