using System.Net;
using System.Net.Http.Headers;
using dragonrescue.Util;
using dragonrescue.Schema;
using static System.Net.Mime.MediaTypeNames;

/*
 * TODO:
 * - GetImage (s)
 * - GetKeyValuePair (s)
 * - GetAnnouncementsByUser
 * - GetUserMessageQueue
 * - GetSubscriptionInfo
 */

const string KEY = "56BB211B-CF06-48E1-9C1D-E40B5173D759";

string[] commandlineArgs = Environment.GetCommandLineArgs();
if (commandlineArgs.Length < 3) {
    Console.WriteLine("Not enough args.\nUsage: ./dragonrescue <username> <password> <full_output_path>");
}
string username = commandlineArgs[1];
string password = commandlineArgs[2];
string outputPath = commandlineArgs[3];

Console.WriteLine(string.Format("Logging into School of Dragons as '{0}' with password '{1}'...", username, password));

using HttpClient client = new();
string loginInfo = await LoginParent(client, username, password, KEY);
WriteToFile(outputPath, "LoginParent.xml", loginInfo);

ParentLoginInfo loginInfoObject = XmlUtil.DeserializeXml<ParentLoginInfo>(loginInfo);
if (loginInfoObject.Status != MembershipUserStatus.Success) {
    Console.WriteLine("Login error. Please check username and password, or try again in a few minutes.");
    Environment.Exit(1);
}

Console.WriteLine("Fetching account information...");
string parentInfo = await GetUserInfoByApiToken(client, loginInfoObject.ApiToken);
WriteToFile(outputPath, "GetUserInfoByApiToken.xml", parentInfo);

Console.WriteLine("Fetching child profiles...");
string children = await GetDetailedChildList(client, loginInfoObject.ApiToken);
WriteToFile(outputPath, "GetDetailedChildList.xml", children);
UserProfileDataList childrenObject = XmlUtil.DeserializeXml<UserProfileDataList>(children);
Console.WriteLine(string.Format("Found {0} child profiles.", childrenObject.UserProfiles.Length));

foreach (UserProfileData profile in childrenObject.UserProfiles) {
    Console.WriteLine(string.Format("Selecting profile {0}...", profile.AvatarInfo.UserInfo.FirstName));
    string childApiToken = await LoginChild(client, loginInfoObject.ApiToken, profile.ID, KEY);

    Console.WriteLine(string.Format("Fetching ranks for {0}...", profile.AvatarInfo.UserInfo.FirstName));
    string rankAttributes = await GetRankAttributeData(client, childApiToken);
    WriteToChildFile(outputPath, profile.ID, "GetRankAttributeData.xml", rankAttributes);

    Console.WriteLine(string.Format("Fetching inventory for {0}...", profile.AvatarInfo.UserInfo.FirstName));
    string childInventory = await GetCommonInventory(client, childApiToken);
    WriteToChildFile(outputPath, profile.ID, "GetCommonInventory.xml", childInventory);

    Console.WriteLine(string.Format("Fetching dragons for {0}...", profile.AvatarInfo.UserInfo.FirstName));
    string pets = await GetAllActivePetsByuserId(client, childApiToken, profile.ID);
    WriteToChildFile(outputPath, profile.ID, "GetAllActivePetsByuserId.xml", pets);

    Console.WriteLine(string.Format("Fetching dragon achievements for {0}...", profile.AvatarInfo.UserInfo.FirstName));
    string petAchievements = await GetPetAchievementsByUserID(client, childApiToken, profile.ID);
    WriteToChildFile(outputPath, profile.ID, "GetPetAchievementsByUserID.xml", petAchievements);

    Console.WriteLine(string.Format("Fetching achievements for {0}...", profile.AvatarInfo.UserInfo.FirstName));
    string childAchievement = await GetUserAchievementInfo(client, childApiToken);
    WriteToChildFile(outputPath, profile.ID, "GetUserAchievementInfo.xml", childAchievement);

    Console.WriteLine(string.Format("Fetching quests for {0}...", profile.AvatarInfo.UserInfo.FirstName));
    string missionsState = await GetUserMissionState(client, childApiToken, profile.ID);
    WriteToChildFile(outputPath, profile.ID, "GetUserMissionState.xml", missionsState);

    Console.WriteLine(string.Format("Fetching gamedata for {0}...", profile.AvatarInfo.UserInfo.FirstName));
    string gameData = await GetGameData(client, childApiToken);
    WriteToChildFile(outputPath, profile.ID, "GetGameData.xml", gameData);

    Console.WriteLine(string.Format("Fetching buddies for {0}...", profile.AvatarInfo.UserInfo.FirstName));
    string buddyList = await GetBuddyList(client, childApiToken);
    WriteToChildFile(outputPath, profile.ID, "GetBuddyList.xml", buddyList);

    Console.WriteLine(string.Format("Fetching clans for {0}...", profile.AvatarInfo.UserInfo.FirstName));
    string groups = await GetGroups(client, childApiToken, profile.ID);
    WriteToChildFile(outputPath, profile.ID, "GetGroups.xml", groups);

    Console.WriteLine(string.Format("Fetching activity for {0}...", profile.AvatarInfo.UserInfo.FirstName));
    string userActivity = await GetUserActivity(client, childApiToken, profile.ID);
    WriteToChildFile(outputPath, profile.ID, "GetUserActivity.xml", userActivity);

    Console.WriteLine(string.Format("Fetching active dragons for {0}...", profile.AvatarInfo.UserInfo.FirstName));
    string activePets = await GetSelectedRaisedPet(client, childApiToken, profile.ID);
    WriteToChildFile(outputPath, profile.ID, "GetSelectedRaisedPet.xml", activePets);

    Console.WriteLine(string.Format("Fetching rooms for {0}...", profile.AvatarInfo.UserInfo.FirstName));
    string rooms = await GetUserRoomList(client, childApiToken, profile.ID);
    WriteToChildFile(outputPath, profile.ID, "GetUserRoomList.xml", rooms);

    UserRoomResponse roomsObject = XmlUtil.DeserializeXml<UserRoomResponse>(rooms);
    foreach (UserRoom room in roomsObject.UserRoomList) {
        if (room.RoomID is null) continue;
        Console.WriteLine(string.Format("Fetching item positions for room {0} for {1}...", room.RoomID, profile.AvatarInfo.UserInfo.FirstName));
        string itemPositions = await GetUserItemPositions(client, childApiToken, profile.ID, room.RoomID);
        WriteToChildFile(outputPath, profile.ID, String.Format("{0}-{1}", room.RoomID, "GetUserItemPositions.xml"), itemPositions);
    }
}


static void WriteToFile(string path, string name, string contents) {
    string fullPath = Path.Join(path, name);
    using (StreamWriter writer = new StreamWriter(fullPath)) {
        writer.WriteLine(contents);
    }
}


static void WriteToChildFile(string path, string childId, string name, string contents) {
    string fullName = string.Format("{0}-{1}", childId, name);
    string fullPath = Path.Join(path, fullName);
    using (StreamWriter writer = new StreamWriter(fullPath)) {
        writer.WriteLine(contents);
    }
}


static async Task<string> LoginParent(HttpClient client, string UserName, string Password, string key) {
    ParentLoginData loginData = new ParentLoginData {
        UserName = UserName,
        Password = Password,
        Locale = "en-US"
    };

    var loginDataString = XmlUtil.SerializeXml(loginData);
    var loginDataStringEncrypted = TripleDES.EncryptUnicode(loginDataString, key);

    var formContent = new FormUrlEncodedContent(new[] {
        new KeyValuePair<string, string>("apiKey", "b99f695c-7c6e-4e9b-b0f7-22034d799013"),
        new KeyValuePair<string, string>("parentLoginData", loginDataStringEncrypted)
    });

    var response = await client.PostAsync("https://common.api.jumpstart.com/v3/AuthenticationWebService.asmx/LoginParent", formContent);
    var bodyRaw = await response.Content.ReadAsStringAsync();
    var bodyEncrypted = XmlUtil.DeserializeXml<string>(bodyRaw);
    var bodyDecrypted = TripleDES.DecryptUnicode(bodyEncrypted, key);
    return bodyDecrypted;
    //return XmlUtil.DeserializeXml<ParentLoginInfo>(bodyDecrypted);
}


static async Task<string> GetUserInfoByApiToken(HttpClient client, string apiToken) {
    var formContent = new FormUrlEncodedContent(new[] {
        new KeyValuePair<string, string>("apiKey", "b99f695c-7c6e-4e9b-b0f7-22034d799013"),
        new KeyValuePair<string, string>("apiToken", apiToken)
    });

    var response = await client.PostAsync("https://common.api.jumpstart.com/AuthenticationWebService.asmx/GetUserInfoByApiToken", formContent);
    var bodyRaw = await response.Content.ReadAsStringAsync();
    return bodyRaw;
    //return XmlUtil.DeserializeXml<UserInfo>(bodyRaw);
}


static async Task<string> GetDetailedChildList(HttpClient client, string apiToken) {
    var formContent = new FormUrlEncodedContent(new[] {
        new KeyValuePair<string, string>("apiKey", "b99f695c-7c6e-4e9b-b0f7-22034d799013"),
        new KeyValuePair<string, string>("parentApiToken", apiToken)
    });

    var response = await client.PostAsync("https://user.api.jumpstart.com/ProfileWebService.asmx/GetDetailedChildList", formContent);
    var bodyRaw = await response.Content.ReadAsStringAsync();
    return bodyRaw;
    //return XmlUtil.DeserializeXml<UserProfileDataList>(bodyRaw);
}


static async Task<string> LoginChild(HttpClient client, string apiToken, string childUserId, string key) {
    var childUserIdEncrypted = TripleDES.EncryptUnicode(childUserId, key);

    var ticks = DateTime.UtcNow.Ticks.ToString();
    var locale = "en-US";
    var signature = Md5.GetMd5Hash(string.Concat(new string[]
        {
            ticks,
            key,
            apiToken,
            childUserIdEncrypted,
            locale
        }));

    var formContent = new FormUrlEncodedContent(new[] {
        new KeyValuePair<string, string>("apiKey", "b99f695c-7c6e-4e9b-b0f7-22034d799013"),
        new KeyValuePair<string, string>("parentApiToken", apiToken),
        new KeyValuePair<string, string>("ticks", ticks),
        new KeyValuePair<string, string>("signature", signature),
        new KeyValuePair<string, string>("childUserID", childUserIdEncrypted),
        new KeyValuePair<string, string>("locale", locale),
    });

    var response = await client.PostAsync("https://common.api.jumpstart.com/AuthenticationWebService.asmx/LoginChild", formContent);
    var bodyRaw = await response.Content.ReadAsStringAsync();
    var bodyEncrypted = XmlUtil.DeserializeXml<string>(bodyRaw);
    return TripleDES.DecryptUnicode(bodyEncrypted, key);
}


static async Task<string> GetAllActivePetsByuserId(HttpClient client, string apiToken, string userId) {
    var formContent = new FormUrlEncodedContent(new[] {
        new KeyValuePair<string, string>("apiKey", "b99f695c-7c6e-4e9b-b0f7-22034d799013"),
        new KeyValuePair<string, string>("apiToken", apiToken),
        new KeyValuePair<string, string>("userId", userId),
        new KeyValuePair<string, string>("active", "True"),
    });

    var response = await client.PostAsync("https://contentserver.api.jumpstart.com/V2/ContentWebService.asmx/GetAllActivePetsByuserId", formContent);
    var bodyRaw = await response.Content.ReadAsStringAsync();
    return bodyRaw;
    //return XmlUtil.DeserializeXml<RaisedPetData[]>(bodyRaw);
}


static async Task<string> GetCommonInventory(HttpClient client, string apiToken) {
    GetCommonInventoryRequest request = new GetCommonInventoryRequest {
        ContainerId = 1,
        LoadItemStats = true,
        Locale = "en-US"
    };
    var requestString = XmlUtil.SerializeXml(request);

    var formContent = new FormUrlEncodedContent(new[] {
        new KeyValuePair<string, string>("apiKey", "b99f695c-7c6e-4e9b-b0f7-22034d799013"),
        new KeyValuePair<string, string>("apiToken", apiToken),
        new KeyValuePair<string, string>("getCommonInventoryRequestXml", requestString)
    });

    var response = await client.PostAsync("https://contentserver.api.jumpstart.com/V2/ContentWebService.asmx/GetCommonInventory", formContent);
    var bodyRaw = await response.Content.ReadAsStringAsync();
    return bodyRaw;
    //return XmlUtil.DeserializeXml<CommonInventoryData>(bodyRaw);
}


static async Task<string> GetRankAttributeData(HttpClient client, string apiToken) {
    var formContent = new FormUrlEncodedContent(new[] {
        new KeyValuePair<string, string>("apiToken", apiToken),
        new KeyValuePair<string, string>("apiKey", "b99f695c-7c6e-4e9b-b0f7-22034d799013"),
    });

    var response = await client.PostAsync("https://itemstoremission.api.jumpstart.com/ItemStoreWebService.asmx/GetRankAttributeData", formContent);
    var bodyRaw = await response.Content.ReadAsStringAsync();
    return bodyRaw;
    //return XmlUtil.DeserializeXml<ArrayOfRankAttributeData>(bodyRaw);
}


static async Task<string> GetPetAchievementsByUserID(HttpClient client, string apiToken, string userId) {
    var formContent = new FormUrlEncodedContent(new[] {
        new KeyValuePair<string, string>("apiToken", apiToken),
        new KeyValuePair<string, string>("apiKey", "b99f695c-7c6e-4e9b-b0f7-22034d799013"),
        new KeyValuePair<string, string>("userId", userId),
    });

    var response = await client.PostAsync("https://achievement.api.jumpstart.com/AchievementWebService.asmx/GetPetAchievementsByUserID", formContent);
    var bodyRaw = await response.Content.ReadAsStringAsync();
    return bodyRaw;
    //return XmlUtil.DeserializeXml<ArrayOfUserAchievementInfo>(bodyRaw);
}


static async Task<string> GetUserAchievementInfo(HttpClient client, string apiToken) {
    var formContent = new FormUrlEncodedContent(new[] {
        new KeyValuePair<string, string>("apiToken", apiToken),
        new KeyValuePair<string, string>("apiKey", "b99f695c-7c6e-4e9b-b0f7-22034d799013"),
    });

    var response = await client.PostAsync("https://achievement.api.jumpstart.com/AchievementWebService.asmx/GetUserAchievementInfo", formContent);
    var bodyRaw = await response.Content.ReadAsStringAsync();
    return bodyRaw;
    //return XmlUtil.DeserializeXml<UserAchievementInfo>(bodyRaw);
}


static async Task<string> GetUserMissionState(HttpClient client, string apiToken, string userId) {
    int[] missions = { 1003, 1014, 1015, 1016, 1017, 1027, 1028, 1029, 1031, 1033, 1035, 1036, 1037, 1038, 1044, 1046, 1047, 1048, 1053, 1054, 1055, 1057, 1058, 1062, 1067, 1074, 1085, 1090, 1091, 1093, 1095, 1096, 1097, 1099, 1101, 1102, 1106, 1108, 1110, 1111, 1114, 1120, 1121, 1128, 1134, 1140, 1143, 1144, 1150, 1153, 1155, 1159, 1163, 1164, 1166, 1167, 1168, 1169, 1171, 1173, 1179, 1185, 1186, 1187, 1188, 1189, 1191, 1192, 1193, 1194, 1195, 1196, 1197, 1198, 1199, 1200, 1201, 1202, 1203, 1204, 1205, 1206, 1207, 1208, 1209, 1210, 1211, 1212, 1213, 1214, 1215, 1216, 1217, 1218, 1219, 1220, 1221, 1222, 1223, 1224, 1225, 1226, 1227, 1228, 1229, 1230, 1231, 1232, 1233, 1234, 1235, 1236, 1237, 1238, 1239, 1240, 1241, 1242, 1243, 1244, 1245, 1247, 1250, 1251, 1252, 1253, 1254, 1255, 1256, 1257, 1258, 1259, 1260, 1261, 1262, 1263, 1264, 1265, 1266, 1267, 1268, 1269, 1270, 1271, 1272, 1273, 1274, 1275, 1276, 1277, 1278, 1279, 1280, 1281, 1282, 1283, 1284, 1285, 1286, 1287, 1288, 1289, 1290, 1291, 1292, 1293, 1294, 1295, 1296, 1304, 1305, 1307, 1308, 1309, 1310, 1311, 1312, 1313, 1314, 1315, 1316, 1317, 1318, 1321, 1322, 1323, 1324, 1325, 1326, 1327, 1328, 1329, 1330, 1331, 1333, 1335, 1338, 1343, 1344, 1345, 1346, 1347, 1348, 1349, 1350, 1351, 1352, 1353, 1354, 1357, 1361, 1362, 1390, 1508, 1529, 1530, 1575, 1579, 1605, 1606, 1607, 1608, 1611, 1612, 1613, 1614, 1615, 1617, 1618, 1619, 1620, 1622, 1623, 1624, 1625, 1626, 1627, 1628, 1629, 1630, 1632, 1633, 1634, 1636, 1638, 1640, 1641, 1642, 1646, 1648, 1652, 1655, 1656, 1657, 1658, 1660, 1661, 1663, 1666, 1667, 1669, 1671, 1672, 1673, 1674, 1675, 1676, 1678, 1681, 1683, 1689, 1690, 1691, 1692, 1693, 1694, 1695, 1696, 1697, 1698, 1699, 1700, 1701, 1702, 1703, 1704, 1705, 1706, 1707, 1708, 1709, 1710, 1711, 1712, 1713, 1714, 1715, 1716, 1717, 1718, 1719, 1720, 1721, 1722, 1723, 1724, 1725, 1726, 1727, 1728, 1729, 1730, 1736, 1749, 1769, 1771, 1777, 1781, 1787, 1788, 1813, 1815, 1818, 1822, 1828, 1961, 1967, 1969, 1970, 1971, 1972, 1973, 1974, 1978, 2176, 2178, 2180, 2182, 2195, 2196, 2199, 2206, 2207, 2208, 2212, 2213, 2215, 2217, 2218, 2219, 2223, 2225, 2226, 2228, 2229, 2232, 2233, 2235, 2284, 2287, 2288, 2300, 2302, 2303, 2304, 2307, 2308, 2309, 2311, 2314, 2315, 2318, 2319, 2320, 2324, 2328, 2329, 2330, 2331, 2332, 2333, 2335, 2337, 2338, 2339, 2344, 2346, 2347, 2353, 2354, 2357, 2363, 2372, 2389, 2392, 2393, 2394, 2395, 2396, 2397, 2398, 2399, 2400, 2401, 2402, 2403, 2404, 2405, 2406, 2407, 2408, 2415, 2416, 2421, 2422, 2423, 2424, 2425, 2426, 2427, 2428, 2429, 2430, 2431, 2432, 2433, 2434, 2435, 2436, 2437, 2438, 2439, 2444, 2457, 2458, 2459, 2472, 2474, 2475, 2476, 2477, 2478, 2479, 2481, 2482, 2483, 2485, 2486, 2493, 2494, 2496, 2498, 2499, 2500, 2503, 2506, 2510, 2511, 2512, 2513, 2514, 2515, 2516, 2517, 2518, 2519, 2520, 2521, 2522, 2526, 2528, 2529, 2532, 2533, 2536, 2538, 2542, 2548, 2551, 2554, 2556, 2557, 2562, 2563, 2564, 2565, 2566, 2567, 2568, 2569, 2570, 2571, 2572, 2573, 2574, 2578, 2579, 2580, 2581, 2582, 2583, 2584, 2585, 2586, 2587, 2588, 2589, 2590, 2591, 2592, 2593, 2594, 2596, 2597, 2598, 2603, 2604, 2605, 2606, 2607, 2608, 2613, 2614, 2622, 2626, 2631, 2632, 2633, 2634, 2635, 2636, 2637, 2638, 2639, 2640, 2641, 2643, 2645, 2646, 2649, 2650, 2652, 2653, 2655, 2656, 2659, 2661, 2662, 2663, 2664, 2665, 2666, 2667, 2668, 2669, 2670, 2671, 2673, 2674, 2675, 2676, 2677, 2678, 2679, 2680, 2681, 2682, 2683, 2684, 2685, 2686, 2687, 2688, 2689, 2690, 2691, 2692, 2693, 2694, 2695, 2697, 2699, 2700, 2705, 2707, 2708, 2711, 2714, 2717, 2718, 2719, 2723, 2730, 2731, 2776, 2781, 2782, 2783, 2786, 2787, 2790, 2806, 2807, 2808, 2809, 2810, 2811, 2812, 2813, 2814, 2815, 2816, 2817, 2818, 2819, 2836, 2837, 2838, 2839, 2843, 2844, 2845, 2874, 2875, 2876, 2878, 2879, 2884, 2887, 2889, 2892, 2893, 2894, 2895, 2897, 2898, 2899, 2931, 2932, 2937, 2946, 2947, 2956, 2957, 2958, 2959, 2961, 2962, 2963, 2966, 2968, 2969, 2972, 2973, 2974, 2983, 2984, 2985, 2986, 2988, 2989, 2990, 2992, 2993, 2996, 3013, 3015, 3018, 3022, 3023, 3024, 3025, 3031, 3032, 3034, 3035, 3036, 3037, 3038, 3039, 3041, 3043, 3052, 3053, 3054, 3055, 3056, 3057, 3067, 3068, 3069, 3071, 3072, 3073, 3074, 3075, 3076, 3077, 3078, 3079, 3080, 3081, 3091, 3093, 3095, 3099, 3100, 3101, 3102, 3103, 3104, 3108, 3111, 3112, 3113, 3114, 3115, 3116, 3117, 3118, 3119, 3120, 3121, 3122, 3123, 3124, 3125, 3126, 3127, 3128, 3129, 3130, 3131, 3132, 3133, 3134, 3135, 3136, 3137, 3141, 3145, 3148, 3149, 3150, 3158, 999 };
    MissionRequestFilterV2 filter = new MissionRequestFilterV2 {
        ProductGroupID = 0,
        MissionPair = missions.Select(m => new MissionPair { MissionID = m, VersionID = -1}).ToList(),
    };
    var filterString = XmlUtil.SerializeXml(filter);

    var formContent = new FormUrlEncodedContent(new[] {
        new KeyValuePair<string, string>("apiToken", apiToken),
        new KeyValuePair<string, string>("apiKey", "b99f695c-7c6e-4e9b-b0f7-22034d799013"),
        new KeyValuePair<string, string>("userId", userId),
        new KeyValuePair<string, string>("filter", filterString),
    });

    var response = await client.PostAsync("https://contentserver.api.jumpstart.com/V2/ContentWebService.asmx/GetUserMissionState", formContent);
    var bodyRaw = await response.Content.ReadAsStringAsync();
    return bodyRaw;
    //return XmlUtil.DeserializeXml<UserMissionStateResult>(bodyRaw);
}


static async Task<string> GetGameData(HttpClient client, string apiToken) {
    GetGameDataRequest request = new GetGameDataRequest {
        GameID = 14,
        TopScoresOnly = true,
    };
    var requestString = XmlUtil.SerializeXml(request);

    var formContent = new FormUrlEncodedContent(new[] {
        new KeyValuePair<string, string>("apiToken", apiToken),
        new KeyValuePair<string, string>("apiKey", "b99f695c-7c6e-4e9b-b0f7-22034d799013"),
        new KeyValuePair<string, string>("gameDataRequest", requestString),
    });

    var response = await client.PostAsync("https://contentserver.api.jumpstart.com/V2/ContentWebService.asmx/GetGameData", formContent);
    var bodyRaw = await response.Content.ReadAsStringAsync();
    return bodyRaw;
    //return XmlUtil.DeserializeXml<GetGameDataResponse>(bodyRaw);
}


static async Task<string> GetBuddyList(HttpClient client, string apiToken) {
    var formContent = new FormUrlEncodedContent(new[] {
        new KeyValuePair<string, string>("apiToken", apiToken),
        new KeyValuePair<string, string>("apiKey", "b99f695c-7c6e-4e9b-b0f7-22034d799013"),
    });

    var response = await client.PostAsync("https://contentserver.api.jumpstart.com/ContentWebService.asmx/GetBuddyList", formContent);
    var bodyRaw = await response.Content.ReadAsStringAsync();
    return bodyRaw;
    //return XmlUtil.DeserializeXml<BuddyList>(bodyRaw);
}


static async Task<string> GetGroups(HttpClient client, string apiToken, string userId) {
    GetGroupsRequest request = new GetGroupsRequest {
        ForUserID = userId,
        IncludeMemberCount = true,
        IncludeMinFields = true,
    };
    var requestString = XmlUtil.SerializeXml(request);

    var formContent = new FormUrlEncodedContent(new[] {
        new KeyValuePair<string, string>("apiToken", apiToken),
        new KeyValuePair<string, string>("apiKey", "b99f695c-7c6e-4e9b-b0f7-22034d799013"),
        new KeyValuePair<string, string>("getGroupsRequest", requestString),
    });

    var response = await client.PostAsync("https://groups.api.jumpstart.com/V2/GroupWebService.asmx/GetGroups", formContent);
    var bodyRaw = await response.Content.ReadAsStringAsync();
    return bodyRaw;
    //return XmlUtil.DeserializeXml<UserRoomResponse>(bodyRaw);
}


static async Task<string> GetUserRoomList(HttpClient client, string apiToken, string userId) {
    UserRoomGetRequest request = new UserRoomGetRequest {
        UserID = Guid.Parse(userId),
        CategoryID = 541 // this seems hard coded
    };
    var requestString = XmlUtil.SerializeXml(request);

    var formContent = new FormUrlEncodedContent(new[] {
        new KeyValuePair<string, string>("apiToken", apiToken),
        new KeyValuePair<string, string>("apiKey", "b99f695c-7c6e-4e9b-b0f7-22034d799013"),
        new KeyValuePair<string, string>("request", requestString),
    });

    var response = await client.PostAsync("https://contentserver.api.jumpstart.com/ContentWebService.asmx/GetUserRoomList", formContent);
    var bodyRaw = await response.Content.ReadAsStringAsync();
    return bodyRaw;
    //return XmlUtil.DeserializeXml<GetGroupsResult>(bodyRaw);
}


static async Task<string> GetUserItemPositions(HttpClient client, string apiToken, string userId, string roomId) {
    var formContent = new FormUrlEncodedContent(new[] {
        new KeyValuePair<string, string>("apiToken", apiToken),
        new KeyValuePair<string, string>("apiKey", "b99f695c-7c6e-4e9b-b0f7-22034d799013"),
        new KeyValuePair<string, string>("userId", userId),
        new KeyValuePair<string, string>("roomId", roomId),
    });

    var response = await client.PostAsync("https://contentserver.api.jumpstart.com/ContentWebService.asmx/GetUserRoomItemPositions", formContent);
    var bodyRaw = await response.Content.ReadAsStringAsync();
    return bodyRaw;
    //return XmlUtil.DeserializeXml<UserItemPositionList>(bodyRaw);
}


static async Task<string> GetUserActivity(HttpClient client, string apiToken, string userId) {
    var formContent = new FormUrlEncodedContent(new[] {
        new KeyValuePair<string, string>("apiToken", apiToken),
        new KeyValuePair<string, string>("apiKey", "b99f695c-7c6e-4e9b-b0f7-22034d799013"),
        new KeyValuePair<string, string>("userId", userId),
    });

    var response = await client.PostAsync("https://contentserver.api.jumpstart.com/ContentWebService.asmx/GetUserActivityByUserID", formContent);
    var bodyRaw = await response.Content.ReadAsStringAsync();
    return bodyRaw;
    //return XmlUtil.DeserializeXml<ArrayOfUserActivity>(bodyRaw);
}


static async Task<string> GetSelectedRaisedPet(HttpClient client, string apiToken, string userId) {
    var formContent = new FormUrlEncodedContent(new[] {
        new KeyValuePair<string, string>("apiToken", apiToken),
        new KeyValuePair<string, string>("apiKey", "b99f695c-7c6e-4e9b-b0f7-22034d799013"),
        new KeyValuePair<string, string>("userId", userId),
        new KeyValuePair<string, string>("isActive", "true"),
    });

    var response = await client.PostAsync("https://contentserver.api.jumpstart.com/ContentWebService.asmx/GetSelectedRaisedPet", formContent);
    var bodyRaw = await response.Content.ReadAsStringAsync();
    return bodyRaw;
    //return XmlUtil.DeserializeXml<RaisedPetData[]>(bodyRaw);
}