myApp
    .factory('countryCommonService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchCountry = function (q) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C0038'
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