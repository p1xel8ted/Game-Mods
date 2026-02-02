// Decompiled with JetBrains decompiler
// Type: Interaction_DropOffItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Interaction_DropOffItem : Interaction
{
  [SerializeField]
  public LineRenderer lineRenderer;
  [SerializeField]
  public SpriteRenderer groundCircle;
  [SerializeField]
  public float distanceTrigger = 5f;
  [Header("Debug")]
  public float debugDistance;
  public bool enabled;

  public void Start()
  {
    this.lineRenderer.enabled = false;
    this.groundCircle.enabled = false;
    this.lineRenderer.gameObject.SetActive(false);
    this.groundCircle.gameObject.SetActive(false);
  }

  public void SetOnState(bool on)
  {
    if ((Object) this.lineRenderer != (Object) null)
    {
      this.lineRenderer.enabled = on;
      this.lineRenderer.gameObject.SetActive(on);
    }
    if ((Object) this.groundCircle != (Object) null)
    {
      this.groundCircle.enabled = on;
      this.groundCircle.gameObject.SetActive(on);
    }
    this.enabled = on;
  }

  public void SetDropOffState(bool inProximity, bool willAccept)
  {
    if (inProximity)
    {
      if (willAccept)
      {
        this.lineRenderer.startColor = StaticColors.GreenColor;
        this.lineRenderer.endColor = StaticColors.GreenColor;
        this.groundCircle.color = new Color(StaticColors.GreenColor.r, StaticColors.GreenColor.g, StaticColors.GreenColor.b, 0.5f);
      }
      else
      {
        this.lineRenderer.startColor = StaticColors.RedColor;
        this.lineRenderer.endColor = StaticColors.RedColor;
        this.groundCircle.color = new Color(StaticColors.RedColor.r, StaticColors.RedColor.g, StaticColors.RedColor.b, 0.5f);
      }
    }
    else
    {
      this.lineRenderer.startColor = StaticColors.OffWhiteColor;
      this.lineRenderer.endColor = StaticColors.OffWhiteColor;
      this.groundCircle.color = new Color(StaticColors.OffWhiteColor.r, StaticColors.OffWhiteColor.g, StaticColors.OffWhiteColor.b, 0.5f);
    }
  }
}
