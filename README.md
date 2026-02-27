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
- **Profile Management**: Support multiple profiles, each with independent key mappings
- **Case Sensitive**: Supports Shift+direction keys for sidestep symbols (⇈/⇊)
- **Hotkey Toggle**: Default F8 to quickly enable/disable without affecting normal typing
- **Highly Customizable**: Customize all key mappings to create your own input scheme
- **Import/Export**: Export and import profiles as JSON files for easy sharing and backup

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

#### Tekken Profile (Default):

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

#### Arrow Keys Profile (Simple Mode):

| Key | Output | Description |
|------|------|------|
| W | ↑ | Up |
| A | ← | Left |
| S | ↓ | Down |
| D | → | Right |
| Q | ↖ | Up-Left Diagonal |
| E | ↗ | Up-Right Diagonal |
| Z | ↙ | Down-Left Diagonal |
| C | ↘ | Down-Right Diagonal |

### 4. Profile Differences

**Tekken Profile**:
- Designed specifically for Tekken games
- Includes complete directional and button mappings
- Features combo buttons, special symbols, and more
- Supports case sensitivity (Shift+direction for sidestep symbols)
- Ideal for recording game moves and combos

**Arrow Keys Profile**:
- Simple directional key mapping
- Only includes basic directions and diagonals
- Suitable for general arrow input needs
- Cleaner interface

### 5. Profile Management

#### Profile Management Tab
- **Profile List**: Display all saved profiles
- **Switch to this Profile**: Switch to the selected profile
- **New Profile**: Create a new custom profile
- **Duplicate Profile**: Copy the selected profile as a new one
- **Delete Profile**: Delete the selected profile (cannot delete the last one)
- **Edit Profile**: Jump to the Key Mappings tab to edit the selected profile
- **Import Profile**: Import a profile from a JSON file
- **Export Profile**: Export the selected profile as a JSON file

#### Key Mappings Tab
- Display the key mapping table of the current profile
- **Add**: Add a new key mapping
- **Remove**: Remove the selected key mapping (click any cell in the row, then click Remove)
- **Save**: Save changes to the current profile
- **Cancel**: Discard changes and close the configuration window
- Supports case sensitivity (e.g., `w` and `W` can map to different symbols)
- Supports number key mapping (e.g., `8` can map to `↑`)

#### System Settings Tab
- **Run on Startup**: Configure whether to start with Windows
- **Start Minimized**: Configure whether to minimize to tray on startup
- **Activation Hotkey**: Set the hotkey to enable/disable the input method

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

## ⚙️ Configuration Files

Configuration files are saved in: `%APPDATA%\TekkenInputMethod\Profiles\`

- Each profile is an independent JSON file (`{ProfileID}.json`)
- `settings.json` saves the current active profile ID and system settings
- You can manually backup and copy configuration files

---

## 🗑️ Uninstallation

1. Close the input method program
2. Delete the configuration directory `%APPDATA%\TekkenInputMethod`
3. If you need to remove startup with Windows, please uncheck "Run on Startup" in the configuration

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 🙏 Acknowledgments

Thanks to all Tekken players who contributed ideas and feedback for this tool!
