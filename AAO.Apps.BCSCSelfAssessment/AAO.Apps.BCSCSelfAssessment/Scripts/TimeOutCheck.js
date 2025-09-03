var SessionTime = 0;
var fromcontroller = 0;
var currentat = 0;
var redirecttime = 1200000; //20 min;
var _returnSiteUrl;

function initScript(returnSiteUrl) {
    _returnSiteUrl = returnSiteUrl;
    console.log("_returnSiteUrl : " + _returnSiteUrl);
}

let userInterval = setInterval(function () {
    console.log("_returnSiteUrl : " + _returnSiteUrl);
    Date.prototype.getUTCUnixTime = function () {
        return Math.floor(new Date(
            this.getUTCFullYear(),
            this.getUTCMonth(),
            this.getUTCDate(),
            this.getUTCHours(),
            this.getUTCMinutes(),
            this.getUTCSeconds()
        ).getTime());
    }

    // get asp.net cookie session value.
    var sessiontimeobj = getCookie("SessionUnixTimeoutExpireAt");
    console.log("Session object : " + sessiontimeobj);

    //Convert cookie value string to date
    if (sessiontimeobj != '') {
        fromcontroller = new Date(Date.parse(sessiontimeobj.replace('-', '/').replace('-', '/').replace('.', ':').replace('.', ':')));
        console.log("Converted string to Date (fromcontroller) : " + fromcontroller);

        fromcontroller = fromcontroller.getTime();
        console.log("Date to miliseconds (fromcontroller) : " + fromcontroller);
        console.log("                    Current UTC time : " + (new Date()).getUTCUnixTime());
    }

    // set current date of UTC date time.
    console.log("                    Current Date Time: " + new Date().getTime());
    currentat = (new Date()).getUTCUnixTime();
    console.log("Current Date Time Converted to UTC (currentat): " + currentat);

    // convert expire time in miliseconds.
    SessionTime = (currentat - fromcontroller);
    console.log("expire diff currentat - fromcontroller in milliseconds: " + SessionTime);

    
    if (redirecttime <= SessionTime)
    {
        //Cookies Expires
        document.cookie = "AAOMASTER_SUBDOMAIN =;                        expires = Fri, 01-Jan-2021 00:00:01 GMT; path=/;";
        document.cookie = "BCSC_AAO_MasterCustomerId =;                  expires = Fri, 01-Jan-2021 00:00:01 GMT; path=/;";
        document.cookie = "BCSC_AAO_MasterCustomerId_store_redirect =;   expires = Fri, 01-Jan-2021 00:00:01 GMT; path=/;";
        document.cookie = "ASP.NET_SessionId =;                          expires = Fri, 01-Jan-2021 00:00:01 GMT; path=/;";
        document.cookie = ".ASPXROLES =;                                 expires = Fri, 01-Jan-2021 00:00:01 GMT; path=/;";
        document.cookie = ".ASPXAUTH =;                                  expires = Fri, 01-Jan-2021 00:00:01 GMT; path=/;";
        document.cookie = "AAOMASTER =;                                  expires = Fri, 01-Jan-2021 00:00:01 GMT; path=/;";
        document.cookie = "__RequestVerificationToken =;                 expires = Fri, 01-Jan-2021 00:00:01 GMT; path=/;";

        //Redirect to Login
        window.location.href = _returnSiteUrl;       
    }

    console.log('');

}, 3000);


function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}