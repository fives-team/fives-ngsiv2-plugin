﻿// This file is part of FiVES.
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
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace NGSIv2Plugin
{
    public class RequestDispatcher
    {
        public async Task<T> Get<T>(string uri)
        {
            HttpResponseMessage response = await httpClient.GetAsync(uri);
            string responseText = await response.Content.ReadAsStringAsync();
            var responseObject = serializer.Deserialize<T>(responseText);
            return responseObject;
        }

        private HttpClient httpClient = new HttpClient();
        private JavaScriptSerializer serializer = new JavaScriptSerializer();
    }
}
