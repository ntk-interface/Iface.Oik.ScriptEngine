# Получаем значения сигнала и измерения с адресом 100:1:1
ts = GetTmStatus(100, 1, 1)
ti = GetTmAnalog(100, 1, 1)

# Записываем эти значения в сигналы и измерения с адресами 100:1:1..9 (в цикле)
for point in range(2, 9):
    SetTmStatus(100, 1, point, ts)
    SetTmAnalog(100, 1, point, ti)