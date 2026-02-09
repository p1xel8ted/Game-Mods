// Decompiled with JetBrains decompiler
// Type: Expressive.Token
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Runtime.CompilerServices;

#nullable disable
namespace Expressive;

public sealed class Token
{
  [CompilerGenerated]
  public string \u003CCurrentToken\u003Ek__BackingField;
  [CompilerGenerated]
  public int \u003CLength\u003Ek__BackingField;
  [CompilerGenerated]
  public int \u003CStartIndex\u003Ek__BackingField;

  public string CurrentToken
  {
    get => this.\u003CCurrentToken\u003Ek__BackingField;
    set => this.\u003CCurrentToken\u003Ek__BackingField = value;
  }

  public int Length
  {
    get => this.\u003CLength\u003Ek__BackingField;
    set => this.\u003CLength\u003Ek__BackingField = value;
  }

  public int StartIndex
  {
    get => this.\u003CStartIndex\u003Ek__BackingField;
    set => this.\u003CStartIndex\u003Ek__BackingField = value;
  }

  public Token(string currentToken, int startIndex)
  {
    this.CurrentToken = currentToken;
    this.StartIndex = startIndex;
    this.Length = this.CurrentToken == null ? 0 : this.CurrentToken.Length;
  }
}
