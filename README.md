# obsidian-char-mapper
obsidian插件,特殊字符(emoji)键盘映射工具
(快捷键)开启映射模式后,像正常文字输入似的,输入你预先设置好的常用的特殊字符
## 起初想法是在obsidian笔记中,方便简单的记录俺经常玩的铁拳8出招指令
### 例如:
| 浮空技  |                |        |              | 伤害  |
| ---- | -------------- | ------ | ------------ | --- |
| ↗❹   | ❹▸⭢⭢✩▸⭢❸▸⭠❶❷Ⓣ▸ | ⭢❸+❹▸❷ | ⭢⭠↙⭣↘⭢❶/❷    |     |

# Obsidian Character Mapper

A powerful keyboard character mapping plugin for Obsidian. Convert any key to any character with custom mappings.

## ✨ Features

- **Custom Key Mapping**: Define your own key-to-character mappings
- **Quick Activation**: Type `\mp` to activate mapping mode
- **Multiple Presets**: Built-in presets for gamepad mode and arrow keys
- **Easy Configuration**: Simple text-based configuration in settings
- **Real-time Mapping**: Instantly insert mapped characters while typing

## 🎮 Default Mappings (Gamepad Mode)

### Movement Keys
| Key | Output | Purpose |
|-----|--------|---------|
| W | ⭡ | Up |
| A | ⭠ | Left |
| S | ⭣ | Down |
| D | ⭢ | Right |

### Diagonal Keys
| Key | Output | Purpose |
|-----|--------|---------|
| Q | ↖ | Up-Left |
| E | ↗ | Up-Right |
| Z | ↙ | Down-Left |
| C | ↘ | Down-Right |

### Action Buttons
| Key | Output | Purpose |
|-----|--------|---------|
| J | ❶ | Button 1 |
| I | ❷ | Button 2 |
| K | ❸ | Button 3 |
| L | ❹ | Button 4 |
| U | ❶+❷ | Button Combo 1 |
| O | ❸+❹ | Button Combo 2 |

### Special Keys
| Key | Output | Purpose |
|-----|--------|---------|
| X | 蹲 | Crouch |
| Space | ▸ | Play/Sprint |
| N | ✩ | Special |
| H | Ⓗ | H Function |
| R | Ⓡ | R Function |
| T | Ⓣ | T Function |

## 🚀 Installation

### Method 1: Using BRAT (Recommended)

1. Install [BRAT](https://github.com/TfTHacker/obsidian42-brat) plugin from community plugins
2. Open BRAT settings
3. Add this repository: `https://github.com/sh2288/obsidian-char-mapper`
4. Install and enable "Character Mapper"

### Method 2: Manual Installation

1. Download the latest release
2. Extract files to: `VaultFolder/.obsidian/plugins/obsidian-char-mapper/`
3. Files needed: `main.js`, `manifest.json`
4. Enable the plugin in Obsidian settings

## 📖 Usage

### Activation Methods

**Method 1: Type `\mp`**
- Type `\mp` in your note
- Select "⌨️ Character Mapper" from suggestions
- Start typing with mapped keys
- Press `ESC` to deactivate

**Method 2: Command Palette**
- Press `Ctrl+P` (or `Cmd+P`)
- Search for "Activate Character Mapper"
- Press Enter
- Press `ESC` to deactivate

### Configuration

1. Go to Settings → Character Mapper
2. Edit the text area with your custom mappings
3. Format: `key=character` (one per line)
4. Click **"💾 Save and Apply"** to save changes
5. Or use quick presets: "🎮 Gamepad Mode" or "↑↓←→ Arrow Keys Mode"

## ⚙️ Customization

### Custom Mappings

In settings, you can define any key-to-character mapping:

