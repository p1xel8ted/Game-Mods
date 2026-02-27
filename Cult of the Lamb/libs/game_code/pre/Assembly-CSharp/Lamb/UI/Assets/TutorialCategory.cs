// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Assets.TutorialCategory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Lamb.UI.Assets;

[CreateAssetMenu(fileName = "Tutorial Category", menuName = "Massive Monster/Tutorial Category", order = 1)]
public class TutorialCategory : ScriptableObject
{
  [SerializeField]
  private TutorialTopic _topic;
  [SerializeField]
  private Sprite _topicImage;
  [SerializeField]
  private TutorialCategory.TutorialEntry[] _entries;

  public TutorialTopic Topic => this._topic;

  public TutorialCategory.TutorialEntry[] Entries => this._entries;

  public Sprite TopicImage => this._topicImage;

  public string GetTitle() => LocalizationManager.GetTranslation($"Tutorial UI/{this._topic}");

  public string GetDescription()
  {
    return LocalizationManager.GetTranslation($"Tutorial UI/{this._topic}/Description");
  }

  private void AddEntries()
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

  private void Read()
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
    private Sprite _image;

    public string Description => LocalizationManager.GetTranslation(this._description);

    public Sprite Image => this._image;
  }
}
