myApp
    .factory('userInfoService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.GetLoggedUserData = function () {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/S0003'
                }).then(function (response) {
                    return response.data;
                }, function (error) {
                    console.log(error);
                });
            }

            return fac;
        }
    ]);