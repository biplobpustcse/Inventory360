myApp
    .factory('accountService', ['$http', '$q', 'serviceBasePath', 'userService',
        function ($http, $q, serviceBasePath, userService) {
            var fac = {};

            fac.fetchAllCompany = function () {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/S0001'
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }

            fac.fetchLocationByCompany = function (companyId) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/S0002?companyId=' + companyId
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }

            fac.login = function (user) {
                var obj = { 'username': user.username, 'password': user.password, 'companyId': user.companyId, 'locationId': user.locationId, 'grant_type': 'password' };
                Object.toparams = function ObjectToParams(obj) {
                    var p = [];
                    for (var key in obj) {
                        p.push(key + '=' + encodeURIComponent(obj[key]));
                    }
                    return p.join('&');
                }

                var defer = $q.defer();
                $http({
                    method: 'POST',
                    url: serviceBasePath + '/token',
                    data: Object.toparams(obj),
                    headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
                }).then(function (response) {
                    userService.SetCurrentUser(response.data);
                    defer.resolve(response.data);
                }, function (error) {
                    defer.reject(error);
                });

                return defer.promise;
            }

            fac.logout = function () {
                userService.CurrentUser = null;
                userService.LoggedUserInfo = null;
                userService.SetCurrentUser(userService.CurrentUser);
                userService.SetLoggedUserInfo(userService.LoggedUserInfo);
            }

            return fac;
        }
    ]);