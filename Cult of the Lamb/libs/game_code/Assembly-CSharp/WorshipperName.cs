// Decompiled with JetBrains decompiler
// Type: WorshipperName
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
