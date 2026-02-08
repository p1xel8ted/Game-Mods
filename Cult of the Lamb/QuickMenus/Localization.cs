namespace QuickMenus;

internal static class Localization
{
    // ── Config Display Names: Hotkeys ──
    internal static string NameFollowerForms => Get("NameFollowerForms");
    internal static string NameBuildMenu => Get("NameBuildMenu");
    internal static string NameTailorClothing => Get("NameTailorClothing");
    internal static string NamePlayerUpgrades => Get("NamePlayerUpgrades");

    // ── Config Display Names: Toggles ──
    internal static string NameEnableFollowerForms => Get("NameEnableFollowerForms");
    internal static string NameEnableBuildMenu => Get("NameEnableBuildMenu");
    internal static string NameEnableTailor => Get("NameEnableTailor");
    internal static string NameEnablePlayerUpgrades => Get("NameEnablePlayerUpgrades");

    // ── Config Descriptions: Hotkeys ──
    internal static string DescFollowerForms => Get("DescFollowerForms");
    internal static string DescBuildMenu => Get("DescBuildMenu");
    internal static string DescTailorClothing => Get("DescTailorClothing");
    internal static string DescPlayerUpgrades => Get("DescPlayerUpgrades");

    // ── Config Descriptions: Toggles ──
    internal static string DescEnableFollowerForms => Get("DescEnableFollowerForms");
    internal static string DescEnableBuildMenu => Get("DescEnableBuildMenu");
    internal static string DescEnableTailor => Get("DescEnableTailor");
    internal static string DescEnablePlayerUpgrades => Get("DescEnablePlayerUpgrades");

    // ── Menu Title Labels ──
    internal static string Structures => Get("Structures");
    internal static string Clothing => Get("Clothing");

    private static string Get(string key)
    {
        var lang = LocalizationManager.CurrentLanguage ?? "English";
        if (Strings.TryGetValue(lang, out var dict) && dict.TryGetValue(key, out var value))
        {
            return value;
        }
        return Strings["English"].TryGetValue(key, out var fallback) ? fallback : key;
    }

    private static readonly Dictionary<string, Dictionary<string, string>> Strings = new()
    {
        {
            "English", new()
            {
                // Config names
                { "NameFollowerForms", "Follower Forms" },
                { "NameBuildMenu", "Build Menu" },
                { "NameTailorClothing", "Tailor / Clothing" },
                { "NamePlayerUpgrades", "Player Upgrades / Fleeces" },
                { "NameEnableFollowerForms", "Enable Follower Forms" },
                { "NameEnableBuildMenu", "Enable Build Menu" },
                { "NameEnableTailor", "Enable Tailor" },
                { "NameEnablePlayerUpgrades", "Enable Player Upgrades" },
                // Config descriptions
                { "DescFollowerForms", "Open the follower forms/skins collection menu." },
                { "DescBuildMenu", "Open the build menu." },
                { "DescTailorClothing", "Open the tailor menu to craft and assign clothing." },
                { "DescPlayerUpgrades", "Open the player upgrades menu \u2014 fleeces, doctrines, and crown abilities." },
                { "DescEnableFollowerForms", "Enable the follower forms hotkey." },
                { "DescEnableBuildMenu", "Enable the build menu hotkey." },
                { "DescEnableTailor", "Enable the tailor menu hotkey." },
                { "DescEnablePlayerUpgrades", "Enable the player upgrades hotkey." },
                // Menu labels
                { "Structures", "Structures" },
                { "Clothing", "Clothing" }
            }
        },
        {
            "French", new()
            {
                { "NameFollowerForms", "Formes de fid\u00e8les" },
                { "NameBuildMenu", "Menu de construction" },
                { "NameTailorClothing", "Tailleur / V\u00eatements" },
                { "NamePlayerUpgrades", "Am\u00e9liorations / Toisons" },
                { "NameEnableFollowerForms", "Activer les formes de fid\u00e8les" },
                { "NameEnableBuildMenu", "Activer le menu de construction" },
                { "NameEnableTailor", "Activer le tailleur" },
                { "NameEnablePlayerUpgrades", "Activer les am\u00e9liorations" },
                { "DescFollowerForms", "Ouvrir le menu de collection de formes/apparences de fid\u00e8les." },
                { "DescBuildMenu", "Ouvrir le menu de construction." },
                { "DescTailorClothing", "Ouvrir le menu du tailleur pour fabriquer et attribuer des v\u00eatements." },
                { "DescPlayerUpgrades", "Ouvrir le menu d\u2019am\u00e9liorations \u2014 toisons, doctrines et pouvoirs de la couronne." },
                { "DescEnableFollowerForms", "Activer le raccourci des formes de fid\u00e8les." },
                { "DescEnableBuildMenu", "Activer le raccourci du menu de construction." },
                { "DescEnableTailor", "Activer le raccourci du tailleur." },
                { "DescEnablePlayerUpgrades", "Activer le raccourci des am\u00e9liorations." },
                { "Structures", "Structures" },
                { "Clothing", "V\u00eatements" }
            }
        },
        {
            "French (Canadian)", new()
            {
                { "NameFollowerForms", "Formes de fid\u00e8les" },
                { "NameBuildMenu", "Menu de construction" },
                { "NameTailorClothing", "Tailleur / V\u00eatements" },
                { "NamePlayerUpgrades", "Am\u00e9liorations / Toisons" },
                { "NameEnableFollowerForms", "Activer les formes de fid\u00e8les" },
                { "NameEnableBuildMenu", "Activer le menu de construction" },
                { "NameEnableTailor", "Activer le tailleur" },
                { "NameEnablePlayerUpgrades", "Activer les am\u00e9liorations" },
                { "DescFollowerForms", "Ouvrir le menu de collection de formes/apparences de fid\u00e8les." },
                { "DescBuildMenu", "Ouvrir le menu de construction." },
                { "DescTailorClothing", "Ouvrir le menu du tailleur pour fabriquer et attribuer des v\u00eatements." },
                { "DescPlayerUpgrades", "Ouvrir le menu d\u2019am\u00e9liorations \u2014 toisons, doctrines et pouvoirs de la couronne." },
                { "DescEnableFollowerForms", "Activer le raccourci des formes de fid\u00e8les." },
                { "DescEnableBuildMenu", "Activer le raccourci du menu de construction." },
                { "DescEnableTailor", "Activer le raccourci du tailleur." },
                { "DescEnablePlayerUpgrades", "Activer le raccourci des am\u00e9liorations." },
                { "Structures", "Structures" },
                { "Clothing", "V\u00eatements" }
            }
        },
        {
            "German", new()
            {
                { "NameFollowerForms", "Anh\u00e4ngerformen" },
                { "NameBuildMenu", "Baumen\u00fc" },
                { "NameTailorClothing", "Schneider / Kleidung" },
                { "NamePlayerUpgrades", "Spieler-Upgrades / Vliese" },
                { "NameEnableFollowerForms", "Anh\u00e4ngerformen aktivieren" },
                { "NameEnableBuildMenu", "Baumen\u00fc aktivieren" },
                { "NameEnableTailor", "Schneider aktivieren" },
                { "NameEnablePlayerUpgrades", "Spieler-Upgrades aktivieren" },
                { "DescFollowerForms", "\u00d6ffnet das Men\u00fc der Anh\u00e4ngerformen/-skins." },
                { "DescBuildMenu", "\u00d6ffnet das Baumen\u00fc." },
                { "DescTailorClothing", "\u00d6ffnet das Schneidermen\u00fc zum Herstellen und Zuweisen von Kleidung." },
                { "DescPlayerUpgrades", "\u00d6ffnet das Verbesserungsmen\u00fc \u2014 Vliese, Doktrinen und Kronenf\u00e4higkeiten." },
                { "DescEnableFollowerForms", "Aktiviert die Tastenkombination f\u00fcr Anh\u00e4ngerformen." },
                { "DescEnableBuildMenu", "Aktiviert die Tastenkombination f\u00fcr das Baumen\u00fc." },
                { "DescEnableTailor", "Aktiviert die Tastenkombination f\u00fcr den Schneider." },
                { "DescEnablePlayerUpgrades", "Aktiviert die Tastenkombination f\u00fcr Spieler-Upgrades." },
                { "Structures", "Geb\u00e4ude" },
                { "Clothing", "Kleidung" }
            }
        },
        {
            "Spanish", new()
            {
                { "NameFollowerForms", "Formas de seguidores" },
                { "NameBuildMenu", "Men\u00fa de construcci\u00f3n" },
                { "NameTailorClothing", "Sastre / Ropa" },
                { "NamePlayerUpgrades", "Mejoras / Vellones" },
                { "NameEnableFollowerForms", "Activar formas de seguidores" },
                { "NameEnableBuildMenu", "Activar men\u00fa de construcci\u00f3n" },
                { "NameEnableTailor", "Activar sastre" },
                { "NameEnablePlayerUpgrades", "Activar mejoras" },
                { "DescFollowerForms", "Abrir el men\u00fa de colecci\u00f3n de formas/aspectos de seguidores." },
                { "DescBuildMenu", "Abrir el men\u00fa de construcci\u00f3n." },
                { "DescTailorClothing", "Abrir el men\u00fa del sastre para crear y asignar ropa." },
                { "DescPlayerUpgrades", "Abrir el men\u00fa de mejoras \u2014 vellones, doctrinas y habilidades de la corona." },
                { "DescEnableFollowerForms", "Activar el atajo de formas de seguidores." },
                { "DescEnableBuildMenu", "Activar el atajo del men\u00fa de construcci\u00f3n." },
                { "DescEnableTailor", "Activar el atajo del sastre." },
                { "DescEnablePlayerUpgrades", "Activar el atajo de mejoras." },
                { "Structures", "Estructuras" },
                { "Clothing", "Ropa" }
            }
        },
        {
            "Italian", new()
            {
                { "NameFollowerForms", "Forme dei seguaci" },
                { "NameBuildMenu", "Menu costruzione" },
                { "NameTailorClothing", "Sarto / Abbigliamento" },
                { "NamePlayerUpgrades", "Miglioramenti / Velli" },
                { "NameEnableFollowerForms", "Attiva forme dei seguaci" },
                { "NameEnableBuildMenu", "Attiva menu costruzione" },
                { "NameEnableTailor", "Attiva sarto" },
                { "NameEnablePlayerUpgrades", "Attiva miglioramenti" },
                { "DescFollowerForms", "Apri il menu della collezione di forme/aspetti dei seguaci." },
                { "DescBuildMenu", "Apri il menu di costruzione." },
                { "DescTailorClothing", "Apri il menu del sarto per creare e assegnare abbigliamento." },
                { "DescPlayerUpgrades", "Apri il menu dei miglioramenti \u2014 velli, dottrine e abilit\u00e0 della corona." },
                { "DescEnableFollowerForms", "Attiva la scorciatoia per le forme dei seguaci." },
                { "DescEnableBuildMenu", "Attiva la scorciatoia per il menu di costruzione." },
                { "DescEnableTailor", "Attiva la scorciatoia per il sarto." },
                { "DescEnablePlayerUpgrades", "Attiva la scorciatoia per i miglioramenti." },
                { "Structures", "Strutture" },
                { "Clothing", "Abbigliamento" }
            }
        },
        {
            "Portuguese (Brazil)", new()
            {
                { "NameFollowerForms", "Formas de seguidores" },
                { "NameBuildMenu", "Menu de constru\u00e7\u00e3o" },
                { "NameTailorClothing", "Alfaiate / Roupas" },
                { "NamePlayerUpgrades", "Melhorias / Tosas" },
                { "NameEnableFollowerForms", "Ativar formas de seguidores" },
                { "NameEnableBuildMenu", "Ativar menu de constru\u00e7\u00e3o" },
                { "NameEnableTailor", "Ativar alfaiate" },
                { "NameEnablePlayerUpgrades", "Ativar melhorias" },
                { "DescFollowerForms", "Abrir o menu de cole\u00e7\u00e3o de formas/apar\u00eancias de seguidores." },
                { "DescBuildMenu", "Abrir o menu de constru\u00e7\u00e3o." },
                { "DescTailorClothing", "Abrir o menu do alfaiate para criar e atribuir roupas." },
                { "DescPlayerUpgrades", "Abrir o menu de melhorias \u2014 tosas, doutrinas e habilidades da coroa." },
                { "DescEnableFollowerForms", "Ativar o atalho de formas de seguidores." },
                { "DescEnableBuildMenu", "Ativar o atalho do menu de constru\u00e7\u00e3o." },
                { "DescEnableTailor", "Ativar o atalho do alfaiate." },
                { "DescEnablePlayerUpgrades", "Ativar o atalho de melhorias." },
                { "Structures", "Estruturas" },
                { "Clothing", "Roupas" }
            }
        },
        {
            "Dutch", new()
            {
                { "NameFollowerForms", "Volgelingvormen" },
                { "NameBuildMenu", "Bouwmenu" },
                { "NameTailorClothing", "Kleermaker / Kleding" },
                { "NamePlayerUpgrades", "Spelerverbeteringen / Vachten" },
                { "NameEnableFollowerForms", "Volgelingvormen inschakelen" },
                { "NameEnableBuildMenu", "Bouwmenu inschakelen" },
                { "NameEnableTailor", "Kleermaker inschakelen" },
                { "NameEnablePlayerUpgrades", "Spelerverbeteringen inschakelen" },
                { "DescFollowerForms", "Open het menu met de collectie volgelingvormen/-skins." },
                { "DescBuildMenu", "Open het bouwmenu." },
                { "DescTailorClothing", "Open het kleermakermenu om kleding te maken en toe te wijzen." },
                { "DescPlayerUpgrades", "Open het verbeteringsmenu \u2014 vachten, doctrines en kroonvaardigheden." },
                { "DescEnableFollowerForms", "Schakel de sneltoets voor volgelingvormen in." },
                { "DescEnableBuildMenu", "Schakel de sneltoets voor het bouwmenu in." },
                { "DescEnableTailor", "Schakel de sneltoets voor de kleermaker in." },
                { "DescEnablePlayerUpgrades", "Schakel de sneltoets voor spelerverbeteringen in." },
                { "Structures", "Gebouwen" },
                { "Clothing", "Kleding" }
            }
        },
        {
            "Russian", new()
            {
                { "NameFollowerForms", "\u0424\u043e\u0440\u043c\u044b \u043f\u043e\u0441\u043b\u0435\u0434\u043e\u0432\u0430\u0442\u0435\u043b\u0435\u0439" },
                { "NameBuildMenu", "\u041c\u0435\u043d\u044e \u0441\u0442\u0440\u043e\u0438\u0442\u0435\u043b\u044c\u0441\u0442\u0432\u0430" },
                { "NameTailorClothing", "\u041f\u043e\u0440\u0442\u043d\u043e\u0439 / \u041e\u0434\u0435\u0436\u0434\u0430" },
                { "NamePlayerUpgrades", "\u0423\u043b\u0443\u0447\u0448\u0435\u043d\u0438\u044f / \u0420\u0443\u043d\u0430" },
                { "NameEnableFollowerForms", "\u0412\u043a\u043b\u044e\u0447\u0438\u0442\u044c \u0444\u043e\u0440\u043c\u044b \u043f\u043e\u0441\u043b\u0435\u0434\u043e\u0432\u0430\u0442\u0435\u043b\u0435\u0439" },
                { "NameEnableBuildMenu", "\u0412\u043a\u043b\u044e\u0447\u0438\u0442\u044c \u043c\u0435\u043d\u044e \u0441\u0442\u0440\u043e\u0438\u0442\u0435\u043b\u044c\u0441\u0442\u0432\u0430" },
                { "NameEnableTailor", "\u0412\u043a\u043b\u044e\u0447\u0438\u0442\u044c \u043f\u043e\u0440\u0442\u043d\u043e\u0433\u043e" },
                { "NameEnablePlayerUpgrades", "\u0412\u043a\u043b\u044e\u0447\u0438\u0442\u044c \u0443\u043b\u0443\u0447\u0448\u0435\u043d\u0438\u044f" },
                { "DescFollowerForms", "\u041e\u0442\u043a\u0440\u044b\u0442\u044c \u043c\u0435\u043d\u044e \u043a\u043e\u043b\u043b\u0435\u043a\u0446\u0438\u0438 \u0444\u043e\u0440\u043c/\u043e\u0431\u043b\u0438\u043a\u043e\u0432 \u043f\u043e\u0441\u043b\u0435\u0434\u043e\u0432\u0430\u0442\u0435\u043b\u0435\u0439." },
                { "DescBuildMenu", "\u041e\u0442\u043a\u0440\u044b\u0442\u044c \u043c\u0435\u043d\u044e \u0441\u0442\u0440\u043e\u0438\u0442\u0435\u043b\u044c\u0441\u0442\u0432\u0430." },
                { "DescTailorClothing", "\u041e\u0442\u043a\u0440\u044b\u0442\u044c \u043c\u0435\u043d\u044e \u043f\u043e\u0440\u0442\u043d\u043e\u0433\u043e \u0434\u043b\u044f \u0441\u043e\u0437\u0434\u0430\u043d\u0438\u044f \u0438 \u043d\u0430\u0437\u043d\u0430\u0447\u0435\u043d\u0438\u044f \u043e\u0434\u0435\u0436\u0434\u044b." },
                { "DescPlayerUpgrades", "\u041e\u0442\u043a\u0440\u044b\u0442\u044c \u043c\u0435\u043d\u044e \u0443\u043b\u0443\u0447\u0448\u0435\u043d\u0438\u0439 \u2014 \u0440\u0443\u043d\u0430, \u0434\u043e\u043a\u0442\u0440\u0438\u043d\u044b \u0438 \u0441\u043f\u043e\u0441\u043e\u0431\u043d\u043e\u0441\u0442\u0438 \u043a\u043e\u0440\u043e\u043d\u044b." },
                { "DescEnableFollowerForms", "\u0412\u043a\u043b\u044e\u0447\u0438\u0442\u044c \u0433\u043e\u0440\u044f\u0447\u0443\u044e \u043a\u043b\u0430\u0432\u0438\u0448\u0443 \u0434\u043b\u044f \u0444\u043e\u0440\u043c \u043f\u043e\u0441\u043b\u0435\u0434\u043e\u0432\u0430\u0442\u0435\u043b\u0435\u0439." },
                { "DescEnableBuildMenu", "\u0412\u043a\u043b\u044e\u0447\u0438\u0442\u044c \u0433\u043e\u0440\u044f\u0447\u0443\u044e \u043a\u043b\u0430\u0432\u0438\u0448\u0443 \u0434\u043b\u044f \u043c\u0435\u043d\u044e \u0441\u0442\u0440\u043e\u0438\u0442\u0435\u043b\u044c\u0441\u0442\u0432\u0430." },
                { "DescEnableTailor", "\u0412\u043a\u043b\u044e\u0447\u0438\u0442\u044c \u0433\u043e\u0440\u044f\u0447\u0443\u044e \u043a\u043b\u0430\u0432\u0438\u0448\u0443 \u0434\u043b\u044f \u043f\u043e\u0440\u0442\u043d\u043e\u0433\u043e." },
                { "DescEnablePlayerUpgrades", "\u0412\u043a\u043b\u044e\u0447\u0438\u0442\u044c \u0433\u043e\u0440\u044f\u0447\u0443\u044e \u043a\u043b\u0430\u0432\u0438\u0448\u0443 \u0434\u043b\u044f \u0443\u043b\u0443\u0447\u0448\u0435\u043d\u0438\u0439." },
                { "Structures", "\u0421\u0442\u0440\u043e\u0435\u043d\u0438\u044f" },
                { "Clothing", "\u041e\u0434\u0435\u0436\u0434\u0430" }
            }
        },
        {
            "Turkish", new()
            {
                { "NameFollowerForms", "Takip\u00e7i Formlar\u0131" },
                { "NameBuildMenu", "\u0130n\u015fa Men\u00fcs\u00fc" },
                { "NameTailorClothing", "Terzi / K\u0131yafetler" },
                { "NamePlayerUpgrades", "Oyuncu Geli\u015ftirmeleri / Post" },
                { "NameEnableFollowerForms", "Takip\u00e7i Formlar\u0131n\u0131 Etkinle\u015ftir" },
                { "NameEnableBuildMenu", "\u0130n\u015fa Men\u00fcs\u00fcn\u00fc Etkinle\u015ftir" },
                { "NameEnableTailor", "Terziyi Etkinle\u015ftir" },
                { "NameEnablePlayerUpgrades", "Oyuncu Geli\u015ftirmelerini Etkinle\u015ftir" },
                { "DescFollowerForms", "Takip\u00e7i formlar\u0131/g\u00f6r\u00fcn\u00fcm koleksiyonu men\u00fcs\u00fcn\u00fc a\u00e7." },
                { "DescBuildMenu", "\u0130n\u015fa men\u00fcs\u00fcn\u00fc a\u00e7." },
                { "DescTailorClothing", "K\u0131yafet \u00fcretmek ve atamak i\u00e7in terzi men\u00fcs\u00fcn\u00fc a\u00e7." },
                { "DescPlayerUpgrades", "Geli\u015ftirmeler men\u00fcs\u00fcn\u00fc a\u00e7 \u2014 postlar, doktrinler ve ta\u00e7 yetenekleri." },
                { "DescEnableFollowerForms", "Takip\u00e7i formlar\u0131 k\u0131sayolunu etkinle\u015ftir." },
                { "DescEnableBuildMenu", "\u0130n\u015fa men\u00fcs\u00fc k\u0131sayolunu etkinle\u015ftir." },
                { "DescEnableTailor", "Terzi k\u0131sayolunu etkinle\u015ftir." },
                { "DescEnablePlayerUpgrades", "Oyuncu geli\u015ftirmeleri k\u0131sayolunu etkinle\u015ftir." },
                { "Structures", "Yap\u0131lar" },
                { "Clothing", "K\u0131yafetler" }
            }
        },
        {
            "Japanese", new()
            {
                { "NameFollowerForms", "\u4fe1\u8005\u306e\u59ff" },
                { "NameBuildMenu", "\u5efa\u8a2d\u30e1\u30cb\u30e5\u30fc" },
                { "NameTailorClothing", "\u4ed5\u7acb\u5c4b / \u8863\u88c5" },
                { "NamePlayerUpgrades", "\u30d7\u30ec\u30a4\u30e4\u30fc\u5f37\u5316 / \u30d5\u30ea\u30fc\u30b9" },
                { "NameEnableFollowerForms", "\u4fe1\u8005\u306e\u59ff\u3092\u6709\u52b9\u5316" },
                { "NameEnableBuildMenu", "\u5efa\u8a2d\u30e1\u30cb\u30e5\u30fc\u3092\u6709\u52b9\u5316" },
                { "NameEnableTailor", "\u4ed5\u7acb\u5c4b\u3092\u6709\u52b9\u5316" },
                { "NameEnablePlayerUpgrades", "\u30d7\u30ec\u30a4\u30e4\u30fc\u5f37\u5316\u3092\u6709\u52b9\u5316" },
                { "DescFollowerForms", "\u4fe1\u8005\u306e\u59ff/\u30b9\u30ad\u30f3\u30b3\u30ec\u30af\u30b7\u30e7\u30f3\u30e1\u30cb\u30e5\u30fc\u3092\u958b\u304d\u307e\u3059\u3002" },
                { "DescBuildMenu", "\u5efa\u8a2d\u30e1\u30cb\u30e5\u30fc\u3092\u958b\u304d\u307e\u3059\u3002" },
                { "DescTailorClothing", "\u4ed5\u7acb\u5c4b\u30e1\u30cb\u30e5\u30fc\u3092\u958b\u3044\u3066\u8863\u88c5\u3092\u4f5c\u6210\u30fb\u5272\u308a\u5f53\u3066\u307e\u3059\u3002" },
                { "DescPlayerUpgrades", "\u5f37\u5316\u30e1\u30cb\u30e5\u30fc\u3092\u958b\u304d\u307e\u3059 \u2014 \u30d5\u30ea\u30fc\u30b9\u3001\u6559\u7fa9\u3001\u738b\u51a0\u306e\u80fd\u529b\u3002" },
                { "DescEnableFollowerForms", "\u4fe1\u8005\u306e\u59ff\u306e\u30db\u30c3\u30c8\u30ad\u30fc\u3092\u6709\u52b9\u306b\u3057\u307e\u3059\u3002" },
                { "DescEnableBuildMenu", "\u5efa\u8a2d\u30e1\u30cb\u30e5\u30fc\u306e\u30db\u30c3\u30c8\u30ad\u30fc\u3092\u6709\u52b9\u306b\u3057\u307e\u3059\u3002" },
                { "DescEnableTailor", "\u4ed5\u7acb\u5c4b\u306e\u30db\u30c3\u30c8\u30ad\u30fc\u3092\u6709\u52b9\u306b\u3057\u307e\u3059\u3002" },
                { "DescEnablePlayerUpgrades", "\u30d7\u30ec\u30a4\u30e4\u30fc\u5f37\u5316\u306e\u30db\u30c3\u30c8\u30ad\u30fc\u3092\u6709\u52b9\u306b\u3057\u307e\u3059\u3002" },
                { "Structures", "\u5efa\u9020\u7269" },
                { "Clothing", "\u8863\u88c5" }
            }
        },
        {
            "Korean", new()
            {
                { "NameFollowerForms", "\ucd94\uc885\uc790 \ud615\ud0dc" },
                { "NameBuildMenu", "\uac74\uc124 \uba54\ub274" },
                { "NameTailorClothing", "\uc7ac\ub2e8\uc0ac / \uc758\uc0c1" },
                { "NamePlayerUpgrades", "\ud50c\ub808\uc774\uc5b4 \uac15\ud654 / \uc591\ud138" },
                { "NameEnableFollowerForms", "\ucd94\uc885\uc790 \ud615\ud0dc \ud65c\uc131\ud654" },
                { "NameEnableBuildMenu", "\uac74\uc124 \uba54\ub274 \ud65c\uc131\ud654" },
                { "NameEnableTailor", "\uc7ac\ub2e8\uc0ac \ud65c\uc131\ud654" },
                { "NameEnablePlayerUpgrades", "\ud50c\ub808\uc774\uc5b4 \uac15\ud654 \ud65c\uc131\ud654" },
                { "DescFollowerForms", "\ucd94\uc885\uc790 \ud615\ud0dc/\uc2a4\ud0a8 \ucef4\ub809\uc158 \uba54\ub274\ub97c \uc5fd\ub2c8\ub2e4." },
                { "DescBuildMenu", "\uac74\uc124 \uba54\ub274\ub97c \uc5fd\ub2c8\ub2e4." },
                { "DescTailorClothing", "\uc7ac\ub2e8\uc0ac \uba54\ub274\ub97c \uc5f4\uc5b4 \uc758\uc0c1\uc744 \uc81c\uc791\ud558\uace0 \ud560\ub2f9\ud569\ub2c8\ub2e4." },
                { "DescPlayerUpgrades", "\uac15\ud654 \uba54\ub274\ub97c \uc5fd\ub2c8\ub2e4 \u2014 \uc591\ud138, \uad50\ub9ac, \uc655\uad00 \ub2a5\ub825." },
                { "DescEnableFollowerForms", "\ucd94\uc885\uc790 \ud615\ud0dc \ub2e8\ucd95\ud0a4\ub97c \ud65c\uc131\ud654\ud569\ub2c8\ub2e4." },
                { "DescEnableBuildMenu", "\uac74\uc124 \uba54\ub274 \ub2e8\ucd95\ud0a4\ub97c \ud65c\uc131\ud654\ud569\ub2c8\ub2e4." },
                { "DescEnableTailor", "\uc7ac\ub2e8\uc0ac \ub2e8\ucd95\ud0a4\ub97c \ud65c\uc131\ud654\ud569\ub2c8\ub2e4." },
                { "DescEnablePlayerUpgrades", "\ud50c\ub808\uc774\uc5b4 \uac15\ud654 \ub2e8\ucd95\ud0a4\ub97c \ud65c\uc131\ud654\ud569\ub2c8\ub2e4." },
                { "Structures", "\uac74\ucd95\ubb3c" },
                { "Clothing", "\uc758\uc0c1" }
            }
        },
        {
            "Chinese (Simplified)", new()
            {
                { "NameFollowerForms", "\u4fe1\u5f92\u5f62\u6001" },
                { "NameBuildMenu", "\u5efa\u9020\u83dc\u5355" },
                { "NameTailorClothing", "\u88c1\u7f1d / \u670d\u88c5" },
                { "NamePlayerUpgrades", "\u73a9\u5bb6\u5347\u7ea7 / \u7f8a\u6bdb\u886b" },
                { "NameEnableFollowerForms", "\u542f\u7528\u4fe1\u5f92\u5f62\u6001" },
                { "NameEnableBuildMenu", "\u542f\u7528\u5efa\u9020\u83dc\u5355" },
                { "NameEnableTailor", "\u542f\u7528\u88c1\u7f1d" },
                { "NameEnablePlayerUpgrades", "\u542f\u7528\u73a9\u5bb6\u5347\u7ea7" },
                { "DescFollowerForms", "\u6253\u5f00\u4fe1\u5f92\u5f62\u6001/\u76ae\u80a4\u6536\u85cf\u83dc\u5355\u3002" },
                { "DescBuildMenu", "\u6253\u5f00\u5efa\u9020\u83dc\u5355\u3002" },
                { "DescTailorClothing", "\u6253\u5f00\u88c1\u7f1d\u83dc\u5355\u4ee5\u5236\u4f5c\u548c\u5206\u914d\u670d\u88c5\u3002" },
                { "DescPlayerUpgrades", "\u6253\u5f00\u5347\u7ea7\u83dc\u5355\u2014\u2014\u7f8a\u6bdb\u886b\u3001\u6559\u4e49\u548c\u738b\u51a0\u80fd\u529b\u3002" },
                { "DescEnableFollowerForms", "\u542f\u7528\u4fe1\u5f92\u5f62\u6001\u5feb\u6377\u952e\u3002" },
                { "DescEnableBuildMenu", "\u542f\u7528\u5efa\u9020\u83dc\u5355\u5feb\u6377\u952e\u3002" },
                { "DescEnableTailor", "\u542f\u7528\u88c1\u7f1d\u5feb\u6377\u952e\u3002" },
                { "DescEnablePlayerUpgrades", "\u542f\u7528\u73a9\u5bb6\u5347\u7ea7\u5feb\u6377\u952e\u3002" },
                { "Structures", "\u5efa\u7b51" },
                { "Clothing", "\u670d\u88c5" }
            }
        },
        {
            "Chinese (Traditional)", new()
            {
                { "NameFollowerForms", "\u4fe1\u5f92\u578b\u614b" },
                { "NameBuildMenu", "\u5efa\u9020\u9078\u55ae" },
                { "NameTailorClothing", "\u88c1\u7e2b / \u670d\u88dd" },
                { "NamePlayerUpgrades", "\u73a9\u5bb6\u5347\u7d1a / \u7f8a\u6bdb\u886b" },
                { "NameEnableFollowerForms", "\u555f\u7528\u4fe1\u5f92\u578b\u614b" },
                { "NameEnableBuildMenu", "\u555f\u7528\u5efa\u9020\u9078\u55ae" },
                { "NameEnableTailor", "\u555f\u7528\u88c1\u7e2b" },
                { "NameEnablePlayerUpgrades", "\u555f\u7528\u73a9\u5bb6\u5347\u7d1a" },
                { "DescFollowerForms", "\u958b\u555f\u4fe1\u5f92\u578b\u614b/\u5916\u89c0\u6536\u85cf\u9078\u55ae\u3002" },
                { "DescBuildMenu", "\u958b\u555f\u5efa\u9020\u9078\u55ae\u3002" },
                { "DescTailorClothing", "\u958b\u555f\u88c1\u7e2b\u9078\u55ae\u4ee5\u88fd\u4f5c\u548c\u5206\u914d\u670d\u88dd\u3002" },
                { "DescPlayerUpgrades", "\u958b\u555f\u5347\u7d1a\u9078\u55ae\u2014\u2014\u7f8a\u6bdb\u886b\u3001\u6559\u7fa9\u548c\u738b\u51a0\u80fd\u529b\u3002" },
                { "DescEnableFollowerForms", "\u555f\u7528\u4fe1\u5f92\u578b\u614b\u5feb\u6377\u9375\u3002" },
                { "DescEnableBuildMenu", "\u555f\u7528\u5efa\u9020\u9078\u55ae\u5feb\u6377\u9375\u3002" },
                { "DescEnableTailor", "\u555f\u7528\u88c1\u7e2b\u5feb\u6377\u9375\u3002" },
                { "DescEnablePlayerUpgrades", "\u555f\u7528\u73a9\u5bb6\u5347\u7d1a\u5feb\u6377\u9375\u3002" },
                { "Structures", "\u5efa\u7bc9" },
                { "Clothing", "\u670d\u88dd" }
            }
        },
        {
            "Arabic", new()
            {
                { "NameFollowerForms", "\u0623\u0634\u0643\u0627\u0644 \u0627\u0644\u0623\u062a\u0628\u0627\u0639" },
                { "NameBuildMenu", "\u0642\u0627\u0626\u0645\u0629 \u0627\u0644\u0628\u0646\u0627\u0621" },
                { "NameTailorClothing", "\u0627\u0644\u062e\u064a\u0627\u0637 / \u0627\u0644\u0645\u0644\u0627\u0628\u0633" },
                { "NamePlayerUpgrades", "\u062a\u0631\u0642\u064a\u0627\u062a \u0627\u0644\u0644\u0627\u0639\u0628 / \u0627\u0644\u062c\u0644\u0648\u062f" },
                { "NameEnableFollowerForms", "\u062a\u0641\u0639\u064a\u0644 \u0623\u0634\u0643\u0627\u0644 \u0627\u0644\u0623\u062a\u0628\u0627\u0639" },
                { "NameEnableBuildMenu", "\u062a\u0641\u0639\u064a\u0644 \u0642\u0627\u0626\u0645\u0629 \u0627\u0644\u0628\u0646\u0627\u0621" },
                { "NameEnableTailor", "\u062a\u0641\u0639\u064a\u0644 \u0627\u0644\u062e\u064a\u0627\u0637" },
                { "NameEnablePlayerUpgrades", "\u062a\u0641\u0639\u064a\u0644 \u062a\u0631\u0642\u064a\u0627\u062a \u0627\u0644\u0644\u0627\u0639\u0628" },
                { "DescFollowerForms", "\u0627\u0641\u062a\u062d \u0642\u0627\u0626\u0645\u0629 \u0645\u062c\u0645\u0648\u0639\u0629 \u0623\u0634\u0643\u0627\u0644/\u0645\u0638\u0627\u0647\u0631 \u0627\u0644\u0623\u062a\u0628\u0627\u0639." },
                { "DescBuildMenu", "\u0627\u0641\u062a\u062d \u0642\u0627\u0626\u0645\u0629 \u0627\u0644\u0628\u0646\u0627\u0621." },
                { "DescTailorClothing", "\u0627\u0641\u062a\u062d \u0642\u0627\u0626\u0645\u0629 \u0627\u0644\u062e\u064a\u0627\u0637 \u0644\u0635\u0646\u0639 \u0648\u062a\u0639\u064a\u064a\u0646 \u0627\u0644\u0645\u0644\u0627\u0628\u0633." },
                { "DescPlayerUpgrades", "\u0627\u0641\u062a\u062d \u0642\u0627\u0626\u0645\u0629 \u0627\u0644\u062a\u0631\u0642\u064a\u0627\u062a \u2014 \u0627\u0644\u062c\u0644\u0648\u062f \u0648\u0627\u0644\u0639\u0642\u0627\u0626\u062f \u0648\u0642\u062f\u0631\u0627\u062a \u0627\u0644\u062a\u0627\u062c." },
                { "DescEnableFollowerForms", "\u062a\u0641\u0639\u064a\u0644 \u0645\u0641\u062a\u0627\u062d \u0627\u0644\u0627\u062e\u062a\u0635\u0627\u0631 \u0644\u0623\u0634\u0643\u0627\u0644 \u0627\u0644\u0623\u062a\u0628\u0627\u0639." },
                { "DescEnableBuildMenu", "\u062a\u0641\u0639\u064a\u0644 \u0645\u0641\u062a\u0627\u062d \u0627\u0644\u0627\u062e\u062a\u0635\u0627\u0631 \u0644\u0642\u0627\u0626\u0645\u0629 \u0627\u0644\u0628\u0646\u0627\u0621." },
                { "DescEnableTailor", "\u062a\u0641\u0639\u064a\u0644 \u0645\u0641\u062a\u0627\u062d \u0627\u0644\u0627\u062e\u062a\u0635\u0627\u0631 \u0644\u0644\u062e\u064a\u0627\u0637." },
                { "DescEnablePlayerUpgrades", "\u062a\u0641\u0639\u064a\u0644 \u0645\u0641\u062a\u0627\u062d \u0627\u0644\u0627\u062e\u062a\u0635\u0627\u0631 \u0644\u062a\u0631\u0642\u064a\u0627\u062a \u0627\u0644\u0644\u0627\u0639\u0628." },
                { "Structures", "\u0627\u0644\u0645\u0628\u0627\u0646\u064a" },
                { "Clothing", "\u0627\u0644\u0645\u0644\u0627\u0628\u0633" }
            }
        }
    };
}
