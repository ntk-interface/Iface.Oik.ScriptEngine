// Записываем в файл значение ТИТ с адресом #TT24:1:7

var filename = 'D:/test/log.txt';

var value = GetTmAnalog(24, 1, 7);

System.IO.File.AppendAllText(filename, value + '\n');