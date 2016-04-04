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
    /// Implements all methods that are defined to perform in Entity context, i.e. full entity data, or partial data based on
    /// types or attributes as documented at http://telefonicaid.github.io/fiware-orion/api/v2/cookbook/
    /// </summary>
    public class EntityContext : EntityOperation
    {
        public EntityContext(NGSIv2Client client, string resource) : base(client, resource) { }

        /// <summary>
        /// Sends an asynchronous request to receive all data about one entity based on ID
        /// </summary>
        /// <param name="id">Id of the entity under which it is stored in the provider</param>
        /// <param name="callback">Function that is called as soon as the request has returned</param>
        /// <param name="options">Optional parameters as specified in the NGSIv2 Spec</param>
        public void RetrieveEntityData
            (string id, Action<RequestResponse<EntityObject>> callback, string options = null)
        {
            RestRequest request = new RestRequest(EntitiesResource + "/" + id, Method.GET);
            if (options != null)
                request.AddParameter("options", options);
            Client.SendRequest<EntityObject>(request, callback);
        }

        /// <summary>
        /// Sends an asynchronous request to receive all data about one entity based on ID and type of the entity
        /// </summary>
        /// <param name="id">Id of the entity under which it is stored in the provider</param>
        /// <param name="type">Type to specify the wanted entity further</param>
        /// <param name="callback">Function that is called as soon as the request has returned</param>
        /// <param name="options">Optional parameters as specified in the NGSIv2 Spec</param>
        public void RetrieveEntityData
            (string id, string type, Action<RequestResponse<EntityObject>> callback, string options = null)
        {
            RestRequest request = new RestRequest(EntitiesResource + "/" + id, Method.GET);
            request.AddParameter("type", type);
            if (options != null)
                request.AddParameter("options", options);
            Client.SendRequest<EntityObject>(request, callback);
        }

        /// <summary>
        /// Sends an asynchronous request to retrieve one specific attribute of an entity
        /// </summary>
        /// <param name="id">Id of the entity under which it is stored in the provider</param>
        /// <param name="attribute">Name of the attribute that should be returned</param>
        /// <param name="callback">Function that is called as soon as the request has returned</param>
        /// <param name="options">Optional parameters as specified in the NGSIv2 Spec</param>
        public void RetrieveEntityAttribute
            (string id, string attribute, Action<RequestResponse<EntityObject>> callback, string options = null)
        {
            RestRequest request = new RestRequest(EntitiesResource + "/" + id, Method.GET);
            request.AddParameter("attrs", attribute);
            if (options != null)
                request.AddParameter("options", options);
            Client.SendRequest<EntityObject>(request, callback);
        }

        /// <summary>
        /// Sends an asynchronous request to retrieve a set of attribute of an entity
        /// </summary>
        /// <param name="id">Id of the entity under which it is stored in the provider</param>
        /// <param name="attributes">Names of the attributes that should be returned</param>
        /// <param name="callback">Function that is called as soon as the request has returned</param>
        /// <param name="options">Optional parameters as specified in the NGSIv2 Spec</param>
        public void RetrieveEntityAttributeSet
            (string id, ISet<string> attributes, Action<RequestResponse<EntityObject>> callback, string options = null)
        {
            RetrieveEntityAttribute(id, string.Join(",", attributes), callback, options);
        }

    }
}
