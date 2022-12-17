using System.Security.Principal;

namespace ByteBank_v01;
public class CLI
{
    public uint Startup()
    {
        uint inputValue;
        bool inputValid;

        do
        {
            Console.Clear();

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
        Console.Clear();

        Console.WriteLine("Para criar uma nova conta, forneça os dados a seguir: \n");
        
        Console.Write("Nome do titular: ");
        string holderName = Console.ReadLine();

        Console.Write("CPF do titular: ");
        string cpf = Utils.getCpf();

        string password, passwordConfirmation;
        do
        {
            Console.Write("Senha da conta (mín. 8 caractéres): ");
            password = Utils.getPassword();

            Console.Write("Confirme a senha da conta: ");
            passwordConfirmation = Utils.getPassword();

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
            depositValue = Utils.getDecimal();
        }

        AccountDomain account = new AccountDomain(cpf, holderName, password, depositValue);
        Repository.Create(account);

        Console.Clear();
        Console.WriteLine("Conta bancária criada com sucesso. Seguem dados da nova conta abaixo:");
        Console.WriteLine(account);

        Utils.Continue();
    }

    public void DeleteAccount()
    {
        Console.Clear();
        Console.WriteLine("Para encerrar a sua conta, forneça os dados a seguir: \n");

        AccountDomain account = GetAccount();

        if (account is not null)
        {
            Console.WriteLine("\nConfira abaixo os dados da conta a ser excluída");
            Console.WriteLine(account);
            Console.Write("\nTem certeza que deseja encerrar esta conta corrente? (S = Sim/N = Não): ");
            string confirmation = Console.ReadLine();
            
            if(confirmation.ToLower() == "s")
            {
                bool deletionResult = Repository.DeleteAccount(account);
                if (deletionResult == true)
                    Console.WriteLine("Conta encerrada com sucesso.");
                else
                    Console.WriteLine("Erro na operção de exclusão.");
            } else
            {
                Console.WriteLine("Operação de encerramento cancelada.");
            }
        }

        Utils.Continue();
    }

    public void ListAccounts()
    {
        Console.Clear();
        Console.WriteLine("LISTAGEM DE CONTAS ATIVAS:\n");

        foreach (AccountDomain account in Repository.FindAll())
            Console.WriteLine(account.OneLineInfo());

        Utils.Continue();
    }

    public void AccountDetail()
    {
        Console.Clear();
        
        AccountDomain account = GetAccount();
        if (account == null)
            return;

        Console.WriteLine(account);
        Utils.Continue();
    }

    public void GetBankTotalBalance()
    {
        (decimal Balance, int Accounts) total = Repository.GetBankTotalBalance();

        Console.Clear();
        Console.WriteLine("VALOR TOTAL ARMAZENADO NO BANCO:");
        Console.WriteLine($"No momento, o banco possui {total.Accounts} contas ativas e está armazenando R${total.Balance:F2}.");
        Utils.Continue();
    }

    public void ManipulateAccount()
    {
        uint inputValue = 100;
        bool inputValid = false;
        AccountDomain account = null;

        do
        {
            Console.Clear();

            if (account == null)
            {
                account = GetAccount();
                if (account == null)
                    return;

            } else
            {
                Console.Clear();
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

    public void MakeDeposit(AccountDomain account)
    {
        Console.Clear();

        Console.Write("Para efetuar a operação, forneça o valor a ser depositado: ");
        decimal depositValue = Utils.getDecimal();

        account.MakeDeposit(depositValue);
        Console.WriteLine("\nDepósito realizado com sucesso.");
        Console.WriteLine($"O saldo atual da sua conta é: R${account.Balance:F2}.");

        Utils.Continue();
    }

    public void MakeWithdraw(AccountDomain account)
    {
        Console.Clear();

        Console.Write("Para efetuar a operação, forneça o valor a ser sacado: ");
        decimal withdrawValue = Utils.getDecimal();

        if (account.Balance > withdrawValue)
        {
            account.MakeWithDraw(withdrawValue);
            Console.WriteLine("\nSaque realizado com sucesso.");
            Console.WriteLine($"O saldo atual da sua conta é: R${account.Balance:F2}.");
        }
        else
            Console.WriteLine("O valor solicitado para saque é superior ao saldo disponível na conta.");

        Utils.Continue();
    }

    public void MakeTransference(AccountDomain sourceAccount)
    {
        Console.Clear();

        Console.Write("Para efetuar a transferência, forneça o CPF da conta de destino: ");
        string destinationCpf = Console.ReadLine();

        AccountDomain destinationAccount = Repository.FindByCpf(destinationCpf);

        if (destinationAccount == null)
        {
            Console.WriteLine("\nConta não econtrada. Confira os dados fornecidos e tente novamente.");
            Utils.Continue();
            return;
        }

        Console.Write("Qual o valor a ser transferido: ");
        decimal transferValue = Utils.getDecimal();

        bool transferResult = TransferOperation(sourceAccount, destinationAccount, transferValue);
        if (transferResult)
        {
            Console.WriteLine("\nTransferência realizada com sucesso.");
            Console.WriteLine($"O saldo atual da sua conta é: R${sourceAccount.Balance:F2}.");
        } else
            Console.WriteLine("O saldo na conta é insuficiente para prosseguir com a transferência.");

        Utils.Continue();
    }
    #endregion

    public static AccountDomain GetAccount()
    {
        Console.WriteLine("Forneça os dados da sua conta abaixo para prosseguir:\n");

        Console.Write("CPF do titular: ");
        string cpf = Console.ReadLine();

        Console.Write("Senha da conta: ");
        string password = Utils.getPassword();

        AccountDomain account = Repository.FindByCpfAndPass(cpf, password);

        if (account == null)
        {
            Console.WriteLine("\nConta não econtrada. Confira os dados fornecidos e tente novamente.");
            Utils.Continue();
        }

        return account;
    }

    public bool TransferOperation(AccountDomain sourceAccount, AccountDomain destinationAccount, decimal value)
    {
        if (sourceAccount.Balance > value)
        {
            sourceAccount.MakeWithDraw(value);
            destinationAccount.MakeDeposit(value);
            return true;
        }
        else
            return false;
    }
}
