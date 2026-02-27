// Decompiled with JetBrains decompiler
// Type: WorshipperName
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private Color color;
  private float Alpha;
  private float Distance;

  private void Update()
  {
    if (this.Follower.Brain == null)
      return;
    this.Text.text = this.Follower.Brain.Info.Name;
    this.color = this.ColorGradient.Evaluate(this.Follower.Brain.Stats.Happiness / 100f);
    this.Text.color = this.color;
  }
}
