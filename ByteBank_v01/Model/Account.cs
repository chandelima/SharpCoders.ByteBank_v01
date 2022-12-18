using System.Text;

namespace ByteBank_v01.Model;

public class Account
{
    public string Cpf { get; set; }
    public string HolderName { get; set; }
    public string Password { get; set; }
    public decimal Balance { get; set; }

    public Account(string cpf, string holderName, string password, decimal balance = 0)
    {
        Cpf = cpf;
        HolderName = holderName;
        Password = password;
        Balance = balance;
    }

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

