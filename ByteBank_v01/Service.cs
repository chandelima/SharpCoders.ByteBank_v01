using ByteBank_v01.Model;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace ByteBank_v01;
public class Service
{
    Repository repository = new Repository();

    public string CreateAccount(Account account)
    {
        Account repositoryCheck = GetAccountByCpf(account.Cpf);
        
        if (repositoryCheck != null)
            return "Já existe uma conta para o CPF informado.";

        repository.Create(account);
        return "";
    }

    public List<Account> GetAllAccounts()
    {
        return repository.GetAccounts();
    }

    public Account GetAccountByCpf(string cpf)
    {
        List<Account> accounts = repository.GetAccounts();
        return accounts.FirstOrDefault(acc => acc.Cpf == cpf);
    }
    
    public Account GetAccountByCpfAndPass(string cpf, string password)
    {
        List<Account> accounts = repository.GetAccounts();
        return accounts
            .FirstOrDefault(acc => acc.Cpf == cpf && acc.Password == password);
    }

    public (decimal Balance, int Accounts) GetBankTotalBalance()
    {
        List<Account> accounts = repository.GetAccounts();

        decimal totalBalance = accounts.Sum(accs => accs.Balance);
        int totalAccounts = accounts.Count();

        return (totalBalance, totalAccounts);
    }

    public bool DeleteAccount(Account account)
    {
        return repository.DeleteAccount(account);
    }
    
    public Account MakeDeposit(Account account, decimal value)
    {
        account.Balance += value;
        repository.UpdateAccount(account);

        return account;
    }
        
    public Account MakeWithDraw(Account account, decimal value)
    {
        account.Balance -= value;
        repository.UpdateAccount(account);

        return account;
    }

    public string MakeTransfer(Account sourceAccount, Account destinationAccount, decimal value)
    {
        if(sourceAccount.Cpf == destinationAccount.Cpf)
            return "As contas de destino e origem são as mesmas.";


        if (sourceAccount.Balance < value)
            return "Não há saldo suficiente na conta para realizar a operação.";

        MakeWithDraw(sourceAccount, value);
        MakeDeposit(destinationAccount, value);

        return "";
    }
}
