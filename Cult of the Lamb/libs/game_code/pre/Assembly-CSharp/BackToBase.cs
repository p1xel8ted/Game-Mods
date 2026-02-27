// Decompiled with JetBrains decompiler
// Type: BackToBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BackToBase : BaseMonoBehaviour
{
  private bool Activated;
  public GameObject WalkTo;
  public bool WalkedBack = true;
  public bool EnterTemple;

  public void PlayBackToBase()
  {
    BiomeBaseManager.EnterTemple = false;
    GameManager.ToShip();
  }

  public void Play(bool Walking)
  {
    if (Walking)
    {
      this.WalkTo.transform.position = this.WalkTo.transform.position with
      {
        x = PlayerFarming.Instance.transform.position.x
      };
      PlayerFarming.Instance.GoToAndStop(this.WalkTo, DisableCollider: true);
      BiomeBaseManager.WalkedBack = true;
    }
    GameManager.ToShip();
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (this.Activated || !(collision.gameObject.tag == "Player"))
      return;
    this.Play(this.WalkedBack);
    this.Activated = true;
  }
}
