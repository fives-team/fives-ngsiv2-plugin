// This file is part of FiVES.
//
// FiVES is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation (LGPL v3)
//
// FiVES is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with FiVES.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using RestSharp;

namespace NGSIv2Plugin
{
    public class AsyncRESTClient
    {

        /*Docs http://restsharp.org/ ,http://stackoverflow.com/questions/10226089/restsharp-simple-complete-example, http://pawel.sawicz.eu/async-and-restsharp/, 
         http://stackoverflow.com/questions/31526400/restsharp-send-dictionary
         
         */

        /// <summary>
        /// Performs an (asynchronous) REST request to an HTTP resource and returns the result as type that
        /// was specified as Template Parameter
        /// </summary>
        /// <typeparam name="T">The type to which the request result should be casted</typeparam>
        /// <param name="uri">Location of the resource that should be requested</param>
        /// <returns>Object of specified parameter type when async task has finished</returns>
        public T Get<T>(string uri, string content) where T : class, new()
        {
            client = new RestClient(uri);
            T responseObject=null;
            var request = new RestRequest(content,Method.GET);
            client.ExecuteAsync<T>(request, response =>
            {
                if (response.ResponseStatus == ResponseStatus.Completed)
                {
                    responseObject = response.Data;
                }
               
            });
            return responseObject;
        }

        public T Post<T>(string uri, Dictionary<string, string> content) where T : class, new()
        {
            T responseObject = null;
            var client = new RestClient(uri);
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(content);                   
            var asyncHandle = client.ExecuteAsync<T>(request, response =>
            {
                if (response.ResponseStatus == ResponseStatus.Completed)
                {
                    responseObject = response.Data;
                }
            });
            return responseObject;
        }

        public async Task Put(string uri, object content)
        {
            throw new NotImplementedException();
        }

        public async Task Patch(string uri, object content)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(string uri)
        {
            throw new NotImplementedException();
        }

        private RestClient client;        
    }
}
