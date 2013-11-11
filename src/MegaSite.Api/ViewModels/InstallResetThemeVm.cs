namespace MegaSite.Api.ViewModels
{
    public class InstallResetThemeVm
    {
        public string Password { get; set; }
        public bool ReinitializeDatabase { get; set; }
        public string Theme { get; set; }
    }
}