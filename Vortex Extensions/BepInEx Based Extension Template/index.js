const path = require('path');
const { fs, util } = require('vortex-api');

const STEAM_ID = '599140';
const GOG_ID = '1780408621';
const GAME_ID = 'graveyardkeeper';

function findGame() {
  return util.GameStoreHelper.findByName('Graveyard Keeper')
      .catch((err) => {
        util.logger.error(`Error finding game by name: ${err.message}`);
        util.logger.error(err.stack || err);
        return util.GameStoreHelper.findByAppId([STEAM_ID, GOG_ID]);
      })
      .then(game => {
        if (game) {
          util.logger.info(`Game found at path: ${game.gamePath}`);
          return game.gamePath;
        } else {
          throw new Error('Game not found by App ID.');
        }
      })
      .catch((err) => {
        util.logger.error(`Error finding game by App ID: ${err.message}`);
        util.logger.error(err.stack || err);
        throw err;
      });
}

function setup(discovery) {
  const pluginsPath = path.join(discovery.path, 'BepInEx', 'plugins');

  return fs.statAsync(pluginsPath)
      .then(stat => {
        if (!stat.isDirectory()) {
          throw new Error(`${pluginsPath} exists but is not a directory`);
        }
        util.logger.info(`Plugins directory verified at: ${pluginsPath}`);
      })
      .catch(err => {
        if (err.code === 'ENOENT') {
          util.logger.warn(`Plugins directory does not exist at: ${pluginsPath}, creating it now.`);
          return fs.ensureDirAsync(pluginsPath);
        } else {
          // Some other error occurred
          util.logger.error(`An error occurred while setting up the BepInEx plugins directory at ${pluginsPath}: ${err.message}`);
          util.logger.error(err.stack || err);
          throw err;
        }
      })
      .then(() => {
        util.logger.info(`Ensuring directory is writable: ${pluginsPath}`);
        return fs.ensureDirWritableAsync(pluginsPath);
      })
      .catch(err => {
        util.logger.error(`Failed to set up BepInEx plugins directory at ${pluginsPath}: ${err.message}`);
        util.logger.error(err.stack || err);
        throw err;
      });
}

function main(context) {
  context.requireExtension('modtype-bepinex');
  util.logger.info(`Registering game with ID: ${GAME_ID}`);

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
    util.logger.info('Running post-registration setup for BepInEx.');

    if (context.api.ext.bepinexAddGame !== undefined) {
      util.logger.info('Adding BepInEx game configuration.');

      context.api.ext.bepinexAddGame({
        gameId: GAME_ID,
        autoDownloadBepInEx: true,
        customPackDownloader: () => {
          return {
            gameId: GAME_ID,
            domainId: GAME_ID,
            modId: '79',
            fileId: '723',
            archiveName: 'BepInEx - Windows-Wine-Proton.zip',
            allowAutoInstall: true,
          };
        },
        doorstopConfig: {
          doorstopType: 'default',
          ignoreDisableSwitch: true,
        }
      });

      util.logger.info('BepInEx game configuration added successfully.');
    } else {
      util.logger.warn('BepInEx extension is not available.');
    }
  });
}

module.exports = {
  default: main
};