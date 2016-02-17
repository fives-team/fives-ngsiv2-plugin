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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Script.Serialization;

namespace NGSIv2Plugin
{
    class RequestBuilder
    {
        public void RetrieveEntryPoint() {}
        
        public void ListAllEntities(Delegate callback) {            
            var request = client.GetAsync("Asasas");
            // request.ContinueWith(t => { serializer.Deserialize(t.Result.Content.ReadAsStringAsync});
        }

        public void ListAllEntities(int limit, int offset, string options, Delegate callback){}
        public void FilterEntitiesById(string id, Delegate callback){}
        public void FilterEntitiesById(ISet<string> ids, Delegate callback){}
        public void FilterEntitiesByType(string type, Delegate callback){}
        public void FilterEntitiesByType(ISet<string> types, Delegate callback){}

        HttpClient client = new HttpClient();
        JavaScriptSerializer serializer = new JavaScriptSerializer();
    }
}
