using System.Threading.Tasks;
using System.Collections.Generic;
using Munt.Functions.Models;

namespace Munt.Functions
{
    public interface INugetServerService
    {
        Task<IEnumerable<NuGetPackage>> GetPackageVersions(string packageId);
        Task<byte[]> DownloadPackage(string packageId, string version);
    }   
}