namespace CaseOpener.Core.Constants
{
    public static class ReturnMessages
    {
        public const string Required = "The field {0} is required!";
        public const string StringLength = "The field {0} is between {2} and {1} characters long!";
        public const string OperationFailed = "Operation failed! Try again later.";

        public const string SuccessfullyAdded = "Successfully added new {0}!";
        public const string SuccessfullyEdited = "Successfully edited {0}!";
        public const string SuccessfullyDeleted = "Successfully deleted {0}!";
        public const string SuccessfullyUpdated = "Successfully updated {0}!";

        public const string SuccessfullyLoggedIn = "User successfully logged in!";
        public const string SuccessfullyRegistered = "You was successfully registered!";
        public const string Unauthorized = "Unauthorized!";

        public const string SuccessfullyModifiedBalance = "Successfully {0} balance!";
        public const string CannotModifyBalance = "Can not decrease balance!";

        public const string SuccessfullyClaimedDailyReward = "Successfully claimed daily reward!";
        public const string CannotClaimDailyReward = "You can't claim daily reward!";

        public const string DoesntExist = "{0} doesn't exist!";
        public const string AlreadyExist = "{0} already exists!";

        public const string InvalidModel = "Invalid model!";
        public const string InvalidPassword = "Invalid password!";
    }
}
