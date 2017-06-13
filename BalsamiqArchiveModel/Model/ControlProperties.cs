﻿using System;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BalsamiqArchiveModel.Model
{
    public class ControlProperties
    {
        #region Properties
        
        // There are too many properties so not adding more here, until there is a need
        public static String Text { get; set; }
        public static String State { get; set; }

        [JsonExtensionData]
        public Dictionary<String, JToken> UnknownProperties { get; set; } = new Dictionary<String, JToken>();

        #endregion
    }
}