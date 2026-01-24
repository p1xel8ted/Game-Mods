// Decompiled with JetBrains decompiler
// Type: I2.Loc.TranslationJob_Main
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace I2.Loc;

public class TranslationJob_Main : TranslationJob
{
  public TranslationJob_WEB mWeb;
  public TranslationJob_POST mPost;
  public TranslationJob_GET mGet;
  public Dictionary<string, TranslationQuery> _requests;
  public GoogleTranslation.fnOnTranslationReady _OnTranslationReady;
  public string mErrorMessage;

  public TranslationJob_Main(
    Dictionary<string, TranslationQuery> requests,
    GoogleTranslation.fnOnTranslationReady OnTranslationReady)
  {
    this._requests = requests;
    this._OnTranslationReady = OnTranslationReady;
    this.mPost = new TranslationJob_POST(requests, OnTranslationReady);
  }

  public override TranslationJob.eJobState GetState()
  {
    if (this.mWeb != null)
    {
      switch (this.mWeb.GetState())
      {
        case TranslationJob.eJobState.Running:
          return TranslationJob.eJobState.Running;
        case TranslationJob.eJobState.Succeeded:
          this.mJobState = TranslationJob.eJobState.Succeeded;
          break;
        case TranslationJob.eJobState.Failed:
          this.mWeb.Dispose();
          this.mWeb = (TranslationJob_WEB) null;
          this.mPost = new TranslationJob_POST(this._requests, this._OnTranslationReady);
          break;
      }
    }
    if (this.mPost != null)
    {
      switch (this.mPost.GetState())
      {
        case TranslationJob.eJobState.Running:
          return TranslationJob.eJobState.Running;
        case TranslationJob.eJobState.Succeeded:
          this.mJobState = TranslationJob.eJobState.Succeeded;
          break;
        case TranslationJob.eJobState.Failed:
          this.mPost.Dispose();
          this.mPost = (TranslationJob_POST) null;
          this.mGet = new TranslationJob_GET(this._requests, this._OnTranslationReady);
          break;
      }
    }
    if (this.mGet != null)
    {
      switch (this.mGet.GetState())
      {
        case TranslationJob.eJobState.Running:
          return TranslationJob.eJobState.Running;
        case TranslationJob.eJobState.Succeeded:
          this.mJobState = TranslationJob.eJobState.Succeeded;
          break;
        case TranslationJob.eJobState.Failed:
          this.mErrorMessage = this.mGet.mErrorMessage;
          if (this._OnTranslationReady != null)
            this._OnTranslationReady(this._requests, this.mErrorMessage);
          this.mGet.Dispose();
          this.mGet = (TranslationJob_GET) null;
          break;
      }
    }
    return this.mJobState;
  }

  public override void Dispose()
  {
    if (this.mPost != null)
      this.mPost.Dispose();
    if (this.mGet != null)
      this.mGet.Dispose();
    this.mPost = (TranslationJob_POST) null;
    this.mGet = (TranslationJob_GET) null;
  }
}
