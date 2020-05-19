// Сообщаем программе, что скрипт должен пересчитываться раз в секунду
OverrideScriptTimeout(1000);

// Определяем все составляющие даты и времени
var dateTime  = new Date();
var dayOfWeek = dateTime.getDay();
var day       = dateTime.getDate();
var month     = dateTime.getMonth() + 1;
var year      = dateTime.getFullYear();
var hour      = dateTime.getHours();
var minute    = dateTime.getMinutes();
var second    = dateTime.getSeconds();

// Записываем значения в измерения с адресами 24:1:1..7
SetTmAnalog(24, 1, 1, year);
SetTmAnalog(24, 1, 2, month);
SetTmAnalog(24, 1, 3, day);
SetTmAnalog(24, 1, 4, dayOfWeek);
SetTmAnalog(24, 1, 5, hour);
SetTmAnalog(24, 1, 6, minute);
SetTmAnalog(24, 1, 7, second);