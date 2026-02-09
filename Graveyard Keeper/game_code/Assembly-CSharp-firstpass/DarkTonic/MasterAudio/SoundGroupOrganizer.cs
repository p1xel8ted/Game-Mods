// Decompiled with JetBrains decompiler
// Type: DarkTonic.MasterAudio.SoundGroupOrganizer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace DarkTonic.MasterAudio;

public class SoundGroupOrganizer : MonoBehaviour
{
  public GameObject dynGroupTemplate;
  public GameObject dynVariationTemplate;
  public GameObject maGroupTemplate;
  public GameObject maVariationTemplate;
  public DarkTonic.MasterAudio.MasterAudio.DragGroupMode curDragGroupMode;
  public DarkTonic.MasterAudio.MasterAudio.AudioLocation bulkVariationMode;
  public SystemLanguage previewLanguage = SystemLanguage.English;
  public bool useTextGroupFilter;
  public string textGroupFilter = string.Empty;
  public SoundGroupOrganizer.TransferMode transMode;
  public GameObject sourceObject;
  public List<SoundGroupOrganizer.SoundGroupSelection> selectedSourceSoundGroups = new List<SoundGroupOrganizer.SoundGroupSelection>();
  public GameObject destObject;
  public List<SoundGroupOrganizer.SoundGroupSelection> selectedDestSoundGroups = new List<SoundGroupOrganizer.SoundGroupSelection>();
  public SoundGroupOrganizer.MAItemType itemType;
  public List<SoundGroupOrganizer.CustomEventSelection> selectedSourceCustomEvents = new List<SoundGroupOrganizer.CustomEventSelection>();
  public List<SoundGroupOrganizer.CustomEventSelection> selectedDestCustomEvents = new List<SoundGroupOrganizer.CustomEventSelection>();
  public List<CustomEvent> customEvents = new List<CustomEvent>();
  public List<CustomEventCategory> customEventCategories = new List<CustomEventCategory>()
  {
    new CustomEventCategory()
  };
  public string newEventName = "my event";
  public string newCustomEventCategoryName = "New Category";
  public string addToCustomEventCategoryName = "New Category";

  public void Awake()
  {
    Debug.LogError((object) "You have a Sound Group Organizer prefab in this Scene. You should never play a Scene with that type of prefab as it could take up tremendous amounts of audio memory. Please use a Sandbox Scene for that, which is only used to make changes to that prefab and apply them. This Sandbox Scene should never be a Scene that is played in the game.");
  }

  public class CustomEventSelection
  {
    public CustomEvent Event;
    public bool IsSelected;

    public CustomEventSelection(CustomEvent cEvent, bool isSelected)
    {
      this.Event = cEvent;
      this.IsSelected = isSelected;
    }
  }

  public class SoundGroupSelection
  {
    public GameObject Go;
    public bool IsSelected;

    public SoundGroupSelection(GameObject go, bool isSelected)
    {
      this.Go = go;
      this.IsSelected = isSelected;
    }
  }

  public enum MAItemType
  {
    SoundGroups,
    CustomEvents,
  }

  public enum TransferMode
  {
    None,
    Import,
    Export,
  }
}
