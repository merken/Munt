using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using BaGet.Protocol;
using NuGet.Versioning;
using System.IO;

namespace Munt.Functions.Models
{
    public class NuGetPackage
    {
        public string Name { get; set; }
        public Version Version { get; set; }
    }
}