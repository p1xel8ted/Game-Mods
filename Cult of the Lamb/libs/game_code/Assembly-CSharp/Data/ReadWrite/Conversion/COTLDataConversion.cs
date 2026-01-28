// Decompiled with JetBrains decompiler
// Type: Data.ReadWrite.Conversion.COTLDataConversion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
    // ISSUE: variable of a compiler-generated type
    COTLDataConversion.\u003C\u003Ec__DisplayClass1_0 cDisplayClass10;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass10.idConversion = new Dictionary<string, string>();
    COTLDataConversion.\u003CConvertObjectiveIDs\u003Eg__SanitizeAndProcessObjectivesData\u007C1_0(dataManager.Objectives, ref cDisplayClass10);
    COTLDataConversion.\u003CConvertObjectiveIDs\u003Eg__SanitizeAndProcessObjectivesData\u007C1_0(dataManager.CompletedObjectives, ref cDisplayClass10);
    COTLDataConversion.\u003CConvertObjectiveIDs\u003Eg__SanitizeAndProcessObjectivesData\u007C1_0(dataManager.FailedObjectives, ref cDisplayClass10);
    COTLDataConversion.\u003CConvertObjectiveIDs\u003Eg__SanitizeAndProcessObjectivesDataFinalized\u007C1_1(dataManager.CompletedObjectivesHistory, ref cDisplayClass10);
    COTLDataConversion.\u003CConvertObjectiveIDs\u003Eg__SanitizeAndProcessObjectivesDataFinalized\u007C1_1(dataManager.FailedObjectivesHistory, ref cDisplayClass10);
  }

  public static void UpgradeTierMismatchFix(DataManager dataManager)
  {
    if (dataManager.UnlockedUpgrades.Contains(UpgradeSystem.Type.Temple_III) && dataManager.UnlockedUpgrades.Contains(UpgradeSystem.Type.Temple_IV) && dataManager.CurrentUpgradeTreeTier == UpgradeTreeNode.TreeTier.Tier4)
      dataManager.CurrentUpgradeTreeTier = UpgradeTreeNode.TreeTier.Tier5;
    if (!dataManager.UnlockedUpgrades.Contains(UpgradeSystem.Type.Economy_Refinery) || dataManager.CurrentUpgradeTreeTier >= UpgradeTreeNode.TreeTier.Tier3)
      return;
    dataManager.CurrentUpgradeTreeTier = UpgradeTreeNode.TreeTier.Tier3;
  }

  [CompilerGenerated]
  public static void \u003CConvertObjectiveIDs\u003Eg__SanitizeAndProcessObjectivesData\u007C1_0(
    List<ObjectivesData> objectives,
    [In] ref COTLDataConversion.\u003C\u003Ec__DisplayClass1_0 obj1)
  {
    if (objectives == null)
      return;
    objectives.RemoveAll((Predicate<ObjectivesData>) (x => x == null));
    foreach (ObjectivesData objective in objectives)
    {
      if ((objective.UniqueGroupID == null || objective.UniqueGroupID.IsNullOrEmpty()) && !string.IsNullOrEmpty(objective.GroupId))
      {
        // ISSUE: reference to a compiler-generated field
        if (obj1.idConversion.ContainsKey(objective.GroupId))
        {
          // ISSUE: reference to a compiler-generated field
          objective.UniqueGroupID = obj1.idConversion[objective.GroupId];
        }
        else
        {
          int stableHashCode = objective.GroupId.GetStableHashCode();
          objective.UniqueGroupID = stableHashCode.ToString();
          // ISSUE: reference to a compiler-generated field
          obj1.idConversion.Add(objective.GroupId, objective.UniqueGroupID);
        }
      }
    }
  }

  [CompilerGenerated]
  public static void \u003CConvertObjectiveIDs\u003Eg__SanitizeAndProcessObjectivesDataFinalized\u007C1_1(
    List<ObjectivesDataFinalized> objectives,
    [In] ref COTLDataConversion.\u003C\u003Ec__DisplayClass1_0 obj1)
  {
    if (objectives == null)
      return;
    objectives.RemoveAll((Predicate<ObjectivesDataFinalized>) (x => x == null));
    foreach (ObjectivesDataFinalized objective in objectives)
    {
      if ((objective.UniqueGroupID == null || objective.UniqueGroupID.IsNullOrEmpty()) && !string.IsNullOrEmpty(objective.GroupId))
      {
        // ISSUE: reference to a compiler-generated field
        if (obj1.idConversion.ContainsKey(objective.GroupId))
        {
          // ISSUE: reference to a compiler-generated field
          objective.UniqueGroupID = obj1.idConversion[objective.GroupId];
        }
        else
        {
          int stableHashCode = objective.GroupId.GetStableHashCode();
          objective.UniqueGroupID = stableHashCode.ToString();
          // ISSUE: reference to a compiler-generated field
          obj1.idConversion.Add(objective.GroupId, objective.UniqueGroupID);
        }
      }
    }
  }
}
