const path = require('path');
const { fs, util } = require('vortex-api');

const STEAM_ID = '1044720';
const GAME_ID = 'farthestfrontier';

function findGame() {
  return util.GameStoreHelper.findByName('Farthest Frontier')
    .catch(() => util.GameStoreHelper.findByAppId([STEAM_ID]))
    .then(game => game.gamePath);
}

function setup(discovery) {
  return fs.ensureDirWritableAsync(join(discovery.path, 'Mods'));
}

function main(context) {
  context.registerGame({
    id: GAME_ID,
    name: 'Farthest Frontier',
    logo: 'gameart.jpg',
    mergeMods: true,
    queryPath: findGame,
    queryModPath: () => 'Mods',
    executable: () => 'Farthest Frontier.exe',
    setup,
    requiredFiles: [
      'Farthest Frontier.exe'
    ],
    environment: {
      SteamAPPId: STEAM_ID,
    },
    details: {
      steamAppId: STEAM_ID,
    },
  });
}

module.exports = {
  default: main
};
