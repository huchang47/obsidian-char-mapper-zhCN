const { Plugin, Notice, PluginSettingTab, Setting, Editor, EditorSuggest, TAbstractFile, TFolder } = require('obsidian');

const DEFAULT_SETTINGS = {
    charMappings: {
        'w': '⭡',
        'a': '⭠',
        's': '⭣',
        'd': '⭢',
        'q': '↖',
        'e': '↗',
        'z': '↙',
        'c': '↘',
        'x': '蹲',
        'j': '❶',
        'i': '❷',
        'k': '❸',
        'l': '❹',
        'u': '❶+❷',
        'o': '❸+❹',
        ' ': '▸',
        'n': '✩',
        'h': 'Ⓗ',
        'r': 'Ⓡ',
        't': 'Ⓣ'
    }
};

class CharMapperPlugin extends Plugin {
    async onload() {
        await this.loadSettings();

        // 添加激活命令（保留备用）
        this.addCommand({
            id: 'activate-char-mapper',
            name: 'Activate Character Mapper',
            editorCallback: (editor) => {
                this.activateCharMapper(editor);
            }
        });

        // 注册编辑器建议器（用于 \mp 命令）
        this.registerEditorSuggest(new CharMapperSuggest(this));

        // 添加设置选项卡
        this.addSettingTab(new CharMapperSettingTab(this.app, this));

        console.log('✅ Character Mapper Plugin loaded');
    }

    activateCharMapper(editor) {
        if (this.isMapperActive) {
            new Notice('Character Mapper already active');
            return;
        }

        this.isMapperActive = true;
        this.currentEditor = editor;
        new Notice('✅ Character Mapper activated. Press ESC to deactivate.');

        // 绑定键盘事件
        this.keyDownListener = (evt) => {
            this.handleCharMapping(evt);
        };

        document.addEventListener('keydown', this.keyDownListener, true);
    }

    handleCharMapping(evt) {
        if (!this.isMapperActive || !this.currentEditor) {
            return;
        }

        const key = evt.key === ' ' ? ' ' : evt.key.toLowerCase();

        // ESC 退出映射模式
        if (evt.key === 'Escape') {
            this.deactivateCharMapper();
            evt.preventDefault();
            return;
        }

        // 检查是否有对应的映射
        if (this.settings.charMappings[key]) {
            const mappedChar = this.settings.charMappings[key];
            const cursor = this.currentEditor.getCursor();

            // 插入字符
            this.currentEditor.replaceRange(mappedChar, cursor);

            // 移动光标到插入字符之后
            this.currentEditor.setCursor({
                line: cursor.line,
                ch: cursor.ch + mappedChar.length
            });

            evt.preventDefault();
        }
    }

    deactivateCharMapper() {
        this.isMapperActive = false;
        this.currentEditor = null;

        if (this.keyDownListener) {
            document.removeEventListener('keydown', this.keyDownListener, true);
            this.keyDownListener = null;
        }

        new Notice('❌ Character Mapper deactivated');
    }

    onunload() {
        this.deactivateCharMapper();
        console.log('Character Mapper Plugin unloaded');
    }

    async loadSettings() {
        this.settings = Object.assign({}, DEFAULT_SETTINGS, await this.loadData());
    }

    async saveSettings() {
        await this.saveData(this.settings);
    }
}

// 编辑器建议器类 - 实现 \mp 斜杠命令
class CharMapperSuggest extends EditorSuggest {
    constructor(plugin) {
        super(plugin.app);
        this.plugin = plugin;
    }

    onTrigger(cursor, editor) {
        const line = editor.getLine(cursor.line);
        const beforeCursor = line.substring(0, cursor.ch);

        // 检查是否输入了 \mp
        if (beforeCursor.endsWith('\\mp')) {
            return {
                start: {
                    line: cursor.line,
                    ch: cursor.ch - 3
                },
                end: cursor,
                query: 'mp'
            };
        }

        return null;
    }

    getSuggestions(context) {
        // 返回建议
        return [
            {
                title: '⌨️ Character Mapper',
                description: 'Activate character mapping mode'
            }
        ];
    }

    renderSuggestion(value, el) {
        el.createEl('div', { text: value.title });
        el.createEl('small', { text: value.description });
    }

    selectSuggestion(value, evt) {
        const editor = this.context.editor;
        const cursor = this.context.start;

        // 删除 \mp 文本
        editor.replaceRange('', cursor, this.context.end);

        // 激活字符映射模式
        this.plugin.activateCharMapper(editor);
    }
}

class CharMapperSettingTab extends PluginSettingTab {
    constructor(app, plugin) {
        super(app, plugin);
        this.plugin = plugin;
    }

    display() {
        const { containerEl } = this;
        containerEl.empty();

        containerEl.createEl('h2', { text: '⌨️ 字符映射设置' });

        containerEl.createEl('p', {
            text: '自定义字符映射关系。每行一个映射，格式：按键=输出字符',
            cls: 'setting-item-description'
        });

        // 文本区域设置
        const textAreaSetting = new Setting(containerEl)
            .setName('字符映射')
            .setDesc('按键=字符，每行一个');

        let textAreaElement = null;

        textAreaSetting.addTextArea((text) => {
            textAreaElement = text;
            text
                .setPlaceholder('w=⬆\na=⬅\ns=⬇\nd=➡')
                .setValue(this.getMappingsText());
            
            // 移除自动保存，改为手动保存
            text.onChange((value) => {
                // 只显示临时更改，不自动保存
                console.log('临时更改（尚未保存）:', value);
            });
        });

        // 保存并应用按钮
        new Setting(containerEl)
            .setName('保存设置')
            .setDesc('点击按钮保存并应用当前的按键映射')
            .addButton((btn) =>
                btn
                    .setButtonText('💾 保存并应用')
                    .setCta()
                    .onClick(async () => {
                        if (textAreaElement) {
                            const newMappings = this.parseMappings(textAreaElement.getValue());
                            
                            // 检查是否有有效的映射
                            if (Object.keys(newMappings).length === 0) {
                                new Notice('❌ 错误：没有有效的按键映射');
                                return;
                            }

                            // 应用新的映射
                            this.plugin.settings.charMappings = newMappings;
                            await this.plugin.saveSettings();

                            // 提示成功
                            new Notice('✅ 按键映射已保存并应用！');
                            console.log('已保存的映射:', newMappings);
                        }
                    })
            );

        // 快速预设按钮
        containerEl.createEl('h3', { text: '⚡ 快速预设' });

        new Setting(containerEl)
            .addButton((btn) =>
                btn
                    .setButtonText('🎮 游戏手柄模式')
                    .onClick(async () => {
                        const presetMappings = {
                            'w': '⭡',
                            'a': '⭠',
                            's': '⭣',
                            'd': '⭢',
                            'q': '↖',
                            'e': '↗',
                            'z': '↙',
                            'c': '↘',
                            'x': '蹲',
                            'j': '❶',
                            'i': '❷',
                            'k': '❸',
                            'l': '❹',
                            'u': '❶+❷',
                            'o': '❸+❹',
                            ' ': '▸',
                            'n': '✩',
                            'h': 'Ⓗ',
                            'r': 'Ⓡ',
                            't': 'Ⓣ'
                        };
                        this.plugin.settings.charMappings = presetMappings;
                        await this.plugin.saveSettings();
                        new Notice('✅ 已应用游戏手柄模式');
                        this.display();
                    })
            )
            .addButton((btn) =>
                btn
                    .setButtonText('↑↓←→ 方向键模式')
                    .onClick(async () => {
                        const presetMappings = {
                            'w': '⬆',
                            'a': '⬅',
                            's': '⬇',
                            'd': '➡'
                        };
                        this.plugin.settings.charMappings = presetMappings;
                        await this.plugin.saveSettings();
                        new Notice('✅ 已应用方向键模式');
                        this.display();
                    })
            );

        // 使用说明
        containerEl.createEl('h3', { text: '📖 使用说明' });
        
        const instructions = containerEl.createEl('div', {
            cls: 'setting-item-description'
        });
        
        instructions.createEl('p', { text: '🔧 编辑方法：' });
        instructions.createEl('ol').createEl('li', { text: '在上方文本框中编辑按键映射（格式：按键=字符）' });
        instructions.createEl('ol').createEl('li', { text: '点击"💾 保存并应用"按钮保存更改' });
        instructions.createEl('ol').createEl('li', { text: '或点击预设按钮快速应用' });
        
        instructions.createEl('p', { text: '⚙️ 激活方法：' });
        instructions.createEl('ol').createEl('li', { text: '在笔记中输入 \\mp' });
        instructions.createEl('ol').createEl('li', { text: '选择建议中的"Character Mapper"' });
        instructions.createEl('ol').createEl('li', { text: '开始按键输入' });
        
        instructions.createEl('p', { text: '🎮 当前模式：游戏手柄模式' });
        instructions.createEl('p', { text: '按键映射：' });
        instructions.createEl('ul').createEl('li', { text: 'WASD: 方向 (⭡⭠⭣⭢)' });
        instructions.createEl('ul').createEl('li', { text: 'QEZC: 对角线 (↖↗↙↘)' });
        instructions.createEl('ul').createEl('li', { text: 'JIKL: 按钮 (❶❷❸❹)' });
        instructions.createEl('ul').createEl('li', { text: 'UO: 组合按钮' });
        instructions.createEl('ul').createEl('li', { text: '其他: X蹲, Space▸, N✩, H/R/T符号' });
        
        instructions.createEl('p', { text: '⛔ 退出：按 ESC 键' });
    }

    getMappingsText() {
        return Object.entries(this.plugin.settings.charMappings)
            .map(([key, char]) => {
                // 特殊处理空格键的显示
                if (key === ' ') {
                    return `SPACE=${char}`;
                }
                return `${key}=${char}`;
            })
            .join('\n');
    }

    parseMappings(text) {
        const mappings = {};
        text.split('\n').forEach((line) => {
            const parts = line.split('=');
            if (parts.length === 2) {
                let key = parts[0].trim().toLowerCase();
                const char = parts[1].trim();
                
                // 特殊处理 SPACE
                if (key === 'space') {
                    key = ' ';
                }
                
                if (key && char) {
                    mappings[key] = char;
                }
            }
        });
        return mappings;
    }
}

module.exports = CharMapperPlugin;