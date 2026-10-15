# 绯血者 The Bloodbinder

《杀戮尖塔2》（Slay the Spire 2）新角色模组——一名以鲜血与痛楚为武器的近战战士，用生命换取力量。

A new character mod for Slay the Spire 2 — a melee fighter who wields blood and pain, trading life for power.

## 核心机制 / Core Mechanics

- **流血 Bleed**：每回合造成持续伤害，可被「血爆」引爆。
- **痛楚 Pain**：以受伤换取暴击与力量。
- **吸血 Lifesteal**：以血还血，造成伤害时回复生命。

## 内容 / Contents

- 99 张专属卡牌 / 99 unique cards
- 12 件遗物 / 12 relics
- 5 瓶药水 / 5 potions
- 26 个能力 / 26 powers
- 专属先古之民对话 / unique Ancient dialogues

## 安装（玩家）/ Installation (Players)

1. 下载最新的发布包（Release），得到三个文件：`bloodbinder.json`、`bloodbinder.pck`、`bloodbinder.dll`。
2. 找到游戏可执行文件所在目录，在旁边新建（或进入）`mods` 文件夹。
3. 在 `mods` 里新建子文件夹 `bloodbinder`，把三个文件放进去。
4. 启动游戏，在模组界面启用「绯血者」。

1. Download the latest Release, which contains three files: `bloodbinder.json`, `bloodbinder.pck`, `bloodbinder.dll`.
2. Locate the game executable directory and create (or open) a `mods` folder next to it.
3. Create a `bloodbinder` subfolder inside `mods` and place the three files there.
4. Launch the game and enable "The Bloodbinder" in the mod menu.

## 从源码构建 / Build from Source

环境要求：Godot 4.5.1 (.NET / Mono) + .NET 9。

1. 用 Godot 打开本项目，导出 PCK（`bloodbinder.pck`）。
2. 运行 `dotnet build mods.csproj -c Debug` 生成程序集，重命名为 `bloodbinder.dll`。
3. 与 `bloodbinder.json` 一起放入 `mods/bloodbinder/`。

Requires Godot 4.5.1 (.NET/Mono) and .NET 9. Open the project in Godot to export the PCK, then `dotnet build mods.csproj -c Debug` to produce the assembly (rename it to `bloodbinder.dll`).

## 作者 / Author

Sayoili
