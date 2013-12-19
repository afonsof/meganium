namespace MegaSite.Api.Managers
{
    public class LicenseManager
    {
        public IOptions GetOptions()
        {
            return Options.Instance;
        }
    }
}