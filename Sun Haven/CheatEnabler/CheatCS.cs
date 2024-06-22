using System.Diagnostics;
using System.Text;
using PSS;
using Shared;
using Object = UnityEngine.Object;

namespace CheatEnabler;

[CommandPrefix("/")]
public static class QuantumConsoleManager
{

    [CommandDescription("Generates a user manual for any given command, including built in ones. To use the man command, simply put the desired command name in front of it. For example, 'man my-command' will generate the manual for 'my-command'")]
    [Command("help")]
    [Command("manual")]
    [Command]
    public static void man(string commandName)
    {
        //remove all / from commandName, and add just one to the beginning
        commandName = commandName.Replace("/", "");
        commandName = "/" + commandName;

        var manual = QuantumConsoleProcessor.GenerateCommandManual(commandName);
        if (string.IsNullOrWhiteSpace(manual))
        {
            Utils.SendNotification($"Manual for command '{commandName}' not found!");
        }
        else
        {
            throw new ArgumentException(manual);
        }
    }


    [Command(Description = "Save the current game.")]
    public static void savegame()
    {
        SingletonBehaviour<GameSave>.Instance.SaveGame(true);
        SingletonBehaviour<NotificationStack>.Instance.SendNotification("Game Saved!");
    }

    [Command(Description = "Add item to the player inventory using the numeric item code.")]
    public static void additembyid(int itemId, int amount = 1)
    {
        var item = Utils.GetNameByID(itemId);
        var qcm = Object.FindObjectOfType<Wish.QuantumConsoleManager>();
        try
        {
            qcm.additem(item, amount);
            QuantumConsole.Instance.LogPlayerText($"Added {amount} {item} to inventory.");
        }
        catch (Exception)
        {
            QuantumConsole.Instance.LogPlayerText($"Failed to add item {item} to inventory. Try /finditemid to get the correct item id.");
        }
    }

    [Command(Description = "Search for an item by name. This will return all items that contain the search term.")]
    public static void finditemid(string itemName)
    {
        List<KeyValuePair<string, int>> Exists = [];
        
        foreach (var word in itemName.Split(' '))
        {
            Exists.AddRange(Database.Instance.ids.Where(a => a.Key.ToLower().Contains(word.ToLower())));
        }
        
        if (Exists.Count == 0)
        {
            QuantumConsole.Instance.LogPlayerText($"No item found for '{itemName}'");
            return;
        }

        foreach (var item in Exists)
        {
            var itemKeyK = $"{item.Key}.Name";
            QuantumConsole.Instance.LogPlayerText($"{item.Value} - {item.Key} - {LocalizeText.TranslateText(itemKeyK, item.Key).Trim()}");
        }
    }

    [Command(Description = "Save all items to file.")]
    public static void saveallitems()
    {
        var path = Path.Combine(Paths.GameRootPath, "items.txt");
        var sb = new StringBuilder();


        var itemsWithLocalizedNames = ItemInfoDatabase.Instance.allItemSellInfos
            .Select(item => new
            {
                item.Key,
                Name = Utils.GetNameByID(item.Key),
                LocalizedName = LocalizeText.TranslateText(item.Value.keyName, item.Value.name).Trim()
            })
            .ToList();


        var sortedItems = itemsWithLocalizedNames.OrderBy(item => item.LocalizedName, StringComparer.OrdinalIgnoreCase).ToList();


        foreach (var item in sortedItems)
        {
            sb.AppendLine($"{item.Key} - {item.Name} - {item.LocalizedName}");
        }


        File.WriteAllText(path, sb.ToString());

        QuantumConsole.Instance.LogPlayerText($"{sortedItems.Count} items saved to {path}");

        var processStartInfo = new ProcessStartInfo
        {
            FileName = path,
            UseShellExecute = true
        };

        Process.Start(processStartInfo);
    }
    
   
}