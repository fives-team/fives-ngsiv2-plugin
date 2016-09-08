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
    /// <summary>
    /// Implements all methods that are defined to perform in Attribute context, i.e. complete collection of attributes,
    /// with meta data, or single attributes or attribute values in text or JSON format based on
    /// types or attributes as documented at http://telefonicaid.github.io/fiware-orion/api/v2/cookbook/
    /// </summary>
    public class AttributeContext : EntityOperation
    {
        public AttributeContext(NGSIv2Client client, string resource) : base(client, resource) { }

        /// <summary>
        /// Lists the complete collection of attributes
        /// </summary>
        /// <param name="entityId">Id of the entity of which the attribute list should be retrieved</param>
        /// <param name="callback">Callback that should be invoked when the request returned</param>
        public void ListAllAttributes(string entityId, Action<RequestResponse<EntityObject>> callback)
        {
            string path = this.EntitiesResource + "/" + entityId + "/attrs";
            RestRequest request = new RestRequest(path, Method.GET);
            Client.SendRequest<EntityObject>(request, callback);
        }

        /// <summary>
        /// Replace the all attributes that are attached to an entity by a new set
        /// </summary>
        /// <param name="entityId">Id of the entity of which attributes should be replaced</param>
        /// <param name="newAttributes">New set of attributes by which the existing ones should be replaced</param>
        /// <param name="callback">Callback that should be invoked when the request returned</param>
        public void ReplaceAllAttributes(string entityId, EntityObject newAttributes, Action<RequestResponse> callback)
        {
            string path = EntitiesResource + "/" + entityId + "/";
            RestRequest request = new RestRequest(path, Method.PUT);
            request.AddBody(newAttributes);
            Client.SendRequest(request, callback);
        }

        /// <summary>
        /// Retrieve a single attribute of an entity along with its metadata
        /// </summary>
        /// <param name="entityId">Id of the entity of which the attribute should be retrieved</param>
        /// <param name="attributeName">Name of the attribute that should be retrieved</param>
        /// <param name="callback">Callback that should be invoked when the request returned</param>
        public void GetAttributeWithMetadata
            (string entityId, string attributeName, Action<RequestResponse<AttributeObject>> callback)
        {
            string path = EntitiesResource + "/" + entityId + "/attrs/" + attributeName;
            RestRequest request = new RestRequest(path, Method.GET);
            Client.SendRequest<AttributeObject>(request, callback);
        }

        /// <summary>
        /// Retrieves the value of an attribute in text format
        /// </summary>
        /// <param name="entityId">Id of the entity of which the attribute should be retrieved</param>
        /// <param name="attributeName">Name of the attribute that should be retrieved</param>
        /// <param name="callback">Callback that should be invoked when the request returned</param>
        public void GetAttributeValueAsText
            (string entityId, string attributeName, Action<RequestResponse<string>> callback)
        {
            GetAttributeValue<string>(entityId, attributeName, "text/plain", callback);
        }

        /// <summary>
        /// Retrieves the value of an attribute in JSON format
        /// </summary>
        /// <param name="entityId">Id of the entity of which the attribute should be retrieved</param>
        /// <param name="attributeName">Name of the attribute that should be retrieved</param>
        /// <param name="callback">Callback that should be invoked when the request returned</param>
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

        /// <summary>
        /// Updates an attribute of an entity along with its metadata
        /// </summary>
        /// <param name="entityId">Id of the entity of which the attribute should be updated</param>
        /// <param name="attributeName">Name of the attribute that should be updated</param>
        /// <param name="newAttribute">New attribute data</param>
        /// <param name="callback">Callback that should be invoked when the request returned</param>
        public void UpdateAttribute
            (string entityId, string attributeName, AttributeObject newAttribute, Action<RequestResponse> callback)
        {
            string path = EntitiesResource + "/" + entityId + "/attrs/" + attributeName;
            RestRequest request = new RestRequest(path, Method.PUT);
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(newAttribute);
            Client.SendRequest(request, callback);
        }

        /// <summary>
        /// Updates an value of an attribute in text format
        /// </summary>
        /// <param name="entityId">Id of the entity of which the attribute should be updated</param>
        /// <param name="attributeName">Name of the attribute that should be updated</param>
        /// <param name="newValue">New value of the attribute in text format</param>
        /// <param name="callback">Callback that should be invoked when the request returned</param>
        public void UpdateAttributeValueAsText
            (string entityId, string attributeName, string newValue, Action<RequestResponse> callback)
        {
            UpdateAttributeValue(entityId, attributeName, "text/plain", newValue, callback);
        }

        /// <summary>
        /// Updates an value of an attribute in JSON format
        /// </summary>
        /// <param name="entityId">Id of the entity of which the attribute should be updated</param>
        /// <param name="attributeName">Name of the attribute that should be updated</param>
        /// <param name="newValue">New value of the attribute in text format</param>
        /// <param name="callback">Callback that should be invoked when the request returned</param>
        public void UpdateAttributValueAsJSON
            (string entityId, string attributeName, object newValue, Action<RequestResponse> callback)
        {
            AttributeValue v = new AttributeValue(newValue);
            UpdateAttributeValue(entityId, attributeName, "application/json", v, callback);
        }

        private void UpdateAttributeValue
            (string entityId, string attributeName, string contentType, object value, Action<RequestResponse> callback)
        {
            string path = EntitiesResource + "/" + entityId + "/attrs/" + attributeName + "/value";
            RestRequest request = new RestRequest(path, Method.PUT);
            request.AddBody(value);
            request.AddHeader("Content-Type", contentType);
            Client.SendRequest(request, callback);
        }

        public void UpdateOrAppendAttributes(string entityId, EntityObject update, Action<RequestResponse> callback, string type = null, string options = null)
        {
            string path = BuildUpdatePath(entityId, type, options);
            RestRequest request = new RestRequest(path, Method.POST);
            request.AddParameter("application/json", request.JsonSerializer.Serialize(update), ParameterType.RequestBody);
            Client.SendRequest(request, callback);
        }

        private string BuildUpdatePath(string entityId, string type, string options)
        {
            string path = EntitiesResource + "/" + entityId + "/attrs";
            if (type != null)
            {
                path = path + "?type=" + type;
            }
            if (options != null)
            {
                path = path +
                    type == null ? "?" : "&";
                path = path + "options=" + options;
            }
            return path;
        }
        /// <summary>
        /// Removes an attribute from an entity
        /// </summary>
        /// <param name="entityId">Entity from which attribute should be removed</param>
        /// <param name="attributeName">Name of the attribute that should be removed</param>
        /// <param name="callback">Callback that should be invoked when the request returned</param>
        public void RemoveAttribute(string entityId, string attributeName, Action<RequestResponse> callback)
        {
            string path = EntitiesResource + "/" + entityId + "/attrs/" + attributeName;
            RestRequest request = new RestRequest(path, Method.DELETE);
            Client.SendRequest(request, callback);
        }
    }
}
