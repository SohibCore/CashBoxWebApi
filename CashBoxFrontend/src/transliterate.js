// 1. Xaritalar
const latinToCyrillicMap = {
  "yo": "ё", "sh": "ш", "ch": "ч",
  "ng": "нг", "gh": "ғ", "ts": "ц",
  "o'": "ў", "g'": "ғ",
  "a": "а", "b": "б", "d": "д", "e": "е",
  "f": "ф", "g": "г", "h": "ҳ", "i": "и",
  "j": "ж", "k": "к", "l": "л", "m": "м",
  "n": "н", "o": "о", "p": "п", "q": "қ",
  "r": "р", "s": "с", "t": "т", "u": "у",
  "v": "в", "x": "х", "y": "й", "z": "з",
};

const cyrillicToLatinMap = {
  "ё": "yo", "ш": "sh", "ч": "ch", "ц": "ts",
  "ғ": "g'", "ҳ": "h", "қ": "q", "ў": "o'",
  "а": "a", "б": "b", "д": "d", "е": "e",
  "ф": "f", "г": "g", "и": "i", "ж": "j",
  "к": "k", "л": "l", "м": "m", "н": "n",
  "о": "o", "п": "p", "р": "r", "с": "s",
  "т": "t", "у": "u", "в": "v", "х": "x",
  "й": "y", "з": "z", "ъ": ""
};

// 2. Til aniqlash
export function detectScript(text) {
  const cyr = (text.match(/[а-яёА-ЯЁқғҳўҚҒҲЎ]/g) || []).length;
  const lat = (text.match(/[a-zA-Z]/g) || []).length;
  if (cyr > lat) return "cyrillic";
  if (lat > cyr) return "latin";
  return "unknown";
}

// 3. Lotin → Kirill
function toLatin(text) {
  let result = "";
  for (const char of text) {
    result += cyrillicToLatinMap[char] ?? char;
  }
  return result;
}

// 4. Kirill → Lotin
function toCyrillic(text) {
  let result = text;
  const twoChar = ["yo","sh","ch","ng","gh","ts","o'","g'"];
  for (const key of twoChar) {
    result = result.replaceAll(key, latinToCyrillicMap[key]);
  }
  for (const [key, val] of Object.entries(latinToCyrillicMap)) {
    if (key.length === 1) result = result.replaceAll(key, val);
  }
  return result;
}

// 5. Asosiy funksiya
export function transliterate(text) {
  const script = detectScript(text);
  if (script === "latin")    return toCyrillic(text);
  if (script === "cyrillic") return toLatin(text);
  return text;
}