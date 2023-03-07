// Получаем значение из предыдущего выполнения
var previousValue = ReadFromStorage('prev');

// Получаем текущее значение
var value = GetTmAnalog(20, 1, 1);

// Если предыдущее значение есть, и относительное отклонение больше 1%, то взводим сигнал АПС
if (previousValue !== null) {
	var deviation = Math.abs(value - previousValue) / previousValue;
	if (deviation > 0.01) {
		SetTmStatus(24, 1, 1, 1);
	}
}

// Сохраняем значение для следующего выполнения
WriteToStorage('prev', value);