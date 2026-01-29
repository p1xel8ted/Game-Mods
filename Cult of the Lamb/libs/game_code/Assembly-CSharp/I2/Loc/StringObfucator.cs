// Decompiled with JetBrains decompiler
// Type: I2.Loc.StringObfucator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Text;

#nullable disable
namespace I2.Loc;

public class StringObfucator
{
  public static char[] StringObfuscatorPassword = "ÝúbUu¸CÁÂ§*4PÚ©-á©\u00BE@T6Dl±ÒWâuzÅm4GÐóØ$=Íg,¥Që®iKEßr¡×60Ít4öÃ~^«y:Èd1<QÛÝúbUu¸CÁÂ§*4PÚ©-á©\u00BE@T6Dl±ÒWâuzÅm4GÐóØ$=Íg,¥Që®iKEßr¡×60Ít4öÃ~^«y:Èd".ToCharArray();

  public static string Encode(string NormalString)
  {
    try
    {
      return StringObfucator.ToBase64(StringObfucator.XoREncode(NormalString));
    }
    catch (Exception ex)
    {
      return (string) null;
    }
  }

  public static string Decode(string ObfucatedString)
  {
    try
    {
      return StringObfucator.XoREncode(StringObfucator.FromBase64(ObfucatedString));
    }
    catch (Exception ex)
    {
      return (string) null;
    }
  }

  public static string ToBase64(string regularString)
  {
    return Convert.ToBase64String(Encoding.UTF8.GetBytes(regularString));
  }

  public static string FromBase64(string base64string)
  {
    byte[] bytes = Convert.FromBase64String(base64string);
    return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
  }

  public static string XoREncode(string NormalString)
  {
    try
    {
      char[] obfuscatorPassword = StringObfucator.StringObfuscatorPassword;
      char[] charArray = NormalString.ToCharArray();
      int length1 = obfuscatorPassword.Length;
      int index = 0;
      for (int length2 = charArray.Length; index < length2; ++index)
        charArray[index] = (char) ((int) charArray[index] ^ (int) obfuscatorPassword[index % length1] ^ (index % 2 == 0 ? (int) (byte) (index * 23) : (int) (byte) (-index * 51)));
      return new string(charArray);
    }
    catch (Exception ex)
    {
      return (string) null;
    }
  }
}
