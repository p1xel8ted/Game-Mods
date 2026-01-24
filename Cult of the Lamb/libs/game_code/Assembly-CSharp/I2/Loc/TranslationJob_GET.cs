// Decompiled with JetBrains decompiler
// Type: I2.Loc.TranslationJob_GET
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;

#nullable disable
namespace I2.Loc;

public class TranslationJob_GET : TranslationJob_WWW
{
  public Dictionary<string, TranslationQuery> _requests;
  public GoogleTranslation.fnOnTranslationReady _OnTranslationReady;
  public List<string> mQueries;
  public string mErrorMessage;

  public TranslationJob_GET(
    Dictionary<string, TranslationQuery> requests,
    GoogleTranslation.fnOnTranslationReady OnTranslationReady)
  {
    this._requests = requests;
    this._OnTranslationReady = OnTranslationReady;
    this.mQueries = GoogleTranslation.ConvertTranslationRequest(requests, true);
    int state = (int) this.GetState();
  }

  public void ExecuteNextQuery()
  {
    if (this.mQueries.Count == 0)
    {
      this.mJobState = TranslationJob.eJobState.Succeeded;
    }
    else
    {
      int index = this.mQueries.Count - 1;
      string mQuery = this.mQueries[index];
      this.mQueries.RemoveAt(index);
      this.www = UnityWebRequest.Get($"{LocalizationManager.GetWebServiceURL()}?action=Translate&list={mQuery}");
      I2Utils.SendWebRequest(this.www);
    }
  }

  public override TranslationJob.eJobState GetState()
  {
    if (this.www != null && this.www.isDone)
    {
      this.ProcessResult(this.www.downloadHandler.data, this.www.error);
      this.www.Dispose();
      this.www = (UnityWebRequest) null;
    }
    if (this.www == null)
      this.ExecuteNextQuery();
    return this.mJobState;
  }

  public void ProcessResult(byte[] bytes, string errorMsg)
  {
    if (string.IsNullOrEmpty(errorMsg))
    {
      errorMsg = GoogleTranslation.ParseTranslationResult(Encoding.UTF8.GetString(bytes, 0, bytes.Length), this._requests);
      if (string.IsNullOrEmpty(errorMsg))
      {
        if (this._OnTranslationReady == null)
          return;
        this._OnTranslationReady(this._requests, (string) null);
        return;
      }
    }
    this.mJobState = TranslationJob.eJobState.Failed;
    this.mErrorMessage = errorMsg;
  }
}
