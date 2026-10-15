# 绯血者（Bloodbinder）Spine 动画资源规格

> 本文档与 `art_guide.md`（卡牌/图标/UI 静态资源）互补，聚焦**角色 Spine 骨骼动画**的资源制作与提示词。

## 0. 软件与版本

| 项 | 值 |
|----|----|
| Spine 版本 | 3.8.75（3.8 系列最终版） |
| 本地软件路径 | `D:\实用\Spine绿色学习版spine软件安装教程【鱼】\Spinepro_3.8.75学习版\Spine pro 3.8.75` |
| 导出格式 | Spine 3.8（`.json` 内 `"skeleton": { "spine": "3.8.75" }`） |

> ⚠️ **兼容性**：Spine 3.8 与 4.x 导出格式互不兼容。接入 Godot 前需确认框架 `MegaSpine`（`MegaSprite` / `MegaSkeletonDataResource`）支持 3.8 格式，否则加载会失败或动画错乱。

## 1. 需要交付的资源

### 运行时必需（3 件套，缺一不可）

| 文件 | 作用 |
|------|------|
| 骨架数据 `.json` 或 `.skel`（二进制，推荐） | 骨骼层级、插槽、皮肤、动画关键帧 |
| 图集描述 `.atlas` | 记录每块纹理在图集里的坐标/旋转/尺寸 |
| 纹理图集 `.png` | 所有部件打包成一张大图（多页自动命名 `xxx.png`、`xxx2.png`…） |

### 源文件（强烈建议一并交付）

- `.spine` 项目文件（方便后续改动画/加皮肤，导出文件单向不可逆）
- 原始分层切图（PSD/AI/PNG 分层）

## 2. 动画名硬性要求

`src/Core/Models/Characters/Bloodbinder.cs` 的 `GenerateAnimator()` 已写死状态机，Spine 骨骼**必须包含**以下 6 个动画名，缺一个对应状态就切不过去：

| 动画名 | 含义 | 是否循环 |
|--------|------|----------|
| `idle_loop` | 待机 | 是 |
| `cast` | 施法 | 否 |
| `attack` | 攻击 | 否 |
| `hurt` | 受击 | 否 |
| `die` | 死亡 | 否 |
| `relaxed_loop` | 放松 | 是 |

资源到位后，把 `VisualsPath` 指向绯血者视觉场景即可。

## 3. AI 生成提示词

AI 绘图主要吃英文，提示词统一给英文。

### 3.1 风格基座（所有图都追加这段，保证风格统一）

```
gothic dark fantasy game art, eastern blood magic aesthetic, crimson and dark red color palette with dark purple and black gold accents, blood manipulation motifs, vein-like textures, dark red crystals, crescent moon and chain details, low-key lighting with high contrast, blood moon atmosphere, stylized painterly 2D character art, clean lineart, high detail
```

### 3.2 A-pose 全身底图（核心，Spine 拆件靠它）

Spine 需要把角色拆成可动部件，先有一张**四肢张开、无遮挡**的底图，再手动拆件。

```
full body character, front view, A-pose, arms slightly spread away from body at 30 degrees, legs shoulder-width apart, standing straight, facing forward, perfectly symmetrical, clean silhouette, no limb overlap, hands open with fingers clearly visible

character: a young female blood knight, pale skin, long flowing black hair with crimson red streaks, glowing crimson eyes, gothic black and crimson armored dress with dark red crystal ornaments and metal chain details, flowing blood-red ribbon accents

[风格基座], plain dark grey background, character only, full body visible from head to toe
```

> 关键：`A-pose / arms spread / no limb overlap / symmetrical` —— 四肢不交叉、关节露出来，后面拆件和补画遮挡才省事。

### 3.3 独立道具 / 特效件（Spine 里做挂件和 VFX）

武器、锁链、血滴单独生成透明背景图，比从底图抠更干净。

**血刃（武器）**

```
a crimson blood crystal greatsword, jagged translucent dark red blade with vein-like glowing patterns, black gold hilt wrapped in chains, floating blood droplets around the blade, [风格基座], isolated on transparent background, game asset, no character
```

**锁链（挂件）**

```
dark iron chains with glowing red runes, crescent moon charm hanging at the end, blood mist drifting along the links, [风格基座], isolated on transparent background, game asset, no character
```

**血滴 / 血雾（VFX 粒子）**

```
single stylized blood droplet, glossy dark red with crimson glow, painterly 2D game VFX element, [风格基座], isolated on transparent background, no character
```

## 4. Spine 3.8 标准拆件清单（绑骨骼用）

| 区域 | 需要的部件 |
|------|-----------|
| 头 | 头/脸、后发、前发（刘海）、眉×2、眼×2、嘴 |
| 躯干 | 脖子、上躯干（胸）、下躯干（腰）、骨盆 |
| 上肢 | 上臂×2、前臂×2、手×2 |
| 下肢 | 大腿×2、小腿×2、脚×2 |
| 附加 | 披风/飘带、武器（血刃）、锁链、血滴 VFX |

## 5. AI 图拆件的注意事项

1. **关节重叠区**：AI 生成时四肢和躯干连在一起，拆件后肩、胯、肘会缺一块。拆件时要在关节处「补画」被遮住的部分——所以底图要求 `arms spread`，就是为了减少补画量。
2. **对称性**：AI 容易画歪脸/手，可加 `perfectly symmetrical`，或生成后做镜像处理。
3. **透明底**：`transparent background` 或纯色底均可；纯色底选与角色反差大的颜色（浅灰/深灰），方便一键抠图。

## 6. 交付规格（统一）

- 骨架数据：`.skel`（二进制，优先）或 `.json`
- 图集纹理：PNG，透明背景
- 画布尺寸：战斗角色 Spine 一般高 **512~1024px**，按原版角色规格定
- 命名：骨骼/插槽/动画名用英文小写蛇形或驼峰，避免中文和空格
