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
using NGSIv2Plugin.Messages;
using NGSIv2Plugin.Operations;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;

namespace NGSIv2Plugin
{
    public class NGSIv2Client
    {
        public bool Initialized { get; private set; }
        public RestClient RestClient { get; private set; }

        public EntityCollection EntityCollection { get; private set; }
        public EntityContext EntityContext { get; private set; }
        public EntityIngestion EntityIngestion { get; private set; }
        public AttributeContext AttributeContext { get; private set; }

        internal EntryPoint EntryPoints { get; set; }

        public NGSIv2Client(string baseUrl)
        {
            Initialized = false;
            RestClient = new RestClient();
            RestClient.BaseUrl = new Uri(baseUrl);
            try
            {
                RetrieveEntryPoints();
            }
            catch(Exception e)
            {
                Console.WriteLine("Could not retrieve entry points for endpoint {0}. The reported error was: {1}. Please check if the remote end is "
                    + "running and connected to the network", baseUrl, e);
            }
        }

        private void RetrieveEntryPoints()
        {
            RestRequest request = new RestRequest();
            request.Resource = "/v2";
            var response = RestClient.Execute<EntryPoint>(request);
            EntryPoints = response.Data;
            EntityCollection = new EntityCollection(this, EntryPoints.Entities);
            EntityContext = new EntityContext(this, EntryPoints.Entities);
            EntityIngestion = new EntityIngestion(this, EntryPoints.Entities);
            AttributeContext = new AttributeContext(this, EntryPoints.Entities);
            Initialized = true;
        }

        public void SendRequest(RestRequest request, Action<RequestResponse> callback)
        {
            RestClient.ExecuteAsync(request, response =>
            {
                callback(new RequestResponse(response.StatusCode));
            });
        }

        public void SendRequest<T>(RestRequest request, Action<RequestResponse<T>> callback)
        {
            RestClient.ExecuteAsync(request, response =>
            {
                var data = Serializer.Deserialize(response.Content, typeof(T));
                callback(new RequestResponse<T>(response.StatusCode, (T)data));
            });
        }
        JavaScriptSerializer Serializer = new JavaScriptSerializer();
    }
}
