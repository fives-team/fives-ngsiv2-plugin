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

namespace NGSIv2Plugin.Operations
{
    public class EntityIngestion : EntityOperation
    {
        public EntityIngestion(NGSIv2Client client, string resource) : base(client, resource) { }

        public void CreateEntity(EntityObject entity, Action<RequestResponse> callback, string options = null)
        {
            RestRequest request = new RestRequest(EntitiesResource, Method.POST);
            request.AddBody(entity);
            request.AddHeader("Content-Type", "application/json");
            if (options != null)
            {
                request.AddParameter("options", options);
            }
            Client.SendRequest(request, callback);
        }

        public void CreateEntityFromKeyValues(EntityObject entity, Action<RequestResponse> callback)
        {
            CreateEntity(entity, callback, "keyValues");
        }

        public void UpdateEntity
            (string entityId, EntityObject update, Action<RequestResponse> callback, string options = null)
        {
            RestRequest request = new RestRequest(EntitiesResource + "/" + entityId, Method.PATCH);
            request.AddHeader("Content-Type", "application/json");
            if (options != null)
            {
                request.AddParameter("options", options);
            }
            request.AddBody(update);
            Client.SendRequest(request, callback);
        }

        public void UpdateEntityFromKeyValues(string entityId, EntityObject update, Action<RequestResponse> callback)
        {
            UpdateEntity(entityId, update, callback, "keyValues");
        }

        public void UpdateOrAppendFromKeyValues(string entityId, EntityObject update, Action<RequestResponse> callback)
        {
            RestRequest request = new RestRequest(EntitiesResource + "/" + entityId, Method.PUT);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("options", "keyValues");
            request.AddBody(update);
            Client.SendRequest(request, callback);
        }

        public void RemoveEntity(string entityId, Action<RequestResponse> callback)
        {
            RestRequest request = new RestRequest(EntitiesResource + "/" + entityId, Method.DELETE);
            Client.SendRequest(request, callback);
        }
    }
}
