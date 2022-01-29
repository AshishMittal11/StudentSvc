using System.Threading.Tasks;

namespace StudentSvc.Api.Azure
{
    public interface IProcessData
    {
        Task Process(MyPayload myPayload);
    }
}
