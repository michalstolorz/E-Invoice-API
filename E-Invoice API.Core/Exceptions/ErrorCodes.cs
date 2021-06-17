
namespace E_Invoice_API.Core.Exceptions
{
    public static class ErrorCodes
    {
        //Auth
        public static string PrivateKeyNotFound => "private_key_not_found";

        //Repository
        public static string PredicateCannotBeNull => "parameter_predicate_cannot_be_null";
        public static string EntitiestCannotBeNull => "parameter_entities_cannot_be_null";
        public static string IdCannotBeNullOrZero => "parameter_id_cannot_be_null_or_zero";
        public static string InvalidIdType => "invalid_id_type";
        public static string ObjectOfTypeNotFound => "object_of_type_not_found";

        //Validator
        public static string InvalidParameter => "invalid_parameter";

        //Service
        public static string InvoiceStatusWithGivenIdNotFound => "invoice_status_with_given_id_not_found";
        public static string NoConversionForGivenStatus => "no_conversion_for_given_status";
        public static string InvoiceStatusWithGivenUserIdNoFound => "invoice_status_with_given_user_id_not_found";
        public static string InvoiceStatusAlreadyExists => "invoice_status_already_exists";
        public static string MailSendingHasFailed => "mail_sending_has_failed";
    }
}
