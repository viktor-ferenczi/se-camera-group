# Space Engineers Camera Group Plugin

Allows viewing groups of cameras in a round-robin manner 
using only a single toolbar slot.

Thanks goes to **mkaito** for the great plugin idea.

## Prerequisites

- [Space Engineers](https://store.steampowered.com/app/244850/Space_Engineers/)
- [Plugin Loader](https://github.com/sepluginloader/PluginLoader/)

## Usage

1. Enable the Camera Group plugin in Plugin Loader, Apply, restart the game
2. Add the cameras on your ship to a single group
3. Add that group to your cockpit toolbar with the View action, which is now available
4. Use that toolbar slot to view each of the working cameras in a round-robin manner

## Remarks

- Cameras are selected in the order of their name
- Broken or disabled cameras are automatically skipped
- The plugin works for both offline and online multiplayer games
- Game saves and blueprints preserve the camera group view toolbar action