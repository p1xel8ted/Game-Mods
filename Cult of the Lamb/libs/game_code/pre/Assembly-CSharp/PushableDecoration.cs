// Decompiled with JetBrains decompiler
// Type: PushableDecoration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (Rigidbody2D))]
public class PushableDecoration : BaseMonoBehaviour
{
  [SerializeField]
  private bool randomOrientation;
  private const float force = 6f;
  private const float torque = 40f;
  private Rigidbody2D rigidbody;
  private SpriteRenderer spriteRenderer;
  private bool pushed;

  private void Awake()
  {
    this.rigidbody = this.GetComponent<Rigidbody2D>();
    this.spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
    if (!this.randomOrientation)
      return;
    this.transform.eulerAngles = new Vector3(0.0f, 0.0f, (float) Random.Range(0, 360));
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (this.pushed || !(collision.gameObject.tag == "Player"))
      return;
    this.pushed = true;
    float num = (float) ((double) Vector3.Distance(PlayerFarming.Instance.PreviousPosition, PlayerFarming.Instance.transform.position) * 100.0 / 15.0);
    Vector2 toPosition = (Vector2) new Vector3(PlayerFarming.Instance.playerController.xDir, PlayerFarming.Instance.playerController.yDir);
    float angle = Utils.GetAngle((Vector3) Vector2.zero, (Vector3) toPosition);
    this.rigidbody.AddForce((toPosition + Utils.DegreeToVector2(angle + (float) Random.Range(-20, 20))) * 6f * num, ForceMode2D.Impulse);
    this.rigidbody.AddTorque(40f * num, ForceMode2D.Impulse);
    this.spriteRenderer.DOFade(0.0f, 1f).OnComplete<TweenerCore<Color, Color, ColorOptions>>((TweenCallback) (() => this.gameObject.SetActive(false)));
  }
}
