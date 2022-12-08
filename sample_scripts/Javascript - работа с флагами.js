// дублируем флаг неактуальности с #TC20:1:1 на #TC20:1:2
if (IsTmStatusFlagRaised(20, 1, 1, TmFlagUnreliable))
{
  RaiseTmStatusFlag(20, 1, 2, TmFlagUnreliable);
}
else {
  ClearTmStatusFlag(20, 1, 2, TmFlagUnreliable);
}


// в зависимости от состояния сигнала #TC22:1:11 взводим или снимаем флаг для #TC22:1:1
if (IsTmStatusOn(22, 1, 11))
{
  RaiseTmStatusFlag(22, 1, 1, TmFlagLevel1);
}
else {
  ClearTmStatusFlag(22, 1, 1, TmFlagLevel1);
}


// очищаем флаги ручной установки и блокировки для #TT100:1:3 
ClearTmAnalogFlag(100, 1, 3, TmFlagManuallySet | TmFlagManuallyBlocked);