using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AspNetCore.Mvc.Fragments.Context;
using AspNetCore.Mvc.Fragments.Filters;
using AspNetCore.Mvc.Fragments.Log;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.Mvc.Fragments.Streams
{
    public class FragmentResponseStream : Stream
    {
        public Stream BodyStream { get; }
        public int FlusIndex { get; private set; }
        private readonly List<FragmentResponseFilter> _responseFilters;
        private readonly IFragmentContextProvider _contextProvider;
        private readonly IFragmentLogger _logger;

        public FragmentResponseStream(HttpContext context, List<FragmentResponseFilter> responseFilters, bool isGzipEnabled)
        {
            _responseFilters = responseFilters;
            _contextProvider = context.RequestServices.GetService<IFragmentContextProvider>();
            _logger = context.RequestServices.GetService<IFragmentLogger>();

            if (isGzipEnabled)
            {
                BodyStream = new GZipStream(context.Response.Body, CompressionLevel.Optimal);
                context.Response.Headers.Add("Content-Encoding", "gzip");
            }
            else
            {
                BodyStream = context.Response.Body;
            }
        }

        public override void Flush()
        {
            FlusIndex++;
            BodyStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return BodyStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return BodyStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            BodyStream.SetLength(value);
        }

        public override async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            if (_contextProvider.GetContexts().Count > 0)
            {
                FragmentResponseFilterContext filterContext = new FragmentResponseFilterContext
                {
                    ResponseHtml = Encoding.Default.GetString(buffer.Take(count).ToArray()),
                    FlushIndex = FlusIndex,
                    BodyStream = BodyStream
                };

                _responseFilters?.ForEach(f => f.OnRendering(filterContext));

                byte[] responseBytes = Encoding.Default.GetBytes(filterContext.ResponseHtml);
                _logger.Info($"Chunk size : {responseBytes.Length}");
                await BodyStream.WriteAsync(responseBytes, offset, responseBytes.Length, cancellationToken);
                await FlushAsync(cancellationToken);
                _responseFilters?.ForEach(f => f.OnRendered(filterContext));
            }
            else
            {
                await BodyStream.WriteAsync(buffer, offset, count, cancellationToken);
            }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            BodyStream.Write(buffer, offset, count);
        }

        public override void Close()
        {
            _responseFilters?.ForEach(f => f.OnComplete());
            base.Close();
        }

        public override bool CanRead => BodyStream.CanRead;

        public override bool CanSeek => BodyStream.CanSeek;

        public override bool CanWrite => BodyStream.CanWrite;

        public override long Length => BodyStream.Length;

        public override long Position
        {
            get => BodyStream.Position;
            set => BodyStream.Position = value;
        }
    }
}
