# Turtix2Data
Tools and info to understand and manipulate "Turtix: Rescue Adventures" game files.

Stuff is missing here and there but the main features are there.

## Packer tools
All the packer tools use the same command line arguments:
* Extract: u(npack) [Input.iex] [Output directory]
* Repack: p(ack) [Output.iex] [Input directory]

## Content files
- **Sound.iex**: Can be modified using the "SoundPacker" tool. Contains OGGs.
- **Graphics.iex** / **Language.iex** / **Localization.iex**: Can be modified using the "GraphicsPacker" tool. More info in the "Graphics format" section.
- **Logo_X.iex**: They are JPGs.
- **Data.iex**: Can be modified using the "DataPacker" tool. Contains many things like levels, guis, scripts, etc.

## Graphics format:
Graphics is stored in 3 different files.

There are 2 JPGs, one stores color data, the other stores alpha in a grayscale format.

The third file stores tileset information, cell IDs, locations and sizes.

##Patches
The "Patches" folder contains x64dbg patch files, which can be applied to "Turtix.exe".
- **windowed720p.1337**: Makes the game run in a window in 1280x720.
- **fullscreen1080p.1337**: Makes the game run in fullscreen 1920x1080.