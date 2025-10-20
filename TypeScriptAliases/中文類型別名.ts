// 繁體中文 TypeScript 類型別名
// 這些是類型別名，為 TypeScript 常用類型提供中文名稱

// 基本類型
export type 文本 = string;
export type 字符串 = string;
export type 整數 = number;
export type 數字 = number;
export type 浮點數 = number;
export type 小數 = number;
export type 布林值 = boolean;
export type 真假值 = boolean;
export type 字元 = string;
export type 位元組陣列 = Uint8Array;
export type 日期時間 = Date;

// 函數類型
export type 空函數 = () => void;
export type 非同步操作<T = void> = Promise<T>;
export type 回調函數<T = void> = (結果: T) => void;
export type 錯誤回調 = (錯誤: Error) => void;
export type 謂詞<T> = (項目: T) => boolean;
export type 映射器<T輸入, T輸出> = (輸入: T輸入) => T輸出;
export type 比較器<T> = (a: T, b: T) => number;

// 集合類型
export type 列表<T> = T[];
export type 唯讀列表<T> = readonly T[];
export type 字典<T> = Record<string, T>;
export type 字串映射<T> = Map<string, T>;
export type 數字映射<T> = Map<number, T>;
export type 唯一集合<T> = Set<T>;
export type 二元組<T1, T2> = [T1, T2];
export type 三元組<T1, T2, T3> = [T1, T2, T3];
export type 四元組<T1, T2, T3, T4> = [T1, T2, T3, T4];

// 工具類型
export type 可選<T> = T | null | undefined;
export type 可空<T> = T | null;
export type 也許<T> = T | undefined;
export type 非空<T> = Exclude<T, null | undefined>;
export type 任意物件 = Record<string, any>;
export type 空物件 = Record<string, never>;
export type 未知物件 = Record<string, unknown>;

// DOM 類型（瀏覽器環境）
export type Html元素 = HTMLElement;
export type Dom元素 = Element;
export type Dom節點 = Node;
export type 事件監聽器<T extends Event = Event> = (事件: T) => void;
export type 滑鼠事件監聽器 = 事件監聽器<MouseEvent>;
export type 鍵盤事件監聽器 = 事件監聽器<KeyboardEvent>;

// HTTP/網路類型
export type Http標頭 = Record<string, string>;
export type Json物件 = Record<string, any>;
export type Json陣列 = any[];
export type Json值 = string | number | boolean | null | Json物件 | Json陣列;

// 時間類型
export type 毫秒 = number;
export type 秒 = number;
export type 分鐘 = number;
export type 小時 = number;
export type 時間戳記 = number;

// 檔案類型
export type 檔案名稱 = string;
export type 檔案路徑 = string;
export type 檔案副檔名 = string;
export type Mime類型 = string;

// ID 類型
export type 唯一識別碼 = string;
export type 數字識別碼 = number;
export type 通用唯一識別碼 = string;

// 結果類型
export type 結果<T, E = Error> = { 成功: true; 值: T } | { 成功: false; 錯誤: E };
export type 二選一<L, R> = { 左: L } | { 右: R };

// 類別建構子類型
export type 建構子<T = any> = new (...args: any[]) => T;

// 類型守衛
export type 類型守衛<T> = (值: unknown) => 值 is T;

// 非同步類型
export type 非同步函數<T = void> = () => Promise<T>;
export type 非同步回調<T = void> = (結果: T) => Promise<void>;
