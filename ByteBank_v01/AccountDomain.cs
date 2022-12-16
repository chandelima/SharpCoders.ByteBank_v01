using System.Text;

namespace ByteBank_v01;
public class AccountDomain
{
    public string Cpf { get; }
    public string HolderName { get; }
    public string Password { get; }
    public decimal Balance { get; private set; }

    public AccountDomain(string cpf, string holderName, string password, decimal balance = 0)
    {
        Cpf = cpf;
        HolderName = holderName;
        Password = password;
        Balance = balance;
    }

    public void MakeDeposit(decimal value) => Balance += value;

    public void MakeWithDraw(decimal value) => Balance -= value;

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"- Titular:     {HolderName}");
        sb.AppendLine($"- CPF:         {Utils.ShowFormattedCpf(Cpf)}");
        sb.AppendLine($"- Saldo Atual: R${Balance:F2}");

        return sb.ToString();
    }

    public string OneLineInfo() =>
        $"- Titular: {HolderName}  |  CPF: {Utils.ShowFormattedCpf(Cpf)}  |  Saldo Atual: R${Balance:F2}";
}

