# ManifestPreprocessor

一个静态类，提供通过传入的 [ManifestTagInfo](./ManifestTagInfo.md) 列表信息修改 `AndroidManifest.xml` 文件的方法。

其中只包含有一个函数 [PatchAndroidManifest](<xref:YVR.Utilities.Editor.PackingProcessor.ManifestProcessor.PatchAndroidManifest(System.Collections.Generic.List{YVR.Utilities.Editor.PackingProcessor.ManifestTagInfo},System.String,System.String)>)，该函数加纳一个 `ManifestTagInfo` 列表和一个 `AndroidManifest.xml` 文件路径(`sourceFile`)，返回一个修改后的 `AndroidManifest.xml` 文件路径(`destinationFile`)。

-   若 `destinationFile` 为 `null`，则表示修改后的文件为 `sourceFile` 文件本身。

正确修改 `AndroidManifest.xml` 文件后和核心在于正确的配置 `ManifestTagInfo` 列表，下面将介绍如何配置 `ManifestTagInfo` 列表，具体可参考 [ManifestTagInfo](./ManifestTagInfo.md) 的介绍。
