using System.Threading;
using System.Threading.Tasks;

namespace SafeModel
{
    public interface ISafeProvider : IHasWorkingDirectory , ISafeProviderForNonExistingUser , ISafeProviderForExistingUser
    {
    }
}