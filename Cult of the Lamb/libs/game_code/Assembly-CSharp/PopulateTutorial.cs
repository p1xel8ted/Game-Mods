// Decompiled with JetBrains decompiler
// Type: PopulateTutorial
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using TMPro;
using UnityEngine;

#nullable disable
public class PopulateTutorial : MonoBehaviour
{
  public TutorialTopic Topic;
  public TextMeshProUGUI Title;
  public TextMeshProUGUI Description;
  public TextMeshProUGUI Info1;
  public TextMeshProUGUI Info2;
  public TextMeshProUGUI Info3;

  public void OnEnable() => this.Populate();

  public void Populate()
  {
    string Term = "Tutorial UI/" + this.Topic.ToString();
    this.Title.text = LocalizationManager.GetTranslation(Term);
    this.Description.text = LocalizationManager.GetTranslation(Term + "/Description");
    this.Info1.text = LocalizationManager.GetTranslation(Term + "/Info1");
    this.Info2.text = LocalizationManager.GetTranslation(Term + "/Info2");
    this.Info3.text = LocalizationManager.GetTranslation(Term + "/Info3");
  }
}
