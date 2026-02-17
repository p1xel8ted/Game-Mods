// Decompiled with JetBrains decompiler
// Type: BackToBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BackToBase : BaseMonoBehaviour
{
  public bool Activated;
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

  public void OnTriggerEnter2D(Collider2D collision)
  {
    if (this.Activated || !collision.gameObject.CompareTag("Player"))
      return;
    this.Play(this.WalkedBack);
    this.Activated = true;
  }
}
