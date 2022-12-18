using ByteBank_v01.Model;

namespace ByteBank_v01;
public class Repository
{
    private List<Account> Accounts { get; set; } = new List<Account>();

    public Account Create(Account account)
    {
        Accounts.Add(account);
        JsonIO.JsonSerialize(Accounts);

        return account;
    }

    public List<Account> GetAccounts()
    {
        Accounts = JsonIO.JsonDesserialize();
        return Accounts;
    }

    public bool DeleteAccount(Account account)
    {
        bool result = Accounts.Remove(account);
        JsonIO.JsonSerialize(Accounts);

        return result;
    }

    public void UpdateAccount(Account changedAccount)
    {
        var repositoryAccount = Accounts
            .FirstOrDefault(acc => acc.Cpf == changedAccount.Cpf);

        repositoryAccount.Balance = changedAccount.Balance;
        JsonIO.JsonSerialize(Accounts);

    }
}

