// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Assets.TutorialCategory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

#nullable disable
namespace Lamb.UI.Assets;

[CreateAssetMenu(fileName = "Tutorial Category", menuName = "Massive Monster/Tutorial Category", order = 1)]
public class TutorialCategory : ScriptableObject
{
  [SerializeField]
  public TutorialTopic _topic;
  [SerializeField]
  public AssetReferenceSprite _topicImageRef;
  [SerializeField]
  public TutorialCategory.TutorialEntry[] _entries;

  public TutorialTopic Topic => this._topic;

  public TutorialCategory.TutorialEntry[] Entries => this._entries;

  public AssetReferenceSprite TopicImageRef => this._topicImageRef;

  public string GetTitle() => LocalizationManager.GetTranslation($"Tutorial UI/{this._topic}");

  public string GetDescription()
  {
    return LocalizationManager.GetTranslation($"Tutorial UI/{this._topic}/Description");
  }

  public void AddEntries()
  {
    int num = 1;
    List<TutorialCategory.TutorialEntry> tutorialEntryList = new List<TutorialCategory.TutorialEntry>();
    for (; LocalizationManager.GetTermData($"Tutorial UI/{this._topic}/Info{num}") != null; ++num)
    {
      TutorialCategory.TutorialEntry tutorialEntry = new TutorialCategory.TutorialEntry()
      {
        _description = $"Tutorial UI/{this._topic}/Info{num}"
      };
      tutorialEntryList.Add(tutorialEntry);
    }
    this._entries = new TutorialCategory.TutorialEntry[tutorialEntryList.Count];
    int index = -1;
    while (++index < tutorialEntryList.Count)
      this._entries[index] = tutorialEntryList[index];
  }

  public void Read()
  {
    Debug.Log((object) this.GetTitle());
    Debug.Log((object) this.GetDescription());
    foreach (TutorialCategory.TutorialEntry entry in this._entries)
      Debug.Log((object) LocalizationManager.GetTranslation(entry._description));
  }

  [Serializable]
  public class TutorialEntry
  {
    [SerializeField]
    [TermsPopup("")]
    public string _description;
    [SerializeField]
    public AssetReferenceSprite _imageRef;

    public string Description => LocalizationManager.GetTranslation(this._description);

    public AssetReferenceSprite ImageRef => this._imageRef;
  }
}
