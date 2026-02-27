// Decompiled with JetBrains decompiler
// Type: WorshipperData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Spine;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#nullable disable
public class WorshipperData : BaseMonoBehaviour
{
  public SkeletonAnimation SkeletonData;
  public List<WorshipperData.SlotsAndColours> GlobalColourList = new List<WorshipperData.SlotsAndColours>();
  public List<WorshipperData.SkinAndData> Characters = new List<WorshipperData.SkinAndData>();
  public static WorshipperData _Instance;

  public static WorshipperData Instance
  {
    get
    {
      if (!Application.isPlaying)
        return WorshipperData._Instance = UnityEngine.Object.FindObjectOfType<WorshipperData>();
      if ((UnityEngine.Object) WorshipperData._Instance == (UnityEngine.Object) null)
        WorshipperData._Instance = (UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Worshipper Data")) as GameObject).GetComponent<WorshipperData>();
      return WorshipperData._Instance;
    }
    set => WorshipperData._Instance = value;
  }

  public void Awake()
  {
    foreach (WorshipperData.SkinAndData character in this.Characters)
      character.SlotAndColours.AddRange((IEnumerable<WorshipperData.SlotsAndColours>) this.GlobalColourList);
  }

  public int GetSkinIndexFromName(string skinName)
  {
    foreach (WorshipperData.SkinAndData character in this.Characters)
    {
      if (character.Skin[0].Skin == skinName)
        return this.Characters.IndexOf(character);
    }
    Debug.Log((object) ("Failed to find skin index for " + skinName));
    return 0;
  }

  public string GetSkinNameFromIndex(int index) => this.Characters[index].Skin[0].Skin;

  public int GetRandomAvailableSkin(
    bool includeBlacklist = false,
    bool includeBosses = false,
    bool includeInvariant = false)
  {
    List<string> stringList = new List<string>();
    foreach (string skin in DataManager.Instance.FollowerSkinsUnlocked)
    {
      if ((includeBosses || !skin.Contains("Boss")) && (includeBlacklist || !DataManager.OnBlackList(skin)))
        stringList.Add(skin);
    }
    string str = stringList[UnityEngine.Random.Range(0, stringList.Count)];
    int index = -1;
    while (++index < this.Characters.Count)
    {
      if (this.Characters[index].Title == str && (includeInvariant || !this.Characters[index].Invariant))
        return index;
    }
    return 0;
  }

  public string GetRandomAvailableSkinName()
  {
    List<string> stringList = new List<string>();
    foreach (string str in DataManager.Instance.FollowerSkinsUnlocked)
    {
      if (!str.Contains("Boss"))
        stringList.Add(str);
    }
    return stringList[UnityEngine.Random.Range(0, stringList.Count)];
  }

  public int GetRandomSkinAny()
  {
    List<string> stringList = DataManager.AvailableSkins();
    string str = stringList[UnityEngine.Random.Range(0, stringList.Count)];
    int index = -1;
    while (++index < this.Characters.Count)
    {
      if (this.Characters[index].Title == str)
        return index;
    }
    return 0;
  }

  public void OnEnable()
  {
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) this.gameObject);
    WorshipperData.Instance = this;
  }

  public void Start()
  {
    int index = -1;
    while (++index < this.transform.childCount)
      this.transform.GetChild(index).gameObject.SetActive(false);
  }

  public void OnDisable()
  {
    if (!((UnityEngine.Object) WorshipperData.Instance == (UnityEngine.Object) this))
      return;
    WorshipperData.Instance = (WorshipperData) null;
  }

  public List<WorshipperData.SkinAndData> GetCharacters() => this.Characters;

  public WorshipperData.SkinAndData GetCharacters(string SkinName)
  {
    foreach (WorshipperData.SkinAndData character in this.Characters)
    {
      if (character.Contains(SkinName))
        return character;
    }
    return (WorshipperData.SkinAndData) null;
  }

  public WorshipperData.SkinAndData GetColourData(string Skin)
  {
    foreach (WorshipperData.SkinAndData character in this.Characters)
    {
      if (character.Contains(Skin))
        return character;
    }
    return (WorshipperData.SkinAndData) null;
  }

  public List<WorshipperData.SkinAndData> GetSkinsFromFollowerLocation(FollowerLocation location)
  {
    switch (location)
    {
      case FollowerLocation.Base:
      case FollowerLocation.HubShore:
      case FollowerLocation.Dungeon1_1:
      case FollowerLocation.IntroDungeon:
        return this.GetSkinsFromLocation(WorshipperData.DropLocation.Dungeon1);
      case FollowerLocation.Dungeon1_2:
      case FollowerLocation.Sozo_Cave:
        return this.GetSkinsFromLocation(WorshipperData.DropLocation.Dungeon2);
      case FollowerLocation.Dungeon1_3:
      case FollowerLocation.Dungeon_Decoration_Shop1:
        return this.GetSkinsFromLocation(WorshipperData.DropLocation.Dungeon3);
      case FollowerLocation.Dungeon1_4:
      case FollowerLocation.Dungeon_Location_3:
        return this.GetSkinsFromLocation(WorshipperData.DropLocation.Dungeon4);
      case FollowerLocation.Dungeon1_5:
      case FollowerLocation.Boss_Wolf:
        return this.GetSkinsFromLocation(WorshipperData.DropLocation.Dungeon5);
      case FollowerLocation.Dungeon1_6:
      case FollowerLocation.Boss_Yngya:
        return this.GetSkinsFromLocation(WorshipperData.DropLocation.Dungeon6);
      case FollowerLocation.DLC_ShrineRoom:
        List<WorshipperData.SkinAndData> skinsFromLocation = this.GetSkinsFromLocation(WorshipperData.DropLocation.Dungeon5);
        skinsFromLocation.AddRange((IEnumerable<WorshipperData.SkinAndData>) this.GetSkinsFromLocation(WorshipperData.DropLocation.Dungeon6));
        return skinsFromLocation;
      default:
        return this.Characters;
    }
  }

  public List<WorshipperData.SkinAndData> GetSkinsFromLocation(
    WorshipperData.DropLocation dropLocation,
    bool ignoreHidden = false)
  {
    List<WorshipperData.SkinAndData> skinsFromLocation = new List<WorshipperData.SkinAndData>();
    foreach (WorshipperData.SkinAndData character in this.Characters)
    {
      WorshipperData.SkinAndData data = character;
      if (!data.Invariant && (ignoreHidden || DataManager.GetFollowerSkinUnlocked(data.Skin[0].Skin) || !data.Hidden) && data.DropLocation == dropLocation)
      {
        if (skinsFromLocation.Find((Predicate<WorshipperData.SkinAndData>) (skinData => skinData.Title.Equals(data.Title))) == null)
          skinsFromLocation.Add(data);
        else
          Debug.LogWarning((object) ("Found duplicate of skin " + data.Title));
      }
    }
    return skinsFromLocation;
  }

  public List<WorshipperData.SkinAndData> GetSkinsAll(bool ignoreHidden = false)
  {
    List<WorshipperData.SkinAndData> skinsAll = new List<WorshipperData.SkinAndData>();
    foreach (WorshipperData.SkinAndData character in this.Characters)
    {
      if (!character.Invariant && (ignoreHidden || DataManager.GetFollowerSkinUnlocked(character.Skin[0].Skin) || !character.Hidden))
        skinsAll.Add(character);
    }
    return skinsAll;
  }

  public string GetSkinsLocationString(WorshipperData.SkinAndData skin)
  {
    switch (skin.DropLocation)
    {
      case WorshipperData.DropLocation.Dungeon1:
        return ScriptLocalization.NAMES_Places.Dungeon1_1;
      case WorshipperData.DropLocation.Dungeon2:
        return ScriptLocalization.NAMES_Places.Dungeon1_2;
      case WorshipperData.DropLocation.Dungeon3:
        return ScriptLocalization.NAMES_Places.Dungeon1_3;
      case WorshipperData.DropLocation.Dungeon4:
        return ScriptLocalization.NAMES_Places.Dungeon1_4;
      case WorshipperData.DropLocation.Other:
        return ScriptLocalization.UI_Generic.General;
      case WorshipperData.DropLocation.Dungeon5:
        return ScriptLocalization.NAMES_Places.DLC_Dungeon_1;
      case WorshipperData.DropLocation.Dungeon6:
        return ScriptLocalization.NAMES_Places.DLC_Dungeon_2;
      default:
        Debug.Log((object) ("Couldn't find drop location: " + skin.DropLocation.ToString()));
        return "";
    }
  }

  public void AddMissingSlots()
  {
    for (int index1 = 0; index1 < this.Characters.Count; ++index1)
    {
      for (int index2 = 0; index2 < this.Characters[index1].SlotAndColours.Count; ++index2)
        this.Characters[index1].SlotAndColours[index2].AddAllSlots();
    }
  }

  public void ExportCSV()
  {
    string path = Application.persistentDataPath + "/CotL Skin Variations.csv";
    TextWriter textWriter1 = (TextWriter) new StreamWriter(path, false);
    textWriter1.WriteLine("skinName, premium, colorOptionIndex, HEAD_SKIN_TOP, HEAD_SKIN_BTM, ARM_LEFT_SKIN, ARM_RIGHT_SKIN, LEG_LEFT_SKIN, LEG_RIGHT_SKIN, MARKINGS, BODY_NAKED");
    textWriter1.Close();
    TextWriter textWriter2 = (TextWriter) new StreamWriter(path, true);
    foreach (WorshipperData.SkinAndData character in this.Characters)
    {
      List<WorshipperData.SlotsAndColours> slotsAndColoursList = new List<WorshipperData.SlotsAndColours>((IEnumerable<WorshipperData.SlotsAndColours>) character.SlotAndColours);
      slotsAndColoursList.AddRange((IEnumerable<WorshipperData.SlotsAndColours>) this.GlobalColourList);
      for (int index = 0; index < slotsAndColoursList.Count; ++index)
      {
        string str1 = $"{$"{$"{character.Skin[0].Skin},"}{(character.TwitchPremium ? "TRUE" : "FALSE")},"}{index.ToString()},";
        Color colorFromSlot1 = WorshipperData.GetColorFromSlot(slotsAndColoursList[index].SlotAndColours, "HEAD_SKIN_TOP");
        Color colorFromSlot2 = WorshipperData.GetColorFromSlot(slotsAndColoursList[index].SlotAndColours, "HEAD_SKIN_BTM");
        Color colorFromSlot3 = WorshipperData.GetColorFromSlot(slotsAndColoursList[index].SlotAndColours, "ARM_LEFT_SKIN");
        Color colorFromSlot4 = WorshipperData.GetColorFromSlot(slotsAndColoursList[index].SlotAndColours, "ARM_RIGHT_SKIN");
        Color colorFromSlot5 = WorshipperData.GetColorFromSlot(slotsAndColoursList[index].SlotAndColours, "LEG_LEFT_SKIN");
        Color colorFromSlot6 = WorshipperData.GetColorFromSlot(slotsAndColoursList[index].SlotAndColours, "LEG_RIGHT_SKIN");
        Color colorFromSlot7 = WorshipperData.GetColorFromSlot(slotsAndColoursList[index].SlotAndColours, "MARKINGS");
        Color colorFromSlot8 = WorshipperData.GetColorFromSlot(slotsAndColoursList[index].SlotAndColours, "BODY_NAKED");
        string str2 = str1;
        int num = Mathf.RoundToInt(colorFromSlot1.r * (float) byte.MaxValue);
        string str3 = num.ToString();
        num = Mathf.RoundToInt(colorFromSlot1.g * (float) byte.MaxValue);
        string str4 = num.ToString();
        num = Mathf.RoundToInt(colorFromSlot1.b * (float) byte.MaxValue);
        string str5 = num.ToString();
        string str6 = $"\"{str3}, {str4}, {str5}\"";
        string str7 = $"{str2}{str6},";
        num = Mathf.RoundToInt(colorFromSlot2.r * (float) byte.MaxValue);
        string str8 = num.ToString();
        num = Mathf.RoundToInt(colorFromSlot2.g * (float) byte.MaxValue);
        string str9 = num.ToString();
        num = Mathf.RoundToInt(colorFromSlot2.b * (float) byte.MaxValue);
        string str10 = num.ToString();
        string str11 = $"\"{str8}, {str9}, {str10}\"";
        string str12 = $"{str7}{str11},";
        num = Mathf.RoundToInt(colorFromSlot3.r * (float) byte.MaxValue);
        string str13 = num.ToString();
        num = Mathf.RoundToInt(colorFromSlot3.g * (float) byte.MaxValue);
        string str14 = num.ToString();
        num = Mathf.RoundToInt(colorFromSlot3.b * (float) byte.MaxValue);
        string str15 = num.ToString();
        string str16 = $"\"{str13}, {str14}, {str15}\"";
        string str17 = $"{str12}{str16},";
        num = Mathf.RoundToInt(colorFromSlot4.r * (float) byte.MaxValue);
        string str18 = num.ToString();
        num = Mathf.RoundToInt(colorFromSlot4.g * (float) byte.MaxValue);
        string str19 = num.ToString();
        num = Mathf.RoundToInt(colorFromSlot4.b * (float) byte.MaxValue);
        string str20 = num.ToString();
        string str21 = $"\"{str18}, {str19}, {str20}\"";
        string str22 = $"{str17}{str21},";
        num = Mathf.RoundToInt(colorFromSlot5.r * (float) byte.MaxValue);
        string str23 = num.ToString();
        num = Mathf.RoundToInt(colorFromSlot5.g * (float) byte.MaxValue);
        string str24 = num.ToString();
        num = Mathf.RoundToInt(colorFromSlot5.b * (float) byte.MaxValue);
        string str25 = num.ToString();
        string str26 = $"\"{str23}, {str24}, {str25}\"";
        string str27 = $"{str22}{str26},";
        num = Mathf.RoundToInt(colorFromSlot6.r * (float) byte.MaxValue);
        string str28 = num.ToString();
        num = Mathf.RoundToInt(colorFromSlot6.g * (float) byte.MaxValue);
        string str29 = num.ToString();
        num = Mathf.RoundToInt(colorFromSlot6.b * (float) byte.MaxValue);
        string str30 = num.ToString();
        string str31 = $"\"{str28}, {str29}, {str30}\"";
        string str32 = $"{str27}{str31},";
        num = Mathf.RoundToInt(colorFromSlot7.r * (float) byte.MaxValue);
        string str33 = num.ToString();
        num = Mathf.RoundToInt(colorFromSlot7.g * (float) byte.MaxValue);
        string str34 = num.ToString();
        num = Mathf.RoundToInt(colorFromSlot7.b * (float) byte.MaxValue);
        string str35 = num.ToString();
        string str36 = $"\"{str33}, {str34}, {str35}\"";
        string str37 = $"{str32}{str36},";
        num = Mathf.RoundToInt(colorFromSlot8.r * (float) byte.MaxValue);
        string str38 = num.ToString();
        num = Mathf.RoundToInt(colorFromSlot8.g * (float) byte.MaxValue);
        string str39 = num.ToString();
        num = Mathf.RoundToInt(colorFromSlot8.b * (float) byte.MaxValue);
        string str40 = num.ToString();
        string str41 = $"\"{str38}, {str39}, {str40}\"";
        string str42 = $"{str37}{str41},";
        textWriter2.WriteLine(str42);
        if (character.LockColor)
          break;
      }
    }
    textWriter2.Close();
  }

  public void ExportJson()
  {
    TextWriter textWriter = (TextWriter) new StreamWriter(Application.persistentDataPath + "/CotL Skin Variations.json", false);
    WorshipperData.JsonData jsonData = new WorshipperData.JsonData();
    jsonData.SkinData = new List<WorshipperData.SkinData>();
    foreach (WorshipperData.SkinAndData character in this.Characters)
    {
      List<WorshipperData.SlotsAndColours> slotsAndColoursList = new List<WorshipperData.SlotsAndColours>((IEnumerable<WorshipperData.SlotsAndColours>) character.SlotAndColours);
      slotsAndColoursList.AddRange((IEnumerable<WorshipperData.SlotsAndColours>) this.GlobalColourList);
      WorshipperData.SkinData skinData = new WorshipperData.SkinData();
      skinData.ColourData = new WorshipperData.ColourData[slotsAndColoursList.Count];
      skinData.SkinName = character.Title;
      for (int index = 0; index < slotsAndColoursList.Count; ++index)
      {
        skinData.ColourData[index].HEAD_SKIN_TOP = "#" + ColorUtility.ToHtmlStringRGB(WorshipperData.GetColorFromSlot(slotsAndColoursList[index].SlotAndColours, "HEAD_SKIN_TOP"));
        skinData.ColourData[index].HEAD_SKIN_BTM = "#" + ColorUtility.ToHtmlStringRGB(WorshipperData.GetColorFromSlot(slotsAndColoursList[index].SlotAndColours, "HEAD_SKIN_BTM"));
        skinData.ColourData[index].ARM_LEFT_SKIN = "#" + ColorUtility.ToHtmlStringRGB(WorshipperData.GetColorFromSlot(slotsAndColoursList[index].SlotAndColours, "ARM_LEFT_SKIN"));
        skinData.ColourData[index].ARM_RIGHT_SKIN = "#" + ColorUtility.ToHtmlStringRGB(WorshipperData.GetColorFromSlot(slotsAndColoursList[index].SlotAndColours, "ARM_RIGHT_SKIN"));
        skinData.ColourData[index].LEG_LEFT_SKIN = "#" + ColorUtility.ToHtmlStringRGB(WorshipperData.GetColorFromSlot(slotsAndColoursList[index].SlotAndColours, "LEG_LEFT_SKIN"));
        skinData.ColourData[index].LEG_RIGHT_SKIN = "#" + ColorUtility.ToHtmlStringRGB(WorshipperData.GetColorFromSlot(slotsAndColoursList[index].SlotAndColours, "LEG_RIGHT_SKIN"));
        skinData.ColourData[index].MARKINGS = "#" + ColorUtility.ToHtmlStringRGB(WorshipperData.GetColorFromSlot(slotsAndColoursList[index].SlotAndColours, "MARKINGS"));
        skinData.ColourData[index].BODY_NAKED = "#" + ColorUtility.ToHtmlStringRGB(WorshipperData.GetColorFromSlot(slotsAndColoursList[index].SlotAndColours, "BODY_NAKED"));
        if (character.LockColor)
          break;
      }
      jsonData.SkinData.Add(skinData);
    }
    string json = JsonUtility.ToJson((object) jsonData, true);
    textWriter.Write(json);
    textWriter.Close();
  }

  public static Color GetColorFromSlot(
    List<WorshipperData.SlotAndColor> SlotAndColours,
    string slot)
  {
    for (int index = 0; index < SlotAndColours.Count; ++index)
    {
      if (SlotAndColours[index].Slot == slot)
        return SlotAndColours[index].color;
    }
    return Color.blue;
  }

  public static int GetClosestColorSlotFromColor(
    int fromSkinCharacter,
    int fromColorSlot,
    int toSkinCharacter)
  {
    WorshipperData.SkinAndData character = WorshipperData._Instance.Characters[fromSkinCharacter];
    Color allColor = character.SlotAndColours[Mathf.Min(fromColorSlot, character.SlotAndColours.Count - 1)].AllColor;
    List<WorshipperData.SlotsAndColours> slotAndColours = WorshipperData._Instance.Characters[toSkinCharacter].SlotAndColours;
    float num1 = float.MaxValue;
    int colorSlotFromColor = 0;
    for (int index = 0; index < slotAndColours.Count; ++index)
    {
      WorshipperData.SlotsAndColours slotsAndColours = slotAndColours[index];
      float num2 = Vector3.Distance(new Vector3(slotsAndColours.AllColor.r, slotsAndColours.AllColor.g, slotsAndColours.AllColor.b), new Vector3(allColor.r, allColor.g, allColor.b));
      if ((double) num2 < (double) num1)
      {
        num1 = num2;
        colorSlotFromColor = index;
      }
    }
    return colorSlotFromColor;
  }

  public enum DropLocation
  {
    Dungeon1,
    Dungeon2,
    Dungeon3,
    Dungeon4,
    Other,
    DLC,
    SpecialEvents,
    Major_DLC,
    Dungeon5,
    Dungeon6,
  }

  [Serializable]
  public class SkinAndData
  {
    [SerializeField]
    public string Title = "Character Name";
    [SerializeField]
    public WorshipperData.DropLocation _dropLocation;
    [SerializeField]
    public bool _hidden;
    [SerializeField]
    public bool _invariant;
    [SerializeField]
    public bool _lockColor;
    public bool TwitchPremium;
    public List<WorshipperData.CharacterSkin> Skin = new List<WorshipperData.CharacterSkin>();
    public List<WorshipperData.SlotsAndColours> SlotAndColours = new List<WorshipperData.SlotsAndColours>();

    public bool LockColor => this._lockColor;

    public bool Contains(string SkinName)
    {
      foreach (WorshipperData.CharacterSkin characterSkin in this.Skin)
      {
        if (characterSkin.Skin == SkinName)
          return true;
      }
      return false;
    }

    public string GetName(WorshipperData.CharacterSkin skin) => skin.Skin;

    public WorshipperData.DropLocation DropLocation => this._dropLocation;

    public bool Hidden => this._hidden;

    public bool Invariant => this._invariant;

    public void AddNewColours()
    {
      this.SlotAndColours.Add(new WorshipperData.SlotsAndColours()
      {
        SlotAndColours = {
          new WorshipperData.SlotAndColor("HEAD_SKIN_TOP", Color.white),
          new WorshipperData.SlotAndColor("HEAD_SKIN_BTM", Color.white),
          new WorshipperData.SlotAndColor("MARKINGS", Color.white),
          new WorshipperData.SlotAndColor("ARM_LEFT_SKIN", Color.white),
          new WorshipperData.SlotAndColor("ARM_RIGHT_SKIN", Color.white),
          new WorshipperData.SlotAndColor("LEG_LEFT_SKIN", Color.white),
          new WorshipperData.SlotAndColor("LEG_RIGHT_SKIN", Color.white)
        }
      });
    }
  }

  [Serializable]
  public class CharacterSkin
  {
    [SpineSkin("", "SkeletonData", true, false, false)]
    public string Skin;

    public void Test()
    {
      WorshipperData.Instance.SkeletonData.gameObject.SetActive(true);
      WorshipperData.Instance.SkeletonData.skeleton.SetSkin(this.Skin);
    }
  }

  [Serializable]
  public class SlotsAndColours
  {
    public List<WorshipperData.SlotAndColor> SlotAndColours = new List<WorshipperData.SlotAndColor>();
    public Color AllColor = Color.white;

    public void AddNewColours()
    {
      this.SlotAndColours.Add(new WorshipperData.SlotAndColor("", this.AllColor));
    }

    public void SetAll()
    {
      foreach (WorshipperData.SlotAndColor slotAndColour in this.SlotAndColours)
        slotAndColour.color = this.AllColor;
    }

    public void Test()
    {
      WorshipperData.Instance.SkeletonData.gameObject.SetActive(true);
      foreach (WorshipperData.SlotAndColor slotAndColour in this.SlotAndColours)
      {
        Slot slot = WorshipperData.Instance.SkeletonData.skeleton.FindSlot(slotAndColour.Slot);
        if (slot != null)
          slot.SetColor(slotAndColour.color);
      }
    }

    public bool HasSlot(string slot)
    {
      foreach (WorshipperData.SlotAndColor slotAndColour in this.SlotAndColours)
      {
        if (slotAndColour.Slot == slot)
          return true;
      }
      return false;
    }

    public void AddAllSlots()
    {
      if (!this.HasSlot("HEAD_SKIN_TOP"))
        this.SlotAndColours.Add(new WorshipperData.SlotAndColor("HEAD_SKIN_TOP", this.AllColor));
      if (!this.HasSlot("HEAD_SKIN_BTM"))
        this.SlotAndColours.Add(new WorshipperData.SlotAndColor("HEAD_SKIN_BTM", this.AllColor));
      if (!this.HasSlot("ARM_LEFT_SKIN"))
        this.SlotAndColours.Add(new WorshipperData.SlotAndColor("ARM_LEFT_SKIN", this.AllColor));
      if (!this.HasSlot("ARM_RIGHT_SKIN"))
        this.SlotAndColours.Add(new WorshipperData.SlotAndColor("ARM_RIGHT_SKIN", this.AllColor));
      if (!this.HasSlot("LEG_LEFT_SKIN"))
        this.SlotAndColours.Add(new WorshipperData.SlotAndColor("LEG_LEFT_SKIN", this.AllColor));
      if (!this.HasSlot("LEG_RIGHT_SKIN"))
        this.SlotAndColours.Add(new WorshipperData.SlotAndColor("LEG_RIGHT_SKIN", this.AllColor));
      if (this.HasSlot("BODY_NAKED"))
        return;
      this.SlotAndColours.Add(new WorshipperData.SlotAndColor("BODY_NAKED", this.SlotAndColours[this.SlotAndColours.Count - 1].color));
    }

    public void AddAllClothingSlots()
    {
      this.SlotAndColours.Add(new WorshipperData.SlotAndColor("SLEEVE_RIGHT_BTM", Color.white));
      this.SlotAndColours.Add(new WorshipperData.SlotAndColor("SLEEVE_RIGHT_TOP", Color.white));
      this.SlotAndColours.Add(new WorshipperData.SlotAndColor("SLEEVE_LEFT_BTM", Color.white));
      this.SlotAndColours.Add(new WorshipperData.SlotAndColor("SLEEVE_LEFT_TOP", Color.white));
      this.SlotAndColours.Add(new WorshipperData.SlotAndColor("BODY_BTM", Color.white));
      this.SlotAndColours.Add(new WorshipperData.SlotAndColor("BODY_TOP", Color.white));
      this.SlotAndColours.Add(new WorshipperData.SlotAndColor("BODY_EXTRA", Color.white));
      this.SlotAndColours.Add(new WorshipperData.SlotAndColor("HOOD_BTM", Color.white));
      this.SlotAndColours.Add(new WorshipperData.SlotAndColor("HOOD_TOP", Color.white));
      this.SlotAndColours.Add(new WorshipperData.SlotAndColor("SHAWL_BTM", Color.white));
      this.SlotAndColours.Add(new WorshipperData.SlotAndColor("SHAWL_TOP", Color.white));
    }

    public void AddAllAnimalSlots()
    {
      this.SlotAndColours.Add(new WorshipperData.SlotAndColor("Body", Color.white));
      this.SlotAndColours.Add(new WorshipperData.SlotAndColor("Body_Top", Color.white));
      this.SlotAndColours.Add(new WorshipperData.SlotAndColor("Head", Color.white));
      this.SlotAndColours.Add(new WorshipperData.SlotAndColor("Head_Top", Color.white));
      this.SlotAndColours.Add(new WorshipperData.SlotAndColor("Horns", Color.white));
      this.SlotAndColours.Add(new WorshipperData.SlotAndColor("Ear_Left", Color.white));
      this.SlotAndColours.Add(new WorshipperData.SlotAndColor("Ear_Right", Color.white));
      this.SlotAndColours.Add(new WorshipperData.SlotAndColor("Leg1", Color.white));
      this.SlotAndColours.Add(new WorshipperData.SlotAndColor("Leg2", Color.white));
      this.SlotAndColours.Add(new WorshipperData.SlotAndColor("Leg3", Color.white));
      this.SlotAndColours.Add(new WorshipperData.SlotAndColor("Leg4", Color.white));
      this.SlotAndColours.Add(new WorshipperData.SlotAndColor("Tail", Color.white));
    }
  }

  [Serializable]
  public class SlotAndColor
  {
    [SpineSlot("", "SkeletonData", false, true, false)]
    public string Slot;
    public Color color = Color.white;

    public SlotAndColor(string Slot, Color color)
    {
      this.Slot = Slot;
      this.color = color;
    }
  }

  [Serializable]
  public class Character
  {
    public string skinName;
    public bool premium;
    public int colorOptionIndex;
    public string HEAD_SKIN_TOP;
    public string HEAD_SKIN_BTM;
    public string ARM_LEFT_SKIN;
    public string ARM_RIGHT_SKIN;
    public string LEG_LEFT_SKIN;
    public string LEG_RIGHT_SKIN;
    public string MARKINGS;
  }

  [Serializable]
  public struct JsonData
  {
    public List<WorshipperData.SkinData> SkinData;
  }

  [Serializable]
  public struct SkinData
  {
    public string SkinName;
    public WorshipperData.ColourData[] ColourData;
  }

  [Serializable]
  public struct ColourData
  {
    public string HEAD_SKIN_TOP;
    public string HEAD_SKIN_BTM;
    public string ARM_LEFT_SKIN;
    public string ARM_RIGHT_SKIN;
    public string LEG_LEFT_SKIN;
    public string LEG_RIGHT_SKIN;
    public string MARKINGS;
    public string BODY_NAKED;
  }
}
