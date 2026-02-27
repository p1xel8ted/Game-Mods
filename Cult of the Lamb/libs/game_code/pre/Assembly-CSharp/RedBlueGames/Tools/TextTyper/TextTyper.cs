// Decompiled with JetBrains decompiler
// Type: RedBlueGames.Tools.TextTyper.TextTyper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private const float PrintDelaySetting = 0.015f;
  private const float PunctuationDelayMultiplier = 8f;
  private static readonly List<char> punctutationCharacters = new List<char>()
  {
    '.',
    ',',
    '!',
    '?'
  };
  [SerializeField]
  [Tooltip("The library of ShakePreset animations that can be used by this component.")]
  private ShakeLibrary shakeLibrary;
  [SerializeField]
  [Tooltip("The library of CurvePreset animations that can be used by this component.")]
  private CurveLibrary curveLibrary;
  [SerializeField]
  [Tooltip("Event that's called when the text has finished printing.")]
  private UnityEvent printCompleted = new UnityEvent();
  [SerializeField]
  [Tooltip("Event called when a character is printed. Inteded for audio callbacks.")]
  private RedBlueGames.Tools.TextTyper.TextTyper.CharacterPrintedEvent characterPrinted = new RedBlueGames.Tools.TextTyper.TextTyper.CharacterPrintedEvent();
  private TextMeshProUGUI textComponent;
  private float defaultPrintDelay;
  private List<float> characterPrintDelays;
  private List<TextAnimation> animations;
  private Coroutine typeTextCoroutine;
  private byte[] Alphas = new byte[500];
  private TMP_TextInfo textInfo;
  private TMP_CharacterInfo charInfo;
  private int characterCount;
  private int vertexIndex;
  private int materialIndex;
  private Color32[] newVertexColors;
  [Range(0.01f, 1f)]
  private float AlphaSpeed = 0.25f;

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
    this.typeTextCoroutine = this.StartCoroutine((IEnumerator) this.TypeTextCharByChar(text));
  }

  public void Skip()
  {
    this.CleanupCoroutine();
    this.TextComponent.maxVisibleCharacters = int.MaxValue;
    this.UpdateMeshAndAnims();
    this.OnTypewritingComplete();
  }

  public bool IsSkippable() => this.IsTyping;

  private void CleanupCoroutine()
  {
    if (this.typeTextCoroutine == null)
      return;
    this.StopCoroutine(this.typeTextCoroutine);
    this.typeTextCoroutine = (Coroutine) null;
  }

  private IEnumerator TypeTextCharByChar(string text)
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

  private void ResetAlphas()
  {
    int index = -1;
    while (++index < 500)
      this.Alphas[index] = (byte) 0;
    this.textInfo = this.TextComponent.textInfo;
  }

  private void LateUpdate()
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

  private void UpdateMeshAndAnims()
  {
    this.TextComponent.ForceMeshUpdate(false, false);
    for (int index = 0; index < this.animations.Count; ++index)
      this.animations[index].AnimateAllChars();
  }

  private void ProcessCustomTags(string text)
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

  private bool IsAnimationShake(string animName) => this.shakeLibrary.ContainsKey(animName);

  private bool IsAnimationCurve(string animName) => this.curveLibrary.ContainsKey(animName);

  private void OnCharacterPrinted(string printedCharacter)
  {
    if (this.CharacterPrinted == null)
      return;
    this.CharacterPrinted.Invoke(printedCharacter);
  }

  private void OnTypewritingComplete()
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
