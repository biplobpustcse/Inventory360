myApp
    .factory('unitTypeCommonService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchUnitType = function (q) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C0030'
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