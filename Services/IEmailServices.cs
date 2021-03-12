using ServiceRequests.Models;

namespace ServiceRequests.Services
{
    public interface IEmailServices
    {
        void SendEmail(ServiceModel serviceModel);
    }
}