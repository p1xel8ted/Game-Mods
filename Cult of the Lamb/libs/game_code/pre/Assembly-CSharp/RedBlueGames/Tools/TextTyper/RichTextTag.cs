// Decompiled with JetBrains decompiler
// Type: RedBlueGames.Tools.TextTyper.RichTextTag
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace RedBlueGames.Tools.TextTyper;

public class RichTextTag
{
  public static readonly RichTextTag ClearColorTag = new RichTextTag("<color=#00000000>");
  private const char OpeningNodeDelimeter = '<';
  private const char CloseNodeDelimeter = '>';
  private const char EndTagDelimeter = '/';
  private const string ParameterDelimeter = "=";

  public RichTextTag(string tagText) => this.TagText = tagText;

  public string TagText { get; private set; }

  public string ClosingTagText => !this.IsClosingTag ? $"</{this.TagType}>" : this.TagText;

  public string TagType
  {
    get
    {
      string tagType = this.TagText.Substring(1, this.TagText.Length - 2).TrimStart('/');
      int length = tagType.IndexOf("=");
      if (length > 0)
        tagType = tagType.Substring(0, length);
      return tagType;
    }
  }

  public string Parameter
  {
    get
    {
      int num = this.TagText.IndexOf("=");
      if (num < 0)
        return string.Empty;
      int length = this.TagText.Length - num - 2;
      string parameter = this.TagText.Substring(num + 1, length);
      if (parameter.Length > 0 && parameter[0] == '"' && parameter[parameter.Length - 1] == '"')
        parameter = parameter.Substring(1, parameter.Length - 2);
      return parameter;
    }
  }

  public bool IsOpeningTag => !this.IsClosingTag;

  public bool IsClosingTag => this.TagText.Length > 2 && this.TagText[1] == '/';

  public int Length => this.TagText.Length;

  public static bool StringStartsWithTag(string text) => text.StartsWith('<'.ToString());

  public static RichTextTag ParseNext(string text)
  {
    int startIndex = text.IndexOf('<');
    if (startIndex < 0)
      return (RichTextTag) null;
    int num = text.IndexOf('>');
    return num < 0 ? (RichTextTag) null : new RichTextTag(text.Substring(startIndex, num - startIndex + 1));
  }

  public static string RemoveTagsFromString(string text, string tagType)
  {
    string str = text;
    for (int startIndex = 0; startIndex < text.Length; ++startIndex)
    {
      string text1 = text.Substring(startIndex, text.Length - startIndex);
      if (RichTextTag.StringStartsWithTag(text1))
      {
        RichTextTag next = RichTextTag.ParseNext(text1);
        if (next.TagType == tagType)
          str = str.Replace(next.TagText, string.Empty);
        startIndex += next.Length - 1;
      }
    }
    return str;
  }

  public override string ToString() => this.TagText;
}
