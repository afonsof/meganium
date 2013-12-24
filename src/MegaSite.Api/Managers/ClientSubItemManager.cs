namespace MegaSite.Api.Managers
{
    public class ClientSubItemManager
    {
        private readonly UnitOfWork _uow;

        public ClientSubItemManager(UnitOfWork uow)
        {
            _uow = uow;
        }
    }
}