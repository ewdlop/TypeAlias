// English Type Aliases for TypeScript
// These are type aliases that provide English names for common TypeScript types

// Basic Types
export type Text = string;
export type WholeNumber = number;
export type Integer = number;
export type FloatingPoint = number;
export type Decimal = number;
export type TrueOrFalse = boolean;
export type Character = string;
export type ByteArray = Uint8Array;
export type DateTime = Date;

// Function Types
export type VoidFunction = () => void;
export type AsyncOperation<T = void> = Promise<T>;
export type Callback<T = void> = (result: T) => void;
export type ErrorCallback = (error: Error) => void;
export type Predicate<T> = (item: T) => boolean;
export type Mapper<TInput, TOutput> = (input: TInput) => TOutput;
export type Comparator<T> = (a: T, b: T) => number;

// Collection Types
export type List<T> = T[];
export type ReadonlyList<T> = readonly T[];
export type Dictionary<T> = Record<string, T>;
export type StringMap<T> = Map<string, T>;
export type NumberMap<T> = Map<number, T>;
export type UniqueSet<T> = Set<T>;
export type Tuple2<T1, T2> = [T1, T2];
export type Tuple3<T1, T2, T3> = [T1, T2, T3];
export type Tuple4<T1, T2, T3, T4> = [T1, T2, T3, T4];

// Utility Types
export type Optional<T> = T | null | undefined;
export type Nullable<T> = T | null;
export type Maybe<T> = T | undefined;
export type NonNullable<T> = Exclude<T, null | undefined>;
export type AnyObject = Record<string, any>;
export type EmptyObject = Record<string, never>;
export type UnknownObject = Record<string, unknown>;

// DOM Types (for browser environments)
export type HtmlElement = HTMLElement;
export type DomElement = Element;
export type DomNode = Node;
export type EventListener<T extends Event = Event> = (event: T) => void;
export type MouseEventListener = EventListener<MouseEvent>;
export type KeyboardEventListener = EventListener<KeyboardEvent>;

// HTTP/Network Types
export type HttpHeaders = Record<string, string>;
export type JsonObject = Record<string, any>;
export type JsonArray = any[];
export type JsonValue = string | number | boolean | null | JsonObject | JsonArray;

// Time Types
export type Milliseconds = number;
export type Seconds = number;
export type Minutes = number;
export type Hours = number;
export type Timestamp = number;

// File Types
export type FileName = string;
export type FilePath = string;
export type FileExtension = string;
export type MimeType = string;

// ID Types
export type UniqueId = string;
export type NumericId = number;
export type Uuid = string;

// Result Types
export type Result<T, E = Error> = { success: true; value: T } | { success: false; error: E };
export type Either<L, R> = { left: L } | { right: R };

// Class Constructor Type
export type Constructor<T = any> = new (...args: any[]) => T;

// Type Guards
export type TypeGuard<T> = (value: unknown) => value is T;

// Async Types
export type AsyncFunction<T = void> = () => Promise<T>;
export type AsyncCallback<T = void> = (result: T) => Promise<void>;
