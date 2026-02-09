// Decompiled with JetBrains decompiler
// Type: UIProjectileCharge
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UIProjectileCharge : BaseMonoBehaviour
{
  [SerializeField]
  public Image bar;
  [SerializeField]
  public RectTransform target;
  [SerializeField]
  public Vector3 offset;
  public static UIProjectileCharge instance;
  public Camera camera;
  public Canvas canvas;

  public static void Play()
  {
    if (!((Object) UIProjectileCharge.instance == (Object) null))
      return;
    Canvas component = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
    UIProjectileCharge.instance = Object.Instantiate<GameObject>(UnityEngine.Resources.Load<GameObject>("Prefabs/UI/UI Projectile Charge"), component.transform).GetComponent<UIProjectileCharge>();
    UIProjectileCharge.instance.bar.fillAmount = 0.0f;
    UIProjectileCharge.instance.camera = Camera.main;
    UIProjectileCharge.instance.canvas = component;
  }

  public static void Hide()
  {
    if (!((Object) UIProjectileCharge.instance != (Object) null))
      return;
    UIProjectileCharge.instance.bar.fillAmount = 0.0f;
    Object.Destroy((Object) UIProjectileCharge.instance.gameObject);
  }

  public void Update()
  {
    if (!((Object) PlayerFarming.Instance != (Object) null))
      return;
    this.transform.position = this.camera.WorldToScreenPoint(PlayerFarming.Instance.transform.position) + this.offset * this.canvas.scaleFactor;
  }

  public static void UpdateBar(float fillAmount)
  {
    UIProjectileCharge.instance.bar.fillAmount = fillAmount;
  }

  public static bool CorrectRelease()
  {
    return (Object) UIProjectileCharge.instance != (Object) null && (double) UIProjectileCharge.instance.bar.fillAmount >= 0.62000000476837158 && (double) UIProjectileCharge.instance.bar.fillAmount <= 0.800000011920929;
  }
}
