myApp
    .factory('transportCommonService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchTransportName = function (q) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C0048'
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