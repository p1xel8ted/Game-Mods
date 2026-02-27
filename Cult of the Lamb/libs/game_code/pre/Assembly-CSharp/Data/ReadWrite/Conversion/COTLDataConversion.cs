// Decompiled with JetBrains decompiler
// Type: Data.ReadWrite.Conversion.COTLDataConversion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using WebSocketSharp;

#nullable disable
namespace Data.ReadWrite.Conversion;

public class COTLDataConversion
{
  public static IEnumerator ConvertFiles(System.Action andThen = null)
  {
    string path = Path.Combine(Application.persistentDataPath, "saves");
    MMJsonDataReadWriter<DataManager> saveFileReadWriterJSON = new MMJsonDataReadWriter<DataManager>();
    MMJsonDataReadWriter<MetaData> metaDataReadWriterJSON = new MMJsonDataReadWriter<MetaData>();
    MMJsonDataReadWriter<SettingsData> settingsDataReadWriterJSON = new MMJsonDataReadWriter<SettingsData>();
    MMXMLDataReadWriter<DataManager> saveFileReadWriterXML = new MMXMLDataReadWriter<DataManager>();
    MMXMLDataReadWriter<MetaData> metaDataReadWriterXML = new MMXMLDataReadWriter<MetaData>();
    MMXMLDataReadWriter<SettingsData> settingsDataReadWriterXML = new MMXMLDataReadWriter<SettingsData>();
    foreach (string file1 in Directory.GetFiles(path))
    {
      string file = file1;
      if (Path.GetExtension(file).Equals(".xml"))
      {
        bool canContinue = false;
        if (file.IndexOf("xml_") > -1)
        {
          string newFilename = file;
          newFilename = newFilename.Replace("xml_", "slot_");
          newFilename = newFilename.Replace(".xml", ".json");
          MMXMLDataReadWriter<DataManager> mmxmlDataReadWriter1 = saveFileReadWriterXML;
          mmxmlDataReadWriter1.OnReadCompleted = mmxmlDataReadWriter1.OnReadCompleted + (Action<DataManager>) (dataManager =>
          {
            COTLDataConversion.ConvertObjectiveIDs(dataManager);
            saveFileReadWriterJSON.Write(dataManager, newFilename, true, true);
          });
          MMJsonDataReadWriter<DataManager> jsonDataReadWriter = saveFileReadWriterJSON;
          jsonDataReadWriter.OnWriteCompleted = jsonDataReadWriter.OnWriteCompleted + (System.Action) (() => saveFileReadWriterXML.Delete(file));
          MMXMLDataReadWriter<DataManager> mmxmlDataReadWriter2 = saveFileReadWriterXML;
          mmxmlDataReadWriter2.OnDeletionComplete = mmxmlDataReadWriter2.OnDeletionComplete + (System.Action) (() => canContinue = true);
          saveFileReadWriterXML.Read(file);
        }
        else if (file.IndexOf("meta_") > -1)
        {
          string newFilename = file;
          newFilename = newFilename.Replace(".xml", ".json");
          MMXMLDataReadWriter<MetaData> mmxmlDataReadWriter3 = metaDataReadWriterXML;
          mmxmlDataReadWriter3.OnReadCompleted = mmxmlDataReadWriter3.OnReadCompleted + (Action<MetaData>) (metaData => metaDataReadWriterJSON.Write(metaData, newFilename, true, true));
          MMJsonDataReadWriter<MetaData> jsonDataReadWriter = metaDataReadWriterJSON;
          jsonDataReadWriter.OnWriteCompleted = jsonDataReadWriter.OnWriteCompleted + (System.Action) (() => metaDataReadWriterXML.Delete(file));
          MMXMLDataReadWriter<MetaData> mmxmlDataReadWriter4 = metaDataReadWriterXML;
          mmxmlDataReadWriter4.OnDeletionComplete = mmxmlDataReadWriter4.OnDeletionComplete + (System.Action) (() => canContinue = true);
          metaDataReadWriterXML.Read(file);
        }
        else if (file.IndexOf("settings") > -1)
        {
          string newFilename = file;
          newFilename = newFilename.Replace(".xml", ".json");
          MMXMLDataReadWriter<SettingsData> mmxmlDataReadWriter5 = settingsDataReadWriterXML;
          mmxmlDataReadWriter5.OnReadCompleted = mmxmlDataReadWriter5.OnReadCompleted + (Action<SettingsData>) (settings => settingsDataReadWriterJSON.Write(settings, newFilename, true, true));
          MMJsonDataReadWriter<SettingsData> jsonDataReadWriter = settingsDataReadWriterJSON;
          jsonDataReadWriter.OnWriteCompleted = jsonDataReadWriter.OnWriteCompleted + (System.Action) (() => settingsDataReadWriterXML.Delete(file));
          MMXMLDataReadWriter<SettingsData> mmxmlDataReadWriter6 = settingsDataReadWriterXML;
          mmxmlDataReadWriter6.OnDeletionComplete = mmxmlDataReadWriter6.OnDeletionComplete + (System.Action) (() => canContinue = true);
          settingsDataReadWriterXML.Read(file);
        }
        while (!canContinue)
          yield return (object) null;
        saveFileReadWriterJSON.OnWriteCompleted = (System.Action) null;
        saveFileReadWriterXML.OnReadCompleted = (Action<DataManager>) null;
        saveFileReadWriterXML.OnDeletionComplete = (System.Action) null;
        metaDataReadWriterJSON.OnWriteCompleted = (System.Action) null;
        metaDataReadWriterXML.OnReadCompleted = (Action<MetaData>) null;
        metaDataReadWriterXML.OnDeletionComplete = (System.Action) null;
        settingsDataReadWriterJSON.OnWriteCompleted = (System.Action) null;
        settingsDataReadWriterXML.OnReadCompleted = (Action<SettingsData>) null;
        settingsDataReadWriterXML.OnDeletionComplete = (System.Action) null;
      }
    }
    System.Action action = andThen;
    if (action != null)
      action();
  }

  public static void ConvertObjectiveIDs(DataManager dataManager)
  {
    Dictionary<string, string> dictionary = new Dictionary<string, string>();
    foreach (ObjectivesData objective in dataManager.Objectives)
    {
      if (objective.UniqueGroupID == null || objective.UniqueGroupID.IsNullOrEmpty())
      {
        if (dictionary.ContainsKey(objective.GroupId))
        {
          objective.UniqueGroupID = dictionary[objective.GroupId];
        }
        else
        {
          int stableHashCode = objective.GroupId.GetStableHashCode();
          objective.UniqueGroupID = stableHashCode.ToString();
          dictionary.Add(objective.GroupId, objective.UniqueGroupID);
        }
      }
    }
    foreach (ObjectivesData completedObjective in dataManager.CompletedObjectives)
    {
      if (completedObjective.UniqueGroupID == null || completedObjective.UniqueGroupID.IsNullOrEmpty())
      {
        if (dictionary.ContainsKey(completedObjective.GroupId))
        {
          completedObjective.UniqueGroupID = dictionary[completedObjective.GroupId];
        }
        else
        {
          int stableHashCode = completedObjective.GroupId.GetStableHashCode();
          completedObjective.UniqueGroupID = stableHashCode.ToString();
          dictionary.Add(completedObjective.GroupId, completedObjective.UniqueGroupID);
        }
      }
    }
    foreach (ObjectivesData failedObjective in dataManager.FailedObjectives)
    {
      if (failedObjective.UniqueGroupID == null || failedObjective.UniqueGroupID.IsNullOrEmpty())
      {
        if (dictionary.ContainsKey(failedObjective.GroupId))
        {
          failedObjective.UniqueGroupID = dictionary[failedObjective.GroupId];
        }
        else
        {
          int stableHashCode = failedObjective.GroupId.GetStableHashCode();
          failedObjective.UniqueGroupID = stableHashCode.ToString();
          dictionary.Add(failedObjective.GroupId, failedObjective.UniqueGroupID);
        }
      }
    }
    foreach (ObjectivesDataFinalized objectivesDataFinalized in dataManager.CompletedObjectivesHistory)
    {
      if (objectivesDataFinalized.UniqueGroupID == null || objectivesDataFinalized.UniqueGroupID.IsNullOrEmpty())
      {
        if (dictionary.ContainsKey(objectivesDataFinalized.GroupId))
        {
          objectivesDataFinalized.UniqueGroupID = dictionary[objectivesDataFinalized.GroupId];
        }
        else
        {
          int stableHashCode = objectivesDataFinalized.GroupId.GetStableHashCode();
          objectivesDataFinalized.UniqueGroupID = stableHashCode.ToString();
          dictionary.Add(objectivesDataFinalized.GroupId, objectivesDataFinalized.UniqueGroupID);
        }
      }
    }
    foreach (ObjectivesDataFinalized objectivesDataFinalized in dataManager.FailedObjectivesHistory)
    {
      if (objectivesDataFinalized.UniqueGroupID == null || objectivesDataFinalized.UniqueGroupID.IsNullOrEmpty())
      {
        if (dictionary.ContainsKey(objectivesDataFinalized.GroupId))
        {
          objectivesDataFinalized.UniqueGroupID = dictionary[objectivesDataFinalized.GroupId];
        }
        else
        {
          int stableHashCode = objectivesDataFinalized.GroupId.GetStableHashCode();
          objectivesDataFinalized.UniqueGroupID = stableHashCode.ToString();
          dictionary.Add(objectivesDataFinalized.GroupId, objectivesDataFinalized.UniqueGroupID);
        }
      }
    }
  }

  public static void UpgradeTierMismatchFix(DataManager dataManager)
  {
    if (dataManager.UnlockedUpgrades.Contains(UpgradeSystem.Type.Temple_III) && dataManager.UnlockedUpgrades.Contains(UpgradeSystem.Type.Temple_IV) && dataManager.CurrentUpgradeTreeTier == UpgradeTreeNode.TreeTier.Tier4)
      dataManager.CurrentUpgradeTreeTier = UpgradeTreeNode.TreeTier.Tier5;
    if (!dataManager.UnlockedUpgrades.Contains(UpgradeSystem.Type.Economy_Refinery) || dataManager.CurrentUpgradeTreeTier >= UpgradeTreeNode.TreeTier.Tier3)
      return;
    dataManager.CurrentUpgradeTreeTier = UpgradeTreeNode.TreeTier.Tier3;
  }
}
