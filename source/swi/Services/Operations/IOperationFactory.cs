using swi.Models.Operations;
namespace swi.Services.Operations;

public interface IOperationFactory
{
    Operation Create(OperationDto dto);
}