name = GetTmAccumName(20, 1, 1)
unit = GetTmAccumUnit(20, 1, 1)

value = GetTmAccum(20, 1, 1)
load = GetTmAccumLoad(20, 1, 1)

fromRetro = GetTmAccumFromRetro(20, 1, 1, 1768314600)

LogDebug(name)
LogDebug(unit)
LogDebug(str(value))
LogDebug(str(load))
LogDebug(str(fromRetro))