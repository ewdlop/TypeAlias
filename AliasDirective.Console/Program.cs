using AliasDirective.Shared;
using Azure;
using Azure.AI.Translation.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using NuGet.Packaging;
using System.Reflection;

namespace AliasDirective.Console;

public class Program
{
    protected static IReadOnlyDictionary<string, TransliterationLanguage> _transliterationLanguages;
    protected static string[] _targetLanguages = [];
    protected static TextTranslationClient? client = null;

    public static async Task Main(string[] args)
    {
        System.Console.OutputEncoding = System.Text.Encoding.UTF8;
        {
            //https://learn.microsoft.com/en-us/nuget/api/catalog-resource
        }

        System.Console.WriteLine("\n=== Dependency Context ===\n");
        Task.Run(DependencyUtilities.PrintDependencyContext).Wait();

        System.Console.WriteLine("Setting up Text Translation Client...");


        string key = "<Text Translator Custom key>";
        AzureKeyCredential credential = new AzureKeyCredential(key);
        string endpoint = "<Text Translator Custom Endpoint>";
        client = new TextTranslationClient(credential, new Uri(endpoint));

        Azure.Response<GetSupportedLanguagesResult> result = await client.GetSupportedLanguagesAsync();


        ////https://en.wikipedia.org/wiki/IETF_language_tag
        _targetLanguages = [.. result.Value.Translation.Keys];
        string[] _targetNativeNames = [.. result.Value.Translation.Values.Select(v => v.NativeName)];
        _transliterationLanguages = result.Value.Transliteration;
        IEnumerable<(string Key, string NativeName)>? values =
            result.Value.Transliteration.SelectMany(kVP => kVP.Value.Scripts,
            (kvP,tLS)=>(kvP.Key, tLS.NativeName));
        string[] _languagesNativeNames = [.. _transliterationLanguages.Keys];
        string[] _transliterationNativeScriptNames = [.. values.Select(v=> v.NativeName)];
        string join = string.Join(", ", values.Select(v => v.ToString())); ;

        //Is Microsoft saying that no literal translation to any language is supported from English?

        //choclate -> 巧克力 (zh-Hant)?
        //choclate -> besuboru  (ja)?

        //(ar, العربية), (ar, اللاتينية), (as, বাংলা), (as, লেটিন), (be, Кірыліца), (be, Лацініца), (bg, кирилица), (bg, латиница), (bn, বাংলা), (bn, ল্যাটিন), (brx, Devanagari), (brx, Latin), (el, ελληνικό), (el, λατινικό), (fa, عربی), (fa, لاتین), (gom, Devanagari), (gom, Latin), (gu, ગુજરાતી), (gu, લેટિન), (he, עברי), (he, לטיני), (hi, देवनागरी), (hi, लैटिन), (ja, 日本語の文字), (ja, ラテン文字), (kk, кирилл жазуы), (kk, латын жазуы), (kn, ಕನ್ನಡ), (kn, ಲ್ಯಾಟಿನ್), (ko, 한국 문자), (ko, 로마자), (ks, Arabic), (ks, Latin), (ky, кирилл), (ky, латын), (mai, Devanagari), (mai, Latin), (mk, кирилско писмо), (mk, латинично писмо), (ml, മലയാളം), (ml, ലാറ്റിൻ), (mn-Cyrl, Cyrillic), (mni, Meetei Mayek), (mni, Latin), (mr, देवनागरी), (mr, लॅटिन), (ne, देवानागरी), (ne, ल्याटिन), (or, ଓଡ଼ିଆ), (or, ଲାଟିନ୍), (pa, ਗੁਰਮੁਖੀ), (pa, ਲਾਤੀਨੀ), (ru, кириллица), (ru, латиница), (sa, Devanagari), (sa, Latin), (sd, عربي), (sd, لاطيني), (si, සිංහල), (si, ලතින්), (sr-Cyrl, ћирилица), (sr-Latn, latinica), (ta, தமிழ்), (ta, லத்தின்), (te, తెలుగు), (te, లాటిన్), (tg, Кириллӣ), (tg, Лотинӣ), (th, ไทย), (tt, кирилл), (tt, латин), (uk, кирилиця), (uk, латиниця), (ur, عربی), (ur, لاطینی), (zh-Hans, 简体), (zh-Hans, 拉丁文), (zh-Hant, 繁體), (zh-Hant, 拉丁文)

        //english to traditional chinese transliteration
        //Integer32 -> 英特爾32!!!
        //Tea is originally from China...
        //Chai is originally from India...
        //pronunciation differences?

        //
        //Google's "Chai": "The Mandarin pronunciation of the character 茶 is chá."
        //"This pronunciation traveled westward along the Silk Road, evolving into pronunciations like "chai" in Persian, Russian, and other languages"


#if false
        await foreach (var result2 in TransliterateAsync(client, _transliterationLanguages["en"],
            _transliterationLanguages["zh-Hant"].Scripts.FirstOrDefault(), _transliterationLanguages["en"].Scripts.FirstOrDefault(), ["Int32"]))
        {
            System.Console.WriteLine($"Transliteration Result: {result2.Text}");
        }
#endif



        System.Console.WriteLine("Supported Languages");
        System.Console.WriteLine(join);
        System.Console.WriteLine("Supported Target Languages:");
        System.Console.WriteLine(string.Join(", ", _targetNativeNames));
        System.Console.WriteLine();
        System.Console.ReadLine();

        System.Console.WriteLine("Translating Type Names...");

        var packageArchiveReaderBag = await PackageUtilities.DownloadPackagesAndAsync([("Newtonsoft.Json", "13.0.3")]);

        while(packageArchiveReaderBag.TryTake(out PackageArchiveReader? packageArchiveReader))
        {
            if (packageArchiveReader == null) continue;
            Assembly loadedAssembly = await PackageUtilities.ExtraTransformLoadAssembly(packageArchiveReader);
            ILookup<string, string?> typeLookUp = TypeTraversalUtilities.PrintPosteriorTraversal(loadedAssembly)
                .DistinctBy(type => $"{type.FullName?.Split(type.Name).First()}{type.Name}")
                .ToLookup(type => type.Name, type => type.FullName);
            
            foreach (var typeGroup in typeLookUp)
            {
                string endonym = typeGroup.Key;
                if (typeGroup.Count() > 1)
                {
                    System.Console.WriteLine($"Warning: Duplicate type name found: {endonym}");
                }
                System.Console.WriteLine($"Translating type name: {endonym}");
                await foreach (TranslatedTextItem translatedTextItem in TranslateAsync(client, _targetLanguages, [typeGroup.Key], sourceLanguage: "en"))
                {
                    foreach (TranslationText translationText in translatedTextItem.Translations)
                    {
                        foreach (string? typeFullName in typeLookUp[endonym])
                        {
                            string aliasDirective = $"using {endonym} = {typeFullName};";
                            System.Console.WriteLine(translationText?.Transliteration?.Text);
                            System.Console.WriteLine(aliasDirective);
                            string? exonym = translationText?.Text;
                            string exonymAliasDirective = $"using {exonym} = {typeFullName};";
                            System.Console.WriteLine(exonymAliasDirective);
                        }
                        //check compilation
                    }
                }
            }

            await foreach (TranslatedTextItem translatedTextItem in TranslateAsync(client, _targetLanguages, typeLookUp.Select(tlU => tlU.Key), sourceLanguage: "en"))
            {
                foreach (TranslationText translationText in translatedTextItem.Translations)
                {
                    foreach(string? typeName in typeLookUp[translatedTextItem.SourceText.Text])
                    {
                        string alias = $"using {translationText.Text} = {typeName};";
                        System.Console.WriteLine(alias);
                    }
                    //check compilation
                }
            }
        }
    }

    static async IAsyncEnumerable<TranslatedTextItem> TranslateAsync(TextTranslationClient client, IEnumerable<string> targetLanguages, IEnumerable<string> texts, string? sourceLanguage = null, CancellationToken cancellationToken = default)
    {
        Azure.Response<IReadOnlyList<TranslatedTextItem>>? result = await client.TranslateAsync(targetLanguages, texts, Guid.NewGuid(), sourceLanguage: sourceLanguage, cancellationToken: cancellationToken);
        foreach (TranslatedTextItem item in result.Value)
        {
            yield return item;
        }
    }

    static async IAsyncEnumerable<TransliteratedText> TransliterateAsync(TextTranslationClient client, TransliterationLanguage fromTransliterationLanguage, TransliteratedText fromTransliterationText, TransliteratedText toTransliterationText, IEnumerable<string> content, string? sourceLanguage = null, CancellationToken cancellationToken = default)
    {
        Azure.Response<IReadOnlyList<TransliteratedText>>? result = await client.TransliterateAsync(fromTransliterationLanguage.Name, fromTransliterationText.Script, toTransliterationText.Script, content, Guid.NewGuid(), cancellationToken);
        foreach (TransliteratedText item in result.Value)
        {
            yield return item;
        }
    }

    static async IAsyncEnumerable<TransliteratedText> TransliterateAsync(TextTranslationClient client, TransliterationLanguage fromTransliterationLanguage, TransliterableScript fromTransliterationScript, TransliterableScript toTransliterationScript, IEnumerable<string> content, string? sourceLanguage = null, CancellationToken cancellationToken = default)
    {
        Azure.Response<IReadOnlyList<TransliteratedText>>? result = await client.TransliterateAsync(fromTransliterationLanguage.Name, fromTransliterationScript.Code, toTransliterationScript.Code, content, Guid.NewGuid(), cancellationToken);
        foreach (TransliteratedText item in result.Value)
        {
            yield return item;
        }
    }

    /// <summary>
    /// Compiles an alias directive to check if it's syntactically valid
    /// </summary>
    static bool ValidateAliasDirective(string aliasDirective)
    {
        try
        {
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(aliasDirective);
            IEnumerable<Diagnostic> diagnostics = syntaxTree.GetDiagnostics();
            return !diagnostics.Any(d => d.Severity == DiagnosticSeverity.Error);
        }
        catch
        {
            return false;
        }
    }

    static EmitResult Compile(string code, MetadataReference[] metadataReference, string outPutPath)
    {
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);
        string assemblyName = Path.GetRandomFileName();
        MetadataReference[] references = metadataReference;

        CSharpCompilation compilation = CSharpCompilation.Create(
            assemblyName,
            syntaxTrees: new[] { syntaxTree },
            references: references,
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        return compilation.Emit(outPutPath);
    }

    static IList<MetadataReference> GetMetadataReferences(Assembly assembly, bool load = false)
    {
        IList<MetadataReference> references = new List<MetadataReference>()
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(assembly.Location)
        };

        foreach (AssemblyName referencedAssembly in assembly.GetReferencedAssemblies())
        {
            try
            {
                if (load)
                {
                    Assembly loadedAssembly = Assembly.Load(referencedAssembly);
                    references.Add(MetadataReference.CreateFromFile(loadedAssembly.Location));
                }
            }
            catch
            {
                // Skip assemblies that can't be loaded
            }
        }

        return references;
    }
}