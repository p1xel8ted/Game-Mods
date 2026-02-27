// Decompiled with JetBrains decompiler
// Type: MMTools.ConversationEntry
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using FMODUnity;
using I2.Loc;
using Spine.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace MMTools;

[Serializable]
public class ConversationEntry
{
  [TermsPopup("")]
  public string CharacterName = "-";
  [TermsPopup("")]
  public string TermToSpeak = "-";
  [TextArea(5, 10)]
  public string DisplayText = "";
  [HideInInspector]
  public string TermName;
  [HideInInspector]
  public string TermCategory = "";
  [HideInInspector]
  public string TermDescription;
  [HideInInspector]
  public string Text;
  public GameObject Speaker;
  public bool SpeakerIsPlayer;
  public Vector3 Offset = (Vector3) Vector2.zero;
  public bool SetZoom;
  public float Zoom = 8f;
  public SkeletonAnimation SkeletonData;
  [SpineAnimation("", "SkeletonData", true, false)]
  public string Animation;
  public bool LoopAnimation = true;
  [SpineAnimation("", "SkeletonData", true, false)]
  public string DefaultAnimation = "";
  [EventRef]
  public string soundPath = string.Empty;
  public float pitchValue = -1f;
  public float vibratoValue = -1f;
  public UnityEvent Callback;

  public ConversationEntry(
    GameObject Speaker,
    string TermToSpeak = "-",
    string Animation = "talk",
    string soundPath = "",
    UnityEvent Callback = null)
  {
    this.Speaker = Speaker;
    this.TermToSpeak = TermToSpeak;
    this.Animation = Animation;
    this.soundPath = soundPath;
    this.Callback = Callback;
  }

  private string TermString
  {
    get
    {
      this.DisplayText = LocalizationManager.Sources.Count <= 0 || !(this.TermToSpeak != "-") || !(this.TermToSpeak != "") ? "" : LocalizationManager.Sources[0].GetTranslation(this.TermToSpeak);
      return "";
    }
  }

  private void AddTerm()
  {
    LanguageSourceData source = LocalizationManager.Sources[0];
    TermData termData = source.AddTerm($"Conversation_{this.TermCategory}/ {this.TermName}", eTermType.Text);
    termData.SetTranslation(0, this.Text);
    termData.Description = this.TermDescription;
    source.UpdateDictionary();
    this.TermToSpeak = termData.Term;
  }

  internal static ConversationEntry Clone(ConversationEntry conversationEntry)
  {
    return new ConversationEntry(conversationEntry.Speaker, conversationEntry.TermToSpeak, conversationEntry.Animation, conversationEntry.soundPath)
    {
      CharacterName = conversationEntry.CharacterName,
      SkeletonData = conversationEntry.SkeletonData,
      Offset = conversationEntry.Offset,
      SpeakerIsPlayer = conversationEntry.SpeakerIsPlayer,
      LoopAnimation = conversationEntry.LoopAnimation
    };
  }

  internal static List<ConversationEntry> CloneList(List<ConversationEntry> List)
  {
    List<ConversationEntry> conversationEntryList = new List<ConversationEntry>();
    foreach (ConversationEntry conversationEntry in List)
      conversationEntryList.Add(ConversationEntry.Clone(conversationEntry));
    return conversationEntryList;
  }
}
