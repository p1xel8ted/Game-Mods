/*////////////////////////////////////////////
Name: Resident Evil Requiem Vortex Extension
Structure: 3rd Party Mod Manager (Fluffy)
Author: ChemBoy1
Version: 0.1.0
Date: 2026-03-04
///////////////////////////////////////////*/

//Import libraries
const { actions, fs, util, selectors, log } = require('vortex-api');
const path = require('path');
const template = require('string-template');
const Bluebird = require('bluebird');

//Specify all information about the game
const STEAMAPP_ID = "3764200";
const GAME_ID = "residentevilrequiem";
const GAME_NAME = "Resident Evil Requiem";
const GAME_NAME_SHORT = "RE9";
const EXEC = "re9.exe";
const PCGAMINGWIKI_URL = "https://www.pcgamingwiki.com/wiki/Resident_Evil_Requiem";
const EXTENSION_URL = "https://www.nexusmods.com/site/mods/1653"; //Nexus link to this extension. Used for links

const FLUFFY_FOLDER = "RE9";
const MOD_PATH = path.join("Games", FLUFFY_FOLDER, "Mods");

let GAME_PATH = '';
let DOWNLOAD_FOLDER = '';
let STAGING_FOLDER = '';

//Information for mod types, tools, and installers
const ROOT_ID = `re9-root`;
const ROOT_NAME = "Binaries / Root Folder";
const ROOT_FILES = ['dxgi.dll', 'nvngx_dlss.dll', "amd_fidelityfx_dx12.dll"];
const ROOT_EXTS = [".exe"];

const REF_ID = `re9-reframework`;
const REF_NAME = "REFramework";
const REF_FILE = "dinput8.dll";

const FLUFFY_ID = `re9-fluffymodmanager`;
const FLUFFY_NAME = "Fluffy Mod Manager";
const FLUFFY_EXEC = "modmanager.exe";
const FLUFFY_PAGE_NO = 818;
const FLUFFY_FILE_NO = 4736;

const FLUFFYMOD_ID = `${GAME_ID}-fluffymod`;
const FLUFFYMOD_NAME = "Fluffy Mod";
const FLUFFYPAK_ID = `${GAME_ID}-fluffypakmod`;
const FLUFFYPAK_NAME = "Fluffy Pak Mod";
const FLUFFYMOD_PATH = path.join("Games", FLUFFY_FOLDER, "Mods");
const FLUFFYMOD_FILE = "modinfo.ini";
const PAK_EXT = '.pak';

const LOOSELUA_ID = `${GAME_ID}-looselua`;
const LOOSELUA_NAME = "Loose Lua (REFramework)";
const LOOSELUA_PATH = path.join(".");
const LUA_EXT = '.lua';
const REF_FOLDERS = ['reframework', 'autorun'];

const UPSCALER_ID = `${GAME_ID}-upscaler`;
const UPSCALER_NAME = "Upscaler";
const UPSCALER_PATH = path.join('reframework', 'plugins');
const UPSCALER_FILE = 'PDPerfPlugin.dll';

const REQ_FILE = 're_chunk_000.pak';

const IGNORE_CONFLICTS = [path.join('**', 'screenshot.png'), path.join('**', 'screenshot.jpg'), path.join('**', 'readme.txt'), path.join('**', 'README.txt'), path.join('**', 'ReadMe.txt'), path.join('**', 'Readme.txt')];
const IGNORE_DEPLOY = [path.join('**', 'readme.txt'), path.join('**', 'README.txt'), path.join('**', 'ReadMe.txt'), path.join('**', 'Readme.txt')];

//Filled in from data above
const spec = {
  "game": {
    "id": GAME_ID,
    "name": GAME_NAME,
    "shortName": GAME_NAME_SHORT,
    "logo": `${GAME_ID}.jpg`,
    "mergeMods": true,
    "modPathIsRelative": true,
    "requiredFiles": [
      REQ_FILE,
    ],
    "details": {
      "steamAppId": +STEAMAPP_ID,
      "ignoreDeploy": IGNORE_DEPLOY,
      "ignoreConflicts": IGNORE_CONFLICTS,
    },
    "environment": {
      "SteamAPPId": STEAMAPP_ID,
    }
  },
  "modTypes": [
    {
      "id": ROOT_ID,
      "name": ROOT_NAME,
      "priority": "high",
      "targetPath": "{gamePath}"
    },
    {
      "id": LOOSELUA_ID,
      "name": LOOSELUA_NAME,
      "priority": "high",
      "targetPath": path.join('{gamePath}', LOOSELUA_PATH)
    },
    {
      "id": FLUFFYPAK_ID,
      "name": FLUFFYPAK_NAME,
      "priority": "high",
      "targetPath": path.join('{gamePath}', FLUFFYMOD_PATH)
    },
    {
      "id": FLUFFY_ID,
      "name": FLUFFY_NAME,
      "priority": "low",
      "targetPath": "{gamePath}"
    },
    {
      "id": REF_ID,
      "name": REF_NAME,
      "priority": "low",
      "targetPath": "{gamePath}"
    },
    {
      "id": UPSCALER_ID,
      "name": UPSCALER_NAME,
      "priority": "low",
      "targetPath": path.join('{gamePath}', UPSCALER_PATH)
    },
  ],
  "discovery": {
    "ids": [
      STEAMAPP_ID,
    ],
    "names": []
  }
};

//launchers and 3rd party tools
const tools = [
  {
    id: FLUFFY_ID,
    name: FLUFFY_NAME,
    logo: "fluffy.png",
    executable: () => FLUFFY_EXEC,
    requiredFiles: [FLUFFY_EXEC],
    detach: true,
    relative: true,
    exclusive: true,
  },
  {
    id: `${GAME_ID}-customlaunch`,
    name: `Custom Launch`,
    logo: "exec.png",
    executable: () => EXEC,
    requiredFiles: [EXEC],
    detach: true,
    relative: true,
    exclusive: true,
    shell: true,
    defaultPrimary: false,
    parameters: [],
  },
];

// BASIC FUNCTIONS //////////////////////////////////////////////////////////////////////

//Set mod type priorities
function modTypePriority(priority) {
  return {
    high: 25,
    low: 75,
  }[priority];
}

//Replace string placeholders with actual folder paths
function pathPattern(api, game, pattern) {
  var _a;
  return template(pattern, {
    gamePath: (_a = api.getState().settings.gameMode.discovered[game.id]) === null || _a === void 0 ? void 0 : _a.path,
    documents: util.getVortexPath('documents'),
    localAppData: util.getVortexPath('localAppData'),
    appData: util.getVortexPath('appData'),
  });
}

//Find game installation directory
function makeFindGame(api, gameSpec) {
  return () => util.GameStoreHelper.findByAppId(gameSpec.discovery.ids)
    .then((game) => game.gamePath);
}

//Set mod path
function makeGetModPath(api, gameSpec) {
  return () => gameSpec.game.modPathIsRelative !== false
    ? gameSpec.game.modPath || '.'
    : pathPattern(api, gameSpec.game, gameSpec.game.modPath);
}

//Set launcher requirements
async function requiresLauncher(gamePath, store) {

  if (store === 'steam') {
      return Promise.resolve({
          launcher: 'steam',
      });
  }
  return Promise.resolve(undefined);
}

//Get correct executable
function getExecutable(gamePath) {
  return EXEC;
}

//Get correct mod path
function getModPath(gamePath) {
  return MOD_PATH;
}

// AUTOMATIC INSTALLER FUNCTIONS /////////////////////////////////////////////////////////

//Check if Fluffy Mod Manager is installed
function isFluffyInstalled(api, spec) {
  const MOD_TYPE = FLUFFY_ID;
  const state = api.getState();
  const mods = state.persistent.mods[spec.game.id] || {};
  return Object.keys(mods).some(id => mods[id]?.type === MOD_TYPE);
}

//Function to auto-download Fluffy Mod Manager
async function downloadFluffy(api, gameSpec) {
  let isInstalled = isFluffyInstalled(api, gameSpec);
  if (!isInstalled) {
    //notification indicating install process
    const MOD_NAME = FLUFFY_NAME;
    const MOD_TYPE = FLUFFY_ID;
    const NOTIF_ID = `${GAME_ID}-${MOD_TYPE}-installing`;
    const modPageId = FLUFFY_PAGE_NO;
    const FILE_ID = FLUFFY_FILE_NO;  //If using a specific file id because "input" below gives an error
    const GAME_DOMAIN = 'site';
    api.sendNotification({
      id: NOTIF_ID,
      message: `Installing ${MOD_NAME}`,
      type: 'activity',
      noDismiss: true,
      allowSuppress: false,
    });
    //make sure user is logged into Nexus Mods account in Vortex
    if (api.ext?.ensureLoggedIn !== undefined) {
      await api.ext.ensureLoggedIn();
    }
    try {
      let FILE = null;
      let URL = null;
      try {
        //get the mod files information from Nexus
        const modFiles = await api.ext.nexusGetModFiles(GAME_DOMAIN, modPageId);
        const fileTime = (input) => Number.parseInt(input.uploaded_time, 10);
        const file = modFiles
          .filter(file => file.category_id === 1)
          .sort((lhs, rhs) => fileTime(lhs) - fileTime(rhs))
          .reverse()[0];
        if (file === undefined) {
          throw new util.ProcessCanceled(`No ${MOD_NAME} main file found`);
        }
        FILE = file.file_id;
        URL = `nxm://${GAME_DOMAIN}/mods/${modPageId}/files/${FILE}`;
      } catch (err) {
        FILE = FILE_ID;
        URL = `nxm://${GAME_DOMAIN}/mods/${modPageId}/files/${FILE}`;
      }
      //Download the mod
      const dlInfo = {
        game: GAME_DOMAIN,
        name: MOD_NAME,
      };
      const dlId = await util.toPromise(cb =>
        api.events.emit('start-download', [URL], dlInfo, undefined, cb, undefined, { allowInstall: false }));
      const modId = await util.toPromise(cb =>
        api.events.emit('start-install-download', dlId, { allowAutoEnable: false }, cb));
      const profileId = selectors.lastActiveProfileForGame(api.getState(), gameSpec.game.id);
      const batched = [
        actions.setModsEnabled(api, profileId, [modId], true, {
          allowAutoDeploy: true,
          installed: true,
        }),
        actions.setModType(gameSpec.game.id, modId, MOD_TYPE), // Set the mod type
      ];
      util.batchDispatch(api.store, batched); // Will dispatch both actions.
    //Show the user the download page if the download, install process fails
    } catch (err) {
      const errPage = `https://www.nexusmods.com/${GAME_DOMAIN}/mods/${modPageId}/files/?tab=files`;
      api.showErrorNotification(`Failed to download/install ${MOD_NAME}`, err);
      util.opn(errPage).catch(() => null);
    } finally {
      api.dismissNotification(NOTIF_ID);
    }
  }
}

// MOD INSTALLER FUNCTIONS //////////////////////////////////////////////////////////////

//Installer test for Fluffy Mod Manager files
function testFluffy(files, gameId) {
  const isFluffy = files.some(file => path.basename(file).toLowerCase() === FLUFFY_EXEC);
  let supported = (gameId === spec.game.id) && isFluffy;

  return Promise.resolve({
    supported,
    requiredFiles: [],
  });
}

//Installer install Fluffy Mod Manger files
function installFluffy(files) {
  const modFile = files.find(file => path.basename(file).toLowerCase() === FLUFFY_EXEC);
  const idx = modFile.indexOf(path.basename(modFile));
  const rootPath = path.dirname(modFile);
  const setModTypeInstruction = { type: 'setmodtype', value: FLUFFY_ID };

  // Remove directories and anything that isn't in the rootPath.
  const filtered = files.filter(file => (
    (file.indexOf(rootPath) !== -1)
    && (!file.endsWith(path.sep))
  ));
  const instructions = filtered.map(file => {
    return {
      type: 'copy',
      source: file,
      destination: path.join(file.substr(idx)),
    };
  });
  instructions.push(setModTypeInstruction);
  return Promise.resolve({ instructions });
}

//Installer test for REFramework file
function testREF(files, gameId) {
  const isREF = files.some(file => path.basename(file).toLowerCase() === REF_FILE);
  let supported = (gameId === spec.game.id) && isREF

  return Promise.resolve({
    supported,
    requiredFiles: [],
  });
}

//Installer install REFramework file
function installREF(files) {
  const modFile = files.find(file => path.basename(file).toLowerCase() === REF_FILE);
  const idx = modFile.indexOf(path.basename(modFile));
  const rootPath = path.dirname(modFile);
  const setModTypeInstruction = { type: 'setmodtype', value: REF_ID };

  // Remove directories and anything that isn't in the rootPath.
  const filtered = files.filter(file =>
    ((file.indexOf(rootPath) !== -1) &&
      (!file.endsWith(path.sep))));

  const instructions = filtered.map(file => {
    return {
      type: 'copy',
      source: file,
      destination: path.join(file.substr(idx)),
    };
  });
  instructions.push(setModTypeInstruction);

  return Promise.resolve({ instructions });
}

//Installer test for mod files
function testFluffyMod(files, gameId) {
  const isMod = files.some(file => path.basename(file).toLowerCase() === FLUFFYMOD_FILE);
  let supported = (gameId === spec.game.id) && isMod;

  // Test for a mod installer
  if (supported && files.find(file =>
      (path.basename(file).toLowerCase() === 'moduleconfig.xml') &&
      (path.basename(path.dirname(file)).toLowerCase() === 'fomod'))) {
    supported = false;
  }

  return Promise.resolve({
    supported,
    requiredFiles: [],
  });
}

//Installer install mod files
function installFluffyMod(files, fileName) {
  const modFile = files.find(file => path.basename(file).toLowerCase() === FLUFFYMOD_FILE);
  const idx = modFile.indexOf(path.basename(modFile));
  const rootPath = path.dirname(modFile);
  const setModTypeInstruction = { type: 'setmodtype', value: FLUFFYMOD_ID };
  const MOD_NAME = path.basename(fileName);
  const MOD_FOLDER = MOD_NAME.replace(/(\.installing)*(\.zip)*(\.rar)*(\.7z)*( )*/gi, '');

  // Remove directories and anything that isn't in the rootPath.
  const filtered = files.filter(file =>
    ((file.indexOf(rootPath) !== -1) && (!file.endsWith(path.sep)))
  );

  const instructions = filtered.map(file => {
    return {
      type: 'copy',
      source: file,
      destination: path.join(MOD_FOLDER, file.substr(idx)),
      //destination: path.join(file.substr(idx)),
    };
  });
  instructions.push(setModTypeInstruction);

  return Promise.resolve({ instructions });
}

//Installer test for mod files
function testFluffyPak(files, gameId) {
  const isMod = files.some(file => path.extname(file).toLowerCase() === PAK_EXT);
  let supported = (gameId === spec.game.id) && isMod;

  // Test for a mod installer
  if (supported && files.find(file =>
      (path.basename(file).toLowerCase() === 'moduleconfig.xml') &&
      (path.basename(path.dirname(file)).toLowerCase() === 'fomod'))) {
    supported = false;
  }

  return Promise.resolve({
    supported,
    requiredFiles: [],
  });
}

//Installer install mod files
function installFluffyPak(files, fileName) {
  const modFile = files.find(file => path.extname(file).toLowerCase() === PAK_EXT);
  const idx = modFile.indexOf(path.basename(modFile));
  const rootPath = path.dirname(modFile);
  const setModTypeInstruction = { type: 'setmodtype', value: FLUFFYPAK_ID };
  const MOD_NAME = path.basename(fileName);
  const MOD_FOLDER = MOD_NAME.replace(/(\.installing)*(\.zip)*(\.rar)*(\.7z)*( )*/gi, '');

  // Remove directories and anything that isn't in the rootPath.
  const filtered = files.filter(file =>
    ((file.indexOf(rootPath) !== -1) && (!file.endsWith(path.sep)))
  );

  const instructions = filtered.map(file => {
    return {
      type: 'copy',
      source: file,
      //destination: path.join(MOD_FOLDER, file.substr(idx)),
      destination: path.join(file.substr(idx)),
    };
  });
  instructions.push(setModTypeInstruction);

  return Promise.resolve({ instructions });
}

//Installer test for root folders/files
function testRoot(files, gameId) {
  const isFile = files.some(file => ROOT_FILES.includes(path.basename(file)));
  const isExt = files.some(file => ROOT_EXTS.includes(path.extname(file).toLowerCase()));
  const isFluffy = files.some(file => path.basename(file).toLowerCase() === FLUFFYMOD_FILE);
  let supported = (gameId === spec.game.id) && ( isFile || isExt ) && !isFluffy;

  // Test for a mod installer.
  if (supported && files.find(file =>
    (path.basename(file).toLowerCase() === 'moduleconfig.xml') &&
    (path.basename(path.dirname(file)).toLowerCase() === 'fomod'))) {
    supported = false;
  }

  return Promise.resolve({
    supported,
    requiredFiles: [],
  });
}

//Installer install root folders/files
function installRoot(files) {
  let modFile = files.find(file => ROOT_FILES.includes(path.basename(file)));
  if (modFile === undefined) {
    modFile = files.find(file => ROOT_EXTS.includes(path.extname(file).toLowerCase()));
  }
  const rootPath = path.dirname(modFile);
  const setModTypeInstruction = { type: 'setmodtype', value: ROOT_ID };
  const idx = modFile.indexOf(path.basename(modFile));

  // Remove directories and anything that isn't in the rootPath.
  const filtered = files.filter(file =>
    ((file.indexOf(rootPath) !== -1) && (!file.endsWith(path.sep)))
  );
  const instructions = filtered.map(file => {
    return {
      type: 'copy',
      source: file,
      destination: path.join(file.substr(idx)),
    };
  });
  instructions.push(setModTypeInstruction);
  return Promise.resolve({ instructions });
}

//Installer test for mod files
function testLooseLua(files, gameId) {
  const isLua = files.some(file => path.extname(file).toLowerCase() === LUA_EXT);
  const isRefFolder = files.some(file => REF_FOLDERS.includes(path.basename(file).toLowerCase()));
  let supported = (gameId === spec.game.id) && ( isLua && !isRefFolder );

  // Test for a mod installer
  if (supported && files.find(file =>
      (path.basename(file).toLowerCase() === 'moduleconfig.xml') &&
      (path.basename(path.dirname(file)).toLowerCase() === 'fomod'))) {
    supported = false;
  }

  return Promise.resolve({
    supported,
    requiredFiles: [],
  });
}

//Installer install mod files
function installLooseLua(files) {
  const modFile = files.find(file => path.extname(file).toLowerCase() === LUA_EXT);
  const idx = modFile.indexOf(path.basename(modFile));
  const rootPath = path.dirname(modFile);
  const setModTypeInstruction = { type: 'setmodtype', value: LOOSELUA_ID };

  // Remove directories and anything that isn't in the rootPath.
  const filtered = files.filter(file =>
    ((file.indexOf(rootPath) !== -1) && (!file.endsWith(path.sep)))
  );

  const instructions = filtered.map(file => {
    return {
      type: 'copy',
      source: file,
      destination: path.join('reframework', 'autorun', file.substr(idx)),
    };
  });
  instructions.push(setModTypeInstruction);
  return Promise.resolve({ instructions });
}

//Installer test for Upscaler files
function testUpscaler(files, gameId) {
  const isMod = files.some(file => path.basename(file).toLowerCase() === UPSCALER_FILE.toLowerCase());
  let supported = (gameId === spec.game.id) && isMod;

  return Promise.resolve({
    supported,
    requiredFiles: [],
  });
}

//Installer install Upscaler files
function installUpscaler(files) {
  const modFile = files.find(file => path.basename(file).toLowerCase() === UPSCALER_FILE.toLowerCase());
  const idx = modFile.indexOf(path.basename(modFile));
  const rootPath = path.dirname(modFile);
  const setModTypeInstruction = { type: 'setmodtype', value: UPSCALER_ID };

  // Remove directories and anything that isn't in the rootPath.
  const filtered = files.filter(file => (
    (file.indexOf(rootPath) !== -1)
    && (!file.endsWith(path.sep))
  ));
  const instructions = filtered.map(file => {
    return {
      type: 'copy',
      source: file,
      destination: path.join(file.substr(idx)),
    };
  });
  instructions.push(setModTypeInstruction);
  return Promise.resolve({ instructions });
}

//test for zips
async function testZipContent(files, gameId) {
  const isFluffy = files.some(file => path.basename(file).toLowerCase() === FLUFFY_EXEC);
  const isREF = files.some(file => path.basename(file).toLowerCase() === REF_FILE);
  return Promise.resolve({
    supported: (gameId === spec.game.id) && !isFluffy && !isREF,
    requiredFiles: []
  });
}

//install zips
async function installZipContent(files, destinationPath) {
  const zipFiles = files.filter(file => ['.zip', '.7z', '.rar'].includes(path.extname(file)));
  // If it's a double zip, we don't need to repack.
  if (zipFiles.length > 0) {
    const instructions = zipFiles.map(file => {
      return {
        type: 'copy',
        source: file,
        destination: path.basename(file),
      }
    });
    return Promise.resolve({ instructions });
  }
  // Repack the ZIP
  else {
    const szip = new util.SevenZip();
    const archiveName = path.basename(destinationPath, '.installing') + '.zip';
    const archivePath = path.join(destinationPath, archiveName);
    const rootRelPaths = await fs.readdirAsync(destinationPath);
    await szip.add(archivePath, rootRelPaths.map(relPath => path.join(destinationPath, relPath)), { raw: ['-r'] });
    const instructions = [{
      type: 'copy',
      source: archiveName,
      destination: path.basename(archivePath),
    }];
    return Promise.resolve({ instructions });
  }
}

//convert installer functions to Bluebird promises
function toBlue(func) {
  return (...args) => Bluebird.Promise.resolve(func(...args));
}

// MAIN FUNCTIONS ////////////////////////////////////////////////////////////////////////

//Notify User of Setup instructions
function setupNotify(api) {
  const MOD_NAME = `Fluffy Mod Manager`;
  api.sendNotification({
    id: `${GAME_ID}-setup-notification`,
    type: 'warning',
    message: 'Mod Installation and Setup Instructions',
    allowSuppress: true,
    actions: [
      {
        title: 'More',
        action: (dismiss) => {
          api.showDialog('question', 'Action required', {
            text: `You must use ${MOD_NAME} to enable mods after installing with Vortex.\n`
                + `Use the included tool to launch ${MOD_NAME} (at top of window or in "Dashboard" tab).\n`
                + `If your mod is not for ${MOD_NAME}, you must extract the zip file in the staging folder and change the mod type to "Binaries / Root Folder".\n`
          }, [
            { label: 'Acknowledge', action: () => dismiss() },
            {
              label: 'Never Show Again', action: () => {
                api.suppressNotification(NOTIF_ID);
                dismiss();
              }
            },
          ]);
        },
      },
    ],
  });
}

//Notify User to run Fluffy Mod Manager after deployment
function deployNotify(api) {
  const NOTIF_ID = `${GAME_ID}-deploy-notification`;
  const MOD_NAME = FLUFFY_NAME;
  const MESSAGE = `Run Fluffy Mod Manager after Deploy`;
  api.sendNotification({
    id: NOTIF_ID,
    type: 'warning',
    message: MESSAGE,
    allowSuppress: true,
    actions: [
      {
        title: 'Run Fluffy',
        action: (dismiss) => {
          runFluffy(api);
          dismiss();
        },
      },
      {
        title: 'More',
        action: (dismiss) => {
          api.showDialog('question', 'Run Fluffy Mod Manager to Enable Mods', {
            text: `You must use ${MOD_NAME} to enable mods after installing with Vortex.\n`
                + `Use the included tool to launch ${MOD_NAME} (button on notification or in "Dashboard" tab).\n`
                + `If your mod is not for ${MOD_NAME}, you may need to change the mod type to "Binaries / Root Folder" manually.\n`
          }, [
            {
              label: 'Run Fluffy', action: () => {
                runFluffy(api);
                dismiss();
              }
            },
            { label: 'Continue', action: () => dismiss() },
            {
              label: 'Never Show Again', action: () => {
                api.suppressNotification(NOTIF_ID);
                dismiss();
              }
            },
          ]);
        },
      },
    ],
  });
}

function runFluffy(api) {
  const TOOL_ID = FLUFFY_ID;
  const TOOL_NAME = FLUFFY_NAME;
  const state = api.store.getState();
  const tool = util.getSafe(state, ['settings', 'gameMode', 'discovered', GAME_ID, 'tools', TOOL_ID], undefined);

  try {
    const TOOL_PATH = tool.path;
    if (TOOL_PATH !== undefined) {
      return api.runExecutable(TOOL_PATH, [], { suggestDeploy: false })
        .catch(err => api.showErrorNotification(`Failed to run ${TOOL_NAME}`, err,
          { allowReport: ['EPERM', 'EACCESS', 'ENOENT'].indexOf(err.code) !== -1 })
        );
    }
    else {
      return api.showErrorNotification(`Failed to run ${TOOL_NAME}`, `Path to ${TOOL_NAME} executable could not be found. Ensure ${TOOL_NAME} is installed through Vortex.`);
    }
  } catch (err) {
    return api.showErrorNotification(`Failed to run ${TOOL_NAME}`, err, { allowReport: ['EPERM', 'EACCESS', 'ENOENT'].indexOf(err.code) !== -1 });
  }
}

//Setup function
async function setup(discovery, api, gameSpec) {
  //setupNotify(api);
  GAME_PATH = discovery.path;
  DOWNLOAD_FOLDER = selectors.downloadPathForGame(api.getState(), GAME_ID);
  STAGING_FOLDER = selectors.installPathForGame(api.getState(), GAME_ID);
  await downloadFluffy(api, gameSpec);
  await fs.ensureDirWritableAsync(path.join(GAME_PATH, UPSCALER_PATH));
  return fs.ensureDirWritableAsync(path.join(GAME_PATH, MOD_PATH));
}

//Let Vortex know about the game
function applyGame(context, gameSpec) {
  //register the game
  const game = {
    ...gameSpec.game,
    queryPath: makeFindGame(context.api, gameSpec),
    executable: getExecutable,
    queryModPath: getModPath,
    requiresLauncher: requiresLauncher,
    requiresCleanup: true,
    setup: async (discovery) => await setup(discovery, context.api, gameSpec),
    supportedTools: tools,
  };
  context.registerGame(game);

  //register mod types
  (gameSpec.modTypes || []).forEach((type, idx) => {
    context.registerModType(type.id, modTypePriority(type.priority) + idx, (gameId) => {
      var _a;
      return (gameId === gameSpec.game.id)
        && !!((_a = context.api.getState().settings.gameMode.discovered[gameId]) === null || _a === void 0 ? void 0 : _a.path);
    }, (game) => pathPattern(context.api, game, type.targetPath), () => Promise.resolve(false), { name: type.name });
  });

  //register mod types explicitly
  context.registerModType(FLUFFYMOD_ID, 35,
    (gameId) => {
      var _a;
      return (gameId === GAME_ID) && !!((_a = context.api.getState().settings.gameMode.discovered[gameId]) === null || _a === void 0 ? void 0 : _a.path);
    },
    (game) => {
      return pathPattern(context.api, game, path.join('{gamePath}', MOD_PATH))
    },
    () => Promise.resolve(false),
    { name: FLUFFYMOD_NAME }
  );

  //register mod installers
  context.registerInstaller(FLUFFY_ID, 25, testFluffy, installFluffy);
  context.registerInstaller(REF_ID, 27, testREF, installREF);
  //context.registerInstaller(FLUFFYMOD_ID, 35, testFluffyMod, installFluffyMod);
  //context.registerInstaller(`${FLUFFYMOD_ID}pak`, 40, testFluffyPak, installFluffyPak);
  context.registerInstaller(LOOSELUA_ID, 29, testLooseLua, installLooseLua);
  context.registerInstaller(ROOT_ID, 31, testRoot, installRoot);
  context.registerInstaller(UPSCALER_ID, 33, testUpscaler, installUpscaler);
  context.registerInstaller(`${FLUFFYMOD_ID}zip`, 45, toBlue(testZipContent), toBlue(installZipContent));

  context.registerAction('mod-icons', 300, 'open-ext', {}, 'Open PCGamingWiki Page', () => {
    util.opn(PCGAMINGWIKI_URL).catch(() => null);
  }, () => {
    const state = context.api.getState();
    const gameId = selectors.activeGameId(state);
    return gameId === GAME_ID;
  });
  context.registerAction('mod-icons', 300, 'open-ext', {}, 'View Changelog', () => {
    util.opn(path.join(__dirname, 'CHANGELOG.md')).catch(() => null);
    }, () => {
      const state = context.api.getState();
      const gameId = selectors.activeGameId(state);
      return gameId === GAME_ID;
  });
  context.registerAction('mod-icons', 300, 'open-ext', {}, 'Submit Bug Report', () => {
    util.opn(`${EXTENSION_URL}?tab=bugs`).catch(() => null);
  }, () => {
    const state = context.api.getState();
    const gameId = selectors.activeGameId(state);
    return gameId === GAME_ID;
  });
  context.registerAction('mod-icons', 300, 'open-ext', {}, 'Open Downloads Folder', () => {
    util.opn(DOWNLOAD_FOLDER).catch(() => null);
  }, () => {
    const state = context.api.getState();
    const gameId = selectors.activeGameId(state);
    return gameId === GAME_ID;
  });
}

//Main function
function main(context) {
  applyGame(context, spec);
  context.once(() => {
    // put code here that should be run (once) when Vortex starts up
    context.api.onAsync('did-deploy', async (profileId, deployment) => {
      const LAST_ACTIVE_PROFILE = selectors.lastActiveProfileForGame(context.api.getState(), GAME_ID);
      if (profileId !== LAST_ACTIVE_PROFILE) return;
      return deployNotify(context.api);
    });

  });
  return true;
}

//export to Vortex
module.exports = {
  default: main,
};
