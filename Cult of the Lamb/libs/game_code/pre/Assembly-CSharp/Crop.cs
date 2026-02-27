// Decompiled with JetBrains decompiler
// Type: Crop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Crop : BaseMonoBehaviour
{
  public static List<Crop> Crops = new List<Crop>();
  public Sprite[] sprites;
  public SpriteRenderer image;
  private float rotateSpeedY;
  private float rotateY;
  private float delay;
  private float WorkRequired = 5f;
  private Health health;
  [HideInInspector]
  public bool WorkCompleted;
  [HideInInspector]
  public GameObject Reserved;
  private StructuresData CropInfo;
  private Animator animator;
  public AnimationClip a;
  private float progress;

  private void Start()
  {
    this.animator = this.GetComponentInChildren<Animator>();
    this.CropInfo = this.GetComponent<Structure>().Structure_Info;
    this.SetImage();
  }

  private void OnEnable() => Crop.Crops.Add(this);

  private void OnDisable() => Crop.Crops.Remove(this);

  public void DoWork(float WorkDone)
  {
    this.CropInfo.Progress += WorkDone;
    if ((double) this.CropInfo.Progress >= (double) this.WorkRequired)
    {
      this.WorkCompleted = true;
      this.CropInfo.Progress = this.WorkRequired;
    }
    this.SetImage();
  }

  private void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
  }

  private float Progress
  {
    get => this.progress;
    set
    {
      this.progress = value;
      if ((double) this.progress < 1.0)
        return;
      this.progress = 1f;
    }
  }

  public void SetImage()
  {
    this.Progress = this.CropInfo.Progress / this.WorkRequired;
    this.animator.speed = 0.0f;
    this.animator.Play("CropGrow", 0, this.Progress);
    if ((double) this.Progress < 1.0 || !((Object) this.gameObject.GetComponent<Health>() == (Object) null))
      return;
    this.health = this.gameObject.AddComponent<Health>();
    this.health.OnDie += new Health.DieAction(this.OnDie);
    this.GetComponent<DropLootOnDeath>().SetHealth();
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if ((double) this.delay >= 0.0)
      return;
    this.delay = 0.5f;
    this.rotateSpeedY = (float) ((10.0 + (double) Random.Range(-2, 2)) * ((double) collision.transform.position.x < (double) this.transform.position.x ? -1.0 : 1.0));
  }

  private void OnTriggerExit2D(Collider2D collision)
  {
    if ((double) this.delay >= 0.0)
      return;
    this.delay = 0.5f;
    this.rotateSpeedY = (float) ((10.0 + (double) Random.Range(-2, 2)) * ((double) collision.transform.position.x < (double) this.transform.position.x ? -1.0 : 1.0));
  }

  private void Update()
  {
    this.delay -= Time.deltaTime;
    this.rotateSpeedY += (float) ((0.0 - (double) this.rotateY) * 0.10000000149011612);
    this.rotateY += (this.rotateSpeedY *= 0.8f);
    this.image.gameObject.transform.eulerAngles = new Vector3(-90f, this.rotateY, 0.0f);
  }

  private void onDestroy()
  {
    if (!((Object) this.health != (Object) null))
      return;
    this.health.OnDie -= new Health.DieAction(this.OnDie);
  }
}
