// Decompiled with JetBrains decompiler
// Type: PushableDecoration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (Rigidbody2D))]
public class PushableDecoration : BaseMonoBehaviour
{
  [SerializeField]
  public bool randomOrientation;
  public const float force = 6f;
  public const float torque = 40f;
  public Rigidbody2D rigidbody;
  public SpriteRenderer spriteRenderer;
  public bool pushed;

  public void Awake()
  {
    this.rigidbody = this.GetComponent<Rigidbody2D>();
    this.spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
    if (!this.randomOrientation)
      return;
    this.transform.eulerAngles = new Vector3(0.0f, 0.0f, (float) Random.Range(0, 360));
  }

  public void OnTriggerEnter2D(Collider2D collision)
  {
    if (this.pushed || !collision.gameObject.CompareTag("Player"))
      return;
    this.pushed = true;
    float num = (float) ((double) Vector3.Distance(PlayerFarming.Instance.PreviousPosition, PlayerFarming.Instance.transform.position) * 100.0 / 15.0);
    Vector2 toPosition = (Vector2) new Vector3(PlayerFarming.Instance.playerController.xDir, PlayerFarming.Instance.playerController.yDir);
    float angle = Utils.GetAngle((Vector3) Vector2.zero, (Vector3) toPosition);
    this.rigidbody.AddForce((toPosition + Utils.DegreeToVector2(angle + (float) Random.Range(-20, 20))) * 6f * num, ForceMode2D.Impulse);
    this.rigidbody.AddTorque(40f * num, ForceMode2D.Impulse);
    this.spriteRenderer.DOFade(0.0f, 1f).OnComplete<TweenerCore<Color, Color, ColorOptions>>((TweenCallback) (() => this.gameObject.SetActive(false)));
  }

  [CompilerGenerated]
  public void \u003COnTriggerEnter2D\u003Eb__7_0() => this.gameObject.SetActive(false);
}
