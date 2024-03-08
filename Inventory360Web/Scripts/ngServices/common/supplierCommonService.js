myApp
    .factory('supplierCommonService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchAllSupplier = function (q) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C0068'
                        + '?query=' + q
                }).then(function (response) {
                    return response;
                }, function (error) {
                    console.log(error);
                });
            };

            fac.FetchSupplierShortInfo = function (id, currency) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/S0079'
                        + '?id=' + id
                        + '&currency=' + currency
                }).then(function (response) {
                    return response;
                }, function (error) {
                    return error;
                });
            }

            return fac;
        }
    ]);