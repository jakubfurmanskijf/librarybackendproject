using System.ServiceModel;

namespace Library.Api.Soap;

[ServiceContract]
public interface ILibrarySoapService
{
    [OperationContract] string Ping();
    [OperationContract] SoapBookDto? GetBookByIsbn(string isbn);
    [OperationContract] bool IsAvailable(string isbn);
}
