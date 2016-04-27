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
    /// Implements operations to create content in Entity Context, i.e. create new entities, or create new attributes
    /// within entities
    /// </summary>
    public class EntityIngestion : EntityOperation
    {
        public EntityIngestion(NGSIv2Client client, string resource) : base(client, resource) { }

        /// <summary>
        /// Creates a new entity from a provided entity object
        /// </summary>
        /// <param name="entity">Entity that should be provided on NGSIv2 counterpart</param>
        /// <param name="callback">Function that is called after the request response has returned</param>
        /// <param name="options">Optional parameters according to NGSIv2 spec</param>
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

        /// <summary>
        /// Creates a new entity from a provided entity object in key values notation
        /// </summary>
        /// <param name="entity">Entity that should be provided on NGSIv2 counterpart</param>
        /// <param name="callback">Function that is called after the request response has returned</param>
        public void CreateEntityFromKeyValues(EntityObject entity, Action<RequestResponse> callback)
        {
            CreateEntity(entity, callback, "keyValues");
        }

        /// <summary>
        /// Updates an existing entity by appending new attributes to an existing entity
        /// </summary>
        /// <param name="entityId">ID of the entity that should be updated</param>
        /// <param name="update">Entity data that should be appended to an existing entity</param>
        /// <param name="callback">Function that is called after the request response has returned</param>
        /// <param name="options">Optional parameters according to NGSIv2 spec</param>
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

        /// <summary>
        /// Updates an existing entity by appending new attributes to an existing entity in key value notation
        /// </summary>
        /// <param name="entityId">ID of the entity that should be updated</param>
        /// <param name="update">Entity data as key values that should be appended to an existing entity</param>
        /// <param name="callback">Function that is called after the request response has returned</param>
        public void UpdateEntityFromKeyValues(string entityId, EntityObject update, Action<RequestResponse> callback)
        {
            UpdateEntity(entityId, update, callback, "keyValues");
        }

        /// <summary>
        /// Updates an existing entity by appending new attributes to an existing entity in key value notation.
        /// Creates an entirely new set of attributes if needed
        /// </summary>
        /// <param name="entityId">ID of the entity that should be updated</param>
        /// <param name="update">Entity data as key values that should be appended to an existing entity</param>
        /// <param name="callback">Function that is called after the request response has returned</param>
        public void UpdateOrAppendFromKeyValues(string entityId, EntityObject update, Action<RequestResponse> callback)
        {
            RestRequest request = new RestRequest(EntitiesResource + "/" + entityId, Method.PUT);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("options", "keyValues");
            request.AddBody(update);
            Client.SendRequest(request, callback);
        }

        /// <summary>
        /// Removes an entity
        /// </summary>
        /// <param name="entityId">ID of the entity that should be removed</param>
        /// <param name="callback">Function that is called when the response of the request has returned</param>
        public void RemoveEntity(string entityId, Action<RequestResponse> callback)
        {
            RestRequest request = new RestRequest(EntitiesResource + "/" + entityId, Method.DELETE);
            Client.SendRequest(request, callback);
        }
    }
}
