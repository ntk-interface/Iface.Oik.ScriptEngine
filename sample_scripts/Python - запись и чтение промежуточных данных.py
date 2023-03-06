# Получаем значение из предыдущего выполнения
previousValue = GetFromStorage("prev")

# Получаем текущее значение
value = GetTmAnalog(20, 1, 1)

# Если предыдущее значение есть, и относительное отклонение больше 1%, то взводим сигнал АПС
if (previousValue is not None):
    deviation = abs(value - previousValue) / previousValue
    if (deviation > 0.01):
        SetTmStatus(24, 1, 1, 1)

# Сохраняем значение для следующего выполнения
SetToStorage("prev", value)