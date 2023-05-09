namespace BusinessLayer.Account
{
    public interface IAccountFacade
    {
        bool Login(string email, string password);

        bool Register(string firstName, string lastName, string email, string password);

        string GetToken(string email);

        void ForgotPassword(string email);

        bool GetUserByToken(string token);

        void SetNewPassword(string token, string password);
    }
}