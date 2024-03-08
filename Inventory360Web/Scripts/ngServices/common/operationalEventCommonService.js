myApp
    .factory('operationalEventCommonService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchOperationalEvent = function (q) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C0039'
                        + '?query=' + q
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            };

            return fac;
        }
    ]);