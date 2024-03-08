myApp
    .factory('transportTypeCommonService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchTransportType = function (q) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C0049'
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