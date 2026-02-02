namespace Namify;

internal static class Localization
{
    // Button labels (dynamic - updated every GUI frame)
    internal static string AddNameButton => Get("AddNameButton");
    internal static string OpenNamifyList => Get("OpenNamifyList");
    internal static string OpenUserList => Get("OpenUserList");
    internal static string GenerateNewNames => Get("GenerateNewNames");
    internal static string ReloadNamesFromFile => Get("ReloadNamesFromFile");
    internal static string Yes => Get("Yes");
    internal static string No => Get("No");

    // Confirmation dialogs
    internal static string ConfirmGenerateNew => Get("ConfirmGenerateNew");
    internal static string ConfirmReload => Get("ConfirmReload");

    // Popup messages
    internal static string NamesReloaded => Get("NamesReloaded");
    internal static string ErrorReloading => Get("ErrorReloading");
    internal static string NewNamesGenerated => Get("NewNamesGenerated");
    internal static string ErrorGenerating => Get("ErrorGenerating");
    internal static string NameAdded => Get("NameAdded");
    internal static string NameExists => Get("NameExists");
    internal static string NoNameEntered => Get("NoNameEntered");
    internal static string FileNotFound => Get("FileNotFound");
    internal static string ApiError => Get("ApiError");
    internal static string ApiBackupError => Get("ApiBackupError");
    internal static string NamesRetrieved => Get("NamesRetrieved");
    internal static string NamesRetrievedBackup => Get("NamesRetrievedBackup");

    // Config descriptions (read at startup, won't update mid-session)
    internal static string DescAsteriskNames => Get("DescAsteriskNames");
    internal static string DescAddName => Get("DescAddName");
    internal static string DescAddNameButton => Get("DescAddNameButton");
    internal static string DescOpenNamifyFile => Get("DescOpenNamifyFile");
    internal static string DescOpenUserFile => Get("DescOpenUserFile");
    internal static string DescGenerateNew => Get("DescGenerateNew");
    internal static string DescReloadNames => Get("DescReloadNames");
    internal static string DescApiKey => Get("DescApiKey");

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
                // Buttons
                { "AddNameButton", "Add Name" },
                { "OpenNamifyList", "Open Namify List" },
                { "OpenUserList", "Open User List" },
                { "GenerateNewNames", "Generate New Names" },
                { "ReloadNamesFromFile", "Reload Names From File" },
                { "Yes", "Yes" },
                { "No", "No" },
                // Dialogs
                { "ConfirmGenerateNew", "Are you sure you want to generate new names?" },
                { "ConfirmReload", "Are you sure you want to reload names from file?" },
                // Popups
                { "NamesReloaded", "Names reloaded from file!" },
                { "ErrorReloading", "Error reloading names. Check log for details." },
                { "NewNamesGenerated", "New names generated!" },
                { "ErrorGenerating", "Error generating new names!" },
                { "NameAdded", "Added '{0}' to available names!" },
                { "NameExists", "'{0}' already exists!" },
                { "NoNameEntered", "You haven't entered a name to add?" },
                { "FileNotFound", "File not found: {0}" },
                { "ApiError", "Error retrieving names! Trying backup source..." },
                { "ApiBackupError", "Error retrieving names from backup source!" },
                { "NamesRetrieved", "Names retrieved for Namify!" },
                { "NamesRetrievedBackup", "Names retrieved from backup source!" },
                // Descriptions
                { "DescAsteriskNames", "Show an asterisk next to Namify-generated names in the indoctrination UI." },
                { "DescAddName", "Enter a custom name to add to your personal name list." },
                { "DescAddNameButton", "Add the name entered above to your personal list." },
                { "DescOpenNamifyFile", "Opens the API-generated names file for viewing/editing." },
                { "DescOpenUserFile", "Opens your personal names file for viewing/editing." },
                { "DescGenerateNew", "Fetches a fresh batch of names from the API. Your personal names are preserved." },
                { "DescReloadNames", "Reloads names from file (useful after manual edits)." },
                { "DescApiKey", "The default key is shared and limited to 1000 requests/day. Get your own free key at https://randommer.io/" }
            }
        },
        {
            "French", new()
            {
                { "AddNameButton", "Ajouter un nom" },
                { "OpenNamifyList", "Ouvrir la liste Namify" },
                { "OpenUserList", "Ouvrir ma liste" },
                { "GenerateNewNames", "Générer de nouveaux noms" },
                { "ReloadNamesFromFile", "Recharger depuis le fichier" },
                { "Yes", "Oui" },
                { "No", "Non" },
                { "ConfirmGenerateNew", "Voulez-vous vraiment générer de nouveaux noms?" },
                { "ConfirmReload", "Voulez-vous vraiment recharger les noms depuis le fichier?" },
                { "NamesReloaded", "Noms rechargés depuis le fichier!" },
                { "ErrorReloading", "Erreur lors du rechargement. Consultez le journal." },
                { "NewNamesGenerated", "Nouveaux noms générés!" },
                { "ErrorGenerating", "Erreur lors de la génération!" },
                { "NameAdded", "'{0}' ajouté aux noms disponibles!" },
                { "NameExists", "'{0}' existe déjà!" },
                { "NoNameEntered", "Vous n'avez pas entré de nom?" },
                { "FileNotFound", "Fichier non trouvé: {0}" },
                { "ApiError", "Erreur de récupération! Essai de la source de secours..." },
                { "ApiBackupError", "Erreur de récupération depuis la source de secours!" },
                { "NamesRetrieved", "Noms récupérés pour Namify!" },
                { "NamesRetrievedBackup", "Noms récupérés depuis la source de secours!" },
                { "DescAsteriskNames", "Afficher un astérisque à côté des noms générés par Namify." },
                { "DescAddName", "Entrez un nom personnalisé à ajouter à votre liste." },
                { "DescAddNameButton", "Ajouter le nom ci-dessus à votre liste personnelle." },
                { "DescOpenNamifyFile", "Ouvre le fichier des noms générés par l'API." },
                { "DescOpenUserFile", "Ouvre votre fichier de noms personnels." },
                { "DescGenerateNew", "Récupère de nouveaux noms depuis l'API. Vos noms personnels sont conservés." },
                { "DescReloadNames", "Recharge les noms depuis le fichier (utile après modification manuelle)." },
                { "DescApiKey", "La clé par défaut est partagée et limitée à 1000 requêtes/jour. Obtenez votre clé gratuite sur https://randommer.io/" }
            }
        },
        {
            "German", new()
            {
                { "AddNameButton", "Name hinzufügen" },
                { "OpenNamifyList", "Namify-Liste öffnen" },
                { "OpenUserList", "Benutzerliste öffnen" },
                { "GenerateNewNames", "Neue Namen generieren" },
                { "ReloadNamesFromFile", "Namen aus Datei laden" },
                { "Yes", "Ja" },
                { "No", "Nein" },
                { "ConfirmGenerateNew", "Möchten Sie wirklich neue Namen generieren?" },
                { "ConfirmReload", "Möchten Sie wirklich Namen aus der Datei laden?" },
                { "NamesReloaded", "Namen aus Datei geladen!" },
                { "ErrorReloading", "Fehler beim Laden. Siehe Protokoll." },
                { "NewNamesGenerated", "Neue Namen generiert!" },
                { "ErrorGenerating", "Fehler beim Generieren!" },
                { "NameAdded", "'{0}' zu verfügbaren Namen hinzugefügt!" },
                { "NameExists", "'{0}' existiert bereits!" },
                { "NoNameEntered", "Sie haben keinen Namen eingegeben?" },
                { "FileNotFound", "Datei nicht gefunden: {0}" },
                { "ApiError", "Abruffehler! Versuche Backup-Quelle..." },
                { "ApiBackupError", "Fehler beim Abruf von Backup-Quelle!" },
                { "NamesRetrieved", "Namen für Namify abgerufen!" },
                { "NamesRetrievedBackup", "Namen von Backup-Quelle abgerufen!" },
                { "DescAsteriskNames", "Zeigt ein Sternchen neben Namify-generierten Namen an." },
                { "DescAddName", "Geben Sie einen benutzerdefinierten Namen ein." },
                { "DescAddNameButton", "Fügt den obigen Namen zu Ihrer Liste hinzu." },
                { "DescOpenNamifyFile", "Öffnet die API-generierte Namensdatei." },
                { "DescOpenUserFile", "Öffnet Ihre persönliche Namensdatei." },
                { "DescGenerateNew", "Ruft neue Namen von der API ab. Ihre Namen bleiben erhalten." },
                { "DescReloadNames", "Lädt Namen aus Datei neu (nützlich nach manueller Bearbeitung)." },
                { "DescApiKey", "Der Standardschlüssel ist geteilt und auf 1000 Anfragen/Tag begrenzt. Holen Sie sich Ihren kostenlosen Schlüssel auf https://randommer.io/" }
            }
        },
        {
            "Spanish", new()
            {
                { "AddNameButton", "Añadir nombre" },
                { "OpenNamifyList", "Abrir lista Namify" },
                { "OpenUserList", "Abrir mi lista" },
                { "GenerateNewNames", "Generar nuevos nombres" },
                { "ReloadNamesFromFile", "Recargar desde archivo" },
                { "Yes", "Sí" },
                { "No", "No" },
                { "ConfirmGenerateNew", "¿Seguro que quieres generar nuevos nombres?" },
                { "ConfirmReload", "¿Seguro que quieres recargar los nombres desde el archivo?" },
                { "NamesReloaded", "¡Nombres recargados desde el archivo!" },
                { "ErrorReloading", "Error al recargar. Consulta el registro." },
                { "NewNamesGenerated", "¡Nuevos nombres generados!" },
                { "ErrorGenerating", "¡Error al generar!" },
                { "NameAdded", "¡'{0}' añadido a los nombres disponibles!" },
                { "NameExists", "¡'{0}' ya existe!" },
                { "NoNameEntered", "¿No has introducido un nombre?" },
                { "FileNotFound", "Archivo no encontrado: {0}" },
                { "ApiError", "¡Error de recuperación! Probando fuente de respaldo..." },
                { "ApiBackupError", "¡Error de recuperación desde fuente de respaldo!" },
                { "NamesRetrieved", "¡Nombres recuperados para Namify!" },
                { "NamesRetrievedBackup", "¡Nombres recuperados desde fuente de respaldo!" },
                { "DescAsteriskNames", "Muestra un asterisco junto a los nombres generados por Namify." },
                { "DescAddName", "Introduce un nombre personalizado para añadir a tu lista." },
                { "DescAddNameButton", "Añade el nombre de arriba a tu lista personal." },
                { "DescOpenNamifyFile", "Abre el archivo de nombres generados por la API." },
                { "DescOpenUserFile", "Abre tu archivo de nombres personales." },
                { "DescGenerateNew", "Obtiene nuevos nombres de la API. Tus nombres se conservan." },
                { "DescReloadNames", "Recarga nombres desde el archivo (útil después de editar manualmente)." },
                { "DescApiKey", "La clave predeterminada es compartida y limitada a 1000 solicitudes/día. Obtén tu clave gratuita en https://randommer.io/" }
            }
        },
        {
            "Japanese", new()
            {
                { "AddNameButton", "名前を追加" },
                { "OpenNamifyList", "Namifyリストを開く" },
                { "OpenUserList", "ユーザーリストを開く" },
                { "GenerateNewNames", "新しい名前を生成" },
                { "ReloadNamesFromFile", "ファイルから再読み込み" },
                { "Yes", "はい" },
                { "No", "いいえ" },
                { "ConfirmGenerateNew", "新しい名前を生成してもよろしいですか？" },
                { "ConfirmReload", "ファイルから名前を再読み込みしてもよろしいですか？" },
                { "NamesReloaded", "ファイルから名前を再読み込みしました！" },
                { "ErrorReloading", "再読み込みエラー。ログを確認してください。" },
                { "NewNamesGenerated", "新しい名前を生成しました！" },
                { "ErrorGenerating", "生成エラー！" },
                { "NameAdded", "'{0}'を利用可能な名前に追加しました！" },
                { "NameExists", "'{0}'は既に存在します！" },
                { "NoNameEntered", "名前が入力されていません？" },
                { "FileNotFound", "ファイルが見つかりません: {0}" },
                { "ApiError", "取得エラー！バックアップソースを試しています..." },
                { "ApiBackupError", "バックアップソースからの取得エラー！" },
                { "NamesRetrieved", "Namify用の名前を取得しました！" },
                { "NamesRetrievedBackup", "バックアップソースから名前を取得しました！" },
                { "DescAsteriskNames", "Namifyで生成された名前の横にアスタリスクを表示します。" },
                { "DescAddName", "リストに追加するカスタム名を入力してください。" },
                { "DescAddNameButton", "上記の名前を個人リストに追加します。" },
                { "DescOpenNamifyFile", "API生成の名前ファイルを開きます。" },
                { "DescOpenUserFile", "個人の名前ファイルを開きます。" },
                { "DescGenerateNew", "APIから新しい名前を取得します。個人の名前は保持されます。" },
                { "DescReloadNames", "ファイルから名前を再読み込みします（手動編集後に便利）。" },
                { "DescApiKey", "デフォルトキーは共有され、1日1000リクエストに制限されています。https://randommer.io/で無料キーを取得してください。" }
            }
        },
        {
            "Russian", new()
            {
                { "AddNameButton", "Добавить имя" },
                { "OpenNamifyList", "Открыть список Namify" },
                { "OpenUserList", "Открыть мой список" },
                { "GenerateNewNames", "Создать новые имена" },
                { "ReloadNamesFromFile", "Перезагрузить из файла" },
                { "Yes", "Да" },
                { "No", "Нет" },
                { "ConfirmGenerateNew", "Вы уверены, что хотите создать новые имена?" },
                { "ConfirmReload", "Вы уверены, что хотите перезагрузить имена из файла?" },
                { "NamesReloaded", "Имена перезагружены из файла!" },
                { "ErrorReloading", "Ошибка перезагрузки. Проверьте журнал." },
                { "NewNamesGenerated", "Новые имена созданы!" },
                { "ErrorGenerating", "Ошибка создания!" },
                { "NameAdded", "'{0}' добавлено в доступные имена!" },
                { "NameExists", "'{0}' уже существует!" },
                { "NoNameEntered", "Вы не ввели имя?" },
                { "FileNotFound", "Файл не найден: {0}" },
                { "ApiError", "Ошибка получения! Пробуем резервный источник..." },
                { "ApiBackupError", "Ошибка получения из резервного источника!" },
                { "NamesRetrieved", "Имена получены для Namify!" },
                { "NamesRetrievedBackup", "Имена получены из резервного источника!" },
                { "DescAsteriskNames", "Показывать звёздочку рядом с именами, созданными Namify." },
                { "DescAddName", "Введите пользовательское имя для добавления в ваш список." },
                { "DescAddNameButton", "Добавляет вышеуказанное имя в ваш личный список." },
                { "DescOpenNamifyFile", "Открывает файл имён, созданных API." },
                { "DescOpenUserFile", "Открывает ваш личный файл имён." },
                { "DescGenerateNew", "Получает новые имена из API. Ваши имена сохраняются." },
                { "DescReloadNames", "Перезагружает имена из файла (полезно после ручного редактирования)." },
                { "DescApiKey", "Ключ по умолчанию общий и ограничен 1000 запросами в день. Получите бесплатный ключ на https://randommer.io/" }
            }
        },
        {
            "Portuguese (Brazil)", new()
            {
                { "AddNameButton", "Adicionar nome" },
                { "OpenNamifyList", "Abrir lista Namify" },
                { "OpenUserList", "Abrir minha lista" },
                { "GenerateNewNames", "Gerar novos nomes" },
                { "ReloadNamesFromFile", "Recarregar do arquivo" },
                { "Yes", "Sim" },
                { "No", "Não" },
                { "ConfirmGenerateNew", "Tem certeza que deseja gerar novos nomes?" },
                { "ConfirmReload", "Tem certeza que deseja recarregar os nomes do arquivo?" },
                { "NamesReloaded", "Nomes recarregados do arquivo!" },
                { "ErrorReloading", "Erro ao recarregar. Verifique o log." },
                { "NewNamesGenerated", "Novos nomes gerados!" },
                { "ErrorGenerating", "Erro ao gerar!" },
                { "NameAdded", "'{0}' adicionado aos nomes disponíveis!" },
                { "NameExists", "'{0}' já existe!" },
                { "NoNameEntered", "Você não digitou um nome?" },
                { "FileNotFound", "Arquivo não encontrado: {0}" },
                { "ApiError", "Erro de recuperação! Tentando fonte de backup..." },
                { "ApiBackupError", "Erro de recuperação da fonte de backup!" },
                { "NamesRetrieved", "Nomes recuperados para Namify!" },
                { "NamesRetrievedBackup", "Nomes recuperados da fonte de backup!" },
                { "DescAsteriskNames", "Mostra um asterisco ao lado dos nomes gerados pelo Namify." },
                { "DescAddName", "Digite um nome personalizado para adicionar à sua lista." },
                { "DescAddNameButton", "Adiciona o nome acima à sua lista pessoal." },
                { "DescOpenNamifyFile", "Abre o arquivo de nomes gerados pela API." },
                { "DescOpenUserFile", "Abre seu arquivo de nomes pessoais." },
                { "DescGenerateNew", "Obtém novos nomes da API. Seus nomes são preservados." },
                { "DescReloadNames", "Recarrega nomes do arquivo (útil após edição manual)." },
                { "DescApiKey", "A chave padrão é compartilhada e limitada a 1000 solicitações/dia. Obtenha sua chave gratuita em https://randommer.io/" }
            }
        },
        {
            "Chinese (Simplified)", new()
            {
                { "AddNameButton", "添加名字" },
                { "OpenNamifyList", "打开Namify列表" },
                { "OpenUserList", "打开用户列表" },
                { "GenerateNewNames", "生成新名字" },
                { "ReloadNamesFromFile", "从文件重新加载" },
                { "Yes", "是" },
                { "No", "否" },
                { "ConfirmGenerateNew", "确定要生成新名字吗？" },
                { "ConfirmReload", "确定要从文件重新加载名字吗？" },
                { "NamesReloaded", "已从文件重新加载名字！" },
                { "ErrorReloading", "重新加载错误。请检查日志。" },
                { "NewNamesGenerated", "已生成新名字！" },
                { "ErrorGenerating", "生成错误！" },
                { "NameAdded", "已将'{0}'添加到可用名字！" },
                { "NameExists", "'{0}'已存在！" },
                { "NoNameEntered", "您没有输入名字？" },
                { "FileNotFound", "找不到文件：{0}" },
                { "ApiError", "获取错误！正在尝试备用源..." },
                { "ApiBackupError", "从备用源获取错误！" },
                { "NamesRetrieved", "已为Namify获取名字！" },
                { "NamesRetrievedBackup", "已从备用源获取名字！" },
                { "DescAsteriskNames", "在Namify生成的名字旁边显示星号。" },
                { "DescAddName", "输入要添加到列表的自定义名字。" },
                { "DescAddNameButton", "将上面的名字添加到您的个人列表。" },
                { "DescOpenNamifyFile", "打开API生成的名字文件。" },
                { "DescOpenUserFile", "打开您的个人名字文件。" },
                { "DescGenerateNew", "从API获取新名字。您的名字将被保留。" },
                { "DescReloadNames", "从文件重新加载名字（手动编辑后有用）。" },
                { "DescApiKey", "默认密钥是共享的，每天限制1000个请求。在https://randommer.io/获取免费密钥。" }
            }
        },
        {
            "Chinese (Traditional)", new()
            {
                { "AddNameButton", "新增名字" },
                { "OpenNamifyList", "開啟Namify清單" },
                { "OpenUserList", "開啟使用者清單" },
                { "GenerateNewNames", "產生新名字" },
                { "ReloadNamesFromFile", "從檔案重新載入" },
                { "Yes", "是" },
                { "No", "否" },
                { "ConfirmGenerateNew", "確定要產生新名字嗎？" },
                { "ConfirmReload", "確定要從檔案重新載入名字嗎？" },
                { "NamesReloaded", "已從檔案重新載入名字！" },
                { "ErrorReloading", "重新載入錯誤。請檢查日誌。" },
                { "NewNamesGenerated", "已產生新名字！" },
                { "ErrorGenerating", "產生錯誤！" },
                { "NameAdded", "已將'{0}'新增至可用名字！" },
                { "NameExists", "'{0}'已存在！" },
                { "NoNameEntered", "您沒有輸入名字？" },
                { "FileNotFound", "找不到檔案：{0}" },
                { "ApiError", "取得錯誤！正在嘗試備用來源..." },
                { "ApiBackupError", "從備用來源取得錯誤！" },
                { "NamesRetrieved", "已為Namify取得名字！" },
                { "NamesRetrievedBackup", "已從備用來源取得名字！" },
                { "DescAsteriskNames", "在Namify產生的名字旁邊顯示星號。" },
                { "DescAddName", "輸入要新增至清單的自訂名字。" },
                { "DescAddNameButton", "將上面的名字新增至您的個人清單。" },
                { "DescOpenNamifyFile", "開啟API產生的名字檔案。" },
                { "DescOpenUserFile", "開啟您的個人名字檔案。" },
                { "DescGenerateNew", "從API取得新名字。您的名字將被保留。" },
                { "DescReloadNames", "從檔案重新載入名字（手動編輯後有用）。" },
                { "DescApiKey", "預設金鑰是共用的，每天限制1000個請求。在https://randommer.io/取得免費金鑰。" }
            }
        },
        {
            "Korean", new()
            {
                { "AddNameButton", "이름 추가" },
                { "OpenNamifyList", "Namify 목록 열기" },
                { "OpenUserList", "사용자 목록 열기" },
                { "GenerateNewNames", "새 이름 생성" },
                { "ReloadNamesFromFile", "파일에서 다시 로드" },
                { "Yes", "예" },
                { "No", "아니오" },
                { "ConfirmGenerateNew", "새 이름을 생성하시겠습니까?" },
                { "ConfirmReload", "파일에서 이름을 다시 로드하시겠습니까?" },
                { "NamesReloaded", "파일에서 이름을 다시 로드했습니다!" },
                { "ErrorReloading", "다시 로드 오류. 로그를 확인하세요." },
                { "NewNamesGenerated", "새 이름이 생성되었습니다!" },
                { "ErrorGenerating", "생성 오류!" },
                { "NameAdded", "'{0}'이(가) 사용 가능한 이름에 추가되었습니다!" },
                { "NameExists", "'{0}'이(가) 이미 존재합니다!" },
                { "NoNameEntered", "이름을 입력하지 않았습니까?" },
                { "FileNotFound", "파일을 찾을 수 없음: {0}" },
                { "ApiError", "검색 오류! 백업 소스 시도 중..." },
                { "ApiBackupError", "백업 소스에서 검색 오류!" },
                { "NamesRetrieved", "Namify용 이름을 검색했습니다!" },
                { "NamesRetrievedBackup", "백업 소스에서 이름을 검색했습니다!" },
                { "DescAsteriskNames", "Namify에서 생성한 이름 옆에 별표를 표시합니다." },
                { "DescAddName", "목록에 추가할 사용자 정의 이름을 입력하세요." },
                { "DescAddNameButton", "위의 이름을 개인 목록에 추가합니다." },
                { "DescOpenNamifyFile", "API에서 생성한 이름 파일을 엽니다." },
                { "DescOpenUserFile", "개인 이름 파일을 엽니다." },
                { "DescGenerateNew", "API에서 새 이름을 가져옵니다. 기존 이름은 유지됩니다." },
                { "DescReloadNames", "파일에서 이름을 다시 로드합니다 (수동 편집 후 유용)." },
                { "DescApiKey", "기본 키는 공유되며 하루 1000개 요청으로 제한됩니다. https://randommer.io/에서 무료 키를 받으세요." }
            }
        },
        {
            "Italian", new()
            {
                { "AddNameButton", "Aggiungi nome" },
                { "OpenNamifyList", "Apri lista Namify" },
                { "OpenUserList", "Apri la mia lista" },
                { "GenerateNewNames", "Genera nuovi nomi" },
                { "ReloadNamesFromFile", "Ricarica da file" },
                { "Yes", "Sì" },
                { "No", "No" },
                { "ConfirmGenerateNew", "Sei sicuro di voler generare nuovi nomi?" },
                { "ConfirmReload", "Sei sicuro di voler ricaricare i nomi dal file?" },
                { "NamesReloaded", "Nomi ricaricati dal file!" },
                { "ErrorReloading", "Errore nel ricaricamento. Controlla il log." },
                { "NewNamesGenerated", "Nuovi nomi generati!" },
                { "ErrorGenerating", "Errore nella generazione!" },
                { "NameAdded", "'{0}' aggiunto ai nomi disponibili!" },
                { "NameExists", "'{0}' esiste già!" },
                { "NoNameEntered", "Non hai inserito un nome?" },
                { "FileNotFound", "File non trovato: {0}" },
                { "ApiError", "Errore di recupero! Tentativo con fonte di backup..." },
                { "ApiBackupError", "Errore di recupero dalla fonte di backup!" },
                { "NamesRetrieved", "Nomi recuperati per Namify!" },
                { "NamesRetrievedBackup", "Nomi recuperati dalla fonte di backup!" },
                { "DescAsteriskNames", "Mostra un asterisco accanto ai nomi generati da Namify." },
                { "DescAddName", "Inserisci un nome personalizzato da aggiungere alla tua lista." },
                { "DescAddNameButton", "Aggiunge il nome sopra alla tua lista personale." },
                { "DescOpenNamifyFile", "Apre il file dei nomi generati dall'API." },
                { "DescOpenUserFile", "Apre il tuo file dei nomi personali." },
                { "DescGenerateNew", "Recupera nuovi nomi dall'API. I tuoi nomi vengono preservati." },
                { "DescReloadNames", "Ricarica i nomi dal file (utile dopo modifiche manuali)." },
                { "DescApiKey", "La chiave predefinita è condivisa e limitata a 1000 richieste/giorno. Ottieni la tua chiave gratuita su https://randommer.io/" }
            }
        },
        {
            "Dutch", new()
            {
                { "AddNameButton", "Naam toevoegen" },
                { "OpenNamifyList", "Namify-lijst openen" },
                { "OpenUserList", "Mijn lijst openen" },
                { "GenerateNewNames", "Nieuwe namen genereren" },
                { "ReloadNamesFromFile", "Herladen uit bestand" },
                { "Yes", "Ja" },
                { "No", "Nee" },
                { "ConfirmGenerateNew", "Weet je zeker dat je nieuwe namen wilt genereren?" },
                { "ConfirmReload", "Weet je zeker dat je de namen wilt herladen uit het bestand?" },
                { "NamesReloaded", "Namen herladen uit bestand!" },
                { "ErrorReloading", "Fout bij herladen. Controleer het logboek." },
                { "NewNamesGenerated", "Nieuwe namen gegenereerd!" },
                { "ErrorGenerating", "Fout bij genereren!" },
                { "NameAdded", "'{0}' toegevoegd aan beschikbare namen!" },
                { "NameExists", "'{0}' bestaat al!" },
                { "NoNameEntered", "Je hebt geen naam ingevoerd?" },
                { "FileNotFound", "Bestand niet gevonden: {0}" },
                { "ApiError", "Ophaalfout! Back-upbron proberen..." },
                { "ApiBackupError", "Fout bij ophalen van back-upbron!" },
                { "NamesRetrieved", "Namen opgehaald voor Namify!" },
                { "NamesRetrievedBackup", "Namen opgehaald van back-upbron!" },
                { "DescAsteriskNames", "Toont een asterisk naast door Namify gegenereerde namen." },
                { "DescAddName", "Voer een aangepaste naam in om aan je lijst toe te voegen." },
                { "DescAddNameButton", "Voegt de bovenstaande naam toe aan je persoonlijke lijst." },
                { "DescOpenNamifyFile", "Opent het API-gegenereerde namenbestand." },
                { "DescOpenUserFile", "Opent je persoonlijke namenbestand." },
                { "DescGenerateNew", "Haalt nieuwe namen op van de API. Je namen blijven behouden." },
                { "DescReloadNames", "Herlaadt namen uit bestand (handig na handmatige bewerking)." },
                { "DescApiKey", "De standaardsleutel is gedeeld en beperkt tot 1000 verzoeken/dag. Haal je gratis sleutel op https://randommer.io/" }
            }
        },
        {
            "Turkish", new()
            {
                { "AddNameButton", "İsim Ekle" },
                { "OpenNamifyList", "Namify Listesini Aç" },
                { "OpenUserList", "Kullanıcı Listesini Aç" },
                { "GenerateNewNames", "Yeni İsimler Oluştur" },
                { "ReloadNamesFromFile", "Dosyadan Yeniden Yükle" },
                { "Yes", "Evet" },
                { "No", "Hayır" },
                { "ConfirmGenerateNew", "Yeni isimler oluşturmak istediğinizden emin misiniz?" },
                { "ConfirmReload", "İsimleri dosyadan yeniden yüklemek istediğinizden emin misiniz?" },
                { "NamesReloaded", "İsimler dosyadan yeniden yüklendi!" },
                { "ErrorReloading", "Yeniden yükleme hatası. Günlüğü kontrol edin." },
                { "NewNamesGenerated", "Yeni isimler oluşturuldu!" },
                { "ErrorGenerating", "Oluşturma hatası!" },
                { "NameAdded", "'{0}' mevcut isimlere eklendi!" },
                { "NameExists", "'{0}' zaten mevcut!" },
                { "NoNameEntered", "Bir isim girmediniz mi?" },
                { "FileNotFound", "Dosya bulunamadı: {0}" },
                { "ApiError", "Alma hatası! Yedek kaynak deneniyor..." },
                { "ApiBackupError", "Yedek kaynaktan alma hatası!" },
                { "NamesRetrieved", "Namify için isimler alındı!" },
                { "NamesRetrievedBackup", "Yedek kaynaktan isimler alındı!" },
                { "DescAsteriskNames", "Namify tarafından oluşturulan isimlerin yanında yıldız gösterir." },
                { "DescAddName", "Listenize eklemek için özel bir isim girin." },
                { "DescAddNameButton", "Yukarıdaki ismi kişisel listenize ekler." },
                { "DescOpenNamifyFile", "API tarafından oluşturulan isimler dosyasını açar." },
                { "DescOpenUserFile", "Kişisel isimler dosyanızı açar." },
                { "DescGenerateNew", "API'den yeni isimler alır. İsimleriniz korunur." },
                { "DescReloadNames", "Dosyadan isimleri yeniden yükler (manuel düzenlemeden sonra yararlı)." },
                { "DescApiKey", "Varsayılan anahtar paylaşılır ve günde 1000 istekle sınırlıdır. https://randommer.io/ adresinden ücretsiz anahtarınızı alın." }
            }
        },
        {
            "French (Canada)", new()
            {
                { "AddNameButton", "Ajouter un nom" },
                { "OpenNamifyList", "Ouvrir la liste Namify" },
                { "OpenUserList", "Ouvrir ma liste" },
                { "GenerateNewNames", "Générer de nouveaux noms" },
                { "ReloadNamesFromFile", "Recharger du fichier" },
                { "Yes", "Oui" },
                { "No", "Non" },
                { "ConfirmGenerateNew", "Êtes-vous certain de vouloir générer de nouveaux noms?" },
                { "ConfirmReload", "Êtes-vous certain de vouloir recharger les noms du fichier?" },
                { "NamesReloaded", "Noms rechargés du fichier!" },
                { "ErrorReloading", "Erreur de rechargement. Vérifiez le journal." },
                { "NewNamesGenerated", "Nouveaux noms générés!" },
                { "ErrorGenerating", "Erreur de génération!" },
                { "NameAdded", "'{0}' ajouté aux noms disponibles!" },
                { "NameExists", "'{0}' existe déjà!" },
                { "NoNameEntered", "Vous n'avez pas entré de nom?" },
                { "FileNotFound", "Fichier non trouvé: {0}" },
                { "ApiError", "Erreur de récupération! Essai de la source de secours..." },
                { "ApiBackupError", "Erreur de récupération de la source de secours!" },
                { "NamesRetrieved", "Noms récupérés pour Namify!" },
                { "NamesRetrievedBackup", "Noms récupérés de la source de secours!" },
                { "DescAsteriskNames", "Affiche un astérisque à côté des noms générés par Namify." },
                { "DescAddName", "Entrez un nom personnalisé à ajouter à votre liste." },
                { "DescAddNameButton", "Ajoute le nom ci-dessus à votre liste personnelle." },
                { "DescOpenNamifyFile", "Ouvre le fichier de noms générés par l'API." },
                { "DescOpenUserFile", "Ouvre votre fichier de noms personnels." },
                { "DescGenerateNew", "Récupère de nouveaux noms de l'API. Vos noms sont préservés." },
                { "DescReloadNames", "Recharge les noms du fichier (utile après modification manuelle)." },
                { "DescApiKey", "La clé par défaut est partagée et limitée à 1000 requêtes/jour. Obtenez votre clé gratuite sur https://randommer.io/" }
            }
        },
        {
            "Arabic", new()
            {
                { "AddNameButton", "إضافة اسم" },
                { "OpenNamifyList", "فتح قائمة Namify" },
                { "OpenUserList", "فتح قائمتي" },
                { "GenerateNewNames", "إنشاء أسماء جديدة" },
                { "ReloadNamesFromFile", "إعادة التحميل من الملف" },
                { "Yes", "نعم" },
                { "No", "لا" },
                { "ConfirmGenerateNew", "هل أنت متأكد من أنك تريد إنشاء أسماء جديدة؟" },
                { "ConfirmReload", "هل أنت متأكد من أنك تريد إعادة تحميل الأسماء من الملف؟" },
                { "NamesReloaded", "تم إعادة تحميل الأسماء من الملف!" },
                { "ErrorReloading", "خطأ في إعادة التحميل. تحقق من السجل." },
                { "NewNamesGenerated", "تم إنشاء أسماء جديدة!" },
                { "ErrorGenerating", "خطأ في الإنشاء!" },
                { "NameAdded", "تمت إضافة '{0}' إلى الأسماء المتاحة!" },
                { "NameExists", "'{0}' موجود بالفعل!" },
                { "NoNameEntered", "لم تقم بإدخال اسم؟" },
                { "FileNotFound", "الملف غير موجود: {0}" },
                { "ApiError", "خطأ في الاسترجاع! جارٍ تجربة المصدر الاحتياطي..." },
                { "ApiBackupError", "خطأ في الاسترجاع من المصدر الاحتياطي!" },
                { "NamesRetrieved", "تم استرجاع الأسماء لـ Namify!" },
                { "NamesRetrievedBackup", "تم استرجاع الأسماء من المصدر الاحتياطي!" },
                { "DescAsteriskNames", "إظهار علامة النجمة بجانب الأسماء التي أنشأها Namify." },
                { "DescAddName", "أدخل اسمًا مخصصًا لإضافته إلى قائمتك." },
                { "DescAddNameButton", "يضيف الاسم أعلاه إلى قائمتك الشخصية." },
                { "DescOpenNamifyFile", "يفتح ملف الأسماء الذي أنشأته واجهة برمجة التطبيقات." },
                { "DescOpenUserFile", "يفتح ملف أسمائك الشخصية." },
                { "DescGenerateNew", "يجلب أسماء جديدة من واجهة برمجة التطبيقات. يتم الحفاظ على أسمائك." },
                { "DescReloadNames", "يعيد تحميل الأسماء من الملف (مفيد بعد التحرير اليدوي)." },
                { "DescApiKey", "المفتاح الافتراضي مشترك ومحدود بـ 1000 طلب/يوم. احصل على مفتاحك المجاني من https://randommer.io/" }
            }
        }
    };
}
