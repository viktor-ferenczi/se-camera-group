# Space Engineers Camera Group Plugin

Allows cycling over groups of cameras, searchlights, turrets and turret controllers
using a single toolbar button.

Configuration option to disable the third person view even if the world would allow
for using that. This option is for increased realism and should only be used by
experienced SE players.

Thanks goes to **mkaito** for the great plugin idea, pair-working and testing.

## Prerequisites

- [Space Engineers](https://store.steampowered.com/app/244850/Space_Engineers/)
- [Plugin Loader](https://github.com/sepluginloader/PluginLoader/)

## Usage

1. Enable the Camera Group plugin in Plugin Loader, Apply, restart the game
2. Group the relevant blocks (cameras, searchlights, turrets or turret controllers)
3. Add the groups to the relevant cockpit or RC toolbar as a View or Control action
4. Use the toolbar buttons to cycle through (view or control) the working blocks

Do NOT group cameras with the other supported blocks. Cameras have a View action,
while the searchlight and turrets have a Control action. They cannot be mixed.

## Remarks

- Broken or disabled blocks are automatically skipped
- The blocks in each group are selected in the order of their name
- The plugin works for both offline and online multiplayer games
- Game saves and blueprints preserve the toolbar actions configured
- Only players with the plugin installed will be able to use the toolbar action
- No adverse effect on server performance, this is only a client side QoL plugin