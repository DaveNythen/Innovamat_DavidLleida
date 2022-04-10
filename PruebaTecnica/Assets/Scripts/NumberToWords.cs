using System.Collections.Generic;

public static class NumberToWords
{
    static List<string> ones = new List<string> { "zero", "uno", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "nueve" };
    static List<string> teens = new List<string> {"diez", "once", "doze", "trece", "catorce", "quinze" };
    static List<string> tens = new List<string> {"", "", "veinte", "treinta", "cuarenta", "cincuenta", "sesenta", "setenta", "ochenta", "noventa" };
    static List<string> hundreds = new List<string> { "", "cien", "", "", "", "quinientos", "", "setecientos", "", "novecientos" };

    public static string ConvertNumberToWords(int number)
    {
        string words = "";

        if (number < 10)
        {
            words = getUnits(number);
        }
        else if (number < 20)
        {
            words = getTeens(number);
        }
        else if (number < 100)
        {
            words = getTens(number);
        }
        else if (number < 1000)
        {
            words = getHundreds(number);
        }

        return words;
    }

    private static string getUnits(int number)
    {
        return ones[number];
    }

    private static string getTeens(int number)
    {
        if (number - 10 < 6)
        {
            return teens[number - 10];
        }
        else
        {
            return "dieci" + getUnits(number - 10);
        }
    }

    private static string getTens(int number)
    {
        string words;

        if (number > 20 && number < 30)
        {
            words = "veinti" + getUnits(number % 10);
        }
        else
        {
            words = tens[number / 10];

            if (number % 10 != 0)
            {
                words += " y " + getUnits(number % 10);
            }
        }

        return words;
    }

    private static string getHundreds(int number)
    {
        string words;

        List<int> excepciones = new List<int> {1, 5, 7, 9};

        if (excepciones.Contains(number / 100))
        {
            words = hundreds[number / 100];

            if (number % 100 != 0)
            {
                if (number < 200)
                    words += "to";

                words += " " + ConvertNumberToWords(number % 100);
            }
        }
        else
        {
            words = getUnits(number / 100) + "cientos";

            if (number % 100 != 0)
            {
                words += " " + ConvertNumberToWords(number % 100);
            }
        }

        return words;
    }
}
