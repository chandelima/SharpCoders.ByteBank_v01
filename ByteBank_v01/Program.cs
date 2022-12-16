using ByteBank_v01;

CLI cli = new CLI();
uint option;

do
{
    option = cli.Startup();

    switch(option)
    {
        case 1:
            cli.CreateAccount();
            break;
        case 2:
            cli.DeleteAccount();
            break;
        case 3:
            cli.ListAccounts();
            break;
        case 4:
            cli.AccountDetail();
            break;
        case 5:
            cli.GetBankTotalBalance();
            break;
        case 6:
            cli.ManipulateAccount();
            break;
    }
} while (option != 0);