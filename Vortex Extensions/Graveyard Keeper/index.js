const path = require('path');
const os = require('os');
const { fs, util, log, selectors, actions } = require('vortex-api');

const STEAM_ID = '599140';
const GOG_ID = '1780408621';
const GAME_ID = 'graveyardkeeper';

const BEPINEX_MOD_ID = '79';
const BEPINEX_FILE_ID = '867';
const BEPINEX_ARCHIVE = 'BepInEx - GYK - Windows-Wine-Proton.zip';
const BEPINEX_NXM_URL = `nxm://${GAME_ID}/mods/${BEPINEX_MOD_ID}/files/${BEPINEX_FILE_ID}`;
const BEPINEX_PAGE_URL = `https://www.nexusmods.com/${GAME_ID}/mods/${BEPINEX_MOD_ID}`;

const BEPINEX_PACK_MODTYPE = 'bepinex-pack-gyk';
const BEPINEX_PLUGIN_MODTYPE = 'bepinex-plugin-gyk';
const BEPINEX_PATCHER_MODTYPE = 'bepinex-patcher-gyk';
const GYK_SAVE_MODTYPE = 'gyk-save';
const GYK_RESOURCES_MODTYPE = 'gyk-resources';

function findGame() {
  return util.GameStoreHelper.findByName('Graveyard Keeper')
      .catch((err) => {
        log('error', `Error finding game by name: ${err.message}`);
        log('error', err.stack || err);
        return util.GameStoreHelper.findByAppId([STEAM_ID, GOG_ID]);
      })
      .then(game => {
        if (game) {
          log('info', `Game found at path: ${game.gamePath}`);
          return game.gamePath;
        } else {
          throw new Error('Game not found by App ID.');
        }
      })
      .catch((err) => {
        log('error', `Error finding game by App ID: ${err.message}`);
        log('error', err.stack || err);
        throw err;
      });
}

function setup(discovery) {
  const bepinexPath = path.join(discovery.path, 'BepInEx');
  const requiredDirs = [
    path.join(bepinexPath, 'plugins'),
    path.join(bepinexPath, 'patchers'),
  ];

  return Promise.all(requiredDirs.map(dir =>
    fs.ensureDirWritableAsync(dir)
      .then(() => log('info', `Directory verified/created: ${dir}`))
      .catch(err => {
        log('error', `Failed to set up directory ${dir}: ${err.message}`);
        log('error', err.stack || err);
        throw err;
      })
  ));
}

function isPackArchive(files) {
  const hasDoorstop = files.some(f => /^winhttp\.dll$/i.test(path.basename(f)));
  const hasCore = files.some(f => /^BepInEx[\\\/]core[\\\/].+\.dll$/i.test(f));
  return hasDoorstop && hasCore;
}

function testBepInExPack(files, gameId) {
  if (gameId !== GAME_ID) {
    return Promise.resolve({ supported: false, requiredFiles: [] });
  }
  return Promise.resolve({ supported: isPackArchive(files), requiredFiles: [] });
}

function installBepInExPack(files) {
  const instructions = files
    .filter(f => !f.endsWith(path.sep))
    .map(f => ({ type: 'copy', source: f, destination: f }));
  instructions.push({ type: 'setmodtype', value: BEPINEX_PACK_MODTYPE });
  return Promise.resolve({ instructions });
}

const FRAMEWORK_DLL_RX = /^(0Harmony|BepInEx(\..+)?|HarmonyX|Mono\.Cecil.*|Newtonsoft\.Json|System\..+|MonoMod\..+)\.dll$/i;

function stripBepInExPrefix(file, kind) {
  const patterns = kind === 'patcher'
    ? [/^BepInEx[\\\/]patchers[\\\/](.*)/i, /^patchers[\\\/](.*)/i]
    : [/^BepInEx[\\\/]plugins[\\\/](.*)/i, /^plugins[\\\/](.*)/i];
  for (const rx of patterns) {
    const m = file.match(rx);
    if (m) {
      return m[1];
    }
  }
  return file;
}

function pickWrapName(strippedPaths, destinationPath) {
  const dlls = strippedPaths.filter(f => /\.dll$/i.test(f));
  const nonFramework = dlls.filter(f => !FRAMEWORK_DLL_RX.test(path.basename(f)));
  const chosen = nonFramework[0] || dlls[0];
  if (chosen) {
    return path.basename(chosen, path.extname(chosen));
  }
  return path.basename(destinationPath);
}

function buildWrappedInstructions(files, destinationPath, kind, modType) {
  const nonFolder = files.filter(f => !f.endsWith(path.sep));
  const stripped = nonFolder.map(f => stripBepInExPrefix(f, kind));

  let allHaveTopLevel = stripped.length > 0;
  const topLevelFolders = new Set();
  for (const f of stripped) {
    const parts = f.split(/[\\\/]/).filter(p => p.length > 0);
    if (parts.length < 2) {
      allHaveTopLevel = false;
      break;
    }
    topLevelFolders.add(parts[0].toLowerCase());
  }

  let instructions;
  if (allHaveTopLevel && topLevelFolders.size === 1) {
    instructions = nonFolder.map((src, i) => ({
      type: 'copy',
      source: src,
      destination: stripped[i],
    }));
  } else {
    const wrapName = pickWrapName(stripped, destinationPath);
    instructions = nonFolder.map((src, i) => ({
      type: 'copy',
      source: src,
      destination: path.join(wrapName, stripped[i]),
    }));
  }
  instructions.push({ type: 'setmodtype', value: modType });
  return { instructions };
}

function testBepInExPatcher(files, gameId) {
  if (gameId !== GAME_ID || isPackArchive(files)) {
    return Promise.resolve({ supported: false, requiredFiles: [] });
  }
  const hasPatcherContent = files.some(f => /^BepInEx[\\\/]patchers[\\\/].+\.dll$/i.test(f));
  return Promise.resolve({ supported: hasPatcherContent, requiredFiles: [] });
}

function installBepInExPatcher(files, destinationPath) {
  return Promise.resolve(
    buildWrappedInstructions(files, destinationPath, 'patcher', BEPINEX_PATCHER_MODTYPE)
  );
}

function testBepInExPlugin(files, gameId) {
  if (gameId !== GAME_ID || isPackArchive(files)) {
    return Promise.resolve({ supported: false, requiredFiles: [] });
  }
  const hasPatcherContent = files.some(f => /^BepInEx[\\\/]patchers[\\\/].+\.dll$/i.test(f));
  const hasPluginContent = files.some(f => /^BepInEx[\\\/]plugins[\\\/].+\.dll$/i.test(f));
  const hasRootDll = files.some(f => /^[^\\\/]+\.dll$/i.test(f));
  if (hasPatcherContent && !hasPluginContent && !hasRootDll) {
    return Promise.resolve({ supported: false, requiredFiles: [] });
  }
  const hasAnyDll = files.some(f => /\.dll$/i.test(f));
  return Promise.resolve({ supported: hasAnyDll, requiredFiles: [] });
}

function installBepInExPlugin(files, destinationPath) {
  return Promise.resolve(
    buildWrappedInstructions(files, destinationPath, 'plugin', BEPINEX_PLUGIN_MODTYPE)
  );
}

function isBepInExInstalled(api) {
  const state = api.getState();
  const mods = state.persistent.mods[GAME_ID] || {};
  return Object.values(mods).some(m => m && m.type === BEPINEX_PACK_MODTYPE);
}

function promptInstallBepInEx(api) {
  if (selectors.activeGameId(api.getState()) !== GAME_ID) {
    return;
  }
  if (isBepInExInstalled(api)) {
    return;
  }
  api.sendNotification({
    id: `${GAME_ID}-install-prompt`,
    type: 'info',
    title: 'BepInEx required',
    message: 'Most Graveyard Keeper mods need the BepInEx framework. Click Install to download the curated pack from NexusMods.',
    noDismiss: false,
    actions: [
      {
        title: 'Install',
        action: (dismiss) => {
          dismiss();
          installBepInEx(api).catch(err => {
            log('error', `Error during prompted BepInEx install: ${err.message}`);
          });
        },
      },
      {
        title: 'Dismiss',
        action: (dismiss) => dismiss(),
      },
    ],
  });
}

async function installBepInEx(api) {
  const NOTIF_ID = `${GAME_ID}-bepinex-installing`;

  if (isBepInExInstalled(api)) {
    api.sendNotification({
      id: `${GAME_ID}-bepinex-already`,
      type: 'info',
      message: 'BepInEx is already installed for Graveyard Keeper.',
      displayMS: 4000,
    });
    return;
  }

  api.sendNotification({
    id: NOTIF_ID,
    message: 'Downloading BepInEx for Graveyard Keeper',
    type: 'activity',
    noDismiss: true,
    allowSuppress: false,
  });

  if (api.ext && typeof api.ext.ensureLoggedIn === 'function') {
    await api.ext.ensureLoggedIn();
  }

  try {
    const dlInfo = {
      game: GAME_ID,
      name: BEPINEX_ARCHIVE,
    };
    const dlId = await util.toPromise(cb =>
      api.events.emit('start-download', [BEPINEX_NXM_URL], dlInfo, undefined, cb, undefined, { allowInstall: false }));
    const modId = await util.toPromise(cb =>
      api.events.emit('start-install-download', dlId, { allowAutoEnable: false }, cb));

    const profileId = selectors.lastActiveProfileForGame(api.getState(), GAME_ID);
    if (profileId && modId) {
      util.batchDispatch(api.store, [
        actions.setModType(GAME_ID, modId, BEPINEX_PACK_MODTYPE),
        actions.setModsEnabled(api, profileId, [modId], true, {
          allowAutoDeploy: true,
          installed: true,
        }),
      ]);
    }

    api.sendNotification({
      id: `${GAME_ID}-bepinex-done`,
      type: 'success',
      message: 'BepInEx installed. Deploy mods to finish setup.',
      displayMS: 6000,
    });
  } catch (err) {
    api.showErrorNotification('Failed to download/install BepInEx', err, { allowReport: false });
    util.opn(BEPINEX_PAGE_URL).catch(() => null);
  } finally {
    api.dismissNotification(NOTIF_ID);
  }
}

function pluginsPathFor(api) {
  const gamePath = util.getSafe(api.getState(), ['settings', 'gameMode', 'discovered', GAME_ID, 'path'], undefined);
  return gamePath ? path.join(gamePath, 'BepInEx', 'plugins') : undefined;
}

function patchersPathFor(api) {
  const gamePath = util.getSafe(api.getState(), ['settings', 'gameMode', 'discovered', GAME_ID, 'path'], undefined);
  return gamePath ? path.join(gamePath, 'BepInEx', 'patchers') : undefined;
}

function gameRootFor(api) {
  return util.getSafe(api.getState(), ['settings', 'gameMode', 'discovered', GAME_ID, 'path'], undefined);
}

function resourcesPathFor(api) {
  const gamePath = gameRootFor(api);
  return gamePath ? path.join(gamePath, 'Graveyard Keeper_Data') : undefined;
}

function saveGamePath() {
  const home = os.homedir();
  switch (process.platform) {
    case 'win32':
      return path.join(home, 'AppData', 'LocalLow', 'Lazy Bear Games', 'Graveyard Keeper');
    case 'darwin':
      return path.join(home, 'Library', 'Application Support', 'Lazy Bear Games', 'Graveyard Keeper');
    case 'linux':
      return path.join(home, '.config', 'unity3d', 'Lazy Bear Games', 'Graveyard Keeper');
    default:
      return undefined;
  }
}

function testSaveGame(files, gameId) {
  if (gameId !== GAME_ID) {
    return Promise.resolve({ supported: false, requiredFiles: [] });
  }
  if (files.some(f => /\.dll$/i.test(f))) {
    return Promise.resolve({ supported: false, requiredFiles: [] });
  }
  const hasDat = files.some(f => /\.dat$/i.test(path.basename(f)));
  const hasInfo = files.some(f => /\.info$/i.test(path.basename(f)));
  return Promise.resolve({ supported: hasDat && hasInfo, requiredFiles: [] });
}

function installSaveGame(files) {
  const instructions = files
    .filter(f => !f.endsWith(path.sep))
    .map(f => ({ type: 'copy', source: f, destination: path.basename(f) }));
  instructions.push({ type: 'setmodtype', value: GYK_SAVE_MODTYPE });
  return Promise.resolve({ instructions });
}

function testResources(files, gameId) {
  if (gameId !== GAME_ID) {
    return Promise.resolve({ supported: false, requiredFiles: [] });
  }
  if (files.some(f => /\.dll$/i.test(f))) {
    return Promise.resolve({ supported: false, requiredFiles: [] });
  }
  const hasAssets = files.some(f => /^resources\.assets/i.test(path.basename(f)));
  return Promise.resolve({ supported: hasAssets, requiredFiles: [] });
}

function installResources(files) {
  const instructions = files
    .filter(f => !f.endsWith(path.sep))
    .map(f => ({ type: 'copy', source: f, destination: path.basename(f) }));
  instructions.push({ type: 'setmodtype', value: GYK_RESOURCES_MODTYPE });
  return Promise.resolve({ instructions });
}

function main(context) {
  context.requireExtension('modtype-bepinex');
  log('info', `Registering game with ID: ${GAME_ID}`);

  context.registerGame({
    id: GAME_ID,
    name: 'Graveyard Keeper',
    logo: 'gameart.jpg',
    mergeMods: true,
    queryPath: findGame,
    queryModPath: () => path.join('BepInEx', 'plugins'),
    executable: () => 'Graveyard Keeper.exe',
    setup,
    requiredFiles: [
      'Graveyard Keeper.exe'
    ],
    environment: {
      SteamAPPId: STEAM_ID,
      gogAPPId: GOG_ID
    },
    details: {
      steamAppId: STEAM_ID,
      gogAPPId: GOG_ID
    },
  });

  context.registerModType(
    BEPINEX_PACK_MODTYPE,
    25,
    (gameId) => gameId === GAME_ID,
    () => gameRootFor(context.api),
    () => Promise.resolve(false),
    { name: 'BepInEx Pack' }
  );

  context.registerModType(
    BEPINEX_PATCHER_MODTYPE,
    27,
    (gameId) => gameId === GAME_ID,
    () => patchersPathFor(context.api),
    () => Promise.resolve(false),
    { name: 'BepInEx Patcher' }
  );

  context.registerModType(
    BEPINEX_PLUGIN_MODTYPE,
    30,
    (gameId) => gameId === GAME_ID,
    () => pluginsPathFor(context.api),
    () => Promise.resolve(false),
    { name: 'BepInEx Plugin' }
  );

  context.registerModType(
    GYK_SAVE_MODTYPE,
    28,
    (gameId) => gameId === GAME_ID,
    () => saveGamePath(),
    () => Promise.resolve(false),
    { name: 'Save Game' }
  );

  context.registerModType(
    GYK_RESOURCES_MODTYPE,
    29,
    (gameId) => gameId === GAME_ID,
    () => resourcesPathFor(context.api),
    () => Promise.resolve(false),
    { name: 'Resource Replacement' }
  );

  context.registerInstaller('bepinex-pack-gyk-installer', 25, testBepInExPack, installBepInExPack);
  context.registerInstaller('bepinex-patcher-gyk-installer', 27, testBepInExPatcher, installBepInExPatcher);
  context.registerInstaller('gyk-save-installer', 28, testSaveGame, installSaveGame);
  context.registerInstaller('gyk-resources-installer', 29, testResources, installResources);
  context.registerInstaller('bepinex-plugin-gyk-installer', 30, testBepInExPlugin, installBepInExPlugin);

  context.registerAction('mod-icons', 300, 'download', {}, 'Install BepInEx', () => {
    installBepInEx(context.api).catch(err => {
      log('error', `Unhandled error installing BepInEx: ${err.message}`);
    });
  }, () => {
    const state = context.api.getState();
    return selectors.activeGameId(state) === GAME_ID;
  });

  context.registerAction('mod-icons', 305, 'open-ext', {}, 'Open Save Games Folder', () => {
    const savePath = saveGamePath();
    if (!savePath) {
      log('warn', 'No save games path is defined for this platform.');
      return;
    }
    fs.ensureDirWritableAsync(savePath)
      .then(() => util.opn(savePath))
      .catch(err => log('warn', `Could not open save games folder at ${savePath}: ${err.message}`));
  }, () => {
    const state = context.api.getState();
    return selectors.activeGameId(state) === GAME_ID;
  });

  context.once(() => {
    context.api.events.on('gamemode-activated', (gameId) => {
      if (gameId === GAME_ID) {
        promptInstallBepInEx(context.api);
      }
    });
  });
}

module.exports = {
  default: main
};
