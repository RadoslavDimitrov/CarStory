namespace CarStory.Infrastructure
{
    public class ErrorMessageConstants
    {
        public const string WrongUsarnameOrPassword = "Wrong usarname or password!";

        public const string ShopNotApproved = "Shop is not approved!";

        //vin numbeer already exist
        public const string AlreadyExistCar = "AlreadyExistCar";
        public const string AlreadyExistCarMsg = "Car with that Vin Number already exist!";

        //view car with wrong Id
        public const string CarDoesNotExist = "CarDoesNotExist";
        public const string CarDoesNotExistMsg = "Car does not exist in out database!";

        //edit repair with wrong repairId
        public const string RepairDoesNotExistMsg = "Repair does not exist in our Database";

        //Car has pending repair
        public const string CarHasPendingRepair = $"Car already has one pending repair, pleace finish it, or call customer support for help";
    }
}
