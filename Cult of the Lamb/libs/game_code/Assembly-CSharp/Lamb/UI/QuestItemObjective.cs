// Decompiled with JetBrains decompiler
// Type: Lamb.UI.QuestItemObjective
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class QuestItemObjective : MonoBehaviour
{
  public const string kTickboxOnAnimation = "on";
  public const string kTickBoxOffAnimation = "off";
  public const string kTickBoxOnFailedAnimation = "on-failed";
  [SerializeField]
  public SkeletonGraphic _tickBox;
  [SerializeField]
  public TextMeshProUGUI _description;
  [SerializeField]
  public Image[] _strikethroughs;
  [Header("Time")]
  [SerializeField]
  public GameObject _timeContainer;
  [SerializeField]
  public Image _radialProgress;
  public float _fill;

  public int NumLines => this._description.textInfo.lineCount;

  public void Configure(ObjectivesData objectivesData)
  {
    this._description.text = objectivesData.Text;
    if (objectivesData.IsComplete)
    {
      this._fill = 1f;
      this._description.color = StaticColors.GreyColor;
      this._tickBox.SetAnimation("on");
    }
    else
    {
      this._description.color = StaticColors.OffWhiteColor;
      this._fill = 0.0f;
      this._tickBox.SetAnimation("off");
    }
    this._timeContainer.SetActive(objectivesData.HasExpiry);
    if (objectivesData.HasExpiry)
      this._radialProgress.fillAmount = objectivesData.ExpiryTimeNormalized;
    this.StartCoroutine((IEnumerator) this.DeferredStrikethroughUpdate());
  }

  public void Configure(ObjectivesDataFinalized objectivesData, bool failed = false)
  {
    this._description.text = objectivesData.GetText();
    this._fill = 1f;
    if (failed)
      this._tickBox.SetAnimation("on-failed");
    else
      this._tickBox.SetAnimation("on");
    this._timeContainer.SetActive(false);
    this.StartCoroutine((IEnumerator) this.DeferredStrikethroughUpdate());
  }

  public IEnumerator DeferredStrikethroughUpdate()
  {
    yield return (object) null;
    float num = this._description.rectTransform.rect.height / (float) (this.NumLines * 2);
    for (int index = 0; index < this._strikethroughs.Length; ++index)
    {
      this._strikethroughs[index].gameObject.SetActive(index < this.NumLines);
      this._strikethroughs[index].fillAmount = this._fill;
      if (index < this.NumLines)
        this._strikethroughs[index].rectTransform.anchoredPosition = (Vector2) new Vector3(0.0f, (float) ((double) this._description.rectTransform.rect.height * 0.5 - (double) num - (double) num * 2.0 * (double) index));
    }
  }
}
