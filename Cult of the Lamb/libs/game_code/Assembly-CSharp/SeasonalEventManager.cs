// Decompiled with JetBrains decompiler
// Type: SeasonalEventManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class SeasonalEventManager
{
  public static SeasonalEventData[] _cachedSeasonalEvents;
  public const string SeasonalEventsPath = "Data/Seasonal Event Data";

  public static SeasonalEventData[] SeasonalEvents
  {
    get
    {
      if (SeasonalEventManager._cachedSeasonalEvents == null)
        SeasonalEventManager._cachedSeasonalEvents = Resources.LoadAll<SeasonalEventData>("Data/Seasonal Event Data");
      return SeasonalEventManager._cachedSeasonalEvents;
    }
  }

  public static bool InitialiseEvents()
  {
    bool flag = false;
    foreach (SeasonalEventData seasonalEvent in SeasonalEventManager.SeasonalEvents)
    {
      if (seasonalEvent.IsEventActive() && !DataManager.Instance.ActiveSeasonalEvents.Contains(seasonalEvent.EventType))
      {
        SeasonalEventManager.ActivateEvent(seasonalEvent.EventType);
        flag = true;
      }
      else if (!seasonalEvent.IsEventActive() && DataManager.Instance.ActiveSeasonalEvents.Contains(seasonalEvent.EventType))
        SeasonalEventManager.DeactivateEvent(seasonalEvent.EventType);
    }
    return flag;
  }

  public static SeasonalEventData GetSeasonalEventData(SeasonalEventType eventType)
  {
    foreach (SeasonalEventData seasonalEvent in SeasonalEventManager.SeasonalEvents)
    {
      if (seasonalEvent.EventType == eventType)
        return seasonalEvent;
    }
    return (SeasonalEventData) null;
  }

  public static bool IsSeasonalEventActive(SeasonalEventType eventType)
  {
    foreach (SeasonalEventData seasonalEvent in SeasonalEventManager.SeasonalEvents)
    {
      if (seasonalEvent.EventType == eventType)
        return seasonalEvent.IsEventActive();
    }
    return false;
  }

  public static void ActivateEvent(SeasonalEventType eventType)
  {
    switch (eventType)
    {
      case SeasonalEventType.Halloween:
        SeasonalEventManager.ActivateHalloween();
        break;
      case SeasonalEventType.CNY:
        SeasonalEventManager.ActivateCNY();
        break;
      case SeasonalEventType.Cthulhu:
        SeasonalEventManager.ActivateCthulu();
        break;
    }
    DataManager.Instance.ActiveSeasonalEvents.Add(eventType);
  }

  public static void DeactivateEvent(SeasonalEventType eventType)
  {
    switch (eventType)
    {
      case SeasonalEventType.Halloween:
        SeasonalEventManager.DeactivateHalloween();
        break;
      case SeasonalEventType.CNY:
        SeasonalEventManager.DeactivateCNY();
        break;
      case SeasonalEventType.Cthulhu:
        SeasonalEventManager.DeactivateCthulu();
        break;
    }
    DataManager.Instance.ActiveSeasonalEvents.Remove(eventType);
  }

  public static SeasonalEventData GetActiveEvent()
  {
    SeasonalEventData[] seasonalEvents = SeasonalEventManager.SeasonalEvents;
    int index = 0;
    return index < seasonalEvents.Length ? seasonalEvents[index] : (SeasonalEventData) null;
  }

  public static void ActivateHalloween()
  {
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Halloween);
    DataManager.Instance.LastHalloween = float.MinValue;
  }

  public static void DeactivateHalloween()
  {
    UpgradeSystem.LockAbility(UpgradeSystem.Type.Ritual_Halloween);
  }

  public static void ActivateCNY()
  {
    DataManager.Instance.LastCNY = float.MinValue;
    Interaction_EventGift.GetGift(SeasonalEventType.CNY).gameObject.SetActive(true);
  }

  public static void DeactivateCNY()
  {
    Interaction_EventGift.GetGift(SeasonalEventType.CNY).gameObject.SetActive(false);
  }

  public static void ActivateCthulu()
  {
    DataManager.Instance.LastCthulhu = float.MinValue;
    Interaction_EventGift.GetGift(SeasonalEventType.Cthulhu).gameObject.SetActive(true);
  }

  public static void DeactivateCthulu()
  {
    Interaction_EventGift.GetGift(SeasonalEventType.Cthulhu).gameObject.SetActive(false);
  }
}
