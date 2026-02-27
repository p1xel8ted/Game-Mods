// Decompiled with JetBrains decompiler
// Type: UITwitchHHChoice
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UITwitchHHChoice : MonoBehaviour
{
  [SerializeField]
  private TMP_Text choiceText;
  [SerializeField]
  private TMP_Text votesText;
  [SerializeField]
  private Image bar;

  public string ID { get; private set; }

  public void Configure(string choice, string ID)
  {
    this.ID = ID;
    this.choiceText.text = choice;
  }

  public void UpdateChoice(float votes, int totalVotes)
  {
    this.bar.fillAmount = votes / (float) totalVotes;
    this.votesText.text = votes.ToString();
  }

  public void SetWinner() => this.bar.color = Color.green;
}
