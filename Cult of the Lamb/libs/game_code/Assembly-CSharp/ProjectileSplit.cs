// Decompiled with JetBrains decompiler
// Type: ProjectileSplit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ProjectileSplit : Projectile
{
  public GameObject Arrow;
  public float NumSplit = 10f;
  public float ChildSpeed = 4f;
  [SerializeField]
  public List<GameObject> children = new List<GameObject>();

  public override void OnEnable()
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
    Debug.Log((object) ("Speed " + this.Speed.ToString()));
    base.EndOfLife();
    this.Angle = 45f;
    int num = -1;
    while ((double) ++num < (double) this.NumSplit)
    {
      Projectile component = ObjectPool.Spawn(this.Arrow, this.transform.parent).GetComponent<Projectile>();
      component.transform.position = this.transform.position;
      component.Angle = (this.Angle += 360f / this.NumSplit);
      component.team = this.team;
      component.Speed = this.ChildSpeed;
      component.Owner = this.Owner;
    }
    foreach (GameObject child in this.children)
      child.SetActive(false);
  }
}
