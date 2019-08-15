using System.Threading.Tasks;

namespace SignalRExample.Api.SignalR
{
    public interface IChatHub
    {
        Task SayHello();
    }
}