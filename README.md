## Iface.Oik.ScriptEngine

Программа для написания скриптов дорасчета Сервера «ОИК Диспетчер НТ» на популярных языках программирования Javascript и Python, а также внутри таблиц формата Microsoft Excel.

Программа может работать как внешняя задача Сервера «ОИК Диспетчер НТ» или автономно на любом удаленном компьютера.

При запуске автономно требуется указать следующие параметры командной строки:

    Iface.Oik.ScriptEngine сервер_динамических_данных компьютер имя_пользователя пароль
    
Например:

    Iface.Oik.ScriptEngine TMS 10.0.0.69 admin password

## Скрипты дорасчета

Каждый скрипт дорасчета хранится в папке `scripts` в отдельных файлах с расширением .js (для языка Javascript), .py (для языка Python), .xlsx (для Excel) при этом общее количество файлов не ограничено.



#### Функции для получения данных телеметрии сервера ОИК

```
string GetTmStatusName(int ch, int rtu, int point)

Возвращает наименование сигнала с адресом "ch":"rtu":"point" (канал:кп:объект)
```

```
int GetTmStatus(int ch, int rtu, int point)

Возвращает текущее состояние сигнала с адресом "ch":"rtu":"point" (канал:кп:объект), где 0 - откл, 1 - вкл 
```

```
int GetTmStatusOrDefault(int ch, int rtu, int point)

Возвращает текущее состояние сигнала с адресом "ch":"rtu":"point" (канал:кп:объект), где 0 - откл, 1 - вкл. 
Если сигнал отсутствует, или недостоверен (флаги NT или IV), то возвращает переданное значение по умолчанию defaultValue
```

```
bool IsTmStatusOn(int ch, int rtu, int point)

Возвращает булевое состояние включенности сигнала с адресом "ch":"rtu":"point" (канал:кп:объект)
```

```
bool IsTmStatusFlagRaised(int ch, int rtu, int point, TmFlags flag)

Возвращает булевое состояние взведенности флага "flag" (см. ниже список флагов) сигнала с адресом "ch":"rtu":"point" (канал:кп:объект)
```

```
int GetTmStatusFromRetro(int ch, int rtu, int point, long timestamp)

Возвращает состояние сигнала с адресом "ch":"rtu":"point" (канал:кп:объект) из ретроспективы в момент времени "timestamp" (время в секундах, начиная с 1 января 1970), где 0 - откл, 1 - вкл, null - недостоверно
```

```
string GetTmAnalogName(int ch, int rtu, int point)

Возвращает наименование измерения с адресом "ch":"rtu":"point" (канал:кп:объект)
```

```
string GetTmAnalogUnit(int ch, int rtu, int point)

Возвращает единицу измерения (МВт, кВ и т.п.) измерения с адресом "ch":"rtu":"point" (канал:кп:объект)
```

```
float GetTmAnalog(int ch, int rtu, int point)

Возвращает текущее значение измерения с адресом "ch":"rtu":"point" (канал:кп:объект)
```

```
float GetTmAnalogOrDefault(int ch, int rtu, int point, float defaultValue)

Возвращает текущее значение измерения с адресом "ch":"rtu":"point" (канал:кп:объект)
Если сигнал отсутствует, или недостоверен (флаги NT или IV), то возвращает переданное значение по умолчанию defaultValue
```

```
bool IsTmAnalogFlagRaised(int ch, int rtu, int point, TmFlags flag)

Возвращает булевое состояние взведенности флага "flag" (см. ниже список флагов) измерения с адресом "ch":"rtu":"point" (канал:кп:объект)
```

```
float? GetTmAnalogFromRetro(int ch, int rtu, int point, long timestamp, int? retroNum)

Возвращает значение измерения с адресом "ch":"rtu":"point" (канал:кп:объект) из ретроспективы с номером "retroNum" (можно указать 0, чтобы сервер сам выбрал подходящую ретроспективу) в момент времени "timestamp" (время в секундах, начиная с 1 января 1970). Если значение недостоверно, возвращает null
```

```
float[] GetTmAnalogRetro(int ch, int rtu, int point, long startTimestamp, long endTimestamp, int step, int? retroNum)

Возвращает массив значений измерения с адресом "ch":"rtu":"point" (канал:кп:объект) из ретроспективы с номером "retroNum" (можно указать 0, чтобы сервер сам выбрал подходящую ретроспективу), с момента времени "startTimestamp" (время в секундах, начиная с 1 января 1970) до "endTimestamp" с шагом "step" (в секундах). Если значений нет, массив будет пустым. Если одно из значений недостоверно, элемент массива равен null.
```

```
float[] GetTmAnalogImpulseArchiveAverage(int ch, int rtu, int point, long startTimestamp, long endTimestamp, int step)

Возвращает массив значений измерения с адресом "ch":"rtu":"point" (канал:кп:объект) из средних значений импульс-архива, с момента времени "startTimestamp" (время в секундах, начиная с 1 января 1970) до "endTimestamp" с шагом "step" (в секундах). Если значений нет, массив будет пустым. Если одно из значений недостоверно, элемент массива равен null.
```

```
float[] GetTmAnalogMicroSeries(int ch, int rtu, int point)

Возвращает массив значений измерения с адресом "ch":"rtu":"point" (канал:кп:объект) из микросерий (последние значения ежесекундно, не более десяти значений). Только для сервера 3.Х. Если значений нет, массив будет пустым. Если одно из значений недостоверно, элемент массива равен null.
```

```
string GetTmAccumName(int ch, int rtu, int point)

Возвращает наименование интегрального измерения (ТИИ) с адресом "ch":"rtu":"point" (канал:кп:объект)
```

```
string GetTmAccumUnit(int ch, int rtu, int point)

Возвращает единицу измерения (МВт, кВ и т.п.) интегрального измерения (ТИИ) с адресом "ch":"rtu":"point" (канал:кп:объект)
```

```
float GetTmAccum(int ch, int rtu, int point)

Возвращает текущее значение интегрального измерения (ТИИ) с адресом "ch":"rtu":"point" (канал:кп:объект)
```

```
float GetTmAccumOrDefault(int ch, int rtu, int point, float defaultValue)

Возвращает текущее значение интегрального измерения (ТИИ) с адресом "ch":"rtu":"point" (канал:кп:объект)
Если сигнал отсутствует, или недостоверен (флаги NT или IV), то возвращает переданное значение по умолчанию defaultValue
```

```
float GetTmAccumLoad(int ch, int rtu, int point)

Возвращает значение нагрузки интегрального измерения (ТИИ) с адресом "ch":"rtu":"point" (канал:кп:объект)
```

```
float GetTmAccumLoadOrDefault(int ch, int rtu, int point, float defaultValue)

Возвращает значение нагрузки интегрального измерения (ТИИ) с адресом "ch":"rtu":"point" (канал:кп:объект)
Если сигнал отсутствует, или недостоверен (флаги NT или IV), то возвращает переданное значение по умолчанию defaultValue
```

```
bool IsTmAccumFlagRaised(int ch, int rtu, int point, TmFlags flag)

Возвращает булевое состояние взведенности флага "flag" (см. ниже список флагов) интегрального измерения (ТИИ) с адресом "ch":"rtu":"point" (канал:кп:объект)
```

```
float? GetTmAccumFromRetro(int ch, int rtu, int point, long timestamp, int? retroNum)

Возвращает значение интегрального измерения (ТИИ) с адресом "ch":"rtu":"point" (канал:кп:объект) из ретроспективы в момент времени "timestamp" (время в секундах, начиная с 1 января 1970). Если значение недостоверно, возвращает null
```

#### Функции для занесения данных телеметрии сервера ОИК

```
void Telecontrol(int ch, int rtu, int point, int status)

Подает команду телеуправления "status" (0 - откл, 1 - вкл) на сигнал с адресом "ch":"rtu":"point" (канал:кп:объект). Предварительно требуется вызвать функцию AllowTelecontrol (см. ниже)
```

```
void SetTmStatus(int ch, int rtu, int point, int status)

Устанавливает состояние "status" (где 0 - откл, 1 - вкл) сигнала с адресом "ch":"rtu":"point" (канал:кп:объект)
```

```
void RaiseTmStatusFlag(int ch, int rtu, int point, TmFlags flag)

Взводит флаг "flag" (см. ниже список флагов) сигнала с адресом "ch":"rtu":"point" (канал:кп:объект)
Не допускается воздействие на флаги "TmFlagInvalid" и "TmFlagAbnormal" 
```

```
void ClearTmStatusFlag(int ch, int rtu, int point, TmFlags flag)

Снимает флаг "flag" (см. ниже список флагов) сигнала с адресом "ch":"rtu":"point" (канал:кп:объект)
Не допускается воздействие на флаги "TmFlagInvalid" и "TmFlagAbnormal" 
```

```
void SetTmAnalog(int ch, int rtu, int point, float value)

Устанавливает значение "value" измерения с адресом "ch":"rtu":"point" (канал:кп:объект)
```

```
void RaiseTmAnalogFlag(int ch, int rtu, int point, TmFlags flag)

Взводит флаг "flag" (см. ниже список флагов) измерения с адресом "ch":"rtu":"point" (канал:кп:объект)
Не допускается воздействие на флаги "TmFlagInvalid" и "TmFlagLevel1".."TmFlagLevel4"
```

```
void ClearTmAnalogFlag(int ch, int rtu, int point, TmFlags flag)

Снимает флаг "flag" (см. ниже список флагов) измерения с адресом "ch":"rtu":"point" (канал:кп:объект)
Не допускается воздействие на флаги "TmFlagInvalid" и "TmFlagLevel1".."TmFlagLevel4"
```


#### Константы флагов телеметрии

```
TmFlagUnreliable      - Неактуальное значение
TmFlagInvalid         - Недействительное значение
TmFlagAbnormal        - Несоответствие нормальному режиму
TmFlagManuallyBlocked - Заблокировано оператором
TmFlagManuallySet     - Установлено вручную
TmFlagLevel1          - Взведен флаг 1 (для ТС) или уставка ОС (для ТИ)
TmFlagLevel2          - Взведен флаг 2 (для ТС) или уставка ПС2 (для ТИ)
TmFlagLevel3          - Взведен флаг 3 (для ТС) или уставка ПС1 (для ТИ)
TmFlagLevel4          - Взведен флаг 4 (для ТС) или уставка АС (для ТИ)
TmFlagUnacked         - Неквитирован
```

#### Вспомогательные функции

```
void OverrideScriptTimeout(int timeout)

Устанавливает время пересчета скрипта, в миллисекундах. По умолчанию скрипт пересчитывается раз в 2 секунды (значение 2000).
```

```
void WriteToStorage(string key, object value)

Записывает значение "value" (любого типа) в хранилище с ключом "key". Количество возможных ключей внутри не ограничено. Ячейки каждого скрипта полностью не зависимы друг от друга.  
```

```
object ReadFromStorage(string key)

Возвращает значение из хранилища с ключом "key". Если значение отсутствует (например, при первом запуске скрипта), то возвращается "null" для скрипта на языке Javascript, "None" - на языке Python
```

```
void AllowTelecontrol()

Разрешает выполнение команд телеуправления в скрипте
```

```
void LogDebug(string message)

Выводит отладочное сообщение в консоль программы
```

## Особенности использования 

#### Особенности использования Javascript

Поддерживается большая часть стандартов языка вплоть до версии [ECMAScript 2024](https://www.w3schools.com/js/js_2024.asp).

#### Особенности использования Python

Полностью поддерживается версия языка [Python 3.4](https://www.python.org/downloads/release/python-340/).

## Примеры скриптов
Примеры скриптов доступны в каталоге `sample_scripts` в архиве установки.