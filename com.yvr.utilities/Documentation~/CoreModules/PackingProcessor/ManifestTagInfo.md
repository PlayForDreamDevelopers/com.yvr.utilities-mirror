# ManifestTagInfo

[ManifestTagInfo](xref:YVR.Utilities.Editor.PackingProcessor.ManifestTagInfo) 存储有关清单标记的信息，包括其节点路径、标记名称、属性名称、属性值和任何其他属性等信息。

## 示例

以下 AndroidManifest 中的节点为例子：

如定义如下的 [ManifestTagInfo](xref:YVR.Utilities.Editor.PackingProcessor.ManifestTagInfo) 如下：

### 最常规的 Element 示例

```csharp
var openXRProviderQueries = new ManifestTagInfo
{
    nodePath = "/manifest/queries",
    tag = "provider",
    attrName = "authorities",
    attrValue = "org.khronos.openxr.runtime_broker;org.khronos.openxr.system_runtime_broker",
    attrs = null,
    required = true
};
```

> [!tip]
>
> [ManifestTagInfo](xref:YVR.Utilities.Editor.PackingProcessor.ManifestTagInfo) 是存在构造函数的，这里使用对象初始化器只是为了方便展示。

* `required` 数据表示该 Element 是否需要出现在 Manifest 中，其并不会在 element 中添加任何属性。

其定义的 AndroidManifest Element 如下：
![](assets/ManifestTagInfo/2025-04-10-14-09-31.excalidraw.svg)

### 包含有 Attrs 的 Element 示例

一个包含有 `attrs` 的 Element 示例如下：
```csharp
var manifestTagInfo = new ManifestTagInfo()
{
    nodePath = "/manifest/application",
    tag = "meta-data",
    attrName = "name",
    attrValue = "com.yvr.ossplash",
    attrs = new[] {"value", requiredSplash ? "true" : "false"},
    required = true
};
```

其中 `attrs` 为一个字符串数组，表示该 Element 中需要添加的属性，其元素数目一定是偶数，偶数下标的元素表示属性名称，奇数下标的元素表示属性值。如下示例元素 `attrs[0]` 的值为 `"value"`，`attrs[1]` 的值为 `"false"`

其定义的 AndroidManifest Element 如下：
![](assets/ManifestTagInfo/2025-04-10-14-34-03.excalidraw.svg)

> [!note]
>
> [ManifestTagInfo](xref:YVR.Utilities.Editor.PackingProcessor.ManifestTagInfo) 中的 `attrName` 和 `attrValue` 实质上与 `attrs` 数组定义无区别。这里单独列出只是为了保证至少有一组属性存在的

## 有嵌套 Tag 的 Element 示例

一个包含有嵌套 Tag 的 Element 示例如下：
```csharp
var openXRApiLayerQueries = new ManifestTagInfo
{
    nodePath = "/manifest/queries",
    tag = "intent/action",
    attrName = "name",
    attrValue = "org.khronos.openxr.OpenXRApiLayerService"
};
```

可以看到其中的 `tag` 属性值为 `"intent/action"`，表示该 Element 中的 `tag` 本身就含有嵌套结果，其定义的 AndroidManifest Element 如下：

![](assets/ManifestTagInfo/2025-04-10-14-46-43.excalidraw.svg)

> [!note]
>
> `nodePath` 定义了一个 Element 的父路径，`tag` 定义了该 Element 的自己的路径。


当只有一个嵌套 Tag 的 Manifest Element 时，可能无法体现出其嵌套的特性，但当有多个嵌套 Tag 时，便可以体现出其嵌套的特性。

假设我们定义了两个 `ManifestTagInfo`，如下所示：

```csharp
var openXRApiLayerQueries = new ManifestTagInfo
{
    nodePath = "/manifest/queries",
    tag = "intent/action",
    attrName = "name",
    attrValue = "org.khronos.openxr.OpenXRApiLayerService"
};

var openXRApiLayerQueries = new ManifestTagInfo
{
    nodePath = "/manifest/queries",
    tag = "intent/action",
    attrName = "name",
    attrValue = "org.khronos.openxr.OpenXRRuntimeService"
};
```

其结果为:

```xml
<manifest xmlns:android="http://schemas.android.com/apk/res/android" package="com.unity3d.player" android:sharedUserId="android.uid.system" xmlns:tools="http://schemas.android.com/tools">
  <queries>
    <intent>
      <action android:name="org.khronos.openxr.OpenXRRuntimeService" />
    </intent>
    <intent>
      <action android:name="org.khronos.openxr.OpenXRApiLayerService" />
    </intent>
  </queries>
</manifest>
```

而如果定义的方式为：

```csharp
var openXRApiLayerQueries = new ManifestTagInfo
{
    nodePath = "/manifest/queries/intent",
    tag = "action",
    attrName = "name",
    attrValue = "org.khronos.openxr.OpenXRApiLayerService"
};
var openXRApiLayerQueries = new ManifestTagInfo
{
    nodePath = "/manifest/queries/intent",
    tag = "action",
    attrName = "name",
    attrValue = "org.khronos.openxr.OpenXRRuntimeService"
};
```

其结果如下所示，这并不是一个合法的 Android Manifest Element 定义：

```xml
<manifest xmlns:android="http://schemas.android.com/apk/res/android" package="com.unity3d.player" android:sharedUserId="android.uid.system" xmlns:tools="http://schemas.android.com/tools">
  <queries>
    <intent>
      <action android:name="org.khronos.openxr.OpenXRRuntimeService" />
      <action android:name="org.khronos.openxr.OpenXRApiLayerService" />
    </intent>
  </queries>
</manifest>
```