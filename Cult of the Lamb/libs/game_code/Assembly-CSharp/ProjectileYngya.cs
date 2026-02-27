// Decompiled with JetBrains decompiler
// Type: ProjectileYngya
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ProjectileYngya : MonoBehaviour
{
  [SerializeField]
  public SpriteRenderer sprite;
  [SerializeField]
  public DamageCollider damageCollider;
  [SerializeField]
  public float acceleration;
  [SerializeField]
  public float maxSpeed;
  [SerializeField]
  public float delay;
  [SerializeField]
  public float destroyTime;
  public float speed;
  public float time;

  public float Delay
  {
    get => this.delay;
    set => this.delay = value;
  }

  public void OnEnable()
  {
    this.speed = 0.0f;
    this.time = 0.0f;
  }

  public void Update()
  {
    if ((Object) PlayerFarming.Instance == (Object) null)
      return;
    this.time += Time.deltaTime;
    if ((double) this.time < (double) this.delay)
    {
      this.transform.LookAt(PlayerFarming.Instance.transform.position, Vector3.forward);
      this.sprite.color = Color.Lerp(Color.white, Color.red, this.time / this.delay);
    }
    else
    {
      this.speed = Mathf.Clamp(this.speed + this.acceleration * Time.deltaTime, 0.0f, this.maxSpeed);
      this.transform.position += this.transform.forward * this.speed;
    }
    if ((double) this.time <= (double) this.destroyTime)
      return;
    this.gameObject.Recycle();
  }
}
