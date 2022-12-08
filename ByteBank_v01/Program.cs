using ByteBank_v01;


int option;

do
{
    option = CLI.MainMenu();

    switch (option)
    {
        case 1:
            Console.WriteLine("Abrir conta do cliente...");
            break;
        case 2:
            Console.WriteLine("Realizar um depósito...");
            break;
        case 3:
            Console.WriteLine("Realizar um saque...");
            break;
        case 4:
            Console.WriteLine("Realizar uma transferência...");
            break;
        case 5:
            Console.WriteLine("Encerrar conta corrente...");
            break;
    }
} while (option != 0);