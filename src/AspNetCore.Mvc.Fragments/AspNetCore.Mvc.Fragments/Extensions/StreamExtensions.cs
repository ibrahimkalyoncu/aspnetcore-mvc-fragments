using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCore.Mvc.Fragments.Extensions
{
    internal static class StreamExtensions
    {
        internal static async Task WriteAsync(this Stream stream, string content)
        {
            var bytes = Encoding.UTF8.GetBytes(content);
            await stream.WriteAsync(bytes, 0, bytes.Length);
        }
    }
}
