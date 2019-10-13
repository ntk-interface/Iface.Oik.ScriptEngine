while (true) {
    var dateTime = new Date();
    var dayOfWeek = dateTime.getDay();
    var day = dateTime.getDate();
    var month = dateTime.getMonth() + 1;
    var year = dateTime.getFullYear();
    var hour = dateTime.getHours();
    var minute = dateTime.getMinutes();
    var second = dateTime.getSeconds();
    var linePowered;

    // ?? 110 ?? - ??
    linePowered = (isTsOn(21, 3, 1) && isTsOn(21, 6, 1) && isTsOn(20, 5, 1) && isTsOn(20, 1, 1));
    SetTmAnalog(21, 3, 1, (linePowered) ? -hour / 4 -  7 - dayOfWeek + noise(3) : 0);
    SetTmAnalog(21, 3, 2, (linePowered) ? -hour / 4 -  5 - dayOfWeek + noise(3) : 0);

    // ?? 110 ?? - ???
    if (isTsOn(21, 5, 100)) { // ????????? ?????
        linePowered = (isTsOn(21, 5, 1));
        SetTmAnalog(21, 5, 1, (linePowered) ? 46 + dayOfWeek + noiseAbs(3) : 0);
        SetTmAnalog(21, 5, 2, (linePowered) ? 6 + dayOfWeek + noiseAbs(3) : 0);
    }
    else { // ????????
        linePowered = (isTsOn(21, 5, 1) && isTsOn(21, 1, 1) && isTsOn(20, 3, 1) && isTsOn(20, 4, 1));
        SetTmAnalog(21, 5, 1, (linePowered) ? -hour / 4 - 46 - dayOfWeek - noiseAbs(3) : 0);
        SetTmAnalog(21, 5, 2, (linePowered) ? -hour / 4 - 6 - dayOfWeek - noiseAbs(3) : 0);
    }

    // ?? 110 ?? - ???
    linePowered = (isTsOn(21, 7, 1) && isTsOn(21, 6, 1) && isTsOn(20, 5, 1) && isTsOn(20, 1, 1));
    SetTmAnalog(21, 7, 1, (linePowered) ? -hour / 4 -  5 - dayOfWeek - noiseAbs(3) : 0);
    SetTmAnalog(21, 7, 2, (linePowered) ? -hour / 4 -  5 - dayOfWeek - noiseAbs(3) : 0);

    // ?? 110 ?? - ????
    linePowered = (isTsOn(21, 8, 1) && isTsOn(21, 1, 1) && isTsOn(20, 3, 1) && isTsOn(20, 4, 1));
    SetTmAnalog(21, 8, 1, (linePowered) ? -hour / 4 -  14 - dayOfWeek - noiseAbs(3) : 0);
    SetTmAnalog(21, 8, 2, (linePowered) ? -hour / 4 -  2 - dayOfWeek - noiseAbs(3) : 0);

    // ??-1 110 ??
    SetTmAnalog(21, 1, 1, - (ti(21, 5, 1) + ti(21, 8, 1)));
    SetTmAnalog(21, 1, 2, - (ti(21, 5, 2) + ti(21, 8, 2)));
    // ??-2 110 ??
    SetTmAnalog(21, 6, 1, - (ti(21, 3, 1) + ti(21, 7, 1)));
    SetTmAnalog(21, 6, 2, - (ti(21, 3, 2) + ti(21, 7, 2)));

    // ?????????? ??? 110 ??
    SetTmAnalog(21, 9, 1, (isTiActive(21, 1, 1)) ? 111 + noiseAbs(2) : 0);
    SetTmAnalog(21, 10, 1, (isTiActive(21, 6, 1)) ? 111 + noiseAbs(2) : 0);
    SetTmAnalog(21, 11, 1, (isTsOn(21, 2, 1) && (isTsOn(21, 1, 1) || isTsOn(21, 6, 1))) ? 111 + noiseAbs(2) : 0); // ???


    // ?-5 6??
    linePowered = (isTsOn(21, 1, 1) && isTsOn(22, 13, 1) && isTsOn(22, 5, 1));
    SetTmAnalog(22, 5, 1, (linePowered) ? -hour / 8 - 3 -noiseAbs(2) : 0);
    SetTmAnalog(22, 5, 2, (linePowered) ? -hour / 8 - 1 -noiseAbs(1) : 0);
    // ?-7 6??
    linePowered = (isTsOn(21, 1, 1) && isTsOn(22, 13, 1) && isTsOn(22, 7, 1));
    SetTmAnalog(22, 7, 1, (linePowered) ? -hour / 8 - 3 -noiseAbs(2) : 0);
    SetTmAnalog(22, 7, 2, (linePowered) ? -hour / 8 - 1 -noiseAbs(1) : 0);
    // ?-9 6??
    linePowered = (isTsOn(21, 1, 1) && isTsOn(22, 13, 1) && isTsOn(22, 9, 1));
    SetTmAnalog(22, 9, 1, (linePowered) ? -hour / 8 - 3 -noiseAbs(2) : 0);
    SetTmAnalog(22, 9, 2, (linePowered) ? -hour / 8 - 1 -noiseAbs(1) : 0);

    // ?-6 6??
    linePowered = (isTsOn(21, 6, 1) && isTsOn(22, 14, 1) && isTsOn(22, 6, 1));
    SetTmAnalog(22, 6, 1, (linePowered) ? -hour / 8 - 3 -noiseAbs(2) : 0);
    SetTmAnalog(22, 6, 2, (linePowered) ? -hour / 8 - 1 -noiseAbs(1) : 0);
    // ?-8 6??
    linePowered = (isTsOn(21, 6, 1) && isTsOn(22, 14, 1) && isTsOn(22, 8, 1));
    SetTmAnalog(22, 8, 1, (linePowered) ? -hour / 8 - 3 -noiseAbs(2) : 0);
    SetTmAnalog(22, 8, 2, (linePowered) ? -hour / 8 - 1 -noiseAbs(1) : 0);
    // ?-10 6??
    linePowered = (isTsOn(21, 6, 1) && isTsOn(22, 14, 1) && isTsOn(22, 10, 1));
    SetTmAnalog(22, 10, 1, (linePowered) ? -hour / 8 - 3 -noiseAbs(2) : 0);
    SetTmAnalog(22, 10, 2, (linePowered) ? -hour / 8 - 1 -noiseAbs(1) : 0);

    // ??-1 6 ??
    SetTmAnalog(22, 13, 1, - (ti(22, 5, 1) + ti(22, 7, 1) + ti(22, 9, 1)));
    SetTmAnalog(22, 13, 2, - (ti(22, 5, 2) + ti(22, 7, 2) + ti(22, 9, 2)));
    // ??-2 6 ??
    SetTmAnalog(22, 14, 1, - (ti(22, 6, 1) + ti(22, 8, 1) + ti(22, 10, 1)));
    SetTmAnalog(22, 14, 2, - (ti(22, 6, 2) + ti(22, 8, 2) + ti(22, 10, 2)));

    // ?????????? ?????? 6 ??
    SetTmAnalog(22, 3, 1, (isTiActive(22, 13, 1)) ? 6.2 + noiseAbs(0.3) : 0);
    SetTmAnalog(22, 4, 1, (isTiActive(22, 14, 1)) ? 6.2 + noiseAbs(0.3) : 0);


    // ???-1 0,4 ??
    linePowered = (isTsOn(22, 11, 1) && isTiActive(22, 13, 1));
    SetTmAnalog(23, 1, 1, (linePowered) ? 300 + noise(5) : 0);
    SetTmAnalog(23, 1, 2, (linePowered) ? 70 + noise(5) : 0);
    // ???-2 0,4 ??
    linePowered = (isTsOn(22, 12, 1) && isTiActive(22, 14, 1));
    SetTmAnalog(23, 2, 1, (linePowered) ? 280 + noise(5) : 0);
    SetTmAnalog(23, 2, 2, (linePowered) ? 90 + noise(5) : 0);
    // ?????????? ?????? 0,4 ??
    SetTmAnalog(23, 4, 1, (isTiActive(23, 1, 1)) ? 380 + noise(4) : 0);
    SetTmAnalog(23, 5, 1, (isTiActive(23, 2, 1)) ? 380 + noise(4) : 0);


    // ??-1 220 ??
    linePowered = (isTsOn(20, 3, 1) && isTsOn(20, 4, 1));
    SetTmAnalog(20, 3, 1, (linePowered) ? - (ti(21, 1, 1) + ti(22, 13, 1)) : 0);
    SetTmAnalog(20, 3, 2, (linePowered) ? - (ti(21, 1, 2) + ti(22, 13, 2)) : 0);
    // ??-2 220 ??
    linePowered = (isTsOn(20, 5, 1) && isTsOn(20, 1, 1));
    SetTmAnalog(20, 5, 1, (linePowered) ? - (ti(21, 6, 1) + ti(22, 14, 1)) : 0);
    SetTmAnalog(20, 5, 2, (linePowered) ? - (ti(21, 6, 2) + ti(22, 14, 2)) : 0);

    // ?? 220 ?? - ???
    SetTmAnalog(20, 1, 1, - ti(20, 5, 1));
    SetTmAnalog(20, 1, 2, - ti(20, 5, 2));
    SetTmAnalog(20, 1, 3, ti(20, 1, 1) / 3 - noiseAbs(5)); // Pa
    SetTmAnalog(20, 1, 4, ti(20, 1, 1) / 3 + noiseAbs(2)); // Pb
    SetTmAnalog(20, 1, 5, ti(20, 1, 1) / 3 + noiseAbs(3)); // Pc

    // ?? 220 ?? - ?????
    SetTmAnalog(20, 4, 1, - ti(20, 3, 1));
    SetTmAnalog(20, 4, 2, - ti(20, 3, 2));

    // ?????????? ??? 220 ??
    SetTmAnalog(20, 6, 1, (isTiActive(20, 3, 1)) ? 222 + noiseAbs(3) : 0);
    SetTmAnalog(20, 6, 2, (isTiActive(20, 3, 1)) ? 222 + noiseAbs(3) : 0);
    SetTmAnalog(20, 6, 3, (isTiActive(20, 3, 1)) ? 222 + noiseAbs(3) : 0);
    SetTmAnalog(20, 7, 1, (isTiActive(20, 5, 1)) ? 222 + noiseAbs(3) : 0);
    SetTmAnalog(20, 7, 2, (isTiActive(20, 5, 1)) ? 222 + noiseAbs(3) : 0);
    SetTmAnalog(20, 7, 3, (isTiActive(20, 5, 1)) ? 222 + noiseAbs(3) : 0);
    SetTmAnalog(20, 8, 1, (isTsOn(20, 2, 1) && (isTsOn(20, 3, 1) || isTsOn(20, 5, 1))) ? 222 + noiseAbs(3) : 0); // ???


    // ????-?????
    SetTmAnalog(24, 1, 1, year);
    SetTmAnalog(24, 1, 2, month);
    SetTmAnalog(24, 1, 3, day);
    SetTmAnalog(24, 1, 4, dayOfWeek);
    SetTmAnalog(24, 1, 5, hour);
    SetTmAnalog(24, 1, 6, minute);
    SetTmAnalog(24, 1, 7, second);
}

function ti(ch, rtu, point) {
    return GetTmAnalog(ch, rtu, point);
}

function ts(ch, rtu, point) {
    return GetTmStatus(ch, rtu, point);
}

function isTsOn(ch, rtu, point) {
    return (ts(ch, rtu, point) == 1);
}

function isTiActive(ch, rtu, point) {
    return (ti(ch, rtu, point) != 0);
}

function noise(delta) {
    return Math.random() * delta;
}

function noiseAbs(delta) {
    return delta - Math.random() * delta;
}