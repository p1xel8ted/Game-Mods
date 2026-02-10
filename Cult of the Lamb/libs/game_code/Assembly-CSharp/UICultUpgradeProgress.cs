// Decompiled with JetBrains decompiler
// Type: UICultUpgradeProgress
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UICultUpgradeProgress : MonoBehaviour
{
  [SerializeField]
  public List<UICultUpgradeProgressNode> nodes = new List<UICultUpgradeProgressNode>();
  [SerializeField]
  public Image progress;
  [SerializeField]
  public Image instantBar;
  public int templeLevel;
  public static bool showCultUpgradeProgressSequence;
  public int count;
  public float UnlockIndex = 6f;
  public Coroutine UpdateBars;

  public void OnEnable() => this.SetUpNodes();

  public void SetUpNodes()
  {
    this.count = 0;
    foreach (UICultUpgradeProgressNode node in this.nodes)
    {
      node.Config(this.count);
      ++this.count;
    }
    for (int index = 0; index < this.nodes.Count; ++index)
    {
      UICultUpgradeProgressNode node = this.nodes[index];
      if ((Object) node.associatedAestheticButton != (Object) null)
      {
        node.associatedAestheticButton.isBorderSelectionItem = true;
        if (node.locked)
          node.associatedAestheticButton.FadeIconImages();
        else
          node.associatedAestheticButton.FadeIconImages(1f);
      }
    }
    this.UnlockIndex = (float) (DataManager.Instance.TempleLevel - 1);
    this.templeLevel = (int) this.UnlockIndex;
    this.UpgradeSequence();
  }

  public void UpgradeSequence()
  {
    if (this.UpdateBars != null)
      this.StopCoroutine(this.UpdateBars);
    if (UICultUpgradeProgress.showCultUpgradeProgressSequence)
    {
      this.UpdateBars = this.StartCoroutine((IEnumerator) this.AnimateUpgradeSequence());
      this.SetFillBars(true);
    }
    else
      this.SetFillBars();
    UICultUpgradeProgress.showCultUpgradeProgressSequence = false;
  }

  public void SetFillBars(bool sequence = false)
  {
    if ((double) this.UnlockIndex < 1.0)
    {
      this.progress.fillAmount = 0.0f;
      this.instantBar.fillAmount = 0.0f;
    }
    else
    {
      this.progress.fillAmount = (float) (((double) this.UnlockIndex - 1.0) * 0.125);
      this.progress.fillAmount = sequence ? (float) (((double) this.UnlockIndex - 1.0) * 0.125) : this.UnlockIndex * 0.125f;
      this.instantBar.fillAmount = this.UnlockIndex * 0.125f;
    }
  }

  public IEnumerator AnimateUpgradeSequence()
  {
    UICultUpgradeProgress cultUpgradeProgress = this;
    yield return (object) new WaitForSeconds(0.25f);
    Debug.Log((object) $"Start Filling up upgrade bar {cultUpgradeProgress.progress.fillAmount.ToString()}/{cultUpgradeProgress.instantBar.fillAmount.ToString()}");
    cultUpgradeProgress.progress.transform.DOShakePosition(1.5f, 0.1f);
    AudioManager.Instance.PlayOneShot("event:/Stings/sins_snake_sting", cultUpgradeProgress.gameObject);
    while ((double) cultUpgradeProgress.progress.fillAmount < (double) cultUpgradeProgress.instantBar.fillAmount)
    {
      cultUpgradeProgress.progress.fillAmount += 0.0015f;
      yield return (object) null;
    }
    yield return (object) new WaitForSeconds(0.5f);
    UICultUpgradeProgressNode node = cultUpgradeProgress.nodes[(int) cultUpgradeProgress.UnlockIndex];
    if ((Object) node != (Object) null)
      node.SetUnlocked();
    yield return (object) null;
  }
}
