using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using ServiceStack;
using ServiceStack.Web;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace MyApp
{

    public partial class ServiceStackClient
    {
        private class BlazorFakeHttpContext : HttpContext
        {
            public BlazorFakeHttpContext(ClaimsPrincipal user)
            {
                User = user;
            }

            public override IFeatureCollection Features { get; }
            public override HttpRequest Request { get; }
            public override HttpResponse Response { get; }
            public override Microsoft.AspNetCore.Http.ConnectionInfo Connection { get; }
            public override WebSocketManager WebSockets { get; }
            public override ClaimsPrincipal User { get; set; }
            public override IDictionary<object, object> Items { get; set; } = new Dictionary<object, object>();
            public override IServiceProvider RequestServices { get; set; }
            public override CancellationToken RequestAborted { get; set; }
            public override string TraceIdentifier { get; set; }
            public override ISession Session { get; set; }

            public override void Abort()
            {

            }
        }

        private class BlazorFakeHttpRequest : HttpRequest
        {
            public override HttpContext HttpContext { get; }

            public BlazorFakeHttpRequest(HttpContext httpContext)
            {
                HttpContext = httpContext;
            }

            public override string Method { get; set; }
            public override string Scheme { get; set; }
            public override bool IsHttps { get; set; }
            public override HostString Host { get; set; }
            public override PathString PathBase { get; set; }
            public override PathString Path { get; set; }
            public override QueryString QueryString { get; set; }
            public override IQueryCollection Query { get; set; }
            public override string Protocol { get; set; }
            public override IHeaderDictionary Headers { get; }
            public override IRequestCookieCollection Cookies { get; set; }
            public override long? ContentLength { get; set; }
            public override string ContentType { get; set; }
            public override Stream Body { get; set; }
            public override bool HasFormContentType { get; }
            public override IFormCollection Form { get; set; }

            public override Task<IFormCollection> ReadFormAsync(CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }
        }

        private class BlazorResponse : IResponse
        {
            public object OriginalResponse { get; }
            public IRequest Request { get; }
            public int StatusCode { get; set; }
            public string StatusDescription { get; set; }
            public string ContentType { get; set; }
            public Stream OutputStream { get; }
            public object Dto { get; set; }
            public bool UseBufferedStream { get; set; }
            public bool IsClosed { get; } = false;
            public bool KeepAlive { get; set; }
            public bool HasStarted { get; }
            public Dictionary<string, object> Items { get; } = new Dictionary<string, object>();

            public void AddHeader(string name, string value)
            {
                
            }

            public void Close()
            {
                
            }

            public Task CloseAsync(CancellationToken token = default)
            {
                return Task.CompletedTask;
            }

            public void End()
            {

            }

            public void Flush()
            {

            }

            public Task FlushAsync(CancellationToken token = default)
            {
                return Task.CompletedTask;
            }

            public string GetHeader(string name)
            {
                return null;
            }

            public void Redirect(string url)
            {

            }

            public void RemoveHeader(string name)
            {
              
            }

            public void SetContentLength(long contentLength)
            {
              
            }
        }

        private class BlazorRequest : IRequest
        {
            private readonly HttpRequest httpRequest;

            public BlazorRequest(HttpRequest httpRequest, string verb = "GET", object dto = null)
            {
                this.httpRequest = httpRequest;
                Verb = verb;
                Dto = dto;
            }

            public object OriginalRequest => httpRequest;

            public IResponse Response { get; } = new BlazorResponse();
            public string OperationName { get; set; }
            public string Verb { get; set; }
            public RequestAttributes RequestAttributes { get; set; }
            public IRequestPreferences RequestPreferences { get; }
            public object Dto { get; set; }
            public string ContentType { get; }
            public bool IsLocal { get; }
            public string UserAgent { get; }
            public IDictionary<string, Cookie> Cookies { get; } = new Dictionary<string, Cookie>();
            public string ResponseContentType { get; set; }
            public bool HasExplicitResponseContentType { get; }
            public Dictionary<string, object> Items { get; } = new Dictionary<string, object>();
            public NameValueCollection Headers { get; } = new NameValueCollection();
            public NameValueCollection QueryString { get; } = new NameValueCollection();
            public NameValueCollection FormData { get; } = new NameValueCollection();
            public bool UseBufferedStream { get; set; }
            public string RawUrl { get; }
            public string AbsoluteUri { get; }
            public string UserHostAddress { get; }
            public string RemoteIp { get; }
            public string Authorization { get; }
            public bool IsSecureConnection { get; }
            public string[] AcceptTypes { get; }
            public string PathInfo { get; }
            public string OriginalPathInfo { get; }
            public Stream InputStream { get; }
            public long ContentLength { get; }
            public IHttpFile[] Files { get; } = new IHttpFile[0];
            public Uri UrlReferrer { get; }

            public string GetRawBody()
            {
                throw new NotImplementedException();
            }

            public Task<string> GetRawBodyAsync()
            {
                throw new NotImplementedException();
            }

            public T TryResolve<T>()
            {
                return HostContext.TryResolve<T>();
            }
        }
    }
}
