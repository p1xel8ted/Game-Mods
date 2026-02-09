// Decompiled with JetBrains decompiler
// Type: I2.Loc.LanguageSource
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace I2.Loc;

[AddComponentMenu("I2/Localization/Source")]
[ExecuteInEditMode]
public class LanguageSource : MonoBehaviour, ISerializationCallbackReceiver, ILanguageSource
{
  public LanguageSourceData mSource = new LanguageSourceData();
  public int version;
  public bool NeverDestroy;
  public bool UserAgreesToHaveItOnTheScene;
  public bool UserAgreesToHaveItInsideThePluginsFolder;
  public bool GoogleLiveSyncIsUptoDate = true;
  public List<Object> Assets = new List<Object>();
  public string Google_WebServiceURL;
  public string Google_SpreadsheetKey;
  public string Google_SpreadsheetName;
  public string Google_LastUpdatedVersion;
  public LanguageSourceData.eGoogleUpdateFrequency GoogleUpdateFrequency = LanguageSourceData.eGoogleUpdateFrequency.Weekly;
  public float GoogleUpdateDelay = 5f;
  public List<LanguageData> mLanguages = new List<LanguageData>();
  public bool IgnoreDeviceLanguage;
  public LanguageSourceData.eAllowUnloadLanguages _AllowUnloadingLanguages;
  public List<TermData> mTerms = new List<TermData>();
  public bool CaseInsensitiveTerms;
  public LanguageSourceData.MissingTranslationAction OnMissingTranslation = LanguageSourceData.MissingTranslationAction.Fallback;
  public string mTerm_AppName;

  public LanguageSourceData SourceData
  {
    get => this.mSource;
    set => this.mSource = value;
  }

  public event LanguageSource.fnOnSourceUpdated Event_OnSourceUpdateFromGoogle;

  public void Awake()
  {
    this.mSource.owner = (ILanguageSource) this;
    this.mSource.Awake();
  }

  public void OnDestroy()
  {
    this.NeverDestroy = false;
    if (this.NeverDestroy)
      return;
    this.mSource.OnDestroy();
  }

  public string GetSourceName()
  {
    string sourceName = this.gameObject.name;
    for (Transform parent = this.transform.parent; (bool) (Object) parent; parent = parent.parent)
      sourceName = $"{parent.name}_{sourceName}";
    return sourceName;
  }

  public void OnBeforeSerialize() => this.version = 1;

  public void OnAfterDeserialize()
  {
    if (this.version != 0 && this.mSource != null)
      return;
    this.mSource = new LanguageSourceData();
    this.mSource.owner = (ILanguageSource) this;
    this.mSource.UserAgreesToHaveItOnTheScene = this.UserAgreesToHaveItOnTheScene;
    this.mSource.UserAgreesToHaveItInsideThePluginsFolder = this.UserAgreesToHaveItInsideThePluginsFolder;
    this.mSource.IgnoreDeviceLanguage = this.IgnoreDeviceLanguage;
    this.mSource._AllowUnloadingLanguages = this._AllowUnloadingLanguages;
    this.mSource.CaseInsensitiveTerms = this.CaseInsensitiveTerms;
    this.mSource.OnMissingTranslation = this.OnMissingTranslation;
    this.mSource.mTerm_AppName = this.mTerm_AppName;
    this.mSource.GoogleLiveSyncIsUptoDate = this.GoogleLiveSyncIsUptoDate;
    this.mSource.Google_WebServiceURL = this.Google_WebServiceURL;
    this.mSource.Google_SpreadsheetKey = this.Google_SpreadsheetKey;
    this.mSource.Google_SpreadsheetName = this.Google_SpreadsheetName;
    this.mSource.Google_LastUpdatedVersion = this.Google_LastUpdatedVersion;
    this.mSource.GoogleUpdateFrequency = this.GoogleUpdateFrequency;
    this.mSource.GoogleUpdateDelay = this.GoogleUpdateDelay;
    this.mSource.Event_OnSourceUpdateFromGoogle += this.Event_OnSourceUpdateFromGoogle;
    if (this.mLanguages != null && this.mLanguages.Count > 0)
    {
      this.mSource.mLanguages.Clear();
      this.mSource.mLanguages.AddRange((IEnumerable<LanguageData>) this.mLanguages);
      this.mLanguages.Clear();
    }
    if (this.Assets != null && this.Assets.Count > 0)
    {
      this.mSource.Assets.Clear();
      this.mSource.Assets.AddRange((IEnumerable<Object>) this.Assets);
      this.Assets.Clear();
    }
    if (this.mTerms != null && this.mTerms.Count > 0)
    {
      this.mSource.mTerms.Clear();
      for (int index = 0; index < this.mTerms.Count; ++index)
        this.mSource.mTerms.Add(this.mTerms[index]);
      this.mTerms.Clear();
    }
    this.version = 1;
    this.Event_OnSourceUpdateFromGoogle = (LanguageSource.fnOnSourceUpdated) null;
  }

  public delegate void fnOnSourceUpdated(
    LanguageSourceData source,
    bool ReceivedNewData,
    string errorMsg);
}
