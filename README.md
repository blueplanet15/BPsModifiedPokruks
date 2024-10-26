# Pokruk's Camera Mod

## Main Exclusive Features
* No camera delay, no more laggy cosmetics **glitching** on the camera.
* Settings save: smoothing, camera clipping, FOV — all that **automatically saves**, so you don't need to set it up every time you restart the game.
* Adjustable camera activation bind.
* You can adjust camera bind settings at runtime, no need to restart the game.
* Spectating other players

## Controls
* Press your camera activation bind to spawn the camera in front of you (see the controls config guide below).
* Hold the grip on the banana handle to grab the camera.
* Press **Tab** on the IRL keyboard for monitor UI.
* Use **WASD/arrow keys** for freecam (also a toggle for the gamepad).

### How to Set Camera Activation Bind
1. Start the game with the mod running at least once.
2. Open the `YourGameRootFolder/CameraMod/Configs/Controls.json` config.
3. Set the `activateBind` to the aliases of the buttons you want:

| Alias | Generic meaning | Quest 2 controllers meaning |
|----------|----------|----------|
| RP   | Right Primary  | A |
| RS   | Right Secondary  | B |
| LP   | Left Primary   | X  |
| LS   | Left Secondary | Y  |
4. Save the file and watch controls change immediately (you don't need to restart the game).

### Examples
If you want to bind the camera activation to multiple buttons, set the config bind value like this:

`Controls.json`
```json
{
  "activateBind": "RP RS LP LS"
}
```
If you want to bind it to just the Right Primary button (which is A for Quest 2 controllers):

`Controls.json`
```json
{
  "activateBind": "RP"
}
```
And so on.

---
Camera Mod with in game UI!
### *Features:*
* Monitor UI (tab to enable, shitty OnGUI but im lazy so cope)
* Freecam with gamepad support
* Spectator with ajustable offset (and a toggle to control it with wasd)
* First Person View with smoothing
* Third Person View (like gtags default camera just smoother,misc page for settings, front/back and follow head rotation)
* Follow Player (Camera will look at and follow player, misc page for settings, minimum distance and speed)
* Grabbable Handles (you can only grab right side with right hand and left side with left hand)
* Green Screen (in city)
* Adjustable FOV, Nearclip and smoothing

# *Disclamers:*
* **This product is not affiliated with Gorilla Tag or Another Axiom LLC and is not endorsed or otherwise sponsored by Another Axiom LLC. Portions of the materials contained herein are property of Another Axiom LLC. ©2021 Another Axiom LLC.**
* **controls can be different depending what you're playing on(steamvr,oculuspcvr,index, etc.)**
