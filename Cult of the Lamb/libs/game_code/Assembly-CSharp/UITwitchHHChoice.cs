// Decompiled with JetBrains decompiler
// Type: UITwitchHHChoice
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UITwitchHHChoice : MonoBehaviour
{
  [SerializeField]
  public TMP_Text choiceText;
  [SerializeField]
  public TMP_Text votesText;
  [SerializeField]
  public Image bar;
  [CompilerGenerated]
  public string \u003CID\u003Ek__BackingField;

  public string ID
  {
    get => this.\u003CID\u003Ek__BackingField;
    set => this.\u003CID\u003Ek__BackingField = value;
  }

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
