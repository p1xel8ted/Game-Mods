// Decompiled with JetBrains decompiler
// Type: BonusStatueDevotionDamage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class BonusStatueDevotionDamage : BaseMonoBehaviour
{
  public CameraInclude cameraInclude;
  public Transform DevotionPosition;
  public bool DealDamage = true;
  public int DevotionToGivePerSoul = 2;
  public float SoulsToGive = 30f;
  public bool Activated;
  public Health health;
  public int HP = 3;
  public List<Transform> ShakeTransforms;
  public Vector2[] Shake = new Vector2[0];

  public void OnEnable()
  {
    this.health = this.GetComponent<Health>();
    this.health.OnHit += new Health.HitAction(this.Health_OnHit);
    this.Shake = new Vector2[this.ShakeTransforms.Count];
  }

  public void OnDisable() => this.health.OnHit -= new Health.HitAction(this.Health_OnHit);

  public void Health_OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    Health component = Attacker.GetComponent<Health>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null || this.Activated)
      return;
    --this.HP;
    this.ShakeImages();
    this.HideImages();
    if (this.HP <= 0)
    {
      this.Activated = true;
      this.StartCoroutine((IEnumerator) this.GiveDevotionRoutine(component, Attacker, true));
    }
    else
      this.StartCoroutine((IEnumerator) this.GiveDevotionRoutine(component, Attacker, false));
  }

  public void Update()
  {
    int index = -1;
    while (++index < this.Shake.Length)
    {
      if ((UnityEngine.Object) this.ShakeTransforms[index] != (UnityEngine.Object) this.transform)
      {
        this.Shake[index].y += (float) ((0.0 - (double) this.Shake[index].x) * 0.20000000298023224);
        this.Shake[index].x += (float) ((double) (this.Shake[index].y *= 0.9f) * (double) Time.deltaTime * 60.0);
        this.ShakeTransforms[index].localPosition = new Vector3(this.Shake[index].x, this.ShakeTransforms[index].localPosition.y, this.ShakeTransforms[index].localPosition.z);
      }
    }
  }

  public void TestImages(int HP)
  {
    this.HP = HP;
    this.HideImages();
  }

  public void HideImages()
  {
    int index = -1;
    while (++index < this.ShakeTransforms.Count)
      this.ShakeTransforms[index].gameObject.SetActive((double) index >= (1.0 - (double) this.HP / 3.0) * (double) this.ShakeTransforms.Count);
  }

  public void ShakeImages()
  {
    int index = -1;
    while (++index < this.Shake.Length)
      this.Shake[index] = new Vector2(UnityEngine.Random.Range(0.1f, 0.2f) * ((double) UnityEngine.Random.value < 0.5 ? -1f : 1f), 0.0f);
  }

  public IEnumerator GiveDevotionRoutine(
    Health AttackerHealth,
    GameObject Target,
    bool GiveDevotion)
  {
    BonusStatueDevotionDamage statueDevotionDamage = this;
    if (statueDevotionDamage.DealDamage)
      AttackerHealth.DealDamage(1f, statueDevotionDamage.gameObject, Vector3.Lerp(statueDevotionDamage.transform.position, AttackerHealth.transform.position, 0.8f));
    if (GiveDevotion)
    {
      statueDevotionDamage.health.enabled = false;
      yield return (object) new WaitForSeconds(0.5f);
      statueDevotionDamage.cameraInclude.enabled = false;
      float NumSouls = 0.0f;
      while ((double) ++NumSouls <= (double) statueDevotionDamage.SoulsToGive && !((UnityEngine.Object) Target == (UnityEngine.Object) null))
      {
        if ((!GameManager.HasUnlockAvailable() ? 0 : (!DataManager.Instance.DeathCatBeaten ? 1 : 0)) != 0)
          SoulCustomTarget.Create(Target, statueDevotionDamage.DevotionPosition.position, Color.white, new System.Action(statueDevotionDamage.\u003CGiveDevotionRoutine\u003Eb__17_0));
        else
          InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, statueDevotionDamage.transform.position + Vector3.back, 0.0f).SetInitialSpeedAndDiraction(8f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
        yield return (object) new WaitForSeconds((float) (0.20000000298023224 - 0.20000000298023224 * ((double) NumSouls / (double) statueDevotionDamage.SoulsToGive)));
      }
    }
  }

  [CompilerGenerated]
  public void \u003CGiveDevotionRoutine\u003Eb__17_0()
  {
    Inventory.AddItem(10, this.DevotionToGivePerSoul);
  }
}
