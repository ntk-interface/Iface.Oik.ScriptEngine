// Массив адресов сигналов, которым надо установить состояние ВКЛ - 0:1:1, 0:1:2 и 0:1:4
var tsOn = [
  [0, 1, 1],
  [0, 1, 2],
  [0, 1, 4],  
];

// Массив адресов сигналов, которым надо установить состояние ОТКЛ - 0:1:3 и 0:1:5
var tsOff = [
  [0, 1, 3],
  [0, 1, 5],  
];

// В циклах выставляем требуемые состояния
for (var i = 0; i < tsOn.length; i++) {
  SetTmStatus(tsOn[i][0], tsOn[i][1], tsOn[i][2], 1);
}
for (var j = 0; j < tsOff.length; j++) {
  SetTmStatus(tsOff[j][0], tsOff[j][1], tsOff[j][2], 0);
}

// Сообщаем программе, что скрипт должен пересчитываться раз в минуту
OverrideScriptTimeout(60000);