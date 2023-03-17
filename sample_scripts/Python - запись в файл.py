# Записываем в файл значение ТИТ с адресом #TT24:1:7

filename = 'D:/test/log.txt'

value = GetTmAnalog(24, 1, 7)

with open(filename, 'a') as f:
    f.write(str(value) + '\n')