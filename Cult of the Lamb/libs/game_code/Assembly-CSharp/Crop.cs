// Decompiled with JetBrains decompiler
// Type: Crop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Crop : BaseMonoBehaviour
{
  public static List<Crop> Crops = new List<Crop>();
  public Sprite[] sprites;
  public SpriteRenderer image;
  public float rotateSpeedY;
  public float rotateY;
  public float delay;
  public float WorkRequired = 5f;
  public Health health;
  [HideInInspector]
  public bool WorkCompleted;
  [HideInInspector]
  public GameObject Reserved;
  public StructuresData CropInfo;
  public Animator animator;
  public AnimationClip a;
  public float progress;

  public void Start()
  {
    this.animator = this.GetComponentInChildren<Animator>();
    this.CropInfo = this.GetComponent<Structure>().Structure_Info;
    this.SetImage();
  }

  public void OnEnable() => Crop.Crops.Add(this);

  public void OnDisable() => Crop.Crops.Remove(this);

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

  public void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
  }

  public float Progress
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

  public void OnTriggerEnter2D(Collider2D collision)
  {
    if ((double) this.delay >= 0.0)
      return;
    this.delay = 0.5f;
    this.rotateSpeedY = (float) ((10.0 + (double) Random.Range(-2, 2)) * ((double) collision.transform.position.x < (double) this.transform.position.x ? -1.0 : 1.0));
  }

  public void OnTriggerExit2D(Collider2D collision)
  {
    if ((double) this.delay >= 0.0)
      return;
    this.delay = 0.5f;
    this.rotateSpeedY = (float) ((10.0 + (double) Random.Range(-2, 2)) * ((double) collision.transform.position.x < (double) this.transform.position.x ? -1.0 : 1.0));
  }

  public void Update()
  {
    this.delay -= Time.deltaTime;
    this.rotateSpeedY += (float) ((0.0 - (double) this.rotateY) * 0.10000000149011612);
    this.rotateY += (this.rotateSpeedY *= 0.8f);
    this.image.gameObject.transform.eulerAngles = new Vector3(-90f, this.rotateY, 0.0f);
  }

  public void onDestroy()
  {
    if (!((Object) this.health != (Object) null))
      return;
    this.health.OnDie -= new Health.DieAction(this.OnDie);
  }
}
