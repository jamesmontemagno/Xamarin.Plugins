using System;
using System.Threading.Tasks;

namespace Share.Plugin.Abstractions
{
    /// <summary>
    /// Interface for Share
    /// </summary>
    public interface IShare
    {
        Task Share(string text, string title = null);
        Task OpenBrowser(string url);

    }
}
