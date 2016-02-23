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
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace NGSIv2Plugin
{
    class NGSIv2Client
    {
        public RestClient Client { get; private set; }
        internal EntryPoint EntryPoints { get; set; }

        public NGSIv2Client(string baseUrl)
        {
            Client = new RestClient();
            Client.BaseUrl = new Uri(baseUrl);
            RetrieveEntryPoints();
        }

        private void RetrieveEntryPoints()
        {
            RestRequest request = new RestRequest();
            request.Resource = "/v2";
            var response = Client.Execute<EntryPoint>(request);
            EntryPoints = response.Data;
        }

        public void ListAllEntities(Action<List<Dictionary<string, object>>> callback)
        {
            RestRequest request = new RestRequest();
            request.Resource = EntryPoints.Entities;
            Client.ExecuteAsync<List<Dictionary<string, object>>>(request, response =>
            {
                callback(response.Data);
            });
        }

        public void ListAllEntities(int offset, int limit, string options, Action<List<Dictionary<string, object>>> callback)
        {
            RestRequest request = new RestRequest(EntryPoints.Entities);
            request.AddParameter("offset", offset);
            request.AddParameter("limit", limit);
            request.AddParameter("options", options);
            Client.ExecuteAsync<List<Dictionary<string, object>>>(request, response =>
            {
                callback(response.Data);
            });
        }

        public void FilterEntitiesById(string id, Delegate callback){}
        public void FilterEntitiesById(ISet<string> ids, Delegate callback){}
        public void FilterEntitiesByType(string type, Delegate callback){}
        public void FilterEntitiesByType(ISet<string> types, Delegate callback){}
    }
}
