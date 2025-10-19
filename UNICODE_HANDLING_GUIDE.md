# Unicode Handling Guide

This guide provides comprehensive best practices for handling Unicode across different programming languages, with emphasis on source encoding, string representation, and internationalization.

## 📝 Source Code Encoding

**Project Standard: UTF-8**

- All source files should be saved in UTF-8 encoding
- Most modern compilers and tools support UTF-8 by default
- UTF-8 encoding prevents mojibake (乱码/garbled text) issues
- Configure your IDE/editor to save files as UTF-8

### Editor Configuration Examples

**VS Code** (`settings.json`):
```json
{
  "files.encoding": "utf8",
  "files.autoGuessEncoding": false
}
```

**JetBrains IDEs** (IntelliJ, PyCharm, etc.):
- File → Settings → Editor → File Encodings → Project Encoding: UTF-8

**Vim** (`.vimrc`):
```vim
set encoding=utf-8
set fileencoding=utf-8
```

## 🌍 String & Unicode by Language

Different programming languages handle Unicode strings differently. Understanding these differences is crucial for proper text processing.

### Python 3

**String Type**: `str` is Unicode by default

```python
# Python 3 strings are Unicode
text = "Hello 世界 🌍"
print(len(text))  # 10 (counts Unicode code points, not bytes)

# Encode to bytes when needed
utf8_bytes = text.encode('utf-8')
print(len(utf8_bytes))  # More than 10 (byte length)

# Decode back to string
decoded = utf8_bytes.decode('utf-8')

# Working with code points
for char in text:
    print(f"{char}: U+{ord(char):04X}")

# Unicode normalization
from unicodedata import normalize

# Composed form: é (single code point)
composed = "café"
# Decomposed form: café (e + combining acute accent)
decomposed = "café"

print(composed == decomposed)  # False (different representations)

# Normalize to NFD (decomposed)
nfd = normalize('NFD', composed)
# Normalize to NFC (composed)
nfc = normalize('NFC', decomposed)

print(normalize('NFC', composed) == normalize('NFC', decomposed))  # True
```

**Best Practices**:
- Always use `str` for text, `bytes` for binary data
- Use normalization for text comparison
- Be aware of grapheme clusters (emojis with modifiers)

### Rust

**String Types**: `String` and `&str` are UTF-8 encoded

```rust
fn main() {
    // String and &str are always valid UTF-8
    let text = "Hello 世界 🌍";
    
    // Length in bytes (UTF-8 encoded)
    println!("Byte length: {}", text.len());  // More than 10
    
    // Iterate over characters (code points)
    println!("Char count: {}", text.chars().count());  // 10
    
    // Iterate over grapheme clusters (requires unicode-segmentation crate)
    // use unicode_segmentation::UnicodeSegmentation;
    // for grapheme in text.graphemes(true) {
    //     println!("{}", grapheme);
    // }
    
    // Working with code points
    for ch in text.chars() {
        println!("{}: U+{:04X}", ch, ch as u32);
    }
    
    // Unicode normalization (requires unicode-normalization crate)
    // use unicode_normalization::UnicodeNormalization;
    // let normalized_nfc = text.nfc().collect::<String>();
    // let normalized_nfd = text.nfd().collect::<String>();
}
```

**Best Practices**:
- `String`/`&str` guarantee valid UTF-8
- Use `.chars()` for code point iteration
- Use `unicode-segmentation` crate for grapheme clusters
- Use `unicode-normalization` crate for normalization
- Avoid direct byte indexing; use character boundaries

**Cargo.toml dependencies**:
```toml
[dependencies]
unicode-segmentation = "1.10"
unicode-normalization = "0.1"
```

### Go

**String Type**: `string` is a byte sequence (conventionally UTF-8)

```go
package main

import (
    "fmt"
    "unicode/utf8"
    "golang.org/x/text/unicode/norm"
)

func main() {
    text := "Hello 世界 🌍"
    
    // Length in bytes
    fmt.Println("Byte length:", len(text))
    
    // Count runes (code points)
    fmt.Println("Rune count:", utf8.RuneCountInString(text))
    
    // Iterate over runes
    for i, r := range text {
        fmt.Printf("Position %d: %c (U+%04X)\n", i, r, r)
    }
    
    // Unicode normalization
    composed := "café"
    decomposed := "café"
    
    fmt.Println("Equal:", composed == decomposed)  // false
    
    // Normalize to NFC
    nfcComposed := norm.NFC.String(composed)
    nfcDecomposed := norm.NFC.String(decomposed)
    
    fmt.Println("Normalized equal:", nfcComposed == nfcDecomposed)  // true
}
```

**Best Practices**:
- Strings are conventionally UTF-8 but not enforced
- Use `range` to iterate over runes (code points)
- Use `utf8` package for validation and manipulation
- Use `golang.org/x/text/unicode/norm` for normalization
- Don't assume `len(s)` equals character count

### JavaScript/TypeScript

**String Type**: Strings use UTF-16 encoding units

```javascript
// JavaScript/TypeScript strings are UTF-16
const text = "Hello 世界 🌍";

// Length in UTF-16 code units (not code points!)
console.log(text.length);  // 11 (🌍 counts as 2 units - surrogate pair)

// Iterate over code points (ES6+)
console.log([...text].length);  // 10 (correct code point count)

for (const char of text) {
    console.log(char, char.codePointAt(0)?.toString(16));
}

// Working with emojis and surrogate pairs
const emoji = "🌍";
console.log(emoji.length);  // 2 (surrogate pair)
console.log([...emoji].length);  // 1 (one code point)

// Unicode normalization (modern browsers and Node.js)
const composed = "café";
const decomposed = "café";

console.log(composed === decomposed);  // false

// Normalize to NFC
console.log(composed.normalize('NFC') === decomposed.normalize('NFC'));  // true

// Available normalization forms: 'NFC', 'NFD', 'NFKC', 'NFKD'
const nfc = text.normalize('NFC');   // Composed
const nfd = text.normalize('NFD');   // Decomposed
const nfkc = text.normalize('NFKC'); // Compatibility composed
const nfkd = text.normalize('NFKD'); // Compatibility decomposed
```

**TypeScript Type Definitions**:
```typescript
type NormalizationForm = 'NFC' | 'NFD' | 'NFKC' | 'NFKD';

function normalizeText(text: string, form: NormalizationForm = 'NFC'): string {
    return text.normalize(form);
}

// Safe string comparison
function areStringsEqual(a: string, b: string): boolean {
    return a.normalize('NFC') === b.normalize('NFC');
}
```

**Best Practices**:
- Use spread operator `[...]` or `for...of` for code point iteration
- Be aware of surrogate pairs for emojis and rare characters
- Use `.normalize()` for text comparison
- Use `.codePointAt()` instead of `.charCodeAt()`
- Consider grapheme segmentation libraries for complex text

### C# / .NET

**String Type**: `string` uses UTF-16 encoding units

```csharp
using System;
using System.Text;
using System.Globalization;

class UnicodeExample
{
    static void Main()
    {
        string text = "Hello 世界 🌍";
        
        // Length in UTF-16 code units
        Console.WriteLine($"Length: {text.Length}");  // 11 (🌍 is surrogate pair)
        
        // Iterate over code points
        var enumerator = StringInfo.GetTextElementEnumerator(text);
        int codePointCount = 0;
        while (enumerator.MoveNext())
        {
            codePointCount++;
            string element = enumerator.GetTextElement();
            int codePoint = Char.ConvertToUtf32(element, 0);
            Console.WriteLine($"{element}: U+{codePoint:X4}");
        }
        Console.WriteLine($"Code points: {codePointCount}");
        
        // Unicode normalization
        string composed = "café";
        string decomposed = "café";
        
        Console.WriteLine($"Equal: {composed == decomposed}");  // False
        
        // Normalize to Form C (composed)
        string nfcComposed = composed.Normalize(NormalizationForm.FormC);
        string nfcDecomposed = decomposed.Normalize(NormalizationForm.FormC);
        
        Console.WriteLine($"Normalized equal: {nfcComposed == nfcDecomposed}");  // True
        
        // Available forms: FormC (NFC), FormD (NFD), FormKC (NFKC), FormKD (NFKD)
    }
}
```

**Best Practices**:
- Use `StringInfo` class for proper text element iteration
- Be aware of surrogate pairs
- Use `Char.ConvertToUtf32()` for code points
- Use `.Normalize()` for text comparison
- Consider `System.Globalization.StringInfo` for grapheme clusters

## 🔍 Comparison & Normalization

Unicode allows the same visual character to be represented in multiple ways. For example, "é" can be:
- **Composed** (NFC): Single code point U+00E9 (LATIN SMALL LETTER E WITH ACUTE)
- **Decomposed** (NFD): Two code points U+0065 (e) + U+0301 (combining acute accent)

### Normalization Forms

1. **NFC** (Normalization Form Canonical Composition): Composed characters
2. **NFD** (Normalization Form Canonical Decomposition): Decomposed characters
3. **NFKC** (Compatibility Composition): Composed with compatibility mappings
4. **NFKD** (Compatibility Decomposition): Decomposed with compatibility mappings

### When to Use Normalization

- **Text comparison**: Always normalize before comparing
- **Searching**: Normalize both search query and text
- **Database storage**: Store in consistent form (usually NFC)
- **Hashing**: Normalize before hashing
- **Sorting**: Use locale-aware collation

### Example: Safe String Comparison

```python
# Python
def safe_compare(a, b):
    from unicodedata import normalize
    return normalize('NFC', a) == normalize('NFC', b)
```

```rust
// Rust (with unicode-normalization crate)
use unicode_normalization::UnicodeNormalization;

fn safe_compare(a: &str, b: &str) -> bool {
    a.nfc().eq(b.nfc())
}
```

```go
// Go
import "golang.org/x/text/unicode/norm"

func safeCompare(a, b string) bool {
    return norm.NFC.String(a) == norm.NFC.String(b)
}
```

```javascript
// JavaScript/TypeScript
function safeCompare(a, b) {
    return a.normalize('NFC') === b.normalize('NFC');
}
```

```csharp
// C#
using System.Text;

bool SafeCompare(string a, string b)
{
    return a.Normalize(NormalizationForm.FormC) == 
           b.Normalize(NormalizationForm.FormC);
}
```

## 🗂️ Multi-Language Data Structures

For internationalization (i18n) and localization (l10n), use structured data to manage translations.

### Recommended Structure

```
Dictionary<Locale, Dictionary<TokenKey, LocalizedText>>
```

Or in type alias notation:
```
TokenMap = Map<TokenKey, LocalizedText>
LocalizationStore = Map<Locale, TokenMap>
```

### Implementation Examples

#### TypeScript

```typescript
type TokenKey = string;
type LocalizedText = string;
type Locale = string;  // e.g., 'en-US', 'zh-CN', 'ja-JP'

type TokenMap = Map<TokenKey, LocalizedText>;
type LocalizationStore = Map<Locale, TokenMap>;

// Alternative with objects
interface LocalizationData {
    [locale: string]: {
        [tokenKey: string]: string;
    };
}

// Example usage
const localization: LocalizationData = {
    'en-US': {
        'greeting': 'Hello',
        'farewell': 'Goodbye'
    },
    'zh-CN': {
        'greeting': '你好',
        'farewell': '再见'
    },
    'ja-JP': {
        'greeting': 'こんにちは',
        'farewell': 'さようなら'
    }
};

function getLocalizedText(locale: Locale, key: TokenKey): LocalizedText {
    return localization[locale]?.[key] ?? localization['en-US'][key];
}
```

#### Python

```python
from typing import Dict, NewType

TokenKey = NewType('TokenKey', str)
LocalizedText = NewType('LocalizedText', str)
Locale = NewType('Locale', str)

TokenMap = Dict[TokenKey, LocalizedText]
LocalizationStore = Dict[Locale, TokenMap]

# Example usage
localization: LocalizationStore = {
    'en-US': {
        'greeting': 'Hello',
        'farewell': 'Goodbye'
    },
    'zh-CN': {
        'greeting': '你好',
        'farewell': '再见'
    },
    'ja-JP': {
        'greeting': 'こんにちは',
        'farewell': 'さようなら'
    }
}

def get_localized_text(locale: Locale, key: TokenKey) -> LocalizedText:
    return localization.get(locale, {}).get(key, 
           localization.get('en-US', {}).get(key, ''))
```

#### Rust

```rust
use std::collections::HashMap;

type TokenKey = String;
type LocalizedText = String;
type Locale = String;

type TokenMap = HashMap<TokenKey, LocalizedText>;
type LocalizationStore = HashMap<Locale, TokenMap>;

fn main() {
    let mut localization: LocalizationStore = HashMap::new();
    
    // English
    let mut en_us = HashMap::new();
    en_us.insert("greeting".to_string(), "Hello".to_string());
    en_us.insert("farewell".to_string(), "Goodbye".to_string());
    localization.insert("en-US".to_string(), en_us);
    
    // Chinese
    let mut zh_cn = HashMap::new();
    zh_cn.insert("greeting".to_string(), "你好".to_string());
    zh_cn.insert("farewell".to_string(), "再见".to_string());
    localization.insert("zh-CN".to_string(), zh_cn);
    
    // Japanese
    let mut ja_jp = HashMap::new();
    ja_jp.insert("greeting".to_string(), "こんにちは".to_string());
    ja_jp.insert("farewell".to_string(), "さようなら".to_string());
    localization.insert("ja-JP".to_string(), ja_jp);
}

fn get_localized_text(
    store: &LocalizationStore,
    locale: &str,
    key: &str
) -> Option<&String> {
    store.get(locale)?.get(key)
        .or_else(|| store.get("en-US")?.get(key))
}
```

#### Go

```go
package main

type TokenKey string
type LocalizedText string
type Locale string

type TokenMap map[TokenKey]LocalizedText
type LocalizationStore map[Locale]TokenMap

func main() {
    localization := LocalizationStore{
        "en-US": TokenMap{
            "greeting": "Hello",
            "farewell": "Goodbye",
        },
        "zh-CN": TokenMap{
            "greeting": "你好",
            "farewell": "再见",
        },
        "ja-JP": TokenMap{
            "greeting": "こんにちは",
            "farewell": "さようなら",
        },
    }
    
    text := getLocalizedText(localization, "zh-CN", "greeting")
    println(text)  // 你好
}

func getLocalizedText(
    store LocalizationStore,
    locale Locale,
    key TokenKey,
) LocalizedText {
    if tokenMap, ok := store[locale]; ok {
        if text, ok := tokenMap[key]; ok {
            return text
        }
    }
    // Fallback to en-US
    if tokenMap, ok := store["en-US"]; ok {
        if text, ok := tokenMap[key]; ok {
            return text
        }
    }
    return ""
}
```

#### C#

```csharp
using System;
using System.Collections.Generic;

public class Localization
{
    public record TokenKey(string Value);
    public record LocalizedText(string Value);
    public record Locale(string Value);
    
    public class TokenMap : Dictionary<string, string> { }
    public class LocalizationStore : Dictionary<string, TokenMap> { }
    
    public static void Main()
    {
        var localization = new LocalizationStore
        {
            ["en-US"] = new TokenMap
            {
                ["greeting"] = "Hello",
                ["farewell"] = "Goodbye"
            },
            ["zh-CN"] = new TokenMap
            {
                ["greeting"] = "你好",
                ["farewell"] = "再见"
            },
            ["ja-JP"] = new TokenMap
            {
                ["greeting"] = "こんにちは",
                ["farewell"] = "さようなら"
            }
        };
        
        string text = GetLocalizedText(localization, "zh-CN", "greeting");
        Console.WriteLine(text);  // 你好
    }
    
    public static string GetLocalizedText(
        LocalizationStore store,
        string locale,
        string key)
    {
        if (store.TryGetValue(locale, out var tokenMap) &&
            tokenMap.TryGetValue(key, out var text))
        {
            return text;
        }
        
        // Fallback to en-US
        if (store.TryGetValue("en-US", out tokenMap) &&
            tokenMap.TryGetValue(key, out text))
        {
            return text;
        }
        
        return string.Empty;
    }
}
```

## 🎯 Best Practices Summary

1. **Always use UTF-8** for source files and data exchange
2. **Normalize** strings before comparison, searching, or hashing
3. **Be aware** of the difference between:
   - Bytes vs. code points vs. grapheme clusters
   - Code units (UTF-16) vs. code points (Unicode scalar values)
4. **Use language-specific APIs**:
   - Python: `unicodedata.normalize()`
   - Rust: `unicode-normalization` crate
   - Go: `golang.org/x/text/unicode/norm`
   - JavaScript/TypeScript: `String.prototype.normalize()`
   - C#: `String.Normalize()`
5. **Structure localization data** consistently:
   - `Locale → TokenKey → LocalizedText`
   - Consider fallback mechanisms (e.g., to English)
6. **Test with diverse input**:
   - Emojis: 🌍 👨‍👩‍👧‍👦
   - CJK characters: 你好世界
   - RTL scripts: مرحبا العالم
   - Combining characters: café vs café
   - Surrogate pairs and grapheme clusters

## 📚 Additional Resources

- [Unicode Standard](https://unicode.org/standard/standard.html)
- [Unicode Normalization Forms](https://unicode.org/reports/tr15/)
- [UTF-8 Everywhere Manifesto](http://utf8everywhere.org/)
- [ICU - International Components for Unicode](https://icu.unicode.org/)
- [The Absolute Minimum Every Software Developer Must Know About Unicode](https://www.joelonsoftware.com/2003/10/08/the-absolute-minimum-every-software-developer-absolutely-positively-must-know-about-unicode-and-character-sets-no-excuses/)

## 🔧 Tools & Libraries

### Python
- Built-in: `unicodedata`
- External: `pyicu` (Python bindings for ICU)

### Rust
- `unicode-segmentation`: Grapheme cluster segmentation
- `unicode-normalization`: Unicode normalization
- `icu`: Rust bindings for ICU

### Go
- `golang.org/x/text/unicode/norm`: Normalization
- `golang.org/x/text/language`: Language tags
- `golang.org/x/text/collate`: Locale-sensitive string comparison

### JavaScript/TypeScript
- Built-in: `String.prototype.normalize()`
- External: `grapheme-splitter`, `Intl` APIs

### C# / .NET
- Built-in: `System.Text.NormalizationForm`
- `System.Globalization.StringInfo`: Grapheme iteration
- External: `Icu.Net` (ICU bindings)

---

*This guide is maintained as part of the TypeAlias project to ensure consistent Unicode handling across different programming languages.*
