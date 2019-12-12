﻿using Newtonsoft.Json;

namespace StartApp.API.ViewModels
{
    [JsonObject(MemberSerialization.OptOut)]
    public class ExternalLoginRequestViewModel
    {
        #region Constructor
        public ExternalLoginRequestViewModel()
        {

        }
        #endregion

        #region Properties
        public string access_token { get; set; }
        public string client_id { get; set; }
        #endregion
    }
}