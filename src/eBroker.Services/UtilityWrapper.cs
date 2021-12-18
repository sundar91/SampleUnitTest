using eBroker.Services.Interfaces;

namespace eBroker.Services
{
    public class UtilityWrapper : IUtilityWrapper
    {
        public  bool IsValidDuration(DateTime dateTime)
        {
            return Utility.IsValidDuration(dateTime);
        }
    }
}