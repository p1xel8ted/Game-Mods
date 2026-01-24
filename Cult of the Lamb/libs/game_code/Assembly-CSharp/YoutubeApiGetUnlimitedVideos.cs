// Decompiled with JetBrains decompiler
// Type: YoutubeApiGetUnlimitedVideos
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

#nullable disable
public class YoutubeApiGetUnlimitedVideos : MonoBehaviour
{
  public string APIKey = "AIzaSyDD-lxGLHsBIFPFPt2i31fc0tAHGeAb8mc";
  public string searchKeyword = "Unity";
  [SerializeField]
  public List<YoutubeData> searchResults;
  public int currentResults;
  public int maxResult;

  public void Start()
  {
    this.searchResults = new List<YoutubeData>();
    Debug.Log((object) "Demo call");
    this.StartCoroutine((IEnumerator) this.YoutubeSearch("Unity"));
  }

  public IEnumerator YoutubeSearch(string keyword)
  {
    YoutubeApiGetUnlimitedVideos getUnlimitedVideos = this;
    keyword = keyword.Replace(" ", "%20");
    int num = getUnlimitedVideos.maxResult <= 50 ? getUnlimitedVideos.maxResult : 50;
    UnityWebRequest request = UnityWebRequest.Get(UnityWebRequest.UnEscapeURL(UnityWebRequest.EscapeURL($"https://www.googleapis.com/youtube/v3/search/?q={keyword}&type=video&maxResults={num.ToString()}&part=snippet,id&key={getUnlimitedVideos.APIKey}")));
    yield return (object) request.SendWebRequest();
    JSONNode jsonNode = JSON.Parse(request.downloadHandler.text);
    getUnlimitedVideos.currentResults += jsonNode["items"].Count;
    for (int aIndex = 0; aIndex < jsonNode["items"].Count; ++aIndex)
    {
      YoutubeData youtubeData = new YoutubeData();
      youtubeData.id = (string) jsonNode["items"][aIndex]["id"]["videoId"];
      getUnlimitedVideos.SetSnippet(jsonNode["items"][aIndex]["snippet"], out youtubeData.snippet);
      getUnlimitedVideos.searchResults.Add(youtubeData);
    }
    if (getUnlimitedVideos.currentResults < getUnlimitedVideos.maxResult)
      getUnlimitedVideos.StartCoroutine((IEnumerator) getUnlimitedVideos.YoutubeGetNextPage((string) jsonNode["nextPageToken"]));
    else
      Debug.Log((object) "List is done");
  }

  public IEnumerator YoutubeGetNextPage(string pageToken)
  {
    YoutubeApiGetUnlimitedVideos getUnlimitedVideos = this;
    int num = getUnlimitedVideos.maxResult <= 50 ? getUnlimitedVideos.maxResult : 50;
    UnityWebRequest request = UnityWebRequest.Get(UnityWebRequest.UnEscapeURL(UnityWebRequest.EscapeURL($"https://www.googleapis.com/youtube/v3/search/?pageToken={pageToken}&type=video&maxResults={num.ToString()}&part=snippet,id&key={getUnlimitedVideos.APIKey}")));
    yield return (object) request.SendWebRequest();
    JSONNode jsonNode = JSON.Parse(request.downloadHandler.text);
    getUnlimitedVideos.currentResults += jsonNode["items"].Count;
    for (int aIndex = 0; aIndex < jsonNode["items"].Count; ++aIndex)
    {
      YoutubeData youtubeData = new YoutubeData();
      youtubeData.id = (string) jsonNode["items"][aIndex]["id"]["videoId"];
      getUnlimitedVideos.SetSnippet(jsonNode["items"][aIndex]["snippet"], out youtubeData.snippet);
      getUnlimitedVideos.searchResults.Add(youtubeData);
    }
    if (getUnlimitedVideos.currentResults < getUnlimitedVideos.maxResult)
      getUnlimitedVideos.StartCoroutine((IEnumerator) getUnlimitedVideos.YoutubeGetNextPage((string) jsonNode["nextPageToken"]));
    else
      Debug.Log((object) "List is done");
  }

  public void SetSnippet(JSONNode resultSnippet, out YoutubeSnippet data)
  {
    data = new YoutubeSnippet();
    data.publishedAt = (string) resultSnippet["publishedAt"];
    data.channelId = (string) resultSnippet["channelId"];
    data.title = (string) resultSnippet["title"];
    Debug.Log((object) data.title);
    data.description = (string) resultSnippet["description"];
    data.thumbnails = new YoutubeTumbnails();
    data.thumbnails.defaultThumbnail = new YoutubeThumbnailData();
    data.thumbnails.defaultThumbnail.url = (string) resultSnippet["thumbnails"]["default"]["url"];
    data.thumbnails.defaultThumbnail.width = (string) resultSnippet["thumbnails"]["default"]["width"];
    data.thumbnails.defaultThumbnail.height = (string) resultSnippet["thumbnails"]["default"]["height"];
    data.thumbnails.mediumThumbnail = new YoutubeThumbnailData();
    data.thumbnails.mediumThumbnail.url = (string) resultSnippet["thumbnails"]["medium"]["url"];
    data.thumbnails.mediumThumbnail.width = (string) resultSnippet["thumbnails"]["medium"]["width"];
    data.thumbnails.mediumThumbnail.height = (string) resultSnippet["thumbnails"]["medium"]["height"];
    data.thumbnails.highThumbnail = new YoutubeThumbnailData();
    data.thumbnails.highThumbnail.url = (string) resultSnippet["thumbnails"]["high"]["url"];
    data.thumbnails.highThumbnail.width = (string) resultSnippet["thumbnails"]["high"]["width"];
    data.thumbnails.highThumbnail.height = (string) resultSnippet["thumbnails"]["high"]["height"];
    data.thumbnails.standardThumbnail = new YoutubeThumbnailData();
    data.thumbnails.standardThumbnail.url = (string) resultSnippet["thumbnails"]["standard"]["url"];
    data.thumbnails.standardThumbnail.width = (string) resultSnippet["thumbnails"]["standard"]["width"];
    data.thumbnails.standardThumbnail.height = (string) resultSnippet["thumbnails"]["standard"]["height"];
    data.channelTitle = (string) resultSnippet["channelTitle"];
    data.tags = new string[resultSnippet["tags"].Count];
    for (int aIndex = 0; aIndex < data.tags.Length; ++aIndex)
      data.tags[aIndex] = (string) resultSnippet["tags"][aIndex];
    data.categoryId = (string) resultSnippet["categoryId"];
  }
}
