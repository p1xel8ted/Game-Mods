// Decompiled with JetBrains decompiler
// Type: TextFollowGameObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;

#nullable disable
public class TextFollowGameObject : BaseMonoBehaviour
{
  public GameObject Target;
  public float distance = -1.3f;
  public float angle = -45f;
  public string Text;

  public void Start() => this.GetComponent<TextMeshProUGUI>().text = this.Text;

  public void Update()
  {
    if ((UnityEngine.Object) this.Target == (UnityEngine.Object) null)
      return;
    this.transform.position = Camera.main.WorldToScreenPoint(this.Target.transform.position + new Vector3(0.0f, this.distance * Mathf.Sin((float) Math.PI / 180f * this.angle), this.distance * Mathf.Cos((float) Math.PI / 180f * this.angle)));
  }
}
