# TypeAlias

多語言類型別名（Type Aliases）/ Multilingual Type Aliases / Alias de Tipos Multilingües

Supporting both **C#** and **TypeScript**!

## 中文說明（繁體）

### 這是什麼？

這個項目提供了多種語言的 C# 和 TypeScript 類型別名文件，讓開發者可以使用自己的母語來編寫代碼。

### ⚠️ 重要提醒

#### C# 類型別名

**這些別名不是 C# 關鍵字！** 它們是通過 `using` 指令創建的類型別名。這意味著：

- ✅ 這些是**類型別名**（Type Aliases），使用 `using 別名 = 實際類型;` 語法
- ❌ 這些**不是**編譯器內建的關鍵字（例如 `int`、`string`、`bool` 等）
- 📝 您需要在文件頂部引入相應的 `.cs` 文件才能使用這些別名
- 🔧 這些別名在 IntelliSense 中會顯示為原始的 .NET 類型名稱

#### TypeScript 類型別名

**這些別名不是 TypeScript 關鍵字！** 它們是通過 `type` 關鍵字創建的類型別名。這意味著：

- ✅ 這些是**類型別名**（Type Aliases），使用 `export type 別名 = 實際類型;` 語法
- ❌ 這些**不是**編譯器內建的關鍵字（例如 `string`、`number`、`boolean` 等）
- 📝 您需要導入這些別名才能使用
- 🔧 這些別名在 IntelliSense 中會顯示為原始的 TypeScript 類型名稱

### 範例

#### C# 範例

```csharp
// 使用中文別名
字符串 名字 = "張三";
三十二位元整數 年齡 = 25;
布林 已婚 = false;
控制台.WriteLine($"姓名：{名字}，年齡：{年齡}");
```

#### TypeScript 範例

```typescript
// 使用中文類型別名
import { 文本, 整數, 布林值 } from './中文類型別名';

const 名字: 文本 = "張三";
const 年齡: 整數 = 25;
const 已婚: 布林值 = false;
console.log(`姓名：${名字}，年齡：${年齡}`);
```

### 支援的語言

#### C# 類型別名
- 🇹🇼 繁體中文（Traditional Chinese）- `AliasDirective/中文命定譯.cs`
- 🇪🇸 西班牙語（Español）- `AliasDirective/EspañolDirectivaAlias.cs`

#### TypeScript 類型別名
- 🇬🇧 英語（English）- `TypeScriptAliases/english-type-aliases.ts`
- 🇪🇸 西班牙語（Español）- `TypeScriptAliases/español-alias-tipos.ts`
- 🇹🇼 繁體中文（Traditional Chinese）- `TypeScriptAliases/中文類型別名.ts`

---

## Descripción en Español

### ¿Qué es esto?

Este proyecto proporciona archivos de alias de tipos de C# y TypeScript en varios idiomas, permitiendo a los desarrolladores escribir código en su lengua materna.

### ⚠️ Aviso Importante

#### Alias de Tipos C#

**¡Estos alias NO son palabras clave de C#!** Son alias de tipos creados mediante directivas `using`. Esto significa:

- ✅ Son **alias de tipos** (Type Aliases), usando la sintaxis `using alias = TipoReal;`
- ❌ **NO** son palabras clave integradas del compilador (como `int`, `string`, `bool`, etc.)
- 📝 Necesitas incluir el archivo `.cs` correspondiente al inicio de tu archivo para usar estos alias
- 🔧 Estos alias aparecerán como los nombres de tipos .NET originales en IntelliSense

#### Alias de Tipos TypeScript

**¡Estos alias NO son palabras clave de TypeScript!** Son alias de tipos creados con la palabra clave `type`. Esto significa:

- ✅ Son **alias de tipos** (Type Aliases), usando la sintaxis `export type alias = TipoReal;`
- ❌ **NO** son palabras clave integradas del compilador (como `string`, `number`, `boolean`, etc.)
- 📝 Necesitas importar estos alias para usarlos
- 🔧 Estos alias aparecerán como los nombres de tipos TypeScript originales en IntelliSense

### Ejemplo

#### Ejemplo C#

```csharp
// Usando alias en español
cadena nombre = "Juan";
entero32 edad = 25;
booleano casado = false;
consola.WriteLine($"Nombre: {nombre}, Edad: {edad}");
```

#### Ejemplo TypeScript

```typescript
// Usando alias en español
import { Texto, Entero, Booleano } from './español-alias-tipos';

const nombre: Texto = "Juan";
const edad: Entero = 25;
const casado: Booleano = false;
console.log(`Nombre: ${nombre}, Edad: ${edad}`);
```

### Idiomas Soportados

#### Alias de Tipos C#
- 🇹🇼 Chino Tradicional (Traditional Chinese) - `AliasDirective/中文命定譯.cs`
- 🇪🇸 Español (Spanish) - `AliasDirective/EspañolDirectivaAlias.cs`

#### Alias de Tipos TypeScript
- 🇬🇧 Inglés (English) - `TypeScriptAliases/english-type-aliases.ts`
- 🇪🇸 Español (Spanish) - `TypeScriptAliases/español-alias-tipos.ts`
- 🇹🇼 Chino Tradicional (Traditional Chinese) - `TypeScriptAliases/中文類型別名.ts`

---

## Technical Details / 技術細節 / Detalles Técnicos

### Project Structure / 項目結構 / Estructura del Proyecto

```
TypeAlias/
├── AliasDirective/          # C# Type Aliases
│   ├── 中文命定譯.cs         # Chinese C# aliases
│   └── EspañolDirectivaAlias.cs  # Spanish C# aliases
└── TypeScriptAliases/       # TypeScript Type Aliases
    ├── english-type-aliases.ts   # English TS aliases
    ├── español-alias-tipos.ts    # Spanish TS aliases
    ├── 中文類型別名.ts           # Chinese TS aliases
    └── README.md            # TypeScript documentation
```

### How to Use / 如何使用 / Cómo Usar

#### C# Type Aliases

1. Include the appropriate alias file in your project
2. Reference it at the top of your C# file
3. Start using the aliases!

```csharp
// For Chinese / 中文
// #include or reference 中文命定譯.cs

// For Spanish / 西班牙語
// #include or reference EspañolDirectivaAlias.cs
```

#### TypeScript Type Aliases

1. Navigate to the TypeScriptAliases directory
2. Install dependencies: `npm install`
3. Import the aliases you need in your TypeScript files

```typescript
// For English
import { Text, Integer, TrueOrFalse } from './english-type-aliases';

// For Spanish / Para Español
import { Texto, Entero, Booleano } from './español-alias-tipos';

// For Chinese / 中文
import { 文本, 整數, 布林值 } from './中文類型別名';
```

See the [TypeScript README](TypeScriptAliases/README.md) for more details.

### Language Codes / 語言代碼 / Códigos de Idioma

Reference: https://en.wikipedia.org/wiki/List_of_ISO_639_language_codes

> https://en.wikipedia.org/wiki/IETF_language_tag

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