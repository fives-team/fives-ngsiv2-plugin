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
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGSIv2Plugin.Messages
{
    public class EntryPoint
    {
        [DeserializeAs(Name = "entities_url")]
        public string Entities { get; set; }

        [DeserializeAs(Name = "types_url")]
        public string Types { get; set; }

        [DeserializeAs(Name = "subscriptions_url")]
        public string Subscriptions { get; set; }

        [DeserializeAs(Name = "registrations_url")]
        public string Registrations { get; set; }
    }
}
