// Decompiled with JetBrains decompiler
// Type: I2.Loc.TranslationJob_POST
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

#nullable disable
namespace I2.Loc;

public class TranslationJob_POST : TranslationJob_WWW
{
  private Dictionary<string, TranslationQuery> _requests;
  private GoogleTranslation.fnOnTranslationReady _OnTranslationReady;

  public TranslationJob_POST(
    Dictionary<string, TranslationQuery> requests,
    GoogleTranslation.fnOnTranslationReady OnTranslationReady)
  {
    this._requests = requests;
    this._OnTranslationReady = OnTranslationReady;
    List<string> stringList = GoogleTranslation.ConvertTranslationRequest(requests, false);
    WWWForm formData = new WWWForm();
    formData.AddField("action", "Translate");
    formData.AddField("list", stringList[0]);
    this.www = UnityWebRequest.Post(LocalizationManager.GetWebServiceURL(), formData);
    I2Utils.SendWebRequest(this.www);
  }

  public override TranslationJob.eJobState GetState()
  {
    if (this.www != null && this.www.isDone)
    {
      this.ProcessResult(this.www.downloadHandler.data, this.www.error);
      this.www.Dispose();
      this.www = (UnityWebRequest) null;
    }
    return this.mJobState;
  }

  public void ProcessResult(byte[] bytes, string errorMsg)
  {
    if (!string.IsNullOrEmpty(errorMsg))
    {
      this.mJobState = TranslationJob.eJobState.Failed;
    }
    else
    {
      errorMsg = GoogleTranslation.ParseTranslationResult(Encoding.UTF8.GetString(bytes, 0, bytes.Length), this._requests);
      if (this._OnTranslationReady != null)
        this._OnTranslationReady(this._requests, errorMsg);
      this.mJobState = TranslationJob.eJobState.Succeeded;
    }
  }
}
