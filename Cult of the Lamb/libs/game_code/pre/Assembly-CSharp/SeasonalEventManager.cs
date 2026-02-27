// Decompiled with JetBrains decompiler
// Type: SeasonalEventManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class SeasonalEventManager
{
  private static SeasonalEventData[] _cachedSeasonalEvents;
  private const string SeasonalEventsPath = "Data/Seasonal Event Data";

  private static SeasonalEventData[] SeasonalEvents
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

  private static void ActivateEvent(SeasonalEventType eventType)
  {
    if (eventType == SeasonalEventType.Halloween)
      SeasonalEventManager.ActivateHalloween();
    DataManager.Instance.ActiveSeasonalEvents.Add(eventType);
  }

  private static void DeactivateEvent(SeasonalEventType eventType)
  {
    if (eventType == SeasonalEventType.Halloween)
      SeasonalEventManager.DeactivateHalloween();
    DataManager.Instance.ActiveSeasonalEvents.Remove(eventType);
  }

  public static SeasonalEventData GetActiveEvent()
  {
    SeasonalEventData[] seasonalEvents = SeasonalEventManager.SeasonalEvents;
    int index = 0;
    return index < seasonalEvents.Length ? seasonalEvents[index] : (SeasonalEventData) null;
  }

  private static void ActivateHalloween()
  {
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Halloween);
    DataManager.Instance.LastHalloween = float.MinValue;
  }

  private static void DeactivateHalloween()
  {
    UpgradeSystem.LockAbility(UpgradeSystem.Type.Ritual_Halloween);
  }
}
