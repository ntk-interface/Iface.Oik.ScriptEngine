// выводим в консоль программы значение #TT0:1:1, в начале текущего часа
var dateTime1  = new Date();
dateTime1.setMinutes(0);
dateTime1.setSeconds(0);
LogDebug(GetTmAnalogFromRetro(0, 1, 1, getTimestampForOik(dateTime1)));


// выводим в консоль программы значение #TT24:1:7, 5 секунд назад
// если на сервере не настроена секундная ретроспектива, значение будет отсутствовать
var dateTime2  = new Date();
dateTime2.setSeconds(dateTime2.getSeconds() - 5);
LogDebug(GetTmAnalogFromRetro(24, 1, 7, getTimestampForOik(dateTime2)));


// в Javascript время формируется с учетом миллисекунд, требуется их убрать, а также учитываем часовой пояс
function getTimestampForOik(dateTime)
{
	return Math.floor(dateTime.getTime() / 1000) - dateTime.getTimezoneOffset() * 60;
}