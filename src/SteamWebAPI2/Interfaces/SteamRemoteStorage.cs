﻿using Steam.Models;
using SteamWebAPI2.Models;
using SteamWebAPI2.Utilities;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace SteamWebAPI2.Interfaces
{
    public class SteamRemoteStorage : ISteamRemoteStorage
    {
        private ISteamWebInterface steamWebInterface;

        /// <summary>
        /// Default constructor established the Steam Web API key and initializes for subsequent method calls
        /// </summary>
        /// <param name="steamWebApiKey"></param>
        public SteamRemoteStorage(string steamWebApiKey, ISteamWebInterface steamWebInterface = null)
        {
            this.steamWebInterface = steamWebInterface == null
                ? new SteamWebInterface(steamWebApiKey, "ISteamRemoteStorage")
                : steamWebInterface;
        }

        /// <summary>
        /// Returns information about how to download a user generated content based on a UGC ID, App ID, and Steam ID.
        /// </summary>
        /// <param name="ugcId"></param>
        /// <param name="appId"></param>
        /// <param name="steamId"></param>
        /// <returns></returns>
        public async Task<ISteamWebResponse<UGCFileDetailsModel>> GetUGCFileDetailsAsync(ulong ugcId, uint appId, ulong? steamId = null)
        {
            Debug.Assert(appId > 0);

            List<SteamWebRequestParameter> parameters = new List<SteamWebRequestParameter>();

            parameters.AddIfHasValue(ugcId, "ugcid");
            parameters.AddIfHasValue(appId, "appid");
            parameters.AddIfHasValue(steamId, "steamid");

            try
            {
                var steamWebResponse = await steamWebInterface.GetAsync<UGCFileDetailsResultContainer>("GetUGCFileDetails", 1, parameters);

                var steamWebResponseModel = AutoMapperConfiguration.Mapper.Map<
                    ISteamWebResponse<UGCFileDetailsResultContainer>, 
                    ISteamWebResponse<UGCFileDetailsModel>>(steamWebResponse);

                return steamWebResponseModel;
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }
    }
}