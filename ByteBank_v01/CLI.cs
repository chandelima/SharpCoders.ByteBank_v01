using System.Security.Principal;
using System.Text;
using ByteBank_v01.Model;

namespace ByteBank_v01;
public class CLI
{
    Service service = new Service();
    
    public void Startup()
    {
        uint option;

        do
        {
            option = MainScreen();

            switch (option)
            {
                case 1:
                    CreateAccount();
                    break;
                case 2:
                    DeleteAccount();
                    break;
                case 3:
                    ListAccounts();
                    break;
                case 4:
                    AccountDetail();
                    break;
                case 5:
                    GetBankTotalBalance();
                    break;
                case 6:
                    ManipulateAccount();
                    break;
            }
        } while (option != 0);
    }
    
    public uint MainScreen()
    {
        uint inputValue;
        bool inputValid;

        do
        {
            ShowHeader();

            Console.WriteLine("1 - Abrir uma nova conta");
            Console.WriteLine("2 - Encerrar uma conta existente");
            Console.WriteLine("3 - Listar todas as contas abertas");
            Console.WriteLine("4 - Detalhes de uma conta");
            Console.WriteLine("5 - Quantia total armazenada no banco");
            Console.WriteLine("6 - Manipular a conta");
            Console.WriteLine("0 - Sair do Programa");
            Console.WriteLine();
            Console.Write("Digite a opção desejada: ");

            inputValid = uint.TryParse(Console.ReadLine(), out inputValue);
            if(inputValid == false || inputValue > 6)
            {
                Utils.InvalidOption();
            } 

        } while (inputValid == false || inputValue > 6);

        return inputValue;
    }

#region Menu Operations

    public void CreateAccount()
    {
        ShowHeader();

        Console.WriteLine("Para criar uma nova conta, forneça os dados a seguir: \n");
        
        Console.Write("Nome do titular: ");
        string holderName = Console.ReadLine();

        Console.Write("CPF do titular: ");
        string cpf = GetCpf();

        if(service.GetAccountByCpf(cpf) != null)
        {
            Console.WriteLine("\nO CPF informado já possui conta registrada. Confira os dados informados e tente novamente.");
            Utils.Continue();
            return;
        }


        string password, passwordConfirmation;
        do
        {
            Console.Write("Senha da conta (mín. 8 caractéres): ");
            password = GetPassword();

            Console.Write("Confirme a senha da conta: ");
            passwordConfirmation = GetPassword();

            if (password != passwordConfirmation)
                Console.WriteLine("As senhas digitadas não conferem. Tente novamente...\n");
            else if (password.Length < 8)
                Console.WriteLine("A senha deve conter pelo menos 8 caractéres. Tente novamente...\n");

        } while (password != passwordConfirmation || password.Length < 8);

        Console.Write("Deseja realizar um depósito inicial? (S = Sim/N = Não): ");
        string initialDeposit = Console.ReadLine();
        decimal depositValue = 0.00M;

        if(initialDeposit.ToLower() == "s")
        {
            Console.Write("Forneça abaixo o valor para depósito: ");
            depositValue = GetDecimal();
        }

        Account account = new Account(cpf, holderName, password, depositValue);
        string result = service.CreateAccount(account);

        if (result == "")
        {
            Console.Clear();
            Console.WriteLine("Conta bancária criada com sucesso. Seguem dados da nova conta abaixo:");
            Console.WriteLine(account);
        }
        else
            ShowError(result);

        Utils.Continue();
    }

    public void DeleteAccount()
    {
        ShowHeader();

        Console.WriteLine("Para encerrar a sua conta, forneça os dados a seguir: \n");

        Account account = GetAccount();

        if (account != null)
        {
            Console.WriteLine("\nConfira abaixo os dados da conta a ser excluída");
            Console.WriteLine(account);
            Console.Write("\nTem certeza que deseja encerrar esta conta corrente? (S = Sim/N = Não): ");
            string confirmation = Console.ReadLine();
            
            if(confirmation.ToLower() == "s")
            {
                bool deletionResult = service.DeleteAccount(account);
                if (deletionResult == true)
                    Console.WriteLine("Conta encerrada com sucesso.");
                else
                    Console.WriteLine("Erro na operção de exclusão.");
            } else
                Console.WriteLine("Operação de encerramento cancelada.");
        }

        Utils.Continue();
    }

    public void ListAccounts()
    {
        ShowHeader();

        List<Account> accounts = service.GetAllAccounts();

        if (accounts.Count() > 0)
        {
            Console.WriteLine("LISTAGEM DE CONTAS ATIVAS:\n");

            foreach (Account account in accounts)
                Console.WriteLine(account.OneLineInfo());
        } else
            Console.WriteLine("Não há nenhuma conta cadastrada para ser exibida.");

        Utils.Continue();
    }

    public void AccountDetail()
    {
        ShowHeader();
        
        Account account = GetAccount();
        if (account == null)
        {
            return;
        }

        ShowHeader();
        Console.WriteLine("Detalhes da conta:");
        Console.WriteLine(account);
        Utils.Continue();
    }

    public void GetBankTotalBalance()
    {
        (decimal Balance, int Accounts) total = service.GetBankTotalBalance();

        ShowHeader();
        Console.WriteLine("VALOR TOTAL ARMAZENADO NO BANCO:");
        Console.WriteLine($"No momento, o banco possui {total.Accounts} conta(s) ativa(s) e está armazenando R${total.Balance:F2}.");
        Utils.Continue();
    }

    public void ManipulateAccount()
    {
        uint inputValue = 100;
        bool inputValid = false;
        Account account = null;

        do
        {
            ShowHeader();

            if (account == null)
            {
                account = GetAccount();
                if (account == null)
                    return;

            } else
            {
                ShowHeader();

                Console.WriteLine($"Seja bem-vindo(a), Sr(a). {account.HolderName}!\n");
                Console.WriteLine("Operações disponíveis:");
                Console.WriteLine();
                Console.WriteLine("1 - Efetuar depósito");
                Console.WriteLine("2 - Efetuar saque");
                Console.WriteLine("3 - Realizar uma transferência");
                Console.WriteLine("0 - Voltar ao menu principal");
                Console.WriteLine();
                Console.Write("Digite a opção desejada: ");

                inputValid = uint.TryParse(Console.ReadLine(), out inputValue);
                if (inputValid == false || inputValue > 3)
                {
                    Utils.InvalidOption();
                }
                else
                {
                    switch (inputValue)
                    {
                        case 1:
                            MakeDeposit(account);
                            inputValue = 100;
                            break;
                        case 2:
                            MakeWithdraw(account);
                            inputValue = 100;
                            break;
                        case 3:
                            MakeTransference(account);
                            inputValue = 100;
                            break;
                    }
                }
            }

            
        } while (inputValid == false || inputValue > 3);
    }

    public void MakeDeposit(Account account)
    {
        ShowHeader();

        Console.Write("Para efetuar a operação, forneça o valor a ser depositado: ");
        decimal depositValue = GetDecimal();

        account = service.MakeDeposit(account, depositValue);

        Console.WriteLine("\nDepósito realizado com sucesso.");
        Console.WriteLine($"O saldo atual da sua conta é: R${account.Balance:F2}.");

        Utils.Continue();
    }

    public void MakeWithdraw(Account account)
    {
        ShowHeader();

        Console.Write("Para efetuar a operação, forneça o valor a ser sacado: ");
        decimal withdrawValue = GetDecimal();

        if (account.Balance >= withdrawValue)
        {
            account = service.MakeWithDraw(account, withdrawValue);

            Console.WriteLine("\nSaque realizado com sucesso.");
            Console.WriteLine($"O saldo atual da sua conta é: R${account.Balance:F2}.");
        }
        else
            Console.WriteLine("O valor solicitado para saque é superior ao saldo disponível na conta.");

        Utils.Continue();
    }

    public void MakeTransference(Account sourceAccount)
    {
        ShowHeader();

        Console.Write("Para efetuar a transferência, forneça o CPF da conta de destino: ");
        string destinationCpf = Console.ReadLine();

        Account destinationAccount = service.GetAccountByCpf(destinationCpf);

        if (destinationAccount == null)
        {
            Console.WriteLine("\nConta não econtrada. Confira os dados fornecidos e tente novamente.");
            Utils.Continue();
            return;
        }

        Console.Write("Qual o valor a ser transferido: ");
        decimal transferValue = GetDecimal();

        string result = service.MakeTransfer(sourceAccount, destinationAccount, transferValue);
        if (result == "")
        {
            Console.WriteLine("\nTransferência realizada com sucesso.");
            Console.WriteLine($"O saldo atual da sua conta é: R${sourceAccount.Balance:F2}.");
        }
        else
            ShowError(result);

        Utils.Continue();
    }
    #endregion

    public Account GetAccount()
    {
        ShowHeader();
        Console.WriteLine("Forneça os dados da sua conta abaixo para prosseguir:\n");

        Console.Write("CPF do titular: ");
        string cpf = Console.ReadLine();

        Console.Write("Senha da conta: ");
        string password = GetPassword();

        Account account = service.GetAccountByCpfAndPass(cpf, password);

        if (account == null)
        {
            Console.WriteLine("\nConta não econtrada. Confira os dados fornecidos e tente novamente.");
            Utils.Continue();
        }

        return account;
    }

    public void ShowError(string error)
    {
        Console.WriteLine("\nNão foi possível realizar a operação. Motivo:");
        Console.WriteLine($"\t{error}");
    }

    public static decimal GetDecimal()
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

    public static string GetCpf()
    {
        string inputCpf;
        bool isValid;

        do
        {
            inputCpf = Console.ReadLine();
            isValid = Utils.ValidateCpf(inputCpf);

            if (isValid == false)
                Console.WriteLine("O CPF informado é inválido. Tente novamente e forneça o valor abaixo:");

        } while (isValid == false);

        return inputCpf;
    }

    public static string GetPassword()
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

    public static void ShowHeader()
    {
        Console.Clear();

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("**************************************************************");
        sb.AppendLine("*                                                            *");
        sb.AppendLine("*                          BYTEBANK                          *");
        sb.AppendLine("*                                                            *");
        sb.AppendLine("**************************************************************");

        Console.WriteLine(sb);
    }
}
