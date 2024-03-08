myApp
    .factory('customerGroupCommonService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchCustomerGroup = function (q) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C0022'
                        + '?query=' + q
                }).then(function (response) {
                    return response;
                }, function (error) {
                    return error;
                });
            }

            return fac;
        }
    ]);