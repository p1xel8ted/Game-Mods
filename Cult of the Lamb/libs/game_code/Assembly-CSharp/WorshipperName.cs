// Decompiled with JetBrains decompiler
// Type: WorshipperName
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class WorshipperName : BaseMonoBehaviour
{
  public Follower Follower;
  public TextMeshProUGUI Text;
  public Gradient ColorGradient;
  public Image DevotionRing;
  public TextMeshProUGUI DevotionText;
  public Color color;
  public float Alpha;
  public float Distance;

  public void Update()
  {
    if (this.Follower.Brain == null)
      return;
    this.Text.text = this.Follower.Brain.Info.Name;
    this.color = this.ColorGradient.Evaluate(this.Follower.Brain.Stats.Happiness / 100f);
    this.Text.color = this.color;
  }
}
