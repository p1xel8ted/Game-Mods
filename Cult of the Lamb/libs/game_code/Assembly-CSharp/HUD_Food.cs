// Decompiled with JetBrains decompiler
// Type: HUD_Food
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class HUD_Food : BaseMonoBehaviour
{
  public RectTransform Container;
  public Image ProgressRing;
  public float LerpSpeed = 1f;
  public Image ProgressRingHappiness;
  public bool _offscreen;

  public void Start()
  {
    this._offscreen = DataManager.Instance.Followers.Count <= 0;
    this.Container.anchoredPosition = (Vector2) (this._offscreen ? new Vector3(0.0f, 300f) : Vector3.zero);
  }

  public void Update()
  {
    if (this._offscreen)
    {
      this.Container.anchoredPosition = (Vector2) Vector3.Lerp((Vector3) this.Container.anchoredPosition, new Vector3(0.0f, 300f), Time.deltaTime * 5f);
    }
    else
    {
      this.Container.anchoredPosition = (Vector2) Vector3.Lerp((Vector3) this.Container.anchoredPosition, new Vector3(0.0f, 0.0f), Time.deltaTime * 5f);
      float num1 = 0.0f;
      float num2 = 0.0f;
      float num3 = 0.0f;
      foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
      {
        num1 += allBrain.Stats.Happiness;
        num2 += allBrain.Stats.Satiation + (75f - allBrain.Stats.Starvation);
        ++num3;
      }
      this.ProgressRing.fillAmount = (double) num3 <= 0.0 ? 0.0f : num2 / (175f * num3);
      this.ProgressRingHappiness.fillAmount = (double) num3 <= 0.0 ? 0.0f : num1 / (100f * num3);
    }
  }
}
