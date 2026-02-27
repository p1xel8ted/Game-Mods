// Decompiled with JetBrains decompiler
// Type: TextFollowGameObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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

  private void Start() => this.GetComponent<TextMeshProUGUI>().text = this.Text;

  private void Update()
  {
    if ((UnityEngine.Object) this.Target == (UnityEngine.Object) null)
      return;
    this.transform.position = Camera.main.WorldToScreenPoint(this.Target.transform.position + new Vector3(0.0f, this.distance * Mathf.Sin((float) Math.PI / 180f * this.angle), this.distance * Mathf.Cos((float) Math.PI / 180f * this.angle)));
  }
}
