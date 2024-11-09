namespace ManufacturingManager.Core
{
    public class RegExValidation
    {
        public const string RegExInvalidCharacters = @"[^&<>]*$";
        public const string RegExInvalidCharactersMessage = "<>& are invalid Characters";
        public const string RegExValidEmail = @"^([a-zA-Z0-9_\-\.\']+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
    }
}
