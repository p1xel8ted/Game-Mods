// Decompiled with JetBrains decompiler
// Type: EnemyTimeCop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class EnemyTimeCop : BaseMonoBehaviour
{
  public SpriteRenderer SpawnImage;
  public GameObject Enemy;
  public float Timer;
  public float SpawnTime = 3f;
  public float Scale;
  public float ScaleSpeed;
  public float Pulse;
  public bool init;

  public void Start()
  {
    this.Enemy.SetActive(false);
    this.SpawnImage.gameObject.SetActive(true);
    this.SpawnImage.transform.localScale = Vector3.one * 0.1f;
  }

  public void Update()
  {
    if ((double) (this.Timer += Time.deltaTime) > (double) this.SpawnTime)
    {
      if (!this.init)
      {
        this.ScaleSpeed = 0.2f;
        this.init = true;
      }
      this.ScaleSpeed -= 0.02f;
      this.Scale += this.ScaleSpeed;
      this.SpawnImage.transform.localScale = new Vector3(this.Scale, this.Scale, this.Scale);
      if ((double) this.Scale > 0.0)
        return;
      Health component = this.Enemy.GetComponent<Health>();
      Explosion.CreateExplosion(this.transform.position, component.team, component, 1f);
      this.Enemy.SetActive(true);
      this.Enemy.transform.parent = this.transform.parent;
      Object.Destroy((Object) this.gameObject);
    }
    else
    {
      this.ScaleSpeed += (float) ((1.0 + 0.10000000149011612 * (double) Mathf.Cos(this.Pulse += 0.1f) - (double) this.Scale) * 0.30000001192092896);
      this.Scale += (this.ScaleSpeed *= 0.8f);
      this.SpawnImage.transform.localScale = new Vector3(this.Scale, this.Scale, this.Scale);
    }
  }
}
