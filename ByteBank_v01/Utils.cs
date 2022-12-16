using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteBank_v01;
public class Utils
{
    public static void Continue()
    {
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadLine();
        Console.Clear();
    }

    public static void InvalidOption()
    {
        Console.WriteLine("Opção inválida. Tente novamente.");
        Continue();
    }

    public static string getPassword()
    {
        var pass = string.Empty;
        ConsoleKey key;
        do
        {
            var keyInfo = Console.ReadKey(intercept: true);
            key = keyInfo.Key;

            if (key == ConsoleKey.Backspace && pass.Length > 0)
            {
                Console.Write("\b \b");
                pass = pass[0..^1];
            }
            else if (!char.IsControl(keyInfo.KeyChar))
            {
                Console.Write("*");
                pass += keyInfo.KeyChar;
            }
        } while (key != ConsoleKey.Enter);
        
        Console.WriteLine();
        return pass;
    }

    public static decimal getDecimal()
    {
        bool isValid;
        decimal value;

        do
        {
            isValid = decimal.TryParse(Console.ReadLine(), out value);
            if (isValid == false || value <= 0)
                Console.WriteLine("O valor fornecido é inválido. Tente novamente e forneça o valor abaixo:");

        } while (isValid == false || value <= 0);

        return value;
    }

    public static string getCpf()
    {
        string inputCpf;
        bool isValid;

        do
        {
            inputCpf = Console.ReadLine();
            isValid = ValidateCpf(inputCpf);

            if (isValid == false)
                Console.WriteLine("O CPF informado é inválido. Tente novamente e forneça o valor abaixo:");

        } while (isValid == false);

        return inputCpf;
    }

    public static string ShowFormattedCpf(string cpf)
    {
        return Convert.ToUInt64(cpf).ToString(@"000\.000\.000\-00");
    }

    public static bool ValidateCpf(string cpf)
    {
        int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        string tempCpf;
        string digito;
        int soma;
        int resto;
        cpf = cpf.Trim();
        cpf = cpf.Replace(".", "").Replace("-", "");
        if (cpf.Length != 11)
            return false;
        tempCpf = cpf.Substring(0, 9);
        soma = 0;

        for (int i = 0; i < 9; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
        resto = soma % 11;
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;
        digito = resto.ToString();
        tempCpf = tempCpf + digito;
        soma = 0;
        for (int i = 0; i < 10; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
        resto = soma % 11;
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;
        digito = digito + resto.ToString();
        return cpf.EndsWith(digito);
    }
}
