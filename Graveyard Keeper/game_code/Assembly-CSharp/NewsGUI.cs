// Decompiled with JetBrains decompiler
// Type: NewsGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class NewsGUI : MonoBehaviour
{
  public static string update_url;
  public static float last_ver;
  public GameObject loading_news_go;
  public GameObject update_button;
  public const bool NEWS_AVAILABLE = false;

  public void Open() => this.Hide();

  public void Init() => this.Hide();

  public void Hide() => this.gameObject.SetActive(false);
}
