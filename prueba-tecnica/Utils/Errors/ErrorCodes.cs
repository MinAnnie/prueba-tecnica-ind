namespace prueba_tecnica.Utils.Errors;

public static class ErrorCodes
{
    public static class ProductErrors
    {
        public const string ProductNotFound = "PRODUCT_NOT_FOUND";
        public const string ProductCreationFailed = "PRODUCT_CREATION_FAILED";
        public const string ProductUpdateFailed = "PRODUCT_UPDATE_FAILED";
        public const string ProductDeleteFailed = "PRODUCT_DELETE_FAILED";
    }

    public static class GeneralErrors
    {
        public const string InternalServerError = "INTERNAL_SERVER_ERROR";
        public const string InvalidRequest = "INVALID_REQUEST";
    }
}