// Decompiled with JetBrains decompiler
// Type: TwitchFollowers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Org.OpenAPITools.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

#nullable disable
public static class TwitchFollowers
{
  public static bool Active = false;
  public static bool Request_Active = false;
  public static TwitchFollowers.RaffleData CurrentData;
  public const string CREATED = "CREATED";
  public const string READY_FOR_CREATION = "READY_FOR_CREATION";
  public const string INTRO = "INTRO";
  public const string SKIN_SELECTION = "SKIN_SELECTION";
  public const string COLOR_SELECTION = "COLOR_SELECTION";
  public const string VARIATION_SELECTION = "VARIATION_SELECTION";
  public const string OUTFIT_SELECTION = "OUTFIT_SELECTION";
  public static bool Deactivated = false;
  public static float timer = 0.0f;
  public static float Interval = 120f;
  public static List<string> BLACKLISTED_FOLLOWERS = new List<string>()
  {
    "CultLeader 1",
    "CultLeader 2",
    "CultLeader 3",
    "CultLeader 4",
    "CultLeader 1 Healed",
    "CultLeader 2 Healed",
    "CultLeader 3 Healed",
    "CultLeader 4 Healed",
    "Boss Dog 1",
    "Boss Dog 2",
    "Boss Dog 3",
    "Boss Dog 4",
    "Boss Dog 5",
    "Boss Dog 6",
    "Boss Rot 1",
    "Boss Rot 2",
    "Boss Rot 3",
    "Boss Rot 4",
    "Boss Beholder 5",
    "Boss Beholder 6",
    "ChosenChild",
    "Snowman/Good_1",
    "Snowman/Good_2",
    "Snowman/Good_3",
    "PalworldOne",
    "PalworldTwo",
    "PalworldThree",
    "PalworldFour",
    "PalworldFive"
  };

  public static event TwitchFollowers.RaffleResponse RaffleUpdated;

  public static event TwitchFollowers.FollowerResponse FollowerCreated;

  public static event TwitchFollowers.FollowerResponse FollowerCreationProgress;

  public static void Initialise() => TwitchFollowers.Abort();

  public static void Update()
  {
    if (!TwitchFollowers.Active || TwitchFollowers.Request_Active || (double) Time.unscaledTime <= (double) TwitchFollowers.timer)
      return;
    TwitchFollowers.Request_Active = true;
    TwitchFollowers.GetRaffle((TwitchFollowers.RaffleResponse) ((response, data) =>
    {
      if (data != null)
      {
        if (TwitchFollowers.Active)
        {
          TwitchFollowers.RaffleResponse raffleUpdated = TwitchFollowers.RaffleUpdated;
          if (raffleUpdated != null)
            raffleUpdated(response, data);
        }
        TwitchFollowers.CurrentData = data;
      }
      TwitchFollowers.Request_Active = false;
    }));
    TwitchFollowers.timer = Time.unscaledTime + TwitchFollowers.Interval / 60f;
  }

  public static void StartRaffle(TwitchFollowers.RaffleResponse raffleResponse)
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) TwitchFollowers.StartRaffleIE(raffleResponse));
  }

  public static IEnumerator StartRaffleIE(TwitchFollowers.RaffleResponse raffleResponse)
  {
    TwitchFollowers.Request_Active = false;
    TwitchFollowers.Active = false;
    StartRaffleRequest startRaffleRequest = new StartRaffleRequest(new StartRaffleRequestType("VIEWER_FOLLOWER_INDOCTRINATION"), new StartRaffleRequestWeights(1M, 1M, 1M), 1M);
    System.Threading.Tasks.Task task = TwitchRequest.DEFAULT_API.StartRaffleAsync(startRaffleRequest, new CancellationToken());
    yield return (object) new WaitUntil((Func<bool>) (() => task.IsCompleted));
    Debug.Log((object) "RAFFLE CREATED");
    TwitchFollowers.Active = true;
    TwitchFollowers.RaffleResponse raffleResponse1 = raffleResponse;
    if (raffleResponse1 != null)
      raffleResponse1(task.IsCompletedSuccessfully ? TwitchRequest.ResponseType.Success : TwitchRequest.ResponseType.Failure, (TwitchFollowers.RaffleData) null);
  }

  public static void EndRaffle(TwitchFollowers.RaffleResponse raffleResponse)
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) TwitchFollowers.EndRaffleIE(raffleResponse));
  }

  public static IEnumerator EndRaffleIE(TwitchFollowers.RaffleResponse raffleResponse)
  {
    TwitchFollowers.Active = false;
    TwitchFollowers.Request_Active = false;
    Task<ResolvedRaffle> endRaffleTask = TwitchRequest.DEFAULT_API.EndRaffleAsync(new CancellationToken());
    yield return (object) new WaitUntil((Func<bool>) (() => endRaffleTask.IsCompleted));
    Debug.Log((object) "RAFFLE ENDED");
    if (endRaffleTask.Result.Winners.Count <= 0)
    {
      raffleResponse(TwitchRequest.ResponseType.Failure, (TwitchFollowers.RaffleData) null);
    }
    else
    {
      string viewerId = endRaffleTask.Result.Winners[0].ViewerId;
      Task<GetViewerFollowerCustomization200Response> customisationTask = TwitchRequest.DEFAULT_API.GetViewerFollowerCustomizationAsync(viewerId, new CancellationToken());
      yield return (object) new WaitUntil((Func<bool>) (() => customisationTask.IsCompleted));
      if (customisationTask.Result != null)
      {
        TwitchFollowers.ViewerFollowerData data = new TwitchFollowers.ViewerFollowerData()
        {
          viewer_display_name = endRaffleTask.Result.Winners[0].ViewerDisplayName,
          id = customisationTask.Result.ChannelId,
          viewer_id = customisationTask.Result.ViewerId,
          customisations = new TwitchFollowers.FollowerData()
        };
        data.customisations.skin_name = customisationTask.Result.Customizations.SkinName;
        data.customisations.color = new TwitchFollowers.ColorData()
        {
          colorOptionIndex = (int) customisationTask.Result.Customizations.SkinColorOptionIndex
        };
        data.customisations.outfit_skin_name = customisationTask.Result.Customizations.OutfitSkinName;
        TwitchFollowers.FollowerResponse followerCreated = TwitchFollowers.FollowerCreated;
        if (followerCreated != null)
          followerCreated(data);
      }
      TwitchFollowers.RaffleResponse raffleResponse1 = raffleResponse;
      if (raffleResponse1 != null)
        raffleResponse1(TwitchRequest.ResponseType.Success, new TwitchFollowers.RaffleData());
    }
  }

  public static void GetRaffle(TwitchFollowers.RaffleResponse raffleResponse)
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) TwitchFollowers.GetRaffleIE(raffleResponse));
  }

  public static IEnumerator GetRaffleIE(TwitchFollowers.RaffleResponse raffleResponse)
  {
    Task<Raffle> raffle = TwitchRequest.DEFAULT_API.GetRaffleAsync(new CancellationToken());
    yield return (object) new WaitUntil((Func<bool>) (() => raffle.IsCompleted));
    TwitchFollowers.RaffleResponse raffleResponse1 = raffleResponse;
    if (raffleResponse1 != null)
      raffleResponse1(TwitchRequest.ResponseType.Success, new TwitchFollowers.RaffleData()
      {
        participants = (int) raffle.Result.Participants
      });
  }

  public static void SendEnabledSkins()
  {
    if (!TwitchAuthentication.IsAuthenticated)
      return;
    List<string> followerNames = new List<string>();
    for (int index = 0; index < DataManager.Instance.FollowerSkinsUnlocked.Count; ++index)
    {
      WorshipperData.SkinAndData characters = WorshipperData.Instance.GetCharacters(DataManager.Instance.FollowerSkinsUnlocked[index]);
      if (characters != null && !characters.Invariant && !TwitchFollowers.BLACKLISTED_FOLLOWERS.Contains(DataManager.Instance.FollowerSkinsUnlocked[index]))
        followerNames.Add(DataManager.Instance.FollowerSkinsUnlocked[index]);
    }
    List<FollowerClothingType> availableClothing = new List<FollowerClothingType>();
    foreach (ClothingData clothingData in TailorManager.GetAvailableClothing())
    {
      if (TailorManager.GetCraftedCount(clothingData.ClothingType) > 0)
        availableClothing.Add(clothingData.ClothingType);
    }
    if (availableClothing.Count <= 0)
      availableClothing.Add(FollowerClothingType.None);
    GameManager.GetInstance().StartCoroutine((IEnumerator) TwitchFollowers.SendEnabledSkinsIE(followerNames, availableClothing));
  }

  public static IEnumerator SendEnabledSkinsIE(
    List<string> followerNames,
    List<FollowerClothingType> availableClothing)
  {
    List<string> enabledOutfitNames = new List<string>();
    Dictionary<string, Decimal> outfitColorVariationMap = new Dictionary<string, Decimal>();
    for (int index = 0; index < availableClothing.Count; ++index)
    {
      List<string> stringList = enabledOutfitNames;
      FollowerClothingType followerClothingType = availableClothing[index];
      string str = followerClothingType.ToString();
      stringList.Add(str);
      Dictionary<string, Decimal> dictionary = outfitColorVariationMap;
      followerClothingType = availableClothing[index];
      string key = followerClothingType.ToString();
      Decimal clothingColour = (Decimal) DataManager.Instance.GetClothingColour(availableClothing[index]);
      dictionary.Add(key, clothingColour);
    }
    FollowerCustomizationOptionsGameData followerCustomizationOptionsGameData = new FollowerCustomizationOptionsGameData(followerNames, enabledOutfitNames, outfitColorVariationMap);
    yield return (object) TwitchRequest.DEFAULT_API.UpdateFollowerCustomizationOptionsGameDataAsync(followerCustomizationOptionsGameData, new CancellationToken());
  }

  public static void SendFollowers()
  {
    if (!TwitchAuthentication.IsAuthenticated || TwitchFollowers.Deactivated)
      return;
    List<FollowerInfo> followerInfoList = new List<FollowerInfo>((IEnumerable<FollowerInfo>) DataManager.Instance.Followers);
    followerInfoList.AddRange((IEnumerable<FollowerInfo>) DataManager.Instance.Followers_Possessed);
    followerInfoList.AddRange((IEnumerable<FollowerInfo>) DataManager.Instance.Followers_Dissented);
    followerInfoList.AddRange((IEnumerable<FollowerInfo>) DataManager.Instance.Followers_Dead);
    List<TwitchFollowers.FollowerInfoData_Send> followerInfoDataSendList1 = new List<TwitchFollowers.FollowerInfoData_Send>();
    List<TwitchFollowers.FollowerInfoData_Send> followerInfoDataSendList2 = new List<TwitchFollowers.FollowerInfoData_Send>();
    for (int index1 = 0; index1 < followerInfoList.Count; ++index1)
    {
      if (!TwitchFollowers.BLACKLISTED_FOLLOWERS.Contains(followerInfoList[index1].SkinName))
      {
        TwitchFollowers.FollowerInfoData_Send followerInfoDataSend = new TwitchFollowers.FollowerInfoData_Send()
        {
          id = string.IsNullOrEmpty(followerInfoList[index1].ViewerID) ? followerInfoList[index1].ID.ToString() : followerInfoList[index1].ViewerID,
          stats = new TwitchFollowers.FollowerStatsData()
          {
            level = followerInfoList[index1].XPLevel,
            name = followerInfoList[index1].Name,
            reason_of_death = DataManager.Instance.Followers_Dead_IDs.Contains(followerInfoList[index1].ID) ? followerInfoList[index1].GetDeathText(false, false) : ""
          },
          customisations = new TwitchFollowers.FollowerData()
          {
            skin_name = followerInfoList[index1].SkinName,
            color = new TwitchFollowers.ColorData()
          }
        };
        followerInfoDataSend.customisations.color.colorOptionIndex = followerInfoList[index1].SkinColour;
        followerInfoDataSend.stats.traits = new List<FollowerStatsTraitsInner>();
        for (int index2 = 0; index2 < followerInfoList[index1].Traits.Count; ++index2)
        {
          string name = FollowerTrait.GetLocalizedTitle(followerInfoList[index1].Traits[index2]);
          if (followerInfoList[index1].Traits[index2] == FollowerTrait.TraitType.BishopOfCult)
            name = name.Replace("<color=#FFD201>", "").Replace("</color>", "");
          followerInfoDataSend.stats.traits.Add(new FollowerStatsTraitsInner(followerInfoList[index1].Traits[index2].ToString(), name, FollowerTrait.IsPositiveTrait(followerInfoList[index1].Traits[index2]) ? FollowerStatsTraitsInner.TypeEnum.Positive : FollowerStatsTraitsInner.TypeEnum.Negative));
        }
        WorshipperData.SkinAndData colourData = WorshipperData.Instance.GetColourData(followerInfoList[index1].SkinName);
        if (colourData != null)
        {
          foreach (WorshipperData.SlotAndColor slotAndColour in colourData.SlotAndColours[Mathf.Clamp(followerInfoList[index1].SkinColour, 0, colourData.SlotAndColours.Count - 1)].SlotAndColours)
          {
            if (slotAndColour.Slot == "HEAD_SKIN_TOP")
              followerInfoDataSend.customisations.color.HEAD_SKIN_TOP = $"{Mathf.RoundToInt(slotAndColour.color.r * (float) byte.MaxValue)}, {Mathf.RoundToInt(slotAndColour.color.g * (float) byte.MaxValue)}, {Mathf.RoundToInt(slotAndColour.color.b * (float) byte.MaxValue)}";
            else if (slotAndColour.Slot == "HEAD_SKIN_BTM")
              followerInfoDataSend.customisations.color.HEAD_SKIN_BTM = $"{Mathf.RoundToInt(slotAndColour.color.r * (float) byte.MaxValue)}, {Mathf.RoundToInt(slotAndColour.color.g * (float) byte.MaxValue)}, {Mathf.RoundToInt(slotAndColour.color.b * (float) byte.MaxValue)}";
            else if (slotAndColour.Slot == "ARM_LEFT_SKIN")
              followerInfoDataSend.customisations.color.ARM_LEFT_SKIN = $"{Mathf.RoundToInt(slotAndColour.color.r * (float) byte.MaxValue)}, {Mathf.RoundToInt(slotAndColour.color.g * (float) byte.MaxValue)}, {Mathf.RoundToInt(slotAndColour.color.b * (float) byte.MaxValue)}";
            else if (slotAndColour.Slot == "ARM_RIGHT_SKIN")
              followerInfoDataSend.customisations.color.ARM_RIGHT_SKIN = $"{Mathf.RoundToInt(slotAndColour.color.r * (float) byte.MaxValue)}, {Mathf.RoundToInt(slotAndColour.color.g * (float) byte.MaxValue)}, {Mathf.RoundToInt(slotAndColour.color.b * (float) byte.MaxValue)}";
            else if (slotAndColour.Slot == "LEG_LEFT_SKIN")
              followerInfoDataSend.customisations.color.LEG_LEFT_SKIN = $"{Mathf.RoundToInt(slotAndColour.color.r * (float) byte.MaxValue)}, {Mathf.RoundToInt(slotAndColour.color.g * (float) byte.MaxValue)}, {Mathf.RoundToInt(slotAndColour.color.b * (float) byte.MaxValue)}";
            else if (slotAndColour.Slot == "LEG_RIGHT_SKIN")
              followerInfoDataSend.customisations.color.LEG_RIGHT_SKIN = $"{Mathf.RoundToInt(slotAndColour.color.r * (float) byte.MaxValue)}, {Mathf.RoundToInt(slotAndColour.color.g * (float) byte.MaxValue)}, {Mathf.RoundToInt(slotAndColour.color.b * (float) byte.MaxValue)}";
            else if (slotAndColour.Slot == "MARKINGS")
              followerInfoDataSend.customisations.color.MARKINGS = $"{Mathf.RoundToInt(slotAndColour.color.r * (float) byte.MaxValue)}, {Mathf.RoundToInt(slotAndColour.color.g * (float) byte.MaxValue)}, {Mathf.RoundToInt(slotAndColour.color.b * (float) byte.MaxValue)}";
          }
        }
        if (string.IsNullOrEmpty(followerInfoList[index1].ViewerID))
          followerInfoDataSendList2.Add(followerInfoDataSend);
        else
          followerInfoDataSendList1.Add(followerInfoDataSend);
      }
    }
    List<Org.OpenAPITools.Model.Follower> follower = new List<Org.OpenAPITools.Model.Follower>();
    for (int index = 0; index < followerInfoDataSendList1.Count; ++index)
      follower.Add(new Org.OpenAPITools.Model.Follower(Org.OpenAPITools.Model.Follower.TypeEnum.Viewer, followerInfoDataSendList1[index].id, followerInfoDataSendList1[index].stats.name, new FollowerStats((Decimal) followerInfoDataSendList1[index].stats.level, followerInfoDataSendList1[index].stats.reason_of_death, followerInfoDataSendList1[index].stats.traits), new FollowerCustomizationProperties(followerInfoDataSendList1[index].customisations.skin_name, (Decimal) followerInfoDataSendList1[index].customisations.color.colorOptionIndex, string.IsNullOrEmpty(followerInfoDataSendList1[index].customisations.outfit_skin_name) ? "" : followerInfoDataSendList1[index].customisations.outfit_skin_name)));
    for (int index = 0; index < followerInfoDataSendList2.Count; ++index)
      follower.Add(new Org.OpenAPITools.Model.Follower(Org.OpenAPITools.Model.Follower.TypeEnum.Npc, followerInfoDataSendList2[index].id, followerInfoDataSendList2[index].stats.name, new FollowerStats((Decimal) followerInfoDataSendList2[index].stats.level, followerInfoDataSendList2[index].stats.reason_of_death, followerInfoDataSendList2[index].stats.traits), new FollowerCustomizationProperties(followerInfoDataSendList2[index].customisations.skin_name, (Decimal) followerInfoDataSendList2[index].customisations.color.colorOptionIndex, string.IsNullOrEmpty(followerInfoDataSendList2[index].customisations.outfit_skin_name) ? "" : followerInfoDataSendList2[index].customisations.outfit_skin_name)));
    TwitchRequest.DEFAULT_API.UpdateFollowersGameDataAsync(follower, new CancellationToken());
  }

  public static void Abort()
  {
    if (TwitchAuthentication.IsAuthenticated)
      TwitchRequest.DEFAULT_API.EndRaffleAsync(new CancellationToken());
    TwitchFollowers.Active = false;
  }

  [Serializable]
  public class RaffleData
  {
    public long channel_id;
    public int winning_viewer_id;
    public string created_at;
    public string updated_at;
    public int participants;
    public string OutfitSkinName;
    public Decimal SkinColorOptionIndex;
    public string SkinName;
    public string winning_viewer_display_name;
  }

  [Serializable]
  public class ViewerFollowerData
  {
    public long channel_id;
    public string viewer_id;
    public string viewer_display_name;
    public string status;
    public TwitchFollowers.FollowerData customisations;
    public string premium_bits_transaction_id;
    public string created_at;
    public float updated_at;
    public string customisation_step;
    public string recent_chat_message;
    public string id;
    public string save_id;
  }

  [Serializable]
  public class FollowerData
  {
    public string skin_name;
    public TwitchFollowers.ColorData color;
    public string outfit_skin_name;
    public TwitchFollowers.OutfitColorData outfit_color;
  }

  [Serializable]
  public class ColorData
  {
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
  public class OutfitColorData
  {
    public string SLEEVE_RIGHT_BTM;
    public string SLEEVE_RIGHT_TOP;
    public string SLEEVE_LEFT_BTM;
    public string SLEEVE_LEFT_TOP;
    public string BODY_BTM;
    public string BODY_TOP;
    public string BODY_EXTRA;
    public string HOOD_BTM;
    public string HOOD_TOP;
    public string SHAWL_BTM;
    public string SHAWL_TOP;
  }

  [Serializable]
  public class UnlockedSkinsData
  {
    public string[] enabled_skin_names;
    public string[] enabled_outfit_names;
    public TwitchFollowers.OutfitColourData outfit_color_variation_map;
  }

  [Serializable]
  public class OutfitColourData
  {
    public int Robe;
    public int Robes_Fancy;
    public int Warrior;
    public int Suit_Fancy;
    public int Cultist_DLC;
    public int Cultist_DLC2;
    public int Heretic_DLC;
    public int Heretic_DLC2;
    public int DLC_1;
    public int DLC_2;
    public int DLC_3;
    public int DLC_4;
    public int DLC_5;
    public int DLC_6;
    public int Pilgrim_DLC;
    public int Pilgrim_DLC2;
    public int Special_1;
    public int Special_2;
    public int Special_3;
    public int Special_4;
    public int Special_5;
    public int Special_6;
    public int Special_7;
    public int Normal_1;
    public int Normal_2;
    public int Normal_3;
    public int Normal_4;
    public int Normal_5;
    public int Normal_6;
    public int Normal_7;
    public int Normal_8;
    public int Normal_9;
    public int Normal_10;
    public int Normal_11;
    public int Normal_12;
    public int Naked;
  }

  [Serializable]
  public class FollowerInfoData
  {
    public string type;
    public TwitchFollowers.FollowerStatsData stats;
  }

  [Serializable]
  public class FollowerStatsData
  {
    public string name;
    public int level;
    public string reason_of_death;
    public List<FollowerStatsTraitsInner> traits;
  }

  [Serializable]
  public class FollowerTraitData
  {
    public string id;
    public string name;
    public string type;
  }

  [Serializable]
  public class FollowerData_Send
  {
    public TwitchFollowers.FollowerInfoData_Send[] viewers;
    public TwitchFollowers.FollowerInfoData_Send[] npcs;
  }

  [Serializable]
  public class FollowerInfoData_Send
  {
    public string id;
    public TwitchFollowers.FollowerStatsData stats;
    public TwitchFollowers.FollowerData customisations;
  }

  public delegate void RaffleResponse(
    TwitchRequest.ResponseType response,
    TwitchFollowers.RaffleData data);

  public delegate void FollowerAllResponse(TwitchFollowers.ViewerFollowerData[] data);

  public delegate void FollowerResponse(TwitchFollowers.ViewerFollowerData data);
}
