myApp
    .factory('productBrandCommonService', ['$http', 'serviceBasePath',
        function ($http, serviceBasePath) {
            var fac = {};

            fac.FetchProductBrand = function (q) {
                return $http({
                    method: 'GET',
                    url: serviceBasePath + '/api/C0029'
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