// Decompiled with JetBrains decompiler
// Type: ActivateOnSeason
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ActivateOnSeason : MonoBehaviour
{
  public bool ActivateOnWinter;
  public bool DisableOnWinter;
  public bool OnlyInOnlyWinterMode;
  public bool useRenderer = true;
  public MeshRenderer meshRenderer;
  public SpriteRenderer spriteRenderer;
  public Structure structure;
  public Transform[] children;

  public void Awake()
  {
    this.structure = this.GetComponentInParent<Structure>();
    this.children = this.transform.GetComponentsInChildren<Transform>();
    if (this.OnlyInOnlyWinterMode && !DataManager.Instance.WinterModeActive)
    {
      this.gameObject.SetActive(false);
    }
    else
    {
      if (this.useRenderer)
      {
        this.meshRenderer = this.GetComponent<MeshRenderer>();
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
      }
      SeasonsManager.OnSeasonChanged += new SeasonsManager.SeasonEvent(this.OnSeasonChanged);
    }
  }

  public void OnEnable() => this.CheckState();

  public void OnDestroy()
  {
    SeasonsManager.OnSeasonChanged -= new SeasonsManager.SeasonEvent(this.OnSeasonChanged);
  }

  public void OnSeasonChanged(SeasonsManager.Season newseason) => this.CheckState();

  public void CheckState()
  {
    if (SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter)
    {
      if (this.useRenderer)
      {
        if ((Object) this.meshRenderer != (Object) null)
        {
          this.meshRenderer.enabled = this.ActivateOnWinter;
          this.meshRenderer.enabled = !this.DisableOnWinter;
        }
        if ((Object) this.spriteRenderer != (Object) null)
        {
          this.spriteRenderer.enabled = this.ActivateOnWinter;
          this.spriteRenderer.enabled = !this.DisableOnWinter;
        }
      }
      else
      {
        this.gameObject.SetActive(this.ActivateOnWinter);
        this.gameObject.SetActive(!this.DisableOnWinter);
      }
      if (!(bool) (Object) this.structure)
        return;
      int num = Mathf.RoundToInt((float) this.children.Length * (float) ((double) Mathf.Clamp((float) SeasonsManager.WinterSeverity, 1f, 4f) * 0.25 + 0.25));
      ((IList<Transform>) this.children).Shuffle<Transform>();
      for (int index = 0; index < this.children.Length; ++index)
        this.children[index].gameObject.SetActive(index <= num);
    }
    else if (this.useRenderer)
    {
      if ((Object) this.meshRenderer != (Object) null)
      {
        this.meshRenderer.enabled = !this.ActivateOnWinter;
        this.meshRenderer.enabled = this.DisableOnWinter;
      }
      if (!((Object) this.spriteRenderer != (Object) null))
        return;
      this.spriteRenderer.enabled = !this.ActivateOnWinter;
      this.spriteRenderer.enabled = this.DisableOnWinter;
    }
    else
    {
      this.gameObject.SetActive(!this.ActivateOnWinter);
      this.gameObject.SetActive(this.DisableOnWinter);
    }
  }
}
