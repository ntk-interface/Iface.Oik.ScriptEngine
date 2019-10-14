ts = GetTmStatus(100, 1, 1)
ti = GetTmAnalog(100, 1, 1)

for point in range(2, 9):
    SetTmStatus(100, 1, point, ts)
    SetTmAnalog(100, 1, point, ti)