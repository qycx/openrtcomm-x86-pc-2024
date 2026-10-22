# openRtComm (mono-openRtComm) X86 PC 客户端 — 源码历史归档

本仓库是 **openRtComm 视频通信系统客户端（X86 PC 版本）** 的**只读历史档案**，仅作代码留存与溯源之用。

> ⚠️ 原始快照位于 `original-archive/openRtComm.zip.20241201.zip`，本仓库已剔除所有构建产物与版本控制元数据，仅保留源码与资源。

## 来源介质

| 项目 | 说明 |
|---|---|
| 介质文件 | `openRtComm.zip.20241201.zip` |
| 介质大小 | 32,696,723 字节（约 31.2 MiB） |
| 介质 SHA-256 | `a89138aba6b4cfd4cfcaff4cebf15e43fa69122f008db6cbff251cc21c42d9c9` |
| 介质日期 | 2024-12-01（压缩包内部时间戳） |
| 项目时间 | 2025 年 1 月（用户标注版本时间） |
| 原始说明 | “MIT 开源 视频通信系统客户端 X86 PC 版本” |

## 工程实质

源码解包后实际为 **C# / .NET / XAML 工程**（并非纯 C++），由 Xamarin 绑定库与 C# 主程序构成，对应 openRtComm 跨平台客户端的 .NET/C# 实现（Gitee 仓库名 `mono-openRtComm`）。

## 模块结构

| 目录 | 内容 |
|---|---|
| `qyMessengerApp/` | 主程序（C# / XAML，含 `.sln` 解决方案 `qyMessengerApp.sln`） |
| `myBindingClassLibrary/` | Xamarin Java Binding 基础库 |
| `myBindingClassLibrary_autosize/` | 自动尺寸适配绑定 |
| `myBindingClassLibrary_easypermission/` | 权限申请绑定 |
| `myBindingClassLibrary_ffmpeg/` | FFmpeg 绑定（对应 Android 端 ffmpeg） |
| `myBindingClassLibrary_gles/` | OpenGL ES 绑定 |
| `myBindingClassLibrary_tmpJavaLib/` | tmpJavaLib 绑定 |

## 归档处理

为保证「历史档案」纯净，已从源码树中剔除以下内容（均非版本控制对象）：

- **版本控制元数据**：内嵌的完整 `.git/` 目录（40 个文件，约 15 MiB）
- **构建产物**：`obj/`、`bin/`、`.vs/` 目录；`.dll`、`.aar`、`.jar`、`.pdb`、`.cache`、`.log`、`.stamp`、`.rsp`、`.user`、`.class-parse`、`.fixed`、Xamarin `obj/Debug/__AndroidLibraryProjects__.zip` 等二进制/中间产物
- **IDE 产物**：`UpgradeLog.htm`（Visual Studio 升级日志）

仓库根保留的项目自带文件：`.gitignore`、原 `README.md`（Gitee 占位模板，已另存为 `README.gitee.md`）、`README.en.md`。

## 字节保真

所有文本/源码文件均**保留原始字节**，未做任何编码转换或换行符（CRLF/LF）改写。相关规则由 `.gitattributes` 中的 `* -text` 强制保证，确保 2000 年代工程在 Windows 上的原始状态可被准确复现。

## 版权与许可

用户标注本项目为 **MIT 开源**。但原始快照中**未包含 `LICENSE` 文件**，原 `README.md` 仅为 Gitee 建仓占位模板（无实质项目描述，已另存 `README.gitee.md`）。如需正式 MIT 许可，建议后续补充 `LICENSE` 文件。
