// Decompiled with JetBrains decompiler
// Type: TrapSpiderWeb
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class TrapSpiderWeb : BaseMonoBehaviour
{
  private float Timer;
  private float Scale;
  private float ScaleSpeed;
  private List<PlayerController> playerControllers = new List<PlayerController>();

  private void Start()
  {
    this.transform.position = this.transform.position with
    {
      z = 0.0f
    };
  }

  private void Update()
  {
    foreach (PlayerController playerController in this.playerControllers)
      playerController.RunSpeed = 1f;
    if ((double) (this.Timer += Time.deltaTime) > 30.0)
    {
      this.Scale -= 0.5f * Time.deltaTime;
      this.transform.localScale = Vector3.one * this.Scale;
      if ((double) this.Scale > 0.0)
        return;
      Object.Destroy((Object) this.gameObject);
    }
    else
    {
      this.ScaleSpeed += (float) ((1.0 - (double) this.Scale) * 0.20000000298023224);
      this.Scale += (this.ScaleSpeed *= 0.8f);
      this.transform.localScale = Vector3.one * this.Scale;
    }
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    PlayerController component = collision.gameObject.GetComponent<PlayerController>();
    if (!((Object) component != (Object) null) || this.playerControllers.Contains(component))
      return;
    this.playerControllers.Add(component);
  }

  private void OnTriggerExit2D(Collider2D collision)
  {
    PlayerController component = collision.gameObject.GetComponent<PlayerController>();
    if (!this.playerControllers.Contains(component))
      return;
    component.RunSpeed = component.DefaultRunSpeed;
    this.playerControllers.Remove(component);
  }
}
