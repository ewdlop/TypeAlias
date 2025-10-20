// Alias de Tipos en Español para TypeScript
// Estos son alias de tipos que proporcionan nombres en español para tipos comunes de TypeScript

// Tipos Básicos
export type Texto = string;
export type NúmeroEntero = number;
export type Entero = number;
export type Número = number;
export type PuntoFlotante = number;
export type Decimal = number;
export type Booleano = boolean;
export type VerdaderoOFalso = boolean;
export type Carácter = string;
export type ArregloByte = Uint8Array;
export type FechaHora = Date;

// Tipos de Función
export type FunciónVacía = () => void;
export type OperaciónAsíncrona<T = void> = Promise<T>;
export type LlamadaRetorno<T = void> = (resultado: T) => void;
export type LlamadaError = (error: Error) => void;
export type Predicado<T> = (elemento: T) => boolean;
export type Mapeador<TEntrada, TSalida> = (entrada: TEntrada) => TSalida;
export type Comparador<T> = (a: T, b: T) => number;

// Tipos de Colección
export type Lista<T> = T[];
export type ListaSoloLectura<T> = readonly T[];
export type Diccionario<T> = Record<string, T>;
export type MapaTexto<T> = Map<string, T>;
export type MapaNúmero<T> = Map<number, T>;
export type ConjuntoÚnico<T> = Set<T>;
export type Tupla2<T1, T2> = [T1, T2];
export type Tupla3<T1, T2, T3> = [T1, T2, T3];
export type Tupla4<T1, T2, T3, T4> = [T1, T2, T3, T4];

// Tipos de Utilidad
export type Opcional<T> = T | null | undefined;
export type Nulable<T> = T | null;
export type Quizás<T> = T | undefined;
export type NoNulable<T> = Exclude<T, null | undefined>;
export type ObjetoCualquiera = Record<string, any>;
export type ObjetoVacío = Record<string, never>;
export type ObjetoDesconocido = Record<string, unknown>;

// Tipos DOM (para entornos de navegador)
export type ElementoHtml = HTMLElement;
export type ElementoDom = Element;
export type NodoDom = Node;
export type EscuchadorEvento<T extends Event = Event> = (evento: T) => void;
export type EscuchadorEventoRatón = EscuchadorEvento<MouseEvent>;
export type EscuchadorEventoTeclado = EscuchadorEvento<KeyboardEvent>;

// Tipos HTTP/Red
export type EncabezadosHttp = Record<string, string>;
export type ObjetoJson = Record<string, any>;
export type ArregloJson = any[];
export type ValorJson = string | number | boolean | null | ObjetoJson | ArregloJson;

// Tipos de Tiempo
export type Milisegundos = number;
export type Segundos = number;
export type Minutos = number;
export type Horas = number;
export type MarcaTiempo = number;

// Tipos de Archivo
export type NombreArchivo = string;
export type RutaArchivo = string;
export type ExtensiónArchivo = string;
export type TipoMime = string;

// Tipos de ID
export type IdÚnico = string;
export type IdNumérico = number;
export type Uuid = string;

// Tipos de Resultado
export type Resultado<T, E = Error> = { éxito: true; valor: T } | { éxito: false; error: E };
export type UnoUOtro<I, D> = { izquierda: I } | { derecha: D };

// Tipo Constructor de Clase
export type Constructor<T = any> = new (...args: any[]) => T;

// Guardias de Tipo
export type GuardiaTipo<T> = (valor: unknown) => valor is T;

// Tipos Asíncronos
export type FunciónAsíncrona<T = void> = () => Promise<T>;
export type LlamadaRetornoAsíncrona<T = void> = (resultado: T) => Promise<void>;
