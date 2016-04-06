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
    public class AttributeContext : EntityOperation
    {
        public AttributeContext(NGSIv2Client client, string resource) : base(client, resource) { }

        public void ListAllAttributes(string entityId, Action<RequestResponse<EntityObject>> callback)
        {
            string path = this.EntitiesResource + "/" + entityId + "/attrs";
            RestRequest request = new RestRequest(path, Method.GET);
            Client.SendRequest<EntityObject>(request, callback);
        }

        public void ReplaceAllAttributes(string entityId, EntityObject body, Action<RequestResponse> callback)
        {
            string path = EntitiesResource + "/" + entityId + "/";
            RestRequest request = new RestRequest(path, Method.PUT);
            request.AddBody(body);
            Client.SendRequest(request, callback);
        }

        public void GetAttributeWithMetadata
            (string entityId, string attributeName, Action<RequestResponse<AttributeObject>> callback)
        {
            string path = EntitiesResource + "/" + entityId + "/attrs/" + attributeName;
            RestRequest request = new RestRequest(path, Method.GET);
            Client.SendRequest<AttributeObject>(request, callback);
        }

        public void GetAttributeValueAsText
            (string entityId, string attributeName, Action<RequestResponse<string>> callback)
        {
            GetAttributeValue<string>(entityId, attributeName, "text/plain", callback);
        }

        public void GetAttributeValueAsJSON
            (string entityId, string attributeName, Action<RequestResponse<AttributeValue>> callback)
        {
            GetAttributeValue<AttributeValue>(entityId, attributeName, "application/json", callback);
        }

        private void GetAttributeValue<T>
            (string entityId, string attributeName, string contentType, Action<RequestResponse<T>> callback)
        {
            string path = EntitiesResource + "/" + entityId + "/attrs/" + attributeName + "/value";
            RestRequest request = new RestRequest(path, Method.GET);
            request.AddHeader("Content-Type", contentType);
            Client.SendRequest<T>(request, callback);
        }

        public void UpdateAttribute() { }
        public void UpdateAttributeValue() { }
        public void RemoveAttribute() { }
    }
}
