// Decompiled with JetBrains decompiler
// Type: I2.Loc.RealTimeTranslation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace I2.Loc;

public class RealTimeTranslation : MonoBehaviour
{
  public string OriginalText = "This is an example showing how to use the google translator to translate chat messages within the game.\nIt also supports multiline translations.";
  public string TranslatedText = string.Empty;
  public bool IsTranslating;

  public void OnGUI()
  {
    GUILayout.Label("Translate:");
    this.OriginalText = GUILayout.TextArea(this.OriginalText, GUILayout.Width((float) Screen.width));
    GUILayout.Space(10f);
    GUILayout.BeginHorizontal();
    if (GUILayout.Button("English -> Español", GUILayout.Height(100f)))
      this.StartTranslating("en", "es");
    if (GUILayout.Button("Español -> English", GUILayout.Height(100f)))
      this.StartTranslating("es", "en");
    GUILayout.EndHorizontal();
    GUILayout.Space(10f);
    GUILayout.BeginHorizontal();
    GUILayout.TextArea("Multiple Translation with 1 call:\n'This is an example' -> en,zh\n'Hola' -> en");
    if (GUILayout.Button("Multi Translate", GUILayout.ExpandHeight(true)))
      this.ExampleMultiTranslations_Async();
    GUILayout.EndHorizontal();
    GUILayout.TextArea(this.TranslatedText, GUILayout.Width((float) Screen.width));
    GUILayout.Space(10f);
    if (!this.IsTranslating)
      return;
    GUILayout.Label("Contacting Google....");
  }

  public void StartTranslating(string fromCode, string toCode)
  {
    this.IsTranslating = true;
    GoogleTranslation.Translate(this.OriginalText, fromCode, toCode, new GoogleTranslation.fnOnTranslated(this.OnTranslationReady));
  }

  public void OnTranslationReady(string Translation, string errorMsg)
  {
    this.IsTranslating = false;
    if (errorMsg != null)
      Debug.LogError((object) errorMsg);
    else
      this.TranslatedText = Translation;
  }

  public void ExampleMultiTranslations_Blocking()
  {
    Dictionary<string, TranslationQuery> dictionary = new Dictionary<string, TranslationQuery>();
    GoogleTranslation.AddQuery("This is an example", "en", "es", dictionary);
    GoogleTranslation.AddQuery("This is an example", "auto", "zh", dictionary);
    GoogleTranslation.AddQuery("Hola", "es", "en", dictionary);
    if (!GoogleTranslation.ForceTranslate(dictionary))
      return;
    Debug.Log((object) GoogleTranslation.GetQueryResult("This is an example", "en", dictionary));
    Debug.Log((object) GoogleTranslation.GetQueryResult("This is an example", "zh", dictionary));
    Debug.Log((object) GoogleTranslation.GetQueryResult("This is an example", "", dictionary));
    Debug.Log((object) dictionary["Hola"].Results[0]);
  }

  public void ExampleMultiTranslations_Async()
  {
    this.IsTranslating = true;
    Dictionary<string, TranslationQuery> dictionary = new Dictionary<string, TranslationQuery>();
    GoogleTranslation.AddQuery("This is an example", "en", "es", dictionary);
    GoogleTranslation.AddQuery("This is an example", "auto", "zh", dictionary);
    GoogleTranslation.AddQuery("Hola", "es", "en", dictionary);
    GoogleTranslation.Translate(dictionary, new GoogleTranslation.fnOnTranslationReady(this.OnMultitranslationReady));
  }

  public void OnMultitranslationReady(Dictionary<string, TranslationQuery> dict, string errorMsg)
  {
    if (!string.IsNullOrEmpty(errorMsg))
    {
      Debug.LogError((object) errorMsg);
    }
    else
    {
      this.IsTranslating = false;
      this.TranslatedText = "";
      this.TranslatedText = $"{this.TranslatedText}{GoogleTranslation.GetQueryResult("This is an example", "es", dict)}\n";
      this.TranslatedText = $"{this.TranslatedText}{GoogleTranslation.GetQueryResult("This is an example", "zh", dict)}\n";
      this.TranslatedText = $"{this.TranslatedText}{GoogleTranslation.GetQueryResult("This is an example", "", dict)}\n";
      this.TranslatedText += dict["Hola"].Results[0];
    }
  }

  public bool IsWaitingForTranslation() => this.IsTranslating;

  public string GetTranslatedText() => this.TranslatedText;

  public void SetOriginalText(string text) => this.OriginalText = text;
}
