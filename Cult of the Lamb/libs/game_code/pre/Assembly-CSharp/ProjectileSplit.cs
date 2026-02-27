// Decompiled with JetBrains decompiler
// Type: ProjectileSplit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ProjectileSplit : Projectile
{
  public GameObject Arrow;
  public float NumSplit = 10f;
  [SerializeField]
  private List<GameObject> children = new List<GameObject>();

  protected override void OnEnable()
  {
    base.OnEnable();
    if (!string.IsNullOrEmpty(this.OnSpawnSoundPath))
      AudioManager.Instance.PlayOneShot(this.OnSpawnSoundPath, this.gameObject);
    if ((bool) (Object) this.Trail)
    {
      this.Trail.Clear();
      this.Trail.emit = true;
    }
    foreach (GameObject child in this.children)
      child.SetActive(true);
    this.Speed = 8f;
  }

  public override void EndOfLife()
  {
    AudioManager.Instance.PlayOneShot("event:/player/Curses/arrow_hit", this.transform.position);
    Debug.Log((object) ("Speed " + (object) this.Speed));
    base.EndOfLife();
    this.Angle = 45f;
    int num = -1;
    while ((double) ++num < (double) this.NumSplit)
    {
      Projectile component = ObjectPool.Spawn(this.Arrow, this.transform.parent).GetComponent<Projectile>();
      component.transform.position = this.transform.position;
      component.Angle = (this.Angle += 360f / this.NumSplit);
      component.team = this.team;
      component.Speed = 4f;
      component.Owner = this.Owner;
    }
    foreach (GameObject child in this.children)
      child.SetActive(false);
  }
}
