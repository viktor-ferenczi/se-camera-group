# Space Engineers Camera Group Plugin

Allows viewing groups of cameras in a round-robin manner 
using only a single toolbar slot.

## Prerequisites

- [Space Engineers](https://store.steampowered.com/app/244850/Space_Engineers/)
- [Plugin Loader](https://github.com/sepluginloader/PluginLoader/)

## Usage

1. Enable the Camera Group plugin in Plugin Loader, Apply, restart the game
2. Add the cameras on your ship to a single group
3. Add that group to your cockpit toolbar with the View action, which is now available
4. Use that toolbar slot to view each of the working cameras in a round-robin manner

## Remarks

- Cameras are always selected in the same order determined by their name
- Broken cameras are automatically skipped from the rotation
- The action becomes disabled if all the cameras are broken or missing or the group is deleted
- The plugin works for both offline and online multiplayer games
- Only those who have the client side plugin can use the camera group view action
- While this toolbar action is not available in the game by default the save files and blueprints properly preserve it