// Decompiled with JetBrains decompiler
// Type: VideoSearchDemo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class VideoSearchDemo : MonoBehaviour
{
  public YoutubeAPIManager youtubeapi;
  public Text searchField;
  public Text categoryField;
  public Toggle categoryFilter;
  public Dropdown mainFilters;
  public YoutubeVideoUi[] videoListUI;
  public GameObject videoUIResult;
  public GameObject mainUI;
  public string customFilter;
  public string regionCode;

  public void Start()
  {
    this.youtubeapi = UnityEngine.Object.FindObjectOfType<YoutubeAPIManager>();
    if (!((UnityEngine.Object) this.youtubeapi == (UnityEngine.Object) null))
      return;
    this.youtubeapi = this.gameObject.AddComponent<YoutubeAPIManager>();
  }

  public void CustomFilterCheck()
  {
    if (this.customFilter == "" || this.customFilter == null)
    {
      this.customFilter = "";
    }
    else
    {
      this.customFilter = "&" + this.customFilter;
      Debug.Log((object) this.customFilter);
    }
  }

  public void GetTrendingVideos()
  {
    this.youtubeapi.TrendingVideos(this.regionCode, 10, new Action<YoutubeData[]>(this.OnSearchDone));
  }

  public void Search()
  {
    this.CustomFilterCheck();
    YoutubeAPIManager.YoutubeSearchOrderFilter order = YoutubeAPIManager.YoutubeSearchOrderFilter.none;
    switch (this.mainFilters.value)
    {
      case 0:
        order = YoutubeAPIManager.YoutubeSearchOrderFilter.none;
        break;
      case 1:
        order = YoutubeAPIManager.YoutubeSearchOrderFilter.date;
        break;
      case 2:
        order = YoutubeAPIManager.YoutubeSearchOrderFilter.rating;
        break;
      case 3:
        order = YoutubeAPIManager.YoutubeSearchOrderFilter.relevance;
        break;
      case 4:
        order = YoutubeAPIManager.YoutubeSearchOrderFilter.title;
        break;
      case 5:
        order = YoutubeAPIManager.YoutubeSearchOrderFilter.videoCount;
        break;
      case 6:
        order = YoutubeAPIManager.YoutubeSearchOrderFilter.viewCount;
        break;
    }
    if (this.categoryFilter.isOn)
      this.youtubeapi.SearchByCategory(this.searchField.text, this.categoryField.text, 10, order, YoutubeAPIManager.YoutubeSafeSearchFilter.none, new Action<YoutubeData[]>(this.OnSearchDone));
    else
      this.youtubeapi.Search(this.searchField.text, 10, order, YoutubeAPIManager.YoutubeSafeSearchFilter.none, this.customFilter, new Action<YoutubeData[]>(this.OnSearchDone));
  }

  public void SearchByLocation(string location)
  {
    this.CustomFilterCheck();
    YoutubeAPIManager.YoutubeSearchOrderFilter order = YoutubeAPIManager.YoutubeSearchOrderFilter.none;
    switch (this.mainFilters.value)
    {
      case 0:
        order = YoutubeAPIManager.YoutubeSearchOrderFilter.none;
        break;
      case 1:
        order = YoutubeAPIManager.YoutubeSearchOrderFilter.date;
        break;
      case 2:
        order = YoutubeAPIManager.YoutubeSearchOrderFilter.rating;
        break;
      case 3:
        order = YoutubeAPIManager.YoutubeSearchOrderFilter.relevance;
        break;
      case 4:
        order = YoutubeAPIManager.YoutubeSearchOrderFilter.title;
        break;
      case 5:
        order = YoutubeAPIManager.YoutubeSearchOrderFilter.videoCount;
        break;
      case 6:
        order = YoutubeAPIManager.YoutubeSearchOrderFilter.viewCount;
        break;
    }
    string[] strArray = location.Split(',', StringSplitOptions.None);
    this.youtubeapi.SearchByLocation(this.searchField.text, 10, 10, float.Parse(strArray[0]), float.Parse(strArray[1]), order, YoutubeAPIManager.YoutubeSafeSearchFilter.none, this.customFilter, new Action<YoutubeData[]>(this.OnSearchDone));
  }

  public void OnSearchDone(YoutubeData[] results)
  {
    this.videoUIResult.SetActive(true);
    this.LoadVideosOnUI(results);
  }

  public void LoadVideosOnUI(YoutubeData[] videoList)
  {
    for (int message = 0; message < videoList.Length; ++message)
    {
      Debug.Log((object) message);
      this.videoListUI[message].GetComponent<YoutubeVideoUi>().videoName.text = videoList[message].snippet.title;
      this.videoListUI[message].GetComponent<YoutubeVideoUi>().videoId = videoList[message].id;
      this.videoListUI[message].GetComponent<YoutubeVideoUi>().thumbUrl = videoList[message].snippet.thumbnails.defaultThumbnail.url;
      this.videoListUI[message].GetComponent<YoutubeVideoUi>().LoadThumbnail();
    }
  }
}
