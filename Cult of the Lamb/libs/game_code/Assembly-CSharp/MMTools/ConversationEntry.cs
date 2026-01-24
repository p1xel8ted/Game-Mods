// Decompiled with JetBrains decompiler
// Type: MMTools.ConversationEntry
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public int followerID = -1;
  public UnityEvent Callback;

  public ConversationEntry(
    GameObject Speaker,
    string TermToSpeak = "-",
    string Animation = "talk",
    string soundPath = "",
    UnityEvent Callback = null,
    string CharacterName = null)
  {
    this.Speaker = Speaker;
    this.TermToSpeak = TermToSpeak;
    this.Animation = Animation;
    this.soundPath = soundPath;
    this.Callback = Callback;
    if (CharacterName == null)
      return;
    this.CharacterName = CharacterName;
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

  public static ConversationEntry Clone(ConversationEntry conversationEntry)
  {
    return new ConversationEntry(conversationEntry.Speaker, conversationEntry.TermToSpeak, conversationEntry.Animation, conversationEntry.soundPath)
    {
      CharacterName = conversationEntry.CharacterName,
      SkeletonData = conversationEntry.SkeletonData,
      Offset = conversationEntry.Offset,
      SpeakerIsPlayer = conversationEntry.SpeakerIsPlayer,
      LoopAnimation = conversationEntry.LoopAnimation,
      SetZoom = conversationEntry.SetZoom,
      Zoom = conversationEntry.Zoom,
      DefaultAnimation = conversationEntry.DefaultAnimation
    };
  }

  public static List<ConversationEntry> CloneList(List<ConversationEntry> List)
  {
    List<ConversationEntry> conversationEntryList = new List<ConversationEntry>();
    foreach (ConversationEntry conversationEntry in List)
      conversationEntryList.Add(ConversationEntry.Clone(conversationEntry));
    return conversationEntryList;
  }
}
