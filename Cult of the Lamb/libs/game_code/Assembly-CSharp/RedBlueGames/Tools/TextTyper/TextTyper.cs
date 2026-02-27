// Decompiled with JetBrains decompiler
// Type: RedBlueGames.Tools.TextTyper.TextTyper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace RedBlueGames.Tools.TextTyper;

[RequireComponent(typeof (TextMeshProUGUI))]
public class TextTyper : MonoBehaviour
{
  public const float PrintDelaySetting = 0.015f;
  public const float PunctuationDelayMultiplier = 8f;
  public static List<char> punctutationCharacters = new List<char>()
  {
    '.',
    ',',
    '!',
    '?'
  };
  [SerializeField]
  [Tooltip("The library of ShakePreset animations that can be used by this component.")]
  public ShakeLibrary shakeLibrary;
  [SerializeField]
  [Tooltip("The library of CurvePreset animations that can be used by this component.")]
  public CurveLibrary curveLibrary;
  [SerializeField]
  [Tooltip("Event that's called when the text has finished printing.")]
  public UnityEvent printCompleted = new UnityEvent();
  [SerializeField]
  [Tooltip("Event called when a character is printed. Inteded for audio callbacks.")]
  public RedBlueGames.Tools.TextTyper.TextTyper.CharacterPrintedEvent characterPrinted = new RedBlueGames.Tools.TextTyper.TextTyper.CharacterPrintedEvent();
  public TextMeshProUGUI textComponent;
  public float defaultPrintDelay;
  public List<float> characterPrintDelays;
  public List<TextAnimation> animations;
  public Coroutine typeTextCoroutine;
  public byte[] Alphas = new byte[500];
  public TMP_TextInfo textInfo;
  public TMP_CharacterInfo charInfo;
  public int characterCount;
  public int vertexIndex;
  public int materialIndex;
  public Color32[] newVertexColors;
  [Range(0.01f, 1f)]
  public float AlphaSpeed = 0.25f;

  public UnityEvent PrintCompleted => this.printCompleted;

  public RedBlueGames.Tools.TextTyper.TextTyper.CharacterPrintedEvent CharacterPrinted
  {
    get => this.characterPrinted;
  }

  public bool IsTyping => this.typeTextCoroutine != null;

  public TextMeshProUGUI TextComponent
  {
    get
    {
      if ((UnityEngine.Object) this.textComponent == (UnityEngine.Object) null)
        this.textComponent = this.GetComponent<TextMeshProUGUI>();
      return this.textComponent;
    }
  }

  public TextMeshProUGUI GetTextComponent() => this.TextComponent;

  public void TypeText(string text, float printDelay = -1f)
  {
    this.CleanupCoroutine();
    foreach (UnityEngine.Object component in this.GetComponents<TextAnimation>())
      UnityEngine.Object.Destroy(component);
    this.defaultPrintDelay = (double) printDelay > 0.0 ? printDelay : 0.015f;
    this.ProcessCustomTags(text);
    this.typeTextCoroutine = this.StartCoroutine(this.TypeTextCharByChar(text));
  }

  public void Skip()
  {
    this.CleanupCoroutine();
    this.TextComponent.maxVisibleCharacters = int.MaxValue;
    this.UpdateMeshAndAnims();
    this.OnTypewritingComplete();
  }

  public bool IsSkippable() => this.IsTyping;

  public void CleanupCoroutine()
  {
    if (this.typeTextCoroutine == null)
      return;
    this.StopCoroutine(this.typeTextCoroutine);
    this.typeTextCoroutine = (Coroutine) null;
  }

  public IEnumerator TypeTextCharByChar(string text)
  {
    string taglessText = TextTagParser.RemoveAllTags(text);
    int totalPrintedChars = taglessText.Length;
    int currPrintedChars = 1;
    this.TextComponent.text = TextTagParser.RemoveCustomTags(text);
    this.ResetAlphas();
    do
    {
      this.TextComponent.maxVisibleCharacters = currPrintedChars;
      this.UpdateMeshAndAnims();
      this.OnCharacterPrinted(taglessText[currPrintedChars - 1].ToString());
      yield return (object) new WaitForSeconds(this.characterPrintDelays[currPrintedChars - 1]);
      ++currPrintedChars;
    }
    while (currPrintedChars <= totalPrintedChars);
    this.typeTextCoroutine = (Coroutine) null;
    this.OnTypewritingComplete();
  }

  public void ResetAlphas()
  {
    int index = -1;
    while (++index < 500)
      this.Alphas[index] = (byte) 0;
    this.textInfo = this.TextComponent.textInfo;
  }

  public void LateUpdate()
  {
    this.textInfo = this.TextComponent.textInfo;
    this.characterCount = this.textInfo.characterCount;
    for (int index = 0; index < this.characterCount; ++index)
    {
      if (this.textInfo.characterInfo[index].isVisible)
      {
        this.charInfo = this.textInfo.characterInfo[index];
        this.vertexIndex = this.charInfo.vertexIndex;
        this.materialIndex = this.charInfo.materialReferenceIndex;
        this.newVertexColors = this.textInfo.meshInfo[this.materialIndex].colors32;
        this.Alphas[index] += (byte) ((double) ((int) byte.MaxValue - (int) this.Alphas[index]) * (double) this.AlphaSpeed);
        this.newVertexColors[this.vertexIndex].a = this.Alphas[index];
        this.newVertexColors[this.vertexIndex + 1].a = this.Alphas[index];
        this.newVertexColors[this.vertexIndex + 2].a = this.Alphas[index];
        this.newVertexColors[this.vertexIndex + 3].a = this.Alphas[index];
      }
    }
    this.TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
  }

  public void UpdateMeshAndAnims()
  {
    this.TextComponent.ForceMeshUpdate(false, false);
    for (int index = 0; index < this.animations.Count; ++index)
      this.animations[index].AnimateAllChars();
  }

  public void ProcessCustomTags(string text)
  {
    this.characterPrintDelays = new List<float>(text.Length);
    this.animations = new List<TextAnimation>();
    List<TextTagParser.TextSymbol> symbolListFromText = TextTagParser.CreateSymbolListFromText(text);
    int num1 = 0;
    int firstChar = 0;
    string str = "";
    float num2 = this.defaultPrintDelay;
    foreach (TextTagParser.TextSymbol textSymbol in symbolListFromText)
    {
      if (textSymbol.IsTag)
      {
        if (textSymbol.Tag.TagType == "delay")
          num2 = !textSymbol.Tag.IsClosingTag ? textSymbol.GetFloatParameter(this.defaultPrintDelay) : this.defaultPrintDelay;
        else if (textSymbol.Tag.TagType == "anim" || textSymbol.Tag.TagType == "animation")
        {
          if (textSymbol.Tag.IsClosingTag)
          {
            TextAnimation textAnimation = (TextAnimation) null;
            if (this.IsAnimationShake(str))
            {
              textAnimation = (TextAnimation) this.gameObject.AddComponent<ShakeAnimation>();
              ((ShakeAnimation) textAnimation).LoadPreset(this.shakeLibrary, str);
            }
            else if (this.IsAnimationCurve(str))
            {
              textAnimation = (TextAnimation) this.gameObject.AddComponent<CurveAnimation>();
              ((CurveAnimation) textAnimation).LoadPreset(this.curveLibrary, str);
            }
            textAnimation.SetCharsToAnimate(firstChar, num1 - 1);
            textAnimation.enabled = true;
            this.animations.Add(textAnimation);
          }
          else
          {
            firstChar = num1;
            str = textSymbol.Tag.Parameter;
          }
        }
      }
      else
      {
        ++num1;
        if (RedBlueGames.Tools.TextTyper.TextTyper.punctutationCharacters.Contains(textSymbol.Character))
          this.characterPrintDelays.Add(num2 * 8f);
        else
          this.characterPrintDelays.Add(num2);
      }
    }
  }

  public bool IsAnimationShake(string animName) => this.shakeLibrary.ContainsKey(animName);

  public bool IsAnimationCurve(string animName) => this.curveLibrary.ContainsKey(animName);

  public void OnCharacterPrinted(string printedCharacter)
  {
    if (this.CharacterPrinted == null)
      return;
    this.CharacterPrinted.Invoke(printedCharacter);
  }

  public void OnTypewritingComplete()
  {
    if (this.PrintCompleted == null)
      return;
    this.PrintCompleted.Invoke();
  }

  [Serializable]
  public class CharacterPrintedEvent : UnityEvent<string>
  {
  }
}
