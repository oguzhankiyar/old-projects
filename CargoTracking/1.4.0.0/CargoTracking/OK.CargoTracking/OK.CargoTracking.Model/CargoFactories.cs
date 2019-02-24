
using System;
using System.Collections.Generic;

namespace OK.CargoTracking.Model
{
    public static class CargoFactories
    {
        public static Factory Aras = new Factory(1, "Aras Kargo", "http://kargotakip.ogzkyr.net/Assets/Factories/factory1.png");
        public static Factory Dhl = new Factory(3, "Dhl Kargo", "http://kargotakip.ogzkyr.net/Assets/Factories/factory2.png");
        public static Factory InterGlobal = new Factory(11, "InterGlobal Kargo", "http://kargotakip.ogzkyr.net/Assets/Factories/factory9.png");
        public static Factory Mng = new Factory(4, "Mng Kargo", "http://kargotakip.ogzkyr.net/Assets/Factories/factory3.png");
        public static Factory Ptt = new Factory(5, "Ptt Kargo", "http://kargotakip.ogzkyr.net/Assets/Factories/factory4.png");
        public static Factory PttInternational = new Factory(6, "Ptt Kargo (Yurtdışı)", "http://kargotakip.ogzkyr.net/Assets/Factories/factory5.png");
        public static Factory Surat = new Factory(7, "Sürat Kargo", "http://kargotakip.ogzkyr.net/Assets/Factories/factory6.png");
        public static Factory Ups = new Factory(8, "Ups Kargo", "http://kargotakip.ogzkyr.net/Assets/Factories/factory7.png");
        public static Factory Yurtici = new Factory(9, "Yurtiçi Kargo", "http://kargotakip.ogzkyr.net/Assets/Factories/factory8.png");
    }
}
