using System;
using System.Collections.Generic;
using System.Text;

namespace Papyrus.Core.SDK.Interfaces
{
    public interface IPluginInfo
    {
        string Name { get; }
        string Description { get; }
        string Version { get; }
        string WebSite { get; }
        string SourceCode { get; }
    }
}
