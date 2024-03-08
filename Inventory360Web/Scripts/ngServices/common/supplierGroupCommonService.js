myApp
    .factory('supplierGroupCommonService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchSupplierGroup = function (q) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C0020'
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