myApp
    .factory('securityUserCommonService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchLocationWiseSecurityUserAll = function (q) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C0065'
                        + '?query=' + q
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            }

            return fac;
        }
    ]);