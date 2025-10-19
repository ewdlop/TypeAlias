# TypeAlias

A repository for documenting type alias best practices and Unicode handling across different programming languages.

## 📚 Documentation

- **[Unicode Handling Guide](UNICODE_HANDLING_GUIDE.md)** - Comprehensive guide for Unicode handling in Python, Rust, Go, JavaScript/TypeScript, and C#

## 🌍 Unicode Best Practices

This repository provides guidance on:

- **Source Code Encoding**: UTF-8 as project standard
- **String & Unicode**: Language-specific handling (Python, Rust, Go, JS/TS, C#)
- **Normalization**: Unicode equivalence comparison techniques
- **Internationalization**: Multi-language data structure patterns

### Quick Overview

Different languages handle Unicode differently:

| Language | String Type | Encoding | Notes |
|----------|------------|----------|-------|
| Python 3 | `str` | Unicode | Unicode by default |
| Rust | `String`/`&str` | UTF-8 | Guaranteed valid UTF-8 |
| Go | `string` | Byte sequence | Conventionally UTF-8 |
| JavaScript/TypeScript | `string` | UTF-16 | Beware of surrogate pairs |
| C# / .NET | `string` | UTF-16 | Beware of surrogate pairs |

### Key Considerations

1. **Encoding**: Always use UTF-8 for source files and data exchange
2. **Normalization**: Use NFC/NFD normalization for string comparison
3. **Code Points vs Bytes**: Understand the difference between byte length and character count
4. **Emojis & Combining Characters**: Be aware of grapheme clusters and surrogate pairs
5. **Localization Structure**: Use `Dictionary<Locale, TokenMap>` pattern for multi-language support

For detailed information, examples, and best practices, see the [Unicode Handling Guide](UNICODE_HANDLING_GUIDE.md).

## 🚀 Getting Started

The documentation in this repository is applicable to any project that needs to handle Unicode text properly. Choose your language and follow the relevant section in the guide.

## 📖 Contributing

Contributions are welcome! Please ensure all documentation files are saved in UTF-8 encoding.