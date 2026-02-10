// Decompiled with JetBrains decompiler
// Type: CommentsDemo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class CommentsDemo : MonoBehaviour
{
  public YoutubeAPIManager youtubeapi;
  public Text videoIdInput;
  public Text commentsTextArea;

  public void Start()
  {
    this.youtubeapi = UnityEngine.Object.FindObjectOfType<YoutubeAPIManager>();
    if (!((UnityEngine.Object) this.youtubeapi == (UnityEngine.Object) null))
      return;
    this.youtubeapi = this.gameObject.AddComponent<YoutubeAPIManager>();
  }

  public void GetComments()
  {
    this.youtubeapi.GetComments(this.videoIdInput.text, new Action<YoutubeComments[]>(this.OnFinishLoadingComments));
  }

  public void OnFinishLoadingComments(YoutubeComments[] comments)
  {
    string str = "";
    for (int index = 0; index < comments.Length; ++index)
      str = $"{str}<color=red>{comments[index].authorDisplayName}</color>: {comments[index].textDisplay}\n";
    this.commentsTextArea.text = str;
  }
}
