// Decompiled with JetBrains decompiler
// Type: Com.LuisPedroFonseca.ProCamera2D.ProCamera2DPixelPerfectSprite
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace Com.LuisPedroFonseca.ProCamera2D;

[HelpURL("http://www.procamera2d.com/user-guide/extension-pixel-perfect/")]
[ExecuteInEditMode]
public class ProCamera2DPixelPerfectSprite : BasePC2D, IPostMover
{
  public bool IsAMovingObject;
  public bool IsAChildSprite;
  public Vector2 LocalPosition;
  [Range(-8f, 32f)]
  public int SpriteScale;
  public Sprite _sprite;
  public ProCamera2DPixelPerfect _pixelPerfectPlugin;
  [SerializeField]
  public Vector3 _initialScale = Vector3.one;
  public int _prevSpriteScale;
  public int _pmOrder = 2000;

  public override void Awake()
  {
    base.Awake();
    if ((Object) this.ProCamera2D == (Object) null)
    {
      this.enabled = false;
    }
    else
    {
      this.GetPixelPerfectPlugin();
      this.GetSprite();
      this.ProCamera2D.AddPostMover((IPostMover) this);
    }
  }

  public void Start() => this.SetAsPixelPerfect();

  public void PostMove(float deltaTime)
  {
    if (!this.enabled)
      return;
    this.Step();
  }

  public int PMOrder
  {
    get => this._pmOrder;
    set => this._pmOrder = value;
  }

  public void Step()
  {
    if (!this._pixelPerfectPlugin.enabled)
      return;
    if (this.IsAMovingObject)
      this.SetAsPixelPerfect();
    this._prevSpriteScale = this.SpriteScale;
  }

  public void GetPixelPerfectPlugin()
  {
    this._pixelPerfectPlugin = this.ProCamera2D.GetComponent<ProCamera2DPixelPerfect>();
  }

  public void GetSprite()
  {
    SpriteRenderer component = this.GetComponent<SpriteRenderer>();
    if (!((Object) component != (Object) null))
      return;
    this._sprite = component.sprite;
  }

  public void SetAsPixelPerfect()
  {
    if (this.IsAChildSprite)
      this._transform.localPosition = this.VectorHVD(Utils.AlignToGrid(this.LocalPosition.x, this._pixelPerfectPlugin.PixelStep), Utils.AlignToGrid(this.LocalPosition.y, this._pixelPerfectPlugin.PixelStep), this.Vector3D(this._transform.localPosition));
    this._transform.position = this.VectorHVD(Utils.AlignToGrid(this.Vector3H(this._transform.position), this._pixelPerfectPlugin.PixelStep), Utils.AlignToGrid(this.Vector3V(this._transform.position), this._pixelPerfectPlugin.PixelStep), this.Vector3D(this._transform.position));
    if (this.SpriteScale == 0)
    {
      if (this._prevSpriteScale == 0)
        this._initialScale = this._transform.localScale;
      else
        this._transform.localScale = this._initialScale;
    }
    else
    {
      float num = (float) ((double) this._sprite.pixelsPerUnit * (double) (this.SpriteScale < 0 ? (float) (1.0 / (double) this.SpriteScale * -1.0) : (float) this.SpriteScale) * (1.0 / (double) this._pixelPerfectPlugin.PixelsPerUnit));
      this._transform.localScale = new Vector3(Mathf.Sign(this._transform.localScale.x) * num, Mathf.Sign(this._transform.localScale.y) * num, this._transform.localScale.z);
    }
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if (!((Object) this.ProCamera2D != (Object) null))
      return;
    this.ProCamera2D.RemovePostMover((IPostMover) this);
  }
}
