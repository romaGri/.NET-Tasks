using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Text;
namespace AsyncIO
{
    public static class Tasks
    {


        /// <summary>
        /// Returns the content of required uris.
        /// Method has to use the synchronous way and can be used to compare the performace of sync \ async approaches. 
        /// </summary>
        /// <param name="uris">Sequence of required uri</param>
        /// <returns>The sequence of downloaded url content</returns>
        public static IEnumerable<string> GetUrlContent(this IEnumerable<Uri> uris)
        {
            // TODO : Implement GetUrlContent

            WebClient client = new WebClient { Encoding = Encoding.UTF8 };
            IEnumerable<string> dnlad;
            //string d;
            //foreach (var uri in uris)
            //{
            //    client.Headers[HttpRequestHeader.AcceptEncoding] = "gzip";
            //    var responseStream = new GZipStream(client.OpenRead(uri), CompressionMode.Decompress);
            //    var reader = new StreamReader(responseStream);
            //    var textResponse = reader.ReadToEnd();

            //}

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            dnlad = uris.Select(uri => client.DownloadString(uri));
            return dnlad;

        }



        /// <summary>
        /// Returns the content of required uris.
        /// Method has to use the asynchronous way and can be used to compare the performace of sync \ async approaches. 
        /// 
        /// maxConcurrentStreams parameter should control the maximum of concurrent streams that are running at the same time (throttling). 
        /// </summary>
        /// <param name="uris">Sequence of required uri</param>
        /// <param name="maxConcurrentStreams">Max count of concurrent request streams</param>
        /// <returns>The sequence of downloaded url content</returns>
        public static IEnumerable<string> GetUrlContentAsync(this IEnumerable<Uri> uris, int maxConcurrentStreams)
        {
            // TODO : Implement GetUrlContentAsync
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            int index = 0;
            var task = new Task<string>[maxConcurrentStreams];

            foreach (var uri in uris)
            {
                if (index >= maxConcurrentStreams)
                {
                    var indexCompletedTasks = Task.WaitAny(task);
                    var completedTasks = task[indexCompletedTasks];
                    task[indexCompletedTasks] = GetOneUri(uri);
                    yield return completedTasks.Result;
                }
                else
                {
                    task[index] = GetOneUri(uri);
                    index++;
                }
            }

            var listTasks = task.ToList();
            while (!listTasks.Any())
            {
                var indexTask = Task.WaitAny(listTasks.ToArray());
                var resultTask = listTasks[indexTask];
                listTasks.RemoveAt(indexTask);
                yield return resultTask.Result;
            } 
        }
        private static Task<string> GetOneUri(Uri uri)
        {
            return new HttpClient().GetStringAsync(uri);
        }

        /// <summary>
        /// Calculates MD5 hash of required resource.
        /// 
        /// Method has to run asynchronous. 
        /// Resource can be any of type: http page, ftp file or local file.
        /// </summary>
        /// <param name="resource">Uri of resource</param>
        /// <returns>MD5 hash</returns>
        public static async Task<string> GetMD5Async(this Uri resource)
        {
            // TODO : Implement GetMD5Async
            // var data = MD5.Create().ComputeHash(await new WebClient().DownloadDataAsync(resource)).Select<byte, string>(x => x.ToString());
            //using (Stream stream = await new WebClient { Encoding = Encoding.UTF8 }.OpenReadTaskAsync(resource))
            //{
            //    var hashAlgoritm = MD5.Create();
            //    var hashValue = hashAlgoritm.ComputeHash(stream);
            //    return BitConverter.ToString(hashValue).Replace("-", string.Empty);
            //}
            var data = MD5.Create().ComputeHash(await new WebClient().DownloadDataTaskAsync(resource))
            .Select<byte, string>(x => x.ToString()).ToArray();
            return string.Concat(data);

            //using (Stream stream = await new WebClient { Encoding = Encoding.UTF8 }.OpenReadTaskAsync(resource))
            //{
            //    var hashAlgorithm = MD5.Create();
            //    var hashValue = hashAlgorithm.ComputeHash(stream);
            //    return BitConverter.ToString(hashValue).Replace("-", string.Empty);
            //}

        }

    }



}
