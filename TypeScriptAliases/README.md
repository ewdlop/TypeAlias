# TypeScript Type Aliases

Multilingual TypeScript type aliases providing type names in different languages.

## Overview

This directory contains TypeScript type alias files in multiple languages, allowing developers to write code using their native language.

## ⚠️ Important Note

**These are type aliases, not TypeScript keywords!** They are created using the `type` keyword. This means:

- ✅ These are **type aliases** using the syntax `export type Alias = ActualType;`
- ❌ These are **not** built-in TypeScript keywords (like `string`, `number`, `boolean`, etc.)
- 📝 You need to import these aliases in your TypeScript files to use them
- 🔧 These aliases will show their underlying types in IntelliSense

## Supported Languages

- 🇬🇧 English - `english-type-aliases.ts`
- 🇪🇸 Spanish (Español) - `español-alias-tipos.ts`
- 🇹🇼 Traditional Chinese (繁體中文) - `中文類型別名.ts`

## How to Use

### Installation

```bash
cd TypeScriptAliases
npm install
```

### Building

```bash
npm run build
```

### Type Checking

```bash
npm run check
```

### Using in Your Code

#### English Example

```typescript
import { Text, Integer, TrueOrFalse, List } from './english-type-aliases';

const name: Text = "John Doe";
const age: Integer = 25;
const isMarried: TrueOrFalse = false;
const hobbies: List<Text> = ["reading", "coding", "gaming"];

console.log(`Name: ${name}, Age: ${age}, Married: ${isMarried}`);
```

#### Spanish Example (Ejemplo en Español)

```typescript
import { Texto, Entero, Booleano, Lista } from './español-alias-tipos';

const nombre: Texto = "Juan Pérez";
const edad: Entero = 25;
const casado: Booleano = false;
const pasatiempos: Lista<Texto> = ["leer", "programar", "jugar"];

console.log(`Nombre: ${nombre}, Edad: ${edad}, Casado: ${casado}`);
```

#### Chinese Example (中文範例)

```typescript
import { 文本, 整數, 布林值, 列表 } from './中文類型別名';

const 名字: 文本 = "張三";
const 年齡: 整數 = 25;
const 已婚: 布林值 = false;
const 愛好: 列表<文本> = ["閱讀", "編程", "遊戲"];

console.log(`姓名：${名字}，年齡：${年齡}，已婚：${已婚}`);
```

## Available Type Aliases

Each language file includes aliases for:

### Basic Types
- String/Text types
- Number/Integer types
- Boolean types
- Date/Time types
- Byte arrays

### Function Types
- Void functions
- Async operations (Promises)
- Callbacks
- Predicates
- Mappers
- Comparators

### Collection Types
- Arrays/Lists
- Dictionaries/Records
- Maps
- Sets
- Tuples

### Utility Types
- Optional/Nullable types
- Non-nullable types
- Object types

### DOM Types
- HTML elements
- Event listeners
- Mouse/Keyboard events

### HTTP/Network Types
- HTTP headers
- JSON types

### Time Types
- Milliseconds, Seconds, Minutes, Hours
- Timestamps

### File Types
- File names and paths
- MIME types

### ID Types
- Unique identifiers
- UUIDs

### Result Types
- Result/Either types for error handling

### Advanced Types
- Constructors
- Type guards
- Async functions

## Contributing

Want to add more languages? Feel free to contribute!

## License

This is an educational and experimental project.
