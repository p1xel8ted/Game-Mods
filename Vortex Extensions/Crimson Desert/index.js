/*////////////////////////////////////////////////
Name: Crimson Desert Vortex Extension
Structure: CD JSON Mod Manager
Author: p1xel8ted
Version: 0.1.0
Date: 2026-03-30
/////////////////////////////////////////////////*/

//Import libraries
const { actions, fs, util, selectors, log } = require('vortex-api');
const path = require('path');
const https = require('https');

//Game information
const STEAMAPP_ID = '3321460';
const GAME_ID = 'crimsondesert';
const GAME_NAME = 'Crimson Desert';
const GAME_NAME_SHORT = 'CD';
const EXEC_FOLDER = 'bin64';
const EXEC = path.join(EXEC_FOLDER, 'CrimsonDesert.exe');

//Mod manager paths (relative to game root)
const JSONMM_FOLDER = 'CD JSON Mod Manager';
const MOD_PATH = path.join(JSONMM_FOLDER, 'mods');
const ASI_MOD_PATH = path.join(MOD_PATH, '_asi');

//Mod types
const ROOT_ID = `${GAME_ID}-root`;
const ROOT_NAME = 'Binaries / Root Folder';

const JSONMM_ID = `${GAME_ID}-jsonmodmanager`;
const JSONMM_NAME = 'CD JSON Mod Manager';
const JSONMM_EXEC = 'CD JSON Mod Manager.exe';
const JSONMM_PAGE_NO = 113;
const JSONMM_FILE_NO = 0; //Fallback file ID if auto-detect fails
const JSONMM_TOOL_ID = 'CDJsonModManager';

const JSONMOD_ID = `${GAME_ID}-jsonmod`;
const JSONMOD_NAME = 'JSON Mod';

const ASI_ID = `${GAME_ID}-asimod`;
const ASI_NAME = 'ASI Mod';
const ASI_EXT = '.asi';

const ASILOADER_ID = `${GAME_ID}-asiloader`;
const ASILOADER_NAME = 'Ultimate ASI Loader';
const ASILOADER_URL = 'https://github.com/ThirteenAG/Ultimate-ASI-Loader/releases/download/v9.7.0/Ultimate-ASI-Loader_x64.zip';
const ASILOADER_DLLS = [
  'winmm.dll', 'version.dll', 'dsound.dll', 'dinput8.dll',
  'd3d9.dll', 'd3d11.dll', 'winhttp.dll', 'dxgi.dll',
];

const PRECOMP_ID = `${GAME_ID}-precompmod`;
const PRECOMP_NAME = 'Precompiled Mod';
const PRECOMP_EXTS = ['.pamt', '.paz', '.papgt'];

const RESHADE_ID = `${GAME_ID}-reshadepreset`;
const RESHADE_NAME = 'ReShade Preset';
const RESHADE_EXT = '.ini';

const IGNORE_CONFLICTS = [
  path.join('**', 'screenshot.png'),
  path.join('**', 'screenshot.jpg'),
  path.join('**', 'readme.txt'),
  path.join('**', 'README.txt'),
  path.join('**', 'ReadMe.txt'),
  path.join('**', 'Readme.txt'),
];
const IGNORE_DEPLOY = [
  path.join('**', 'readme.txt'),
  path.join('**', 'README.txt'),
  path.join('**', 'ReadMe.txt'),
  path.join('**', 'Readme.txt'),
];

let GAME_PATH = '';

//Spec object
const spec = {
  game: {
    id: GAME_ID,
    name: GAME_NAME,
    shortname: GAME_NAME_SHORT,
    executable: EXEC,
    logo: 'icon.jpg',
    mergeMods: true,
    modPath: MOD_PATH,
    modPathIsRelative: true,
    requiredFiles: [EXEC],
    details: {
      steamAppId: +STEAMAPP_ID,
      nexusPageId: GAME_ID,
      ignoreDeploy: IGNORE_DEPLOY,
      ignoreConflicts: IGNORE_CONFLICTS,
    },
    environment: {
      SteamAPPId: STEAMAPP_ID,
    },
  },
  modTypes: [
    {
      id: ROOT_ID,
      name: ROOT_NAME,
      priority: 'high',
      targetPath: '{gamePath}',
    },
    {
      id: JSONMM_ID,
      name: JSONMM_NAME,
      priority: 'low',
      targetPath: path.join('{gamePath}', JSONMM_FOLDER),
    },
    {
      id: JSONMOD_ID,
      name: JSONMOD_NAME,
      priority: 'high',
      targetPath: path.join('{gamePath}', MOD_PATH),
    },
    {
      id: PRECOMP_ID,
      name: PRECOMP_NAME,
      priority: 'high',
      targetPath: path.join('{gamePath}', MOD_PATH),
    },
    {
      id: ASILOADER_ID,
      name: ASILOADER_NAME,
      priority: 'low',
      targetPath: path.join('{gamePath}', EXEC_FOLDER),
    },
    {
      id: ASI_ID,
      name: ASI_NAME,
      priority: 'high',
      targetPath: path.join('{gamePath}', ASI_MOD_PATH),
    },
    {
      id: RESHADE_ID,
      name: RESHADE_NAME,
      priority: 'high',
      targetPath: path.join('{gamePath}', EXEC_FOLDER),
    },
  ],
  discovery: {
    ids: [STEAMAPP_ID],
    names: [],
  },
};

//Tools
const tools = [
  {
    id: JSONMM_TOOL_ID,
    name: JSONMM_NAME,
    logo: 'icon.jpg',
    executable: () => path.join(JSONMM_FOLDER, JSONMM_EXEC),
    requiredFiles: [path.join(JSONMM_FOLDER, JSONMM_EXEC)],
    detach: true,
    relative: true,
    exclusive: true,
  },
];

// BASIC FUNCTIONS //////////////////////////////////////////////////////////////////////

function modTypePriority(priority) {
  return {
    high: 25,
    low: 75,
  }[priority];
}

function pathPattern(api, game, pattern) {
  var _a;
  const discovered = (_a = api.getState().settings.gameMode.discovered[game.id]) === null || _a === void 0 ? void 0 : _a.path;
  return pattern.replace(/\{gamePath\}/g, discovered || '');
}

function makeFindGame(api, gameSpec) {
  return () => util.GameStoreHelper.findByAppId(gameSpec.discovery.ids)
    .then((game) => game.gamePath);
}

function makeGetModPath(api, gameSpec) {
  return () => gameSpec.game.modPathIsRelative !== false
    ? gameSpec.game.modPath || '.'
    : pathPattern(api, gameSpec.game, gameSpec.game.modPath);
}

async function requiresLauncher(gamePath, store) {
  if (store === 'steam') {
    return Promise.resolve({
      launcher: 'steam',
    });
  }
  return Promise.resolve(undefined);
}

// AUTO-DOWNLOAD FUNCTIONS //////////////////////////////////////////////////////////////

function isJsonMMInstalled(api, gameSpec) {
  const state = api.getState();
  const mods = state.persistent.mods[gameSpec.game.id] || {};
  return Object.keys(mods).some(id => mods[id]?.type === JSONMM_ID);
}

async function downloadJsonMM(api, gameSpec) {
  if (isJsonMMInstalled(api, gameSpec)) {
    return;
  }

  const MOD_NAME = JSONMM_NAME;
  const MOD_TYPE = JSONMM_ID;
  const NOTIF_ID = `${GAME_ID}-${MOD_TYPE}-installing`;
  const modPageId = JSONMM_PAGE_NO;
  const GAME_DOMAIN = GAME_ID;

  api.sendNotification({
    id: NOTIF_ID,
    message: `Installing ${MOD_NAME}`,
    type: 'activity',
    noDismiss: true,
    allowSuppress: false,
  });

  if (api.ext?.ensureLoggedIn !== undefined) {
    await api.ext.ensureLoggedIn();
  }

  try {
    let FILE = null;
    let URL = null;
    try {
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
      if (JSONMM_FILE_NO > 0) {
        FILE = JSONMM_FILE_NO;
        URL = `nxm://${GAME_DOMAIN}/mods/${modPageId}/files/${FILE}`;
      } else {
        throw err;
      }
    }

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
      actions.setModType(gameSpec.game.id, modId, MOD_TYPE),
    ];
    util.batchDispatch(api.store, batched);
  } catch (err) {
    const errPage = `https://www.nexusmods.com/${GAME_DOMAIN}/mods/${modPageId}/files/?tab=files`;
    api.showErrorNotification(`Failed to download/install ${MOD_NAME}`, err);
    util.opn(errPage).catch(() => null);
  } finally {
    api.dismissNotification(NOTIF_ID);
  }
}

//Fetch latest release info from GitHub
function fetchGitHubRelease(owner, repo) {
  return new Promise((resolve, reject) => {
    const options = {
      hostname: 'api.github.com',
      path: `/repos/${owner}/${repo}/releases/latest`,
      headers: { 'User-Agent': 'Vortex-CrimsonDesert-Extension' },
    };
    https.get(options, (res) => {
      let data = '';
      res.on('data', (chunk) => { data += chunk; });
      res.on('end', () => {
        try {
          resolve(JSON.parse(data));
        } catch (err) {
          reject(err);
        }
      });
    }).on('error', reject);
  });
}

function isAsiLoaderInstalled(api, gameSpec) {
  const state = api.getState();
  const mods = state.persistent.mods[gameSpec.game.id] || {};
  return Object.keys(mods).some(id => mods[id]?.type === ASILOADER_ID);
}

async function downloadAsiLoader(api, gameSpec) {
  if (isAsiLoaderInstalled(api, gameSpec)) {
    return;
  }

  const MOD_NAME = ASILOADER_NAME;
  const MOD_TYPE = ASILOADER_ID;
  const NOTIF_ID = `${GAME_ID}-${MOD_TYPE}-installing`;

  api.sendNotification({
    id: NOTIF_ID,
    message: `Installing ${MOD_NAME}`,
    type: 'activity',
    noDismiss: true,
    allowSuppress: false,
  });

  try {
    //Fetch latest release info from GitHub
    let downloadUrl = ASILOADER_URL;
    let version = '';
    try {
      const release = await fetchGitHubRelease('ThirteenAG', 'Ultimate-ASI-Loader');
      version = (release.tag_name || '').replace(/^v/i, '');
      const x64Asset = release.assets?.find(a => a.name === 'Ultimate-ASI-Loader_x64.zip');
      if (x64Asset) {
        downloadUrl = x64Asset.browser_download_url;
      }
    } catch (err) {
      //Fallback to hardcoded URL
    }

    const dlInfo = {
      game: GAME_ID,
      name: MOD_NAME,
    };
    const dlId = await util.toPromise(cb =>
      api.events.emit('start-download', [downloadUrl], dlInfo, undefined, cb, undefined, { allowInstall: false }));
    const modId = await util.toPromise(cb =>
      api.events.emit('start-install-download', dlId, { allowAutoEnable: false }, cb));
    const profileId = selectors.lastActiveProfileForGame(api.getState(), gameSpec.game.id);
    const batched = [
      actions.setModsEnabled(api, profileId, [modId], true, {
        allowAutoDeploy: true,
        installed: true,
      }),
      actions.setModType(gameSpec.game.id, modId, MOD_TYPE),
      actions.setModAttribute(gameSpec.game.id, modId, 'customFileName', MOD_NAME),
      actions.setModAttribute(gameSpec.game.id, modId, 'logicalFileName', MOD_NAME),
      actions.setModAttribute(gameSpec.game.id, modId, 'version', version),
      actions.setModAttribute(gameSpec.game.id, modId, 'author', 'ThirteenAG'),
      actions.setModAttribute(gameSpec.game.id, modId, 'shortDescription', 'Proxy DLL for loading ASI plugins'),
      actions.setModAttribute(gameSpec.game.id, modId, 'description', 'Proxy DLL for loading ASI plugins. Required for .asi mods to function.'),
      actions.setModAttribute(gameSpec.game.id, modId, 'category', 15),
      actions.setModAttribute(gameSpec.game.id, modId, 'source', 'website'),
      actions.setModAttribute(gameSpec.game.id, modId, 'url', 'https://github.com/ThirteenAG/Ultimate-ASI-Loader'),
    ];
    util.batchDispatch(api.store, batched);
  } catch (err) {
    api.showErrorNotification(`Failed to download/install ${MOD_NAME}`, err);
    util.opn('https://github.com/ThirteenAG/Ultimate-ASI-Loader/releases').catch(() => null);
  } finally {
    api.dismissNotification(NOTIF_ID);
  }
}

// MOD INSTALLER FUNCTIONS //////////////////////////////////////////////////////////////

//Installer test for JSON Mod Manager files
function testJsonMM(files, gameId) {
  const isJsonMM = files.some(file => path.basename(file).toLowerCase() === JSONMM_EXEC.toLowerCase());
  const supported = (gameId === spec.game.id) && isJsonMM;
  return Promise.resolve({
    supported,
    requiredFiles: [],
  });
}

//Installer install JSON Mod Manager files
function installJsonMM(files) {
  const modFile = files.find(file => path.basename(file).toLowerCase() === JSONMM_EXEC.toLowerCase());
  const idx = modFile.indexOf(path.basename(modFile));
  const rootPath = path.dirname(modFile);
  const setModTypeInstruction = { type: 'setmodtype', value: JSONMM_ID };

  const filtered = files.filter(file =>
    ((file.indexOf(rootPath) !== -1) && (!file.endsWith(path.sep))));

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

//Check if archive contents are already inside a single subfolder
function hasExistingSubfolder(files) {
  const contentFiles = files.filter(file => !file.endsWith(path.sep));
  if (contentFiles.length === 0) return false;
  const topDirs = new Set(contentFiles.map(file => {
    const parts = file.split(path.sep);
    return parts.length > 1 ? parts[0] : null;
  }));
  //All files share a single top-level folder
  return topDirs.size === 1 && !topDirs.has(null);
}

//Derive a clean mod folder name from the archive filename
function modFolderFromFileName(fileName) {
  return path.basename(fileName).replace(/(\.installing)*(\.zip)*(\.rar)*(\.7z)*( )*/gi, '');
}

//Installer test for JSON mod files
function testJsonMod(files, gameId) {
  const isJsonMM = files.some(file => path.basename(file).toLowerCase() === JSONMM_EXEC.toLowerCase());
  const hasJson = files.some(file => path.extname(file).toLowerCase() === '.json');
  const supported = (gameId === spec.game.id) && hasJson && !isJsonMM;
  return Promise.resolve({
    supported,
    requiredFiles: [],
  });
}

//Installer install JSON mod files
function installJsonMod(files, fileName) {
  const modFile = files.find(file => path.extname(file).toLowerCase() === '.json');
  const idx = modFile.indexOf(path.basename(modFile));
  const rootPath = path.dirname(modFile);
  const setModTypeInstruction = { type: 'setmodtype', value: JSONMOD_ID };
  const needsWrap = !hasExistingSubfolder(files);
  const modFolder = needsWrap ? modFolderFromFileName(fileName) : '';

  const filtered = files.filter(file =>
    ((file.indexOf(rootPath) !== -1) && (!file.endsWith(path.sep))));

  const instructions = filtered.map(file => {
    return {
      type: 'copy',
      source: file,
      destination: needsWrap ? path.join(modFolder, file.substr(idx)) : file,
    };
  });
  instructions.push(setModTypeInstruction);
  return Promise.resolve({ instructions });
}

//Installer test for precompiled mod files (0036/meta folder structure)
function testPrecompMod(files, gameId) {
  const isJsonMM = files.some(file => path.basename(file).toLowerCase() === JSONMM_EXEC.toLowerCase());
  const hasJson = files.some(file => path.extname(file).toLowerCase() === '.json');
  const hasPrecomp = files.some(file => PRECOMP_EXTS.includes(path.extname(file).toLowerCase()));
  const supported = (gameId === spec.game.id) && hasPrecomp && !hasJson && !isJsonMM;
  return Promise.resolve({
    supported,
    requiredFiles: [],
  });
}

//Installer install precompiled mod files
function installPrecompMod(files, fileName) {
  const modFile = files.find(file => PRECOMP_EXTS.includes(path.extname(file).toLowerCase()));
  const idx = modFile.indexOf(path.basename(modFile));
  const rootPath = path.dirname(modFile);
  const setModTypeInstruction = { type: 'setmodtype', value: PRECOMP_ID };
  const needsWrap = !hasExistingSubfolder(files);
  const modFolder = needsWrap ? modFolderFromFileName(fileName) : '';

  const filtered = files.filter(file =>
    ((file.indexOf(rootPath) !== -1) && (!file.endsWith(path.sep))));

  const instructions = filtered.map(file => {
    return {
      type: 'copy',
      source: file,
      destination: needsWrap ? path.join(modFolder, file.substr(idx)) : file,
    };
  });
  instructions.push(setModTypeInstruction);
  return Promise.resolve({ instructions });
}

//Check if a file is a known ASI Loader proxy DLL
function isLoaderDll(file) {
  return ASILOADER_DLLS.includes(path.basename(file).toLowerCase());
}

//Installer test for standalone ASI Loader (proxy DLL without .asi files)
function testAsiLoader(files, gameId) {
  const hasLoader = files.some(file => isLoaderDll(file));
  const hasAsi = files.some(file => path.extname(file).toLowerCase() === ASI_EXT);
  const supported = (gameId === spec.game.id) && hasLoader && !hasAsi;
  return Promise.resolve({
    supported,
    requiredFiles: [],
  });
}

//Installer install ASI Loader files (renames proxy DLL to version.dll)
function installAsiLoader(files) {
  const modFile = files.find(file => isLoaderDll(file));
  const idx = modFile.indexOf(path.basename(modFile));
  const rootPath = path.dirname(modFile);
  const setModTypeInstruction = { type: 'setmodtype', value: ASILOADER_ID };

  const filtered = files.filter(file =>
    ((file.indexOf(rootPath) !== -1) && (!file.endsWith(path.sep))));

  const instructions = filtered.map(file => {
    const dest = isLoaderDll(file) ? 'version.dll' : path.join(file.substr(idx));
    return {
      type: 'copy',
      source: file,
      destination: dest,
    };
  });
  instructions.push(setModTypeInstruction);
  return Promise.resolve({ instructions });
}

//Installer test for ASI mod files (with or without bundled loader DLL)
function testAsiMod(files, gameId) {
  const hasAsi = files.some(file => path.extname(file).toLowerCase() === ASI_EXT);
  const supported = (gameId === spec.game.id) && hasAsi;
  return Promise.resolve({
    supported,
    requiredFiles: [],
  });
}

//Installer install ASI mod files (strips bundled loader DLLs, wraps in subfolder)
function installAsiMod(files, fileName) {
  const modFile = files.find(file => path.extname(file).toLowerCase() === ASI_EXT);
  const idx = modFile.indexOf(path.basename(modFile));
  const rootPath = path.dirname(modFile);
  const setModTypeInstruction = { type: 'setmodtype', value: ASI_ID };
  const needsWrap = !hasExistingSubfolder(files);
  const modFolder = needsWrap ? modFolderFromFileName(fileName) : '';

  const filtered = files.filter(file =>
    ((file.indexOf(rootPath) !== -1) && (!file.endsWith(path.sep)) && !isLoaderDll(file)));

  const instructions = filtered.map(file => {
    return {
      type: 'copy',
      source: file,
      destination: needsWrap ? path.join(modFolder, file.substr(idx)) : file,
    };
  });
  instructions.push(setModTypeInstruction);
  return Promise.resolve({ instructions });
}

//Installer test for ReShade preset files
function testReshadeMod(files, gameId) {
  const isJsonMM = files.some(file => path.basename(file).toLowerCase() === JSONMM_EXEC.toLowerCase());
  const hasIni = files.some(file => path.extname(file).toLowerCase() === RESHADE_EXT);
  const hasAsi = files.some(file => path.extname(file).toLowerCase() === ASI_EXT);
  const hasJson = files.some(file => path.extname(file).toLowerCase() === '.json');
  const hasPrecomp = files.some(file => PRECOMP_EXTS.includes(path.extname(file).toLowerCase()));
  const supported = (gameId === spec.game.id) && hasIni && !hasAsi && !hasJson && !hasPrecomp && !isJsonMM;
  return Promise.resolve({
    supported,
    requiredFiles: [],
  });
}

//Installer install ReShade preset files
function installReshadeMod(files) {
  const modFile = files.find(file => path.extname(file).toLowerCase() === RESHADE_EXT);
  const idx = modFile.indexOf(path.basename(modFile));
  const rootPath = path.dirname(modFile);
  const setModTypeInstruction = { type: 'setmodtype', value: RESHADE_ID };

  const filtered = files.filter(file =>
    ((file.indexOf(rootPath) !== -1) && (!file.endsWith(path.sep))));

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

// MAIN FUNCTIONS ///////////////////////////////////////////////////////////////////////

//Check if any ASI mods are installed but no ASI Loader is present
function checkAsiLoaderWarning(api) {
  const state = api.getState();
  const mods = state.persistent.mods[GAME_ID] || {};
  const hasAsiMods = Object.keys(mods).some(id => mods[id]?.type === ASI_ID);
  const hasLoader = Object.keys(mods).some(id => mods[id]?.type === ASILOADER_ID);
  if (hasAsiMods && !hasLoader) {
    const NOTIF_ID = `${GAME_ID}-asiloader-warning`;
    api.sendNotification({
      id: NOTIF_ID,
      type: 'warning',
      message: 'ASI mods require Ultimate ASI Loader to function. Install an ASI Loader (proxy DLL) to bin64.',
      allowSuppress: true,
    });
  }
}

//Notify user to run JSON Mod Manager after deployment
function deployNotify(api) {
  const NOTIF_ID = `${GAME_ID}-deploy-notification`;
  const MOD_NAME = JSONMM_NAME;
  api.sendNotification({
    id: NOTIF_ID,
    type: 'warning',
    message: `Run ${MOD_NAME} after Deploy`,
    allowSuppress: true,
    actions: [
      {
        title: 'Run',
        action: (dismiss) => {
          runJsonMM(api);
          dismiss();
        },
      },
      {
        title: 'More',
        action: (dismiss) => {
          api.showDialog('question', `Run ${MOD_NAME} to Enable Mods`, {
            text: `You must use ${MOD_NAME} to apply mods after installing with Vortex.\n`
              + `Use the included tool to launch ${MOD_NAME} (button on notification or in "Dashboard" tab).\n`,
          }, [
            {
              label: `Run ${MOD_NAME}`, action: () => {
                runJsonMM(api);
                dismiss();
              },
            },
            { label: 'Continue', action: () => dismiss() },
            {
              label: 'Never Show Again', action: () => {
                api.suppressNotification(NOTIF_ID);
                dismiss();
              },
            },
          ]);
        },
      },
    ],
  });
}

function runJsonMM(api) {
  const state = api.store.getState();
  const tool = util.getSafe(state, ['settings', 'gameMode', 'discovered', GAME_ID, 'tools', JSONMM_TOOL_ID], undefined);

  try {
    const TOOL_PATH = tool.path;
    if (TOOL_PATH !== undefined) {
      return api.runExecutable(TOOL_PATH, [], { suggestDeploy: false })
        .catch(err => api.showErrorNotification(`Failed to run ${JSONMM_NAME}`, err,
          { allowReport: ['EPERM', 'EACCESS', 'ENOENT'].indexOf(err.code) !== -1 }));
    } else {
      return api.showErrorNotification(`Failed to run ${JSONMM_NAME}`,
        `Path to ${JSONMM_NAME} executable could not be found. Ensure ${JSONMM_NAME} is installed through Vortex.`);
    }
  } catch (err) {
    return api.showErrorNotification(`Failed to run ${JSONMM_NAME}`, err,
      { allowReport: ['EPERM', 'EACCESS', 'ENOENT'].indexOf(err.code) !== -1 });
  }
}

//Setup function
async function setup(discovery, api, gameSpec) {
  GAME_PATH = discovery.path;
  await downloadJsonMM(api, gameSpec);
  await downloadAsiLoader(api, gameSpec);
  return fs.ensureDirWritableAsync(path.join(GAME_PATH, MOD_PATH));
}

//Register the game with Vortex
function applyGame(context, gameSpec) {
  const game = {
    ...gameSpec.game,
    queryPath: makeFindGame(context.api, gameSpec),
    queryModPath: makeGetModPath(context.api, gameSpec),
    requiresLauncher: requiresLauncher,
    requiresCleanup: true,
    setup: async (discovery) => await setup(discovery, context.api, gameSpec),
    executable: () => gameSpec.game.executable,
    supportedTools: tools,
  };
  context.registerGame(game);

  //Register mod types
  (gameSpec.modTypes || []).forEach((type, idx) => {
    context.registerModType(type.id, modTypePriority(type.priority) + idx, (gameId) => {
      var _a;
      return (gameId === gameSpec.game.id)
        && !!((_a = context.api.getState().settings.gameMode.discovered[gameId]) === null || _a === void 0 ? void 0 : _a.path);
    }, (game) => pathPattern(context.api, game, type.targetPath), () => Promise.resolve(false), { name: type.name });
  });

  //Register actions
  context.registerAction('mod-icons', 300, 'open-ext', {}, 'Open Game Exe Folder', () => {
    const state = context.api.getState();
    const gamePath = util.getSafe(state, ['settings', 'gameMode', 'discovered', GAME_ID, 'path'], undefined);
    if (gamePath) {
      util.opn(path.join(gamePath, EXEC_FOLDER)).catch(() => null);
    }
  }, () => {
    const state = context.api.getState();
    const gameId = selectors.activeGameId(state);
    return gameId === GAME_ID;
  });

  //Register mod installers
  context.registerInstaller(JSONMM_ID, 25, testJsonMM, installJsonMM);
  context.registerInstaller(ASILOADER_ID, 28, testAsiLoader, installAsiLoader);
  context.registerInstaller(ASI_ID, 30, testAsiMod, installAsiMod);
  context.registerInstaller(PRECOMP_ID, 32, testPrecompMod, installPrecompMod);
  context.registerInstaller(RESHADE_ID, 33, testReshadeMod, installReshadeMod);
  context.registerInstaller(JSONMOD_ID, 35, testJsonMod, installJsonMod);
}

//Main function
function main(context) {
  applyGame(context, spec);
  context.once(() => {
    context.api.onAsync('did-deploy', async (profileId, deployment) => {
      const LAST_ACTIVE_PROFILE = selectors.lastActiveProfileForGame(context.api.getState(), GAME_ID);
      if (profileId !== LAST_ACTIVE_PROFILE) return;
      checkAsiLoaderWarning(context.api);
      return deployNotify(context.api);
    });
  });
  return true;
}

module.exports = {
  default: main,
};
