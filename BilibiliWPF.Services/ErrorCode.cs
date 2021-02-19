using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliWpf.Services
{
    public enum ErrorCode
    {
        Success = 0,
        AppDoesNotExist1 = -1,
        InvalidAccessKey = -2,
        InvalidApiKey = -3,
        NoPermission1 = -4,
        AccountNotLoggedIn = -101,
        AccountBanned = -102,
        CreditNotEnough = -103,
        CoinNotEnough = -104,
        InvalidCaptcha = -105,
        AccountNotFullMember = -106,
        AppDoesNotExist2 = -107,
        TelNotBound1 = -108,
        TelNotBound2 = -110,
        CheckCsrfFailed = -111,
        SystemUpdating = -112,
        AccountNotRealName1 = -113,
        TelNotBound3 = -114,
        AccountNotRealName2 = -115,
        NotChanged = -304,
        RequestError = -400,
        NotCertificated = -401,
        NoPermission2 = -403,
        NotFound = -404,
        NotSupported = -405,
        ServerError = -500,
        Overloaded = -503,
        Timedout = -504,
        OutOfLimit = -509,
        FileNotFound = -616,
        FileSizeOutOfBound = -617,
        TooManyLoginAttempt = -625,
        UserDoesNotExist = -626,
        TooWeakPassword = -628,
        InvalidAccountOrPassword = -629,
        ObjectAmountOutOfBound = -632,
        Locked = -643,
        LevelNotEnough = -650,
        DuplicatedUser = -652,
        TokenOutOfDated = -658,
        TimestampOutOfDated = -662,
        LimitedRegion = -688,
        LimitedCopyright = -689
    }
}
