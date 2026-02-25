# Tekken Input Method

[![.NET 6](https://img.shields.io/badge/.NET-6.0-blue)](https://dotnet.microsoft.com/)
[![Platform](https://img.shields.io/badge/Platform-Windows-lightgrey)](https://www.microsoft.com/windows)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

[中文](README.zh-CN.md) | English | [日本語](README.ja.md) | [한국어](README.ko.md)

A system-wide input method for Tekken notation on Windows. Convert keyboard input to Tekken-style directional and button symbols in real-time.

---

## 📖 Introduction

Tekken Input Method is a system-level input tool for Windows designed specifically for players of the Tekken fighting game series. It converts standard keyboard input (like WASD) into Tekken community-standard notation symbols (like ⬆⬅⬇➡, ❶❷❸❹, etc.) in real-time, making it easy for players to write guides, discuss moves, and record combos using standard notation.

### Key Features

- **System-wide Input**: Works in any application (QQ, WeChat, Notepad, browsers, etc.)
- **Dual Mode Design**: Tekken mode for gaming, Arrow mode for daily use
- **Case Sensitive**: Supports Shift+direction keys for sidestep symbols (⇈/⇊)
- **Hotkey Toggle**: Default F8 to quickly enable/disable without affecting normal typing
- **Highly Customizable**: Customize all key mappings to create your own input scheme

---

## 🚀 Usage

### 1. Launch
- Double-click `TekkenInputMethod.exe`
- The program will run in the system tray

### 2. Activate
- **Hotkey**: Press `F8` to toggle on/off (default hotkey, can be customized; avoid using keys that will be converted)
- Or click the "Activate" button in the main window
- Or use the system tray menu
- Once activated, typing WASD and other keys will automatically convert to corresponding symbols

### 3. Input Mappings

#### Tekken Mode (Default):

| Key | Output | Description |
|-----|--------|-------------|
| W | ⬆ | Up |
| Shift+W | ⇈ | Sidestep In |
| A | ⬅ | Left |
| S | ⬇ | Down |
| Shift+S | ⇊ | Sidestep Out |
| D | ➡ | Right |
| Q | ↖ | Up-Left Diagonal |
| E | ↗ | Up-Right Diagonal |
| Z | ↙ | Down-Left Diagonal |
| C | ↘ | Down-Right Diagonal |
| J | ❶ | Button 1 (Left Punch) |
| I | ❷ | Button 2 (Right Punch) |
| K | ❸ | Button 3 (Left Kick) |
| L | ❹ | Button 4 (Right Kick) |
| U | ❶✚❷ | Combo Buttons 1+2 |
| O | ❸✚❹ | Combo Buttons 3+4 |
| Space | ▸ | Connector |
| N | ✩ | Neutral |
| X | ⇩ | Crouch |
| H | Ⓗ | H Function |
| R | Ⓡ | R Function |
| T | Ⓣ | T Function |
| B | Ⓑ | B Function |

#### Arrow Keys Mode (Simple Mode):

| Key | Output | Description |
|------|------|------|
| W | ⬆ | Up |
| A | ⬅ | Left |
| S | ⬇ | Down |
| D | ➡ | Right |
| Q | ↖ | Up-Left Diagonal |
| E | ↗ | Up-Right Diagonal |
| Z | ↙ | Down-Left Diagonal |
| C | ↘ | Down-Right Diagonal |

### 4. Mode Differences

**Tekken Mode**:
- Designed specifically for Tekken games
- Includes complete directional and button mappings
- Features combo buttons, special symbols, and more
- Supports case sensitivity (Shift+direction for sidestep symbols)
- Ideal for recording game moves and combos

**Arrow Keys Mode**:
- Simple directional key mapping
- Only includes basic directions and diagonals
- Suitable for general arrow input needs
- Cleaner interface

### 5. Configuration
- Click the "Config" button to open the configuration window
- **Key Mappings Tab**: Customize key mappings with case sensitivity support (e.g., `w` and `W` can map to different symbols)
- **Preset Modes Tab**:
  - View detailed mode descriptions
  - One-click switch between Tekken mode and Arrow mode
  - Restore default settings
- **System Settings Tab**: Configure startup with Windows, customize hotkey, etc.

### 6. System Tray Functions
- Right-click the tray icon
- Quickly activate/deactivate the input method
- Open configuration window
- Exit the program

### 7. Deactivate
- Press `F8` hotkey again (or your customized hotkey)
- Click the "Activate" button again
- Or use the system tray menu to select "Deactivate"

---

## 🔧 Technical Improvements

### Key Interception Mechanism
- Uses low-level keyboard hooks to intercept key events
- Blocks original key transmission after successful mapping
- Ensures only converted characters are output

### Character Compatibility
- Uses standard Unicode characters with Emoji support required
- Displays correctly in QQ, WeChat, and other applications
- Diagonal symbols use universal encoding

---

## 📝 Notes

1. **Admin Rights**: Some features may require administrator privileges
2. **Antivirus**: Add to whitelist if blocked by antivirus software
3. **Compatibility**: Supports Windows 10/11
4. **Log Files**: Logs are saved in `%APPDATA%\TekkenInputMethod\Logs` directory

---

## ⚙️ Configuration File

Configuration file location: `%APPDATA%\TekkenInputMethod\config.json`

You can manually edit this file to customize mapping relationships.

---

## 🗑️ Uninstallation

1. Close the input method program
2. Delete the configuration directory `%APPDATA%\TekkenInputMethod`
3. To remove startup with Windows, uncheck "Startup with Windows" in the configuration

---

## 🆘 Technical Support

If you encounter issues, please check the log files.

---

## ⚠️ Known Limitations

- Diagonal symbols (↖↗↙↘) in QQ may be escaped in certain situations. This is caused by QQ's rich text processing mechanism. The input method uses standard Unicode characters, but the display processing on the application side is beyond the input method's control.
