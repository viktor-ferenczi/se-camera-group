# Space Engineers Camera Group Plugin

Allows cycling over groups of cameras, turrets, turret controllers and search lights
using a single toolbar button.

Thanks goes to **mkaito** for the great plugin idea, pair-working and testing.

## Prerequisites

- [Space Engineers](https://store.steampowered.com/app/244850/Space_Engineers/)
- [Plugin Loader](https://github.com/sepluginloader/PluginLoader/)

## Usage

1. Enable the Camera Group plugin in Plugin Loader, Apply, restart the game
2. Group the relevant blocks (cameras, turrets, turret controllers or search lights)
3. Add the groups to the relevant cockpit or RC toolbar as a View or Control action
4. Use the toolbar buttons to cycle through (view or control) the working blocks

## Remarks

- The blocks in each group are selected in the order of their name
- Broken or disabled blocks are automatically skipped
- The plugin works for both offline and online multiplayer games
- Game saves and blueprints preserve the toolbar actions configured
- Only players with the plugin installed will be able to use the toolbar action
- No adverse effect on server performance, this is only a client side QoL plugin