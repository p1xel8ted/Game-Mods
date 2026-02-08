namespace MysticAssistantRedux;

internal static class Localization
{
    // Interaction label
    internal static string MysticAssistantLabel => Get("MysticAssistantLabel");

    // Shop item labels
    internal static string ShopLabelRelic => Get("ShopLabelRelic");
    internal static string ShopLabelSkinApple => Get("ShopLabelSkinApple");
    internal static string ShopLabelDecoApple => Get("ShopLabelDecoApple");
    internal static string ShopLabelOutfitApple => Get("ShopLabelOutfitApple");
    internal static string ShopLabelSkinBoss => Get("ShopLabelSkinBoss");

    // Config display names
    internal static string DLCNecklacesName => Get("DLCNecklacesName");
    internal static string BossSkinsName => Get("BossSkinsName");
    internal static string GodTearCostName => Get("GodTearCostName");

    // Config descriptions
    internal static string DLCNecklacesDesc => Get("DLCNecklacesDesc");
    internal static string BossSkinsDesc => Get("BossSkinsDesc");
    internal static string GodTearCostDesc => Get("GodTearCostDesc");

    // Cost popup messages
    internal static string CostRequiresSave => Get("CostRequiresSave");
    internal static string CostIncreaseConfirm(int from, int to, int slot) => string.Format(Get("CostIncreaseConfirm"), from, to, slot);
    internal static string CostSetConfirm(int cost, int slot) => string.Format(Get("CostSetConfirm"), cost, slot);

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
                { "MysticAssistantLabel", "Mystic Assistant" },
                { "ShopLabelRelic", "Relic" },
                { "ShopLabelSkinApple", "Follower Skin (Apple)" },
                { "ShopLabelDecoApple", "Decoration (Apple)" },
                { "ShopLabelOutfitApple", "Outfit (Apple)" },
                { "ShopLabelSkinBoss", "Follower Skin (Boss)" },
                { "DLCNecklacesName", "DLC Necklaces" },
                { "DLCNecklacesDesc", "Add Woolhaven DLC necklaces to the shop. These are normally obtained through DLC gameplay. Re-open the shop for changes to take effect." },
                { "BossSkinsName", "Boss Skins" },
                { "BossSkinsDesc", "Add boss follower skins to the shop. These are normally obtained by defeating bosses. Re-open the shop for changes to take effect." },
                { "GodTearCostName", "God Tear Cost" },
                { "GodTearCostDesc", "God Tears per item. Cost can be increased but not decreased per save." },
                { "CostRequiresSave", "Please load a save before changing the God Tear cost." },
                { "CostIncreaseConfirm", "Increase cost from {0} to {1} God Tears per item for save slot {2}?\n\nThis cannot be reversed." },
                { "CostSetConfirm", "Set cost to {0} God Tears per item for save slot {1}?\n\nCost can only be increased after this, not decreased." }
            }
        },
        {
            "Japanese", new()
            {
                { "MysticAssistantLabel", "ミスティックアシスタント" },
                { "ShopLabelRelic", "レリック" },
                { "ShopLabelSkinApple", "フォロワースキン (Apple)" },
                { "ShopLabelDecoApple", "デコレーション (Apple)" },
                { "ShopLabelOutfitApple", "衣装 (Apple)" },
                { "ShopLabelSkinBoss", "フォロワースキン (ボス)" },
                { "DLCNecklacesName", "DLCネックレス" },
                { "DLCNecklacesDesc", "ウールヘイブンDLCのネックレスをショップに追加します。通常はDLCのゲームプレイで入手できます。変更を反映するにはショップを開き直してください。" },
                { "BossSkinsName", "ボススキン" },
                { "BossSkinsDesc", "ボスのフォロワースキンをショップに追加します。通常はボスを倒すと入手できます。変更を反映するにはショップを開き直してください。" },
                { "GodTearCostName", "神の涙コスト" },
                { "GodTearCostDesc", "アイテムごとの神の涙数。セーブごとにコストは増やせますが、減らせません。" },
                { "CostRequiresSave", "神の涙コストを変更するには、セーブデータをロードしてください。" },
                { "CostIncreaseConfirm", "セーブスロット{2}のアイテムあたりのコストを{0}から{1}神の涙に増やしますか？\n\nこの操作は元に戻せません。" },
                { "CostSetConfirm", "セーブスロット{1}のアイテムあたりのコストを{0}神の涙に設定しますか？\n\n設定後はコストの増加のみ可能で、減少はできません。" }
            }
        },
        {
            "Russian", new()
            {
                { "MysticAssistantLabel", "Мистический помощник" },
                { "ShopLabelRelic", "Реликвия" },
                { "ShopLabelSkinApple", "Облик последователя (Apple)" },
                { "ShopLabelDecoApple", "Украшение (Apple)" },
                { "ShopLabelOutfitApple", "Наряд (Apple)" },
                { "ShopLabelSkinBoss", "Облик последователя (Босс)" },
                { "DLCNecklacesName", "Ожерелья DLC" },
                { "DLCNecklacesDesc", "Добавить ожерелья DLC Вулхейвен в магазин. Обычно получаются в ходе DLC. Переоткройте магазин для применения изменений." },
                { "BossSkinsName", "Скины боссов" },
                { "BossSkinsDesc", "Добавить скины боссов-последователей в магазин. Обычно получаются за победу над боссами. Переоткройте магазин для применения изменений." },
                { "GodTearCostName", "Стоимость Слезы Бога" },
                { "GodTearCostDesc", "Слёз Бога за предмет. Стоимость можно повысить, но не понизить для сохранения." },
                { "CostRequiresSave", "Пожалуйста, загрузите сохранение перед изменением стоимости Слёз Бога." },
                { "CostIncreaseConfirm", "Увеличить стоимость с {0} до {1} Слёз Бога за предмет для слота {2}?\n\nЭто действие необратимо." },
                { "CostSetConfirm", "Установить стоимость {0} Слёз Бога за предмет для слота {1}?\n\nПосле установки стоимость можно только повысить, но не понизить." }
            }
        },
        {
            "French", new()
            {
                { "MysticAssistantLabel", "Assistant mystique" },
                { "ShopLabelRelic", "Relique" },
                { "ShopLabelSkinApple", "Apparence de suiveur (Apple)" },
                { "ShopLabelDecoApple", "Décoration (Apple)" },
                { "ShopLabelOutfitApple", "Tenue (Apple)" },
                { "ShopLabelSkinBoss", "Apparence de suiveur (Boss)" },
                { "DLCNecklacesName", "Colliers DLC" },
                { "DLCNecklacesDesc", "Ajouter les colliers du DLC Woolhaven à la boutique. Normalement obtenus via le gameplay du DLC. Rouvrez la boutique pour appliquer les changements." },
                { "BossSkinsName", "Skins de boss" },
                { "BossSkinsDesc", "Ajouter les skins de boss au shop. Normalement obtenus en battant les boss. Rouvrez la boutique pour appliquer les changements." },
                { "GodTearCostName", "Coût en Larmes Divines" },
                { "GodTearCostDesc", "Larmes Divines par objet. Le coût peut être augmenté mais pas diminué par sauvegarde." },
                { "CostRequiresSave", "Veuillez charger une sauvegarde avant de modifier le coût en Larmes Divines." },
                { "CostIncreaseConfirm", "Augmenter le coût de {0} à {1} Larmes Divines par objet pour le slot {2} ?\n\nCette action est irréversible." },
                { "CostSetConfirm", "Définir le coût à {0} Larmes Divines par objet pour le slot {1} ?\n\nLe coût ne pourra qu'être augmenté après cela, pas diminué." }
            }
        },
        {
            "German", new()
            {
                { "MysticAssistantLabel", "Mystischer Assistent" },
                { "ShopLabelRelic", "Relikt" },
                { "ShopLabelSkinApple", "Anhänger-Skin (Apple)" },
                { "ShopLabelDecoApple", "Dekoration (Apple)" },
                { "ShopLabelOutfitApple", "Outfit (Apple)" },
                { "ShopLabelSkinBoss", "Anhänger-Skin (Boss)" },
                { "DLCNecklacesName", "DLC-Halsketten" },
                { "DLCNecklacesDesc", "Woolhaven-DLC-Halsketten zum Shop hinzufügen. Werden normalerweise im DLC-Gameplay erhalten. Shop erneut öffnen, damit Änderungen wirksam werden." },
                { "BossSkinsName", "Boss-Skins" },
                { "BossSkinsDesc", "Boss-Anhänger-Skins zum Shop hinzufügen. Werden normalerweise durch Besiegen von Bossen erhalten. Shop erneut öffnen, damit Änderungen wirksam werden." },
                { "GodTearCostName", "Gottestränen-Kosten" },
                { "GodTearCostDesc", "Gottestränen pro Gegenstand. Kosten können pro Speicherstand erhöht, aber nicht gesenkt werden." },
                { "CostRequiresSave", "Bitte laden Sie einen Spielstand, bevor Sie die Gottestränen-Kosten ändern." },
                { "CostIncreaseConfirm", "Kosten von {0} auf {1} Gottestränen pro Gegenstand für Speicherplatz {2} erhöhen?\n\nDies kann nicht rückgängig gemacht werden." },
                { "CostSetConfirm", "Kosten auf {0} Gottestränen pro Gegenstand für Speicherplatz {1} festlegen?\n\nDanach können die Kosten nur erhöht, nicht gesenkt werden." }
            }
        },
        {
            "Spanish", new()
            {
                { "MysticAssistantLabel", "Asistente místico" },
                { "ShopLabelRelic", "Reliquia" },
                { "ShopLabelSkinApple", "Aspecto de seguidor (Apple)" },
                { "ShopLabelDecoApple", "Decoración (Apple)" },
                { "ShopLabelOutfitApple", "Atuendo (Apple)" },
                { "ShopLabelSkinBoss", "Aspecto de seguidor (Jefe)" },
                { "DLCNecklacesName", "Collares DLC" },
                { "DLCNecklacesDesc", "Añadir collares del DLC Woolhaven a la tienda. Normalmente se obtienen jugando al DLC. Reabre la tienda para aplicar los cambios." },
                { "BossSkinsName", "Skins de jefes" },
                { "BossSkinsDesc", "Añadir skins de jefes seguidores a la tienda. Normalmente se obtienen al derrotar jefes. Reabre la tienda para aplicar los cambios." },
                { "GodTearCostName", "Coste en Lágrimas Divinas" },
                { "GodTearCostDesc", "Lágrimas Divinas por objeto. El coste puede aumentarse pero no reducirse por partida guardada." },
                { "CostRequiresSave", "Por favor, cargue una partida antes de cambiar el coste en Lágrimas Divinas." },
                { "CostIncreaseConfirm", "¿Aumentar el coste de {0} a {1} Lágrimas Divinas por objeto para la ranura {2}?\n\nEsto no se puede revertir." },
                { "CostSetConfirm", "¿Establecer el coste en {0} Lágrimas Divinas por objeto para la ranura {1}?\n\nDespués de esto, el coste solo puede aumentarse, no reducirse." }
            }
        },
        {
            "Portuguese (Brazil)", new()
            {
                { "MysticAssistantLabel", "Assistente místico" },
                { "ShopLabelRelic", "Relíquia" },
                { "ShopLabelSkinApple", "Aparência de seguidor (Apple)" },
                { "ShopLabelDecoApple", "Decoração (Apple)" },
                { "ShopLabelOutfitApple", "Traje (Apple)" },
                { "ShopLabelSkinBoss", "Aparência de seguidor (Chefe)" },
                { "DLCNecklacesName", "Colares DLC" },
                { "DLCNecklacesDesc", "Adicionar colares do DLC Woolhaven à loja. Normalmente obtidos no gameplay do DLC. Reabra a loja para aplicar as alterações." },
                { "BossSkinsName", "Skins de chefes" },
                { "BossSkinsDesc", "Adicionar skins de chefes seguidores à loja. Normalmente obtidas ao derrotar chefes. Reabra a loja para aplicar as alterações." },
                { "GodTearCostName", "Custo em Lágrimas Divinas" },
                { "GodTearCostDesc", "Lágrimas Divinas por item. O custo pode ser aumentado, mas não diminuído por save." },
                { "CostRequiresSave", "Por favor, carregue um save antes de alterar o custo em Lágrimas Divinas." },
                { "CostIncreaseConfirm", "Aumentar o custo de {0} para {1} Lágrimas Divinas por item para o slot {2}?\n\nIsso não pode ser revertido." },
                { "CostSetConfirm", "Definir o custo em {0} Lágrimas Divinas por item para o slot {1}?\n\nApós isso, o custo só pode ser aumentado, não diminuído." }
            }
        },
        {
            "Chinese (Simplified)", new()
            {
                { "MysticAssistantLabel", "神秘助手" },
                { "ShopLabelRelic", "遗物" },
                { "ShopLabelSkinApple", "追随者皮肤 (Apple)" },
                { "ShopLabelDecoApple", "装饰 (Apple)" },
                { "ShopLabelOutfitApple", "服装 (Apple)" },
                { "ShopLabelSkinBoss", "追随者皮肤 (Boss)" },
                { "DLCNecklacesName", "DLC项链" },
                { "DLCNecklacesDesc", "将羊毛港DLC项链添加到商店。通常通过DLC游戏获得。重新打开商店以使更改生效。" },
                { "BossSkinsName", "Boss皮肤" },
                { "BossSkinsDesc", "将Boss追随者皮肤添加到商店。通常通过击败Boss获得。重新打开商店以使更改生效。" },
                { "GodTearCostName", "神之泪花费" },
                { "GodTearCostDesc", "每件物品需要的神之泪数量。每个存档只能增加花费，不能减少。" },
                { "CostRequiresSave", "请先加载存档再更改神之泪花费。" },
                { "CostIncreaseConfirm", "将存档位{2}的每件物品花费从{0}增加到{1}神之泪？\n\n此操作无法撤销。" },
                { "CostSetConfirm", "将存档位{1}的每件物品花费设置为{0}神之泪？\n\n设置后只能增加花费，不能减少。" }
            }
        },
        {
            "Chinese (Traditional)", new()
            {
                { "MysticAssistantLabel", "神秘助手" },
                { "ShopLabelRelic", "遺物" },
                { "ShopLabelSkinApple", "追隨者皮膚 (Apple)" },
                { "ShopLabelDecoApple", "裝飾 (Apple)" },
                { "ShopLabelOutfitApple", "服裝 (Apple)" },
                { "ShopLabelSkinBoss", "追隨者皮膚 (Boss)" },
                { "DLCNecklacesName", "DLC項鏈" },
                { "DLCNecklacesDesc", "將羊毛港DLC項鏈新增至商店。通常透過DLC遊戲獲得。重新開啟商店以使變更生效。" },
                { "BossSkinsName", "Boss皮膚" },
                { "BossSkinsDesc", "將Boss追隨者皮膚新增至商店。通常透過擊敗Boss獲得。重新開啟商店以使變更生效。" },
                { "GodTearCostName", "神之淚花費" },
                { "GodTearCostDesc", "每件物品需要的神之淚數量。每個存檔只能增加花費，不能減少。" },
                { "CostRequiresSave", "請先載入存檔再更改神之淚花費。" },
                { "CostIncreaseConfirm", "將存檔位{2}的每件物品花費從{0}增加到{1}神之淚？\n\n此操作無法撤銷。" },
                { "CostSetConfirm", "將存檔位{1}的每件物品花費設置為{0}神之淚？\n\n設置後只能增加花費，不能減少。" }
            }
        },
        {
            "Korean", new()
            {
                { "MysticAssistantLabel", "미스틱 어시스턴트" },
                { "ShopLabelRelic", "유물" },
                { "ShopLabelSkinApple", "추종자 스킨 (Apple)" },
                { "ShopLabelDecoApple", "장식 (Apple)" },
                { "ShopLabelOutfitApple", "의상 (Apple)" },
                { "ShopLabelSkinBoss", "추종자 스킨 (보스)" },
                { "DLCNecklacesName", "DLC 목걸이" },
                { "DLCNecklacesDesc", "울헤이븐 DLC 목걸이를 상점에 추가합니다. 보통 DLC 게임플레이에서 획득합니다. 변경 사항을 적용하려면 상점을 다시 여세요." },
                { "BossSkinsName", "보스 스킨" },
                { "BossSkinsDesc", "보스 추종자 스킨을 상점에 추가합니다. 보통 보스를 처치하면 획득합니다. 변경 사항을 적용하려면 상점을 다시 여세요." },
                { "GodTearCostName", "신의 눈물 비용" },
                { "GodTearCostDesc", "아이템당 신의 눈물 수. 세이브별로 비용을 올릴 수 있지만 내릴 수 없습니다." },
                { "CostRequiresSave", "신의 눈물 비용을 변경하려면 먼저 세이브를 로드해 주세요." },
                { "CostIncreaseConfirm", "슬롯 {2}의 아이템당 비용을 {0}에서 {1} 신의 눈물로 올리시겠습니까?\n\n이 작업은 되돌릴 수 없습니다." },
                { "CostSetConfirm", "슬롯 {1}의 아이템당 비용을 {0} 신의 눈물로 설정하시겠습니까?\n\n설정 후에는 비용을 올릴 수만 있고 내릴 수 없습니다." }
            }
        },
        {
            "Italian", new()
            {
                { "MysticAssistantLabel", "Assistente mistico" },
                { "ShopLabelRelic", "Reliquia" },
                { "ShopLabelSkinApple", "Aspetto seguace (Apple)" },
                { "ShopLabelDecoApple", "Decorazione (Apple)" },
                { "ShopLabelOutfitApple", "Vestito (Apple)" },
                { "ShopLabelSkinBoss", "Aspetto seguace (Boss)" },
                { "DLCNecklacesName", "Collane DLC" },
                { "DLCNecklacesDesc", "Aggiunge le collane del DLC Woolhaven al negozio. Normalmente ottenute nel gameplay del DLC. Riapri il negozio per applicare le modifiche." },
                { "BossSkinsName", "Skin dei boss" },
                { "BossSkinsDesc", "Aggiunge le skin dei boss seguaci al negozio. Normalmente ottenute sconfiggendo i boss. Riapri il negozio per applicare le modifiche." },
                { "GodTearCostName", "Costo Lacrime Divine" },
                { "GodTearCostDesc", "Lacrime Divine per oggetto. Il costo può essere aumentato ma non diminuito per salvataggio." },
                { "CostRequiresSave", "Carica un salvataggio prima di modificare il costo delle Lacrime Divine." },
                { "CostIncreaseConfirm", "Aumentare il costo da {0} a {1} Lacrime Divine per oggetto per lo slot {2}?\n\nQuesta azione non può essere annullata." },
                { "CostSetConfirm", "Impostare il costo a {0} Lacrime Divine per oggetto per lo slot {1}?\n\nDopo questa operazione il costo può solo essere aumentato, non diminuito." }
            }
        },
        {
            "Dutch", new()
            {
                { "MysticAssistantLabel", "Mystieke assistent" },
                { "ShopLabelRelic", "Relikwie" },
                { "ShopLabelSkinApple", "Volgelingskin (Apple)" },
                { "ShopLabelDecoApple", "Decoratie (Apple)" },
                { "ShopLabelOutfitApple", "Outfit (Apple)" },
                { "ShopLabelSkinBoss", "Volgelingskin (Baas)" },
                { "DLCNecklacesName", "DLC-kettingen" },
                { "DLCNecklacesDesc", "Voeg Woolhaven DLC-kettingen toe aan de winkel. Normaal verkregen via DLC-gameplay. Heropen de winkel om wijzigingen toe te passen." },
                { "BossSkinsName", "Baas-skins" },
                { "BossSkinsDesc", "Voeg baas-volgelingskins toe aan de winkel. Normaal verkregen door bazen te verslaan. Heropen de winkel om wijzigingen toe te passen." },
                { "GodTearCostName", "Godstraan-kosten" },
                { "GodTearCostDesc", "Godstranen per item. Kosten kunnen per opslag verhoogd maar niet verlaagd worden." },
                { "CostRequiresSave", "Laad een opslag voordat u de Godstraan-kosten wijzigt." },
                { "CostIncreaseConfirm", "Kosten verhogen van {0} naar {1} Godstranen per item voor opslagslot {2}?\n\nDit kan niet ongedaan worden gemaakt." },
                { "CostSetConfirm", "Kosten instellen op {0} Godstranen per item voor opslagslot {1}?\n\nNa instelling kunnen de kosten alleen verhoogd, niet verlaagd worden." }
            }
        },
        {
            "Turkish", new()
            {
                { "MysticAssistantLabel", "Mistik Asistan" },
                { "ShopLabelRelic", "Kalıntı" },
                { "ShopLabelSkinApple", "Takipçi Görünümü (Apple)" },
                { "ShopLabelDecoApple", "Dekorasyon (Apple)" },
                { "ShopLabelOutfitApple", "Kıyafet (Apple)" },
                { "ShopLabelSkinBoss", "Takipçi Görünümü (Patron)" },
                { "DLCNecklacesName", "DLC Kolyeler" },
                { "DLCNecklacesDesc", "Woolhaven DLC kolyelerini dükkâna ekler. Normalde DLC oynanışıyla elde edilir. Değişikliklerin geçerli olması için dükkânı yeniden açın." },
                { "BossSkinsName", "Patron Görünümleri" },
                { "BossSkinsDesc", "Patron takipçi görünümlerini dükkâna ekler. Normalde patronları yenerek elde edilir. Değişikliklerin geçerli olması için dükkânı yeniden açın." },
                { "GodTearCostName", "Tanrı Gözyaşı Maliyeti" },
                { "GodTearCostDesc", "Eşya başına Tanrı Gözyaşı. Maliyet kayıt başına artırılabilir ama azaltılamaz." },
                { "CostRequiresSave", "Tanrı Gözyaşı maliyetini değiştirmeden önce lütfen bir kayıt yükleyin." },
                { "CostIncreaseConfirm", "Kayıt yuvası {2} için eşya başına maliyeti {0}'den {1} Tanrı Gözyaşı'na yükseltilsin mi?\n\nBu işlem geri alınamaz." },
                { "CostSetConfirm", "Kayıt yuvası {1} için eşya başına maliyet {0} Tanrı Gözyaşı olarak ayarlansın mı?\n\nBundan sonra maliyet yalnızca artırılabilir, azaltılamaz." }
            }
        },
        {
            "French (Canadian)", new()
            {
                { "MysticAssistantLabel", "Assistant mystique" },
                { "ShopLabelRelic", "Relique" },
                { "ShopLabelSkinApple", "Apparence de suiveur (Apple)" },
                { "ShopLabelDecoApple", "Décoration (Apple)" },
                { "ShopLabelOutfitApple", "Tenue (Apple)" },
                { "ShopLabelSkinBoss", "Apparence de suiveur (Boss)" },
                { "DLCNecklacesName", "Colliers DLC" },
                { "DLCNecklacesDesc", "Ajouter les colliers du DLC Woolhaven à la boutique. Normalement obtenus via le gameplay du DLC. Rouvrez la boutique pour appliquer les changements." },
                { "BossSkinsName", "Skins de boss" },
                { "BossSkinsDesc", "Ajouter les skins de boss au shop. Normalement obtenus en battant les boss. Rouvrez la boutique pour appliquer les changements." },
                { "GodTearCostName", "Coût en Larmes Divines" },
                { "GodTearCostDesc", "Larmes Divines par objet. Le coût peut être augmenté mais pas diminué par sauvegarde." },
                { "CostRequiresSave", "Veuillez charger une sauvegarde avant de modifier le coût en Larmes Divines." },
                { "CostIncreaseConfirm", "Augmenter le coût de {0} à {1} Larmes Divines par objet pour le slot {2} ?\n\nCette action est irréversible." },
                { "CostSetConfirm", "Définir le coût à {0} Larmes Divines par objet pour le slot {1} ?\n\nLe coût ne pourra qu'être augmenté après cela, pas diminué." }
            }
        },
        {
            "Arabic", new()
            {
                { "MysticAssistantLabel", "المساعد الغامض" },
                { "ShopLabelRelic", "أثر" },
                { "ShopLabelSkinApple", "مظهر التابع (Apple)" },
                { "ShopLabelDecoApple", "زينة (Apple)" },
                { "ShopLabelOutfitApple", "زي (Apple)" },
                { "ShopLabelSkinBoss", "مظهر التابع (زعيم)" },
                { "DLCNecklacesName", "قلادات المحتوى الإضافي" },
                { "DLCNecklacesDesc", "إضافة قلادات محتوى وولهيفن الإضافي إلى المتجر. عادةً يتم الحصول عليها من خلال لعب المحتوى الإضافي. أعد فتح المتجر لتطبيق التغييرات." },
                { "BossSkinsName", "مظاهر الزعماء" },
                { "BossSkinsDesc", "إضافة مظاهر أتباع الزعماء إلى المتجر. عادةً يتم الحصول عليها بهزيمة الزعماء. أعد فتح المتجر لتطبيق التغييرات." },
                { "GodTearCostName", "تكلفة دموع الإله" },
                { "GodTearCostDesc", "دموع الإله لكل عنصر. يمكن زيادة التكلفة لكن لا يمكن تقليلها لكل حفظ." },
                { "CostRequiresSave", "يرجى تحميل حفظ قبل تغيير تكلفة دموع الإله." },
                { "CostIncreaseConfirm", "زيادة التكلفة من {0} إلى {1} دموع الإله لكل عنصر لفتحة الحفظ {2}؟\n\nلا يمكن التراجع عن هذا الإجراء." },
                { "CostSetConfirm", "تعيين التكلفة إلى {0} دموع الإله لكل عنصر لفتحة الحفظ {1}؟\n\nبعد ذلك يمكن فقط زيادة التكلفة وليس تقليلها." }
            }
        }
    };
}
