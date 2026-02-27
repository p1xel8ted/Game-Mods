// Decompiled with JetBrains decompiler
// Type: DivineInspirationCounts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;

#nullable disable
public class DivineInspirationCounts : MonoBehaviour
{
  public TextMeshProUGUI DivineInspiration;
  public TextMeshProUGUI DisciplePoints;

  private void Start() => this.CheckText();

  private void OnEnable()
  {
    UpgradeSystem.OnAbilityPointDelta += new System.Action(this.CheckText);
    UpgradeSystem.OnDisciplePointDelta += new System.Action(this.CheckText);
  }

  private void OnDisable()
  {
    UpgradeSystem.OnAbilityPointDelta -= new System.Action(this.CheckText);
    UpgradeSystem.OnDisciplePointDelta -= new System.Action(this.CheckText);
  }

  private void CheckText()
  {
    int num;
    if (UpgradeSystem.AbilityPoints > 0)
    {
      string text = this.DivineInspiration.text;
      num = UpgradeSystem.AbilityPoints;
      string str1 = num.ToString();
      if (text != str1)
      {
        this.DivineInspiration.transform.parent.localScale = Vector3.one * 1.5f;
        this.DivineInspiration.transform.parent.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
      }
      TextMeshProUGUI divineInspiration = this.DivineInspiration;
      num = UpgradeSystem.AbilityPoints;
      string str2 = num.ToString();
      divineInspiration.text = str2;
      this.DivineInspiration.transform.parent.gameObject.SetActive(true);
    }
    else
    {
      this.DivineInspiration.text = "";
      this.DivineInspiration.transform.parent.gameObject.SetActive(false);
    }
    if (UpgradeSystem.DisciplePoints > 0)
    {
      string text = this.DisciplePoints.text;
      num = UpgradeSystem.DisciplePoints;
      string str3 = num.ToString();
      if (text != str3)
      {
        this.DisciplePoints.transform.parent.localScale = Vector3.one * 1.5f;
        this.DisciplePoints.transform.parent.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
      }
      TextMeshProUGUI disciplePoints = this.DisciplePoints;
      num = UpgradeSystem.DisciplePoints;
      string str4 = num.ToString();
      disciplePoints.text = str4;
      this.DisciplePoints.transform.parent.gameObject.SetActive(true);
    }
    else
    {
      this.DisciplePoints.text = "";
      this.DisciplePoints.transform.parent.gameObject.SetActive(false);
    }
  }
}
