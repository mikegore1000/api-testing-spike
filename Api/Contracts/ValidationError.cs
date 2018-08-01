namespace Api.Contracts
{
    public class ValidationError
    {
        public string ParameterName { get; }

        public string ErrorCode { get; }

        public string ErrorMessage { get; set; }

        public ValidationError(string parameterName, string errorCode, string errorMessage)
        {
            ParameterName = parameterName;
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }
    }
}