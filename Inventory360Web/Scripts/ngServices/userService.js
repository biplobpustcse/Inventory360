myApp
    .factory('userService', [
        function () {
            var fac = {};
            fac.CurrentUser = null;
            fac.LoggedUserInfo = null;

            fac.SetCurrentUser = function (user) {
                fac.CurrentUser = user;
                sessionStorage.user = angular.toJson(user);
            }

            fac.GetCurrentUser = function () {
                fac.CurrentUser = angular.fromJson(sessionStorage.user);
                return fac.CurrentUser;
            }

            fac.SetLoggedUserInfo = function (userInfo) {
                fac.LoggedUserInfo = userInfo;
                sessionStorage.loggedInfo = angular.toJson(userInfo);
            }

            fac.GetLoggedUserInfo = function () {
                fac.LoggedUserInfo = angular.fromJson(sessionStorage.loggedInfo);
                return fac.LoggedUserInfo;
            }

            return fac;
        }
    ]);