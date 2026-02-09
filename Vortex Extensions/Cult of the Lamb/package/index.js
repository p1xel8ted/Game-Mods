const path = require('path');
const { fs, util } = require('vortex-api');

const STEAM_ID = '1313140';
const GOG_ID = '2034949552';
const GAME_ID = 'cultofthelamb';

function findGame() {
  return util.GameStoreHelper.findByName('Cult of the Lamb')
      .catch(() => util.GameStoreHelper.findByAppId([STEAM_ID, GOG_ID]))
      .then(game => game.gamePath);
}

function setup(discovery) {
  const pluginsPath = path.join(discovery.path, 'BepInEx', 'plugins');

  return fs.statAsync(pluginsPath)
      .then(stat => {
        if (!stat.isDirectory()) {
          throw new Error(`${pluginsPath} exists but is not a directory`);
        }
      })
      .catch(err => {
        if (err.code === 'ENOENT') {
          // Directory does not exist, so create it
          return fs.ensureDirAsync(pluginsPath);
        } else {
          // Some other error occurred
          throw err;
        }
      })
      .then(() => fs.ensureDirWritableAsync(pluginsPath))
      .catch(err => {
        // Log the error to the Vortex logger
        util.logger.error(`Failed to set up BepInEx plugins directory at ${pluginsPath}: ${err.message}`);
        // Optionally, rethrow the error to prevent further setup if needed
        throw err;
      });
}

function main(context) {
  context.requireExtension('modtype-bepinex');
  context.registerGame({
    id: GAME_ID,
    name: 'Cult of the Lamb',
    logo: 'gameart.jpg',
    mergeMods: false,
    queryPath: findGame,
    queryModPath: () => path.join('BepInEx', 'plugins'),
    executable: () => 'Cult Of The Lamb.exe',
    setup,
    requiredFiles: [
      'Cult Of The Lamb.exe'
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

  context.once(() => {
    if (context.api.ext.bepinexAddGame !== undefined) {
      context.api.ext.bepinexAddGame({
        gameId: GAME_ID,
        autoDownloadBepInEx: true,
        customPackDownloader: () => {
          return {
            // The game extension's domain Id/gameId as defined when registering
            // the extension - in this case lets say it's moonlighter
            gameId: GAME_ID,
            // We extracted this from the pack's mod page
            domainId: GAME_ID,
            // Same as the domain Id, extracted from the URL
            modId: '31',
            // We extracted this one by hovering over the download buttons on the site
            fileId: '258',
            // What we want to call the archive of the downloaded pack.
            archiveName: 'BepInEx - Windows-Wine-Proton.zip',
            // Whether we want this to be installed automatically - should always be true
            allowAutoInstall: true,
          };
        },
        doorstopConfig: {
          doorstopType: 'default',
          ignoreDisableSwitch: true,
        }
      });
    }
  });
}

module.exports = {
  default: main
};