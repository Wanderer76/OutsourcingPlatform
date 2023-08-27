namespace OutsourcePlatformApp.Utils;

public static class InnCheck
{
    private static readonly int[] arrMul10 = { 2, 4, 10, 3, 5, 9, 4, 6, 8 };
    private static readonly int[] arrMul121 = { 7, 2, 4, 10, 3, 5, 9, 4, 6, 8 };
    private static readonly int[] arrMul122 = { 3, 7, 2, 4, 10, 3, 5, 9, 4, 6, 8 };

    public static bool CheckInn(string value)
    {
        if (value.Length == 11)
        {
            if (value[0] != 'F')
                return false;
            value = value.Remove(0, 1);
        }

        // должно быть 10 или 12 цифр
        if (value.Length is not (10 or 12))
            return false;
        try
        {
            return IsINN(value);
        }
        catch
        {
            return false;
        }
    }

    private static bool IsINN(string INNstring)
    {
        try
        {
            long.Parse(INNstring);
        }
        catch
        {
            return false;
        }

        // проверка на 10 и 12 цифр
        if (INNstring.Length != 10 && INNstring.Length != 12)
        {
            return false;
        }

        // проверка по контрольным цифрам
        if (INNstring.Length == 10) // для юридического лица
        {
            int dgt10;
            try
            {
                dgt10 = (2 * int.Parse(INNstring.Substring(0, 1))
                         + 4 * int.Parse(INNstring.Substring(1, 1))
                         + 10 * int.Parse(INNstring.Substring(2, 1))
                         + 3 * int.Parse(INNstring.Substring(3, 1))
                         + 5 * int.Parse(INNstring.Substring(4, 1))
                         + 9 * int.Parse(INNstring.Substring(5, 1))
                         + 4 * int.Parse(INNstring.Substring(6, 1))
                         + 6 * int.Parse(INNstring.Substring(7, 1))
                         + 8 * int.Parse(INNstring.Substring(8, 1))) % 11 % 10;
            }
            catch
            {
                return false;
            }

            return int.Parse(INNstring.Substring(9, 1)) == dgt10;
        }

        // для физического лица
        int dgt11, dgt12;
        try
        {
            dgt11 = (
                7 * int.Parse(INNstring.Substring(0, 1))
                + 2 * int.Parse(INNstring.Substring(1, 1))
                + 4 * int.Parse(INNstring.Substring(2, 1))
                + 10 * int.Parse(INNstring.Substring(3, 1))
                + 3 * int.Parse(INNstring.Substring(4, 1))
                + 5 * int.Parse(INNstring.Substring(5, 1))
                + 9 * int.Parse(INNstring.Substring(6, 1))
                + 4 * int.Parse(INNstring.Substring(7, 1))
                + 6 * int.Parse(INNstring.Substring(8, 1))
                + 8 * int.Parse(INNstring.Substring(9, 1))) % 11 % 10;
            dgt12 = (
                3 * int.Parse(INNstring.Substring(0, 1))
                + 7 * int.Parse(INNstring.Substring(1, 1))
                + 2 * int.Parse(INNstring.Substring(2, 1))
                + 4 * int.Parse(INNstring.Substring(3, 1))
                + 10 * int.Parse(INNstring.Substring(4, 1))
                + 3 * int.Parse(INNstring.Substring(5, 1))
                + 5 * int.Parse(INNstring.Substring(6, 1))
                + 9 * int.Parse(INNstring.Substring(7, 1))
                + 4 * int.Parse(INNstring.Substring(8, 1))
                + 6 * int.Parse(INNstring.Substring(9, 1))
                + 8 * int.Parse(INNstring.Substring(10, 1))) % 11 % 10;
        }
        catch
        {
            return false;
        }

        return int.Parse(INNstring.Substring(10, 1)) == dgt11
               && int.Parse(INNstring.Substring(11, 1)) == dgt12;
    }
}