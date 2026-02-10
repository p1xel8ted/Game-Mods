// Decompiled with JetBrains decompiler
// Type: MMTools.BuyEntry
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Spine.Unity;
using System;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace MMTools;

[Serializable]
public class BuyEntry
{
  [TermsPopup("")]
  public string CharacterName = "-";
  [TermsPopup("")]
  public string TermToSpeak = "-";
  [TextArea(5, 10)]
  public string DisplayText = "";
  public string TermName;
  public string TermCategory = "";
  public string TermDescription;
  [TextArea(5, 10)]
  public string Text;
  public Vector3 Offset = (Vector3) Vector2.zero;
  public Color color;
  public GameObject Speaker;
  public SkeletonAnimation SkeletonData;
  [SpineAnimation("", "SkeletonData", true, false)]
  public string Animation;
  public AudioClip audioClip;
  public UnityEvent Callback;

  public BuyEntry(
    GameObject Speaker,
    string TermToSpeak = "-",
    string Animation = "talk",
    AudioClip audioClip = null,
    UnityEvent Callback = null)
  {
    this.Speaker = Speaker;
    this.TermToSpeak = TermToSpeak;
    this.Animation = Animation;
    this.audioClip = audioClip;
    this.Callback = Callback;
  }

  public string TermString
  {
    get
    {
      this.DisplayText = LocalizationManager.Sources.Count <= 0 || !(this.TermToSpeak != "-") || !(this.TermToSpeak != "") ? "" : LocalizationManager.Sources[0].GetTranslation(this.TermToSpeak);
      return "";
    }
  }

  public void AddTerm()
  {
    LanguageSourceData source = LocalizationManager.Sources[0];
    TermData termData = source.AddTerm($"Conversation_{this.TermCategory}/ {this.TermName}", eTermType.Text);
    termData.SetTranslation(0, this.Text);
    termData.Description = this.TermDescription;
    source.UpdateDictionary();
    this.TermToSpeak = termData.Term;
  }

  public void Shake() => this.Text += "<shake></shake>";

  public void Wiggle() => this.Text += "<wiggle></wiggle>";

  public void Bounce() => this.Text += "<bounce></bounce>";

  public void Rotation() => this.Text += "<rot></rot>";

  public void Swing() => this.Text += "<swing></swing>";

  public void Rainbow() => this.Text += "<rainb></rainb>";

  public void Speed() => this.Text += "<speed=0.2><speed=1>";

  public void Bold() => this.Text += "<b></b>";

  public void Italic() => this.Text += "<i></i>";

  public void Spirits() => this.Text += "<sprite name=\"icon_spirits\">";

  public void Meat() => this.Text += "<sprite name=\"icon_meat\">";

  public void Wood() => this.Text += "<sprite name=\"icon_wood\">";

  public void Stone() => this.Text += "<sprite name=\"icon_stone\">";

  public void Heart() => this.Text += "<sprite name=\"icon_heart\">";

  public void HalfHeart() => this.Text += "<sprite name=\"icon_heart_half\">";

  public void BlueHeart() => this.Text += "<sprite name=\"icon_blueheart\">";

  public void BlueHeartHalf() => this.Text += "<sprite name=\"icon_blueheart_half\">";

  public void TimeToken() => this.Text += "<sprite name=\"icon_timetoken\">";

  public void Flowers() => this.Text += "<sprite name=\"icon_flowers\">";

  public void StainedGlass() => this.Text += "<sprite name=\"icon_stainedglass\">";

  public void Bones() => this.Text += "<sprite name=\"icon_bones\">";

  public void BlackGold() => this.Text += "<sprite name=\"icon_blackgold\">";

  public void Brambles() => this.Text += "<sprite name=\"icon_brambles\">";

  public void SetColor()
  {
    Debug.Log((object) "COLOR!");
    this.Text = $"{this.Text}<#{ColorUtility.ToHtmlStringRGB(this.color)}></color>";
  }
}
