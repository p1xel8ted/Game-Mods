// Decompiled with JetBrains decompiler
// Type: YoutubeAPIManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using SimpleJSON;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

#nullable disable
public class YoutubeAPIManager : MonoBehaviour
{
  public YoutubeData data;
  public YoutubeData[] searchResults;
  public YoutubeComments[] comments;
  public YoutubePlaylistItems[] playslistItems;
  public YoutubeChannel[] channels;
  public const string APIKey = "AIzaSyCSh8NNjutCdJ3pAMJjWrGFbB8_onIMn1Y";

  public void GetVideoData(string videoId, Action<YoutubeData> callback)
  {
    this.StartCoroutine((IEnumerator) this.LoadSingleVideo(videoId, callback));
  }

  public void Start()
  {
    Debug.LogWarning((object) "REMEMBER TO CHANGE THE API KEY TO YOUR OWN KEY - REMOVE THIS IF YOU CHANGED");
  }

  public void GetChannelVideos(string channelId, int maxResults, Action<YoutubeData[]> callback)
  {
    this.StartCoroutine((IEnumerator) this.GetVideosFromChannel(channelId, maxResults, callback));
  }

  public void Search(
    string keyword,
    int maxResult,
    YoutubeAPIManager.YoutubeSearchOrderFilter order,
    YoutubeAPIManager.YoutubeSafeSearchFilter safeSearch,
    string customFilters,
    Action<YoutubeData[]> callback)
  {
    this.StartCoroutine((IEnumerator) this.YoutubeSearch(keyword, maxResult, order, safeSearch, customFilters, callback));
  }

  public void TrendingVideos(string regionCode, int maxResult, Action<YoutubeData[]> callback)
  {
    this.StartCoroutine((IEnumerator) this.GetTrendingVideos(regionCode, maxResult, callback));
  }

  public void SearchForChannels(
    string keyword,
    int maxResult,
    YoutubeAPIManager.YoutubeSearchOrderFilter order,
    YoutubeAPIManager.YoutubeSafeSearchFilter safeSearch,
    Action<YoutubeChannel[]> callback)
  {
    this.StartCoroutine((IEnumerator) this.YoutubeSearchChannel(keyword, maxResult, order, safeSearch, callback));
  }

  public void SearchByCategory(
    string keyword,
    string category,
    int maxResult,
    YoutubeAPIManager.YoutubeSearchOrderFilter order,
    YoutubeAPIManager.YoutubeSafeSearchFilter safeSearch,
    Action<YoutubeData[]> callback)
  {
    this.StartCoroutine((IEnumerator) this.YoutubeSearchUsingCategory(keyword, category, maxResult, order, safeSearch, callback));
  }

  public void SearchByLocation(
    string keyword,
    int maxResult,
    int locationRadius,
    float latitude,
    float longitude,
    YoutubeAPIManager.YoutubeSearchOrderFilter order,
    YoutubeAPIManager.YoutubeSafeSearchFilter safeSearch,
    string customFilters,
    Action<YoutubeData[]> callback)
  {
    this.StartCoroutine((IEnumerator) this.YoutubeSearchByLocation(keyword, maxResult, locationRadius, latitude, longitude, order, safeSearch, customFilters, callback));
  }

  public void GetComments(string videoId, Action<YoutubeComments[]> callback)
  {
    this.StartCoroutine((IEnumerator) this.YoutubeCallComments(videoId, callback));
  }

  public void GetPlaylistItems(
    string playlistId,
    int maxResults,
    Action<YoutubePlaylistItems[]> callback)
  {
    this.StartCoroutine((IEnumerator) this.YoutubeCallPlaylist(playlistId, maxResults, callback));
  }

  public IEnumerator GetVideosFromChannel(
    string channelId,
    int maxResults,
    Action<YoutubeData[]> callback)
  {
    UnityWebRequest request = UnityWebRequest.Get($"https://www.googleapis.com/youtube/v3/search?order=date&type=video&part=snippet&channelId={channelId}&maxResults={maxResults.ToString()}&key=AIzaSyCSh8NNjutCdJ3pAMJjWrGFbB8_onIMn1Y");
    yield return (object) request.SendWebRequest();
    Debug.Log((object) request.url);
    JSONNode jsonNode = JSON.Parse(request.downloadHandler.text);
    this.searchResults = new YoutubeData[jsonNode["items"].Count];
    for (int aIndex = 0; aIndex < this.searchResults.Length; ++aIndex)
    {
      this.searchResults[aIndex] = new YoutubeData();
      this.searchResults[aIndex].id = (string) jsonNode["items"][aIndex]["id"]["videoId"];
      this.SetSnippet(jsonNode["items"][aIndex]["snippet"], out this.searchResults[aIndex].snippet);
    }
    callback(this.searchResults);
  }

  public IEnumerator YoutubeCallPlaylist(
    string playlistId,
    int maxResults,
    Action<YoutubePlaylistItems[]> callback)
  {
    UnityWebRequest request = UnityWebRequest.Get($"https://www.googleapis.com/youtube/v3/playlistItems/?playlistId={playlistId}&maxResults={maxResults.ToString()}&part=snippet%2CcontentDetails&key=AIzaSyCSh8NNjutCdJ3pAMJjWrGFbB8_onIMn1Y");
    yield return (object) request.SendWebRequest();
    Debug.Log((object) request.url);
    JSONNode jsonNode = JSON.Parse(request.downloadHandler.text);
    this.playslistItems = new YoutubePlaylistItems[jsonNode["items"].Count];
    for (int aIndex = 0; aIndex < this.playslistItems.Length; ++aIndex)
    {
      this.playslistItems[aIndex] = new YoutubePlaylistItems();
      this.playslistItems[aIndex].videoId = (string) jsonNode["items"][aIndex]["snippet"]["resourceId"]["videoId"];
      this.SetSnippet(jsonNode["items"][aIndex]["snippet"], out this.playslistItems[aIndex].snippet);
    }
    callback(this.playslistItems);
  }

  public IEnumerator YoutubeCallComments(string videoId, Action<YoutubeComments[]> callback)
  {
    UnityWebRequest request = UnityWebRequest.Get($"https://www.googleapis.com/youtube/v3/commentThreads/?videoId={videoId}&part=snippet%2Creplies&key=AIzaSyCSh8NNjutCdJ3pAMJjWrGFbB8_onIMn1Y");
    yield return (object) request.SendWebRequest();
    Debug.Log((object) request.url);
    JSONNode jsonNode = JSON.Parse(request.downloadHandler.text);
    this.comments = new YoutubeComments[jsonNode["items"].Count];
    for (int aIndex = 0; aIndex < this.comments.Length; ++aIndex)
    {
      this.comments[aIndex] = new YoutubeComments();
      this.SetComment(jsonNode["items"][aIndex]["snippet"], out this.comments[aIndex]);
    }
    callback(this.comments);
  }

  public IEnumerator YoutubeSearchUsingCategory(
    string keyword,
    string category,
    int maxresult,
    YoutubeAPIManager.YoutubeSearchOrderFilter order,
    YoutubeAPIManager.YoutubeSafeSearchFilter safeSearch,
    Action<YoutubeData[]> callback)
  {
    keyword = keyword.Replace(" ", "%20");
    category = category.Replace(" ", "%20");
    string str1 = "";
    if (order != YoutubeAPIManager.YoutubeSearchOrderFilter.none)
      str1 = "&order=" + order.ToString();
    string str2 = "&safeSearch=" + safeSearch.ToString();
    UnityWebRequest request = UnityWebRequest.Get($"https://www.googleapis.com/youtube/v3/search/?q={keyword}&videoCategoryId={category}&maxResults={maxresult.ToString()}&type=video&part=snippet,id&key=AIzaSyCSh8NNjutCdJ3pAMJjWrGFbB8_onIMn1Y{str1}{str2}");
    yield return (object) request.SendWebRequest();
    Debug.Log((object) request.url);
    JSONNode jsonNode = JSON.Parse(request.downloadHandler.text);
    this.searchResults = new YoutubeData[jsonNode["items"].Count];
    Debug.Log((object) this.searchResults.Length);
    for (int aIndex = 0; aIndex < this.searchResults.Length; ++aIndex)
    {
      this.searchResults[aIndex] = new YoutubeData();
      this.searchResults[aIndex].id = (string) jsonNode["items"][aIndex]["id"]["videoId"];
      this.SetSnippet(jsonNode["items"][aIndex]["snippet"], out this.searchResults[aIndex].snippet);
    }
    callback(this.searchResults);
  }

  public IEnumerator YoutubeSearchChannel(
    string keyword,
    int maxresult,
    YoutubeAPIManager.YoutubeSearchOrderFilter order,
    YoutubeAPIManager.YoutubeSafeSearchFilter safeSearch,
    Action<YoutubeChannel[]> callback)
  {
    keyword = keyword.Replace(" ", "%20");
    string str1 = "";
    if (order != YoutubeAPIManager.YoutubeSearchOrderFilter.none)
      str1 = "&order=" + order.ToString();
    string str2 = "&safeSearch=" + safeSearch.ToString();
    UnityWebRequest request = UnityWebRequest.Get($"https://www.googleapis.com/youtube/v3/search/?q={keyword}&type=channel&maxResults={maxresult.ToString()}&part=snippet,id&key=AIzaSyCSh8NNjutCdJ3pAMJjWrGFbB8_onIMn1Y{str1}{str2}");
    yield return (object) request.SendWebRequest();
    Debug.Log((object) request.url);
    JSONNode jsonNode = JSON.Parse(request.downloadHandler.text);
    this.channels = new YoutubeChannel[jsonNode["items"].Count];
    for (int aIndex = 0; aIndex < this.channels.Length; ++aIndex)
    {
      this.channels[aIndex] = new YoutubeChannel();
      this.channels[aIndex].id = (string) jsonNode["items"][aIndex]["id"]["channelId"];
      this.channels[aIndex].title = (string) jsonNode["items"][aIndex]["snippet"]["title"];
      this.channels[aIndex].description = (string) jsonNode["items"][aIndex]["snippet"]["description"];
      this.channels[aIndex].thumbnail = (string) jsonNode["items"][aIndex]["snippet"]["thumbnails"]["high"]["url"];
    }
    callback(this.channels);
  }

  public IEnumerator GetTrendingVideos(
    string regionCode,
    int maxresult,
    Action<YoutubeData[]> callback)
  {
    UnityWebRequest request = UnityWebRequest.Get(UnityWebRequest.UnEscapeURL(UnityWebRequest.EscapeURL($"https://www.googleapis.com/youtube/v3/videos?part=snippet,id&chart=mostPopular&regionCode={regionCode}&maxResults={maxresult.ToString()}&key=AIzaSyCSh8NNjutCdJ3pAMJjWrGFbB8_onIMn1Y")));
    yield return (object) request.SendWebRequest();
    Debug.Log((object) request.url);
    JSONNode jsonNode = JSON.Parse(request.downloadHandler.text);
    this.searchResults = new YoutubeData[jsonNode["items"].Count];
    Debug.Log((object) this.searchResults.Length);
    for (int aIndex = 0; aIndex < this.searchResults.Length; ++aIndex)
    {
      this.searchResults[aIndex] = new YoutubeData();
      this.searchResults[aIndex].id = (string) jsonNode["items"][aIndex]["id"];
      this.SetSnippet(jsonNode["items"][aIndex]["snippet"], out this.searchResults[aIndex].snippet);
    }
    callback(this.searchResults);
  }

  public IEnumerator YoutubeSearch(
    string keyword,
    int maxresult,
    YoutubeAPIManager.YoutubeSearchOrderFilter order,
    YoutubeAPIManager.YoutubeSafeSearchFilter safeSearch,
    string customFilters,
    Action<YoutubeData[]> callback)
  {
    keyword = keyword.Replace(" ", "%20");
    string str1 = "";
    if (order != YoutubeAPIManager.YoutubeSearchOrderFilter.none)
      str1 = "&order=" + order.ToString();
    string str2 = "&safeSearch=" + safeSearch.ToString();
    string str3 = UnityWebRequest.EscapeURL($"https://www.googleapis.com/youtube/v3/search/?q={keyword}&type=video&maxResults={maxresult.ToString()}&part=snippet,id&key=AIzaSyCSh8NNjutCdJ3pAMJjWrGFbB8_onIMn1Y{str1}{str2}{customFilters}");
    Debug.Log((object) str3);
    UnityWebRequest request = UnityWebRequest.Get(UnityWebRequest.UnEscapeURL(str3));
    yield return (object) request.SendWebRequest();
    Debug.Log((object) request.url);
    JSONNode jsonNode = JSON.Parse(request.downloadHandler.text);
    this.searchResults = new YoutubeData[jsonNode["items"].Count];
    Debug.Log((object) this.searchResults.Length);
    for (int aIndex = 0; aIndex < this.searchResults.Length; ++aIndex)
    {
      this.searchResults[aIndex] = new YoutubeData();
      this.searchResults[aIndex].id = (string) jsonNode["items"][aIndex]["id"]["videoId"];
      this.SetSnippet(jsonNode["items"][aIndex]["snippet"], out this.searchResults[aIndex].snippet);
    }
    callback(this.searchResults);
  }

  public IEnumerator YoutubeSearchByLocation(
    string keyword,
    int maxResult,
    int locationRadius,
    float latitude,
    float longitude,
    YoutubeAPIManager.YoutubeSearchOrderFilter order,
    YoutubeAPIManager.YoutubeSafeSearchFilter safeSearch,
    string customFilters,
    Action<YoutubeData[]> callback)
  {
    keyword = keyword.Replace(" ", "%20");
    string str1 = "";
    if (order != YoutubeAPIManager.YoutubeSearchOrderFilter.none)
      str1 = "&order=" + order.ToString();
    string str2 = "&safeSearch=" + safeSearch.ToString();
    UnityWebRequest request = UnityWebRequest.Get($"https://www.googleapis.com/youtube/v3/search/?type=video&q={keyword}&type=video&locationRadius={locationRadius.ToString()}mi&location={latitude.ToString()}%2C{longitude.ToString()}&part=snippet,id&maxResults={maxResult.ToString()}&key=AIzaSyCSh8NNjutCdJ3pAMJjWrGFbB8_onIMn1Y{str1}{str2}{customFilters}");
    yield return (object) request.SendWebRequest();
    Debug.Log((object) request.url);
    JSONNode jsonNode = JSON.Parse(request.downloadHandler.text);
    this.searchResults = new YoutubeData[jsonNode["items"].Count];
    Debug.Log((object) this.searchResults.Length);
    for (int aIndex = 0; aIndex < this.searchResults.Length; ++aIndex)
    {
      this.searchResults[aIndex] = new YoutubeData();
      this.searchResults[aIndex].id = (string) jsonNode["items"][aIndex]["id"]["videoId"];
      this.SetSnippet(jsonNode["items"][aIndex]["snippet"], out this.searchResults[aIndex].snippet);
    }
    callback(this.searchResults);
  }

  public IEnumerator LoadSingleVideo(string videoId, Action<YoutubeData> callback)
  {
    UnityWebRequest request = UnityWebRequest.Get($"https://www.googleapis.com/youtube/v3/videos?id={videoId}&part=snippet,id,contentDetails,statistics&key=AIzaSyCSh8NNjutCdJ3pAMJjWrGFbB8_onIMn1Y");
    yield return (object) request.SendWebRequest();
    Debug.Log((object) request.url);
    this.data = new YoutubeData();
    JSONNode jsonNode = JSON.Parse(request.downloadHandler.text)["items"][0];
    this.data.id = (string) jsonNode["id"];
    this.SetSnippet(jsonNode["snippet"], out this.data.snippet);
    this.SetStatistics(jsonNode["statistics"], out this.data.statistics);
    this.SetContentDetails(jsonNode["contentDetails"], out this.data.contentDetails);
    callback(this.data);
  }

  public void SetSnippet(JSONNode resultSnippet, out YoutubeSnippet data)
  {
    data = new YoutubeSnippet();
    data.publishedAt = (string) resultSnippet["publishedAt"];
    data.channelId = (string) resultSnippet["channelId"];
    data.title = (string) resultSnippet["title"];
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

  public void SetStatistics(JSONNode resultStatistics, out YoutubeStatistics data)
  {
    data = new YoutubeStatistics();
    data.viewCount = (string) resultStatistics["viewCount"];
    data.likeCount = (string) resultStatistics["likeCount"];
    data.dislikeCount = (string) resultStatistics["dislikeCount"];
    data.favoriteCount = (string) resultStatistics["favoriteCount"];
    data.commentCount = (string) resultStatistics["commentCount"];
  }

  public void SetContentDetails(JSONNode resultContentDetails, out YoutubeContentDetails data)
  {
    data = new YoutubeContentDetails();
    data.duration = (string) resultContentDetails["duration"];
    data.dimension = (string) resultContentDetails["dimension"];
    data.definition = (string) resultContentDetails["definition"];
    data.caption = (string) resultContentDetails["caption"];
    data.licensedContent = (string) resultContentDetails["licensedContent"];
    data.projection = (string) resultContentDetails["projection"];
    if (resultContentDetails["contentRating"] != (object) null)
    {
      Debug.Log((object) "Age restrict found!");
      if (resultContentDetails["contentRating"]["ytRating"] == (object) "ytAgeRestricted")
        data.ageRestrict = true;
      else
        data.ageRestrict = false;
    }
    else
      data.ageRestrict = false;
  }

  public void SetComment(JSONNode commentsData, out YoutubeComments data)
  {
    data = new YoutubeComments();
    data.videoId = (string) commentsData["videoId"];
    JSONNode jsonNode = commentsData["topLevelComment"]["snippet"];
    data.authorDisplayName = (string) jsonNode["authorDisplayName"];
    data.authorProfileImageUrl = (string) jsonNode["authorProfileImageUrl"];
    data.authorChannelUrl = (string) jsonNode["authorChannelUrl"];
    data.authorChannelId = (string) jsonNode["authorChannelId"]["value"];
    data.textDisplay = (string) jsonNode["textDisplay"];
    data.textOriginal = (string) jsonNode["textOriginal"];
    data.canRate = jsonNode["canRate"].AsBool;
    data.viewerRating = (string) jsonNode["viewerRating"];
    data.likeCount = jsonNode["likeCount"].AsInt;
    data.publishedAt = (string) jsonNode["publishedAt"];
    data.updatedAt = (string) jsonNode["updatedAt"];
  }

  public enum YoutubeSearchOrderFilter
  {
    none,
    date,
    rating,
    relevance,
    title,
    videoCount,
    viewCount,
  }

  public enum YoutubeSafeSearchFilter
  {
    none,
    moderate,
    strict,
  }
}
