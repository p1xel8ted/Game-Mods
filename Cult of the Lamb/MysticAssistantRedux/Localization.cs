namespace MysticAssistantRedux;

internal static class Localization
{
    // Config display names
    internal static string DLCNecklacesName => Get("DLCNecklacesName");
    internal static string BossSkinsName => Get("BossSkinsName");
    internal static string GodTearCostName => Get("GodTearCostName");

    // Config descriptions
    internal static string DLCNecklacesDesc => Get("DLCNecklacesDesc");
    internal static string BossSkinsDesc => Get("BossSkinsDesc");
    internal static string GodTearCostDesc => Get("GodTearCostDesc");

    private static string Get(string key)
    {
        var lang = LocalizationManager.CurrentLanguage ?? "English";
        if (Strings.TryGetValue(lang, out var dict) && dict.TryGetValue(key, out var value))
        {
            return value;
        }
        // Fallback to English
        return Strings["English"].TryGetValue(key, out var fallback) ? fallback : key;
    }

    private static readonly Dictionary<string, Dictionary<string, string>> Strings = new()
    {
        {
            "English", new()
            {
                { "DLCNecklacesName", "DLC Necklaces" },
                { "DLCNecklacesDesc", "Add Woolhaven DLC necklaces to the shop. These are normally obtained through DLC gameplay. Re-open the shop for changes to take effect." },
                { "BossSkinsName", "Boss Skins" },
                { "BossSkinsDesc", "Add boss follower skins to the shop. These are normally obtained by defeating bosses. Re-open the shop for changes to take effect." },
                { "GodTearCostName", "God Tear Cost" },
                { "GodTearCostDesc", "God Tears per item. Cost can be increased but not decreased per save." }
            }
        },
        {
            "Japanese", new()
            {
                { "DLCNecklacesName", "DLCネックレス" },
                { "DLCNecklacesDesc", "ウールヘイブンDLCのネックレスをショップに追加します。通常はDLCのゲームプレイで入手できます。変更を反映するにはショップを開き直してください。" },
                { "BossSkinsName", "ボススキン" },
                { "BossSkinsDesc", "ボスのフォロワースキンをショップに追加します。通常はボスを倒すと入手できます。変更を反映するにはショップを開き直してください。" },
                { "GodTearCostName", "神の涙コスト" },
                { "GodTearCostDesc", "アイテムごとの神の涙数。セーブごとにコストは増やせますが、減らせません。" }
            }
        },
        {
            "Russian", new()
            {
                { "DLCNecklacesName", "Ожерелья DLC" },
                { "DLCNecklacesDesc", "Добавить ожерелья DLC Вулхейвен в магазин. Обычно получаются в ходе DLC. Переоткройте магазин для применения изменений." },
                { "BossSkinsName", "Скины боссов" },
                { "BossSkinsDesc", "Добавить скины боссов-последователей в магазин. Обычно получаются за победу над боссами. Переоткройте магазин для применения изменений." },
                { "GodTearCostName", "Стоимость Слезы Бога" },
                { "GodTearCostDesc", "Слёз Бога за предмет. Стоимость можно повысить, но не понизить для сохранения." }
            }
        },
        {
            "French", new()
            {
                { "DLCNecklacesName", "Colliers DLC" },
                { "DLCNecklacesDesc", "Ajouter les colliers du DLC Woolhaven à la boutique. Normalement obtenus via le gameplay du DLC. Rouvrez la boutique pour appliquer les changements." },
                { "BossSkinsName", "Skins de boss" },
                { "BossSkinsDesc", "Ajouter les skins de boss au shop. Normalement obtenus en battant les boss. Rouvrez la boutique pour appliquer les changements." },
                { "GodTearCostName", "Coût en Larmes Divines" },
                { "GodTearCostDesc", "Larmes Divines par objet. Le coût peut être augmenté mais pas diminué par sauvegarde." }
            }
        },
        {
            "German", new()
            {
                { "DLCNecklacesName", "DLC-Halsketten" },
                { "DLCNecklacesDesc", "Woolhaven-DLC-Halsketten zum Shop hinzufügen. Werden normalerweise im DLC-Gameplay erhalten. Shop erneut öffnen, damit Änderungen wirksam werden." },
                { "BossSkinsName", "Boss-Skins" },
                { "BossSkinsDesc", "Boss-Anhänger-Skins zum Shop hinzufügen. Werden normalerweise durch Besiegen von Bossen erhalten. Shop erneut öffnen, damit Änderungen wirksam werden." },
                { "GodTearCostName", "Gottestränen-Kosten" },
                { "GodTearCostDesc", "Gottestränen pro Gegenstand. Kosten können pro Speicherstand erhöht, aber nicht gesenkt werden." }
            }
        },
        {
            "Spanish", new()
            {
                { "DLCNecklacesName", "Collares DLC" },
                { "DLCNecklacesDesc", "Añadir collares del DLC Woolhaven a la tienda. Normalmente se obtienen jugando al DLC. Reabre la tienda para aplicar los cambios." },
                { "BossSkinsName", "Skins de jefes" },
                { "BossSkinsDesc", "Añadir skins de jefes seguidores a la tienda. Normalmente se obtienen al derrotar jefes. Reabre la tienda para aplicar los cambios." },
                { "GodTearCostName", "Coste en Lágrimas Divinas" },
                { "GodTearCostDesc", "Lágrimas Divinas por objeto. El coste puede aumentarse pero no reducirse por partida guardada." }
            }
        },
        {
            "Portuguese (Brazil)", new()
            {
                { "DLCNecklacesName", "Colares DLC" },
                { "DLCNecklacesDesc", "Adicionar colares do DLC Woolhaven à loja. Normalmente obtidos no gameplay do DLC. Reabra a loja para aplicar as alterações." },
                { "BossSkinsName", "Skins de chefes" },
                { "BossSkinsDesc", "Adicionar skins de chefes seguidores à loja. Normalmente obtidas ao derrotar chefes. Reabra a loja para aplicar as alterações." },
                { "GodTearCostName", "Custo em Lágrimas Divinas" },
                { "GodTearCostDesc", "Lágrimas Divinas por item. O custo pode ser aumentado, mas não diminuído por save." }
            }
        },
        {
            "Chinese (Simplified)", new()
            {
                { "DLCNecklacesName", "DLC项链" },
                { "DLCNecklacesDesc", "将羊毛港DLC项链添加到商店。通常通过DLC游戏获得。重新打开商店以使更改生效。" },
                { "BossSkinsName", "Boss皮肤" },
                { "BossSkinsDesc", "将Boss追随者皮肤添加到商店。通常通过击败Boss获得。重新打开商店以使更改生效。" },
                { "GodTearCostName", "神之泪花费" },
                { "GodTearCostDesc", "每件物品需要的神之泪数量。每个存档只能增加花费，不能减少。" }
            }
        },
        {
            "Chinese (Traditional)", new()
            {
                { "DLCNecklacesName", "DLC項鏈" },
                { "DLCNecklacesDesc", "將羊毛港DLC項鏈新增至商店。通常透過DLC遊戲獲得。重新開啟商店以使變更生效。" },
                { "BossSkinsName", "Boss皮膚" },
                { "BossSkinsDesc", "將Boss追隨者皮膚新增至商店。通常透過擊敗Boss獲得。重新開啟商店以使變更生效。" },
                { "GodTearCostName", "神之淚花費" },
                { "GodTearCostDesc", "每件物品需要的神之淚數量。每個存檔只能增加花費，不能減少。" }
            }
        },
        {
            "Korean", new()
            {
                { "DLCNecklacesName", "DLC 목걸이" },
                { "DLCNecklacesDesc", "울헤이븐 DLC 목걸이를 상점에 추가합니다. 보통 DLC 게임플레이에서 획득합니다. 변경 사항을 적용하려면 상점을 다시 여세요." },
                { "BossSkinsName", "보스 스킨" },
                { "BossSkinsDesc", "보스 추종자 스킨을 상점에 추가합니다. 보통 보스를 처치하면 획득합니다. 변경 사항을 적용하려면 상점을 다시 여세요." },
                { "GodTearCostName", "신의 눈물 비용" },
                { "GodTearCostDesc", "아이템당 신의 눈물 수. 세이브별로 비용을 올릴 수 있지만 내릴 수 없습니다." }
            }
        },
        {
            "Italian", new()
            {
                { "DLCNecklacesName", "Collane DLC" },
                { "DLCNecklacesDesc", "Aggiunge le collane del DLC Woolhaven al negozio. Normalmente ottenute nel gameplay del DLC. Riapri il negozio per applicare le modifiche." },
                { "BossSkinsName", "Skin dei boss" },
                { "BossSkinsDesc", "Aggiunge le skin dei boss seguaci al negozio. Normalmente ottenute sconfiggendo i boss. Riapri il negozio per applicare le modifiche." },
                { "GodTearCostName", "Costo Lacrime Divine" },
                { "GodTearCostDesc", "Lacrime Divine per oggetto. Il costo può essere aumentato ma non diminuito per salvataggio." }
            }
        },
        {
            "Dutch", new()
            {
                { "DLCNecklacesName", "DLC-kettingen" },
                { "DLCNecklacesDesc", "Voeg Woolhaven DLC-kettingen toe aan de winkel. Normaal verkregen via DLC-gameplay. Heropen de winkel om wijzigingen toe te passen." },
                { "BossSkinsName", "Baas-skins" },
                { "BossSkinsDesc", "Voeg baas-volgelingskins toe aan de winkel. Normaal verkregen door bazen te verslaan. Heropen de winkel om wijzigingen toe te passen." },
                { "GodTearCostName", "Godstraan-kosten" },
                { "GodTearCostDesc", "Godstranen per item. Kosten kunnen per opslag verhoogd maar niet verlaagd worden." }
            }
        },
        {
            "Turkish", new()
            {
                { "DLCNecklacesName", "DLC Kolyeler" },
                { "DLCNecklacesDesc", "Woolhaven DLC kolyelerini dükkâna ekler. Normalde DLC oynanışıyla elde edilir. Değişikliklerin geçerli olması için dükkânı yeniden açın." },
                { "BossSkinsName", "Patron Görünümleri" },
                { "BossSkinsDesc", "Patron takipçi görünümlerini dükkâna ekler. Normalde patronları yenerek elde edilir. Değişikliklerin geçerli olması için dükkânı yeniden açın." },
                { "GodTearCostName", "Tanrı Gözyaşı Maliyeti" },
                { "GodTearCostDesc", "Eşya başına Tanrı Gözyaşı. Maliyet kayıt başına artırılabilir ama azaltılamaz." }
            }
        },
        {
            "French (Canadian)", new()
            {
                { "DLCNecklacesName", "Colliers DLC" },
                { "DLCNecklacesDesc", "Ajouter les colliers du DLC Woolhaven à la boutique. Normalement obtenus via le gameplay du DLC. Rouvrez la boutique pour appliquer les changements." },
                { "BossSkinsName", "Skins de boss" },
                { "BossSkinsDesc", "Ajouter les skins de boss au shop. Normalement obtenus en battant les boss. Rouvrez la boutique pour appliquer les changements." },
                { "GodTearCostName", "Coût en Larmes Divines" },
                { "GodTearCostDesc", "Larmes Divines par objet. Le coût peut être augmenté mais pas diminué par sauvegarde." }
            }
        },
        {
            "Arabic", new()
            {
                { "DLCNecklacesName", "قلادات المحتوى الإضافي" },
                { "DLCNecklacesDesc", "إضافة قلادات محتوى وولهيفن الإضافي إلى المتجر. عادةً يتم الحصول عليها من خلال لعب المحتوى الإضافي. أعد فتح المتجر لتطبيق التغييرات." },
                { "BossSkinsName", "مظاهر الزعماء" },
                { "BossSkinsDesc", "إضافة مظاهر أتباع الزعماء إلى المتجر. عادةً يتم الحصول عليها بهزيمة الزعماء. أعد فتح المتجر لتطبيق التغييرات." },
                { "GodTearCostName", "تكلفة دموع الإله" },
                { "GodTearCostDesc", "دموع الإله لكل عنصر. يمكن زيادة التكلفة لكن لا يمكن تقليلها لكل حفظ." }
            }
        }
    };
}
