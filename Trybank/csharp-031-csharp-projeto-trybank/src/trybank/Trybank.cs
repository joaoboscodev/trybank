namespace Trybank.Lib;

public class TrybankLib
{
    public bool Logged;
    public int loggedUser;

    //0 -> Número da conta
    //1 -> Agência
    //2 -> Senha
    //3 -> Saldo
    public int[,] Bank;
    public int registeredAccounts;
    private int maxAccounts = 50;

    public TrybankLib()
    {
        loggedUser = -99;
        registeredAccounts = 0;
        Logged = false;
        Bank = new int[maxAccounts, 4];
    }

    // 1. Construa a funcionalidade de cadastrar novas contas
    public void RegisterAccount(int number, int agency, int pass)
    {
        for (int i = 0; i < registeredAccounts; i++) {
            if (Bank[i, 0] == number && Bank[i, 1] == agency) {
                throw new ArgumentException("A conta já está sendo usada!");
            }
        }

        Bank[registeredAccounts, 0] = number;
        Bank[registeredAccounts, 1] = agency;
        Bank[registeredAccounts, 2] = pass;
        Bank[registeredAccounts, 3] = 0;
        
        registeredAccounts++;
    }

    // 2. Construa a funcionalidade de fazer Login
    public void Login(int number, int agency, int pass)
    {
        bool existAccount = false;

        if (Logged == true) {
            throw new AccessViolationException("Usuário já está logado");
        }

        for (int i = 0; i < registeredAccounts; i++) {
            if (Bank[i, 0] == number && Bank[i, 1] == agency) {
                existAccount = true;
                
                if (Bank[i, 2] == pass) {
                    Logged = true;
                    loggedUser = i;
                } else {
                    throw new ArgumentException("Senha incorreta");
                }
            }
        }

        if (existAccount == false) {
            throw new ArgumentException("Agência + Conta não encontrada");
        }
    }

    // 3. Construa a funcionalidade de fazer Logout
    public void Logout()
    {
        if (Logged == false) {
            throw new AccessViolationException("Usuário não está logado");
        } else if (Logged == true) {
            Logged = false;
            loggedUser = -99;
        }
    }

    // 4. Construa a funcionalidade de checar o saldo
    public int CheckBalance()
    {
        if (Logged == false) {
            throw new AccessViolationException("Usuário não está logado");
        }

        int Balance = Bank[loggedUser, 3];
        return Balance;
    }

    // 5. Construa a funcionalidade de depositar dinheiro
    public void Deposit(int value)
    {
        if (Logged == false) {
            throw new AccessViolationException("Usuário não está logado");
        }

        int totalBalance = Bank[loggedUser, 3];
        totalBalance = totalBalance + value;
        Bank[loggedUser, 3] = totalBalance;
    }

    // 6. Construa a funcionalidade de sacar dinheiro
    public void Withdraw(int value)
    {
        if (Logged == false) {
            throw new AccessViolationException("Usuário não está logado");
        }

        int totalBalance = Bank[loggedUser, 3];
        totalBalance = totalBalance - value;

        if (totalBalance == Math.Abs(totalBalance) * -1) {
            throw new InvalidOperationException("Saldo insuficiente");
        } else {
            Bank[loggedUser, 3] = totalBalance;
        }
    }

    // 7. Construa a funcionalidade de transferir dinheiro entre contas
    public void Transfer(int destinationNumber, int destinationAgency, int value)
    {
        if (Logged == false) {
            throw new AccessViolationException("Usuário não está logado");
        }

        int Balance = Bank[loggedUser, 3];

        if (Balance < value) {
            throw new InvalidOperationException("Saldo insuficiente");
        } else {
            Withdraw(value);
        }

        for (int i = 0; i < registeredAccounts; i++) {
            if (Bank[i, 0] == destinationNumber && Bank[i, 1] == destinationAgency) {
                Bank[i, 3] += value;
            }
        }
    }

   
}
