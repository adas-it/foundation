using Adasit.Foundation.Domain.ValuesObjects;

namespace Adasit.Foundation.Domain;

public record CommonErrorCodes
{
    public static readonly DomainErrorCode General = DomainErrorCode.New(1_000);
    public static readonly DomainErrorCode Internal = DomainErrorCode.New(1_001);
    public static readonly DomainErrorCode Validation = DomainErrorCode.New(1_002);
}
