using System.Text;

namespace ByteBank_v01.Representation
{
    internal class Account
    {
        public int Number { get; }
        public string HolderName { get; }
        public decimal Balance { get; private set; } = 0;


        public Account(int number, string holderName, decimal initialDeposit = 0)
        {
            Number = number;
            HolderName = holderName;

            if (initialDeposit != 0)
                Deposit(initialDeposit);
        }


        public void Deposit(decimal amount) => Balance += amount;

        public void Withdraw(decimal amount) => Balance -= amount;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"Nome do Cliente: {HolderName}");
            sb.Append($"Número da Conta: {Number}");
            sb.Append($"Saldo: {Balance}");

            return sb.ToString();
        }
    }
}
