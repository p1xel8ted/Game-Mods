// Decompiled with JetBrains decompiler
// Type: RedBlueGames.Tools.TextTyper.TextTyperTester
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace RedBlueGames.Tools.TextTyper;

public class TextTyperTester : MonoBehaviour
{
  [SerializeField]
  public AudioClip printSoundEffect;
  [Header("UI References")]
  [SerializeField]
  public Button printNextButton;
  [SerializeField]
  public Button printNoSkipButton;
  public Queue<string> dialogueLines = new Queue<string>();
  [SerializeField]
  [Tooltip("The text typer element to test typing with")]
  public RedBlueGames.Tools.TextTyper.TextTyper testTextTyper;

  public void Start()
  {
    this.testTextTyper.PrintCompleted.AddListener(new UnityAction(this.HandlePrintCompleted));
    this.testTextTyper.CharacterPrinted.AddListener(new UnityAction<string>(this.HandleCharacterPrinted));
    this.dialogueLines.Enqueue("Hello! <animation=bounce>My name</animation> is... <delay=0.5>NPC</delay>. Got it, <i>bub</i>?");
    this.dialogueLines.Enqueue("You can <b>use</b> <i>uGUI</i> <size=40>text</size> <size=20>tag</size> and <color=#ff0000ff>color</color> tag <color=#00ff00ff>like this</color>.");
    this.dialogueLines.Enqueue("bold <b>text</b> test <b>bold</b> text <b>test</b>");
    this.dialogueLines.Enqueue("You can <size=40>size 40</size> <animation=crazyflip>and</animation> <size=20>size 20</size>");
    this.dialogueLines.Enqueue("You can <color=#ff0000ff>color</color> tag <color=#00ff00ff>like this</color>.");
    this.dialogueLines.Enqueue("Sample Shake Animations: <anim=lightrot>Light Rotation</anim>, <anim=lightpos>Light Position</anim>, <anim=fullshake>Full Shake</anim>\nSample Curve Animations: <animation=slowsine>Slow Sine</animation>, <animation=bounce>Bounce</animation>, <animation=crazyflip>Crazy Flip</animation>");
    this.ShowScript();
  }

  public void Update()
  {
    if (Input.GetMouseButtonDown(0))
      this.HandlePrintNextClicked();
    if (!Input.GetKeyDown(KeyCode.Space))
      return;
    this.LogTag(RichTextTag.ParseNext("blah<color=red>boo</color"));
    this.LogTag(RichTextTag.ParseNext("<color=blue>blue</color"));
    this.LogTag(RichTextTag.ParseNext("No tag in here"));
    this.LogTag(RichTextTag.ParseNext("No <color=blueblue</color tag here either"));
    this.LogTag(RichTextTag.ParseNext("This tag is a closing tag </bold>"));
  }

  public void HandlePrintNextClicked()
  {
    if (this.testTextTyper.IsSkippable() && this.testTextTyper.IsTyping)
      this.testTextTyper.Skip();
    else
      this.ShowScript();
  }

  public void HandlePrintNoSkipClicked() => this.ShowScript();

  public void ShowScript()
  {
    if (this.dialogueLines.Count <= 0)
      return;
    this.testTextTyper.TypeText(this.dialogueLines.Dequeue());
  }

  public void LogTag(RichTextTag tag)
  {
    if (tag == null)
      return;
    Debug.Log((object) ("Tag: " + tag.ToString()));
  }

  public void HandleCharacterPrinted(string printedCharacter)
  {
    switch (printedCharacter)
    {
      case " ":
        break;
      case "\n":
        break;
      default:
        AudioSource audioSource = this.GetComponent<AudioSource>();
        if ((Object) audioSource == (Object) null)
          audioSource = this.gameObject.AddComponent<AudioSource>();
        audioSource.clip = this.printSoundEffect;
        audioSource.Play();
        break;
    }
  }

  public void HandlePrintCompleted() => Debug.Log((object) "TypeText Complete");
}
