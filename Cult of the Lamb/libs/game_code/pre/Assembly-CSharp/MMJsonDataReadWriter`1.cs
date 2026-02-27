// Decompiled with JetBrains decompiler
// Type: MMJsonDataReadWriter`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Data.Serialization;
using FilepathUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

#nullable disable
public class MMJsonDataReadWriter<T> : MMDataReadWriterBase<T>
{
  private const string kSaveDirectory = "saves";
  private const string kBackupDirectory = "backup";
  private const int kBackupLimit = 10;

  public override void Write(T data, string filename, bool encrypt = true, bool backup = true)
  {
    bool flag = true;
    string filepath = this.GetFilepath(filename);
    Debug.Log((object) ("MMJsonDataReadWriter - Write File " + filepath).Colour(Color.yellow));
    if (backup)
      this.MakeBackup(filename);
    FileStream fileStream = (FileStream) null;
    RNGCryptoServiceProvider cryptoServiceProvider = (RNGCryptoServiceProvider) null;
    Aes aes = (Aes) null;
    CryptoStream cryptoStream = (CryptoStream) null;
    StreamWriter streamWriter = (StreamWriter) null;
    try
    {
      if (encrypt)
      {
        using (fileStream = new FileStream(filepath, FileMode.Create))
        {
          fileStream.WriteByte((byte) 69);
          byte[] numArray = new byte[16 /*0x10*/];
          using (cryptoServiceProvider = new RNGCryptoServiceProvider())
            cryptoServiceProvider.GetBytes(numArray);
          fileStream.Write(numArray, 0, numArray.Length);
          using (aes = Aes.Create())
          {
            aes.Key = numArray;
            byte[] iv = aes.IV;
            fileStream.Write(iv, 0, iv.Length);
            using (cryptoStream = new CryptoStream((Stream) fileStream, aes.CreateEncryptor(aes.Key, aes.IV), CryptoStreamMode.Write))
            {
              using (streamWriter = new StreamWriter((Stream) cryptoStream))
                MMSerialization.JsonSerializer.Serialize((TextWriter) streamWriter, (object) data);
            }
          }
        }
      }
      else
      {
        using (streamWriter = File.CreateText(filepath))
          MMSerialization.JsonSerializer.Serialize((TextWriter) streamWriter, (object) data);
      }
    }
    catch (Exception ex)
    {
      Action<MMReadWriteError> onWriteError = this.OnWriteError;
      if (onWriteError != null)
        onWriteError(new MMReadWriteError(ex.Message));
      Debug.Log((object) ex.Message.Colour(Color.red));
      Debug.LogException((Exception) ex);
      flag = false;
    }
    finally
    {
      fileStream?.Dispose();
      cryptoServiceProvider?.Dispose();
      aes?.Dispose();
      cryptoStream?.Dispose();
      streamWriter?.Dispose();
      if (!flag)
      {
        string backupFilepath = this.GetBackupFilepath(this.GetMostRecentBackup(filename));
        if (File.Exists(backupFilepath) & backup)
          File.Copy(backupFilepath, filepath, true);
      }
      else
      {
        if (backup)
          this.MakeBackup(filename);
        System.Action onWriteCompleted = this.OnWriteCompleted;
        if (onWriteCompleted != null)
          onWriteCompleted();
      }
    }
  }

  public override void Read(string filename)
  {
    Debug.Log((object) ("MMJsonDataReadWriter - Read File " + this.GetFilepath(filename)).Colour(Color.yellow));
    FileStream fileStream = (FileStream) null;
    Aes aes = (Aes) null;
    CryptoStream cryptoStream = (CryptoStream) null;
    StreamReader reader1 = (StreamReader) null;
    JsonTextReader reader2 = (JsonTextReader) null;
    try
    {
      if (this.FileExists(filename))
      {
        T message;
        using (fileStream = new FileStream(this.GetFilepath(filename), FileMode.Open))
        {
          byte[] buffer = new byte[1];
          fileStream.Read(buffer, 0, buffer.Length);
          if (Convert.ToChar(buffer[0]) == 'E')
          {
            byte[] numArray1 = new byte[16 /*0x10*/];
            fileStream.Read(numArray1, 0, numArray1.Length);
            using (aes = Aes.Create())
            {
              byte[] numArray2 = new byte[aes.IV.Length];
              int length = aes.IV.Length;
              int offset = 0;
              int num;
              for (; length > 0; length -= num)
              {
                num = fileStream.Read(numArray2, offset, length);
                if (num != 0)
                  offset += num;
                else
                  break;
              }
              using (cryptoStream = new CryptoStream((Stream) fileStream, aes.CreateDecryptor(numArray1, numArray2), CryptoStreamMode.Read))
              {
                using (reader1 = new StreamReader((Stream) cryptoStream))
                {
                  string end = reader1.ReadToEnd();
                  Debug.Log((object) end);
                  message = JsonConvert.DeserializeObject<T>(end, MMSerialization.JsonSerializerSettings);
                  Debug.Log((object) "MMJsonDataReadWriter - Read File - Successfully read encrypted file!".Colour(Color.yellow));
                }
              }
            }
          }
          else
          {
            fileStream.Position = 0L;
            using (reader1 = new StreamReader((Stream) fileStream))
            {
              using (reader2 = new JsonTextReader((TextReader) reader1))
              {
                message = MMSerialization.JsonSerializer.Deserialize<T>((JsonReader) reader2);
                Debug.Log((object) message);
                Debug.Log((object) "MMJsonDataReadWriter - Read File - Successfully read file!".Colour(Color.yellow));
              }
            }
          }
        }
        if ((object) message == null)
        {
          ScrubBackups();
        }
        else
        {
          Action<T> onReadCompleted = this.OnReadCompleted;
          if (onReadCompleted == null)
            return;
          onReadCompleted(message);
        }
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
      ScrubBackups();
      Debug.Log((object) ex.Message.Colour(Color.red));
      Debug.LogException((Exception) ex);
    }
    finally
    {
      fileStream?.Dispose();
      aes?.Dispose();
      cryptoStream?.Dispose();
      reader1?.Dispose();
      reader2?.Close();
    }

    void ScrubBackups()
    {
      filename = filename.NormalizePath().Split(Path.DirectorySeparatorChar).LastElement<string>();
      List<string> backups = this.GetBackups(filename);
      backups.Reverse();
      if (backups.Count > 0)
      {
        int index = 0;
        if (backups.Contains(filename))
        {
          this.Delete(this.GetBackupFilepath(filename));
          index = backups.IndexOf(filename) + 1;
        }
        if (index != -1 && index < backups.Count)
        {
          this.Read(this.GetBackupFilepath(backups[index]));
        }
        else
        {
          Debug.Log((object) "$MMJsonDataReadWriter - Read File - Some kind of corruption has occurred and unable to retrieve file from backup!".Colour(Color.red));
          Action<MMReadWriteError> onReadError = this.OnReadError;
          if (onReadError == null)
            return;
          onReadError(new MMReadWriteError("File is corrupted!"));
        }
      }
      else
      {
        Debug.Log((object) "$MMJsonDataReadWriter - Read File - Some kind of corruption has occurred! No backups available".Colour(Color.red));
        Action<MMReadWriteError> onReadError = this.OnReadError;
        if (onReadError == null)
          return;
        onReadError(new MMReadWriteError("File is corrupted and reached the end of backups!"));
      }
    }
  }

  public override void Delete(string filename)
  {
    Debug.Log((object) ("MMJsonDataReadWriter - Delete " + this.GetFilepath(filename)).Colour(Color.yellow));
    if (!this.FileExists(filename))
      return;
    Debug.Log((object) ("MMDataReadWriter - Deletion Successful " + this.GetFilepath(filename)).Colour(Color.yellow));
    File.Delete(this.GetFilepath(filename));
    System.Action deletionComplete = this.OnDeletionComplete;
    if (deletionComplete != null)
      deletionComplete();
    foreach (string backup in this.GetBackups(filename))
      File.Delete(this.GetBackupFilepath(backup));
  }

  public override bool FileExists(string filename)
  {
    Debug.Log((object) ("MMJsonDataReadWriter - File Exists? " + this.GetFilepath(filename)).Colour(Color.yellow));
    return File.Exists(this.GetFilepath(filename));
  }

  private void MakeBackup(string filename)
  {
    if (!this.FileExists(filename))
      return;
    string backupDirectory = this.GetBackupDirectory();
    if (!Directory.Exists(backupDirectory))
      Directory.CreateDirectory(backupDirectory);
    List<string> backups = this.GetBackups(filename);
    foreach (string filename1 in backups.ToArray())
    {
      if (filename1.Split('.').Length != 3)
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
      string[] strArray = backups.LastElement<string>().Split('.');
      int result;
      if (strArray.Length == 3 && int.TryParse(strArray[1], out result))
      {
        ++result;
        str = $"{strArray[0]}.{(object) result}";
      }
    }
    else
      str = filename.Split('.')[0] + ".1";
    string filename2 = str + ".json";
    File.Copy(this.GetFilepath(filename), this.GetBackupFilepath(filename2), true);
  }

  private string GetMostRecentBackup(string filename)
  {
    List<string> backups = this.GetBackups(filename);
    return backups.Count > 0 ? backups.LastElement<string>() : string.Empty;
  }

  private List<string> GetBackups(string filename)
  {
    string[] strArray = filename.Split('.');
    List<string> backups = new List<string>();
    foreach (FileInfo fileInfo in (IEnumerable<FileInfo>) ((IEnumerable<FileInfo>) new DirectoryInfo(this.GetBackupDirectory()).GetFiles()).OrderBy<FileInfo, DateTime>((Func<FileInfo, DateTime>) (f => f.LastWriteTime)))
    {
      if (strArray.Length != 0 && fileInfo.Name.Contains(strArray[0]))
        backups.Add(fileInfo.Name);
    }
    return backups;
  }

  private string GetDirectory() => Path.Combine(Application.persistentDataPath, "saves");

  private string GetFilepath(string filename) => Path.Combine(this.GetDirectory(), filename);

  private string GetBackupDirectory() => Path.Combine(Application.persistentDataPath, "backup");

  private string GetBackupFilepath(string filename)
  {
    return Path.Combine(this.GetBackupDirectory(), filename);
  }
}
