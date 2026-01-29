// Decompiled with JetBrains decompiler
// Type: MMJsonDataReadWriter`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Data.Serialization;
using FilepathUtils;
using MessagePack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Unify;
using UnityEngine;

#nullable disable
public class MMJsonDataReadWriter<T> : MMDataReadWriterBase<T>
{
  public const string kSaveDirectory = "saves";
  public const string kBackupDirectory = "backup";
  public const int kBackupLimit = 10;
  public object _threadLock = new object();
  public static string _persistentPath;

  public override void Write(T data, string filename, bool encrypt = true, bool backup = true)
  {
    Directory.CreateDirectory(this.GetDirectory());
    if (!this.SavesAsJson(filename))
      this.WriteMessagePack(data, filename, encrypt, backup);
    else
      this.WriteJson(data, filename, encrypt, backup);
  }

  public void WriteMessagePack(T data, string filename, bool encrypt, bool backup)
  {
    string filePath = this.GetFilepath(filename, true);
    Debug.Log((object) ("MMJsonDataReadWriter - Write MP File " + filePath).Colour(Color.yellow));
    new Thread((ThreadStart) (() =>
    {
      lock (this._threadLock)
      {
        try
        {
          this.SerializeResult(MessagePackSerializer.Serialize<T>(data, MPSerialization.options), filePath, encrypt, backup, (System.Action) (() => UnifyComponent.Instance.MainThreadEnqueue((System.Action) (() =>
          {
            System.Action onWriteCompleted = this.OnWriteCompleted;
            if (onWriteCompleted == null)
              return;
            onWriteCompleted();
          }))));
        }
        catch (Exception ex)
        {
          Debug.Log((object) ex.Message.Colour(Color.red));
          UnifyComponent.Instance.MainThreadEnqueue((System.Action) (() =>
          {
            Action<MMReadWriteError> onWriteError = this.OnWriteError;
            if (onWriteError == null)
              return;
            onWriteError(new MMReadWriteError(ex.Message));
          }));
        }
      }
    }))
    {
      Name = "MMJsonDataReadWriter Write Thread"
    }.Start();
  }

  public void WriteJson(T data, string filename, bool encrypt, bool backup)
  {
    string filepath = this.GetFilepath(filename, false);
    Debug.Log((object) ("MMJsonDataReadWriter - Write JSON File " + filepath).Colour(Color.yellow));
    try
    {
      this.SerializeResult(MMJsonDataReadWriter<T>.SerializeJsonToBytes(data), filepath, encrypt, backup, (System.Action) (() =>
      {
        System.Action onWriteCompleted = this.OnWriteCompleted;
        if (onWriteCompleted == null)
          return;
        onWriteCompleted();
      }));
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex.Message.Colour(Color.red));
      Action<MMReadWriteError> onWriteError = this.OnWriteError;
      if (onWriteError == null)
        return;
      onWriteError(new MMReadWriteError(ex.Message));
    }
  }

  public override void Read(string filename)
  {
    Debug.Log((object) "MMJsonDataReadWriter - Read File".Colour(Color.yellow));
    string filepath1 = this.GetFilepath(filename, true);
    string filepath2 = this.GetFilepath(filename, false);
    FileStream _unusedFs = (FileStream) null;
    Aes _unusedAes = (Aes) null;
    CryptoStream _unusedCrypto = (CryptoStream) null;
    StreamReader _unusedSr = (StreamReader) null;
    JsonTextReader _unusedJr = (JsonTextReader) null;
    try
    {
      if (File.Exists(filepath2) || File.Exists(filepath1))
      {
        T objectData;
        if (!this.SavesAsJson(filename) && File.Exists(filepath1) && this.MessagePackRead(filename, filepath1, out _unusedFs, ref _unusedAes, ref _unusedCrypto, out objectData) || !File.Exists(filepath2) || !this.JsonRead(filename, filepath2, out _unusedFs, ref _unusedAes, ref _unusedCrypto, out _unusedSr, ref _unusedJr, out objectData))
          ;
      }
      else
      {
        Debug.Log((object) "MMJsonDataReadWriter - Read File - File did not exist, creating new".Colour(Color.cyan));
        Directory.CreateDirectory(this.GetDirectory());
        Directory.CreateDirectory(this.GetBackupDirectory());
        System.Action onCreateDefault = this.OnCreateDefault;
        if (onCreateDefault == null)
          return;
        onCreateDefault();
      }
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex.Message.Colour(Color.red));
      Debug.LogException((Exception) ex);
      this.ScrubBackups(filename);
    }
    finally
    {
      _unusedFs?.Dispose();
      _unusedAes?.Dispose();
      _unusedCrypto?.Dispose();
      _unusedSr?.Dispose();
      _unusedJr?.Close();
    }
  }

  public bool JsonRead(
    string filename,
    string jsonPath,
    out FileStream _unusedFs,
    ref Aes _unusedAes,
    ref CryptoStream _unusedCrypto,
    out StreamReader _unusedSr,
    ref JsonTextReader _unusedJr,
    out T objectData)
  {
    _unusedFs = (FileStream) null;
    _unusedSr = (StreamReader) null;
    try
    {
      byte[] bytes = this.ReadBytes(jsonPath);
      objectData = MMJsonDataReadWriter<T>.DeserializeJsonFromBytes(bytes);
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex.Message.Colour(Color.red));
      objectData = default (T);
    }
    if (EqualityComparer<T>.Default.Equals(objectData, default (T)) || (object) objectData == null)
      return this.ScrubBackups(filename);
    Action<T> onReadCompleted = this.OnReadCompleted;
    if (onReadCompleted != null)
      onReadCompleted(objectData);
    return true;
  }

  public bool MessagePackRead(
    string filename,
    string msgpackPath,
    out FileStream _unusedFs,
    ref Aes _unusedAes,
    ref CryptoStream _unusedCrypto,
    out T objectData)
  {
    _unusedFs = (FileStream) null;
    try
    {
      byte[] numArray = this.ReadBytes(msgpackPath);
      objectData = MessagePackSerializer.Deserialize<T>(ReadOnlyMemory<byte>.op_Implicit(numArray), MPSerialization.options);
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex.Message.Colour(Color.red));
      objectData = default (T);
    }
    if (EqualityComparer<T>.Default.Equals(objectData, default (T)) || (object) objectData == null)
      return this.ScrubBackups(filename);
    Action<T> onReadCompleted = this.OnReadCompleted;
    if (onReadCompleted != null)
      onReadCompleted(objectData);
    return true;
  }

  public override void Delete(string filename)
  {
    Debug.Log((object) ("UnifyDataReadWriter - Delete File " + filename).Colour(Color.yellow));
    string filepath1 = this.GetFilepath(filename, true);
    string filepath2 = this.GetFilepath(filename, false);
    bool flag = false;
    if (File.Exists(filepath2))
    {
      File.Delete(filepath2);
      Debug.Log((object) ("UnifyDataReadWriter - Deletion Successful " + filepath2).Colour(Color.yellow));
      flag = true;
    }
    if (File.Exists(filepath1))
    {
      File.Delete(filepath1);
      Debug.Log((object) ("UnifyDataReadWriter - Deletion Successful " + filepath1).Colour(Color.yellow));
      flag = true;
    }
    if (!flag)
      return;
    foreach (string backup in this.GetBackups(filename))
      File.Delete(this.GetBackupFilepath(backup));
    System.Action deletionComplete = this.OnDeletionComplete;
    if (deletionComplete == null)
      return;
    deletionComplete();
  }

  public bool ScrubBackups(string filename)
  {
    List<string> backups = this.GetBackups(filename.NormalizePath().Split(Path.DirectorySeparatorChar, StringSplitOptions.None).LastElement<string>());
    backups.Reverse();
    for (int index = 0; index < backups.Count; ++index)
    {
      string filename1 = backups[index];
      string backupFilepath = this.GetBackupFilepath(filename1);
      try
      {
        T data;
        using (FileStream fileStream = new FileStream(backupFilepath, FileMode.Open))
        {
          int num = (int) (ushort) fileStream.ReadByte();
          fileStream.Position = 0L;
          if (num == 69)
          {
            byte[] numArray1 = new byte[16 /*0x10*/];
            fileStream.Read(numArray1, 0, numArray1.Length);
            using (Aes aes = Aes.Create())
            {
              byte[] numArray2 = new byte[aes.IV.Length];
              fileStream.Read(numArray2, 0, numArray2.Length);
              using (CryptoStream cryptoStream = new CryptoStream((Stream) fileStream, aes.CreateDecryptor(numArray1, numArray2), CryptoStreamMode.Read))
              {
                if (filename1.EndsWith(".mp"))
                {
                  data = MessagePackSerializer.Deserialize<T>((Stream) cryptoStream, MPSerialization.options);
                }
                else
                {
                  using (StreamReader streamReader = new StreamReader((Stream) cryptoStream))
                    data = JsonConvert.DeserializeObject<T>(streamReader.ReadToEnd(), MMSerialization.JsonSerializerSettings);
                }
              }
            }
          }
          else if (filename1.EndsWith(".mp"))
          {
            data = MessagePackSerializer.Deserialize<T>((Stream) fileStream, MPSerialization.options);
          }
          else
          {
            using (StreamReader reader1 = new StreamReader((Stream) fileStream))
            {
              using (JsonTextReader reader2 = new JsonTextReader((TextReader) reader1))
                data = MMSerialization.JsonSerializer.Deserialize<T>((JsonReader) reader2);
            }
          }
        }
        Debug.Log((object) ("MMJsonDataReadWriter - Recovered from backup: " + filename1).Colour(Color.green));
        this.Write(data, filename, true, true);
        Action<T> onReadCompleted = this.OnReadCompleted;
        if (onReadCompleted != null)
          onReadCompleted(data);
        return true;
      }
      catch (Exception ex)
      {
        Debug.Log((object) $"Backup failed: {filename1} � {ex.Message}".Colour(Color.red));
      }
    }
    Debug.Log((object) "MMJsonDataReadWriter - All backups failed.".Colour(Color.red));
    Action<MMReadWriteError> onReadError = this.OnReadError;
    if (onReadError != null)
      onReadError(new MMReadWriteError("All backups are corrupted"));
    return false;
  }

  public override bool FileExists(string filename)
  {
    string filepath1 = this.GetFilepath(filename, false);
    string filepath2 = this.GetFilepath(filename, true);
    if (this.SavesAsJson(filename))
    {
      Debug.Log((object) $"MMJsonDataReadWriter - {filepath1} File".Colour(Color.yellow));
      return File.Exists(filepath1);
    }
    if (File.Exists(filepath2))
    {
      Debug.Log((object) $"MMJsonDataReadWriter - {filepath2} File Exists".Colour(Color.green));
      return true;
    }
    Debug.Log((object) $"MMJsonDataReadWriter - {filepath2} File does not Exist".Colour(Color.yellow));
    if (File.Exists(filepath1))
    {
      Debug.Log((object) $"MMJsonDataReadWriter - {filepath1} File Exists".Colour(Color.green));
      return true;
    }
    Debug.Log((object) $"MMJsonDataReadWriter - {filepath1} File does not Exist".Colour(Color.yellow));
    return false;
  }

  public void MakeBackup(string filename, string fullfilepath)
  {
    if (!File.Exists(fullfilepath))
      return;
    string backupDirectory = this.GetBackupDirectory();
    if (!Directory.Exists(backupDirectory))
      Directory.CreateDirectory(backupDirectory);
    List<string> backups = this.GetBackups(filename);
    foreach (string filename1 in backups.ToArray())
    {
      if (filename1.Split('.', StringSplitOptions.None).Length != 3)
      {
        File.Delete(this.GetBackupFilepath(filename1));
        backups.Remove(filename1);
      }
    }
    if (backups.Count >= 10)
      File.Delete(this.GetBackupFilepath(backups[0]));
    string str = string.Empty;
    if (backups.Count > 0)
    {
      string[] strArray = backups.LastElement<string>().Split('.', StringSplitOptions.None);
      int result;
      if (strArray.Length == 3 && int.TryParse(strArray[1], out result))
      {
        ++result;
        str = $"{strArray[0]}.{result.ToString()}";
      }
    }
    else
      str = filename.Split('.', StringSplitOptions.None)[0] + ".1";
    string filename2 = str + Path.GetExtension(fullfilepath);
    File.Copy(fullfilepath, this.GetBackupFilepath(filename2), true);
  }

  public string GetMostRecentBackup(string filename)
  {
    List<string> backups = this.GetBackups(filename);
    return backups.Count > 0 ? backups.LastElement<string>() : string.Empty;
  }

  public List<string> GetBackups(string filename)
  {
    string[] strArray = filename.Split('.', StringSplitOptions.None);
    List<string> backups = new List<string>();
    foreach (FileInfo fileInfo in (IEnumerable<FileInfo>) ((IEnumerable<FileInfo>) new DirectoryInfo(this.GetBackupDirectory()).GetFiles()).OrderBy<FileInfo, DateTime>((Func<FileInfo, DateTime>) (f => f.LastWriteTime)))
    {
      if (strArray.Length != 0 && fileInfo.Name.Contains(strArray[0]) && (fileInfo.Extension == ".json" || fileInfo.Extension == ".mp"))
        backups.Add(fileInfo.Name);
    }
    return backups;
  }

  public void SerializeResult(
    byte[] payload,
    string filepath,
    bool encrypt,
    bool backup,
    System.Action onSuccess = null)
  {
    Directory.CreateDirectory(this.GetDirectory());
    bool flag = true;
    try
    {
      if (backup)
        this.MakeBackup(Path.GetFileName(filepath), filepath);
      using (FileStream fileStream = new FileStream(filepath, FileMode.Create, FileAccess.Write, FileShare.None))
      {
        if (encrypt)
        {
          fileStream.WriteByte((byte) 69);
          byte[] numArray = new byte[16 /*0x10*/];
          using (RNGCryptoServiceProvider cryptoServiceProvider = new RNGCryptoServiceProvider())
            cryptoServiceProvider.GetBytes(numArray);
          fileStream.Write(numArray, 0, numArray.Length);
          using (Aes aes = Aes.Create())
          {
            fileStream.Write(aes.IV, 0, aes.IV.Length);
            using (CryptoStream cryptoStream = new CryptoStream((Stream) fileStream, aes.CreateEncryptor(numArray, aes.IV), CryptoStreamMode.Write))
              cryptoStream.Write(payload, 0, payload.Length);
          }
        }
        else
          fileStream.Write(payload, 0, payload.Length);
      }
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex.Message.Colour(Color.red));
      Action<MMReadWriteError> onWriteError = this.OnWriteError;
      if (onWriteError != null)
        onWriteError(new MMReadWriteError(ex.Message));
      flag = false;
    }
    finally
    {
      if (!flag & backup)
      {
        string backupFilepath = this.GetBackupFilepath(this.GetMostRecentBackup(Path.GetFileName(filepath)));
        if (File.Exists(backupFilepath))
          File.Copy(backupFilepath, filepath, true);
      }
      else if (backup)
        this.MakeBackup(Path.GetFileName(filepath), filepath);
      if (flag && onSuccess != null)
        onSuccess();
    }
  }

  public byte[] ReadBytes(string filepath)
  {
    using (FileStream fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read))
    {
      int num1 = fileStream.ReadByte();
      if (num1 == -1)
        throw new IOException("Empty file");
      if ((ushort) num1 == (ushort) 69)
      {
        byte[] numArray1 = new byte[16 /*0x10*/];
        fileStream.Read(numArray1, 0, numArray1.Length);
        using (Aes aes = Aes.Create())
        {
          byte[] numArray2 = new byte[aes.IV.Length];
          int length = numArray2.Length;
          int offset = 0;
          int num2;
          for (; length > 0; length -= num2)
          {
            num2 = fileStream.Read(numArray2, offset, length);
            if (num2 != 0)
              offset += num2;
            else
              break;
          }
          using (CryptoStream cryptoStream = new CryptoStream((Stream) fileStream, aes.CreateDecryptor(numArray1, numArray2), CryptoStreamMode.Read))
          {
            using (MemoryStream destination = new MemoryStream())
            {
              cryptoStream.CopyTo((Stream) destination);
              return destination.ToArray();
            }
          }
        }
      }
      using (MemoryStream destination = new MemoryStream())
      {
        destination.WriteByte((byte) num1);
        fileStream.CopyTo((Stream) destination);
        return destination.ToArray();
      }
    }
  }

  public static byte[] SerializeJsonToBytes(T data)
  {
    using (MemoryStream memoryStream = new MemoryStream())
    {
      using (StreamWriter streamWriter = new StreamWriter((Stream) memoryStream, (Encoding) new UTF8Encoding(false), 1024 /*0x0400*/, true))
      {
        using (JsonTextWriter jsonTextWriter = new JsonTextWriter((TextWriter) streamWriter))
        {
          MMSerialization.JsonSerializer.Serialize((JsonWriter) jsonTextWriter, (object) data);
          jsonTextWriter.Flush();
          streamWriter.Flush();
          return memoryStream.ToArray();
        }
      }
    }
  }

  public static T DeserializeJsonFromBytes(byte[] bytes)
  {
    using (MemoryStream memoryStream = new MemoryStream(bytes))
    {
      using (StreamReader reader1 = new StreamReader((Stream) memoryStream, Encoding.UTF8))
      {
        using (JsonTextReader reader2 = new JsonTextReader((TextReader) reader1))
          return MMSerialization.JsonSerializer.Deserialize<T>((JsonReader) reader2);
      }
    }
  }

  public static string PersistentPath
  {
    get
    {
      return MMJsonDataReadWriter<T>._persistentPath ?? (MMJsonDataReadWriter<T>._persistentPath = Application.persistentDataPath);
    }
  }

  public string GetDirectory() => Path.Combine(MMJsonDataReadWriter<T>.PersistentPath, "saves");

  public string GetFilepath(string filename, bool useMessagePack = false)
  {
    string extension = useMessagePack ? ".mp" : ".json";
    return Path.Combine(this.GetDirectory(), Path.ChangeExtension(filename, extension));
  }

  public string GetFilepath(string filename) => Path.Combine(this.GetDirectory(), filename);

  public string GetBackupDirectory()
  {
    return Path.Combine(MMJsonDataReadWriter<T>.PersistentPath, "backup");
  }

  public string GetBackupFilepath(string filename)
  {
    return Path.Combine(this.GetBackupDirectory(), filename);
  }

  [CompilerGenerated]
  public void \u003CWriteJson\u003Eb__6_0()
  {
    System.Action onWriteCompleted = this.OnWriteCompleted;
    if (onWriteCompleted == null)
      return;
    onWriteCompleted();
  }
}
