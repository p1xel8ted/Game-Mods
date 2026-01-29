// Decompiled with JetBrains decompiler
// Type: PlayManualConvo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using MMTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class PlayManualConvo : MonoBehaviour
{
  public string characterName;
  public Color characterNameColor = StaticColors.RedColor;
  public List<float> timerList = new List<float>();
  public List<string> convList = new List<string>();
  public MMConversation mmC;
  public float scale = 1.5f;
  public bool useTimer;
  public float timer;
  public bool useFadeOut;
  public float fadeOutDuration = 0.5f;
  public int count;

  public void Play()
  {
    Cursor.visible = false;
    List<ConversationEntry> Entries = new List<ConversationEntry>();
    foreach (string conv in this.convList)
      Entries.Add(new ConversationEntry(this.gameObject, conv));
    foreach (ConversationEntry conversationEntry in Entries)
      conversationEntry.CharacterName = $"<sprite name=\"img_SwirleyLeft\"> {this.characterName} <sprite name=\"img_SwirleyRight\">";
    MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) null), false);
    this.mmC = MMConversation.mmConversation;
    this.mmC.TitleText.color = this.characterNameColor;
    this.mmC.transform.Find("Speech Bubble").localScale = new Vector3(this.scale, this.scale, this.scale);
    Transform transform = this.mmC.transform.Find("Speech Bubble/BG/Text");
    this.mmC.transform.Find("Speech Bubble/BG/Next Arrow Container").gameObject.SetActive(false);
    transform.GetComponent<RectTransform>().sizeDelta = new Vector2(900f, 100f);
    if (!this.useTimer)
      return;
    this.StartCoroutine((IEnumerator) this.TimerStop());
  }

  public IEnumerator TimerStop()
  {
    yield return (object) new WaitForSeconds(this.timerList[this.count]);
    this.Stop();
  }

  public void NextLine()
  {
    ++this.count;
    this.mmC.TextPlayer.ShowText(this.convList[this.count]);
    if (!this.useTimer)
      return;
    this.StartCoroutine((IEnumerator) this.TimerStop());
  }

  public void Stop()
  {
    if (this.useFadeOut)
    {
      if (this.count < this.convList.Count - 1)
        this.NextLine();
      else
        this.mmC.GetComponent<CanvasGroup>().DOFade(0.0f, this.fadeOutDuration);
    }
    else
      this.mmC.Close();
  }
}
