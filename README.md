# TypeAlias

多語言 C# 類型別名（Type Aliases）/ Alias de Tipos C# Multilingües

## 中文說明（繁體）

### 這是什麼？

這個項目提供了多種語言的 C# 類型別名文件，讓開發者可以使用自己的母語來編寫代碼。

### ⚠️ 重要提醒

**這些別名不是 C# 關鍵字！** 它們是通過 `using` 指令創建的類型別名。這意味著：

- ✅ 這些是**類型別名**（Type Aliases），使用 `using 別名 = 實際類型;` 語法
- ❌ 這些**不是**編譯器內建的關鍵字（例如 `int`、`string`、`bool` 等）
- 📝 您需要在文件頂部引入相應的 `.cs` 文件才能使用這些別名
- 🔧 這些別名在 IntelliSense 中會顯示為原始的 .NET 類型名稱

### 範例

```csharp
// 使用中文別名
字符串 名字 = "張三";
三十二位元整數 年齡 = 25;
布林 已婚 = false;
控制台.WriteLine($"姓名：{名字}，年齡：{年齡}");
```

### 支援的語言

- 🇹🇼 繁體中文（Traditional Chinese）- `中文命定譯.cs.cs`
- 🇪🇸 西班牙語（Español）- `EspañolDirectivaAlias.cs`

---

## Descripción en Español

### ¿Qué es esto?

Este proyecto proporciona archivos de alias de tipos de C# en varios idiomas, permitiendo a los desarrolladores escribir código en su lengua materna.

### ⚠️ Aviso Importante

**¡Estos alias NO son palabras clave de C#!** Son alias de tipos creados mediante directivas `using`. Esto significa:

- ✅ Son **alias de tipos** (Type Aliases), usando la sintaxis `using alias = TipoReal;`
- ❌ **NO** son palabras clave integradas del compilador (como `int`, `string`, `bool`, etc.)
- 📝 Necesitas incluir el archivo `.cs` correspondiente al inicio de tu archivo para usar estos alias
- 🔧 Estos alias aparecerán como los nombres de tipos .NET originales en IntelliSense

### Ejemplo

```csharp
// Usando alias en español
cadena nombre = "Juan";
entero32 edad = 25;
booleano casado = false;
consola.WriteLine($"Nombre: {nombre}, Edad: {edad}");
```

### Idiomas Soportados

- 🇹🇼 Chino Tradicional（Traditional Chinese）- `中文命定譯.cs.cs`
- 🇪🇸 Español（Spanish）- `EspañolDirectivaAlias.cs`

---

## Technical Details / 技術細節 / Detalles Técnicos

### How to Use / 如何使用 / Cómo Usar

1. Include the appropriate alias file in your project
2. Reference it at the top of your C# file
3. Start using the aliases!

```csharp
// For Chinese / 中文
// #include or reference 中文命定譯.cs.cs

// For Spanish / 西班牙語
// #include or reference EspañolDirectivaAlias.cs
```

### Language Codes / 語言代碼 / Códigos de Idioma

Reference: https://en.wikipedia.org/wiki/List_of_ISO_639_language_codes

---

## Contributing / 貢獻 / Contribuir

Want to add more languages? Feel free to contribute!

想添加更多語言？歡迎貢獻！

¿Quieres agregar más idiomas? ¡Siéntete libre de contribuir!

---

## License / 許可證 / Licencia

This is an educational and experimental project.

這是一個教育和實驗性項目。

Este es un proyecto educativo y experimental.