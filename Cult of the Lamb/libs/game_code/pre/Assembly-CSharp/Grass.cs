// Decompiled with JetBrains decompiler
// Type: Grass
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Grass : BaseMonoBehaviour
{
  private float rotateSpeedY;
  private float rotateY;
  public GameObject image;
  public float RotationToCamera = -90f;
  private Health health;
  public Sprite[] grassSprites;

  private void Start()
  {
    this.health = this.GetComponent<Health>();
    if ((Object) this.health != (Object) null)
      this.health.OnDie += new Health.DieAction(this.OnDie);
    if (this.grassSprites.Length == 0)
      return;
    int index = Random.Range(0, this.grassSprites.Length);
    this.image.GetComponent<SpriteRenderer>().sprite = this.grassSprites[index];
  }

  private void OnDisable()
  {
    if (!((Object) this.health != (Object) null))
      return;
    this.health.OnDie -= new Health.DieAction(this.OnDie);
  }

  private void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if (!((Object) DungeonDecorator.getInsance() != (Object) null))
      return;
    DungeonDecorator.getInsance().UpdateStructures(NavigateRooms.r, this.transform.position, 0);
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (!(collision.tag == "Player"))
      return;
    this.rotateSpeedY = (float) ((10.0 + (double) Random.Range(-2, 2)) * ((double) collision.transform.position.x < (double) this.transform.position.x ? -1.0 : 1.0));
  }

  private void OnTriggerExit2D(Collider2D collision)
  {
    if (!(collision.tag == "Player") || !((Object) collision.gameObject != (Object) null))
      return;
    this.rotateSpeedY = (float) ((10.0 + (double) Random.Range(-2, 2)) * ((double) collision.transform.position.x < (double) this.transform.position.x ? -1.0 : 1.0));
  }

  private void Update()
  {
    this.rotateSpeedY += (float) ((0.0 - (double) this.rotateY) * 0.10000000149011612) * GameManager.DeltaTime;
    this.rotateY += (this.rotateSpeedY *= 0.8f) * GameManager.DeltaTime;
    if (!((Object) this.image != (Object) null))
      return;
    this.image.transform.eulerAngles = new Vector3(this.RotationToCamera, this.rotateY, 0.0f);
  }
}
