from random import uniform

# Функция, рассчитывающая случайное отклонение
def noiseAbs(delta):
    return delta - uniform(0, delta) * 2

# Устанавливаем значения измерений 0:1:3 и 0:1:4
SetTmAnalog(0, 1, 3, 20 + noiseAbs(2));
SetTmAnalog(0, 1, 4, 50 + noiseAbs(2));