// Decompiled with JetBrains decompiler
// Type: UICursesStatue
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UICursesStatue : UIHeartStatue
{
  public new void OnEnable()
  {
    UIHeartStatue.Instance = (UIHeartStatue) this;
    FollowerManager.OnFollowerAdded += new FollowerManager.FollowerChanged(this.OnFollowerUpdated);
    FollowerManager.OnFollowerRemoved += new FollowerManager.FollowerChanged(this.OnFollowerUpdated);
  }

  public new void OnDisable()
  {
    UIHeartStatue.Instance = (UIHeartStatue) null;
    FollowerManager.OnFollowerAdded -= new FollowerManager.FollowerChanged(this.OnFollowerUpdated);
    FollowerManager.OnFollowerRemoved -= new FollowerManager.FollowerChanged(this.OnFollowerUpdated);
  }

  public new void OnFollowerUpdated(int followerID) => UICursesStatue.UpdateSpiritAmmo();

  public override bool CanUpgrade() => DataManager.Instance.ShrineCurses < 4;

  public override void Repair() => DataManager.Instance.ShrineCurses = 1;

  public override void Upgrade()
  {
    if (DataManager.Instance.ShrineCurses >= 4 || this.Upgrading)
      return;
    Debug.Log((object) "Upgrade!");
    this.StartCoroutine((IEnumerator) this.DoUpgrade());
  }

  public override IEnumerator DoUpgrade()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    UICursesStatue uiCursesStatue = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      uiCursesStatue.ShowButton();
      uiCursesStatue.Upgrading = false;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    uiCursesStatue.Upgrading = true;
    uiCursesStatue.HideButton();
    DataManager.Instance.ShrineCurses = Mathf.Min(++DataManager.Instance.ShrineCurses, 4);
    UICursesStatue.UpdateSpiritAmmo();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(0.3f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public static void UpdateSpiritAmmo()
  {
    PlayerArrows objectOfType = Object.FindObjectOfType<PlayerArrows>();
    if ((Object) objectOfType == (Object) null)
      return;
    int count = DataManager.Instance.Followers.Count;
    switch (DataManager.Instance.ShrineCurses)
    {
      case 0:
        DataManager.Instance.PLAYER_SPIRIT_TOTAL_AMMO = 0;
        break;
      case 1:
        if (count > 1)
        {
          DataManager.Instance.PLAYER_SPIRIT_TOTAL_AMMO = 1;
          break;
        }
        if (count <= 1)
        {
          DataManager.Instance.PLAYER_SPIRIT_TOTAL_AMMO = 0;
          break;
        }
        break;
      case 2:
        if (count > 1 && count <= 4)
        {
          DataManager.Instance.PLAYER_SPIRIT_TOTAL_AMMO = 1;
          break;
        }
        if (count > 4)
        {
          DataManager.Instance.PLAYER_SPIRIT_TOTAL_AMMO = 2;
          break;
        }
        if (count <= 1)
        {
          DataManager.Instance.PLAYER_SPIRIT_TOTAL_AMMO = 0;
          break;
        }
        break;
      case 3:
        if (count > 1 && count <= 4)
        {
          DataManager.Instance.PLAYER_SPIRIT_TOTAL_AMMO = 1;
          break;
        }
        if (count > 4 && count <= 9)
        {
          DataManager.Instance.PLAYER_SPIRIT_TOTAL_AMMO = 2;
          break;
        }
        if (count > 9)
        {
          DataManager.Instance.PLAYER_SPIRIT_TOTAL_AMMO = 3;
          break;
        }
        if (count <= 1)
        {
          DataManager.Instance.PLAYER_SPIRIT_TOTAL_AMMO = 0;
          break;
        }
        break;
      case 4:
        if (count > 1 && count <= 4)
        {
          DataManager.Instance.PLAYER_SPIRIT_TOTAL_AMMO = 1;
          break;
        }
        if (count > 4 && count <= 9)
        {
          DataManager.Instance.PLAYER_SPIRIT_TOTAL_AMMO = 2;
          break;
        }
        if (count > 9 && count <= 14)
        {
          DataManager.Instance.PLAYER_SPIRIT_TOTAL_AMMO = 3;
          break;
        }
        if (count > 14)
        {
          DataManager.Instance.PLAYER_SPIRIT_TOTAL_AMMO = 4;
          break;
        }
        if (count <= 1)
        {
          DataManager.Instance.PLAYER_SPIRIT_TOTAL_AMMO = 0;
          break;
        }
        break;
    }
    objectOfType.RestockAllArrows();
    UIHeartStatue.Instance?.UpdateDisplayIcons();
  }

  public override void UpdateDisplayIcons()
  {
    if (!((Object) UIHeartStatue.Instance != (Object) null))
      return;
    int index1 = -1;
    while (++index1 < UIHeartStatue.Instance.DiplayIcons.Count)
    {
      if (index1 < DataManager.Instance.PLAYER_SPIRIT_TOTAL_AMMO)
        UIHeartStatue.Instance.DiplayIcons[index1].GetComponent<Image>().material = this.normalMaterial;
      else
        UIHeartStatue.Instance.DiplayIcons[index1].GetComponent<Image>().material = this.BlackAndWhiteMaterial;
    }
    if (UIHeartStatue.Instance.LockIcons == null)
      return;
    int index2 = -1;
    while (++index2 < UIHeartStatue.Instance.LockIcons.Count)
    {
      if (index2 < DataManager.Instance.ShrineCurses)
        this.LockIcons[index2].SetActive(false);
      else
        this.LockIcons[index2].SetActive(true);
    }
  }
}
