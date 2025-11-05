using cadena = System.String;
using entero32 = System.Int32;
using entero64 = System.Int64;
using flotante = System.Single;
using doble = System.Double;
using fechaHora = System.DateTime;
using booleano = System.Boolean;
using carácter = System.Char;

#if false
using byte = System.Byte;
#endif

using arregloByte = byte[];
using flujoMemoria = System.IO.MemoryStream;
using lectorBinario = System.IO.BinaryReader;
using escritorBinario = System.IO.BinaryWriter;
using tarea = System.Threading.Tasks.Task;
using excepción = System.Exception;
using flujoArchivo = System.IO.FileStream;
using flujoMem = System.IO.MemoryStream;
using solicitudHttp = System.Net.Http.HttpRequestMessage;
using respuestaHttp = System.Net.Http.HttpResponseMessage;
using clienteHttp = System.Net.Http.HttpClient;
using contenidoHttp = System.Net.Http.HttpContent;
using contenidoCadena = System.Net.Http.StringContent;
using contenidoMultiparte = System.Net.Http.MultipartFormDataContent;
using contenidoFormulario = System.Net.Http.FormUrlEncodedContent;
using métodoHttp = System.Net.Http.HttpMethod;
using códigoEstado = System.Net.HttpStatusCode;
using encabezadosHttp = System.Net.Http.Headers.HttpHeaders;
using valorEncabezadoTipoMedio = System.Net.Http.Headers.MediaTypeHeaderValue;
using encabezadosContenido = System.Net.Http.Headers.HttpContentHeaders;
using encabezadosSolicitud = System.Net.Http.Headers.HttpRequestHeaders;
using encabezadosRespuesta = System.Net.Http.Headers.HttpResponseHeaders;
using contenidoSolicitud = System.Net.Http.HttpRequestMessage;
using contenidoRespuesta = System.Net.Http.HttpResponseMessage;
using encabezadosSolicitudContenido = System.Net.Http.Headers.HttpRequestHeaders;

#if false

using valorEncabezadoSolicitud = System.Net.Http.Headers.HttpRequestHeaderValue;
using valorEncabezadoRespuesta = System.Net.Http.Headers.HttpResponseHeaderValue;
using nombreEncabezadoSolicitud = System.Net.Http.Headers.HttpRequestHeaderName;
using nombreEncabezadoRespuesta = System.Net.Http.Headers.HttpResponseHeaderName;

#endif

#if false
using pila = System.Collections.Generic.Stack;
using cola = System.Collections.Generic.Queue;
using conjunto = System.Collections.Generic.HashSet;
using diccionarioOrdenado = System.Collections.Generic.SortedDictionary;
using conjuntoOrdenado = System.Collections.Generic.SortedSet;
using listaEnlazada = System.Collections.Generic.LinkedList;
using colaPrioridad = System.Collections.Generic.PriorityQueue;
#endif

using hilo = System.Threading.Thread;
using mutex = System.Threading.Mutex;
using semáforo = System.Threading.Semaphore;
using temporizador = System.Timers.Timer;

using expresiónRegular = System.Text.RegularExpressions.Regex;
using aleatorio = System.Random;
using matemática = System.Math;
using ambiente = System.Environment;
using consola = System.Console;

using archivo = System.IO.File;
using directorio = System.IO.Directory;
using ruta = System.IO.Path;

using proceso = System.Diagnostics.Process;
using cronómetro = System.Diagnostics.Stopwatch;

using tipoNulable = System.Nullable;
using tupla = System.Tuple;
using tuplaPorValor = System.ValueTuple;
using enumeración = System.Enum;
using manejadorEvento = System.EventHandler;
using delegado = System.Delegate;
using acción = System.Action;
using comparable = System.IComparable;
using enumerable = System.Collections.IEnumerable;
using desechable = System.IDisposable;
using bloqueo = System.Threading.Monitor;
using colecciónConcurrente = System.Collections.Concurrent;
using factoriaTarea = System.Threading.Tasks.TaskFactory;
using opcionesParalelo = System.Threading.Tasks.ParallelOptions;
using grupoHilos = System.Threading.ThreadPool;
using operaciónAsíncrona = System.Threading.Tasks.Task;
using tokenCancelación = System.Threading.CancellationToken;
using fuenteTokenCancelación = System.Threading.CancellationTokenSource;
using reflexión = System.Reflection;
using tipo = System.Type;
using propiedad = System.Reflection.PropertyInfo;
using método = System.Reflection.MethodInfo;
using atributo = System.Attribute;
using compresión = System.IO.Compression;
using criptografía = System.Security.Cryptography;
using codificación = System.Text.Encoding;
using configuración = System.Configuration;
using registro = System.Diagnostics.Debug;
namespace AliasDirective.spa
{
    public static class ExtensiónDeCadena
    {
        public static cadena? MayúsculaInicial(this cadena cadena)
        {
            if (!string.IsNullOrWhiteSpace(cadena))
            {
                return cadena;
            }
            if (cadena.Length == 1)
            {
                return cadena.ToUpper();
            }
            return char.ToUpper(cadena[0]) + cadena.Substring(1);
        }
    }
}