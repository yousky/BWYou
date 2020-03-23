using log4net;
using System;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BWYou.Web.MVC.Etc
{
    /// <summary>
    /// WebAPI 로그 등으로 사용하기 위한 메세지 핸들러
    /// </summary>
    public abstract class MessageHandler : DelegatingHandler
    {
        public ILog logger = LogManager.GetLogger(typeof(MessageHandler));

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var corrId = string.Format("{0}{1}", DateTime.UtcNow.Ticks, Thread.CurrentThread.ManagedThreadId);
            var requestInfo = string.Format("{0} {1}", request.Method, request.RequestUri);
            try
            {
                var requestMessage = request.Content.Headers.ContentLength < 1024 ?
                                            await request.Content.ReadAsByteArrayAsync()
                                            : Encoding.UTF8.GetBytes(string.Format("ContentLength = {0}", request.Content.Headers.ContentLength));    //1k 넘으면 내용 출력 안 하도록 하기.

                await IncommingMessageAsync(corrId, requestInfo, requestMessage);

                var response = await base.SendAsync(request, cancellationToken);

                byte[] responseMessage;

                if (response.IsSuccessStatusCode)
                    responseMessage = (response.Content.Headers.ContentLength == null || response.Content.Headers.ContentLength < 1024) ?
                                            await response.Content.ReadAsByteArrayAsync()
                                            : Encoding.UTF8.GetBytes(string.Format("ContentLength = {0}", response.Content.Headers.ContentLength));    //1k 넘으면 내용 출력 안 하도록 하기. 이걸로 크기 알 수 없음.. ~_~; 결국 적용 안 됨.
                else
                    responseMessage = Encoding.UTF8.GetBytes(response.ReasonPhrase != null ? response.ReasonPhrase : "");

                await OutgoingMessageAsync(corrId, requestInfo, responseMessage);

                return response;
            }
            catch (OperationCanceledException ex)
            {
                var message = string.Format("{0} - Request: {1}\r\n OperationCanceledException : {2}", corrId, requestInfo, ex.Message);

                logger.Info(message);
                throw ex;
            }
            catch (Exception ex)
            {
                var message = string.Format(CultureInfo.InvariantCulture,
                            "Critical Log Exception occured {0} \r\n {1}",
                            ex.Message,
                            ex.StackTrace);

                logger.Error(message, ex);
                throw ex;
            }
        }


        protected abstract Task IncommingMessageAsync(string correlationId, string requestInfo, byte[] message);
        protected abstract Task OutgoingMessageAsync(string correlationId, string requestInfo, byte[] message);
    }



    public class MessageLoggingHandler : MessageHandler
    {
        public new ILog logger = LogManager.GetLogger(typeof(MessageLoggingHandler));

        protected override async Task IncommingMessageAsync(string correlationId, string requestInfo, byte[] message)
        {
            await Task.Run(() =>
                logger.Debug(string.Format("{0} - Request: {1}\r\n requestMessage : {2}", correlationId, requestInfo, Encoding.UTF8.GetString(message))));
        }


        protected override async Task OutgoingMessageAsync(string correlationId, string requestInfo, byte[] message)
        {
            await Task.Run(() =>
                logger.Debug(string.Format("{0} - Response: {1}\r\n responseMessage : {2}", correlationId, requestInfo, Encoding.UTF8.GetString(message))));
        }
    }
}
