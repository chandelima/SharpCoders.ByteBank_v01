namespace ByteBank_v01;
public static class Repository
{
    private static List<AccountDomain> Accounts { get; set; } = new List<AccountDomain>() {
        new AccountDomain("123456789", "Pedro Álvares Cabral", "12345678"),
        new AccountDomain("987654321", "Monteiro Lobato", "12345678")
    };

    public static AccountDomain Create(AccountDomain account)
    {
        Accounts.Add(account);
        return account;
    }

    public static IEnumerable<AccountDomain> FindAll()
    {
        return Accounts;
    }

    public static AccountDomain FindByCpf(string cpf)
    {
        AccountDomain account = Accounts
            .FirstOrDefault(acc => acc.Cpf == cpf);
        return account;
    }

    public static AccountDomain FindByCpfAndPass(string cpf, string password)
    {
        AccountDomain account = Accounts
            .Where(acc => acc.Cpf == cpf)
            .FirstOrDefault(acc => acc.Password == password);
        return account;
    }

    public static bool DeleteAccount(AccountDomain account)
    {
        return Accounts.Remove(account);
    }
    public static (decimal Balance, int Amount) GetBankTotalBalance()
    {
        decimal totalBalance = Accounts.Sum(accs => accs.Balance);
        int totalAccounts = Accounts.Count();
        return (totalBalance, totalAccounts);

    }
}

