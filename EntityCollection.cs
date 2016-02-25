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
using System.Text;
using System.Threading.Tasks;

namespace NGSIv2Plugin
{
    public class EntityCollection
    {
        private RestClient Client { get; set; }
        private string EntitiesResource { get; set; }

        public EntityCollection(RestClient client, string resource)
        {
            Client = client;
            EntitiesResource = resource;
        }

        public void ListAllEntities(Action<RequestResponse> callback)
        {
            SendRequest(new RestRequest(EntitiesResource), callback);
        }

        public void ListAllEntities(int offset, int limit, Action<RequestResponse> callback, string options = null)
        {
            RestRequest request = new RestRequest(EntitiesResource);
            request.AddParameter("offset", offset);
            request.AddParameter("limit", limit);
            if (options != null)
                request.AddParameter("options", options);
            SendRequest(request, callback);
        }

        public void FilterEntitiesById(string id, Action<RequestResponse> callback, string options = null)
        {
            RestRequest request = new RestRequest(EntitiesResource);
            request.AddParameter("id", id);
            if (options != null)
                request.AddParameter("options", options);
            SendRequest(request, callback);
        }

        public void FilterEntitiesById(ISet<string> ids, Action<RequestResponse> callback, string options = null)
        {
            FilterEntitiesById(string.Join(",", ids), callback, options);
        }

        public void FilterEntitiesByType(string type, Action<RequestResponse> callback, string options = null)
        {
            RestRequest request = new RestRequest(EntitiesResource);
            request.AddParameter("type", type);
            if (options != null)
                request.AddParameter("options", options);
            SendRequest(request, callback);
        }

        public void FilterEntitiesByType(ISet<string> types, Action<RequestResponse> callback, string options = null)
        {
            FilterEntitiesByType(string.Join(",", types), callback, options);
        }

        private void SendRequest(RestRequest request, Action<RequestResponse> callback)
        {
            Client.ExecuteAsync<List<Dictionary<string, object>>>(request, response =>
            {
                callback(new RequestResponse(response.StatusCode, response.Data));
            });
        }
    }
}
