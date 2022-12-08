namespace ByteBank_v01;

static class CLI
{
        
    public static int MainMenu()
    {
        Console.WriteLine("Escolha uma opção do menu abaixo:");
        Console.WriteLine();
        Console.WriteLine("1 - Abrir conta corrente");
        Console.WriteLine("2 - Realizar um depósito");
        Console.WriteLine("3 - Realizar um saque");
        Console.WriteLine("4 - Realizar uma transferência");
        Console.WriteLine("5 - Encerrar conta corrente");
        Console.WriteLine("0 - Encerrar sessão");
        Console.WriteLine();
        Console.Write("Digite a opção escolhida: ");

        string result = Console.ReadLine();

        bool conversionResult = int.TryParse(result, out int intResult);
        if (!conversionResult || intResult < 0 || intResult > 5)
        {
            Console.WriteLine();
            Console.WriteLine("ERRO: Opção inválida!");
            Console.WriteLine("Pressione qualquer tecla para tentar novamente...");
            Console.ReadLine();

            Console.Clear();
            intResult = - 1;
        } 

        return intResult;
    }
}
