## Iface.Oik.ScriptEngine

Программа для написания скриптов дорасчета Сервера «ОИК Диспетчер НТ» на популярных языках программирования Javascript и Python.

Программа может работать как внешняя задача Сервера «ОИК Диспетчер НТ» или автономно на любом удаленном компьютера.

При запуске автономно требуется указать следующие параметры командной строки:

    Iface.Oik.ScriptEngine сервер_динамических_данных компьютер имя_пользователя пароль
    
Например:

    Iface.Oik.ScriptEngine TMS 10.0.0.69 admin password

## Скрипты дорасчета

Каждый скрипт дорасчета хранится в папке `scripts` в отдельных файлах с расширением .js (для языка Javascript) и .py (для языка Python), при этом общее количество файлов не ограничено.



#### Функции для работы с сервером ОИК

```
int GetTmStatus(int ch, int rtu, int point)

Возвращает состояния сигнала с адресом "ch":"rtu":"point" (канал:кп:объект), где 0 - откл, 1 - вкл 
```

```
float GetTmAnalog(int ch, int rtu, int point)

возвращает значение измерения с адресом "ch":"rtu":"point" (канал:кп:объект)
```

```
void SetTmStatus(int ch, int rtu, int point, int status)

устанавливает состояние "status" измерения с адресом "ch":"rtu":"point" (канал:кп:объект)
```

```
void SetTmAnalog(int ch, int rtu, int point, float value)

устанавливает значение "value" измерения с адресом "ch":"rtu":"point" (канал:кп:объект), где 0 - откл, 1 - вкл
```

```
void OverrideScriptTimeout(int timeout)

устанавливает время пересчета скрипта, в миллисекундах. По умолчанию скрипт пересчитывается раз в 2 секунды (значение 2000).
```

#### Особенности использования Javascript

Полностью поддерживается стандарт [ECMAScript 5.1](http://www.ecma-international.org/ecma-262/5.1/).

Возможности ECMAScript 6+ (let/const вместо var, стрелочные функции, деструктуризация и т.п.) не поддерживаются.

#### Особенности использования Python

Полностью поддерживается версия языка [Python 2.7](https://www.python.org/download/releases/2.7/).

#### Примеры скриптов
Примеры скриптов доступны в каталоге `sample_scripts` в архиве установки.