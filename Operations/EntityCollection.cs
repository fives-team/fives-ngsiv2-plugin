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
    /// Implements all methods that are defined to query the Collection of all Entities, a filtered, and a paginated version from the
    /// remote end as documented at http://telefonicaid.github.io/fiware-orion/api/v2/cookbook/
    /// </summary>
    public class EntityCollection : EntityOperation
    {
        private NGSIv2Client Client { get; set; }
        private string EntitiesResource { get; set; }

        /// <summary>
        /// Constructor for a new entity collection object in the context of a previously created NGSIv2 client.
        /// </summary>
        /// <param name="client">RestClient that is used to communicate with the NGSIv2 counterpart</param>
        /// <param name="resource">Resource base URL from which the entity collection is queried</param>
        public EntityCollection(NGSIv2Client client, string resource) : base(client, resource) { }

        /// <summary>
        /// Performs an asynchronous query to list the complete set of entities that is provided by the data provider
        /// </summary>
        /// <param name="callback">Function that is called when the asynchronous call returned</param>
        public void ListAllEntities(Action<RequestResponse<EntityList>> callback)
        {
            Client.SendRequest<EntityList>(new RestRequest(EntitiesResource), callback);
        }

        /// <summary>
        /// Performs an asynchronous request to retrieve a paginated subset of the entity collection
        /// </summary>
        /// <param name="offset">Offset from where entities should be listed</param>
        /// <param name="limit">Number of entities to be shown</param>
        /// <param name="callback">Function that is called when the asynchronous call returned</param>
        /// <param name="options">[optional] options with which the request should be performed</param>
        public void ListAllEntities(int offset, int limit, Action<RequestResponse<EntityList>> callback, string options = null)
        {
            RestRequest request = new RestRequest(EntitiesResource);
            request.AddParameter("offset", offset);
            request.AddParameter("limit", limit);
            if (options != null)
                request.AddParameter("options", options);
            Client.SendRequest<EntityList>(request, callback);
        }

        /// <summary>
        /// Performs an asynchronous request to retrieve a set of entities, filtered by Id
        /// </summary>
        /// <param name="id">ID by with the set of all entities should be filtered</param>
        /// <param name="callback">Function that is called when the asynchronous call returned</param>
        /// <param name="options">[optional] options with which the request should be performed</param>
        public void FilterEntitiesById(string id, Action<RequestResponse<EntityList>> callback, string options = null)
        {
            RestRequest request = new RestRequest(EntitiesResource);
            request.AddParameter("id", id);
            if (options != null)
                request.AddParameter("options", options);
            Client.SendRequest<EntityList>(request, callback);
        }

        /// <summary>
        /// Performs an asynchronous request to retrieve a set of entities, filtered by a list of Ids
        /// </summary>
        /// <param name="ids">Set of IDs by with the set of all entities should be filtered</param>
        /// <param name="callback">Function that is called when the asynchronous call returned</param>
        /// <param name="options">[optional] options with which the request should be performed</param>
        public void FilterEntitiesById(ISet<string> ids, Action<RequestResponse<EntityList>> callback, string options = null)
        {
            FilterEntitiesById(string.Join(",", ids), callback, options);
        }

        /// <summary>
        /// Performs an asynchronous request to retrieve a set of entities, filtered by Type
        /// </summary>
        /// <param name="type">Type by with the set of all entities should be filtered</param>
        /// <param name="callback">Function that is called when the asynchronous call returned</param>
        /// <param name="options">[optional] options with which the request should be performed</param>
        public void FilterEntitiesByType(string type, Action<RequestResponse<EntityList>> callback, string options = null)
        {
            RestRequest request = new RestRequest(EntitiesResource);
            request.AddParameter("type", type);
            if (options != null)
                request.AddParameter("options", options);
            Client.SendRequest<EntityList>(request, callback);
        }

        /// <summary>
        /// Performs an asynchronous request to retrieve a set of entities, filtered by a list of types
        /// </summary>
        /// <param name="types">Set of tytpes by with the set of all entities should be filtered</param>
        /// <param name="callback">Function that is called when the asynchronous call returned</param>
        /// <param name="options">[optional] options with which the request should be performed</param>
        public void FilterEntitiesByType(ISet<string> types, Action<RequestResponse<EntityList>> callback, string options = null)
        {
            FilterEntitiesByType(string.Join(",", types), callback, options);
        }
    }
}
