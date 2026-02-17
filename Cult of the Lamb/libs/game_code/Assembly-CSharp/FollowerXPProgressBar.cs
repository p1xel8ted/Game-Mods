// Decompiled with JetBrains decompiler
// Type: FollowerXPProgressBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class FollowerXPProgressBar : BaseMonoBehaviour
{
  public SpriteRenderer ProgressBar;
  public SpriteRenderer ProgressBG;
  public SpriteRenderer ProgressInstant;
  public Follower follower;
  public bool _shown;
  public Coroutine cUpdateBar;

  public void OnEnable()
  {
    this.Hide();
    Transform transform1 = this.ProgressBar.transform;
    Transform transform2 = this.ProgressInstant.transform;
    Vector3 vector3_1 = new Vector3(0.0f, this.ProgressInstant.transform.localScale.y);
    Vector3 vector3_2 = vector3_1;
    transform2.localScale = vector3_2;
    Vector3 vector3_3 = vector3_1;
    transform1.localScale = vector3_3;
    this.StartCoroutine((IEnumerator) this.WaitForBrain());
  }

  public IEnumerator WaitForBrain()
  {
    while (this.follower.Brain == null)
      yield return (object) null;
  }

  public void OnDisable()
  {
  }

  public void Hide()
  {
    this._shown = false;
    this.SetVisibility();
  }

  public void Show()
  {
    this._shown = true;
    this.SetVisibility();
    if (this.cUpdateBar != null)
      this.StopCoroutine(this.cUpdateBar);
    this.cUpdateBar = this.StartCoroutine((IEnumerator) this.UpdateBar());
  }

  public void SetVisibility()
  {
    this.ProgressBar.gameObject.SetActive(this._shown);
    this.ProgressBG.gameObject.SetActive(this._shown);
    this.ProgressInstant.gameObject.SetActive(this._shown);
  }

  public IEnumerator UpdateBar()
  {
    Vector3 localScale = this.ProgressBar.transform.localScale;
    yield return (object) new WaitForSeconds(1f);
    this.Hide();
  }
}
