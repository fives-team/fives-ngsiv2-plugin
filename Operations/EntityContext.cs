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
    public class EntityContext
    {
        private NGSIv2Client Client { get; set; }
        private string EntitiesResource { get; set; }

        public EntityContext(NGSIv2Client client, string resource)
        {
            this.Client = client;
            this.EntitiesResource = resource;
        }

        public void RetrieveEntityData
            (string id, Action<RequestResponse<EntityObject>> callback, string options = null)
        {
            RestRequest request = new RestRequest(EntitiesResource + "/" + id, Method.GET);
            if (options != null)
                request.AddParameter("options", options);
            Client.SendRequest<EntityObject>(request, callback);
        }

        public void RetrieveEntityData
            (string id, string type, Action<RequestResponse<EntityObject>> callback, string options = null)
        {
        }

    }
}
