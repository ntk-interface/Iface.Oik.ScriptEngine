// выводим в консоль программы значения #TT20:1:1, с начала текущего часа, до текущего момента времени, с шагом 1 минута
var startTime = new Date();
startTime.setMinutes(0);
startTime.setSeconds(0);

var endTime = new Date();

var step = 60; // шаг 1 минута (60 секунд)

var retroNum = 0; // номер ретроспективы (0 значит, что сервер определит ретроспективу самостоятельно)

LogDebug(GetTmAnalogRetro(20, 1, 1, getTimestampForOik(startTime), getTimestampForOik(endTime), step, retroNum).join(', '));




// в Javascript время формируется с учетом миллисекунд, требуется их убрать, а также учитываем часовой пояс
function getTimestampForOik(dateTime)
{
	return Math.floor(dateTime.getTime() / 1000) - dateTime.getTimezoneOffset() * 60;
}