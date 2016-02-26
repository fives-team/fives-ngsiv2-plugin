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

namespace NGSIv2Plugin
{
    public class NGSIv2Client
    {
        public bool Initialized { get; private set; }
        public RestClient RestClient { get; private set; }
        public EntityCollection EntityCollection { get; private set; }

        internal EntryPoint EntryPoints { get; set; }

        public NGSIv2Client(string baseUrl)
        {
            Initialized = false;
            RestClient = new RestClient();
            RestClient.BaseUrl = new Uri(baseUrl);
            RetrieveEntryPoints();
        }

        private void RetrieveEntryPoints()
        {
            RestRequest request = new RestRequest();
            request.Resource = "/v2";
            var response = RestClient.Execute<EntryPoint>(request);
            EntryPoints = response.Data;
            EntityCollection = new EntityCollection(this, EntryPoints.Entities);
            Initialized = true;
        }

        public void SendRequest(RestRequest request, Action<RequestResponse> callback)
        {
            RestClient.ExecuteAsync<List<Dictionary<string, object>>>(request, response =>
            {
                callback(new RequestResponse(response.StatusCode, response.Data));
            });
        }
    }
}
