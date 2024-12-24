const path = require('path');
const { fs, util } = require('vortex-api');

const STEAM_ID = '599140';
const GOG_ID = '1780408621';
const GAME_ID = 'graveyardkeeper';

function findGame(context) {
  return util.GameStoreHelper.findByName('Graveyard Keeper')
      .catch(() => {
        return util.GameStoreHelper.findByAppId([STEAM_ID, GOG_ID]);
      })
      .then(game => {
        if (game) {
          return game.gamePath;
        } else {
          throw new Error('Game not found by App ID.');
        }
      });
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
          return fs.ensureDirAsync(pluginsPath);
        } else {
          throw err;
        }
      })
      .then(() => {
        return fs.ensureDirWritableAsync(pluginsPath);
      });
}

function main(context) {
  context.requireExtension('modtype-bepinex');

  context.registerGame({
    id: GAME_ID,
    name: 'Graveyard Keeper',
    logo: 'gameart.jpg',
    mergeMods: false,
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

  context.once(() => {
    if (context.api.ext.bepinexAddGame !== undefined) {
      context.api.ext.bepinexAddGame({
        gameId: GAME_ID,
        autoDownloadBepInEx: true,
        customPackDownloader: () => {
          return {
            gameId: GAME_ID,
            domainId: GAME_ID,
            modId: '79',
            fileId: '723',
            archiveName: 'BepInEx - GYK - Windows-Wine-Proton.zip',
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
